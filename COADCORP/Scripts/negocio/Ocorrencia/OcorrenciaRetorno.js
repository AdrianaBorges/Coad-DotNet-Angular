appModule.controller('OcorrenciaRetornoController', function ($scope, formHandlerService, $http, conversionService) {
    // salvar...
    $scope.salvar = function () {
        $scope.ocorrencia.OCT_BAIXAR_TITULO = parseInt($scope.ocorrencia.OCT_BAIXAR_TITULO);
        $scope.ocorrencia.OCT_DESALOCAR_TITULO = parseInt($scope.ocorrencia.OCT_DESALOCAR_TITULO);
        $scope.ocorrencia.OCT_REGISTRAR_TITULO = parseInt($scope.ocorrencia.OCT_REGISTRAR_TITULO);
        formHandlerService.submit($scope, {
            url: Util.getUrl("/ocorrenciaRetorno/salvar"),
            objectName: 'ocorrencia',
            showAjaxLoader: true,
            success: function (resp, status, config, message, validationMessage) {
                $scope.message = message;
                $scope.erros = validationMessage;
                if (resp.success) {
                    alert("Salvo com sucesso.");
                    window.location = Util.getUrl("/ocorrenciaRetorno/index");
                }
            }
        });
    }

    $scope.listar = function (pageRequest) {
        var url = Util.getUrl("/ocorrenciaRetorno/Listar");
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
                url: Util.getUrl("/ocorrenciaRetorno/ReadOcorrencia"),
                targetObjectName: 'ocorrencia',
                responseModelName: 'ocorrencia',
                showAjaxLoader: true,
                data: { bco: b, cod: c },
                success: function (r) {
                    $scope.ocorrencia.OCT_BAIXAR_TITULO = r.result.ocorrencia.OCT_BAIXAR_TITULO ? "1" : "0";
                    $scope.ocorrencia.OCT_DESALOCAR_TITULO = r.result.ocorrencia.OCT_DESALOCAR_TITULO ? "1" : "0";
                    $scope.ocorrencia.OCT_REGISTRAR_TITULO = r.result.ocorrencia.OCT_REGISTRAR_TITULO ? "1" : "0";
                }
            });
        };
    }

});