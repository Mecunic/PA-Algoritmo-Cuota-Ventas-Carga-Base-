using PlantillaMVC.Domain.Services;
using PlantillaMVC.Integrations;
using PlantillaMVC.Integrations.Hubspot;
using PlantillaMVC.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;

namespace PlantillaMVC.Jobs.Jobs
{
    public class DemoJob
    {
        static bool executing = false;
        static readonly Object thisLock = new Object();

        public static void DemoMethod()
        {
            bool NotificationProcessEnabled = true;
            Boolean.TryParse(System.Configuration.ConfigurationManager.AppSettings["Jobs.EnabledJobs"], out NotificationProcessEnabled);
            if (Monitor.TryEnter(thisLock))
            {
                try
                {
                    if (!executing && NotificationProcessEnabled)
                    {
                        //TODO: Implementar logica de negocio especifica
                        IHubspotService service = new HubspotService();
                        Trace.TraceInformation(string.Format("[DemoJob.DemoMethod] Executing at {0}", DateTime.Now));
                        //List<HubspotDealModel> hubspotDeals = service.ReadDeals();
                        //foreach (var deal in hubspotDeals)
                        //{
                        //    //Trace.TraceInformation(string.Format("{0} - {1} - {2} - {3}- {4} - {5} - {6} - {7}", deal.Id, deal.Dealname, deal.Amount, deal.CloseDate, deal.DealType, deal.Pipeline, deal.RelatedCompanies, deal.RelatedContacts));
                        //    Trace.TraceInformation(JsonUtil.ConvertToString(deal));
                        //}
                        var dealsObj = service.ReadDeals2();
                        foreach(var deal in dealsObj.Deals)
                        {
                            var associations = deal.Associations;
                            long? contactId = null;
                            if (associations.AssociatedVids != null && associations.AssociatedVids.Any())
                            {
                                contactId = associations.AssociatedVids.First();
                                string contactObj = service.GetContactById(contactId.Value);
                            }
                            long? companyId = null;
                            if (associations.associatedCompanyIds != null && associations.associatedCompanyIds.Any())
                            {
                                companyId = associations.associatedCompanyIds.First();
                                string companyObj = service.GetCompanyById(companyId.Value);
                            }
                        }
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