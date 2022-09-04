<%@ Page Title="Usuários do Sistema" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="users.aspx.cs" Inherits="ENI.pages.users" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">

    <div id="page" ng-controller="UsersCtrl as ctrl">
        
        <h3 class="page-title d-print-none">Usuários do sistema</h3>

        <div id="aba" class="navmenu-tabs d-print-none">
            <ul class="nav nav-tabs ml-3">
                <li class="nav-item active">
                    <button type="button" class="nav-link" ng-click="ctrl.crud.list()"><i class="fas fa-bars mr-2"></i>Usuários</button>
                </li>
                <li class="nav-item">
                    <button type="button" class="nav-link" ng-click="ctrl.crud.add()" data-toggle="modal" data-target="#modal"><i class="fas fa-plus mr-2"></i>Novo</button>
                </li>
            </ul>
        </div>

        <div class="container-fluid" ng-cloak>

            <div id="tab_lista">

                <div class="loading text-center" ng-show="ctrl.list_loading">
                    <i class="fas fa-spinner fa-pulse fa-fw mr-2"></i>carregando..
                </div>

                <div class="p-4 text-center" ng-show="!ctrl.data.length && !ctrl.list_loading">
                    <p>Nenhum registro encontrado.</p>
                </div>
                
                <div id="tabela" class="container-mw">
                    <div class="table-responsive">
                        <table class="table-custom d-print-table" ng-hide="ctrl.list_loading || !ctrl.data.length">
                            <thead>
                                <tr>
                                        
                                    <th>Nome</th>
                                    <th>Email</th>
                                    <th>Perfil</th>
                                    <th>Criado em</th>
                                    <th class="d-print-none" style="width: 135px"></th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr ng-repeat="item in ctrl.data" ng-class="{'text-muted' : !item.is_active}">
                                        
                                    <td><span ng-show="(item.id == ctrl.logged_user.id)" class="badge badge-primary mr-2">Você</span>{{item.name}} {{logged_user.id}}</td>
                                    <td>{{item.email}}</td>
                                    <td><span class="small">{{(item.user_role_id | role)}}</span></td>
                                    <td>{{(item.created_date | dateConvert)}} <span ng-show="item.is_new" class="badge badge-warning">novo</span></td>
                                    <td class="td-buttons d-print-none text-right">
                                        <div ng-hide="item.is_new">
                                            <button type="button" class="btn btn-default btn-sm"  data-toggle="modal" data-target="#modal" ng-click="ctrl.crud.edit(item)" title="Editar" bs-tooltip>
                                                <i class="fas fa-pencil-alt"></i></button>

                                            <button type="button" class="btn btn-default btn-sm" ng-click="ctrl.crud.toggle_activation(item)" title="{{item.is_active ? 'Inativar':'Ativar'}}" bs-tooltip>
                                                <i class="fas" ng-class="{'fa-toggle-on text-success' : item.is_active, 'fa-toggle-off' : !item.is_active}"></i></button>    
                                        
                                            <button type="button" class="btn btn-default btn-sm" ng-click="ctrl.crud.remove(item)">
                                                <i class="fas fa-trash"></i></button>
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>                    

            </div>

        </div>


        <div class="modal" id="modal" tabindex="-1" role="dialog" aria-hidden="true">
            <div class="modal-dialog  modal-dialog-centered modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title">{{ctrl.tab_name}}</h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Fechar">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body p-5">

                        <div class="row mb-3">
                            <div class="col-md-6">
                                <label>Nome</label>
                                <input type="text" id="txtName" autocomplete="off" maxlength="30" ng-model="ctrl.selected_item.name" class="form-control" required />
                            </div>

                            <div class="col-md-6">
                                <label>Celular (ddd+numero)</label>
                                <input type="tel" id="txtPhone" autocomplete="off" maxlength="20" ng-model="ctrl.selected_item.phone" class="form-control" />
                            </div>
                        </div>

                        <div class="row mb-4">
                            <div class="col-md-6">
                                <label>Email</label>
                                <input type="email" id="txtEmail" autocomplete="off" maxlength="150" ng-model="ctrl.selected_item.email" class="form-control" required />
                            </div>

                            <div class="col-md-6">
                                <label>Senha</label>
                                <input type="password" id="txtPassowrd" autocomplete="off" maxlength="45" ng-model="ctrl.selected_item.password" class="form-control" required />
                            </div>
                        </div>

                        <div class="row mb-4">
                            <div class="col-md-6">
                                <label>Perfil</label>
                                <select class="form-control" ng-model="ctrl.selected_item.user_role_id" required>
                                    <option value="">Selecione</option>
                                    <option ng-value="1">Padrão</option>
                                    <option ng-value="2">Admin</option>
                                </select>
                                <%--<asp:DropDownList ID="cboProfile" CssClass="form-control" runat="server" required></asp:DropDownList>--%>
                            </div>
                        </div>
                            
                    </div>
                    <div class="modal-footer">

                        <button type="button" class="btn btn-secondary btn-lg" ng-click="ctrl.crud.list()" data-dismiss="modal">Cancelar</button>
                        
                        <button type="button" class="btn btn-success btn-lg" ng-hide="ctrl.selected_item.id" ng-disabled="ctrl.save_loading" ng-click="ctrl.crud.save()">
                            <i class="fa fa-spinner fa-spin mr-2" ng-show="ctrl.save_loading"></i>
                            {{ctrl.save_loading ? 'Salvando ..':'Inserir'}}
                        </button>

                        <button type="button" class="btn btn-primary btn-lg" ng-show="ctrl.selected_item.id" ng-disabled="ctrl.save_loading" ng-click="ctrl.crud.save_edit()">
                            <i class="fa fa-spinner fa-spin mr-2" ng-show="ctrl.save_loading"></i>
                            {{ctrl.save_loading ? 'Salvando ..':'Salvar'}}
                        </button>

                    </div>
                </div>
            </div>
        </div>

    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="After" runat="server">
    <script type="text/javascript">
        app.value('logged_user', <%= (Newtonsoft.Json.JsonConvert.SerializeObject((ENI.Classes.userSessionDTO)Session[ENI.Constants.LoginSession.LOGIN_SESSION])) %>);
    </script>
    <script src="/angular/users.controller.js"></script>
</asp:Content>
