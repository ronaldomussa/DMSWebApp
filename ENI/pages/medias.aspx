<%@ Page Title="Midias" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="medias.aspx.cs" Inherits="ENI.pages.medias" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/node_modules/bootstrap-select/dist/css/bootstrap-select.min.css" rel="stylesheet" />
    <link href="/node_modules/bootstrap-datepicker/dist/css/bootstrap-datepicker.min.css" rel="stylesheet" />
    <link href="/node_modules/bootstrap-datepicker/dist/css/bootstrap-datepicker3.min.css" rel="stylesheet" />

    <link href="//amp.azure.net/libs/amp/latest/skins/amp-default/azuremediaplayer.min.css" rel="stylesheet" />
    <script src="//amp.azure.net/libs/amp/latest/azuremediaplayer.min.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">

     <div id="page" ng-controller="MediasCtrl as ctrl">

         <%--<h3 class="page-title d-print-none">Mídias</h3>--%>

         <div id="aba" class="navmenu-tabs d-print-none">
             <ul class="nav nav-tabs ml-3">
                 <li class="nav-item active">
                     <button type="button" class="nav-link" ng-click="ctrl.crud.list()"><i class="fas fa-bars mr-2"></i>Mídias</button>
                 </li>
                 <li class="nav-item">
                     <button type="button" class="nav-link" ng-click="ctrl.crud.add()" data-toggle="modal" data-target="#modal"><i class="fas fa-plus mr-2"></i>Nova</button>
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
                         
                         <div class="pt-3" style="height: 70px">
                             <h5 class="card-title text-center mb-0">{{item.name}}</h5>
                         </div>

                         <div class="card-footer text-center">
                             <a href="#" class="card-link smaller text-muted" data-toggle="collapse" data-target="#card_{{item.id}}" aria-expanded="false" aria-controls="collapseExample">mais detalhes</a>
                         </div>

                         <div class="collapse" id="card_{{item.id}}">

                             <table class="table table-hover mb-0" style="font-size: 0.8em">
                                 <tr>
                                     <th style="width: 50%">Data Inicio</th>
                                     <th class="border-left">Data Fim</th>
                                 </tr>
                                 <tr>
                                     <td>{{item.start_date | dateConvert}}</td>
                                     <td class="border-left">{{item.end_date | dateConvert}}</td>
                                 </tr>

                                 <tr>
                                     <th>Expor em</th>
                                     <th class="border-left">Mídia <span class="smaller font-weight-normal">{{item.media_type}}</span></th>
                                 </tr>
                                 <tr>
                                     <td>{{item.expose_at_all ? 'Todos os displays' : 'Selecionados'}}</td>
                                     <td class="border-left">
                                         <a ng-show="item.media_url" href="#modal_media_preview" class="btn btn-sm btn-default border btn-block" data-toggle="modal" data-target="#modal_media_preview"
                                             ng-click="ctrl.media_preview(item)">
                                             <span class="fas mr-1" ng-class="{'fa-film': (item.media_type_id == 1), 'fa-image': (item.media_type_id == 2)}"></span>
                                             Preview
                                         </a>

                                         <span ng-hide="item.media_url" class="smaller">*sem mídia</span>

                                     </td>
                                 </tr>
                             </table>

                         </div>

                         <div class="card-footer text-center">
                             <button type="button" class="btn btn-default btn-sm" ng-click="ctrl.crud.toggle_activation(item)">
                                 <i class="fas mr-1" ng-class="{'fa-toggle-on text-success' : item.is_active, 'fa-toggle-off text-muted' : !item.is_active,}"></i>
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
                                 <div class="col-md-12">
                                     <label>Nome</label>
                                     <input type="text" maxlength="30" ng-model="ctrl.selected_item.name" class="form-control" required />
                                 </div>
                             </div>

                             <div class="row mb-5">
                                 <div class="col-md-6">
                                     <label>Data Inicio</label>
                                     <input type="text" class="form-control datepicker" ng-model="ctrl.selected_item.start_date" required />
                                 </div>

                                 <div class="col-md-6">
                                     <label>Data Fim</label>
                                     <input type="text" class="form-control datepicker" ng-model="ctrl.selected_item.end_date" required />
                                 </div>
                             </div>

                             <div class="row mb-3 pt-5 border-top">
                                 <div class="col-md-3">
                                     <label>Tipo de Mídia</label>
                                     <select class="form-control" ng-model="ctrl.selected_item.media_type_id" required>
                                         <option value="1">Video</option>
                                         <option value="2">Imagem</option>
                                     </select>
                                 </div>

                                 <div class="col-md-3">
                                     <label>Tempo de exibição (s)</label>
                                     <input type="number" maxlength="2"
                                         ng-model="ctrl.selected_item.expose_timing"
                                         ng-disabled="(ctrl.selected_item.media_type_id == '1')" class="form-control" required />
                                 </div>

                                 <div class="col-md-6">
                                     <label>Mídia</label>

                                     <div id="file_uploaded" ng-show="ctrl.selected_item.media_url">
                                         <a href="#modal_media_preview" class="btn btn-default border" data-toggle="modal" data-target="#modal_media_preview"
                                             ng-click="ctrl.media_preview(ctrl.selected_item)">
                                             <i class="fas mr-1" 
                                                 ng-class="{'fa-film': (ctrl.selected_item.media_type_id == 1), 'fa-image': (ctrl.selected_item.media_type_id == 2)}"></i>
                                             Preview <span class="smaller">({{ctrl.selected_item.media_type}})</span>
                                         </a>

                                         <a href="#" class="btn btn-default" ng-click="ctrl.crud.remove_media()" title="Remover arquivo">
                                             <i class="fas fa-trash fa-wm text-danger"></i>
                                         </a>
                                     </div>

                                     <div id="file_upload" ng-hide="ctrl.selected_item.media_url">
                                         <div ng-hide="files[0].name" class="custom-file">
                                             <button type="button" id="fileUpoadButton"
                                                 ngf-select="uploadFiles($files, $invalidFiles)"
                                                 accept=".mp4, .jpg, .jpeg, .png"
                                                 ngf-max-size="20MB">
                                                 Carregue a midia .jpg ou .mp4</button>

                                             <label class="custom-file-label text-truncate" for="fileUpoadButton">
                                                 {{errFiles[0].name || "Carregue a midia .jpg ou .mp4"}}
                                             </label>
                                         </div>

                                         <div class="input-group mb-3" ng-show="files[0].name">
                                             <div class="input-group-prepend">
                                                 <span class="input-group-text">
                                                     <i class="fas"
                                                     ng-class="{'fa-film': (ctrl.selected_item.media_type_id == 1), 'fa-image': (ctrl.selected_item.media_type_id == 2)}">
                                                     </i>
                                                 </span>
                                             </div>
                                             <input type="text" readonly class="form-control" value="{{files[0].name}}" />
                                             <div class="input-group-append">
                                                 <button class="btn btn-default border" ng-click="ctrl.remove_temp_media();" type="button">
                                                     <i class="fas fa-trash fa-wm text-danger"></i>
                                                 </button>
                                             </div>
                                         </div>

                                         <div class="progress mt-2" ng-show="files.progress >= 0">
                                             <div class="progress-bar bg-primary"
                                                 ng-style="{'width': files.progress + '%'}"
                                                 ng-bind="files.progress + '%' ">
                                             </div>
                                         </div>
                                     </div>
                                 </div>

                             </div>

                             <div class="row mb-3">
                                 <div class="col-md-3">
                                     <label>Limite de inserções</label>
                                     <input type="number" maxlength="7" class="form-control" ng-model="ctrl.selected_item.insertions_limit" />
                                 </div>

                                 <div class="col-md-3">
                                     <label>Disponibilidade</label>
                                     <div class="form-check">
                                         <input type="checkbox" class="form-check-input" id="checkDisponibilidade" ng-model="ctrl.selected_item.is_active" checked="checked" />
                                         <label class="form-check-label" for="checkDisponibilidade">Ativo</label>
                                     </div>
                                 </div>

                                 <div class="col-md-6">
                                     <label>Exibir em</label>

                                     <div class="form-inline">
                                         <div class="form-check mr-3">
                                             <input type="radio" name="expose_at" class="form-check-input" id="radioExibirEmTodos"
                                                 ng-value="true"
                                                 ng-model="ctrl.selected_item.expose_at_all"
                                                 ng-change="ctrl.refresh_select()" />
                                             <label class="form-check-label" for="radioExibirEmTodos">Todos os displays</label>
                                         </div>

                                         <div class="form-check">
                                             <input type="radio" name="expose_at" class="form-check-input" id="radioExibirEmSelecionados"
                                                 ng-value="false"
                                                 ng-model="ctrl.selected_item.expose_at_all"
                                                 ng-change="ctrl.refresh_select()" />
                                             <label class="form-check-label" for="radioExibirEmSelecionados">Selecionados</label>
                                         </div>
                                     </div>
                                 </div>
                             </div>

                             <div class="row mb-3" ng-hide="ctrl.selected_item.expose_at_all">
                                 <div class="col-md-12">
                                     <label>Selecione os display</label>

                                     <select id="cbo_expose_display_list" class="form-control border"
                                         
                                         ng-model="ctrl.selected_item.expose_in"
                                         <%--ng-change="ctrl.select()"--%>
                                         data-live-search="true" data-size="10" data-style="btn-white" data-actions-box="true" multiple>

                                         <option
                                             ng-repeat="item in ctrl.display_list"
                                             value="{{item.id}}"
                                             data-content="<span class='badge badge-secondary'>{{item.name}}</span>">{{item.name}}

                                         </option>
                                     </select>

                                 </div>
                             </div>
                         </div>

                     </div>

                     <div class="modal-footer">

                         <button type="button" class="btn btn-secondary btn-lg" ng-click="ctrl.crud.list()" data-dismiss="modal">Cancelar</button>

                         <button type="button" class="btn btn-success btn-lg"
                             ng-hide="ctrl.selected_item.id"
                             ng-disabled="ctrl.save_loading || ctrl.upload_loading"
                             ng-click="ctrl.crud.save()">

                             <i class="fa fa-spinner fa-spin mr-2" ng-show="ctrl.save_loading"></i>
                             {{ctrl.save_loading ? 'Salvando ..': (ctrl.upload_loading ? 'Salvando midia ..' : 'Inserir')}}
                         </button>

                         <button type="button" class="btn btn-primary btn-lg"
                             ng-show="ctrl.selected_item.id"
                             ng-disabled="ctrl.save_loading || ctrl.upload_loading"
                             ng-click="ctrl.crud.save_edit()">

                             <i class="fa fa-spinner fa-spin mr-2" ng-show="ctrl.save_loading"></i>
                             {{ctrl.save_loading ? 'Salvando ..': (ctrl.upload_loading ? 'Salvando midia ..' : 'Salvar')}}
                         </button>

                     </div>
                 </div>
             </div>
         </div>

         <div class="modal" id="modal_media_preview" tabindex="-1" role="dialog" aria-hidden="true">
             <div class="modal-dialog  modal-dialog-centered modal-lg" role="document">
                 <div class="modal-content">
                     <div class="modal-header">
                         <h5 class="modal-title">Midia Preview</h5>
                         <button type="button" class="close" data-dismiss="modal" aria-label="Fechar">
                             <span aria-hidden="true">&times;</span>
                         </button>
                     </div>

                     <div class="modal-body p-5">
                         
                         <img class="img-fluid" ng-show="(ctrl.selected_item.media_type_id == 2)" ng-src="{{ctrl.selected_item.media_url}}" alt="" />

                         <video id="azuremediaplayer" ng-show="(ctrl.selected_item.media_type_id == 1)" 
                             class="azuremediaplayer amp-default-skin amp-big-play-centered" controls muted loop width="100%" height="450" poster="" data-setup='{}' tabindex="0">
                             <source src="{{ctrl.selected_item.media_url}}" type="video/mp4" />
                             <p class="amp-no-js">To view this video please enable JavaScript, and consider upgrading to a web browser that supports HTML5 video</p>
                         </video>

                     </div>

                     <div class="modal-footer">
                         <button type="button" class="btn btn-secondary btn-lg" data-dismiss="modal">Fechar</button>
                     </div>
                 </div>
             </div>
         </div>

    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="After" runat="server">

    <script src="/node_modules/ng-file-upload/dist/ng-file-upload.min.js"></script>
    <script src="/node_modules/bootstrap-select/dist/js/bootstrap-select.min.js"></script>
    <script src="/node_modules/bootstrap-datepicker/dist/js/bootstrap-datepicker.min.js"></script>
    <script src="/node_modules/bootstrap-datepicker/dist/locales/bootstrap-datepicker.pt-BR.min.js"></script>
    <script src="/angular/medias.controller.js"></script>

        <script type="text/javascript">

            app.value('logged_user', <%= (Newtonsoft.Json.JsonConvert.SerializeObject((ENI.Classes.userSessionDTO)Session[ENI.Constants.LoginSession.LOGIN_SESSION])) %>);

            $(function () {
                $('#expose_list').selectpicker();
                $('.datepicker').datepicker({
                    format: 'dd/mm/yyyy',
                    language: 'pt-BR'
                });
            });

        </script>
    
</asp:Content>
