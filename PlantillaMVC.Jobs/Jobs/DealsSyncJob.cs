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
using System.Web.Mvc;
using System.Text;

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
            StringBuilder strResultado = new StringBuilder("Iniciando proceso...");
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
                            procesoInfo.Resultado = strResultado.ToString();
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
                                    //Trace.TraceInformation(JsonUtil.ConvertToString(deal));
                                    strResultado.Append(" | " + JsonUtil.ConvertToString(deal));
                                    var associations = deal.Associations;
                                    long? contactId = null;
                                    string CompanyRFC = string.Empty;
                                    string CompanyName = string.Empty;
                                    long? companyId = null;
                                    decimal amount = 0;
                                    string ContactName = string.Empty;
                                    string DealStage = deal.Properties.DealStage.Value;
                                    string linea = string.Empty;
                                    string Owner = string.Empty;

                                    if (!FiltroDeal.Contains(DealStage))
                                    {
                                        strResultado.Append(" * Paso 1 ");
                                        if (associations.AssociatedVids != null && associations.AssociatedVids.Any())
                                        {
                                            strResultado.Append(" * Paso 1.1 ");
                                            contactId = associations.AssociatedVids.First();
                                            strResultado.Append(" * Paso 1.2 ");
                                            ContactHubSpotResult contactObj = apiService.GetContactById(contactId.Value);
                                            strResultado.Append(" * Paso 1.3 ");
                                            if (/*contactObj!=null && contactObj.Properties!=null &&*/ contactObj.Properties.Email != null && !string.IsNullOrEmpty(contactObj.Properties.Email.Value))
                                            {
                                                ContactName = contactObj.Properties.Email.Value;
                                            }
                                        }
                                        strResultado.Append(" * Paso 2 ");
                                        if (associations.associatedCompanyIds != null && associations.associatedCompanyIds.Any())
                                        {
                                            companyId = associations.associatedCompanyIds.First();
                                            CompanyHubSpotResult companyObj = apiService.GetCompanyById(companyId.Value);
                                            CompanyName = string.Format("{0}", companyObj.Properties.Name.Value);
                                            strResultado.Append(" * Paso 2.1 ");
                                            if (/*companyObj!=null && companyObj.Properties!=null &&*/  companyObj.Properties.RFC != null && !string.IsNullOrEmpty(companyObj.Properties.RFC.Value))
                                            {
                                                CompanyRFC = companyObj.Properties.RFC.Value;
                                            }
                                        }
                                        strResultado.Append(" * Paso 3 ");
                                        if (deal.Properties.Amount != null && !string.IsNullOrEmpty(deal.Properties.Amount.Value))
                                        {
                                            amount = Convert.ToDecimal(deal.Properties.Amount.Value);
                                        }
                                        strResultado.Append(" * Paso 4 ");
                                        if (deal.Properties.LineaDeNegocio!=null && !string.IsNullOrEmpty(deal.Properties.LineaDeNegocio.Value))
                                        {
                                            linea = deal.Properties.LineaDeNegocio.Value;
                                        }
                                        strResultado.Append(" * Paso 5 ");
                                        if (deal.Properties.HubspotOwnerId != null && !string.IsNullOrEmpty(deal.Properties.HubspotOwnerId.SourceId))
                                        {
                                            Owner = deal.Properties.HubspotOwnerId.SourceId;
                                        }
                                        strResultado.Append(" * Paso 6 ");
                                        //INSERCION A BD
                                        DBDealModel dealBD = new DBDealModel()
                                        {
                                            ProcesoDetalleId = ProcesoDetalleId,
                                            DealId = deal.DealId,
                                            DealName = deal.Properties.Dealname.Value,
                                            CompanyRFC = CompanyRFC,
                                            Stage = DealStage,
                                            Amount = amount,
                                            CompanyName = CompanyName,
                                            ContactName = ContactName,
                                            ProductLine = linea,
                                            OwnerName = Owner
                                        };
                                        strResultado.Append(" * Paso 7 ");
                                        dbService.CreateDeal(dealBD);
                                        syncedDeals++;
                                    } //END IF

                                    Thread.Sleep(150);

                                } //END FOR
                            } //END WHILE
                        
                            Trace.TraceInformation(string.Format("[DealsSyncJob.SyncDeals] Finishing at {0}", DateTime.Now));

                            procesoDetalle.FechaFin = DateTime.Now;
                            procesoDetalle.Estatus = false;
                            procesoDetalle.Resultado = string.Format("Se sincronizaron {0} deals", syncedDeals);
                            strResultado.Append(string.Format("|Se sincronizaron {0} deals", syncedDeals));
                            dbService.ActualizarProcesoEjecucion(procesoDetalle);
                            
                        } //FIN DE PROCESO INFO
                    }
                }
                catch (Exception exception)
                {
                    Debug.WriteLine(exception.Message);
                    Trace.TraceInformation(exception.Message);

                    strResultado.Append("|" + exception.Message);

                    if (exception.Source != null)
                    {
                        strResultado.Append("|" + exception.Source);
                    }
                    if (exception.StackTrace != null)
                    {
                        strResultado.Append("|" + exception.StackTrace);
                    }

                    try
                    {
                        System.Diagnostics.StackTrace trace = new System.Diagnostics.StackTrace(exception, true);
                        strResultado.Append("|" + String.Format("<p>Error Detail Message :{0}  => Error In :{1}  => Line Number :{2} => Error Method:{3}</p>",
                                  HttpUtility.HtmlEncode(exception.Message),
                                  trace.GetFrame(0).GetFileName(),
                                  trace.GetFrame(0).GetFileLineNumber(),
                                  trace.GetFrame(0).GetMethod().Name));
                    }
                    catch (Exception ex) { }
                }
                finally
                {
                    procesoInfo.EstatusEjecucion = false;
                    procesoInfo.UltimaEjecucion = DateTime.Now;
                    procesoInfo.Resultado = strResultado.ToString();
                    dbService.ActualizarEstatusProceso(procesoInfo);

                    executing = false;
                    Monitor.Exit(thisLock);
                }
            }
        }
    }
}