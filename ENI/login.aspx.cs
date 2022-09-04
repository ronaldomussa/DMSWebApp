using ENI.Classes;
using System;
using System.Web;

namespace ENI
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        protected void btnEnter_Click(object sender, EventArgs e)
        {
            string email = txtLogin.Text;
            string password = txtPassword.Text;

            var userTryingLogin = Controller.Login.SingIn(email, password);

            if (userTryingLogin != null)
            {
                // user keept in session
                userSessionDTO loggedUser = new userSessionDTO();
                loggedUser.id = userTryingLogin.id;
                loggedUser.name = userTryingLogin.name;
                loggedUser.phone = userTryingLogin.phone;
                loggedUser.email = userTryingLogin.email;
                loggedUser.user_picture_url = userTryingLogin.user_picture_url;
                loggedUser.user_role_id = userTryingLogin.user_role_id;
                loggedUser.is_super_user = userTryingLogin.is_super_user;

                IsLogged.SetSessionAndCookie(loggedUser);

                string url_to_redirect = Request.QueryString["page"];
                if (string.IsNullOrEmpty(url_to_redirect))
                    url_to_redirect = "home";
                Response.Redirect(url_to_redirect, true);
            }
            
            ClientScript.RegisterStartupScript(GetType(), "Erro", "Swal.fire('Ops!','Email ou Senha incorretos.', 'error');", true);
        }

        protected void btnEsqueciSenha_Click(object sender, EventArgs e)
        {
            //string email = txtEmailEsqueceu.Text;
            //string msgAlert = string.Empty;
            //string msg = string.Empty;

            //if (email.Trim() == "" || !email.Contains("@") || !email.Contains(".") || email.Length < 8)
            //{
            //    ClientScript.RegisterStartupScript(GetType(), "Erro", "swal('Erro','Email invalido', 'error');", true);
            //    return;
            //}

            //eternalEntities banco = new eternalEntities();
            //var clienteExiste = banco.cliente.Where(o => o.email == email);

            //if (clienteExiste.Count() > 0)
            //{
            //    var clienteEncontrado = clienteExiste.FirstOrDefault();

            //    string html =
            //        @"
            //        <div style='padding: 30px; background-color:#3b4874; text-align: justify'> 
            //            <img style='display: block; width:150px; margin:20px auto; text-align: center' src='http://portal.eternal.ind.br/assets/img/logo-eternal-150.png'>
            //            <div style='padding: 30px;width:600px; margin: 0 auto; background-color:#fff; color: #666; border-radius: 3px'> 
            //                <h2>Olá <b>{0}</b></h2>
            //                <p>Foi solicitado a recuperação de senha através de nosso site.</p>
            //                <div style='padding: 15px 25px; border-left:2px solid #3b4874'>
            //                    <p>
            //                        <b>Senha:</b> {1}
            //                    </p>
            //                </div>
            //                <p>Você pode alterara senha a qualquer momento dentro do portal.</p>
            //                <p>Caso não tenha feito essa solicitação apenas ignore este email.</p>
            //                <p>Qualquer dúvida entre em contato conosco.</p>
            //                <hr>
            //                <p><i>Equipe Portal Eternal</i></p>
            //            </div>
            //        </div>
            //        ";

            //    html = string.Format(html, clienteEncontrado.nome, clienteEncontrado.senha);

            //    Util.EnviarEmail(clienteEncontrado.email.Trim().ToLower(), "Recuperação de Senha - Eternal", html, string.Empty);

            //    ClientScript.RegisterStartupScript(GetType(), "Erro", "swal('Legal!','O lembrete de senha foi enviado para seu email', 'success');", true);
            //}
            //else
            //    ClientScript.RegisterStartupScript(GetType(), "Erro", "swal('Erro!','Este email nao consta em nossa base.', 'error');", true);

        }
    }
}