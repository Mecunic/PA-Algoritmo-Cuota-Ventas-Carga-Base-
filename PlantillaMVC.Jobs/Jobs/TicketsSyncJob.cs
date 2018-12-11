using PlantillaMVC.Domain.Models;
using PlantillaMVC.Domain.Services;
using PlantillaMVC.Integrations;
using PlantillaMVC.Integrations.Hubspot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;

namespace PlantillaMVC.Jobs.Jobs
{
    public class TicketsSyncJob
    {
        static bool executing = false;
        static readonly Object thisLock = new Object();

        public static void SyncTickets()
        {
            bool ProcessEnabled = true;
            Boolean.TryParse(System.Configuration.ConfigurationManager.AppSettings["Jobs.EnabledJobs"], out ProcessEnabled);

            IHubspotService apiService = new HubspotService();
            Trace.TraceInformation(string.Format("[TicketsSyncJob.SyncTickets] Executing at {0}", DateTime.Now));
            //string result = apiService.CreateTicketToCompany();
            //Trace.TraceInformation(result);
            IDBService dbService = new DBService();
            DBProceso procesoInfo = dbService.GetProcessInfo("SINCRONIZACION_TICKETS");

            if (Monitor.TryEnter(thisLock))
            {
                try
                {
                    if (!executing && ProcessEnabled)
                    {
                        executing = true;
                        //SI ESTA HABILITADO Y NO SE ESTA EJECUTANDO
                        if (!procesoInfo.EstatusEjecucion && procesoInfo.EstatusProceso)
                        {
                            #region SINCRONIZACION DE COMPANIAS EN MEMORIA
                            Dictionary<string, long> CompanyDictionary = new Dictionary<string, long>();
                        long offset = 0;
                        bool hasMoreDeals = true;
                        int totalCompanies = 0;
                        while (hasMoreDeals)
                        {
                            CompaniesHubSpotResult companiesHubSpotResult = apiService.GetAllCompanies(250, offset);
                            Trace.TraceInformation(string.Format("HasMore: {0} Offset: {1}", companiesHubSpotResult.HasMore, companiesHubSpotResult.Offset));
                            hasMoreDeals = companiesHubSpotResult.HasMore;
                            offset = companiesHubSpotResult.Offset;
                            totalCompanies += companiesHubSpotResult.Companies.Count();
                            foreach (Company company in companiesHubSpotResult.Companies)
                            {
                                //TODO: Cambiar por RFC
                                if (company.Properties.RFC != null && !string.IsNullOrEmpty(company.Properties.RFC.Value))
                                {
                                    if (!CompanyDictionary.ContainsKey(company.Properties.RFC.Value)) CompanyDictionary.Add(company.Properties.RFC.Value, company.CompanyId);
                                }
                            }
                        }
                        Trace.TraceInformation(string.Format("Total Companies: {0}", totalCompanies));
                        Trace.TraceInformation(string.Format("Total Companies in Dic: {0}", CompanyDictionary.Count()));

                        foreach (KeyValuePair<string, long> entry in CompanyDictionary)
                        {
                            Trace.TraceInformation(string.Format("Company: {0}  Id: {1} ", entry.Key, entry.Value));
                        }
                        #endregion
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
                    procesoInfo.EstatusEjecucion = false;
                    procesoInfo.UltimaEjecucion = DateTime.Now;
                    dbService.ActualizarEstatusProceso(procesoInfo);

                    executing = false;
                    Monitor.Exit(thisLock);
                }
            }
            Trace.TraceInformation(string.Format("[TicketsSyncJob.SyncTickets] Finishing at {0}", DateTime.Now));
        }
    }
}