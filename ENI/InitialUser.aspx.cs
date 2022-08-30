using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ENI
{
    public partial class InitialUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                eniEntities db = new eniEntities();
                Role.DataSource = db.user_role.ToList();
                Role.DataValueField = "id";
                Role.DataTextField = "name";
                Role.DataBind();
            }
            
        }

        protected void Save_Click(object sender, EventArgs e)
        {
            eniEntities db = new eniEntities();



            user newUser = new user();
            newUser.name = Nome.Text;
            newUser.email = Email.Text;
            newUser.phone = Phone.Text;
            newUser.password = Util.GenerateHashMd5(Password.Text);
            newUser.is_super_user = SuperUser.Checked;

            newUser.created_by = "system";
            newUser.created_date = DateTime.Now;

            
            if(!SuperUser.Checked)
                newUser.user_role_id = int.Parse(Role.SelectedValue);

            db.user.Add(newUser);
            string msg = "Erro ao salvar";

            if (db.SaveChanges() > 0)
                msg = "Salvo com sucesso";

            ScriptManager.RegisterStartupScript(this, GetType(), "", "alert('"+ msg + "');", true);

        }
    }
}