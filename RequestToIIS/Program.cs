using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RequestToIIS
{
    class Program
    {
        static void Main(string[] args)
        {
            CreateLogFile();
            DateTime startTime = DateTime.Now;
            WriteLineLog("Start at: " + startTime.ToString("dd/mm/yyyy HH:mm:ss.fff"));
            string urlsString = ConfigurationManager.AppSettings["DashboardUrls"];
            string[] urls = urlsString.Split(',');
            foreach (string url in urls)
            {
                SendPing(url.Trim());
            }
            DateTime endTime = DateTime.Now;
            WriteLineLog("End at: " + endTime.ToString("dd/mm/yyyy HH:mm:ss.fff"));
            WriteLineLog("#######################################################");

        }
        public static void CreateLogFile()
        {
            string logMaxFileMBSize = ConfigurationManager.AppSettings["MaxLogSizeMB"];

            string logFileDirectory = GetLogFilePath();
            if (logFileDirectory == null)
            {
                return;
            }
            if (logMaxFileMBSize == null)
            {
                return;
            }
            long maxLogFileMBSize = 0;
            if(!Int64.TryParse(logMaxFileMBSize, out maxLogFileMBSize))
            {
                throw new Exception("MaxLogSizeMB config is not a number");
            }
            if (File.Exists(logFileDirectory))
            {
                FileInfo fileInfo = new FileInfo(logFileDirectory);
                long oneMBSize = 1024000;
                long fileSizeMB = fileInfo.Length / oneMBSize;
                if (fileSizeMB >= maxLogFileMBSize)
                {
                    File.Delete(logFileDirectory);
                    File.Create(logFileDirectory).Dispose();
                }
            }
            else
            {
                File.Create(logFileDirectory).Dispose();
            }

        }
        public static string GetLogFilePath()
        {
            string logFileName = ConfigurationManager.AppSettings["LogFileName"];
            if (logFileName == null)
            {
                return null;
            }
            return AppDomain.CurrentDomain.BaseDirectory + logFileName;
        }
        public static void WriteLineLog(string text)
        {
            Console.WriteLine(text);
            text += Environment.NewLine;
            string logFileDirectory = GetLogFilePath();
            if(logFileDirectory == null)
            {
                return;
            }
            if (File.Exists(logFileDirectory))
            {
                File.AppendAllText(logFileDirectory, text);
            }

        }
        public static void SendPing(string url)
        {
            try
            {
                WriteLineLog("Init request to " + url);
                string html = string.Empty;
                string authorizationValue = ConfigurationManager.AppSettings["AuthorizationValue"];
                WriteLineLog("Doing request");
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                HttpStatusCode statusCode;
                request.AutomaticDecompression = DecompressionMethods.GZip;
                request.Headers.Add("Authorization", authorizationValue);
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    html = reader.ReadToEnd();
                    statusCode = response.StatusCode;
                }
                WriteLineLog("Request finished with status " + (int)statusCode);
                if(statusCode == HttpStatusCode.OK)
                {
                    WriteLineLog(url + " request finished successfully!!");
                }
                WriteLineLog(html);
            }
            catch (Exception exception)
            {
                var properties = exception.GetType()
                           .GetProperties();
                var fields = properties
                                 .Select(property => new {
                                     Name = property.Name,
                                     Value = property.GetValue(exception, null)
                                 })
                                 .Select(x => String.Format(
                                     "{0} = {1}",
                                     x.Name,
                                     x.Value != null ? x.Value.ToString() : String.Empty
                                 ));
                string exceptionMessage = String.Join("\n", fields);
                WriteLineLog(exceptionMessage);
            }
            WriteLineLog("<------------------------------------------->");
        }
    }
}
