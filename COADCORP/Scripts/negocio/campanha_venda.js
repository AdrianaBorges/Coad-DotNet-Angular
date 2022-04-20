appModule.controller("CampanhaVendaController", ['$scope', '$http', 'formHandlerService', 'MathService', '$timeout', 'UploadAjax', 'Upload', 'FilterService', 'UenService',
    function ($scope, $http, formHandlerService, MathService, $timeout, UploadAjax, Upload, FilterService, UenService) {
    
    $scope.popoverAberto = false;
    $scope.initList = function (ehGerente) {
        
       
    }

    $scope.init = function (cveId) {
        $scope.carregarTipoProposta();
        $scope.carregarTipoPagamento();
        $scope.carregarEmpresas();

        $scope.retornarUenAtual(function () {

            $scope.$watch('uen', function () {

                $scope.listarProdutoComposicao();
            });
        });

        $scope.query = { show: true };

        if (cveId) {
            $scope.read(cveId, function () {

                $scope.acharTipoPropostaAdicionado($scope.campanhaVendas.CAMPANHA_VENDA_TIPO_PROPOSTA);
            });
        }
        else {
            $scope.campanhaVendas = {

                CAMPANHA_VENDA_TIPO_PROPOSTA: [],
                TIPO_PAGAMENTO_CAMPANHA_VENDA: [],
                CAMPANHA_VENDAS_PRODUTO_COMPOSICAO : []
            };
        }
    };

        $scope.retornarUenAtual = function (callback) {

            var url = Util.getUrl('/UEN/RetornarUenAtual');
            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'uen',
                responseModelName: 'uen',
                success: function () {
                    UenService.observarUenAuterada($scope, 'uen');

                    if (callback != null && typeof callback == 'function') {

                        callback();
                    };
                }
            });
        };


    $scope.carregarTipoProposta = function () {

        var url = Util.getUrl("/proposta/listarTipoProposta");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstTipoProposta',
            responseModelName: 'lstTipoProposta',
            showAjaxLoader: true,
            success: function () {
                $scope.inicializarTipos();
            }

        });
    }

    $scope.carregarTipoPagamento = function () {

        var url = Util.getUrl("/campanhaVenda/listarTipoPagamentoSimples");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstTipoPagamento',
            responseModelName: 'lstTipoPagamento',
            showAjaxLoader: true,
            success: function () {
                $scope.inicializarTipoPagamento();
            }

        });
    }


    $scope.pesquisarCampanhaVenda = function (pagina) {

        $scope.paginaAtual = pagina;
        $scope.listado = true;
        var url = Util.getUrl("/campanhaVenda/pesquisarCampanhaVenda");

        if (pagina) {

            url += "?pagina=" + pagina;
        }
        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstCampanhaVenda',
            responseModelName: 'lstCampanhaVenda',
            showAjaxLoader: true,
            data: $scope.filtro,
            pageConfig: { pageName: 'page' /*, pageTargetName: 'pageAgendamentoDoDia' */ },
            success: function (resp) {

            }
        });
    };
    
    $scope.read = function (cveId, onSuccess) {

        if (cveId != null) {
            var url = Util.getUrl("/campanhaVenda/recuperarDadosDaCampanhaVenda");

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'campanhaVendas',
                responseModelName: 'campanhaVendas',
                showAjaxLoader: true,
                data: { cveId: cveId },
                success: function () {
                    
                    if (onSuccess && typeof (onSuccess) == 'function') {

                        onSuccess();
                    }
                }
            });
        }
    }

       
    $scope.salvarCampanha = function (sair) {

        formHandlerService.submit($scope, {
            url: Util.getUrl("/campanhaVenda/SalvarCampanhaVenda"),
            objectName: 'campanhaVendas',
            showAjaxLoader: true,
            success: function (resp, status, config, message, validationMessage) {

                $scope.message = message;
                $scope.erros = validationMessage;

                $scope.button = 'reset';
                if (resp.success) {

                    $timeout(function () {

                        window.open(Util.getUrl('/campanhaVenda/index'), '_self');

                    }, 1000);

                }
            }
        });
    }


    $scope.adicionarTipoProposta = function (tipoProposta) {

        if (tipoProposta && $scope.campanhaVendas && $scope.campanhaVendas.CAMPANHA_VENDA_TIPO_PROPOSTA) {

            if ($scope.verificaDuplicacaoTipoProposta(tipoProposta)) {

                $scope.campanhaVendas.CAMPANHA_VENDA_TIPO_PROPOSTA.push({ TIPO_PROPOSTA: angular.copy(tipoProposta), TPP_ID: tipoProposta.TPP_ID});
            }
            else {

                $scope.message = Util.createMessage("fail", "O tipo de Proposta/Pedido já existe!");
            }

        }
    }

    $scope.verificaDuplicacaoTipoProposta = function (item) {

        var achou = true;
        if (item && $scope.campanhaVendas && $scope.campanhaVendas.CAMPANHA_VENDA_TIPO_PROPOSTA) {

            angular.forEach($scope.campanhaVendas.CAMPANHA_VENDA_TIPO_PROPOSTA, function (value, index) {

                if (value.TIPO_PROPOSTA.TPP_ID === item.TPP_ID) {

                    achou = false;
                    return;
                }
            });
        }

        return achou;
    }

    $scope.$watch("campanhaVendas.CAMPANHA_VENDA_TIPO_PROPOSTA", function (value, old) {

        if (value) {
            $scope.acharTipoPropostaAdicionado(value);
        }

    }, true);

    $scope.inicializarTipos = function () {

        angular.forEach($scope.lstTipoProposta, function (value, old) {

            value.show = true;
        });

    }
    
    $scope.acharTipoPropostaAdicionado = function (tiposProposta) {

        $scope.inicializarTipos();

        if (tiposProposta) {

            angular.forEach($scope.lstTipoProposta, function (value, old) {

                angular.forEach(tiposProposta, function (subValue, subOld) {

                    if (subValue.TIPO_PROPOSTA && value.TPP_ID == subValue.TIPO_PROPOSTA.TPP_ID) {

                        value.show = false;
                    }
                });
            });
        }

    }

    $scope.excluirTipoProposta = function (index) {

        if ($scope.campanhaVendas && $scope.campanhaVendas.CAMPANHA_VENDA_TIPO_PROPOSTA) {

            $scope.campanhaVendas.CAMPANHA_VENDA_TIPO_PROPOSTA.splice(index, 1);

        }
    }


    //-----
    
    $scope.adicionarTipoPagamento = function (tipoPagamento) {

        if (tipoPagamento && $scope.campanhaVendas && $scope.campanhaVendas.TIPO_PAGAMENTO_CAMPANHA_VENDA) {

            if ($scope.verificaDuplicacaoTipoPagamento(tipoPagamento)) {

                $scope.campanhaVendas.TIPO_PAGAMENTO_CAMPANHA_VENDA.push({ TIPO_PAGAMENTO: angular.copy(tipoPagamento), TPG_ID: tipoPagamento.TPG_ID });
            }
            else {

                $scope.message = Util.createMessage("fail", "O tipo de pagamento já existe!");
            }

        }
    }

    $scope.verificaDuplicacaoTipoPagamento = function (item) {

        var achou = true;
        if (item && $scope.campanhaVendas && $scope.campanhaVendas.TIPO_PAGAMENTO_CAMPANHA_VENDA) {

            angular.forEach($scope.campanhaVendas.TIPO_PAGAMENTO_CAMPANHA_VENDA, function (value, index) {

                if (value.TIPO_PAGAMENTO.TPG_ID === item.TPG_ID) {

                    achou = false;
                    return;
                }
            });
        }

        return achou;
    }

    $scope.$watch("campanhaVendas.TIPO_PAGAMENTO_CAMPANHA_VENDA", function (value, old) {

        if (value) {
            $scope.acharTipoPagamentoAdicionado(value);
        }

    }, true);

    $scope.inicializarTipoPagamento = function () {

        angular.forEach($scope.lstTipoPagamento, function (value, old) {

            value.show = true;
        });

    }

    $scope.acharTipoPagamentoAdicionado = function (tiposPagamentos) {

        $scope.inicializarTipoPagamento();

        if (tiposPagamentos) {

            angular.forEach($scope.lstTipoPagamento, function (value, old) {

                angular.forEach(tiposPagamentos, function (subValue, subOld) {

                    if (subValue.TIPO_PAGAMENTO && value.TPG_ID == subValue.TIPO_PAGAMENTO.TPG_ID) {

                        value.show = false;
                    }
                });
            });
        }

    }

    $scope.excluirTipoPagamento = function (index) {

        if ($scope.campanhaVendas && $scope.campanhaVendas.TIPO_PAGAMENTO_CAMPANHA_VENDA) {

            $scope.campanhaVendas.TIPO_PAGAMENTO_CAMPANHA_VENDA.splice(index, 1);
        }
    }

    $scope.pausarOuAtivarCampanhaVenda = function (cveId, ativa) {

        var semantica = (ativa == true) ? "pausar" : "ativar";
        if (confirm("Deseja " + semantica + " a campanha de vendas?")) {

            $scope.campanhaRequest = { cveId: cveId };
            formHandlerService.submit($scope, {
                url: Util.getUrl("/campanhaVenda/pausarOuAtivarCampanhaVenda"),
                objectName: 'campanhaRequest',
                showAjaxLoader: true,
                success: function (resp, status, config, message, validationMessage) {

                    $scope.message = message;
                    $scope.erros = validationMessage;   

                    $scope.button = 'reset';
                    if (resp.success) {

                        $timeout(function () {
                            $scope.message = null;
                            $scope.pesquisarCampanhaVenda();

                        }, 1000);

                    }
                }
            });
        }
    }

    $scope.validarDesconto = function () {

        if ($scope.campanhaVendas.CVE_DESCONTO_MAX &&
            $scope.campanhaVendas.CVE_ACRESCIMO_MINIMO) {

            $scope.message = Util.createMessage('warning',
                'O desconto máximo é subtraido do acréscimo mínimo.' +
                ' Na maioria dos casos, não é necessário preencher os dois campos. Considere ajustar só o acréscimo mínimo.');


        }
    };

    $scope.excluirCampanhaVenda = function (cveId) {

        if (confirm("Deseja realmente excluir?")) {

            $scope.campanhaRequest = { cveId: cveId };
            formHandlerService.submit($scope, {
                url: Util.getUrl("/campanhaVenda/excluirCampanhaVenda"),
                objectName: 'campanhaRequest',
                showAjaxLoader: true,
                success: function (resp, status, config, message, validationMessage) {

                    $scope.message = message;
                    $scope.erros = validationMessage;

                    $scope.button = 'reset';
                    if (resp.success) {

                        $timeout(function () {
                            $scope.message = null;
                            $scope.pesquisarCampanhaVenda();

                        }, 1000);

                    }
                }
            });
        }
    }

    $scope.abrirModalProduto = function () {

        $scope.filtro = {};
        $scope.listarProdutoComposicao();
        angular.element("#modal-produto").modal();

    };

    $scope.listarProdutoComposicao = function (pageRequest) {

        $scope.listado = false;
        var url = Util.getUrl("/produtoComposicao/ListarProdutosPorUen");

        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }

        var config = {
            url: url,
            targetObjectName: 'lstProdutoComposicao',
            responseModelName: 'produtosComposicao',
            showAjaxLoader: true,
            pageConfig: { pageName: 'page' },
            success: function () {

                $scope.listado = true;
            }
        };

        if ($scope.filtro) {

            config.data = angular.copy($scope.filtro);
            config.data.uenId = $scope.uen.UEN_ID;
        }
        formHandlerService.read($scope, config);
    };

    $scope.adicionarProduto = function (item) {
        
        if (item && $scope.campanhaVendas && $scope.campanhaVendas.CAMPANHA_VENDAS_PRODUTO_COMPOSICAO) {

            if ($scope.verificaDuplicacaoProdutoComposicao(item)) {

                $scope.campanhaVendas.CAMPANHA_VENDAS_PRODUTO_COMPOSICAO.push({ PRODUTO_COMPOSICAO: angular.copy(item), CMP_ID: item.CMP_ID });

                angular.element("#modal-curso").modal('hide');
                angular.element("#modal-produto").modal('hide');
            }
            else {

                $scope.message = Util.createMessage("fail", "O produto já foi adicionado!");
            }

        }

        
    };
        $scope.verificaDuplicacaoProdutoComposicao = function (item) {

            var achou = true;
            if (item && $scope.campanhaVendas && $scope.campanhaVendas.CAMPANHA_VENDAS_PRODUTO_COMPOSICAO) {

                angular.forEach($scope.campanhaVendas.CAMPANHA_VENDAS_PRODUTO_COMPOSICAO, function (value, index) {

                    if (value.PRODUTO_COMPOSICAO.CMP_ID === item.CMP_ID) {

                        achou = false;
                        return;
                    }
                });
            }

            return achou;
        };

    $scope.carregarEmpresas = function () {

        var url = Util.getUrl("/proposta/listarEmpresas");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstEmpresas',
            responseModelName: 'lstEmpresas',
            showAjaxLoader: true,
            success: function () {

                $scope.exibirFiltroEmpresa = true;
            }

        });
    };

        $scope.deletarProdutoComposicao = function (index) {

            if ($scope.campanhaVendas.CAMPANHA_VENDAS_PRODUTO_COMPOSICAO && index != undefined && index != null) {

                $scope.campanhaVendas.CAMPANHA_VENDAS_PRODUTO_COMPOSICAO.splice(index, 1);
            }
        };
        
    if (window.ClienteInadimplenteController !== undefined) {

        ClienteInadimplenteController($scope, formHandlerService);
    }
}]);