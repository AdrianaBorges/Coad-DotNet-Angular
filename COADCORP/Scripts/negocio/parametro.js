appModule.controller('ParametroController', function ($scope, formHandlerService, $http, conversionService, $timeout) {

    $scope.filtro = {};
    $scope.filtro2 = {};
    var now = new Date();


    $scope.init = function () {

        $scope.filtro = {};
        $scope.filtro.dtini = new Date(now.getFullYear(), now.getMonth(), 1);
        $scope.filtro.dtfim = new Date(now.getFullYear(), now.getMonth(), now.getDate());
        $scope.filtro.emp_id = 2;
        $scope.filtro.mes = now.getMonth();
        $scope.filtro.ano = now.getFullYear();
        $scope.filtro.bco = "237";

        $scope.buscarParametroGrupo();
        $scope.buscarContasBanco();
    }

    $scope.buscarContasBanco = function () {

        showAjaxLoader();

        var url = Util.getUrl("/Remessa/BuscarContasBanco");
        $http({
            url: url,
            method: "post",
            data: { bco: $scope.filtro.bco, cta_emite_boleto: ($scope.filtro.tpAlocacao == "avulsa") }
        }).success(function (response) {

            if (response.success == true) {

                $scope.listacontas = response.result.listacontas;

                conversionService.deepConversion($scope.listacontas);
            }
            else {

                $scope.erro = response.message.message;

                angular.element("#Modal-Erro").modal();

                hideAjaxLoader();

            }

            hideAjaxLoader();


        }).error(function (response) {

            $scope.erro = response;

            angular.element("#Modal-Erro").modal();

            hideAjaxLoader();

        });
    }


    $scope.buscarParametroGrupo = function () {

        showAjaxLoader2();

        var url = "/Parametro/BuscarParametroGrupo";
        $http({
            url: url,
            method: "post"
        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {

                $scope.lstParametroGrupo = response.result.lstParametroGrupo;
                conversionService.deepConversion($scope.lstParametroGrupo);

            }
            else {

                $scope.message = response.message;

                $scope.lstParametros = null;

                hideAjaxLoader2();
            }

        }).error(function (response) {

            $scope.message = response.message;
            $scope.lstParametros = null;

            hideAjaxLoader2();
        });
    }
    $scope.buscaParametros = function (pgrid) {

        showAjaxLoader2();

        var url = "/Parametro/BuscarParametros";
        $http({
            url: url,
            method: "post",
            data: { _pgr_id: pgrid }
        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {

                $scope.lstParametros = response.result.lstParametros;
                conversionService.deepConversion($scope.lstParametros);

            }
            else {

                $scope.message = response.message;

                $scope.lstParametros = null;

                hideAjaxLoader2();
            }

        }).error(function (response) {

            $scope.message = response.message;
            $scope.lstParametros = null;

            hideAjaxLoader2();
        });

    }
    $scope.salvar = function () {

        showAjaxLoader2();

        var url = "/Parametro/Salvar";
        $http({
            url: url,
            method: "post",
            data: { _lstParametros: $scope.lstParametros }
        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {

                $scope.message = response.message;
            }
            else {

                $scope.message = response.message;

                $scope.lstParametros = null;

                hideAjaxLoader2();
            }

        }).error(function (response) {

            $scope.message = response.message;

            $scope.lstParametros = null;

            hideAjaxLoader2();
        });

    }

});