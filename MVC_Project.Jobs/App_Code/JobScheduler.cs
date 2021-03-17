using System.Diagnostics;

namespace MVC_Project.Jobs.App_Code
{
    public class JobScheduler
    {
        public static void Start()
        {
            bool notificationProcessEnabled = true;
            //Boolean.TryParse(System.Configuration.ConfigurationManager.AppSettings["Jobs.EnabledJobs"], out enableCopy);

            Debug.WriteLine(string.Format("[JobScheduler] Enable notification process: {0}", notificationProcessEnabled));
            System.Diagnostics.Trace.TraceInformation(string.Format("[JobScheduler] Enable notification process: {0}", notificationProcessEnabled));
            //if (notificationProcessEnabled)
            //{
            //    int intervalMinutes = 2;
            //    //Int32.TryParse(System.Configuration.ConfigurationManager.AppSettings["JOB_INTERVAL_MINUTES"], out intervalMinutes);

            //    IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            //    scheduler.Start();

            //    IJobDetail job = JobBuilder.Create<NotificationProcessJob>().Build();

            //    ITrigger trigger = TriggerBuilder.Create()
            //        .WithDailyTimeIntervalSchedule
            //          (s =>
            //             s.WithIntervalInMinutes(intervalMinutes)
            //            .OnEveryDay()
            //            .StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(0, 0))
            //          )
            //        .Build();

            //    scheduler.ScheduleJob(job, trigger);

            //    scheduler.Start();

            //    System.Diagnostics.Trace.TraceInformation("[JobScheduler] Started: " + scheduler.IsStarted);

            //}
        }
    }
}