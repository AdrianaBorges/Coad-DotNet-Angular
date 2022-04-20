appModule.controller('CargosController', function ($scope, formHandlerService, $http, conversionService) {

    $scope.salvarCargos = function () {

        formHandlerService.submit($scope, {
            url: Util.getUrl("/cargos/salvar"),
            objectName: 'cargos',
            showAjaxLoader: true,
            success: function (resp, status, config, message, validationMessage) {
                $scope.message = message;
                $scope.erros = validationMessage;

                if (resp.success) {
                    alert("Salvo com sucesso.");
                    window.location = Util.getUrl("/cargos/index");
                }
            }
        });
    }

    $scope.listar = function (pageRequest) {

        var url = Util.getUrl("/cargos/cargos");

        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }

        var config = {
            url: url,
            targetObjectName: 'cargos',
            responseModelName: 'cargos',
            pageConfig: { pageName: 'page' }
        };

        if ($scope.filtro) {

            config.data = angular.copy($scope.filtro);
        }
        formHandlerService.read($scope, config);
    };

    $scope.read = function (cargoId) {

        if (cargoId) {

            formHandlerService.read($scope, {
                url: Util.getUrl("/cargos/Readcargo"),
                targetObjectName: 'cargos',
                responseModelName: 'cargos',
                dateFieldsNames: ['DATA_CADASTRO', 'DATA_ALTERA', 'DATA_EXCLUSAO'],
                data: { cargoId: cargoId },
                success: function () {

                    //$scope.produto.PRO_RECEBE_MALA = Boolean($scope.produto.PRO_RECEBE_MALA);
                }
            });
        };


    }
});