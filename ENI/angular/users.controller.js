(function () {

    'use strict';

    angular
        .module('app', [])
        .filter("dateConvert", function () {

            var re = /\/Date\(([0-9]*)\)\//;

            return function (x) {
                if (x === undefined)
                    return;

                var m = x.match(re);
                if (m) {
                    var d = new Date(parseInt(m[1]));
                    return d.toUTCString();
                }
                else return null;
            };

        })
        .filter("role", function () {

            const output = {
                1: 'PADRÃO',
                2: 'ADMIN'
            };

            return function (s) {
                return output[s];
            }

        })
        .directive('bsTooltip', function () {
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
        })
        .controller('UsersCtrl', UsersCtrl);

    UsersCtrl.$inject = ['$http', '$filter'];

    function UsersCtrl($http, $filter) {

        var vm = this;

        vm.data = [];
        vm.selected_item = {};
        vm.tab_name = "";
        vm.list_loading = false;
        vm.save_loading = false;

        vm.crud = {

            edit: function (item) {

               if (item)
                    angular.copy(item, vm.selected_item);
                
                vm.tab_name = "Editar Usuário";


                //console
                console.log('edit item', vm.selected_item);
            },

            list: function () {
                reset();
            },

            add: function () {
                reset();
                vm.tab_name = "Novo Usuário";
            },

            save: function () {
                vm.save_loading = true;
                //console.log('crud save selected_item', vm.selected_item);

                if (!is_valid_form()) {
                    //console.log('is_save_valid: false');
                    vm.save_loading = false;                    
                    return;
                }

                $http({
                    method: 'POST',
                    url: '/controller/users.asmx/insert',
                    data: vm.selected_item
                }).then(function (resposta) {

                    vm.save_loading = false;

                    Swal.fire('', resposta.data.d, 'success');

                    let new_item = {};
                    angular.copy(vm.selected_item, new_item);
                    new_item.is_new = true;
                    vm.data.push(new_item);
                    
                    $('#modal').modal('hide');
                    reset();
                    
                    //console.log('resposta', resposta);

                }, function (resposta) {

                    vm.save_loading = false;

                    let message = "Verifique os campos e tente novamente";
                    if (resposta.data.d)
                        message = resposta.data.d;

                    Swal.fire('Algo errado.', message, 'error');
                    //console.log('resposta erro', resposta);

                });

            },

            save_edit: function () {
                vm.save_loading = true;
                console.log('crud editing selected_item', vm.selected_item);

                if (!is_valid_form()) {                    
                    vm.save_loading = false;                    
                    return;
                }

                $http({
                    method: 'POST',
                    url: '/controller/users.asmx/edit',
                    data: vm.selected_item
                }).then(function (resposta) {

                    Swal.fire('', resposta.data.d, 'success');
                    vm.selected_item.is_new = true;
                    reset();

                    $('#modal').modal('hide');
                    vm.save_loading = false;
                    console.log('resposta', resposta);

                }, function (resposta) {

                    vm.save_loading = false;

                    let message = "Verifique os campos e tente novamente";
                    if (resposta.data.d)
                        message = resposta.data.d;

                    Swal.fire('Algo errado.', message, 'error');
                    //console
                    console.log('resposta erro', resposta);

                });

            },

            remove: function (item) {

                $http({
                    method: 'POST',
                    url: '/controller/users.asmx/remove',
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
                    
                    console.log('users.asmx/remove', resposta);

                }, function (resposta) {

                    vm.save_loading = false;

                    let message = "Algo inesperado";
                    if (resposta.data.d)
                        message = resposta.data.d;

                    Swal.fire('Algo errado.', message, 'error');
                    //console
                    console.log('users.asmx/remove erro', resposta);

                });

                
            },

            toggle_activation: function (item) {

                item.is_active = !item.is_active;
                console.log('ToggleActivation item.', item);
                $http({
                    method: 'POST',
                    url: '/controller/users.asmx/ToggleActivation',
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
            }

        };

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

        function is_valid_form()
        {
            let is_valid = true;
            let message;

            if (!vm.selected_item.name)
                message = "Digite um nome válido";

            else if (!vm.selected_item.email)
                message = "Digite um email válido";

            else if (!vm.selected_item.password)
                message = "Digite uma senha válida";
            
            else if (!vm.selected_item.user_role_id)
                message = "Selecione um perfil válido";

            if (message) {
                is_valid = false;
                Swal.fire('Algo errado.', message, 'error');
            }

            console.log('is_form_validation message', vm.selected_item);

            return is_valid;
        }

        function reset() {

            vm.selected_item = {
                id: null,
                name: '',
                email: '',
                phone: '',
                password: '',
                user_role_id: '',
                is_new: false
            };

            vm.list_loading = false;
            vm.save_loading = false;
            vm.tab_name = "";

            //console
            console.log('Resetando campos');

        }

        function list() {

            vm.list_loading = true;

            $http({
                method: 'POST',
                url: '/controller/users.asmx/list',
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

        list();

    }

})();