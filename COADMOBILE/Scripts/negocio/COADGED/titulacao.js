appModule.controller('TitulacaoController', function ($scope, formHandlerService, $http, conversionService) {

    $scope.init = function () {
        $scope.filtro = { ativoId: 1 };
    }

    $scope.salvarTitulacao = function () {

        formHandlerService.submit($scope, {
            url: Util.getUrl("/titulacao/salvar"),
            objectName: 'titulacao',
            showAjaxLoader: true,
            success: function (resp, status, config, message, validationMessage) {
                $scope.message = message;
                $scope.erros = validationMessage;

                if (resp.success) {
                    alert("Salvo com sucesso.");
                    window.location = Util.getUrl("/titulacao/index");
                }
            }
        });
    }

    $scope.listar = function (pageRequest) {

        var url = Util.getUrl("/titulacao/titulacoes");

        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }

        var config = {
            url: url,
            targetObjectName: 'titulacoes',
            responseModelName: 'titulacoes',
            pageConfig: { pageName: 'page' }
        };

        if ($scope.filtro) {

            config.data = angular.copy($scope.filtro);
        }
        formHandlerService.read($scope, config);
    };

    $scope.read = function (titulacaoId) {

        if (titulacaoId) {

            formHandlerService.read($scope, {
                url: Util.getUrl("/titulacao/Readtitulacao"),
                targetObjectName: 'titulacao',
                responseModelName: 'titulacao',
                dateFieldsNames: ['DATA_CADASTRO', 'DATA_ALTERA', 'DATA_EXCLUSAO'],
                data: { titulacaoId: titulacaoId },
                success: function () {

                    //$scope.produto.PRO_RECEBE_MALA = Boolean($scope.produto.PRO_RECEBE_MALA);
                }
            });
        };


    }
});