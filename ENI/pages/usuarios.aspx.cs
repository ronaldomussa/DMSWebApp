using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ENI.pages
{
    public partial class usuarios : IsLogged
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                preencherCombos();
                showMessage();
            }
        }

        private void preencherCombos()
        {
            eniEntities db = new eniEntities();

            #region cbo profile
            cboProfile.DataSource = (from a in db.user_role where a.is_active == true orderby a.name descending select a).ToList();
            cboProfile.DataTextField = "name";
            cboProfile.DataValueField = "id";
            cboProfile.DataBind();
            cboProfile.Items.Insert(0, new ListItem("Selecione", string.Empty, true));
            #endregion
        }

        private void showMessage()
        {
            string msg_code = Request["msg"];
            string msg = string.Empty;

            if (!string.IsNullOrEmpty(msg_code))
            {
                try
                {
                    switch (msg_code)
                    {
                        case "100": alert("Salvo!", "Registro adicionado com sucesso.", "success", "save"); break;
                        case "000": alert("Opa!", "Não foi possivel executar essa ação", "error", "save"); break;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        protected void btnAdicionar_Click(object sender, EventArgs e)
        {
            save(true);
        }

        protected void btnEditar_Click(object sender, EventArgs e)
        {
            save(false);
        }

        private string save(bool novo)
        {
            string name = txtNome.Value.Trim();
            string email = txtEmail.Value.Trim();

            if (!Util.IsValidEmail(email))
                return "Email invalido";

            string phone = txtPhone.Value.Trim();
            string password = txtPassowrd.Value.Trim();

            bool auto = chkAutoPassword.Checked;
            Nullable<int> role_id = null;
            Nullable<int> id = null;

            if (!novo)
                id = int.Parse(hfdItemId.Value);

            if (!string.IsNullOrEmpty(cboProfile.SelectedValue))
                role_id = int.Parse(cboProfile.SelectedValue);
            else {

                // Somente Usuário perfil Controle Total, pode criar outro.
                role_id = 2; // Padrão    

            }

            Controller.Users ctrl = new Controller.Users();

            string resposta = ctrl.Save(novo, id, name, email, phone, auto, password, role_id);

            if (string.IsNullOrEmpty(resposta))
                Response.Redirect(Request.Path + "/?msg=100");
            else
                alert("Opa!", resposta, "info", "erro");

            return null;
        }

        private void alert(string titulo, string texto, string tipo, string key)
        {
            ClientScript.RegisterStartupScript(GetType(), key, string.Format("Swal.fire('{0}','{1}', '{2}');", titulo, texto, tipo), true);
        }
    }
}