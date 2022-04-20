appModule.controller('TipoAtoController', function ($scope, formHandlerService, $http, conversionService) {

    $scope.init = function () {
        $scope.filtro = { ativoId: 1 };
    }

    $scope.salvarTipoAto = function () {

        formHandlerService.submit($scope, {
            url: Util.getUrl("/tipoAto/salvar"),
            objectName: 'tipoAto',
            showAjaxLoader: true,
            success: function (resp, status, config, message, validationMessage) {
                $scope.message = message;
                $scope.erros = validationMessage;

                if (resp.success) {
                    alert("Salvo com sucesso.");
                    window.location = Util.getUrl("/tipoAto/index");
                }
            }
        });
    }

    $scope.listar = function (pageRequest) {

        var url = Util.getUrl("/tipoAto/tiposAtos");

        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }

        var config = {
            url: url,
            targetObjectName: 'tiposAtos',
            responseModelName: 'tiposAtos',
            pageConfig: { pageName: 'page' }
        };

        if ($scope.filtro) {

            config.data = angular.copy($scope.filtro);
        }
        formHandlerService.read($scope, config);
    };

    $scope.read = function (tipoAtoId) {

        if (tipoAtoId) {

            formHandlerService.read($scope, {
                url: Util.getUrl("/tipoAto/ReadtipoAto"),
                targetObjectName: 'tipoAto',
                responseModelName: 'tipoAto',
                dateFieldsNames: ['DATA_CADASTRO', 'DATA_ALTERA', 'DATA_EXCLUSAO'],
                data: { tipoAtoId: tipoAtoId },
                success: function () {

                    //$scope.produto.PRO_RECEBE_MALA = Boolean($scope.produto.PRO_RECEBE_MALA);
                }
            });
        };


    }
});