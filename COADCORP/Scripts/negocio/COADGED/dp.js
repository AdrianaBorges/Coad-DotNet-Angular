appModule.controller('DpController', function ($scope, formHandlerService, $http, conversionService) {

    $scope.init = function () {
        $scope.filtro = { dpPeriodo: 4 };
        
    }

    $scope.buscarContraCheque = function () {

        showAjaxLoader();

        var url = "/DpController/BuscarContraCheque";
        $http({
            url: url,
            method: "post",
            data: { _cpf: $scope.cpf }

        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.listarRemessa($scope.paginaReq);
            }
            else {

                alert(response.message.message);

                hideAjaxLoader();
            }

        }).error(function (response) {

            alert(message);

            hideAjaxLoader();
        });

    }

    $scope.dpContracheque = function (pageRequest) {
        var url = Util.getUrl("/dp/contracheque");
        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }
        var config = {
            url: url,
            targetObjectName: 'dpContracheque',
            responseModelName: 'dpContracheque',
            pageConfig: { pageName: 'page' }
        };
        if ($scope.filtro) {
            config.data = angular.copy($scope.filtro);
        }
        formHandlerService.read($scope, config);
    };



});