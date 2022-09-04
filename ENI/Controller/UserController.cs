using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ENI.Controller
{
    public class UserController
    {
        /// <summary>
        /// Salva novo uauário ou edita existente através de parametro ID;
        /// </summary>
        /// <param name="isNew"></param>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="phone"></param>
        /// <param name="password"></param>
        /// <param name="user_role_id"></param>
        /// <param name="is_superuser"></param>
        /// <returns></returns>
        public static string Save(int? id, string name, string email, string phone, string password, int? user_role_id, bool is_superuser)
        {
            try
            {
                eniEntities db = new eniEntities();
                user User = new user();
                bool isNewUser = !id.HasValue;
                HttpContext.Current.Response.StatusCode = 400;

                //valid email
                if (!Util.IsValidEmail(email))
                    return "Email inválido.";

                if (isNewUser)
                {
                    User = new user();

                    if (db.user.Where(o => o.email == email).Any()) {
                        return "Email já em uso.";
                    }
                    else
                        User.email = email;

                }
                else
                {
                    var model = (from a in db.user where a.id == id.Value select a);

                    if (model.Any()) 
                    { 
                        User = model.FirstOrDefault();
                        
                        if (db.user.Where(o => o.email == email && o.id != id).Any())
                            return "Email já em uso.";

                    }
                    else
                        return "Usuário não encontrado, não foi possivel editar.";
                }

                User.name = name;
                User.phone = phone;
                User.password = Util.GenerateHashMd5(password);

                if(user_role_id.HasValue)
                    User.user_role_id = user_role_id.Value;

                if (isNewUser)
                {
                    User.created_date = DateTime.Now;
                    User.is_active = true;
                    User.created_by = IsLogged.loggedUser.name;
                    User.is_super_user = is_superuser;
                    db.user.Add(User);
                }
                else
                {
                    User.last_modified_date = DateTime.Now;
                }

                if (db.SaveChanges() > 0)
                {
                    HttpContext.Current.Response.StatusCode = 200;
                    return "Salvo com sucesso";
                }
                else
                    return "Falha ao registrar";
            }
            catch (Exception er)
            {
                return "Erro: " + er.Message;
            }
        }

        /// <summary>
        /// Alterna ativação/inativação de usuário
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string ToggleActivation(int id)
        {
            string resposta = string.Empty;

            try
            {
                eniEntities db = new eniEntities();

                var find = (from a in db.user
                            where a.id == id
                            select a);

                if (find.Any())
                {
                    var item = find.FirstOrDefault();
                    item.is_active = !item.is_active;

                    if (db.SaveChanges() > 0)
                    {
                        HttpContext.Current.Response.StatusCode = 200;
                        resposta = "Usuário " + (item.is_active ? "está ativo." : "foi inativado.");
                    }
                    else
                        resposta = "Nenhuma mudança"; 
                }
                else
                    resposta = "Registro não encontrado.";
            }
            catch (Exception er)
            {
                resposta = "Erro: " + er.Message;
            }
            
            return resposta;
        }

        public static string Remove(int id)
        {
            string resposta = string.Empty;

            try
            {
                eniEntities db = new eniEntities();

                var find = (from a in db.user
                            where a.id == id
                            select a);

                if (find.Any())
                {
                    var item = find.FirstOrDefault();
                    db.user.Remove(item);

                    if (db.SaveChanges() > 0)
                    {
                        HttpContext.Current.Response.StatusCode = 200;
                        resposta = "Usuário removido com sucesso";
                    }
                    else
                        resposta = "Nenhuma mudança";
                }
                else
                    resposta = "Registro não encontrado.";
            }
            catch (Exception er)
            {
                resposta = "Erro: " + er.Message;
            }

            return resposta;
        }
    }
}