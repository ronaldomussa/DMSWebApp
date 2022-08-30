using ENI.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ENI
{
    public partial class Index : IsLogged
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            litNome.Text = loggedUser.name;            
        }
    }
}