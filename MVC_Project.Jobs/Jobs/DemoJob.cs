using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;

namespace MVC_Project.Jobs
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
                        Trace.TraceInformation(string.Format("[DemoJob.DemoMethod] Executing at {0}", DateTime.Now));
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