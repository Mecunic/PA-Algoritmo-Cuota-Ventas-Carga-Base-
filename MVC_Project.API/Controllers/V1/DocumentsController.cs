﻿using Microsoft.Web.Http;
using MVC_Project.API.Models.Api_Request;
using MVC_Project.API.Models.Api_Response;
using MVC_Project.Domain.Entities;
using MVC_Project.Domain.Services;
using MVC_Project.Integrations.Storage;
using MVC_Project.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace MVC_Project.API.Controllers.V1
{
    [ApiVersion("1.0")]
    [RoutePrefix("api/v{version:apiVersion}/documents")]
    public class DocumentsController : BaseApiController
    {
        IUserService _userService;
        IDocumentService _documentService;

        IStorageServiceProvider storageService;
        string containerBucketName;

        public DocumentsController(IUserService userService, IDocumentService documentsService)
        {
            _userService = userService;
            _documentService = documentsService;

            //ESTO DEBE SER DINAMICO, POR UNITY O POR WEB.CONFIG
            storageService = new AzureBlobService();
            containerBucketName = System.Configuration.ConfigurationManager.AppSettings["ContainerBucketName"];
        }


        [HttpPost]
        [Route("add")]
        [AuthorizeApiKey]
        public HttpResponseMessage AddDocuments([FromBody] DocumentRequest request)
        {
            try
            {
                List<MessageResponse> messages = new List<MessageResponse>();
                int UserId = GetUserId();

                foreach (DocumentObject documentObj in request.Documents)
                {
                    if (!string.IsNullOrWhiteSpace(documentObj.Base64))
                    {
                        string fileName = documentObj.Name;
                        var data = Convert.FromBase64String(documentObj.Base64);
                        MemoryStream ms = new MemoryStream(data);
                        Tuple<string, string> tupleUrl = storageService.UploadPublicFile(ms, fileName, containerBucketName);
                        Document newDoc = new Document()
                        {
                            Name = fileName,
                            CreationDate = DateUtil.GetDateTimeNow(),
                            Type = documentObj.Type,
                            URL = tupleUrl.Item1,
                            Uuid = tupleUrl.Item2,
                            User = new User() { Id = UserId }
                        };
                        _documentService.Create(newDoc);
                        messages.Add(new MessageResponse { Type = MessageType.info.ToString("G"), Description = string.Format("Documento agregado correctamente: {0}", newDoc.Name) });
                    }
                    else
                    {
                        messages.Add(new MessageResponse { Type = MessageType.error.ToString("G"), Description = string.Format("No se encontró el contenido del documento: {0}", documentObj.Name) });
                    }
                    
                }

                return CreateResponse(messages);
            }
            catch (Exception e)
            {
                return CreateErrorResponse(e, null);
            }
        }

        //[HttpPost]
        //[Route("addStream")]
        //[AuthorizeApiKey]
        //public async Task<HttpResponseMessage> AddDocumentStream(/*DocumentRequest request*/)
        //{
        //    var provider = await Request.Content.ReadAsMultipartAsync(new InMemoryMultipartFormDataStreamProvider());
        //    try
        //    {
        //        if (!Request.Content.IsMimeMultipartContent())
        //        {
        //            throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
        //        }
        //        int UserId = GetUserId();

        //        List<MessageResponse> messages = new List<MessageResponse>();

        //        IList<HttpContent> files = provider.Files;
        //        foreach (HttpContent fileContent in files)
        //        {
        //            Stream file = await fileContent.ReadAsStreamAsync();
        //            string fileStr = await fileContent.ReadAsStringAsync();
        //            string fileName = string.Format("IMG-{0}-{1}.jpg", "test", 1);
        //            Tuple<string, string> tupleUrl = storageService.UploadPublicFile(file, fileName, containerBucketName);
        //            //byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(fileStr);
        //            Document newDoc = new Document()
        //            {
        //                Name = fileName,
        //                CreationDate = DateUtil.GetDateTimeNow(),
        //                Type = "Otro",
        //                URL = tupleUrl.Item1,
        //                Uuid = tupleUrl.Item2,
        //                User = new User() { Id = UserId }
        //            };
        //            _documentService.Create(newDoc);
        //            messages.Add(new MessageResponse { Type = MessageType.info.ToString("G"), Description = string.Format("Documento agregado correctamente: {0}", newDoc.Name) });
        //        }
                
        //        return CreateResponse(messages);
        //    }
        //    catch (Exception e)
        //    {
        //        return CreateErrorResponse(e, null);
        //    }
        //}

    }
}