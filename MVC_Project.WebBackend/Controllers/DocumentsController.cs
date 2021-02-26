using MVC_Project.Domain.Entities;
using MVC_Project.Domain.Services;
using MVC_Project.Utils;
using MVC_Project.WebBackend.AuthManagement;
using MVC_Project.WebBackend.AuthManagement.Models;
using MVC_Project.WebBackend.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace MVC_Project.WebBackend.Controllers
{
    public class DocumentsController : BaseController
    {
        private IDocumentService _documentService;
        //IStorageServiceProvider storageService;
        string containerBucketName;
        public DocumentsController(IDocumentService documentServce)
        {
            _documentService = documentServce;

            //ESTO DEBE SER DINAMICO, POR UNITY O POR WEB.CONFIG
            //storageService = new AzureBlobService();
            containerBucketName = System.Configuration.ConfigurationManager.AppSettings["ContainerBucketName"];
        }

        [Authorize]
        public ActionResult Index()
        {

            return View();
        }

        [HttpGet, Authorize]
        public JsonResult GetAllByFilter(JQueryDataTableParams param, string filtros)
        {
            try
            {
                AuthUser authUser = Authenticator.AuthenticatedUser;

                NameValueCollection filtersValues = HttpUtility.ParseQueryString(filtros);
                filtersValues["UserId"] = "-1";
                if (authUser.Role.Code != Constants.ROLE_ADMIN && authUser.Role.Code != Constants.ROLE_IT_SUPPORT)
                {
                    filtersValues["UserId"] = Convert.ToString(authUser.Id);
                }
                var documents = _documentService.FilterBy(filtersValues, param.iDisplayStart, param.iDisplayLength);

                IList<DocumentViewModel> dataResponse = new List<DocumentViewModel>();
                foreach (var document in documents.Item1)
                {
                    DocumentViewModel resultData = new DocumentViewModel
                    {
                        Id = document.Id,
                        Uuid = document.Uuid,
                        Name = document.Name,
                        URL = document.URL,
                        DocumentType = document.Type,
                        CreationDate = document.CreationDate.ToString(Constants.DATE_FORMAT_CALENDAR),
                        Username = document.User.Email
                    };
                    dataResponse.Add(resultData);
                }

                return Json(new
                {
                    success = true,
                    param.sEcho,
                    iTotalRecords = dataResponse.Count(),
                    iTotalDisplayRecords = documents.Item2,
                    aaData = dataResponse
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return new JsonResult
                {
                    Data = new { Mensaje = new { title = "Error", message = ex.Message } },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    MaxJsonLength = Int32.MaxValue
                };
            }
        }

        [HttpPost, Authorize, ValidateAntiForgeryToken]
        public ActionResult AddDocument(DocumentViewModel model)
        {
            

            AuthUser authUser = Authenticator.AuthenticatedUser;

            HttpPostedFileBase file = Request.Files["file"];
            string fName = string.Empty;
            string fURL = string.Empty;
            string folder = string.Empty;
            if (file != null && file.ContentLength > 0)
            {
                fName = file.FileName.Replace(" ", "_");
                //folder = "user-" + authUser.Uuid.Substring(24) + "/documents/";
                Tuple<string, string> tupleUrl = null;//storageService.UploadPublicFile(file.InputStream, fName, containerBucketName);
                fURL = tupleUrl.Item1;

                Document newDoc = new Document() {
                    Name = fName,
                    CreationDate = DateUtil.GetDateTimeNow(),
                    Type = model.DocumentType,
                    URL = tupleUrl.Item1,
                    Uuid = tupleUrl.Item2,
                    User = new User() { Id = authUser.Id }
                };
                _documentService.Create(newDoc);
            }

            return Json(new { Message = fName, URL = fURL, Success = true });
        }

        [Authorize]
        public ActionResult Download(string Uuid)
        {
            AuthUser authUser = Authenticator.AuthenticatedUser;
            string SASToken = System.Configuration.ConfigurationManager.AppSettings["StorageSASToken"];
            Document document = _documentService.FindBy(x => x.Uuid == Uuid).FirstOrDefault();
            var client = new WebClient();
            var content = client.DownloadData(document.URL + SASToken);
            var stream = new MemoryStream(content);
            client.Dispose();
            return File(stream, "application/" + Path.GetExtension(document.URL).Substring(1), document.Name);
         
        }
    }
}