app.filter("dateConvert", function () {

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

});

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

app.controller('DisplaysCtrl', ['$http', 'logged_user', DisplaysCtrl]);

function DisplaysCtrl($http, logged_user) {

    var vm = this;

    vm.logged_user = logged_user;
    vm.data = [];
    vm.selected_item = {};
    vm.tab_name = "";
    vm.list_loading = false;
    vm.save_loading = false;

    vm.tipo_display = {
       
        1: 'Paisagem',
        2: 'Retrato',
        3: 'Livre'
    };

    vm.crud = {

        edit: function (item) {

            vm.selected_item_edited = item;
            angular.copy(item, vm.selected_item);
            vm.selected_item.orientation = vm.selected_item.orientation.toString();

            vm.tab_name = "Editar Display";

            //console
            console.log('edit selected_item', vm.selected_item);
            console.log('edit selected_item_edited', vm.selected_item_edited);
        },

        list: function () {
            reset();
        },

        add: function () {
            reset();
            vm.tab_name = "Novo Display";
        },

        save: function () {
            vm.save_loading = true;
            console.log('crud save selected_item', vm.selected_item);

            if (!is_valid_form()) {
                //console.log('is_save_valid: false');
                vm.save_loading = false;
                return;
            }

            $http({
                method: 'POST',
                url: '/controller/displays.asmx/insert',
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
                url: '/controller/displays.asmx/edit',
                data: vm.selected_item
            }).then(function (resposta) {

                Swal.fire('', resposta.data.d, 'success');
                
                reset();
                list();

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
                url: '/controller/displays.asmx/ToggleActivation',
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

    function remove_from_db(item) {

        $http({
            method: 'POST',
            url: '/controller/displays.asmx/remove',
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
            message = "Digite um nome válido";

        if (message) {
            is_valid = false;
            Swal.fire('Algo errado.', message, 'error');
        }

        console.log('display validation message', vm.selected_item);

        return is_valid;
    }

    function reset() {

        vm.selected_item = {
            id: null,
            name: '',
            token: '',
            orientation: '2',
            display_size: '1080x1920',
            location: '',
            is_active: true,
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
            url: '/controller/displays.asmx/list',
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
