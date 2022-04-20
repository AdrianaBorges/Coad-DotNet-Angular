appModule.controller('CadernoController', function ($scope, formHandlerService, $http, conversionService) {
    //$scope.filtro = {};

    $scope.RenomearCaderno = function () {
        var novoTitulo = $("#CAD_NOME").val();
        var id = $("#cadernoid").val();
        var url = "/Caderno/Renomear";
        $http({
            url: url,
            method: "post",
            data: { novoTitulo: novoTitulo, id: id }
        }).success(function (retorno) {
            $("#tituloCad").text(retorno.result.ntitulo);
        });
    };

    $scope.ExcluirCaderno = function (idcad) {
        $("#dialog-confirm").dialog({
            resizable: false,
            height: 140,
            modal: true,
            buttons: {
                "Deletar caderno": function () {
                    $(this).dialog("close");
                    var id = $("#cadernoid").val();
                    var url = "/Caderno/Excluir";
                    $http({
                        url: url,
                        method: "post",
                        data: { id: id }
                    }).success(function (retorno) {
                        window.location.replace("/Caderno");
                        //$("#tituloCad").text(retorno.result.ntitulo);
                    });
                },
                "Cancelar": function () {
                    $(this).dialog("close");
                }
            }
        });
        //alert(idcad);
    };
});