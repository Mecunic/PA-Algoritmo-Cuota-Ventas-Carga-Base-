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
                        service.ReadDeals2();
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