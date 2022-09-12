using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace ENI.Controller
{
    /// <summary>
    /// Descrição resumida de DisplayFunctions
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que esse serviço da web seja chamado a partir do script, usando ASP.NET AJAX, remova os comentários da linha a seguir. 
    [System.Web.Script.Services.ScriptService]
    public class DisplayFunctions : System.Web.Services.WebService
    {

        [WebMethod]
        public string HelloWorld(string nome)
        {
            return "Olá, " + nome;
        }
    }
}
