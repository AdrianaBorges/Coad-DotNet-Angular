appModule.controller('AcessoTabelasController', function ($scope, formHandlerService, $http, conversionService, cepService, $sce) {

    $scope.MES = {};
    $scope.TDC_ID = {};
    $scope.filtro = {};
    $scope.filtro.TIPO = 0;
    $scope.filtro.analitico = false;
    $scope.export = {};
    $scope.quebra = {};

    $scope.trustAsHtml = function (string) {
        return $sce.trustAsHtml(string);
    };

    $scope.ImpEtiqueta = function () {

        var url = Util.getUrl("/AcessoTabelas/Imprimir");

        post(url);

    }

    $scope.carregarTela = function () {

        var data = new Date();
        var now = new Date();
        $scope.filtro.dataini = new Date(now.getFullYear(), now.getMonth(), 1);
        $scope.filtro.datafim = new Date(now.getFullYear(), now.getMonth(), now.getDate());

        $scope.filtro.mes = data.getMonth() + 1;
        $scope.filtro.ano = data.getFullYear();

        $scope.buscarProdutos();

    }

    $scope.listarAcessos = function (pageRequest) {

        showAjaxLoader2();

        var url = "/AcessoTabelas/BuscarAcessoPorAssinatura";
        $http({
            url: url,
            method: "post",
            data: { _dataini: $scope.filtro.dataini, _datafim: $scope.filtro.datafim, _assinatura: $scope.filtro.assinatura }
        }).success(function (response) {

            
            if (response.success == true) {

                $scope.lstAcessoTabelas = response.result.lstAcessoTabelas;

            }
            else {

                $scope.lstAcessoTabelas = null;

                alert(response.message.message);
            }

            hideAjaxLoader2();

        }).error(function (response) {

            alert(response);

            hideAjaxLoader2();
        })

    }
    $scope.buscarProdutos = function () {

        showAjaxLoader2();

        var url = "/AcessoTabelas/BuscarProdutos";
        $http({
            url: url,
            method: "post"
            
        }).success(function (response) {


            if (response.success == true) {

                $scope.lstprodutos = response.result.lstprodutos;
                conversionService.deepConversion($scope.lstprodutos);

            }
            else {

                $scope.lstAcessoTabelas = null;

                alert(response.message.message);
            }

            hideAjaxLoader2();

        }).error(function (response) {

            alert(response);

            hideAjaxLoader2();
        })

    }
    $scope.listar = function (pageRequest) {

        showAjaxLoader2();

        var idtabela = $scope.filtro.TDC_ID;

        if ($scope.filtro.TIPO == 2) {
            idtabela = null;
        }


        var url = "/AcessoTabelas/Pesquisar";
        $http({
            url: url,
            method: "post",
            data: { _mes: $scope.filtro.mes, _ano: $scope.filtro.ano, _tdc_id: idtabela, _tipo: $scope.filtro.TIPO }
        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {

                $scope.listaAcessoTabelas = response.result.listaAcessoTabelas;

            }
            else {

                $scope.listaAcessoTabelas = null;

                alert(response.message.message);
            }

        }).error(function (response) {

            alert(response);

            hideAjaxLoader2();
        })

    }
    $scope.exportar = function () {

        showAjaxLoader2();

        var url = "/AcessoTabelas/ExportarResumo";
        var idtabela = $scope.filtro.TDC_ID;

        if ($scope.filtro.TIPO == 1) {
            url = "/AcessoTabelas/ExportarLista";
        }

        if ($scope.filtro.TIPO == 2){
            url = "/AcessoTabelas/ExportarLista";
            _idtabela = null;
        }

        $http({
            url: url,
            method: "post",
            data: { _mes: $scope.filtro.mes, _ano: $scope.filtro.ano, _tdc_id: _idtabela }
        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {

                $scope.export.lnkPath = response.result.retorno;

            }
            else {

                $scope.export.lnkPath = null;

                alert(response.message.message);
            }

        }).error(function (response) {

            alert(response);

            hideAjaxLoader2();
        })

    }

    function dataAtualFormatadatxt(dataHora) {

        var parseDate = new Date(parseInt(dataHora.substr(6)));
        var jsDate = new Date(parseDate);

        var data = jsDate;
        var dia = data.getDate();
        if (dia.toString().length == 1)
            dia = "0" + dia;
        var mes = data.getMonth() + 1;
        if (mes.toString().length == 1)
            mes = "0" + mes;
        var ano = data.getFullYear();
        return dia + "/" + mes + "/" + ano;

    }

    function dataAtualFormatada(dataHora) {

        var parseDate = new Date(parseInt(dataHora.substr(6)));
        var jsDate = new Date(parseDate);

        return jsDate;
    }



});
