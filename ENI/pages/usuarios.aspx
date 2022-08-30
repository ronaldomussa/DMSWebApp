<%@ Page Title="Usuários do Sistema" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="usuarios.aspx.cs" Inherits="ENI.pages.usuarios" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">

    <div id="page_usuarios" ng-controller="UsuariosCtrl as ctrl">
        
        <h3 class="page-title d-print-none">Usuários do sistema</h3>

        <div id="aba" class="navmenu-tabs d-print-none">
            <ul class="nav nav-tabs ml-3">
                <li class="nav-item" ng-class="{active : ctrl.tab == 1}">
                    <button type="button" class="nav-link" ng-click="ctrl.crud.list()"><i class="fas fa-bars mr-2"></i>Lista</button>
                </li>
                <li class="nav-item" ng-class="{active : ctrl.tab == 2}">
                    <button type="button" class="nav-link" ng-click="ctrl.crud.add()"><i class="fas fa-plus mr-2"></i>Adicionar</button>
                </li>
            </ul>
        </div>

        <div class="container-fluid" ng-cloak>

            <div id="tab_lista" ng-show="(ctrl.tab == 1)">

                <div class="loading text-center" ng-show="ctrl.loading">
                    <i class="fa fa-clock fa-fw mr-2"></i>carregando..
                </div>

                <div class="p-4 text-center" ng-show="!ctrl.data.length">
                    <p>Nenhum registro encontrado.</p>
                </div>

                
                <div id="tabela" class="container-mw">

                    <div class="table-responsive">
                        <table class="table-custom d-print-table" ng-hide="ctrl.loading || !ctrl.data.length">
                            <thead>
                                <tr>
                                        
                                    <th>Nome</th>
                                    <th>Email</th>
                                    <th>Permissão</th>
                                    <th class="d-print-none" style="width: 135px"></th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr ng-repeat="item in ctrl.data" ng-class="{'bg-inativo' : item.inativo}">
                                        
                                    <td>{{item.name}}</td>
                                    <td>{{item.email}}</td>
                                    <td><span class="small">{{item.group_name || 'Controle Total'}}</span></td>
                                    <td class="td-buttons d-print-none text-right">
                                        <button type="button" class="btn btn-default btn-sm" ng-click="ctrl.reset(item)" title="Resetar senha" bs-tooltip><i class="fa fa-key"></i></button>
                                        <button type="button" class="btn btn-default btn-sm" ng-click="ctrl.edit(item)" title="Editar" bs-tooltip><i class="fa fa-pencil-alt"></i></button>
                                        <button type="button" class="btn btn-default btn-sm" ng-click="ctrl.crud.remove(item)" title="{{item.is_active ? 'Inativar':'Ativar'}}"  bs-tooltip>
                                            <i class="fas" ng-class="{'fa-toggle-on text-success' : item.is_active, 'fa-toggle-off' : !item.is_active}"></i></button>                                         
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>                    

            </div>

            <div id="tab_add" class="container-mw form-custom" ng-show="(ctrl.tab == 2)">
                
                <div class="card mb-3">
                    <div class="card-body">

                        <div class="row mb-3">
                            <div class="col-md-6">
                                <label>Nome</label>
                                <input type="text" name="nome" id="txtNome" autocomplete="off" ClientIDMode="Static" maxlength="150" ng-model="ctrl.selected_item.nome" class="form-control" runat="server" required />
                            </div>

                            <div class="col-md-3">
                                <label>Celular (ddd+numero)</label>
                                <input type="text" name="phone" id="txtPhone" autocomplete="off" ClientIDMode="Static" maxlength="14" ng-model="ctrl.selected_item.celular" class="form-control" runat="server" required />
                            </div>
                        </div>

                        <div class="row mb-4">
                            <div class="col-md-6">
                                <label>Email</label>
                                <input type="email" id="txtEmail" autocomplete="off" ClientIDMode="Static" maxlength="150" ng-model="ctrl.selected_item.email" class="form-control" runat="server" required />
                            </div>

                            <div class="col-md-3">
                                <label>Senha</label>
                                <input type="password" id="txtPassowrd" autocomplete="off" ng-disabled="ctrl.selected_item.autopass" ClientIDMode="Static" maxlength="150" ng-model="ctrl.selected_item.senha" class="form-control" runat="server" required />
                            </div>

                            <div class="col-md-3">
                                <label>&nbsp;</label>
                                <div class="custom-control custom-switch pt-2">
                                    <input type="checkbox" id="chkAutoPassword" class="custom-control-input" ng-model="ctrl.selected_item.autopass" clientidmode="Static"  name="status" runat="server" />
                                    <label class="custom-control-label" for="chkAutoPassword">Gerar automatico</label>
                                </div>
                            </div>
                        </div>

                        <div class="row mb-4">
                            <div class="col-md-6">
                                <label>Perfil</label>
                                <asp:DropDownList ID="cboProfile" CssClass="form-control" runat="server"></asp:DropDownList>
                            </div>
                        </div>
                        
                    </div>
                </div>
                

                <div class="actions_box">

                    <input type="hidden" id="hfdItemId" value="{{ctrl.selected_item.id}}" runat="server" />
                    <button type="button" class="btn btn-secondary btn-lg" ng-click="ctrl.crud.list()">Cancelar</button>
                
                    <asp:Button ID="btnAdicionar" OnClick="btnAdicionar_Click" ng-show="ctrl.tab_name == 'Adicionar'" Text="Salvar novo" CssClass="btn btn-success btn-lg" OnClientClick="return validar();" runat="server" />
                    <asp:Button ID="btnEditar" OnClick="btnEditar_Click" ng-show="ctrl.tab_name == 'Editar'" Text="Salvar edição" CssClass="btn btn-success btn-lg" OnClientClick="return validar();" runat="server" />

                </div>
                
            </div>

        </div>

    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="After" runat="server">
    <script src="/angular/usuario.controller.js?v=1.0"></script>
    <script>

        var check = function (obj) {

            var current = obj.checked;
            $('.chk').checked = true;

        };

    </script>
</asp:Content>
