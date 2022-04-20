appModule.controller('TransferenciaAssinaturaController', function ($scope, formHandlerService, $http, conversionService, cepService, $sce) {

    $scope.filtro = {};

    $scope.carregarTela = function () {

        var data = new Date();
        var now = new Date();
        $scope.filtro.dataini = new Date(now.getFullYear(), now.getMonth(), 1);
        $scope.filtro.datafim = new Date(now.getFullYear(), now.getMonth(), now.getDate());

        $scope.filtro.mes = data.getMonth() + 1;
        $scope.filtro.ano = data.getFullYear();

    }
    $scope.dataAtualFormatada = function () {

        var parseDate = new Date();
        var jsDate = new Date(parseDate);

        var data = jsDate;
        var dia = data.getDate();
        if (dia.toString().length == 1)
            dia = "0" + dia;
        var mes = data.getMonth() + 1;
        if (mes.toString().length == 1)
            mes = "0" + mes;
        var ano = data.getFullYear();

        $scope.filtro.dataatual = dia + "/" + mes + "/" + ano;
    }

    $scope.listar = function (pageRequest) {
        
        $scope.dataAtualFormatada();

        showAjaxLoader2();

        var url = "/RelTransferenciaAssinatura/Pesquisar";
        $http({
            url: url,
            method: "post",
            data: { _mes: $scope.filtro.mes, _ano: $scope.filtro.ano}
        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success) {

                $scope.listaTransferencias = response.result.listaTransferencias;

            }
            else {

                $scope.listaTransferencias = null;

                alert(response.message.message);
            }

        }).error(function (response) {

            alert(response);

            hideAjaxLoader2();
        })

    }

});
