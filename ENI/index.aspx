
<%@ Page Title="Dashboard" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="ENI.Index" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="assets/css/_home.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    
    <h3 class="page-title">
        Olá <b> <asp:Literal ID="litNome" Text="" runat="server" /></b>!
    </h3>

    <div class="container-fluid pt-3">
        <div class="container-mw">
            
        </div>
    </div>
    
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="After" runat="server">

</asp:Content>
