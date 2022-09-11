app = angular.module('app', ['ngFileUpload']);

app.filter("dateConvert", function () {

    var re = /\/Date\(([0-9]*)\)\//;

    return function (x) {
        if (x === undefined)
            return;
        const options = { weekday: 'long', year: 'numeric', month: 'numeric', day: 'numeric' };

        var m = x.match(re);
        if (m) {
            var d = new Date(parseInt(m[1]));
            return d.toLocaleDateString('pt-BR', options);
        }
        else return null;
    };

});

app.filter("trustUrl", ['$sce', function ($sce) {
    return function (recordingUrl) {
        console.log('trustUrl', recordingUrl);
        return $sce.trustAsResourceUrl(recordingUrl);
    };
}]);

app.directive('bsTooltip', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            $(element).hover(function () {
                // on mouseenter
                $(element).tooltip('show');
            }, function () {
                // on mouseleave
                $(element).tooltip('hide');
            });
        }
    };
});

app.controller('MediasCtrl', ['$http', '$filter', 'logged_user', '$scope', 'Upload', '$timeout', MediasCtrl]);

function MediasCtrl($http, $filter, logged_user, $scope, Upload, $timeout) {

    var vm = this;

    vm.logged_user = logged_user;
    vm.data = [];
    vm.display_list = [];
    vm.selected_item = {};
    vm.tab_name = "";
    vm.list_loading = false;
    vm.save_loading = false;
    vm.upload_loading = false;

    $scope.files = {};
    $scope.errFiles = {};

    const API_URL = '/controller/medias.asmx';

    vm.tipo_display = {
       
        1: 'Paisagem',
        2: 'Retrato',
        3: 'Livre'
    };

    vm.crud = {

        list: function () {
            reset();
        },

        add: function () {
            reset();
            vm.tab_name = "Nova Mídia";
        },

        save: function () {
            vm.save_loading = true;
            console.log('crud save selected_item', vm.selected_item);

            if (!is_valid_form()) {
                vm.save_loading = false;
                return;
            }


            if (!vm.selected_item.expose_at_all)
                vm.selected_item.expose_in = vm.selected_item.expose_in.toString();
            else
                vm.selected_item.expose_in = '';

            vm.selected_item.is_new = true;

            media_upload();

        },

        edit: function (item) {

            angular.copy(item, vm.selected_item);

            vm.selected_item.media_type_id = vm.selected_item.media_type_id.toString();
            vm.selected_item.start_date = $filter('dateConvert')(vm.selected_item.start_date);
            vm.selected_item.end_date = $filter('dateConvert')(vm.selected_item.end_date);
            vm.selected_item.is_new = false;
            vm.selected_item.expose_in = vm.selected_item.expose_in.split(',');

            vm.refresh_select();

            vm.tab_name = "Editar Display";

            console.log('editing item:', vm.selected_item);
        },

        save_edit: function () {
            vm.save_loading = true;
            console.log('crud editing selected_item', vm.selected_item);

            if (!is_valid_form()) {
                vm.save_loading = false;
                return;
            }
            
            if (!vm.selected_item.expose_at_all)
                vm.selected_item.expose_in = vm.selected_item.expose_in.toString();
            else
                vm.selected_item.expose_in = '';
            
            vm.selected_item.is_new = false;

            media_upload();

        },

        remove: function (item) {

            Swal.fire({
                title: 'Tem certeza?',
                text: "Esta ação não poderá ser desfeita.",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#ff3300',
                cancelButtonColor: '#555',
                confirmButtonText: 'Sim, exclua este registro.'
            }).then((result) => {
                if (result.isConfirmed) {
                    remove_from_db(item);
                }
            })

        },

        toggle_activation: function (item) {

            item.is_active = !item.is_active;

            console.log('toggle_activate item.', item);

            $http({
                method: 'POST',
                url: API_URL + '/ToggleActivation',
                data: { id: item.id }
            }).then(function (resposta) {

                new Noty({
                    theme: 'sunset',
                    type: 'success',
                    layout: 'topRight',
                    timeout: 4000,
                    text: resposta.data.d
                }).show();

                console.log('ToggleActivation', resposta);


            }, function (resposta) {

                item.is_active = !item.is_active;

                let message = "Algo inesperado, tente novamente";
                if (resposta.data.d)
                    message = resposta.data.d;

                new Noty({
                    theme: 'sunset',
                    type: 'error',
                    timeout: 4000,
                    layout: 'topRight',
                    text: message
                }).show();

                //console.log('ToggleActivation error', resposta);

            });
        },

        remove_media: function () {

            vm.save_loading = true;

            Swal.fire({
                title: 'Tem certeza?',
                text: "Esta ação não poderá ser desfeita.",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#ff3300',
                cancelButtonColor: '#555',
                confirmButtonText: 'Sim, exclua esse arquivo.'
            }).then((result) => {

                if (result.isConfirmed) {

                    $http({
                        method: 'POST',
                        url: API_URL + '/RemoveMediaFromAzure',
                        data: { media_id: vm.selected_item.id }
                    }).then(function (resposta) {

                        vm.save_loading = false;
                        vm.remove_temp_media();

                        Swal.fire('', resposta.data.d, 'success');
                        console.log('edit_save', resposta);

                    }, function (resposta) {

                        vm.save_loading = false;

                        let message = "Verifique os campos e tente novamente";
                        if (resposta.data.d)
                            message = resposta.data.d;

                        Swal.fire('Algo errado.', message, 'error');
                        console.log('resposta erro', resposta);

                    });

                }
            })

        },

    };

    $scope.uploadFiles = function (files, errFiles) {

        $scope.files = files;
        $scope.errFiles = errFiles && errFiles[0];

    };

    vm.remove_temp_media = function () {
        $scope.files = {};
        $scope.errFiles = {};
        vm.selected_item.media_url = '';
        vm.selected_item.media_type = '';
    };

    function media_upload() {       

        if ($scope.files && $scope.files.length) {

            vm.save_loading = false;
            vm.upload_loading = true;

            $scope.files.upload = Upload.upload({
                url: 'controller/fileupload.ashx',
                data: { fileUploaded: $scope.files }
            });

            $scope.files.upload.then(
                function (response) {

                    $timeout(function () {

                        vm.upload_loading = false;
                        $scope.files.result = response.data;
                        vm.selected_item.media_url = response.data.media_url;
                        vm.selected_item.media_type = response.data.media_type;

                        new Noty({
                            theme: 'sunset',
                            type: 'success',
                            layout: 'topCenter',
                            timeout: 4000,
                            text: response.data.message
                        }).show();

                        media_save();

                        console.log('uploadOnAzure', response.data);
                    });

                }, function (response) {

                    vm.upload_loading = false;

                    if (response.status > 0)
                        $scope.errorMsg = response.status + ': ' + response.data;

                    new Noty({
                        theme: 'sunset',
                        type: 'error',
                        layout: 'topRight',
                        timeout: 4000,
                        text: response.data.message
                    }).show();

                    media_save();

                    console.log('uploadOnAzure error', response);

                }, function (evt) {
                    $scope.files.progress = Math.min(100, parseInt(100.0 * evt.loaded / evt.total));
                }
            );
        }
        else
            media_save(); // save without media file

    }

    function media_save() {

        console.log('init media_save', vm.selected_item);

        vm.save_loading = true;

        let API_METHOD = '/insert';

        if (!vm.selected_item.is_new) // item is edit
            API_METHOD = '/edit';

        $http({
            method: 'POST',
            url: API_URL + API_METHOD,
            data: vm.selected_item
        }).then(function (response) {

            vm.save_loading = false;
            Swal.fire('', response.data.d, 'success');
            reset();
            list();
            $('#modal').modal('hide');

            //console.log('resposta', resposta);

        }, function (resposta) {

            vm.save_loading = false;

            let message = "Verifique os campos e tente novamente";
            if (resposta.data.d)
                message = resposta.data.d;

            Swal.fire('Algo errado.', message, 'error');
            //console.log('resposta erro', resposta);

        });
    }

    vm.media_preview = function (item) {
        angular.copy(item, vm.selected_item);
        console.log('media_preview', vm.selected_item);

        
    }

    function remove_from_db(item) {

        $http({
            method: 'POST',
            url: API_URL + '/remove',
            data: { id: item.id }
        }).then(function (resposta) {

            new Noty({
                theme: 'sunset',
                type: 'success',
                layout: 'topRight',
                timeout: 4000,
                text: resposta.data.d
            }).show();

            remove_from_array(item);

            console.log('displays.asmx/remove', resposta);

        }, function (resposta) {

            vm.save_loading = false;

            let message = "Algo inesperado";
            if (resposta.data.d)
                message = resposta.data.d;

            Swal.fire('Algo errado.', message, 'error');

            console.log('display.asmx/remove erro', resposta);
        });

    }

    function remove_from_array(item) {

        console.log('ENTROU do for remove_from_array');

        for (let i = 0; i < vm.data.length; i++) {

            console.log('verificando se ' + vm.data[i].id + ' é igual a ' + item.id);

            if (vm.data[i].id === item.id) {
                vm.data.splice(i, 1);
                return;
            }
        }

        console.log('SAIU do for remove_from_array');

    }

    function is_valid_form() {
        let is_valid = true;
        let message;

        if (!vm.selected_item.name)
            message = "Digite um Nome válido";

        else if (!vm.selected_item.start_date)
            message = "Digite uma Data de inicio válida";

        else if (!vm.selected_item.end_date)
            message = "Digite uma Data de fim válida";

        else if (vm.selected_item.media_type_id == 2) {
            if (!vm.selected_item.expose_timing)
                message = "Digite tempo de exibição válido";
        }

        //else if (!$scope.files[0].name) {
        //    console.log('valid $scope.files', $scope.files);
        //    message = "Carregue um arquivo de midia (.jpg ou .mp4)";
        //}

        else if (!vm.selected_item.expose_at_all) {
            if (!vm.selected_item.expose_in.length)
                message = "Selecine os Displays para exibir a Midia";
        }

        if (message) {
            is_valid = false;
            Swal.fire('Algo errado.', message, 'error');
        }

        console.log('media validation', vm.selected_item);

        return is_valid;
    }

    function reset() {

        let today = new Date();
        let date1 = today.getDate() + '/' + (today.getMonth() + 1) + '/' + today.getFullYear();
        let date2 = (today.getDate() + 1) + '/' + (today.getMonth() + 1) + '/' + today.getFullYear();

        vm.selected_item = {
            id: null,
            name: '',
            media_type_id: '1',
            media_type: '',
            media_url: '',
            start_date: date1,
            end_date: date2,
            insertions_limit: null,
            expose_timing: '',
            expose_at_all: true,
            expose_in: '',
            expose_in_groups: '',
            is_active: true,
            is_new: false
        };

        $scope.files = {};
        $scope.errFiles = {};

        vm.list_loading = false;
        vm.save_loading = false;
        vm.tab_name = "";

        vm.refresh_select();

        //console
        console.log('Reseting fields');

    }

    function list() {

        vm.list_loading = true;

        $http({
            method: 'POST',
            url: API_URL + '/list',
            data: {}
        }).then(function (resposta) {

            vm.list_loading = false;
            vm.data = resposta.data.d;

            //console
            console.log('data array', vm.data);

        }, function (resposta) {

            vm.list_loading = false;

            //console
            console.log('erro', resposta);

        });

    }

    function display_list() {

        $http({
            method: 'POST',
            url: 'controller/displays.asmx/list',
            data: {}
        }).then(function (resposta) {

            vm.display_list = resposta.data.d;
            vm.refresh_select();
            //console
            console.log('display_list', resposta);

        }, function (resposta) {

            //console
            console.log('erro display_list', resposta);

        });

    }

    vm.refresh_select = function () {
        //console.log('refresh_select', vm.selected_item.expose_at_all);
        setTimeout(refresh, 300, 'cbo_expose_display_list');
    };

    vm.select = function () {
        console.log('vm.selec', vm.selected_item.expose_in);
    }

    function refresh() {
        $('#cbo_expose_display_list').selectpicker('refresh');
    }

    function init() {
        list();
        display_list()
    }

    init();

}
