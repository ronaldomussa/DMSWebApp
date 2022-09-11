using Azure.Storage.Blobs;
using BlobStorage;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ENI.Controller
{
    public class MediasController
    {
        public static List<media> List(){

            string result = string.Empty;

            List<media> itemList = new List<media>();
            eniEntities db = new eniEntities();
            itemList = (from a in db.media
                           orderby a.last_modified_date descending, a.created_date descending
                           select a).ToList();

            return itemList;

        }

        public static string Insert(media newItem)
        {
            eniEntities db = new eniEntities();
            db.media.Add(newItem);

            if (db.SaveChanges() > 0)
            {
                HttpContext.Current.Response.StatusCode = 200;
                return "Salvo com sucesso";
            }
            else
                return "Falha ao registrar";
        }

        public static string Edit(media editItem)
        {

            eniEntities db = new eniEntities();

            var itemFound = db.media.Where(o => o.id == editItem.id);

            if (itemFound.Any())
            {
                var item = itemFound.FirstOrDefault();
                item.name = editItem.name;
                item.start_date = editItem.start_date;
                item.end_date = editItem.end_date;
                item.media_type_id = editItem.media_type_id;
                item.media_type = editItem.media_type; 
                item.media_url = editItem.media_url;
                item.last_modified_date = editItem.last_modified_date;
                item.insertions_limit = editItem.insertions_limit;
                item.is_active = editItem.is_active;
                item.expose_at_all = editItem.expose_at_all;
                item.expose_in = editItem.expose_in;
                item.expose_timing = editItem.expose_timing;

                if (db.SaveChanges() > 0)
                {
                    HttpContext.Current.Response.StatusCode = 200;
                    return "Salvo com sucesso";
                }
                else
                    return "Falha ao registrar";
            }
            else
                return "Item não encontrado";

        }

        public static string Remove(int id)
        {

            eniEntities db = new eniEntities();

            var itemFound = db.media.Where(o => o.id == id);

            if (itemFound.Any())
            {
                var item = itemFound.FirstOrDefault();

                if (!string.IsNullOrEmpty(item.media_url))
                    RemoveMediaFromAzureByMediaUrl(item.media_url);

                db.media.Remove(item);

                if (db.SaveChanges() > 0)
                {
                    HttpContext.Current.Response.StatusCode = 200;
                    return "Removido com sucesso";
                }
                else
                    return "Falha ao remover";

            }
            else
                return "Item não encontrado";

        }

        private static void RemoveMediaFromAzureByMediaUrl(string media_url)
        {
            BlobServiceClient blobServiceClient = Common.CreateblobServiceClientFromConnectionString();
            string containerStorageName = ConfigurationManager.AppSettings.Get("ContainerStorageName");
            BlobContainerClient container = blobServiceClient.GetBlobContainerClient(containerStorageName);

            string[] media_url_splited = media_url.Split('/');
            string media_file_name = media_url_splited[media_url_splited.Length - 1];

            BlobClient blobClient = container.GetBlobClient(media_file_name);
            blobClient.DeleteIfExistsAsync();
        }


        public static string RemoveMediaFromAzure(int media_id)
        {
            BlobServiceClient blobServiceClient = Common.CreateblobServiceClientFromConnectionString();
            string containerStorageName = ConfigurationManager.AppSettings.Get("ContainerStorageName");
            BlobContainerClient container = blobServiceClient.GetBlobContainerClient(containerStorageName);

            eniEntities db = new eniEntities();
            var find_item = db.media.Where(o => o.id == media_id);

            HttpContext.Current.Response.StatusCode = 400;

            if (find_item.Any())
            {
                var found_item = find_item.First();

                string media_url = found_item.media_url;
                string[] media_url_splited = media_url.Split('/');
                string media_file_name = media_url_splited[media_url_splited.Length - 1];

                found_item.media_type = string.Empty;
                found_item.media_type_id = 1;
                found_item.media_url = string.Empty;
                found_item.last_modified_date = DateTime.Now;
                found_item.is_active = false;

                if (db.SaveChanges() > 0)
                {
                    BlobClient blobClient = container.GetBlobClient(media_file_name);
                    blobClient.DeleteIfExistsAsync();

                    HttpContext.Current.Response.StatusCode = 200;
                    return "Mídia excluída com sucesso.";
                }
                else
                    return "Não foi possível excluir. Tente novamente.";
            }
            else
            {
                HttpContext.Current.Response.StatusCode = 403;
                return "Mídia não localizada. Tente novamente ou contate o suporte.";
            }
        }

        public static string ToggleActivation(int id)
        {

            try
            {
                eniEntities db = new eniEntities();

                var find = (from a in db.media
                            where a.id == id
                            select a);

                if (find.Any())
                {
                    var item = find.FirstOrDefault();

                    if (string.IsNullOrEmpty(item.media_url) && !item.is_active)
                        return "Não é possivel ativar esta mídia sem arquivo, Carregue um arquivo de mídia (.jpg ou .mp4)";

                    item.is_active = !item.is_active;

                    if (db.SaveChanges() > 0)
                    {
                        HttpContext.Current.Response.StatusCode = 200;
                        return $"Media {item.name} {(item.is_active ? "está ATIVA." : "foi DESATIVADA.")}";
                    }
                    else
                        return "Nenhuma mudança";
                }
                else
                    return "Registro não encontrado.";
            }
            catch (Exception er)
            {
                return "Erro: " + er.Message;
            }

        }
    }
}