appModule.controller('LabelsController', function ($scope, formHandlerService, $http, conversionService) {

    $scope.init = function () {
        $scope.filtro = { ativoId: 1 };
    }

    $scope.salvarLabel = function () {

        formHandlerService.submit($scope, {
            url: Util.getUrl("/labels/salvar"),
            objectName: 'label',
            showAjaxLoader: true,
            success: function (resp, status, config, message, validationMessage) {
                $scope.message = message;
                $scope.erros = validationMessage;

                if (resp.success) {
                    alert("Salvo com sucesso.");
                    window.location = Util.getUrl("/labels/index");
                }
            }
        });
    }

    $scope.listar = function (pageRequest) {

        var url = Util.getUrl("/labels/labels");

        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }

        var config = {
            url: url,
            targetObjectName: 'labels',
            responseModelName: 'labels',
            pageConfig: { pageName: 'page' }
        };

        if ($scope.filtro) {

            config.data = angular.copy($scope.filtro);
        }
        formHandlerService.read($scope, config);
    };

    $scope.read = function (labelId) {

        if (labelId) {

            formHandlerService.read($scope, {
                url: Util.getUrl("/labels/Readlabel"),
                targetObjectName: 'label',
                responseModelName: 'label',
                dateFieldsNames: ['DATA_CADASTRO', 'DATA_ALTERA', 'DATA_EXCLUSAO'],
                data: { labelId: labelId },
                success: function () {

                    //$scope.produto.PRO_RECEBE_MALA = Boolean($scope.produto.PRO_RECEBE_MALA);
                }
            });
        };


    }
});