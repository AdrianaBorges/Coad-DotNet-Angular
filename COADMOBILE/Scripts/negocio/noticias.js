appModule.controller('NoticiasController', function ($scope, formHandlerService, $http, conversionService, cepService) {


    $scope.listar = function (pageRequest) {

        $scope.message = null;
        var url = Util.getUrl("/Noticias/BuscaJson");

        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }

        var config = {
            url: url,
            targetObjectName: 'noticias',
            responseModelName: 'noticias',
            showAjaxLoader: true,
            pageConfig: { pageName: 'page' }
        };

        if ($scope.filtro) {

            config.data = $scope.filtro;
        }

        formHandlerService.read($scope, config);
    };


});
