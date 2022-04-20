appModule.controller('InformativoController', function ($scope, formHandlerService, $http, conversionService) {

    $scope.init = function () {
        $scope.filtro = { ativoId: 1 };
    }

    /* Ler informativos ativos em produção */
    $scope.lerInformativosEmProducao = function (pageRequest) {
        var url = Util.getUrl("/informativo/lerInformativosEmProducao");

        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }

        var config = {
            url: url,
            targetObjectName: 'informativosEmProducao',
            responseModelName: 'informativosEmProducao',
            pageConfig: { pageName: 'page' }
        };

        if ($scope.filtro) {
            config.data = angular.copy($scope.filtro);
        }

        formHandlerService.read($scope, config);
    };

    $scope.incluirInformativo = function () {

        formHandlerService.submit($scope, {
            url: Util.getUrl("/informativo/salvarInclusao"),
            objectName: 'informativo',
            showAjaxLoader: true,
            success: function (resp, status, config, message, validationMessage) {
                $scope.message = message;
                $scope.erros = validationMessage;

                if (resp.success) {
                    alert("Salvo com sucesso.");
                    window.location = Util.getUrl("/informativo/index");
                }
            }
        });
    }

    $scope.alterarInformativo = function () {

        formHandlerService.submit($scope, {
            url: Util.getUrl("/informativo/salvarAlteracao"),
            objectName: 'informativo',
            showAjaxLoader: true,
            success: function (resp, status, config, message, validationMessage) {
                $scope.message = message;
                $scope.erros = validationMessage;

                if (resp.success) {
                    alert("Salvo com sucesso.");
                    window.location = Util.getUrl("/informativo/index");
                }
            }
        });
    }

    $scope.listar = function (pageRequest) {

        var url = Util.getUrl("/informativo/informativos");

        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }

        var config = {
            url: url,
            targetObjectName: 'informativos',
            responseModelName: 'informativos',
            pageConfig: { pageName: 'page' }
        };

        if ($scope.filtro) {

            config.data = angular.copy($scope.filtro);
        }
        formHandlerService.read($scope, config);
    };

    $scope.read = function (ano, numero) {

        if (ano && numero) {

            formHandlerService.read($scope, {
                url: Util.getUrl("/informativo/Readinformativo"),
                targetObjectName: 'informativo',
                responseModelName: 'informativo',
                dateFieldsNames: ['DATA_CADASTRO', 'DATA_ALTERA', 'DATA_EXCLUSAO'],
                data: { ano: ano, numero: numero },
                success: function () {

                    //$scope.produto.PRO_RECEBE_MALA = Boolean($scope.produto.PRO_RECEBE_MALA);
                }
            });
        };


    }
});