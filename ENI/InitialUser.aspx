<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InitialUser.aspx.cs" Inherits="ENI.InitialUser" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>initial user</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <label>Nome</label>
            <asp:TextBox ID="Nome" runat="server"></asp:TextBox>
        </div>
        <div>
            <label>Email</label>
            <asp:TextBox ID="Email" runat="server"></asp:TextBox>
        </div>
        <div>
            <label>Phone</label>
            <asp:TextBox ID="Phone" runat="server"></asp:TextBox>
        </div>
        <div>
            <label>Password</label>
            <asp:TextBox ID="Password" runat="server"></asp:TextBox>
        </div>
        <div>
            <label>SuperUser</label>
            <asp:CheckBox ID="SuperUser" Checked="false" runat="server" />
        </div>
        <div>
            <label>Role</label>
            <asp:DropDownList ID="Role" runat="server"></asp:DropDownList>
        </div>
        <div>
            <asp:Button ID="Save" runat="server" Text="Salvar" OnClick="Save_Click" />
        </div>

    </form>
</body>
</html>
