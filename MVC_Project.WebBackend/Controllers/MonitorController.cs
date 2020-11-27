using MVC_Project.Integrations.Storage;
using MVC_Project.Utils;
using MVC_Project.WebBackend.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace MVC_Project.WebBackend.Controllers
{
    [AllowAnonymous]
    public class MonitorController : Controller
    {
        // GET: Monitor
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View(new MonitorViewModel());
        }

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public ActionResult Import(MonitorViewModel model)
        {
            IStorageServiceProvider storageService = null;

            if (model.StorageProvider == "azure") storageService = new AzureBlobService();
            if (model.StorageProvider == "aws") storageService = new AWSBlobService();
            if (storageService != null)
            {
                string myBucketName = System.Configuration.ConfigurationManager.AppSettings["ContainerBucketName"];
                Tuple<string, string> resultUpload = storageService.UploadPublicFile(model.ImportedFile.InputStream, model.ImportedFile.FileName, myBucketName);
                model.FinalUrl = resultUpload.Item1;
            }

            HttpResponseMessage response = ProcessImageUrl(model.FinalUrl);
            IEnumerable<string> values;
            if (response.Headers.TryGetValues("Operation-Location", out values))
            {
                model.FinalUrl = values.First();
            }

            Thread.Sleep(3000);

            model.Content = ProcessResultUrl(model.FinalUrl);

            ResultMonitor result = JsonUtil.ConvertToObject<ResultMonitor>(model.Content);
            model.ResultMonitor = result;

            return View("Index", model);
        }

        static HttpResponseMessage ProcessImageUrl(string url)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "1bf42b61b12243759fe7372f5120082a");

            // Request parameters
            queryString["language"] = "";
            var uri = "https://imagetrainingpoc.cognitiveservices.azure.com/vision/v3.0/read/analyze?" + queryString;

            HttpResponseMessage response;

            // Request body
            byte[] byteData = Encoding.UTF8.GetBytes("{'url':'"+ url + "'}");

            using (var content = new ByteArrayContent(byteData))
            {
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                response = client.PostAsync(uri, content).Result;
            }

            return response;

        }

        static string ProcessResultUrl(string url)
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "1bf42b61b12243759fe7372f5120082a");

            byte[] byteData = Encoding.UTF8.GetBytes("");
            HttpResponseMessage response = client.GetAsync(url).Result;
            
            return response.Content.ReadAsStringAsync().Result;
        }
        
     }

    public class MonitorViewModel
    {
        public HttpPostedFileBase ImportedFile { get; set; }
        
        public string FinalUrl { get; set; }

        public string Content { get; set; }

        public string StorageProvider { get; set; }

        public ResultMonitor ResultMonitor { get; set; }
}

    public class ResultMonitor
    {
        [JsonProperty]
        public AnalyzeResult analyzeResult { set; get; }
    }

    public class AnalyzeResult
    {
        [JsonProperty]
        public List<ReadResults> readResults { set; get; }
    }

    public class ReadResults
    {
        [JsonProperty]
        public int page { set; get; }
        [JsonProperty]
        public List<Lines> lines { set; get; }
    }

    public class Lines
    {
        public string text { set; get; }
    }
}