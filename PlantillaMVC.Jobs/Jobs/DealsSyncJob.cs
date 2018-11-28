using PlantillaMVC.Integrations;
using PlantillaMVC.Integrations.Hubspot;
using PlantillaMVC.Integrations.Mapper;
using PlantillaMVC.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;
using PlantillaMVC.Domain.Models;
using PlantillaMVC.Domain.Services;

namespace PlantillaMVC.Jobs.Jobs
{
    public class DealsSyncJob
    {
        static bool executing = false;
        static readonly Object thisLock = new Object();

        public static void SyncDeals()
        {
            bool NotificationProcessEnabled = true;
            Boolean.TryParse(System.Configuration.ConfigurationManager.AppSettings["Jobs.EnabledJobs"], out NotificationProcessEnabled);
            if (Monitor.TryEnter(thisLock))
            {
                try
                {
                    if (!executing && NotificationProcessEnabled)
                    {
                        IDBService dbService = new DBService();
                        IHubspotService apiService = new HubspotService();
                        Trace.TraceInformation(string.Format("[DealsSyncJob.SyncDeals] Executing at {0}", DateTime.Now));
                        
                        var dealsObj = apiService.ReadDeals(250,0);

                        //MapperFactory mapperFactory = new MapperFactory();
                        //IMapper<DealHubSpotResult, DealListModel> mapper = mapperFactory.CreateMapper<DealHubSpotResult, DealListModel>();
                        //DealListModel dealList = mapper.Map(dealsObj);
                        //Trace.TraceInformation(string.Format("HasMore: {0} Offset: {1}", dealList.HasMore, dealList.Offset));

                        Trace.TraceInformation(string.Format("HasMore: {0} Offset: {1}", dealsObj.HasMore, dealsObj.Offset));

                        //foreach (DealModel deal in dealList.Deals)
                        foreach (Deal deal in dealsObj.Deals)
                        {
                            Trace.TraceInformation(JsonUtil.ConvertToString(deal));
                            var associations = deal.Associations;
                            long? contactId = null;
                            string CompanyDomain = string.Empty;
                            string CompanyName = string.Empty;
                            long? companyId = null;
                            decimal amount = 0;
                            string ContactName = string.Empty;

                            if (associations.AssociatedVids != null && associations.AssociatedVids.Any())
                            {
                                contactId = associations.AssociatedVids.First();
                                ContactHubSpotResult contactObj = apiService.GetContactById(contactId.Value);
                                ContactName = contactObj.Properties.FirstName.Value;
                            }
                            if (associations.associatedCompanyIds != null && associations.associatedCompanyIds.Any())
                            {
                                companyId = associations.associatedCompanyIds.First();
                                CompanyHubSpotResult companyObj = apiService.GetCompanyById(companyId.Value);
                                CompanyName = companyObj.Properties.Name.Value;
                                if (companyObj.Properties.Domain!=null && !string.IsNullOrEmpty(companyObj.Properties.Domain.Value))
                                {
                                    CompanyDomain = companyObj.Properties.Domain.Value;
                                }
                            }
                            if (deal.Properties.Amount!=null && !string.IsNullOrEmpty(deal.Properties.Amount.Value))
                            {
                                amount = Convert.ToDecimal(deal.Properties.Amount.Value);
                            }

                            //INSERCION A BD
                            DBDealModel dealBD = new DBDealModel()
                            {
                                DealId = deal.DealId,
                                DealName = deal.Properties.Dealname.Value,
                                CompanyDomain = CompanyDomain,
                                Stage = deal.Properties.DealStage.Value,
                                Amount = amount,
                                CompanyName = CompanyName,
                                ContactName = ContactName
                            };
                            dbService.CreateDeal(dealBD);
                        }
                        Trace.TraceInformation(string.Format("[DealsSyncJob.SyncDeals] Finishing at {0}", DateTime.Now));
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    Trace.TraceInformation(ex.Message);
                }
                finally
                {
                    executing = false;
                    Monitor.Exit(thisLock);
                }
            }
        }
    }
}