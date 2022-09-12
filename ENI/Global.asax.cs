using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace ENI
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes(RouteTable.Routes);
        }

        void RegisterRoutes(RouteCollection routes)
        {
            routes.MapPageRoute("",
                "", "~/index.aspx", false);

            routes.MapPageRoute("home",
                "home", "~/index.aspx", false);

            routes.MapPageRoute("users",
                "users", "~/pages/users.aspx", false);

            routes.MapPageRoute("displays",
                "displays", "~/pages/displays.aspx", false);

            routes.MapPageRoute("medias",
                "medias", "~/pages/medias.aspx", false);

            routes.MapPageRoute("reports",
                "reports", "~/pages/reports.aspx", false);

            routes.MapPageRoute("login",
                "login", "~/login.aspx", false);


            routes.MapPageRoute("teste",
                "teste", "~/Api/teste.asmx?op=HelloWorld", false);


        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}