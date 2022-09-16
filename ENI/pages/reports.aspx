<%@ Page Title="Relatórios" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="reports.aspx.cs" Inherits="ENI.pages.reports" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/node_modules/bootstrap-datepicker/dist/css/bootstrap-datepicker.min.css" rel="stylesheet" />
    <link href="/node_modules/bootstrap-datepicker/dist/css/bootstrap-datepicker3.min.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">

    <div id="page" ng-controller="ReportsCtrl as ctrl">

         <%--<h3 class="page-title d-print-none">Relatórios</h3>--%>

         <div id="aba" class="navmenu-tabs d-print-none">
             <ul class="nav nav-tabs ml-3">
                 <li class="nav-item active">
                     <button type="button" class="nav-link" ><i class="fas fa-list mr-2"></i>Relatórios</button>
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
                         
                         <div class="card-header text-center bg-primary text-white">
                             <strong>{{ctrl.insertions_sum(item.insertions)}}</strong> inserções
                         </div>
                         <div class="pt-3 card-title" style="height: 70px">
                             <h5 class="text-center mb-0">{{item.media_name}}</h5>
                         </div>

                         <div class="card-footer text-center">
                            <a href="#" class="card-link smaller text-muted" data-toggle="collapse" data-target="#card_{{item.id}}" aria-expanded="false" aria-controls="collapseExample">Histórico</a>
                        </div>

                        <div class="collapse" id="card_{{item.id}}">

                            <table class="table table-sm text-center table-hover mb-0" style="font-size: 0.8em">
                                <tr>
                                    <th style="width: 25%">Inicio</th>
                                    <th style="width: 25%">Fim</th>
                                    <th>Inserções</th>
                                </tr>
                                <tr ng-repeat="i in item.insertions">
                                    <td>{{i.period_start_date | dateConvert}}</td>
                                    <td>{{i.period_end_date | dateConvert}}</td>
                                    <td>{{i.insertions_counted}}</td>
                                </tr>
                                <tr ng-hide="item.insertions.length">
                                    <td colspan="3" class="text-center">Nenhum</td>
                                </tr>
                            </table>

                        </div>

                         <div class="card-footer text-center">
                             {{item.display_name}}
                         </div>

                     </div>
                 </div>

             </div>


         </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="After" runat="server">
    <script src="/node_modules/bootstrap-datepicker/dist/js/bootstrap-datepicker.min.js"></script>
    <script src="/node_modules/bootstrap-datepicker/dist/locales/bootstrap-datepicker.pt-BR.min.js"></script>
    <script src="/angular/reports.controller.js"></script>

    <script type="text/javascript">

        app.value('logged_user', <%= (Newtonsoft.Json.JsonConvert.SerializeObject((ENI.Classes.userSessionDTO)Session[ENI.Constants.LoginSession.LOGIN_SESSION])) %>);

        $(function () {
            //$('#expose_list').selectpicker();
            //$('.datepicker').datepicker({
            //    format: 'dd/mm/yyyy',
            //    language: 'pt-BR'
            //});
        });

    </script>
</asp:Content>
