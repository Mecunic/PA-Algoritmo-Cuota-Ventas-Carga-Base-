using System;
using Hangfire;
using Microsoft.Owin;
using MVC_Project.Jobs.App_Code;

using Owin;

[assembly: OwinStartupAttribute(typeof(MVC_Project.Jobs.Startup))]
namespace MVC_Project.Jobs
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            bool NotificationProcessEnabled = false;
            string JobName = string.Empty;
            string JobCron = string.Empty,
                JobHPagosCron = string.Empty;
            string Dashboardurl = string.Empty;
            string sAttempts = string.Empty;
            int Attempts = 0;

            ConfigureAuth(app);

            try
            {
                #region Configuración basica y Conexión a Base de datos
                GlobalConfiguration.Configuration.UseSqlServerStorage("DBConnectionString");
                Boolean.TryParse(System.Configuration.ConfigurationManager.AppSettings["Jobs.EnabledJobs"], out NotificationProcessEnabled);
                Dashboardurl = System.Configuration.ConfigurationManager.AppSettings["Jobs.Dashboard.Url"].ToString();
                #endregion

                if (NotificationProcessEnabled)
                {
                    JobName = System.Configuration.ConfigurationManager.AppSettings["Jobs.EnviarNotificaciones.Name"].ToString();
                    JobCron = System.Configuration.ConfigurationManager.AppSettings["Jobs.EnviarNotificaciones.Cron"].ToString();
                    RecurringJob.AddOrUpdate(JobName, () => DemoJob.DemoMethod(), JobCron, TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time (Mexico)"));
                }

                int.TryParse(sAttempts, out Attempts);

                GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = Attempts, OnAttemptsExceeded = AttemptsExceededAction.Delete });

                JobCron = System.Configuration.ConfigurationManager.AppSettings["Jobs.HPagos.Cron"].ToString();
                RecurringJob.AddOrUpdate("Notificador_Historico", () => HPagosJob.ListaCedis(), JobHPagosCron, TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time (Mexico)"));
                
                app.UseHangfireDashboard(Dashboardurl, new DashboardOptions
                {
                    DisplayStorageConnectionString = false,
                    Authorization = new[] { new JobsAuthorizationFilter() },
                });
                app.UseHangfireServer();
            }
            catch
            {
                throw;
            }
        }
    }
}