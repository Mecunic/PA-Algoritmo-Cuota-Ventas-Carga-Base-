using PlantillaMVC.Integrations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace PlantillaMVC.Jobs.Jobs
{
    public class TicketsSyncJob
    {
        static bool executing = false;
        static readonly Object thisLock = new Object();

        public static void SyncTickets()
        {
            IHubspotService apiService = new HubspotService();
            Trace.TraceInformation(string.Format("[TicketsSyncJob.SyncTickets] Executing at {0}", DateTime.Now));
            string result = apiService.CreateTicketToCompany();
            Trace.TraceInformation(result);
        }
    }
}