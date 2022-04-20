appModule.controller('OcorrenciaErroController', function ($scope, formHandlerService, $http, conversionService) {
    // salvar...
    $scope.salvar = function () {
        formHandlerService.submit($scope, {
            url: Util.getUrl("/ocorrenciaErro/salvar"),
            objectName: 'ocorrencia',
            showAjaxLoader: true,
            success: function (resp, status, config, message, validationMessage) {
                $scope.message = message;
                $scope.erros = validationMessage;
                if (resp.success) {
                    alert("Salvo com sucesso.");
                    window.location = Util.getUrl("/ocorrenciaErro/index");
                }
            }
        });
    }

    $scope.listar = function (pageRequest) {
        var url = Util.getUrl("/ocorrenciaErro/Listar");
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

    $scope.read = function (b, c, r) {
        if (b && c && r) {
            formHandlerService.read($scope, {
                url: Util.getUrl("/ocorrenciaErro/ReadOcorrencia"),
                targetObjectName: 'ocorrencia',
                responseModelName: 'ocorrencia',
                showAjaxLoader: true,
                data: { bco: b, cod: c, codRet: r },
                success: function (retorno) {
                }
            });
        };
    }

});