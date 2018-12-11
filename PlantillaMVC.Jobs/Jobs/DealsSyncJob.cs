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
            bool ProcessEnabled = true;
            Boolean.TryParse(System.Configuration.ConfigurationManager.AppSettings["Jobs.EnabledJobs"], out ProcessEnabled);
            string FiltroDeal = System.Configuration.ConfigurationManager.AppSettings["Jobs.SincronizarDeals.Filtro"].ToString();

            IDBService dbService = new DBService();
            DBProceso procesoInfo = dbService.GetProcessInfo("SINCRONIZACION_DEALS");

            if (Monitor.TryEnter(thisLock))
            {
                try
                {
                    if (!executing && ProcessEnabled)
                    {
                        executing = true;
                        int syncedDeals = 0;

                        IHubspotService apiService = new HubspotService();
                        Trace.TraceInformation(string.Format("[DealsSyncJob.SyncDeals] Executing at {0}", DateTime.Now));
                        
                        //SI ESTA HABILITADO Y NO SE ESTA EJECUTANDO
                        if (!procesoInfo.EstatusEjecucion && procesoInfo.EstatusProceso)
                        {
                            procesoInfo.EstatusEjecucion = true;
                            procesoInfo.UltimaEjecucion = DateTime.Now;
                            dbService.ActualizarEstatusProceso(procesoInfo);

                            DBProcesoEjecucion procesoDetalle = new DBProcesoEjecucion()
                            {
                                ProcesoId = procesoInfo.ProcesoId,
                                Estatus = true,
                                Resultado = "Procesando..."
                            };
                            int ProcesoDetalleId = dbService.CreateProcesoEjecucion(procesoDetalle);

                            //LIMPIA LA TABLA
                            dbService.ClearDeals();

                            //INICIA SINCRONIZACION
                            long offset = 0;
                            bool hasMoreDeals = true;
                            while (hasMoreDeals)
                            {
                                var dealsObj = apiService.ReadDeals(250, offset);

                                Trace.TraceInformation(string.Format("HasMore: {0} Offset: {1}", dealsObj.HasMore, dealsObj.Offset));
                                hasMoreDeals = dealsObj.HasMore;
                                offset = dealsObj.Offset;
                            
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
                                    string DealStage = deal.Properties.DealStage.Value;
                                    string linea = string.Empty;
                                    string Owner = string.Empty;

                                    if (!FiltroDeal.Contains(DealStage))
                                    {
                                        if (associations.AssociatedVids != null && associations.AssociatedVids.Any())
                                        {
                                            contactId = associations.AssociatedVids.First();
                                            ContactHubSpotResult contactObj = apiService.GetContactById(contactId.Value);
                                            if (contactObj.Properties.Email != null && !string.IsNullOrEmpty(contactObj.Properties.Email.Value))
                                            {
                                                ContactName = contactObj.Properties.Email.Value;
                                            }
                                        }
                                        if (associations.associatedCompanyIds != null && associations.associatedCompanyIds.Any())
                                        {
                                            companyId = associations.associatedCompanyIds.First();
                                            CompanyHubSpotResult companyObj = apiService.GetCompanyById(companyId.Value);
                                            CompanyName = string.Format("{0} - {1}", companyObj.CompanyId, companyObj.Properties.Name.Value);
                                            if (companyObj.Properties.RFC != null && !string.IsNullOrEmpty(companyObj.Properties.RFC.Value))
                                            {
                                                CompanyDomain = companyObj.Properties.RFC.Value;
                                            }
                                        }
                                        if (deal.Properties.Amount != null && !string.IsNullOrEmpty(deal.Properties.Amount.Value))
                                        {
                                            amount = Convert.ToDecimal(deal.Properties.Amount.Value);
                                        }
                                        if (deal.Properties.LineaDeNegocio!=null && !string.IsNullOrEmpty(deal.Properties.LineaDeNegocio.Value))
                                        {
                                            linea = deal.Properties.LineaDeNegocio.Value;
                                        }
                                        if (deal.Properties.HubspotOwnerId != null && !string.IsNullOrEmpty(deal.Properties.HubspotOwnerId.SourceId))
                                        {
                                            Owner = deal.Properties.HubspotOwnerId.SourceId;
                                        }

                                        //INSERCION A BD
                                        DBDealModel dealBD = new DBDealModel()
                                        {
                                            ProcesoDetalleId = ProcesoDetalleId,
                                            DealId = deal.DealId,
                                            DealName = deal.Properties.Dealname.Value,
                                            CompanyDomain = CompanyDomain,
                                            Stage = DealStage,
                                            Amount = amount,
                                            CompanyName = CompanyName,
                                            ContactName = ContactName,
                                            ProductLine = linea,
                                            OwnerName = Owner
                                        };
                                        dbService.CreateDeal(dealBD);
                                        syncedDeals++;
                                    } //END IF
                                } //END FOR
                            } //END WHILE
                        
                            Trace.TraceInformation(string.Format("[DealsSyncJob.SyncDeals] Finishing at {0}", DateTime.Now));

                            procesoDetalle.FechaFin = DateTime.Now;
                            procesoDetalle.Estatus = false;
                            procesoDetalle.Resultado = string.Format("Se sincronizaron {0} deals", syncedDeals);
                            dbService.ActualizarProcesoEjecucion(procesoDetalle);
                            
                        } //FIN DE PROCESO INFO
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
        }
    }
}