appModule.controller('AreasController', function ($scope, formHandlerService, $http, conversionService) {

    $scope.salvarAreas = function () {

        $scope.areas.PRODUTO_REF = null;

        formHandlerService.submit($scope, {
            url: Util.getUrl("/areas/salvar"),
            objectName: 'areas',
            showAjaxLoader: true,
            success: function (resp, status, config, message, validationMessage) {
                $scope.message = message;
                $scope.erros = validationMessage;

                if (resp.success) {
                    alert("Salvo com sucesso.");
                    window.location = Util.getUrl("/areas/index");
                }
            }
        });
    }

    $scope.listar = function (pageRequest) {

        var url = Util.getUrl("/areas/areas");

        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }

        var config = {
            url: url,
            targetObjectName: 'areas',
            responseModelName: 'areas',
            pageConfig: { pageName: 'page' }
        };

        if ($scope.filtro) {

            config.data = angular.copy($scope.filtro);
        }
        formHandlerService.read($scope, config);
    };

    $scope.read = function (areaId) {

        if (areaId) {

            formHandlerService.read($scope, {
                url: Util.getUrl("/areas/Readarea"),
                targetObjectName: 'areas',
                responseModelName: 'areas',
                dateFieldsNames: ['DATA_CADASTRO', 'DATA_ALTERA', 'DATA_EXCLUSAO'],
                data: { areaId: areaId },
                success: function () {

                    //$scope.produto.PRO_RECEBE_MALA = Boolean($scope.produto.PRO_RECEBE_MALA);
                }
            });
        };


    }
});