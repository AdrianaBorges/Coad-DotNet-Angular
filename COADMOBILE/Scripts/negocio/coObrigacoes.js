appModule.controller('CoObrigacoesController', function ($scope, formHandlerService, $http, conversionService, cepService) {

    $scope.init = function () {
        $scope.filtro = {};
        $scope.filtro.data = new Date();
        $scope.listar();
    }

    $scope.$watch("filtro.data", function (value, old) {
        if (value) {
            $scope.listar();
        }
    }, true);


    $scope.listar = function (pageRequest) {
        $scope.message = null;
        var url = Util.getUrl("/Calendario/ObrigacoesJson");

        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }

        var config = {
            url: url,
            targetObjectName: 'obrigacoes',
            responseModelName: 'obrigacoes',
            showAjaxLoader: true,
            pageConfig: { pageName: 'page' },            
            success: function (retorno) {
                $scope.lstAreas = retorno.result.areas;
                $scope.lstEstados = retorno.result.estados;
                $scope.lstMunicipios = retorno.result.municipios;
            }
        };

        if ($scope.filtro) {
            config.data = $scope.filtro;
        }

        formHandlerService.read($scope, config);
    };
});
