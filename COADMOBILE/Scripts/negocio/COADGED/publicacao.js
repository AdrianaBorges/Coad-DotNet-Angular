appModule.controller('PublicacaoController', function ($scope, formHandlerService, $http, conversionService) {

    $scope.init = function () {
        $scope.filtro = { ativoId: 1 };
    }

    $scope.salvarPublicacao = function () {
        formHandlerService.submit($scope, {
            url: Util.getUrl("/publicacao/salvar"),
            objectName: 'publicacao',
            showAjaxLoader: true,
            success: function (resp, status, config, message, validationMessage) {
                $scope.message = message;
                $scope.erros = validationMessage;
                if (resp.success) {
                    alert("Salvo com sucesso.");
                    window.location = Util.getUrl("/publicacao/index");
                }
            }
        });
    }

    $scope.listar = function (pageRequest) {
        var url = Util.getUrl("/publicacao/publicacoes");
        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }
        var config = {
            url: url,
            targetObjectName: 'publicacoes',
            responseModelName: 'publicacoes',
            pageConfig: { pageName: 'page' }
        };
        if ($scope.filtro) {
            config.data = angular.copy($scope.filtro);
        }
        formHandlerService.read($scope, config);
    };

    $scope.read = function (publicacaoId) {
        if (publicacaoId) {
            formHandlerService.read($scope, {
                url: Util.getUrl("/publicacao/Readpublicacao"),
                targetObjectName: 'publicacao',
                responseModelName: 'publicacao',
                dateFieldsNames: ['DATA_CADASTRO', 'DATA_ALTERA', 'DATA_EXCLUSAO'],
                data: { publicacaoId: publicacaoId },
                success: function () {
                }
            });
        }
    }

    $scope.$watch('publicacao.TIT_ID', function (value, oldvalue) {
        if (value !== oldvalue) {
            $scope.lerVerbetes(value);
        }
    });

    $scope.$watch('publicacao.TIT_ID_VERBETE', function (value, oldvalue) {
        if (value !== oldvalue) {
            $scope.lerSubverbetes(value);
        }
    });

    $scope.lerVerbetes = function (ggId) {
        if (ggId) {
            formHandlerService.read($scope, {
                url: Util.getUrl("/publicacao/Verbetes"),
                targetObjectName: 'verbetes',
                responseModelName: 'verbetes',
                data: { ggId: ggId },
                success: function (retorno) {
                    //console.info($scope.verbetes);
                    //targetObjectName                 responseModelName
                    //$scope.verbetes = retorno.result.verbetes;
                    //$scope.publicacao.TIT_ID_VERBETE = retorno;
                }
            });
        } else {
            $scope.verbetes = null;
            $scope.subverbetes = null;
        }
    }

    $scope.lerSubverbetes = function (vbId) {
        if (vbId) {
            formHandlerService.read($scope, {
                url: Util.getUrl("/publicacao/Subverbetes"),
                targetObjectName: 'subverbetes',
                responseModelName: 'subverbetes',
                data: { vbId: vbId },
                success: function (retorno) {
                }
            });
        } else {
            $scope.subverbetes = null;
        }
    }
});