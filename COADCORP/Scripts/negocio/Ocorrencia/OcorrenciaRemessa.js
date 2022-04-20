appModule.controller('OcorrenciaRemessaController', function ($scope, formHandlerService, $http, conversionService) {
    // salvar...
    $scope.salvar = function () {
        formHandlerService.submit($scope, {
            url: Util.getUrl("/ocorrenciaRemessa/salvar"),
            objectName: 'ocorrencia',
            showAjaxLoader: true,
            success: function (resp, status, config, message, validationMessage) {
                $scope.message = message;
                $scope.erros = validationMessage;
                if (resp.success) {
                    alert("Salvo com sucesso.");
                    window.location = Util.getUrl("/ocorrenciaRemessa/index");
                }
            }
        });
    }

    $scope.listar = function (pageRequest) {
        var url = Util.getUrl("/ocorrenciaRemessa/Listar");
        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }
        var config = {
            url: url,
            targetObjectName: 'ocorrencia',
            responseModelName: 'ocorrencia',
            showAjaxLoader: true,
            success: function (retorno) {
            },
            pageConfig: { pageName: 'page' }
        };
        if ($scope.filtro) {
            config.data = angular.copy($scope.filtro);
        }
        formHandlerService.read($scope, config);
    };

    $scope.read = function (b, c) {
        if (b && c) {
            formHandlerService.read($scope, {
                url: Util.getUrl("/ocorrenciaRemessa/ReadOcorrencia"),
                targetObjectName: 'ocorrencia',
                responseModelName: 'ocorrencia',
                showAjaxLoader: true,
                data: { bco: b, cod: c },
                success: function () {
                }
            });
        };
    }

});