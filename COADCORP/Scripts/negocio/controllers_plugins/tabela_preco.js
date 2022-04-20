

function TabelaPrecoController($scope, formHandlerService, $http, conversionService, comparatorService, $timeout) {

    $scope.acaoEdit = { label: 'Incluir', index: null };
    $scope.objControle = {};
    //$scope.buttonSaveTabPreco = { label: 'Salvar', show: true };
    $scope.objSalvar = {TABELA_PRECO_ATUALIZAR : [], TABELA_PRECO_EXCLUSAO : []};

    $scope.readTabelaPreco = function (CMP_ID) {

        if (CMP_ID) {

            $scope.objControle.CMP_ID = CMP_ID;           
        }
        else {

            CMP_ID = $scope.objControle.CMP_ID;
        }

        var url = Util.getUrl("/tabelapreco/readtabelapreco");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'tabelaPreco',
            responseModelName: 'tabelaPreco',
            showAjaxLoader: true,
            data: { CMP_ID: CMP_ID },
            success: function (response) {

                $scope.ehCurso = response.result.ehCurso;
                $scope.getListaTipoPeriodoProduto();
            }
        });
        
    };

    $scope.query = { show: true };
    $scope.queryTipoPagamento = { show: true };


    $scope.inicializarRegioes = function () {

        angular.forEach($scope.lstRegioes, function (value, old) {

            value.show = true;
        });
    }
    
    $scope.inicializarTipoPagamento = function () {

        angular.forEach($scope.lstTipoPagamento, function (value, old) {

            value.show = true;
        });
    }


    $scope.carregaCombos = function () {

        var url = Util.getUrl("/tabelapreco/getcombostabelapreco");

        $http({
            url: url,
            method: "post"
        }).success(function (retorno) {
            $scope.lstCondicaoPagamento = retorno.result.lstCondicaoPagamento;
            $scope.lstTipoPagamento = retorno.result.lstTipoPagamento;
            $scope.lstTipoPagamentoSimples = retorno.result.lstTipoPagamentoSimples;
            $scope.lstTipoPagamentoComposto = $scope.lstTipoPagamento;
            $scope.lstRegioes = retorno.result.lstRegioes;            

        });
      
    };    

    $scope.abrirEdicaoItemTabelaPreco = function($index, item){


        item = (item) ? item : {
            REGIAO_TABELA_PRECO: [],
            TABELA_PRECO_TIPO_PAGAMENTO: [],
            };
                

        if (!item.CMP_ID) {

            item.CMP_ID = ($scope.objControle.CMP_ID) ? ($scope.objControle.CMP_ID) : null;
        }

        item.TP_PRECO_VENDAMask
        $index = ($index == 0 || $index) ? $index : null;
         
        if ($index === null) {

            if ($scope.ehCurso) {

                item.TP_PORCENTAGEM_SERVICO = 70;
            }
            $scope.inicializarRegioes();
            $scope.acaoEdit.label = "Incluir";
            item.Novo = true;
            $scope.lstTipoPagamento = $scope.lstTipoPagamentoComposto;
        }
        else {

            $scope.tipoPeriodoSelecionado(item);
            item.Novo = false;
            $scope.acaoEdit.label = "Alterar";
        }

        $scope.acaoEdit.index = $index;

        $scope.itemTabPreco = {}; //
        $scope.itemTabPreco = angular.copy(item);

        if (item.REGIAO_TABELA_PRECO) {

            angular.forEach(item.REGIAO_TABELA_PRECO, function (value, index) {

                //value.RG_TP_PRECO_VENDAMask = value.RG_TP_PRECO_VENDA;
            });
        }
        angular.element("#modal-tabela-preco").modal();
    }

    $scope.salvarEdicaoTabelaPreco = function () {

        if ($scope.itemTabPreco) {

            if ($scope.acaoEdit.label == "Incluir") {

                $scope.acaoEdit.label = 'Incluindo...';
            }
            else {
                $scope.acaoEdit.label = 'Alterando...';
            }

            // limpa a referência da tabela de preço
            if ($scope.itemTabPreco.REGIAO_TABELA_PRECO) {

                angular.forEach($scope.itemTabPreco.REGIAO_TABELA_PRECO, function (value, old) {

                    value.TABELA_PRECO = null;
                });
            }

            var index = $scope.acaoEdit.index;

            $scope.itemTabPreco.PRODUTO_COMPOSICAO = null;
            formHandlerService.submit($scope, {
                url: Util.getUrl("/tabelapreco/validartabelapreco"),
                objectName: 'itemTabPreco',
                showAjaxLoader: true,
                success: function (resp, status, config, message, validationMessage) {
                    $scope.message = message;
                    $scope.erros = validationMessage;

                    if (resp.success) {

                        $scope.message = message;
                        if (index == 0 || index) {

                            $scope.tabelaPreco[index] = angular.copy($scope.itemTabPreco);
                        }
                        else {
                            var obj = angular.copy($scope.itemTabPreco);
                            $scope.tabelaPreco.push(obj);
                        }

                        angular.element("#modal-tabela-preco").modal('hide');

                    }
                    else {

                        if ($scope.acaoEdit.label == "Incluindo...") {

                            $scope.acaoEdit.label = 'Incluir';
                        }
                        else {
                            $scope.acaoEdit.label = 'Alterar';
                        }
                    }


                },
                fail: function () {

                    if ($scope.acaoEdit.label == "Incluindo...") {

                        $scope.acaoEdit.label = 'Incluir';
                    }
                    else {
                        $scope.acaoEdit.label = 'Alterar';
                    }

                }
            });
            
        }
               
    }

   
    $scope.tipoPagamentoSelecionado = function () {

        if ($scope.itemTabPreco && $scope.itemTabPreco.TIPO_PAGAMENTO) {

            $scope.itemTabPreco.TPG_ID = $scope.itemTabPreco.TIPO_PAGAMENTO.TPG_ID;
        }
        else {
            $scope.itemTabPreco.TPG_ID = null;
        }
    }

    $scope.adicionarRegiao = function (reg) {

        if (reg && $scope.itemTabPreco && $scope.itemTabPreco.REGIAO_TABELA_PRECO) {

            if ($scope.verificaDuplicacaoRegiao(reg)) {

                var precoBase = $scope.itemTabPreco.TP_PRECO_VENDAMask;
                $scope.itemTabPreco.REGIAO_TABELA_PRECO.push({ REGIAO: angular.copy(reg), RTP_PRECO_VENDAMask: precoBase });
            }
            else {

                $scope.message = Util.createMessage("fail", "Região já adicionada!");
            }
            
        }
    }

    $scope.verificaDuplicacaoRegiao = function (reg) {

        var podeAdicionar = true;
        if (reg && $scope.itemTabPreco && $scope.itemTabPreco.REGIAO_TABELA_PRECO) {

            angular.forEach($scope.itemTabPreco.REGIAO_TABELA_PRECO, function (value, index) {

                if (value.RG_ID === reg.RG_ID) {

                    podeAdicionar = false;
                }
            });
        }

        return podeAdicionar;
    }

    $scope.$watch("itemTabPreco.REGIAO_TABELA_PRECO", function (value, old) {

        if (value) {
            $scope.acharRegiaoAdicionada(value);
        }
        
    }, true);

    $scope.acharRegiaoAdicionada = function (regiaoTabelaPreco) {

        $scope.inicializarRegioes();
        if (regiaoTabelaPreco) {

            angular.forEach($scope.lstRegioes, function (value, old) {

                angular.forEach(regiaoTabelaPreco, function (subValue, subOld) {

                    if (subValue.REGIAO && value.RG_ID == subValue.REGIAO.RG_ID) {

                        value.show = false;
                    }
                });
            });
        }
        
    }

    $scope.excluirRegiao = function (index, descricao) {

        if ($scope.itemTabPreco && $scope.itemTabPreco.REGIAO_TABELA_PRECO) {

            $scope.itemTabPreco.REGIAO_TABELA_PRECO.splice(index, 1);       
            
        }
    }

    $scope.salvarTabelaPreco = function (curso) {

        $scope.buttonSaveTabPreco = { label: 'Salvando...', show : false };       
        $scope.objSalvar.TABELA_PRECO_ATUALIZAR = $scope.tabelaPreco;

        formHandlerService.submit($scope, {
            url: Util.getUrl("/tabelapreco/salvar"),
            objectName: 'objSalvar',
            showAjaxLoader: true,
            success: function (resp, status, config, message, validationMessage) {
                $scope.message = message;
                $scope.erros = validationMessage;
               
                if (resp.success) {

                    $scope.message = message;

                    $scope.buttonSaveTabPreco = null; //{ label: 'Salvo', show: false };
                    $timeout(function () {

                        if ($scope.tipoComposicao == 'curso') {
                            window.open(Util.getUrl('/franquia/curso/index'), '_self');
                        } else {
                            window.open(Util.getUrl('/produtocomposicao/index'), '_self');
                        }
                        
                    }, 1000);
                }
                else {
                    $scope.buttonSaveTabPreco = null;//{ label: 'Salvar', show: true };
                }
            },
            fail: function () {
                $scope.buttonSaveTabPreco = null; //{ label: 'Salvar', show: true };
            }
        });
    }


    $scope.verificaDuplicacao = function () {

        var index = $scope.acaoEdit.index;

        if ($scope.itemTabPreco.CO_PG_ID && $scope.itemTabPreco.TPG_ID) {

            if (comparatorService.checkDuplication($scope.tabelaPreco, $scope.itemTabPreco, index, ['CO_PG_ID', 'TPG_ID'])) {

                $scope.message = Util.createMessage("fail", "Já existe uma configuração com a condição de pagamento e o tipo de pagamento selecionado.");
                $scope.itemTabPreco.TPG_ID = null;
                $scope.itemTabPreco.TIPO_PAGAMENTO = null;
            };
        }
        


    }

    $scope.DeletarTabelaPreco = function ($index) {

        if ($scope.tabelaPreco) {

            var obj = angular.copy($scope.tabelaPreco[$index]);
            $scope.tabelaPreco.splice($index, 1);

            $scope.objSalvar.TABELA_PRECO_EXCLUSAO.push(obj);
        }
    };



    $scope.adicionarTipoPagamento = function (tpPagamento) {

        if (tpPagamento && $scope.itemTabPreco && $scope.itemTabPreco.TABELA_PRECO_TIPO_PAGAMENTO) {

            if ($scope.verificaDuplicacaoTipoPagamento(tpPagamento)) {

                $scope.itemTabPreco.TABELA_PRECO_TIPO_PAGAMENTO.push({ TIPO_PAGAMENTO: angular.copy(tpPagamento), TPG_ID: tpPagamento.TPG_ID });
            }
            else {

                $scope.message = Util.createMessage("fail", "Forma de Pagamento já adicionada!");
            }
        }
    }

    $scope.verificaDuplicacaoTipoPagamento = function (tpPagamento) {

        var podeAdicionar = true;
        if (tpPagamento && $scope.itemTabPreco && $scope.itemTabPreco.TABELA_PRECO_TIPO_PAGAMENTO) {

            angular.forEach($scope.itemTabPreco.TABELA_PRECO_TIPO_PAGAMENTO, function (value, index) {

                if (value.TPG_ID === tpPagamento.TPG_ID) {
                    podeAdicionar = false;
                }
            });
        }

        return podeAdicionar;
    }

    $scope.$watch("itemTabPreco.TABELA_PRECO_TIPO_PAGAMENTO", function (value, old) {

        if (value) {
            $scope.acharTabelaTipoPagamento(value);
        }

    }, true);

    $scope.acharTabelaTipoPagamento = function (tipoPagamentoTabelaPreco) {

        $scope.inicializarTipoPagamento();
        if (tipoPagamentoTabelaPreco) {

            angular.forEach($scope.lstTipoPagamento, function (value, old) {

                angular.forEach(tipoPagamentoTabelaPreco, function (subValue, subOld) {

                    if (subValue.TIPO_PAGAMENTO && value.TPG_ID == subValue.TIPO_PAGAMENTO.TPG_ID) {

                        value.show = false;
                    }
                });
            });
        }

    }

    $scope.excluirTipoPagamento = function (index, descricao) {

        if ($scope.itemTabPreco && $scope.itemTabPreco.TABELA_PRECO_TIPO_PAGAMENTO) {

            $scope.itemTabPreco.TABELA_PRECO_TIPO_PAGAMENTO.splice(index, 1);

        }
    }

    $scope.bloquearParcelas = function (tipoPeriodoSelecionado) {

        if (tipoPeriodoSelecionado) {

            tipoPeriodoSelecionado.parcelaBloqueada = true;
            tipoPeriodoSelecionado.TP_NUM_PARCELAS_MAX = 1;
            tipoPeriodoSelecionado.TP_NUM_PARCELAS_MIN = 1;
        }
    };

    $scope.desbloquearParcelas = function (tipoPeriodoSelecionado) {

        if (tipoPeriodoSelecionado) {

            tipoPeriodoSelecionado.parcelaBloqueada = false;
        }
    };
    
    $scope.tipoPeriodoSelecionado = function (item) {

        if (item) {

            $scope.itemTabPrecoSelecionado = item;
        }
        else {
            $scope.itemTabPrecoSelecionado = $scope.itemTabPreco;
        }

        if ($scope.itemTabPrecoSelecionado != null && $scope.itemTabPrecoSelecionado.TIPO_PERIODO) {

            var tipoPeriodo = $scope.itemTabPrecoSelecionado.TIPO_PERIODO;
            $scope.itemTabPrecoSelecionado.TTP_ID = $scope.itemTabPrecoSelecionado.TIPO_PERIODO.TTP_ID;

            if (tipoPeriodo.TTP_RECORRENTE == true) {

                $scope.bloquearParcelas($scope.itemTabPrecoSelecionado);
                $scope.lstTipoPagamento = $scope.lstTipoPagamentoSimples;

            }
            else {
                $scope.desbloquearParcelas($scope.itemTabPrecoSelecionado);
                $scope.lstTipoPagamento = $scope.lstTipoPagamentoComposto;
            }

            if ($scope.itemTabPrecoSelecionado && $scope.itemTabPrecoSelecionado.TABELA_PRECO_TIPO_PAGAMENTO) {

                if (!item) {

                    $scope.itemTabPrecoSelecionado.TABELA_PRECO_TIPO_PAGAMENTO = [];
                }
                
                $scope.acharTabelaTipoPagamento($scope.itemTabPrecoSelecionado.TABELA_PRECO_TIPO_PAGAMENTO);
             }
        }
        else {
            $scope.desbloquearParcelas($scope.itemTabPrecoSelecionado);
            $scope.itemTabPrecoSelecionado.TTP_ID = null;
        }
    }

    $scope.getListaTipoPeriodoProduto = function () {

        var cmpId = $scope.objControle.CMP_ID;

        formHandlerService.read($scope, {
            url: Util.getUrl("/produtoComposicao/listarTipoPeriodoDoProdutoComposto"),
            targetObjectName: 'lstTipoPeriodoDoProduto',
            responseModelName: 'lstTipoPeriodoDoProduto',
            data: {CMP_ID : cmpId},
            success: function () {

            }
        });
    }
    //---------- inicializações --------------------------------



    $scope.carregaCombos();





};
