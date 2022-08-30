function hoje() {
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!
    var yyyy = today.getFullYear();

    var sDate = (dd + '/' + mm + '/' + yyyy);
    //console.log(sDate);
    return sDate;
}

var dias = [
    "Domingo",
    "Segunda",
    "Terça",
    "Quarta",
    "Quinta",
    "Sexta",
    "Sábado"
];

var meses = [
    "Janeiro",
    "Fevereiro",
    "Março",
    "Abril",
    "Maio",
    "Junho",
    "Julho",
    "Agosto",
    "Setembro",
    "Outubro",
    "Novembro",
    "Dezembro"
];


$(document).ready(function () {

    $("#btnSidenavebarToggle").click(function (e) {
        e.preventDefault();
        $("body").toggleClass("sidebar-toggled");
        $("#sidebar").toggleClass("d-none");
    });

});
