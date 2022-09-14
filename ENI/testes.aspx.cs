using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ENI
{
    public partial class testes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            

            try
            {
                eniEntities db = new eniEntities();
                grid.DataSource = db.display.ToList();
                grid.DataBind();
            }
            catch (Exception er)
            {
                lbl.Text = $"{er.Message} {er.StackTrace}";
            }
        }
    }
}