using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BlobStorage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ENI.Controller
{
    /// <summary>
    /// Descrição resumida de FileUpload
    /// </summary>
    public class FileUpload : IHttpHandler
    {
        private class uploadResponse
        {
            public bool media_was_uploaded { get; set; }
            public string message { get; set; }
            public string media_url { get; set; }
            public string media_type { get; set; }
        }

        public void ProcessRequest(HttpContext context)
        {
            if (IsLogged.loggedUser == null)
            {
                context.Response.StatusCode = 401;
                return;
            }

            context.Response.StatusCode = 400;
            context.Response.ContentType = "application/json";
            HttpFileCollection files = context.Request.Files;
            DateTime today = DateTime.Now;
            uploadResponse upload_response = new uploadResponse();
            upload_response.media_was_uploaded = false;

            if (files.Count > 0)
            {               
                HttpPostedFile fileUploaded = files[0];

                float file_size_limit_mb = 20f; // 4mb
                int file_size = fileUploaded.ContentLength / 1024;
                float file_size_limit = file_size_limit_mb * 1024;

                if (file_size > file_size_limit)
                {
                    upload_response.message = $"Arquivo muito pesado. Considere o limite de {file_size_limit_mb}mb.";
                } 
                else
                { 
                    string containerStorageName = ConfigurationManager.AppSettings.Get("ContainerStorageName");
                    string accountName = ConfigurationManager.AppSettings.Get("AccountName");
                    BlobServiceClient blobServiceClient = Common.CreateblobServiceClientFromConnectionString();                
                    BlobContainerClient container = blobServiceClient.GetBlobContainerClient(containerStorageName);

                    string file_name = String.Format("{0:ddMMyyyyHHmmss}", today);
                    string[] ext_split = fileUploaded.FileName.Split(new char[] { '.' });
                    string ext = ext_split[ext_split.Length - 1];
                    string file_path = string.Format("{0}.{1}", file_name, ext);

                    BlobClient blobClient = container.GetBlobClient(file_path);
                    BlobHttpHeaders blobHttpHeader = new BlobHttpHeaders { ContentType = fileUploaded.ContentType };

                    try
                    {
                        BlobContentInfo returnedInfo = blobClient.Upload(fileUploaded.InputStream, new BlobUploadOptions { HttpHeaders = blobHttpHeader });
                        context.Response.StatusCode = 200;
                        upload_response.message = "Arquivo salvo com sucesso.";
                        upload_response.media_was_uploaded = true;
                        upload_response.media_type = fileUploaded.ContentType;
                        upload_response.media_url = string.Format("https://{0}.blob.core.windows.net/{1}/{2}", accountName, containerStorageName, file_path);
                    }
                    catch (Exception er)
                    {
                        upload_response.message = "Error: " + er.Message;
                    }
                }
            }
            else
                upload_response.message = "Nenhum arquivo encontrado.";

            context.Response.Write(JsonConvert.SerializeObject(upload_response));
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}