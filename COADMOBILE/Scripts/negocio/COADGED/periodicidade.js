appModule.controller('PeriodicidadeController', function ($scope, formHandlerService, $http, conversionService) {

    $scope.salvarPeriodicidade = function () {

        formHandlerService.submit($scope, {
            url: Util.getUrl("/periodicidade/salvar"),
            objectName: 'periodicidade',
            showAjaxLoader: true,
            success: function (resp, status, config, message, validationMessage) {
                $scope.message = message;
                $scope.erros = validationMessage;

                if (resp.success) {
                    alert("Salvo com sucesso.");
                    window.location = Util.getUrl("/periodicidade/index");
                }
            }
        });
    }

    $scope.listar = function (pageRequest) {

        var url = Util.getUrl("/periodicidade/periodicidades");

        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }

        var config = {
            url: url,
            targetObjectName: 'periodicidades',  // precisa existir na View
            responseModelName: 'periodicidades', // precisa existir no Controller
            pageConfig: { pageName: 'page' }
        };

        if ($scope.filtro) {

            config.data = angular.copy($scope.filtro);
        }
        formHandlerService.read($scope, config);
    };

    $scope.read = function (periodoId) {

        if (periodoId) {

            formHandlerService.read($scope, {
                url: Util.getUrl("/periodicidade/Readperiodicidade"),
                targetObjectName: 'periodicidade',  // precisa existir na View
                responseModelName: 'periodicidade',       // precisa existir no Controller
                dateFieldsNames: ['DATA_CADASTRO', 'DATA_ALTERA', 'DATA_EXCLUSAO'],
                data: { periodoId: periodoId },
                success: function () {

                    //$scope.produto.PRO_RECEBE_MALA = Boolean($scope.produto.PRO_RECEBE_MALA);
                }
            });
        };


    }
});