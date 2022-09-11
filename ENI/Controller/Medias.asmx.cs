using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BlobStorage;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Services;

namespace ENI.Controller
{
    /// <summary>
    /// Descrição resumida de Medias
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que esse serviço da web seja chamado a partir do script, usando ASP.NET AJAX, remova os comentários da linha a seguir. 
    [System.Web.Script.Services.ScriptService]
    public class Medias : System.Web.Services.WebService
    {

        [WebMethod]
        public List<media> List()
        {
            if (IsLogged.loggedUser == null)
            {
                Context.Response.StatusCode = 401;
                return null;
            }

            //Thread.Sleep(2000); // para testes
            return MediasController.List();
        }

        [WebMethod]
        public string Insert(string name, string start_date, string end_date, string media_type_id, string media_type, string media_url, string expose_timing, string insertions_limit, bool is_active, bool expose_at_all, string expose_in)
        {
            return Save(null, name, start_date, end_date, media_type_id, media_type, media_url, expose_timing, insertions_limit, is_active, expose_at_all, expose_in);
        }

        [WebMethod]
        public string Edit(int id, string name, string start_date, string end_date, string media_type_id, string media_type, string media_url, string expose_timing, string insertions_limit, bool is_active, bool expose_at_all, string expose_in)
        {
            return Save(id, name, start_date, end_date, media_type_id, media_type, media_url, expose_timing, insertions_limit, is_active, expose_at_all, expose_in);
        }

        private string Save(int? id, string name, string start_date, string end_date, string media_type_id, string media_type, string media_url, string expose_timing, string insertions_limit, bool is_active, bool expose_at_all, string expose_in)
        {
            if (IsLogged.loggedUser == null)
            {
                Context.Response.StatusCode = 401;
                return null;
            }

            Context.Response.StatusCode = 400;

            media item = new media();
            DateTime startDate;
            DateTime endDate;

            if (string.IsNullOrEmpty(name))
                return "Digite um nome para esta mídia";

            int mediaTypeId;
            if (!int.TryParse(media_type_id, out mediaTypeId))
                return "Digite um tipo de midia válido";

            //if (string.IsNullOrEmpty(media_url))
            //    return "Carregue um arquivo de mídia (.jpg ou .mp4)";

            int exposeTiming;
            if (mediaTypeId != 1)
            {
                if (!int.TryParse(expose_timing, out exposeTiming))
                    return "Digite um tempo de exibição";
            }

            int insertionsLimit;
            if (!string.IsNullOrEmpty(insertions_limit))
            {
                if (!int.TryParse(insertions_limit, out insertionsLimit))
                    return "Digite um limite de inserção válido ou deixe em branco";
                else
                    item.insertions_limit = insertionsLimit;
            }

            if (!DateTime.TryParse(start_date, out startDate))
                return "Data de inicio formato inválido";

            if (!DateTime.TryParse(end_date, out endDate))
                return "Data de fim formato inválido";

            item.name = name;
            item.start_date = startDate;
            item.end_date = endDate;
            item.media_type_id = mediaTypeId;
            item.media_type = media_type;
            item.media_url = media_url;

            // only admin users can put a media active;
            if (IsLogged.loggedUser.is_super_user || IsLogged.loggedUser.user_role_id.Value == 2)
                item.is_active = is_active;
            else
                item.is_active = false;

            // if this media has not a file uploaded
            if (string.IsNullOrEmpty(media_url))
                item.is_active = false;

            item.expose_at_all = expose_at_all;
            item.expose_in = expose_in;

            // is edit
            if (id != null)
            {
                item.id = id.Value;
                item.last_modified_date = DateTime.Now;
                return MediasController.Edit(item);
            }
            else
            {
                item.created_date = DateTime.Now;
                item.created_by = IsLogged.loggedUser.name;
                return MediasController.Insert(item);
            }
        }

        [WebMethod]
        public string Remove(int id)
        {
            if (IsLogged.loggedUser == null)
            {
                Context.Response.StatusCode = 401;
                return null;
            }

            // only admin and superusers can put a media active;
            if (!IsLogged.loggedUser.is_super_user)
            {
                if (IsLogged.loggedUser.user_role_id != 2)
                {
                    Context.Response.StatusCode = 405;
                    return "Ação não permitida para este usuário.";
                }
            }                       

            return MediasController.Remove(id);
        }

        [WebMethod]
        public string ToggleActivation(int id)
        {
            if (IsLogged.loggedUser == null)
            {
                Context.Response.StatusCode = 401;
                return null;
            }

            // only admin and superusers can put a media active;
            if (!IsLogged.loggedUser.is_super_user)
            {
                if (IsLogged.loggedUser.user_role_id != 2)
                {
                    Context.Response.StatusCode = 405;
                    return "Ação não permitida para este usuário.";
                }
            }

            Context.Response.StatusCode = 400;

            return MediasController.ToggleActivation(id);
        }
        [WebMethod]
        public string RemoveMediaFromAzure(int media_id)
        {
            if (IsLogged.loggedUser == null)
            {
                Context.Response.StatusCode = 401;
                return null;
            }

            // only admin and superusers can put a media active;
            if (!IsLogged.loggedUser.is_super_user)
            {
                if (IsLogged.loggedUser.user_role_id != 2)
                {
                    Context.Response.StatusCode = 405;
                    return "Ação não permitida para este usuário.";
                }
            }

            return MediasController.RemoveMediaFromAzure(media_id); ;
        }
        
    }
}
