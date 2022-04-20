appModule.controller('ColaboradorController', function ($scope, formHandlerService, $http, conversionService) {

    $scope.init = function () {
        $scope.filtro = { ativoId: 1 };
    }

    $scope.salvarColaborador = function () {

        $scope.colaborador.CARGOS = null;

        formHandlerService.submit($scope, {
            url: Util.getUrl("/colaborador/salvar"),
            objectName: 'colaborador',
            showAjaxLoader: true,
            success: function (resp, status, config, message, validationMessage) {
                $scope.message = message;
                $scope.erros = validationMessage;

                if (resp.success) {
                    alert("Salvo com sucesso.");
                    window.location = Util.getUrl("/colaborador/index");
                }
            }
        });
    }

    $scope.listar = function (pageRequest) {

        var url = Util.getUrl("/colaborador/colaboradores");

        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }

        var config = {
            url: url,
            targetObjectName: 'colaboradores',
            responseModelName: 'colaboradores',
            pageConfig: { pageName: 'page' }
        };

        if ($scope.filtro) {

            config.data = angular.copy($scope.filtro);
        }
        formHandlerService.read($scope, config);
    };

    $scope.read = function (colaboradorId) {

        if (colaboradorId) {

            formHandlerService.read($scope, {
                url: Util.getUrl("/colaborador/Readcolaborador"),
                targetObjectName: 'colaborador',
                responseModelName: 'colaborador',
                dateFieldsNames: ['DATA_CADASTRO', 'DATA_ALTERA', 'DATA_EXCLUSAO'],
                data: { colaboradorId: colaboradorId },
                success: function () {

                    //$scope.produto.PRO_RECEBE_MALA = Boolean($scope.produto.PRO_RECEBE_MALA);
                }
            });
        };


    }
});