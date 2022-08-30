using System;
using System.Collections.Generic;
using System.Linq;
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
                            a.is_super_user == false && 
                            a.id != IsLogged.loggedUser.id
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
            

            return list.ToList();
        }

        [WebMethod]
        public string Save(bool isNew, int? id, string name, string email, string phone, bool autopassword, string password, int? role_id)
        {
            var Logado = IsLogged.loggedUser;

            if (Logado == null)
            {
                Context.Response.StatusCode = 401;
                return "Usuário não logado";
            }

            eniEntities db = new eniEntities();
            user item = new user();

            if (isNew)
                item = new user();
            else
            {
                var model = (from a in db.user where a.id == id select a);

                if (model.Count() > 0)
                    item = model.FirstOrDefault();
                else
                    return "Não foi possivel editar.";
            }

            string pass = string.Empty;

            item.name = name;
            
            if (Util.IsValidEmail(email))
                item.email = email;
            else
                return "Email inválido";

            item.phone = phone;

            if (!autopassword)
                pass = password;
            else
                pass = Util.GenerateRandomNumber(6);

            item.password = Util.GenerateHashMd5(pass);

            // if empty, the user has full control
            if (role_id.HasValue)
                item.user_role_id = role_id.Value;

            if (isNew)
            {
                item.created_date = DateTime.Now;
                item.is_active = true;
                item.created_by = IsLogged.loggedUser.name;
                item.is_super_user = false;
                db.user.Add(item);
            }

            if (db.SaveChanges() > 0)
                return "Salvo com sucesso";
            else
                return "Falha ao registrar";
        }

        [WebMethod]
        public string Remover(int id)
        {

            if (IsLogged.loggedUser == null)
            {
                Context.Response.StatusCode = 401;
                return "Nenhum usuário autenticado para esta ação.";
            }

            if (!IsLogged.loggedUser.is_super_user.Value)
            {
                Context.Response.StatusCode = 405;
                return "Ação não permitita para este usuário.";
            }

            string resposta = string.Empty;
            eniEntities db = new eniEntities();

            var busca = (from a in db.user
                         where a.id == id
                         select a);

            if (busca.Count() > 0)
            {
                var item = busca.FirstOrDefault();
                item.is_active = !item.is_active;

                if (db.SaveChanges() > 0)
                    resposta = "Usuário " + (item.is_active.Value ? "está ativo." : "foi inativado.");
            }
            else
            {
                Context.Response.StatusCode = 400;
                resposta = "Registro não encontrado.";
            }

            return resposta;
        }

        [WebMethod]
        public List<user_role> RoleList()
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
