appModule.controller('OrgaoController', function ($scope, formHandlerService, $http, conversionService) {

    $scope.init = function () {
        $scope.filtro = { ativoId: 1 };
    }

    $scope.salvarOrgao = function () {

        formHandlerService.submit($scope, {
            url: Util.getUrl("/orgao/salvar"),
            objectName: 'orgao',
            showAjaxLoader: true,
            success: function (resp, status, config, message, validationMessage) {
                $scope.message = message;
                $scope.erros = validationMessage;

                if (resp.success) {
                    alert("Salvo com sucesso.");
                    window.location = Util.getUrl("/orgao/index");
                }
            }
        });
    }

    $scope.listar = function (pageRequest) {

        var url = Util.getUrl("/orgao/orgaos");

        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }

        var config = {
            url: url,
            targetObjectName: 'orgaos',
            responseModelName: 'orgaos',
            pageConfig: { pageName: 'page' }
        };

        if ($scope.filtro) {

            config.data = angular.copy($scope.filtro);
        }
        formHandlerService.read($scope, config);
    };

    $scope.read = function (orgaoId) {

        if (orgaoId) {

            formHandlerService.read($scope, {
                url: Util.getUrl("/orgao/Readorgao"),
                targetObjectName: 'orgao',
                responseModelName: 'orgao',
                dateFieldsNames: ['DATA_CADASTRO', 'DATA_ALTERA', 'DATA_EXCLUSAO'],
                data: { orgaoId: orgaoId },
                success: function () {

                    //$scope.produto.PRO_RECEBE_MALA = Boolean($scope.produto.PRO_RECEBE_MALA);
                }
            });
        };


    }
});