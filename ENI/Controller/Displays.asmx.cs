using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Services;

namespace ENI.Controller
{
    /// <summary>
    /// Descrição resumida de Displays
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que esse serviço da web seja chamado a partir do script, usando ASP.NET AJAX, remova os comentários da linha a seguir. 
    [System.Web.Script.Services.ScriptService]
    public class Displays : System.Web.Services.WebService
    {

        [WebMethod]
        public List<display> List()
        {
            if (IsLogged.loggedUser == null)
            {
                Context.Response.StatusCode = 401;
                return null;
            }

            //Thread.Sleep(2000); // para testes
            return DisplayController.List();
        }

        [WebMethod]
        public string Insert(string name, string orientation, string token, string display_size, string location, bool is_active)
        {
            if (IsLogged.loggedUser == null)
            {
                Context.Response.StatusCode = 401;
                return null;
            }

            Context.Response.StatusCode = 400;

            if (string.IsNullOrEmpty(name))
                return "Nome não pode ser vazio";

            int orientationId;
            if (!int.TryParse(orientation, out orientationId))
                return "Selecione uma orientação válida";

            display newItem = new display();
            newItem.name = name;
            newItem.orientation = orientationId;
            newItem.token = token;
            newItem.display_size = display_size;
            newItem.location = location;
            newItem.is_active = is_active;

            return DisplayController.Insert(newItem);
        }

        [WebMethod]
        public string Edit(string id, string name, string orientation, string token, string display_size, string location, bool is_active)
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

            int displayId;
            if (!int.TryParse(id, out displayId))
                return "ID inválido";

            if (string.IsNullOrEmpty(name))
                return "Nome não pode ser vazio";

            int orientationId;
            if (!int.TryParse(orientation, out orientationId))
                return "Selecione uma orientação válida";

            display editedItem = new display();
            editedItem.id = displayId;
            editedItem.name = name;
            editedItem.orientation = orientationId;
            editedItem.token = token;
            editedItem.display_size = display_size;
            editedItem.location = location;
            editedItem.is_active = is_active;

            return DisplayController.Edit(editedItem);
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

            return DisplayController.ToggleActivation(id);
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

            return DisplayController.Remove(id);
        }
    }
}
