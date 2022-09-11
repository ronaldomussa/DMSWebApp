<%@ Page Title="Displays" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="displays.aspx.cs" Inherits="ENI.pages.displays" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    
    <div id="page" ng-controller="DisplaysCtrl as ctrl">
        
        <%--<h3 class="page-title d-print-none">Displays e Totens</h3>--%>

        <div id="aba" class="navmenu-tabs d-print-none">
            <ul class="nav nav-tabs ml-3">
                <li class="nav-item active">
                    <button type="button" class="nav-link" ng-click="ctrl.crud.list()"><i class="fas fa-bars mr-2"></i>Displays</button>
                </li>
                <li class="nav-item">
                    <button type="button" class="nav-link" ng-click="ctrl.crud.add()" data-toggle="modal" data-target="#modal"><i class="fas fa-plus mr-2"></i>Novo</button>
                </li>
            </ul>
        </div>

        <div class="container-fluid" ng-cloak>

            <div class="form mb-3">
                <input class="form-control" type="text" name="filter" placeholder="Nome do display" value="" />
            </div>

            <div class="loading text-center" ng-show="ctrl.list_loading">
                <i class="fas fa-spinner fa-pulse fa-fw mr-2"></i>carregando..
            </div>

            <div class="p-4 text-center" ng-show="!ctrl.data.length && !ctrl.list_loading">
                <p>Nenhum registro encontrado.</p>
            </div>

            <div class="row">

                <div class="col-md-3 mb-4" ng-repeat="item in ctrl.data">
                    <div class="card">
                        <div class="card-header">
                            <div class="text-center">                                   
                                <span class="smaller text-danger mr-2" 
                                    data-toggle="tooltip" data-placement="top" title="Ultimo sinal: 08/09/2022 12:34" bs-tooltip>
                                    <i class="fas fa-wifi mr-2"></i>Sem conexão
                                </span>
                            </div>
                        </div>
                        <div class="card-body">
                            <h5 style="height: 50px" class="card-title text-center mb-0">{{item.name}}</h5>
                        </div>
                        <div class="card-footer text-center">
                            <a href="#" class="card-link smaller text-muted" data-toggle="collapse" data-target="#card_{{item.id}}" aria-expanded="false" aria-controls="collapseExample">mais detalhes</a>
                        </div>

                        <div class="collapse" id="card_{{item.id}}">

                            <table class="table table-hover mb-0" style="font-size: 0.8em">
                                <tr>
                                    <th colspan="2">Token</th>
                                </tr>
                                <tr>
                                    <td colspan="2">{{item.token}}</td>
                                </tr>
                                <tr>
                                    <th>Resolução</th>
                                    <th style="width: 50%" class="border-left">Tipo</th>
                                </tr>
                                <tr>
                                    <td>{{item.display_size}}</td>
                                    <td class="border-left">{{ctrl.tipo_display[item.orientation]}}</td>
                                </tr>

                                <tr>
                                    <th colspan="2">Localização</th>
                                </tr>
                                <tr>
                                    <td colspan="2">{{item.location}}</td>
                                </tr>

                            </table>

                        </div>

                        <div class="card-footer text-center">
                            <button type="button" class="btn btn-default btn-sm" ng-click="ctrl.crud.toggle_activation(item)">
                                <i class="fas mr-2" ng-class="{'fa-toggle-on text-success' : item.is_active, 'fa-toggle-off text-muted' : !item.is_active,}"></i>
                                {{(item.is_active ? 'Desativar' : 'Ativar')}}
                            </button>

                            <button type="button" class="btn btn-default btn-sm" data-toggle="modal" data-target="#modal" ng-click="ctrl.crud.edit(item)">
                                <i class="fas fa-pencil"></i>
                            </button>

                            <button type="button" class="btn btn-default btn-sm" ng-click="ctrl.crud.remove(item)">
                                <i class="fas fa-trash"></i>
                            </button>
                            
                        </div>

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

                        <div class="form-custom">
                            <div class="row mb-3">
                                <div class="col-md-6">
                                    <label>Nome</label>
                                    <input type="text" maxlength="30" ng-model="ctrl.selected_item.name" class="form-control" required />
                                </div>

                                <div class="col-md-6">
                                    <label>Token</label>
                                    <input type="text" maxlength="30" ng-model="ctrl.selected_item.token" class="form-control" required />
                                </div>
                            </div>

                            <div class="row mb-3">
                                <div class="col-md-6">
                                    <label>Orientação</label>
                                    <select class="form-control" ng-model="ctrl.selected_item.orientation" required>
                                        <option value="1">Paisagem</option>
                                        <option value="2">Retrato</option>
                                        <option value="3">Livre</option>
                                    </select>
                                </div>

                                <div class="col-md-6">
                                    <label>Dimensões</label>
                                    <input type="text" maxlength="15" ng-model="ctrl.selected_item.display_size" placeholder="1080x1920" class="form-control" required />
                                </div>
                            </div>

                            <div class="row mb-3">
                                <div class="col-md-6">
                                    <label>Horários</label>
                                    <input type="text" maxlength="15" disabled class="form-control" required />
                                </div>

                                <div class="col-md-6">
                                    <label>Disponibilidade</label>
                                    <div class="form-check">
                                        <input type="checkbox" class="form-check-input" id="checkDisponibilidade" ng-model="ctrl.selected_item.is_active" checked="checked" />
                                        <label class="form-check-label" for="checkDisponibilidade">Ativo</label>
                                    </div>
                                </div>
                            </div>

                            <div class="row mb-3">
                                <div class="col-md-12">
                                    <label>Endereço</label>
                                    <input type="text" maxlength="100" ng-model="ctrl.selected_item.location" placeholder="Rua, numero, bairro, cep" class="form-control" required />
                                </div>
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

    <script src="/angular/displays.controller.js"></script>

</asp:Content>
