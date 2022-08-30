<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="ENI.login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta name="description" content="" />
    <meta name="author" content="Enisoft" />

    <title>Entre no Portal | Navetrans by ENISOFT</title>
    <link href="/node_modules/bootstrap/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link href="/node_modules/sweetalert2/dist/sweetalert2.min.css" rel="stylesheet" />
    <link href="/assets/css/_login.css?v=2" rel="stylesheet" />
    <link rel="icon" href="/assets/img/favicon.png" type="image/png" />
    <script src="/node_modules/sweetalert2/dist/sweetalert2.min.js"></script>

</head>
<body>

    <form id="form1" runat="server">

        <div class="offset-xl-7 col-xl-3 col-md-4 offset-md-4 col-sm-12 pt-3">

            <div class="form-signin">

                <div class="text-center mb-2">
                    <img src="/assets/img/logo-squarebox-m.png" alt="" />
                </div>

                <div class="shadow p-4 bg-white rounded">

                    <asp:TextBox type="email" ID="txtLogin" ClientIDMode="Static" CssClass="form-control" runat="server" placeholder="Email" required autofocus></asp:TextBox>

                    <asp:TextBox type="password" ID="txtPassword" CssClass="form-control" runat="server" placeholder="Senha" required></asp:TextBox>

                    <div class="checkbox mb-3 text-right">
                        <label>
                            <asp:LinkButton Text="Esqueci minha senha" OnClientClick="return lembrete_senha();" runat="server" />
                        </label>
                    </div>

                    <asp:Button ID="btnEnter" CssClass="btn btn-lg btn-primary btn-block" OnClick="btnEnter_Click" runat="server" Text="Entrar" />

                </div>

            </div>


        </div>

        <div class="copy-footer">
            <div class="small">&copy; <%= DateTime.Now.Year %> ENI Portal  | powered by <a target="_blank" href="http://www.enisoft.com.br">enisoft</a></div>

        </div>

    </form>

    

    <script>
        function lembrete_senha() {

            var email = document.getElementById('txtLogin');
            var valido = true;

            if (email.value == "") {
                swal.fire('Digite o email', '', 'error');
                valido = false;
            }

            return valido;
        }
    </script>



</body>

</html>
