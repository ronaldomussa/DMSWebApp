using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ENI
{
    public partial class Main : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //lblLoggedUserEmail.Text = IsLogged.loggedUser.email;

            AssemblyName assName = new AssemblyName(Assembly.GetAssembly(typeof(Main)).FullName);
            IsLogged.versao = assName.Version;
            //lblUsuarioLogado.Text = IsLogged.loggedUser.email;

            navUsers.Visible = (IsLogged.loggedUser.is_super_user.Value);

        }

        protected void btnLoggout_Click(object sender, EventArgs e)
        {
            IsLogged.Loggout();
        }
    }
}