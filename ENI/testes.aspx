<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="testes.aspx.cs" Inherits="ENI.testes" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Teste</h2>
            <p>Teste de conexão com a database</p>
            <asp:Label ID="lbl" Text="" runat="server" />
            <asp:GridView ID="grid" runat="server"></asp:GridView>  
        </div>
    </form>
</body>
</html>
