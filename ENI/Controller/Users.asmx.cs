using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.Services;
using ENI.Classes;

namespace ENI.Controller
{
    /// <summary>
    /// Summary description for Usuarios
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 

    [System.Web.Script.Services.ScriptService]
    public class Users : System.Web.Services.WebService
    {

        [WebMethod]
        public List<userDTO> List()
        {


            var Logado = IsLogged.loggedUser;

            if (Logado == null)
            {
                Context.Response.StatusCode = 401;
                return null;
            }

            eniEntities db = new eniEntities();

            var list = (from a in db.user
                        where 
                            a.is_super_user == false
                            //&& a.id != IsLogged.loggedUser.id
                        select new userDTO { 
                            name = a.name,
                            created_by = a.created_by,
                            created_date = a.created_date,
                            email = a.email,
                            id = a.id,
                            is_active = a.is_active,
                            is_super_user = a.is_super_user,
                            last_modified_date = a.last_modified_date,
                            //password = a.password,
                            phone = a.phone,
                            token = a.token,
                            token_expiration_date = a.token_expiration_date,
                            user_picture_url = a.user_picture_url,
                            user_role_id = a.user_role_id
                        });

            //Thread.Sleep(2000);
            return list.ToList();
        }

        [WebMethod]
        public string Insert(string name, string email, string phone, string password, string user_role_id)
        {
            
            var Logado = IsLogged.loggedUser;

            if (Logado == null)
            {
                Context.Response.StatusCode = 401;
                return "Usuário não logado";
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

            if (string.IsNullOrWhiteSpace(name))
                return "Digite um nome válido";

            if (string.IsNullOrWhiteSpace(email))
                return "Digite um email válido";

            if (string.IsNullOrWhiteSpace(password))
                return "Digite uma senha válida";

            int userRoleId;
            if(!int.TryParse(user_role_id, out userRoleId))
                return "Selecione um perfil";

            return UserController.Save(null, name, email, phone, password, userRoleId, false);
            
        }

        [WebMethod]
        public string Edit(int id, string name, string email, string phone, string password, string user_role_id)
        {
            var Logado = IsLogged.loggedUser;

            if (Logado == null)
            {
                Context.Response.StatusCode = 401;
                return "Usuário não logado";
            }

            if(IsLogged.loggedUser.id != id)
            {
                // only admin and superusers can put a media active;
                if (!IsLogged.loggedUser.is_super_user)
                {
                    if (IsLogged.loggedUser.user_role_id != 2)
                    {
                        Context.Response.StatusCode = 405;
                        return "Ação não permitida para este usuário.";
                    }
                }
            }

            Context.Response.StatusCode = 400;

            if (string.IsNullOrWhiteSpace(name))
                return "Digite um nome válido";

            if (string.IsNullOrWhiteSpace(email))
                return "Digite um email válido";

            if (string.IsNullOrWhiteSpace(password))
                return "Digite uma senha válida";

            int userRoleId;
            if (!int.TryParse(user_role_id, out userRoleId))
                return "Selecione um perfil válido";

            return UserController.Save(id, name, email, phone, password, userRoleId, false);

        }

        [WebMethod]
        public string ToggleActivation(int id)
        {

            if (IsLogged.loggedUser == null)
            {
                Context.Response.StatusCode = 401;
                return "Nenhum usuário autenticado para esta ação.";
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

            return UserController.ToggleActivation(id); ;
        }

        [WebMethod]
        public string Remove(int id)
        {
            Context.Response.StatusCode = 400;

            if (IsLogged.loggedUser == null)
            {
                Context.Response.StatusCode = 401;
                return "Nenhum usuário autenticado para esta ação.";
            }

            if (IsLogged.loggedUser.is_super_user)
            {
                Context.Response.StatusCode = 405;
                return UserController.Remove(id); ;
            }

            return "Ação não permitida para este usuário.";
            
        }

        [WebMethod]
        public List<user_role> UserRoleList()
        {
            var Logado = IsLogged.loggedUser;

            if (Logado == null)
            {
                Context.Response.StatusCode = 401;
                return null;
            }

            var list = new List<user_role>();
            eniEntities db = new eniEntities();

            list = (from a in db.user_role
                    where a.is_active == true
                    select a).ToList();

            return list;
        }
    }
}
