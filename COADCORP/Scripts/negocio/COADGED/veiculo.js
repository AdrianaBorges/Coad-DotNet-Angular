appModule.controller('VeiculoController', function ($scope, formHandlerService, $http, conversionService) {

    $scope.init = function () {
        $scope.filtro = { ativoId: 1 };
    }

    $scope.salvarVeiculo = function () {

        $scope.veiculo.PERIODICIDADE = null;

        formHandlerService.submit($scope, {
            url: Util.getUrl("/veiculo/salvar"),
            objectName: 'veiculo',
            showAjaxLoader: true,
            success: function (resp, status, config, message, validationMessage) {
                $scope.message = message;
                $scope.erros = validationMessage;

                if (resp.success) {
                    alert("Salvo com sucesso.");
                    window.location = Util.getUrl("/veiculo/index");
                }
            }
        });
    }

    $scope.listar = function (pageRequest) {

        var url = Util.getUrl("/veiculo/veiculos");

        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }

        var config = {
            url: url,
            targetObjectName: 'veiculos',
            responseModelName: 'veiculos',
            pageConfig: { pageName: 'page' }
        };

        if ($scope.filtro) {

            config.data = angular.copy($scope.filtro);
        }
        formHandlerService.read($scope, config);
    };

    $scope.read = function (veiculoId) {

        if (veiculoId) {

            formHandlerService.read($scope, {
                url: Util.getUrl("/veiculo/Readveiculo"),
                targetObjectName: 'veiculo',
                responseModelName: 'veiculo',
                dateFieldsNames: ['DATA_CADASTRO', 'DATA_ALTERA', 'DATA_EXCLUSAO'],
                data: { veiculoId: veiculoId },
                success: function () {

                    //$scope.produto.PRO_RECEBE_MALA = Boolean($scope.produto.PRO_RECEBE_MALA);
                }
            });
        };


    }
});