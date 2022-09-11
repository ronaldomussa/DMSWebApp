using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ENI.Controller
{
    public class DisplayController
    {
        public static List<display> List()
        {
            List<display> displayList = new List<display>();
            eniEntities db = new eniEntities();
            displayList = (from a in db.display
                           orderby a.last_modified_date descending, a.created_date descending
                           select a).ToList();

            return displayList;
        }

        public static string ToggleActivation(int id)
        {
            string resposta = string.Empty;

            try
            {
                eniEntities db = new eniEntities();

                var find = (from a in db.display
                            where a.id == id
                            select a);

                if (find.Any())
                {
                    var item = find.FirstOrDefault();
                    item.is_active = !item.is_active;

                    if (db.SaveChanges() > 0)
                    {
                        HttpContext.Current.Response.StatusCode = 200;
                        resposta = $"Display {item.name} {(item.is_active ? "está ATIVO." : "foi DESATIVADO.")}";
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

        public static string Insert(display newItem)
        {
            eniEntities db = new eniEntities();

            newItem.created_date = DateTime.Now;
            newItem.created_by = IsLogged.loggedUser.name;
            db.display.Add(newItem);

            if (db.SaveChanges() > 0)
            {
                HttpContext.Current.Response.StatusCode = 200;
                return "Salvo com sucesso";
            }
            else
                return "Falha ao registrar";
        }

        public static string Edit(display editItem)
        {
            eniEntities db = new eniEntities();

            var itemFound = db.display.Where(o => o.id == editItem.id);
            
            if(itemFound.Any())
            {
                var item = itemFound.FirstOrDefault();
                item.name = editItem.name;
                item.token = editItem.token;
                item.orientation = editItem.orientation;
                item.display_size = editItem.display_size;
                item.location = editItem.location;
                item.is_active = editItem.is_active;
                
                item.last_modified_date = DateTime.Now;

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

            var itemFound = db.display.Where(o => o.id == id);

            if (itemFound.Any())
            {
                var item = itemFound.FirstOrDefault();
                db.display.Remove(item);

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
    }
}