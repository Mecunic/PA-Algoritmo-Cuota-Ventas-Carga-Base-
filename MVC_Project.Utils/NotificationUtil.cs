using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace PlataformaSWC.Utils
{
    public class NotificationUtil
    {
        public static void SendNotification(List<string> recipients, Dictionary<string, string> customParams, string template)
        {
            string hubURL = ConfigurationManager.AppSettings["NotificationHubUrl"];
            string apiKey = ConfigurationManager.AppSettings["NotificationHubApiKey"];
            NotificationHubClient.NotificationHubClient client = new NotificationHubClient.NotificationHubClient(hubURL, apiKey);
            Dictionary<string, string> authParams = new Dictionary<string, string>();
            authParams["username"] = ConfigurationManager.AppSettings["SendGridUser"];
            authParams["password"] = ConfigurationManager.AppSettings["SendGridPassword"];
            NotificationHubModels.NotificationResponse response = client.SendNotification(new NotificationHubModels.NotificationMessage()
            {
                Type = "EMAIL",
                Provider = "SENDGRID",
                Template = template, //"51d34567-1960-4501-8e89-e349c18275e6",
                CustomParams = customParams, //new Dictionary<string, string>() { { "param1", userBO.FirstName }, { "param2", "http://localhost:2683/Account/Confirmation?Uuid=" } },
                Recipients = recipients, //new List<string> { userBO.Email },
                ProviderAuthParams = authParams,
            });
        }

        public static AlternateView InsertImages(AlternateView alternateView, IList<Tuple<MemoryStream, string>> images)
        {
            foreach(var item in images)
            {
                LinkedResource linkedResource = new LinkedResource(item.Item1, new ContentType("image/jpeg"));
                linkedResource.ContentId = item.Item2;
                //linkedResource.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;

                alternateView.LinkedResources.Add(linkedResource);
            }
            

            return alternateView;
        }

        public static string GenerateHTML(AlternateView alternateView, string template)
        {
            MailMessage messageTemp = new MailMessage{ Body = template, IsBodyHtml = true };
            messageTemp.AlternateViews.Add(alternateView);
            var stream = messageTemp.AlternateViews[0].ContentStream;
            byte[] byteBuffer = new byte[stream.Length];
            return System.Text.Encoding.ASCII.GetString(byteBuffer, 0, stream.Read(byteBuffer, 0, byteBuffer.Length));
            //using (var rd = new StreamReader(stream))
            //{
            //    return rd.ReadToEnd();
            //}
        }
    }
}
