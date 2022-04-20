appModule.controller('TitulacaoController', function ($scope, formHandlerService, $http, conversionService) {
    $scope.init = function () {
        $scope.filtro = { ativoId: 1 };
    }
    
    // Index: leia os grandes grupos, verbetes e subverbetes do colecionador informado...
    $scope.lerTitulacoesIndex = function (colecionadorId) {
        // informou colecionador?
        $scope.titulacao.todos = null;
        if (colecionadorId) {
            // busca os seus grandes grupos...
            formHandlerService.read($scope, {
                url: Util.getUrl("/titulacao/todos"),
                targetObjectName: 'todos',
                responseModelName: 'todos',
                data: { colecionadorId: colecionadorId },
                success: function (retorno) {
                    $scope.titulacao.todos = retorno.result.titulacao;
                }
            });
        }
        $scope.titulacao.inferiores = [{Selected: true,
                                       Text: "Grande Grupo",
                                       Value: "G"
                                      }];
    }

    // Index: leia os grandes grupos do colecionador informado...
    $scope.lerGgIndex = function (colecionadorId) {
        // informou colecionador?
        $scope.filtro.ggId = null;
        $scope.filtro.vbId = null;
        $scope.filtro.svbId = null;
        $scope.filtro.gg = null;
        colecionadorId = colecionadorId == "" ? null : colecionadorId;
        // busca os seus grandes grupos...
        formHandlerService.read($scope, {
            url: Util.getUrl("/publicacaoAreaConsultoria/Gg"),
            targetObjectName: 'gg',
            responseModelName: 'gg',
            data: { colecionadorId: colecionadorId },
            success: function (retorno) {
                if (!$scope.filtro.gg) {
                    $scope.filtro.gg = retorno.result.gg;
                }
            }
        });
        $scope.filtro.vb = null;
        $scope.filtro.svb = null;
    }

    // leia os verbetes do grande grupo escolhido...
    $scope.lerVerbetesIndex = function (ggId) {
        // informou grande grupo?
        if (ggId) {
            // limpando subverbetes...
            $scope.filtro.vb = null;
            $scope.filtro.ggId = ggId;
            // buscando...
            formHandlerService.read($scope, {
                url: Util.getUrl("/publicacaoAreaConsultoria/Verbetes"),
                targetObjectName: 'verbetes',
                responseModelName: 'verbetes',
                data: { ggId: ggId },
                success: function (retorno) {
                    $scope.filtro.vb = retorno.result.verbetes;
                }
            });
        } else {
            $scope.filtro.ggId = null;
            $scope.filtro.vbId = null;
            $scope.filtro.svbId = null;
        }
        $scope.filtro.svb = null;
    }

    // leia os subverbetes do verbete escolhido...
    $scope.lerSubverbetesIndex = function (vbId, svbId) {
        // limpando os subverbetes...
        $scope.filtro.svb = null;
        $scope.filtro.vbId = vbId;
        $scope.filtro.svbId = svbId;
        if (vbId) {
            // carregando subverbetes...
            formHandlerService.read($scope, {
                url: Util.getUrl("/publicacaoAreaConsultoria/Subverbetes"),
                targetObjectName: 'subverbetes',
                responseModelName: 'subverbetes',
                data: { vbId: vbId },
                success: function (retorno) {
                    $scope.filtro.svb = retorno.result.subverbetes;
                }
            });
        } else {
            $scope.filtro.vbId = null;
            $scope.filtro.svbId = null;
        }
    }

    $scope.titulacaoSuperior = function (superior) {
        $scope.titulacao.TIT_ID_REFERENCIA = superior;
        formHandlerService.read($scope, {
            url: Util.getUrl("/titulacao/titulacoesInferiores"),
            targetObjectName: 'inferiores',
            responseModelName: 'inferiores',
            data: { titulacaoId: $scope.titulacao.TIT_ID_REFERENCIA },
            success: function (retorno) {
                $scope.titulacao.inferiores = retorno.result.inferiores;
            }
        });
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
        var url = Util.getUrl("/titulacao/filtrarTitulacoes");
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
                success: function (retorno) {
                    $scope.titulacaoSuperior(retorno.result.titulacao.TIT_ID_REFERENCIA);
                }
            });
        };
    }
});