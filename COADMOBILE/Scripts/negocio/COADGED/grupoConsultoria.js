appModule.controller('GrupoConsultoriaController', function ($scope, formHandlerService, $http, conversionService) {

    $scope.init = function () {
        $scope.filtro = { ativoId: 1 };
    }

    $scope.salvarGrupoConsultoria = function () {

        formHandlerService.submit($scope, {
            url: Util.getUrl("/grupoConsultoria/salvar"),
            objectName: 'grupoConsultoria',
            showAjaxLoader: true,
            success: function (resp, status, config, message, validationMessage) {
                $scope.message = message;
                $scope.erros = validationMessage;

                if (resp.success) {
                    alert("Salvo com sucesso.");
                    window.location = Util.getUrl("/grupoConsultoria/index");
                }
            }
        });
    }

    $scope.listar = function (pageRequest) {

        var url = Util.getUrl("/grupoConsultoria/gruposConsultoria");

        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }

        var config = {
            url: url,
            targetObjectName: 'gruposConsultoria',
            responseModelName: 'gruposConsultoria',
            pageConfig: { pageName: 'page' }
        };

        if ($scope.filtro) {

            config.data = angular.copy($scope.filtro);
        }
        formHandlerService.read($scope, config);
    };

    $scope.read = function (grupoConsultoriaId) {

        if (grupoConsultoriaId) {

            formHandlerService.read($scope, {
                url: Util.getUrl("/grupoConsultoria/ReadgrupoConsultoria"),
                targetObjectName: 'grupoConsultoria',
                responseModelName: 'grupoConsultoria',
                dateFieldsNames: ['DATA_CADASTRO', 'DATA_ALTERA', 'DATA_EXCLUSAO'],
                data: { grupoConsultoriaId: grupoConsultoriaId },
                success: function () {

                    //$scope.produto.PRO_RECEBE_MALA = Boolean($scope.produto.PRO_RECEBE_MALA);
                }
            });
        };


    }
});