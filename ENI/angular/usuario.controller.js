(function () {

    'use strict';

    angular
        .module('app', [])
        .filter("dateConvert", function () {

            var re = /\/Date\(([0-9]*)\)\//;

            return function (x) {
                if (x == undefined)
                    return;

                var m = x.match(re);
                if (m) return new Date(parseInt(m[1]));
                else return null;
            };

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
        .controller('UsuariosCtrl', DespesaCtrl);

    DespesaCtrl.$inject = ['$http', '$filter'];

    function DespesaCtrl($http, $filter) {

        var vm = this;

        vm.permissao = {
            edit: true,
            remove: true
        };

        var ctrl_api_path = '/controller/users.asmx';

        vm.data = [];

        vm.selected_item = {};

        vm.filters = {
            inativo: null
        };

        vm.tab = 1;
        vm.tab_name = "";
        vm.loading = false;

        vm.crud = {

            view: function (item) {
                angular.copy(item, vm.selected_item);

                //console
                console.log('view item', item);
            },

            edit: function (item) {

                if (!vm.permissao.edit) {
                    sem_permissao();
                    return;
                }

                if (item)
                    angular.copy(item, vm.selected_item);

                vm.tab = 2;
                vm.tab_name = "Editar";

                //console
                console.log('edit item', vm.selected_item);
            },

            list: function () {
                vm.tab = 1;
                reset();
            },

            add: function () {
                vm.tab = 2;
                vm.tab_name = "Adicionar";
                reset();
            },

            remove: function (item) {

                if (!vm.permissao.remove) {
                    sem_permissao();
                    return;
                }

                item.is_active = !item.is_active;

                $http({
                    method: 'POST',
                    url: ctrl_api_path + '/Remover',
                    data: { id: item.id }
                }).then(function (resposta) {                    

                    new Noty({
                        theme: 'sunset',
                        type: 'success',
                        layout: 'topRight',
                        timeout: 4000,
                        text: resposta.data.d
                    }).show();

                    console.log('Remover', resposta);                   

                }, function (resposta) {

                    item.disabled = !item.disabled;

                    new Noty({
                        theme: 'sunset',
                        type: 'error',
                        timeout: 4000,
                        layout: 'topRight',
                        text: resposta.data.d
                    }).show();

                    console.log('Remover erro', resposta);

                });

            }

        };

        function sem_permissao() {
            Swal.fire('Sem permissão', 'Você não tem permissão para acessar este recurso. Contate o administrador.', 'info');
        }

        function reset() {

            vm.selected_item = {};
            //vm.data = [];
            vm.loading = false;

            //console
            console.log('Resetando campos');
        }

        function list() {

            vm.loading = true;

            $http({
                method: 'POST',
                url: ctrl_api_path + '/list',
                data: vm.filters
            }).then(function (resposta) {

                vm.loading = false;
                vm.data = resposta.data.d;

                //console
                console.log('data array', vm.data);

            }, function (resposta) {

                vm.loading = false;

                //console
                console.log('erro', resposta);

            });

        }

        list();

    }

})();