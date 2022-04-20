appModule.controller('CoAreasController', function ($scope, formHandlerService, $http, conversionService, cepService) {
    
    $scope.init = function () {

    }
  
    $scope.listar = function (pageRequest) {

        $scope.message = null;
        var url = Util.getUrl("/areas/areas");

        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }

        var config = {
            url: url,
            targetObjectName: 'areas',
            responseModelName: 'areas',
            showAjaxLoader: true,
            pageConfig: { pageName: 'page' }
        };

        if ($scope.filtro) {

            config.data = $scope.filtro;
        }
       
        formHandlerService.read($scope, config);
    };
   
   
});
