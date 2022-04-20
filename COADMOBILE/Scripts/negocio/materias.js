appModule.controller('MateriasController', function ($scope, formHandlerService, $http, conversionService, cepService) {

    $scope.init = function () {
        $scope.filtro = {};
        $scope.filtro.data = new Date();
    }

    $scope.listarOrientacoes = function (pageRequest) {

        $scope.message = null;
        var url = Util.getUrl("/Materias/MateriasOriJson");

        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }

        var config = {
            url: url,
            targetObjectName: 'materiasOri',
            responseModelName: 'materiasOri',
            showAjaxLoader: true,
            pageConfig: { pageName: 'page' }
        };

        if ($scope.filtroOrientacao) {
            $scope.filtroOrientacao.label = 'Orientacao';
            config.data = $scope.filtroOrientacao;
        }

        formHandlerService.read($scope, config);
    };

    $scope.listarAtoslegais = function (pageRequest) {

        $scope.message = null;
        var url = Util.getUrl("/Materias/MateriasAtoJson");

        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }

        var config = {
            url: url,
            targetObjectName: 'materiasAto',
            responseModelName: 'materiasAto',
            showAjaxLoader: true,
            pageConfig: { pageName: 'page' }//,
            //success: function (retorno) {
            //   $scope.lstAreas = retorno.result.areas;
            //    $scope.lstEstados = retorno.result.estados;
            //    $scope.lstMunicipios = retorno.result.municipios;
            //}
        };

        if ($scope.filtroAtosLegais) {
            $scope.filtroAtosLegais.label = 'Atos_Legais';
            config.data = $scope.filtroAtosLegais;
        }

        formHandlerService.read($scope, config);
    };



    $scope.prencherArea = function (pageRequest) {

        $scope.message = null;
        var url = Util.getUrl("/Materias/TrazerItensOrientacao");

        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }

        var config = {
            url: url,
            targetObjectName: 'itensArea',
            responseModelName: 'itensArea',
            showAjaxLoader: true,
            pageConfig: { pageName: 'page' },
            success: function (retorno) {
                $scope.itensArea = retorno.result.itensArea;
            }
        };

        if ($scope.filtroOrientacao) {

            config.data = $scope.filtroOrientacao;
        }

        formHandlerService.read($scope, config);
    };
});



