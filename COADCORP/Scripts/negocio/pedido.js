
appModule.controller("PedidoController", ['$scope', '$http', 'formHandlerService', 'MathService',
    '$timeout', 'UploadAjax', 'Upload', 'FilterService',
    'conversionService', '$interval', 'UenService','$window',
    function ($scope, $http, formHandlerService, MathService, $timeout, UploadAjax, Upload, FilterService, conversionService, $interval, UenService, $window) {
       
    $scope.init = function (CLI_ID, uenId) {

        $scope.passo = 1;
        $scope.carregaCombos();
        
        $scope.pedido = {
            PED_CRM_VENDA_INFORMADA : false,
            PEDIDO_PAGAMENTO: [{}],
            ITEM_PEDIDO: [],
            UEN_ID: uenId,
            PED_CEM_POR_CENTO_FATURADO: false,
            PED_EMPRESA_DO_SIMPLES: false
        };

        if (CLI_ID != null) {

            $scope.pedido.CLI_ID = CLI_ID;
            $scope._carregarDadosDoCliente(CLI_ID);
        }

    };

    $scope.initListagem = function (ipeId, pedCrmId, isFranquiado) {

        $scope.ehFranquiado = (isFranquiado == true || isFranquiado == "True");
        $scope.listado = false;
        $scope.carregarPedidoStatus();
        $scope.criarFiltros();
        $scope.obterGruposDeFiltroDoPedido();
        $scope.carregaCombos();
        $scope.carregarTipoNegociacao();
        $scope.filtro = {
            IPE_ID: (ipeId) ? ipeId : null,
            PED_CRM_ID : (pedCrmId) ? pedCrmId : null
        }
        if (ipeId || pedCrmId)
            $scope.dispararPesquisa = true;

        $scope.retornarUenAtual(function () {

            $scope.$watch('uen', function () {

                if ($scope.listado === true)
                    $scope.listarPedidos();

                $scope.carregaCombos();
            });
        });

    }

    $scope.initDetalhes = function (PED_CRM_ID) {

        if (PED_CRM_ID) {

            $scope.carregarDadosDoPedido(PED_CRM_ID);
        }
    }

    $scope.listarPedidos = function (pagina) {

       
        $scope.paginaAtual = pagina;
        var url = Util.getUrl("/pedido/ListarPedidos");

        if (pagina) {

            url += "?pagina=" + pagina;
        }
        $scope.listado = true;
        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstPedidos',
            responseModelName: 'lstPedidos',
            showAjaxLoader: true,
            data: $scope.filtro,
            pageConfig: { pageName: 'page' /*, pageTargetName: 'pageAgendamentoDoDia' */ },
            success: function (resp) {
                if (Util.isPathValid($scope, 'notaBatchModal')) {

                    $scope.notaBatchModal.selecionarTodos = false;
                    angular.element("#selecao-geral").removeAttr('selected');
                }

            }
        });
    };

    $scope._carregarDadosDoCliente = function (CLI_ID) {

        if (CLI_ID) {

            var url = Util.getUrl("/franquia/clientes/RecuperarDadosDoClienteParaConfiguracao");

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'pedido.INFO_CLIENTE',
                responseModelName: 'cliente',
                data: { clienteId: CLI_ID },
                showLoader: true,
                success: function (response) {
                
                }
            });
        }
    }

    $scope.carregarProdutosInteresse = function (nome) {

        var parans = {};

        if (nome) {
            parans.nome = nome;
        }
        var url = Util.getUrl("/produtocomposicao/ListarProdutosDeInteresse");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: "produtosComposicao",
            responseModelName: "produtosComposicao",
            data: parans,
            showAjaxLoader: false,
            success: function (resp) {
            }
        });
    }

    $scope.carregaCombos = function () {

        var url = Util.getUrl("/pedido/getCombos");

        $http({
            url: url,
            method: "post"
        }).success(function (retorno) {
            $scope.lstCondicaoPagamento = retorno.result.lstCondicaoPagamento;
            $scope.lstTipoPagamento = retorno.result.lstTipoPagamento;
            $scope.lstRegioes = retorno.result.lstRegioes;
            $scope.lstBandeiraCartao = retorno.result.lstBandeiraCartao;
            $scope.lstUF = retorno.result.lstUF;
            $scope.lstBancos = retorno.result.lstBancos;

            FilterService.adicionarDadoCombo($scope.filtros, $scope.lstRegioes, "RG_ID")

        });

    };

    $scope.tipoPagamentoSelecionado = function (item) {

        if (item && item.TIPO_PAGAMENTO) {

            item.TPG_ID = item.TIPO_PAGAMENTO.TPG_ID;
        }
    }

    $scope.abreModalFormaDePagamento = function () {

        angular.element("#modal-tabela-preco").modal();
    }



    //$scope.buscarRegiaoTabelaPreco = function () {

    //    var parans = {};

    //    if ($scope.pedido && $scope.pedido.CMP) {
    //        parans.CMP = $scope.pedido.CMP;
    //    }
    //    var url = Util.getUrl("/tabelaPreco/listarRegiaoTabelaPreco");

    //    formHandlerService.read($scope, {
    //        url: url,
    //        targetObjectName: "regiaoTabelaPreco",
    //        responseModelName: "regiaoTabelaPreco",
    //        data: parans,
    //        showAjaxLoader: false,
    //        success: function (resp) {
    //        }
    //    });
    //}

    $scope.listarCurso = function (pageRequest) {

        $scope.listado = false;

        var url = Util.getUrl("/curso/ListarCursos");

        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }

        var config = {
            url: url,
            targetObjectName: 'lstCursos',
            responseModelName: 'lstCursos',
            showAjaxLoader: true,
            pageConfig: { pageName: 'page' },
            success: function () {

                $scope.listado = true;
            }
        };

        if ($scope.filtro) {

            config.data = angular.copy($scope.filtro);
        }
        formHandlerService.read($scope, config);
    };

    $scope.listarProdutoComposicao = function (pageRequest) {

        $scope.listado = false;

        var url = Util.getUrl("/produtoComposicao/listarProdutosExcetoCursos");

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
        }
        formHandlerService.read($scope, config);
    };

    $scope.abrirModalCurso = function () {
        $scope.listarCurso();
        angular.element("#modal-curso").modal();
    }

    $scope.abrirModalProdutoComposicao = function () {
        $scope.listarProdutoComposicao();
        angular.element("#modal-produto-composicao").modal();
    }

    $scope.adicionarProduto = function (item) {

        $scope.limparPedidoPagamento();
        $scope.limparParticipantes();
        $scope.passo = 2;

        if (item != null && Util.isPathValid($scope, "pedido.ITEM_PEDIDO")) {

            var itemPedido = {

                PRODUTO_COMPOSICAO: angular.copy(item),
                CMP_ID : item.CMP_ID,
                IPE_QTD: item.QTD,
                ITEM_PEDIDO_PEDIDO_PAGAMENTO: [],
                UEN_ID: $scope.pedido.UEN_ID
            };

            $scope.pedido.ITEM_PEDIDO.push(itemPedido);
        }


        angular.element("#modal-curso").modal('hide');
        angular.element("#modal-produto-composicao").modal('hide');
    }

    $scope.deletarItemProduto = function ($index) {

        if ($index != null && Util.isPathValid($scope, "pedido.ITEM_PEDIDO")) {
            
            $scope.pedido.ITEM_PEDIDO.splice($index, 1);
        }

        if ($scope.pedido.ITEM_PEDIDO.length <= 0) {

            $scope.passo = 1;
        }
    }

    $scope.buscarTabelaDePreco = function (CMP_ID) {

        var url = Util.getUrl("/tabelapreco/listarRegiaoTabelaPreco");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'regiaoTabelaPreco',
            responseModelName: 'regiaoTabelaPreco',
            showAjaxLoader: true,
            data: { CMP_ID: CMP_ID }
        });

    };

    $scope.listarResumoParcelamento = function (CMP_ID, TPG_ID, QTD, TTP_ID) {

        var url = Util.getUrl("/tabelapreco/listarResumoDeParcelamento");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'regiaoResumoParcelas',
            responseModelName: 'regiaoResumoParcelas',
            showAjaxLoader: true,
            data: {
                CMP_ID: CMP_ID,
                TPG_ID: TPG_ID,
                QTD: QTD,
                TTP_ID : TTP_ID
            }
        });

    };

    $scope.abrirModalTabelaPreco = function (CMP_ID, itemSelecionado) {

        $scope.regiaoResumoParcelas = null;
        $scope.tipoPagamentoSelecionado = null;

        $scope.itemSelecionado = itemSelecionado;
        $scope.produtoSelecionado = CMP_ID;
        $scope.listarTipoPeridoDoProduto(CMP_ID);

        angular.element("#modal-tabela-preco").modal();
    }

    $scope.abrirSubModalParticipantes = function (IPE_ID) {

        $scope.lstPedidoParticipante = null;

        $scope.listPedidoParticipanteByItemPedido(IPE_ID);
        angular.element("#modal-participante").modal();
    }

    $scope.tipoPagamentoTabelaPrecoSelecionado = function () {

        if ($scope.produtoSelecionado && Util.isPathValid($scope,"tipoPagamentoSelecionado.TPG_ID")) {

            var CMP_ID = $scope.produtoSelecionado;
            var TPG_ID = $scope.tipoPagamentoSelecionado.TPG_ID;
            var QTD = $scope.itemSelecionado.IPE_QTD;

            var TTP_ID = $scope.idTipoPeriodo;

            if (!$scope.itemSelecionado.PRODUTO_COMPOSICAO.EhCurso && $scope.idTipoPeriodo) {

                $scope.listarResumoParcelamento(CMP_ID, TPG_ID, QTD, TTP_ID);
            }
            else {
                $scope.listarResumoParcelamento(CMP_ID, TPG_ID, QTD);
            }
        }
    }

    $scope.diaVencimentoChange = function (itemPedido) {

        var data = new Date();
        var dia = data.getDate();

    }

    $scope.$watch('pedido.ITEM_PEDIDO', function (lstValue, old) {

        if (lstValue) {

            var existeAlgumRecorrente = false;

            angular.forEach(lstValue, function (value, index) {

                var tipoPeriodo = value.TIPO_PERIODO;
                if (tipoPeriodo && existeAlgumRecorrente == false) {

                    existeAlgumRecorrente = value.TIPO_PERIODO.TTP_RECORRENTE;
                    if (existeAlgumRecorrente == true && !value.IPE_DIA_VENCIMENTO_VENDA_RECORRENTE) {

                        var data = new Date();
                        data.setDate(data.getDate() + 2);
                        value.IPE_DIA_VENCIMENTO_VENDA_RECORRENTE = data.getDate();
                    }
                }

            });

            $scope.existeAlgumRecorrente = existeAlgumRecorrente;
        }


    }, true);

    $scope.adicionarTabelaDePrecoNoItemPedido = function (obj) {

        if (Util.isPathValid($scope, "obj.REGIAO_TABELA_PRECO") != null && $scope.itemSelecionado)
        {

            $scope.passo = 2;
            var detalheParcela = angular.copy(obj);
            $scope.itemSelecionado.IPE_DESCONTO = null;
            $scope.itemSelecionado.IPE_PARCELA = detalheParcela.Parcela;
            $scope.itemSelecionado.IPE_VALOR_PARCELA = detalheParcela.ValorParcela;
            $scope.itemSelecionado.IPE_TOTAL = detalheParcela.Total;
            $scope.itemSelecionado.IPE_PRECO_UNITARIO = detalheParcela.PrecoUnitario;
            $scope.itemSelecionado.TIPO_PAGAMENTO = detalheParcela.TIPO_PAGAMENTO;
            $scope.itemSelecionado.REGIAO_TABELA_PRECO = detalheParcela.REGIAO_TABELA_PRECO;
            $scope.itemSelecionado.TIPO_PERIODO = detalheParcela.TIPO_PERIODO;
            $scope.itemSelecionado.IPE_PRIMEIRA_PARCELA_CORTERIA = (detalheParcela.primeiroMesGratis == true) ? detalheParcela.primeiroMesGratis : false;

            $scope.itemSelecionado.TotalOriginal = $scope.itemSelecionado.IPE_TOTAL;
            $scope.itemSelecionado.IPE_TOTAL_SEM_IMPOSTO = $scope.itemSelecionado.IPE_TOTAL;
            $scope.calcularImpostoDoPedido();
        }
        angular.element("#modal-tabela-preco").modal("hide");
    }

    $scope.limparPedidoPagamento = function (passo) {

        if (Util.isPathValid($scope, "pedido.ITEM_PEDIDO")) {
            // limpar a lista

            if (passo) {
                $scope.passo = passo;
            }
            angular.forEach($scope.pedido.ITEM_PEDIDO, function (value, index) {

                value.ITEM_PEDIDO_PEDIDO_PAGAMENTO = [];
            });
        }
    }
    
    $scope.limparParticipantes = function (passo) {

        if (Util.isPathValid($scope, "pedido.ITEM_PEDIDO")) {
            // limpar a lista

            if (passo) {
                $scope.passo = passo;
            }
            angular.forEach($scope.pedido.ITEM_PEDIDO, function (value, index) {

                value.PEDIDO_PARTICIPANTE = [];
            });
        }
    }
    $scope.gerarFormaDePagamentos = function () {

        if($scope.validarVenda() && Util.isPathValid($scope, "pedido.ITEM_PEDIDO")){

            $scope.limparPedidoPagamento();
            $scope.passo = 3;
            $scope.parcelaUnica = true;
            angular.forEach($scope.pedido.ITEM_PEDIDO, function (value, index) {
                
                var tipoPagamento = { TPG_ID: null };
                value.parcelaUnica = true;

                if (Util.isPathValid(value, "TIPO_PAGAMENTO.TPG_ID")) {

                    tipoPagamento = angular.copy(value.TIPO_PAGAMENTO);
                    var parcelas = value.IPE_PARCELA;
                    
                    if (tipoPagamento.TPG_TIPO == 1) {

                        var lstTipoPagamento = tipoPagamento.ListaTipoPagamento;

                        angular.forEach(lstTipoPagamento, function (valueTipoPagamento, index) {
                                
                            var parcelaAtual = null;

                            if (index == 0) {

                                parcelaAtual = 1;
                                $scope.criarItemPedidoPedidoPagamento(value, valueTipoPagamento, parcelaAtual, true);
                            }
                            else {

                                parcelaAtual = parcelas - 1;

                                if (parcelaAtual > 0) {
                                    value.parcelaUnica = false;
                                    $scope.criarItemPedidoPedidoPagamento(value, valueTipoPagamento, parcelaAtual, false);
                                }
                            }

                        });
                    }
                    else {
                        $scope.criarItemPedidoPedidoPagamento(value, tipoPagamento, parcelas, true);
                    }
                }

                $scope.recalcularParcelas(value);
            });            
        }
    }

    $scope.gerarFormParticipantes = function () {

        if ($scope.validarVenda() && Util.isPathValid($scope, "pedido.ITEM_PEDIDO")) {

            $scope.limparParticipantes();
            $scope.limparPedidoPagamento();
            $scope.passo = 3;
            angular.forEach($scope.pedido.ITEM_PEDIDO, function (value, index) {
                
                if (value.PRODUTO_COMPOSICAO && value.PRODUTO_COMPOSICAO.EhCurso === true) {
                    if (Util.isPathValid(value, "IPE_QTD")) {

                        var index2 = value.IPE_QTD;
                        var lstParticipantes = [];

                        for (var i = 0; i < index2; i++) {

                            lstParticipantes.push({PED_EH_O_COMPRADOR : false});
                        }

                        value.PEDIDO_PARTICIPANTE = lstParticipantes;
                    }
                }
            });
        }
    }


    $scope.criarItemPedidoPedidoPagamento = function (itemPedido, tipoPagamento, parcelas, entrada) {


        if (itemPedido && tipoPagamento && parcelas > 0) {
            //var parcelas = itemPedido.DETALHE_PARCELA.Parcela;
            var valorParcelas = itemPedido.IPE_VALOR_PARCELA;
            //var total = MathService.CalcularTotal(valorParcelas, parcelas);
            
            var itemPedidoPedidoPagamento = {

                PEDIDO_PAGAMENTO: {
                    ITEM_PEDIDO: angular.copy(itemPedido),
                    TPG_ID: tipoPagamento.TPG_ID,
                    TIPO_PAGAMENTO: tipoPagamento,
                    PGT_QTDE_PARCELAS: parcelas,
                    PGT_VLR_PARCELA: valorParcelas,
                    //PGT_VLR_TOTAL: total 

                },
                DATA_ASSOCIACAO: new Date(),
            };

            var dataVencimento = new Date();
            dataVencimento.setDate(dataVencimento.getDate() + 2);

            if (entrada === true) {

                itemPedidoPedidoPagamento.PEDIDO_PAGAMENTO.PGT_ENTRADA = true;
                itemPedidoPedidoPagamento.PEDIDO_PAGAMENTO.PGT_PAGO = false;
                itemPedidoPedidoPagamento.PEDIDO_PAGAMENTO.PGT_DATA_VENCIMENTO = dataVencimento;
            }
            else {

                dataVencimentoParcela2 = dataVencimento;
                dataVencimentoParcela2.setMonth(dataVencimento.getMonth() + 1);

                itemPedidoPedidoPagamento.PEDIDO_PAGAMENTO.PGT_ENTRADA = false;
                itemPedidoPedidoPagamento.PEDIDO_PAGAMENTO.PGT_PAGO = false;
                itemPedidoPedidoPagamento.PEDIDO_PAGAMENTO.PGT_DATA_VENCIMENTO = dataVencimentoParcela2;
            }
            itemPedido.ITEM_PEDIDO_PEDIDO_PAGAMENTO.push(itemPedidoPedidoPagamento);
        }
    }


    $scope.validarVenda = function () {

        var retorno = true;
        if ($scope.pedido) {

            if(Util.isPathValid($scope, 'pedido.ITEM_PEDIDO') && $scope.pedido.ITEM_PEDIDO.length > 0){

                angular.forEach($scope.pedido.ITEM_PEDIDO, function (value, index) {

                    if (!Util.isPathValid(value, 'REGIAO_TABELA_PRECO') || value.REGIAO_TABELA_PRECO.length <= 0) {

                        $scope.message = Util.createMessage('fail', 'O item ' + (index + 1) + ' não possui tabela de preço selecionada.');
                        retorno = false;
                    }
                });
            }
            else{
                $scope.message = Util.createMessage('fail', 'Adicione um produto.');
                retorno = false;
            }
        }
        return retorno;
    }

    $scope.calcularDesconto = function (itemPedido) {

        $scope.limparPedidoPagamento(2);
        if (itemPedido) {

            if (itemPedido.IPE_DESCONTO <= 1) {

                itemPedido.IPE_TOTAL = itemPedido.TotalOriginal;
            }

            if (itemPedido.IPE_TOTAL) {

                var total = itemPedido.TotalOriginal;
                var desconto = itemPedido.IPE_DESCONTO;
                var qtdParcelas = itemPedido.IPE_PARCELA;
                var resp = MathService.ProcessarDesconto(total, desconto, qtdParcelas);

                itemPedido.IPE_TOTAL = resp.total;
                itemPedido.IPE_TOTAL_SEM_IMPOSTO = resp.total;
                itemPedido.IPE_VALOR_PARCELA = resp.valorParcelas
            }
            $scope.calcularImpostosNosItens(itemPedido);
        }
    }

    $scope.exibirValorDescontando = function (preco, porcentagem) {

        var total = ((porcentagem / 100) * preco);
        
        return total;
    }

    $scope.emitirPedido = function () {

        formHandlerService.submit($scope, {
            url: Util.getUrl("/franquia/pedido/SalvarPedido"),
            objectName: 'pedido',
            showAjaxLoader: true,
            success: function (resp, status, config, message, validationMessage) {

                $scope.message = message;
                $scope.erros = validationMessage;

                $scope.buttonSave = 'reset';
                if (resp.success) {

                    $scope.message = Util.createMessage("success", "Pedido emitido com sucesso!");

                    $timeout(function () {
                        window.open(Util.getUrl('/pedido/index'), '_self');                        

                    }, 1000);
                    
                }

            }
        });
    }

    $scope.listarItemPedidoPorPedido = function () {

        if ($scope.pedidoCRM_ID) {
            var url = Util.getUrl("/pedido/listarItemPedidoPorPedido");

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'lstItemPedidoModal',
                responseModelName: 'lstItemPedido',
                showAjaxLoader: true,
                data: {
                    PED_CRM_ID: $scope.pedidoCRM_ID
                }
            });
        }

    };

    $scope.listarItensDoPedido = function (PED_CRM_ID) {

        $scope.lstItemPedidoModal = null;
        $scope.pedidoCRM_ID = PED_CRM_ID;
        $scope.listarItemPedidoPorPedido();
        angular.element("#modal-detalhes-pedido").modal();
    }

    $scope.carregarDadosDoPedido = function (PED_CRM_ID) {

        if (PED_CRM_ID) {
            var url = Util.getUrl("/pedido/CarregarDadosDoPedido");

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'pedido',
                responseModelName: 'pedido',
                showAjaxLoader: true,
                data: {
                    PED_CRM_ID: PED_CRM_ID
                }
            });
        }

    };

    $scope.cancelarItemPedido = function () {
        
        if (confirm("Deseja cancelar o pedido?")) {

            formHandlerService.submit($scope, {
                url: Util.getUrl("/pedido/CancelarItemPedido"),
                objectName: 'cancelamentoDTO',
                showAjaxLoader: true,
                success: function (resp, status, config, message, validationMessage) {

                    $scope.message = message;
                    $scope.erros = validationMessage;

                    $scope.buttonCancel = 'reset';
                    if (resp.success) {

                        $scope.message = Util.createMessage("success", "Pedido cancelado com sucesso!");

                        $timeout(function () {
                            angular.element("#modal-detalhes-pedido").modal('hide');
                            angular.element("#modal-opcoes-pagamento").modal('hide');
                            $scope.message = null;
                            $scope.recarregarListarPedidos();

                            angular.element("#modal-cancelamento-pedido").modal('hide');

                        }, 1000);

                    }

                }
            });
        }
        else {
            return false;
        }
    }

    $scope.cancelarPedido = function () {

        if (confirm("Deseja cancelar o pedido?")) {

            formHandlerService.submit($scope, {
                url: Util.getUrl("/pedido/CancelarPedido"),
                objectName: 'cancelamentoDTO',
                showAjaxLoader: true,
                success: function (resp, status, config, message, validationMessage) {

                    $scope.message = message;
                    $scope.erros = validationMessage;

                    $scope.buttonCancel = 'reset';
                    if (resp.success) {

                        $scope.message = Util.createMessage("success", "Pedido cancelado com sucesso!");

                        $timeout(function () {
                            angular.element("#modal-detalhes-pedido").modal('hide');
                            angular.element("#modal-opcoes-pagamento").modal('hide');
                            $scope.message = null;
                            $scope.recarregarListarPedidos();

                            angular.element("#modal-cancelamento-pedido").modal('hide');

                        }, 1000);

                    }

                }
            });
        }
        else {
            return false;
        }
    }

    $scope.cancelarPedidoComNotificacao = function (pedido){

        $scope.buttonCancel = 'reset';

        //$scope.cancelamentoDTO = $scope.gerarStatusPedidoCrmDTO(pedido);
        $scope.GerarDadosIniCancelamento(pedido.PED_CRM_ID);
        angular.element("#modal-cancelamento-pedido").modal();
    }

    $scope.alterarStatusPedidoParaPagoComPendenciaDeConferencia = function () {

        if (confirm("Deseja realmente alterar o status do pedido?")) {

            formHandlerService.submit($scope, {
                url: Util.getUrl("/pedido/AlterarStatusItemPedidoParaPagoComPendenciaDeConferencia"),
                objectName: 'statusPago',
                showAjaxLoader: true,
                success: function (resp, status, config, message, validationMessage) {

                    $scope.message = message;
                    $scope.erros = validationMessage;

                    $scope.buttonPaid = 'reset';
                    if (resp.success) {

                        $scope.message = Util.createMessage("success", "Status alterado com sucesso!");

                        $timeout(function () {
                            angular.element("#modal-marcar-pago").modal('hide');
                            angular.element("#modal-opcoes-pagamento").modal('hide');
                            $scope.message = null;
                            $scope.recarregarListarPedidos();
                            $scope.listarItemPedidoPorPedido();

                        }, 1000);
                    }

                }
            });
        }
        else {
            return false;
        }
    }


    $scope.alterarStatusPedidoParaPago = function (itemPedido) {

        if (confirm("Deseja realmente alterar o status do pedido?")) {

            $scope.status = $scope.gerarStatusDTO(itemPedido);

            formHandlerService.submit($scope, {
                url: Util.getUrl("/pedido/AlterarStatusItemPedidoParaPago"),
                objectName: 'status',
                showAjaxLoader: true,
                success: function (resp, status, config, message, validationMessage) {

                    $scope.message = message;
                    $scope.erros = validationMessage;

                    $scope.buttonCancelar = 'reset';
                    if (resp.success) {

                        $scope.message = Util.createMessage("success", "Status alterado com sucesso!");

                        $timeout(function () {
                            //angular.element("#modal-detalhes-pedido").modal('hide');

                            angular.element("#modal-opcoes-pagamento").modal('hide');
                            $scope.message = null;
                            $scope.recarregarListarPedidos();
                            $scope.listarItemPedidoPorPedido();
                        }, 1000);
                    }

                }
            });
        }
    }

    $scope.aprovarDescontoNoPedido = function (itemPedido) {

        if (confirm("Deseja realmente aprovar o item do pedido?")) {

            $scope.status = $scope.gerarStatusDTO(itemPedido);

            formHandlerService.submit($scope, {
                url: Util.getUrl("/pedido/AprovarDescontoNoPedido"),
                objectName: 'status',
                showAjaxLoader: true,
                success: function (resp, status, config, message, validationMessage) {

                    $scope.message = message;
                    $scope.erros = validationMessage;

                    $scope.buttonCancelar = 'reset';
                    if (resp.success) {

                        $scope.message = Util.createMessage("success", "Pedido alterado com sucesso!");

                        $timeout(function () {
                            angular.element("#modal-detalhes-pedido").modal('hide');
                            angular.element("#modal-opcoes-pagamento").modal('hide');
                            $scope.message = null;
                            $scope.recarregarListarPedidos();

                        }, 1000);
                    }

                }
            });
        }
    }
    $scope.listPedidoParticipanteByItemPedido = function (IPE_ID) {

        if (IPE_ID) {
            var url = Util.getUrl("/pedido/listPedidoParticipanteByItemPedido");

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'lstPedidoParticipante',
                responseModelName: 'lstPedidoParticipante',
                showAjaxLoader: true,
                data: {
                    IPE_ID: IPE_ID
                }
            });
        }
    };

    $scope.gerarParticipantesEFormaDePagamento = function () {

        $scope.gerarFormParticipantes();
        $scope.gerarFormaDePagamentos();
    }

    $scope.calcularImpostoDoPedido = function () {

        if ($scope.pedido.UEN_ID == 1 && Util.isPathValid($scope, "pedido.ITEM_PEDIDO")) {

            angular.forEach($scope.pedido.ITEM_PEDIDO, function (value, index) {

                $scope.calcularImpostosNosItens(value);
            });
        }
    }

    $scope.calcularImpostosNosItens = function (itemPedido) {

        $scope.limparPedidoPagamento();
        if (Util.isPathValid($scope, "pedido.INFO_CLIENTE.TIPO_CLI_ID")) {
            var url = Util.getUrl("/pedido/CalcularDescontoDosImpostos");

            if (itemPedido && itemPedido.PRODUTO_COMPOSICAO.EhCurso == true) {

                itemPedido.lock = true;
                formHandlerService.read(itemPedido, {
                    url: url,
                    targetObjectName: 'INFO_FATURA',
                    responseModelName: 'infoFatura',
                    showAjaxLoader: true,
                    data: {
                        valor: itemPedido.IPE_TOTAL_SEM_IMPOSTO,
                        tipoCliId: $scope.pedido.INFO_CLIENTE.TIPO_CLI_ID,
                        empresaDoSimples: $scope.pedido.PED_EMPRESA_DO_SIMPLES,
                        cemPorCentoFaturado: $scope.pedido.PED_CEM_POR_CENTO_FATURADO,
                        itemPedido : itemPedido
                    },
                    success: function (resp) {
                        itemPedido.lock = false;
                        if (resp.success && Util.isPathValid(itemPedido, "INFO_FATURA")) {

                            var valorFatura = itemPedido.INFO_FATURA.IFF_TOTAL_LIQUIDO;
                            var parcelas = itemPedido.IPE_PARCELA;
                            itemPedido.IPE_VALOR_PARCELA = MathService.truncarDecimal((valorFatura / parcelas), 2);
                            itemPedido.IPE_TOTAL = itemPedido.INFO_FATURA.IFF_TOTAL_LIQUIDO;
                        }
                    },
                    fail: function () {
                        itemPedido.lock = false;
                    }

                });
            }
        }

    };

    $scope.obterInfoFatura = function (iffId) {

        if (iffId) {
            var url = Util.getUrl("/pedido/obterInfoFatura");

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'infoFaturaModal',
                responseModelName: 'infoFatura',
                showAjaxLoader: true,
                data: {
                    iffId: iffId
                }
            });
        }
    };

    $scope.recalcularParcelas = function (itemPedido) {

        if (Util.isPathValid(itemPedido, "ITEM_PEDIDO_PEDIDO_PAGAMENTO")) {


            valorTotal = itemPedido.IPE_TOTAL;
            valorPago = 0;

            restanteAPagar = 0;

            angular.forEach(itemPedido.ITEM_PEDIDO_PEDIDO_PAGAMENTO, function (valueBase, index) {

                var value = valueBase.PEDIDO_PAGAMENTO;

                if (value.PGT_ENTRADA == true) {

                    valorTotal = itemPedido.IPE_TOTAL;
                    valorPago = value.PGT_VLR_PARCELA;

                    restanteAPagar = valorTotal - valorPago;
                    value.PGT_VLR_TOTAL = valorPago;
                }
                else {

                    if (restanteAPagar) {
                        value.PGT_VLR_PARCELA = MathService.truncarDecimal(restanteAPagar / value.PGT_QTDE_PARCELAS, 2);
                        value.PGT_VLR_TOTAL = restanteAPagar;
                    }
                }
            });
        }
    }


    $scope.faturarPedido = function () {

        if ($scope.requisicaoFaturamento) {

            if (confirm("Faturar ?")) {

                formHandlerService.submit($scope, {
                    url: Util.getUrl("/pedido/faturarPedido"),
                    objectName: 'requisicaoFaturamento',
                    showAjaxLoader: true,
                    success: function (resp, status, config, message, validationMessage) {

                        $scope.faturarRequest = null;
                        $scope.message = message;
                        $scope.erros = validationMessage;

                        $scope.buttonFaturamento = 'reset';
                        if (resp.success) {

                            $scope.message = Util.createMessage("success", "Pedido faturado com sucesso!");

                            $timeout(function () {
                                $scope.message = null;
                                $scope.recarregarListarPedidos();
                                $scope.requisicaoFaturamento = null;
                                angular.element("#modal-faturamento").modal('hide');

                            }, 1000);

                        }
                    },
                    fail: function () {
                        $scope.requisicaoFaturamento = null;
                    }
                });
            }
            else {
                return false;
            }

        }
    }

    $scope.gerarStatusDTO = function (itemPedido) {

        var cliId = null;
        var ipeId = itemPedido.IPE_ID;

        if (Util.isPathValid(itemPedido, "PEDIDO_CRM.CLI_ID")) {

            cliId = itemPedido.PEDIDO_CRM.CLI_ID;
        }
        return { CLI_ID: cliId, IPE_ID: ipeId };
    }
    

    $scope.gerarStatusPedidoCrmDTO = function (pedido) {

        var cliId = null;
        
        if (Util.isPathValid(pedido, "CLI_ID")) {

            cliId = pedido.CLI_ID;
        }
        return { CLI_ID: cliId, PED_CRM_ID: pedido.PED_CRM_ID };
    }
    $scope.recarregarListarPedidos = function(){
        
        if ($scope.paginaAtual) {

            var paginaAtual = $scope.paginaAtual;
            $scope.listarPedidos(paginaAtual);
        }
        else {
            $scope.listarPedidos();
        }
    }


    $scope.carregarAssinatura = function (numeroAssinatura, item) {

        $scope.numeroAssinatura = numeroAssinatura;
        $scope.modalAssinatura = null;
        $scope.lstContratos = null;
        $scope.lstParcelas = null;
        $scope.carregarDadosDaAssinatura(numeroAssinatura);
        $scope.carregarContratos(numeroAssinatura, item.IPE_ID);
        $scope.itemSelecionado = item;

        angular.element("#modal-assinatura-gerada").modal();
    }


    $scope.carregarDadosDaAssinatura = function (numeroAssinatura) {

        if (numeroAssinatura) {

            var url = Util.getUrl("/pedido/carregarDadosDaAssinatura");

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'modalAssinatura',
                responseModelName: 'assinatura',
                showAjaxLoader: true,
                data: {
                    numeroAssinatura: numeroAssinatura
                }
            });
        }

    };

    $scope.listarTipoPeridoDoProduto = function (cmpId) {

        if (cmpId) {

            var url = Util.getUrl("/produtoComposicao/listarTipoPeriodoDoProdutoComposto");

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'lstTipoPeriodoDoProduto',
                responseModelName: 'lstTipoPeriodoDoProduto',
                showAjaxLoader: true,
                data: {
                    CMP_ID: cmpId
                }
            });
        }

    };

    $scope.carregarContratos = function (numeroAssinatura, ipe_id) {

        $scope.listarContratos(1, numeroAssinatura, ipe_id);
        $scope.listarContratosGeradosNoPedido(1, numeroAssinatura, ipe_id);
    }

    $scope.carregarParcelas = function (numeroContrato, idRetorno) {

        $scope.retornoParcela = idRetorno;
        $scope.carregarDadosDaParcela(1, numeroContrato);
    }
        
    $scope.listarContratosGeradosNoPedido = function (pagina, numeroAssinatura, ipe_id) {


        if (numeroAssinatura) {

            $scope.numeroAssinatura = numeroAssinatura;
        }
        else {
            numeroAssinatura = $scope.numeroAssinatura;
        }

        var url = Util.getUrl("/pedido/CarregarDadosDoContratoGeradosNoPedido");

        if (pagina) {

            url += "?pagina=" + pagina;
        }

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstContratosGerados',
            responseModelName: 'lstContratos',
            showAjaxLoader: true,
            data: {
                numeroAssinatura: numeroAssinatura,
                IPE_ID : ipe_id
            },
            pageConfig: { pageName: 'page', pageTargetName: 'pageContratosDoPedido' },
            success: function (resp) {

            }
        });

    };

    $scope.listarContratos = function (pagina, numeroAssinatura, ipe_id) {


        if (numeroAssinatura) {

            $scope.numeroAssinatura = numeroAssinatura;
        }
        else {
            numeroAssinatura = $scope.numeroAssinatura;
        }

        var url = Util.getUrl("/pedido/CarregarDadosDoContrato");

        if (pagina) {

            url += "?pagina=" + pagina;
        }

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstContratos',
            responseModelName: 'lstContratos',
            showAjaxLoader: true,
            data: {
                numeroAssinatura: numeroAssinatura,
                IPE_ID: ipe_id
            },
            pageConfig: { pageName: 'page', pageTargetName: 'pageContratos' },
            success: function (resp) {

            }
        });

    };



    $scope.carregarDadosDaParcela = function (pagina, numeroContrato) {

        
        if (numeroContrato) {

            $scope.numeroContrato = numeroContrato;
        }
        else {
            numeroContrato = $scope.numeroContrato;
        }

        if (numeroContrato) {

            var url = Util.getUrl("/pedido/CarregarDadosDaParcela");
            if (pagina) {

                url += "?pagina=" + pagina;
            }

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'lstParcelas',
                responseModelName: 'lstParcelas',
                showAjaxLoader: true,
                pageConfig: { pageName: 'page', pageTargetName: 'pageParcelas' },
                data: {
                    numeroContrato: numeroContrato
                }
            });
        }

    };
    
    $scope.realizarPagamento = function (itemPedido) {

        $scope.itemPedidoParaPagar = itemPedido;
        angular.element("#modal-pagamento-pedido").modal();
    }

    $scope.abreModalFormasDePagamento = function (itemPedido) {

        $scope.listarFormasDePagamento(itemPedido.IPE_ID);
        $scope.itemPedidoModal = itemPedido;
        angular.element("#modal-forma-pagamento-gerada").modal();
    }
    
    $scope.listarFormasDePagamento = function (ipeId) {

        if (ipeId) {

            var url = Util.getUrl("/pedido/listarFormasDePagamento");

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'lstPedidoPagamento',
                responseModelName: 'lstPedidoPagamento',
                showAjaxLoader: true,
                data: {
                    ipeId: ipeId
                }
                ,
                success: function (response) {
                }
            });
        }

    };

    $scope.enviarEmailPagamento = function (email) {

        var pagamentoGateway = $scope.modalEnvioEmail.pagamentoGateway;

        if (pagamentoGateway == true) {

            $scope.enviarEmailPagamentoGateway(email);
        }
        else {
            $scope.enviarLinkBoletoPorEmail(email);
        }
    }

    $scope.enviarEmailPagamentoGateway = function (email) {

        var ipeId = $scope.modalEnvioEmail.IPE_ID;

        if (ipeId && email) {

            if (confirm("Deseja realmente enviar o email para " + email + " ?")) {

                $scope.emailRequest = {
                    ipeId: ipeId,
                    email: email
                };

                formHandlerService.submit($scope, {
                    url: Util.getUrl("/pedido/enviarEmailPagamento"),
                    objectName: 'emailRequest',
                    showAjaxLoader: true,
                    success: function (resp, status, config, message, validationMessage) {

                        if (resp.success) {

                            $scope.message = Util.createMessage("success", "Email enviado com sucesso!");

                            $timeout(function () {
                                $scope.message = null;
                                angular.element("#modal-enviar-email").modal('hide');
                                angular.element("#modal-opcoes-pagamento").modal('hide');

                            }, 1000);
                        }
                    },
                    fail: function () {

                    }
                });
            }

        }
    }

    $scope.enviarLinkBoletoPorEmail = function (email) {

        if (confirm("Deseja enviar o email para o cliente?")) {

            var IPE_ID = $scope.modalEnvioEmail.IPE_ID;

            $scope.envioEmailDTO = {
                IPE_ID: IPE_ID,
                email: email
            };

            formHandlerService.submit($scope, {
                url: Util.getUrl("/pedido/EnviarLinkBoletoPorEmail"),
                objectName: 'envioEmailDTO',
                showAjaxLoader: true,
                success: function (resp, status, config, message, validationMessage) {

                    $scope.message = message;
                    $scope.erros = validationMessage;

                    if (resp.success) {

                        $scope.message = Util.createMessage("success", "Email enviado com sucesso!");

                        $timeout(function () {
                            $scope.message = null;
                            angular.element("#modal-enviar-email").modal('hide');
                            angular.element("#modal-opcoes-pagamento").modal('hide');
                        }, 1000);
                    }

                }
            });
        }
    }

    $scope.abreModalEnviarEmailParaCliente = function(ipeId, cliId, pagamentoGateway){

        $scope.modalEnvioEmail = {
            IPE_ID: ipeId,
            CLI_ID: cliId,
            pagamentoGateway : pagamentoGateway
        };

        $scope.listarEmailsDoCliente(cliId);
        $scope.listarAssinaturas(cliId);
        angular.element("#modal-enviar-email").modal();
    }

    $scope.listarAssinaturas = function (cliId) {

        $scope.lstAssinatura = [];
        var data = {
            cliId: cliId
        };

        var url = Util.getUrl("/clientes/listarAssinaturas");


        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstAssinatura',
            responseModelName: 'lstAssinatura',
            showAjaxLoader: true,
            //pageConfig: { pageName: 'page' /*, pageTargetName: 'pageAgendamentoDoDia' */ },
            data: data,
            success: function (resp) {

            }
        });
    }


    $scope.assinaturaSelecionada = function () {

        $scope.lstEmails = [];

        if ($scope.objAssinatura) {

            $scope.listarEmails();
        }
    }

    $scope.listarEmails = function () {

        $scope.lstEmails = [];
        var data = {
            asnNumAssinatura: $scope.objAssinatura.ASN_NUM_ASSINATURA
        };

        var url = Util.getUrl("/clientes/listarEmailDasAssinaturas");


        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstEmails',
            responseModelName: 'lstEmails',
            showAjaxLoader: true,
            //pageConfig: { pageName: 'page' /*, pageTargetName: 'pageAgendamentoDoDia' */ },
            data: data,
            success: function (resp) {

            }
        });
    }

    $scope.listarEmailsDoCliente = function (cliId) {

        $scope.lstEmails = [];

        var url = Util.getUrl("/clientes/ListarEmailsDoCliente");


        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstEmailsDoCliente',
            responseModelName: 'lstEmails',
            showAjaxLoader: true,
            //pageConfig: { pageName: 'page' /*, pageTargetName: 'pageAgendamentoDoDia' */ },
            data: { cliId: cliId },
            success: function (resp) {

            }
        });
    }


    

    $scope.abrirPagamentoComGateway = function (itemPedido) {

        if (itemPedido) {
            $scope.message = Util.createMessage("info", "Preparando a parcela para seu pagamento!");

            $scope.request = { IPE_ID: itemPedido.IPE_ID };
            formHandlerService.submit($scope, {
                url: Util.getUrl("/pedido/PrepararParcelaGateway"),
                objectName: 'request',
                showAjaxLoader: true,
                success: function (resp, status, config, message, validationMessage) {

                    $scope.message = message;
                    $scope.erros = validationMessage;

                    if (resp.success) {

                        $scope.message = Util.createMessage("success", "Parcela preparada com sucesso. Redirecionando...");

                        $timeout(function () {

                            var url = itemPedido.UrlPagamento;

                            if (url) {
                                window.open(url, '_new');
                            }
                            else {
                                $scope.message = Util.createMessage("fail", "Não foi possível obter a url de pagamento.");
                            }

                            $scope.message = null;
                        }, 1000);
                    }

                }
            });
            
        }
    }

    $scope.downloadDaNota = function (nfxId) {

        if (nfxId) {

            post(Util.getUrl("/pedido/DownloadXmlNfe?nfxId=" + nfxId), true);
        }
    };


    $scope.regerarNotaFiscal = function (nfxId, ipeId) {

        if (nfxId && ipeId) {

            if (confirm("Regerar ?")) {

                $scope.regerar = {
                    ipeId : ipeId, 
                    nfx_id : nfxId
                };

                formHandlerService.submit($scope, {
                    url: Util.getUrl("/pedido/gerarOuAtualizarNotaFiscal"),
                    objectName: 'regerar',
                    showAjaxLoader: true,
                    success: function (resp, status, config, message, validationMessage) {

                        $scope.message = message;
                        if (resp.success) {

                            $scope.message = Util.createMessage("success", "Nota fiscal regerado com sucesso!");
                            $scope.ListarNFeXmlPorItemPedido(ipeId);
                            $timeout(function () {
                                $scope.message = null;

                            }, 1000);

                        }
                    },
                    fail: function () {
                    }
                });
            }
        }
    };

    $scope.abrirModalNFeXml = function (IPE_ID, ehCurso) {

        $scope.buttonNFe = 'reset';

        $scope.f = null;
        $scope.modalNFe = {
            IPE_ID: IPE_ID,
            ehCurso: ehCurso
        };

        angular.element("#modal-nfe-xml").modal();
        $scope.ListarNFeXmlPorItemPedido(IPE_ID);
    }


    $scope.ListarNFeXmlPorItemPedido = function (IPE_ID) {
        $scope.requisicaoItensNota = null;
        if (IPE_ID) {
            var url = Util.getUrl("/pedido/ListarNFeXmlPorItemPedido");

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'requisicaoItensNota',
                responseModelName: 'requisicaoItensNota',
                showAjaxLoader: true,
                data: {
                    IPE_ID: IPE_ID
                },
                success: function (response) {

                    if (response && response.result) {
                        
                        $scope.modalNFe.possuiServico = response.result.possuiServico;
                    }
                }
            });
        }
    };  

    
    $scope.uploadNFe = function (file, errFiles, IPE_ID) {

        if (IPE_ID) {

            $scope.f = file;
            $scope.errFiles = errFiles && errFiles[0];

            if (file) {

                file.upload = Upload.upload({
                    url: Util.getUrl("/pedido/ReceberUploadXmlNfeServico"),
                    data: { file: file }
                });

            }

            file.upload.then(function (response) {

                $timeout(function () {

                    var data = response.data;
                    file.result = data;

                    $scope.modalNFe.uploaded = true;
                    $scope.modalNFe.chaveDaNota = data.result.chaveDaNota;

                });

                //$timeout(function () {

                //    file.progress = 0;
                //}, 1000);

            }, function (response) {

                if (response.status > 0) {
                    $scope.errorMsg = response.status + ': ' + response.data;
                }
            }, function (evt) {
                file.progress = Math.min(100, parseInt(100.0 * evt.loaded / evt.total));

            });
            //var data = { IPE_ID: IPE_ID };
            //var url = Util.getUrl("/pedido/ReceberUpload");

            //UploadAjax.upload(url, $scope.modalNFe.chaveNFe, data);


        }
    };


    $scope.salvarArquivoUploadNfeService = function () {

        if ($scope.modalNFe) {
            if (confirm("Confirmar ?")) {

                formHandlerService.submit($scope, {
                    url: Util.getUrl("/pedido/SalvarArquivoUploadNFeServico"),
                    objectName: 'modalNFe',
                    showAjaxLoader: true,
                    success: function (resp, status, config, message, validationMessage) {

                        $scope.buttonNFe = 'reset';

                        $scope.message = message;
                        $scope.erros = validationMessage;
                        if (resp.success) {

                            $scope.message = Util.createMessage("success", "Salvo com sucesso!");

                            $timeout(function () {
                                $scope.message = null;
                                $scope.ListarNFeXmlPorItemPedido($scope.modalNFe.IPE_ID);

                            }, 1000);
                        }
                    },
                    fail: function () {
                        $scope.buttonNFe = 'reset';
                    }
                });
            }
        }
    }

    $scope.abrirModalObservacoes = function (pedido) {

        $scope.observacoesDTO = $scope.gerarStatusPedidoCrmDTO(pedido);
        $scope.observacoesDTO.OBSERVACOES = pedido.PED_CRM_DESCRICAO;
        angular.element("#modal-observacao-pedido").modal();
    }

    $scope.alterarObservacoes = function (itemPedido) {
            
            formHandlerService.submit($scope, {
                url: Util.getUrl("/pedido/AlterarObservacoes"),
                objectName: 'observacoesDTO',
                showAjaxLoader: true,
                success: function (resp, status, config, message, validationMessage) {

                    $scope.message = message;
                    $scope.erros = validationMessage;

                    $scope.buttonSave = 'reset';
                    if (resp.success) {

                        $scope.message = Util.createMessage("success", "Observações alterado com sucesso!");

                        $timeout(function () {
                            angular.element("#modal-observacao-pedido").modal('hide');
                            $scope.message = null;
                            $scope.recarregarListarPedidos();
                            $scope.listarItemPedidoPorPedido();

                        }, 1000);
                    }

                }
            });        
    }

    $scope.abrirModalMarcarComoPago = function (itemPedido) {

        $scope.statusPago = $scope.gerarStatusDTO(itemPedido);
        angular.element("#modal-marcar-pago").modal();

    }

    $scope.abrirModalHistorico = function(IPE_ID){

        $scope.listarHistoricos(null, IPE_ID);
        angular.element("#modal-historico").modal();
    }

    $scope.listarHistoricos = function (paginaReq, IPE_ID) {

        $scope.lstHistoricos = [];
        var url = Util.getUrl("/pedido/ListarHistoricoDoPedido");

        if (IPE_ID) {

            $scope.IPE_ID = IPE_ID;
        }
        else {

            IPE_ID = $scope.IPE_ID;
        }

        if (paginaReq) {

            url += "?pagina=" + paginaReq;
        }

        if (!$scope.filtro)
            $scope.filtro = {};

        $scope.filtro.IPE_ID = IPE_ID;
        

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstHistoricos',
            responseModelName: 'lstHistoricos',
            pageConfig: { pageName: 'page', pageTargetName: 'paginaHistorico' },
            data: $scope.filtro,
            showAjaxLoader: true,
            success: function () {
            }
        });


    }

    $scope.mostrarHistoricoGeral = function () {

        if ($scope.IPE_ID) {

            var IPE_ID = $scope.IPE_ID;
            var url = Util.getUrl("/pedido/historico?IPE_ID=" + IPE_ID);
            post(url, true);
        }

    }

    $scope.initHistorico = function(IPE_ID){

        $scope.listarHistoricoCompleto(IPE_ID);
    }

    $scope.listarHistoricoCompleto = function (IPE_ID) {

        $scope.lstHistoricos = [];
        var url = Util.getUrl("/pedido/ListarHistoricoPedidoCompleto");

        if (IPE_ID) {

            $scope.IPE_ID = IPE_ID;
        }
        else {

            IPE_ID = $scope.IPE_ID;
        }

        if (!$scope.filtro)
            $scope.filtro = {};

        $scope.filtro.IPE_ID = IPE_ID;

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstHistoricos',
            responseModelName: 'lstHistoricos',
            data: $scope.filtro,
            showAjaxLoader: true,
            success: function () {
            }
        });
    }

    $scope.abrirModalOpcoesDePagamento = function (itemPedido) {

        $scope.checaSeLinkPagamentoPodeSerGerado(itemPedido);
        $scope.itemModal = itemPedido;
        angular.element("#modal-opcoes-pagamento").modal();
    }

    $scope.checaSeLinkPagamentoPodeSerGerado = function (itemPedido) {

        var url = Util.getUrl("/pedido/checaSeLinkPagamentoPodeSerGerado");
        itemPedido.pedidoPagamentoItemPedido = null;

        formHandlerService.read(itemPedido, {
            url: url,
            targetObjectName: 'pedidoPagamentoItemPedido',
            responseModelName: 'pedidoPagamentoItemPedido',
            data: {IPE_ID: itemPedido.IPE_ID},
            showAjaxLoader: true,
            success: function () {
            }
        });

       
    }

    $scope.abrirModalRecusaDadosDePagamento = function (itemPedido) {

        $scope.alteracaoModal = $scope.gerarStatusDTO(itemPedido);
        $scope.alteracaoModal.header = "Recusa da Indicacao de Pagamento.";
        $scope.alteracaoModal.label = "Motivo da recusa do pagamento.";
        $scope.alteracaoModal.buttonConfig = [{ label: 'Confirmar', state: 'cnf' }, { label: 'Processando...', state: 'process', disabled: true }];
        $scope.alteracaoModal.buttonVar = "buttonAlt";
        $scope.alteracaoModal.acaoDoBotao = function () {

            return $scope.recusarPagamento();
        };

        angular.element("#modal-alterar-status").modal();

    }

    $scope.recusarPagamento = function () {

        if (confirm("Confirmar?")) {
            formHandlerService.submit($scope, {
                url: Util.getUrl("/pedido/RecusarPagamentoDoPedido"),
                objectName: 'alteracaoModal',
                showAjaxLoader: true,
                success: function (resp, status, config, message, validationMessage) {

                    $scope.message = message;
                    $scope.erros = validationMessage;

                    $scope.buttonSave = 'reset';
                    if (resp.success) {

                        $scope.message = Util.createMessage("success", "Pagamento rejeitado com sucesso!");

                        $timeout(function () {

                            angular.element("#modal-opcoes-pagamento").modal('hide');
                            angular.element("#modal-alterar-status").modal('hide');

                            $scope.message = null;
                            $scope.recarregarListarPedidos();
                            $scope.listarItemPedidoPorPedido();

                        }, 1000);
                    }

                }
            });
        }
        else {
            return false;
        }
    }

    $scope.abrirModalDescricao = function (descricao) {

        $scope.descricaoHistorico = descricao;
        angular.element("#descricao-historico-pedido").modal();
    }

    $scope.carregarPedidoStatus = function () {

        var url = Util.getUrl("/proposta/ListarPedidoStatus");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstPedidoStatus',
            responseModelName: 'lstPedidoStatus',
            showAjaxLoader: true,

        });
    }

    $scope.abrirModalFaturamento = function (prtId) {

        $scope.retornarPeriodoFaturamento();
        $scope.carregarEmpresas();
        $scope.gerarRequisicaoFaturamento(prtId);
        angular.element("#modal-faturamento").modal();
    }

    $scope.semanaFatSelecionada = function (modelStr, model) {

        if (modelStr) {

            var date = modelStr.split("/");
            var ano = date[0];
            var mes = date[1];
            var dia = date[2];

            model.DataFaturamento = new Date(ano, mes, dia);
        }
        else
        {
            model.DataFaturamento = null;
        }
    }

    $scope.formatarSemanaFaturamento = function (date) {
    
        if (date) {

            var dateStr = date.getFullYear() + '/' +
                (date.getMonth()) + '/' +
                date.getDate()

            return dateStr;
        }
    }

    $scope.gerarRequisicaoFaturamento = function (prtId) {

        var url = Util.getUrl("/pedido/gerarRequisicaoFaturamento");

        formHandlerService.read($scope, {
            url: url,
            data : {prtId : prtId},
            targetObjectName: 'requisicaoFaturamento',
            responseModelName: 'requisicaoFaturamento',
            showAjaxLoader: true,
            success: function () {

                if (Util.isPathValid($scope, 'requisicaoFaturamento.LstRequisicaoFaturamento')) {
                    var list = $scope.requisicaoFaturamento.LstRequisicaoFaturamento;
                    angular.forEach(list, function (value, index) {

                        value.DATA_FATURAMENTO_STR = $scope.formatarSemanaFaturamento(value.DataFaturamento);
                    });
                }
            }
        });
    }

    $scope.retornarPeriodoFaturamento = function () {

        var url = Util.getUrl("/proposta/retornarPeriodoFaturamento");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstDatasFaturamento',
            responseModelName: 'lstDatasFaturamento',
            showAjaxLoader: true,

        });
    }

    $scope.carregarEmpresas = function () {

        var url = Util.getUrl("/proposta/listarEmpresas");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstEmpresas',
            responseModelName: 'lstEmpresas',
            showAjaxLoader: true,

        });
    }

    $scope.sobreEscreverRequisicaoFaturamentoSemanaFat = function () {

        if (Util.isPathValid($scope, 'requisicaoFaturamento.LstRequisicaoFaturamento') &&
            $scope.requisicaoFaturamento.DataFaturamento) {

            var list = $scope.requisicaoFaturamento.LstRequisicaoFaturamento;
            angular.forEach(list, function (value, index) {

                var dataSemana = $scope.requisicaoFaturamento.DataFaturamento;
                value.DataFaturamento = dataSemana;
                value.DATA_FATURAMENTO_STR = $scope.formatarSemanaFaturamento(dataSemana);
            });

        }
    }

    $scope.sobreEscreverRequisicaoFaturamentoEmpresa = function () {

        if (Util.isPathValid($scope, 'requisicaoFaturamento.LstRequisicaoFaturamento') &&
            $scope.requisicaoFaturamento.EmpId) {

            var list = $scope.requisicaoFaturamento.LstRequisicaoFaturamento;
            angular.forEach(list, function (value, index) {

                value.EmpId = $scope.requisicaoFaturamento.EmpId;
            });
        }
    }

    $scope.sobreEscreverRequisicaoFaturamentoGerarNotaFiscal = function () {

        if (Util.isPathValid($scope, 'requisicaoFaturamento.LstRequisicaoFaturamento') &&
            $scope.requisicaoFaturamento.GerarNotaFiscal != null && $scope.requisicaoFaturamento.GerarNotaFiscal != undefined) {

            var list = $scope.requisicaoFaturamento.LstRequisicaoFaturamento;
            angular.forEach(list, function (value, index) {

                if (value.BloqueiaGeracaoNota == false)
                    value.GerarNotaFiscal = $scope.requisicaoFaturamento.GerarNotaFiscal;
            });
        }
    }

    $scope.listarRegistrosDeFaturamento = function (ipeId) {

        var url = Util.getUrl("/pedido/ListarRegistrosDeFaturamento");

        formHandlerService.read($scope, {
            url: url,
            data : {ipeId : ipeId},
            targetObjectName: 'lstRegistroFaturamento',
            responseModelName: 'lstRegistroFaturamento',
            showAjaxLoader: true,

        });
    }

    $scope.abrirModalRegistroFaturamento = function (ipeId) {
        
        $scope.listarRegistrosDeFaturamento(ipeId);
        angular.element("#modal-registro-faturamento").modal();
    }

    $scope.abrirModalPedidosRetroativos = function (reqNfe) {

        $scope.lstPedidosRetroativos = null;
        $scope.retornarPedidosNotaNaoGeradaPorData(reqNfe, null);
        angular.element("#modal-pedidos-retroativos-sem-nota").modal();
    }

    $scope.retornarPedidosNotaNaoGeradaPorData = function (reqNfe, pagina) {
        
        if (reqNfe) {

            $scope.reqNfe = reqNfe;
        }
        else {
            reqNfe = $scope.reqNfe;
        }

        var url = Util.getUrl("/pedido/retornarPedidosNotaNaoGeradaPorData");

        if (pagina) {

            url += "?pagina=" + pagina;
        }

        formHandlerService.read($scope, {
            url: url,
            data: {
                dataFaturamento: reqNfe.DataFaturamento,
                empId: reqNfe.EmpId,
                ipeIdParaExcluir : reqNfe.IpeId
            },
            targetObjectName: 'lstPedidosRetroativos',
            responseModelName: 'lstPedidosRetroativos',
            pageConfig: { pageName: 'page' , pageTargetName: 'pagePedidosRetroativos'  },
            showAjaxLoader: true,

        });
    }


    $scope.gerarNotaFiscal = function () {

        if(Util.isPathValid($scope,'modalNFe.IPE_ID'))
        {
            $scope.gerarNotaRequest = { ipeId: $scope.modalNFe.IPE_ID };

             if (confirm("Gerar nota fiscal ?")) {

                    formHandlerService.submit($scope, {
                        url: Util.getUrl("/pedido/gerarOuAtualizarNotaFiscal"),
                        objectName: 'gerarNotaRequest',
                        showAjaxLoader: true,
                        success: function (resp, status, config, message, validationMessage) {

                            $scope.message = message;
                            $scope.buttonGerarNota = 'reset';

                            if (resp.success) {

                                $scope.message = Util.createMessage("success", "Nota fiscal gerada com sucesso!");
                                $scope.ListarNFeXmlPorItemPedido($scope.modalNFe.IPE_ID);
                                $timeout(function () {
                                    $scope.message = null;

                                }, 1000);

                            }
                        },
                        fail: function () {
                        }
                    });
                }
            }
    }

    $scope.acaoFiltroBlur = function (fil) {

        if (!$scope.lstFiltrosUtilizados)
            $scope.lstFiltrosUtilizados = [];

        if (fil && fil.value) {

            var achou = false;
            angular.forEach($scope.lstFiltrosUtilizados, function (value, index) {

                if (achou === true || fil.chave == value.chave) {

                    achou = true;
                }

            });

            if(achou != true)
                    $scope.lstFiltrosUtilizados.push(fil);
        }
    }


    $scope.obterGruposDeFiltroDoPedido = function () {

        var url = Util.getUrl("/pedido/obterGruposDeFiltroDoPedido");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'gruposDeFiltro',
            responseModelName: 'gruposDeFiltro',
            showAjaxLoader: true,
            success : function(){

                if ($scope.gruposDeFiltro) {

                    var grupos = $scope.gruposDeFiltro.Grupos;
                    var status = grupos.status;
                    var data = grupos.data;
                    var dataFaturamento = grupos.dataFaturamento;

                    FilterService.adicionarGrupoDeFiltro($scope.filtros, status, "status");
                    FilterService.adicionarGrupoDeFiltro($scope.filtros, data, "data");
                    FilterService.adicionarGrupoDeFiltro($scope.filtros, dataFaturamento, "dataFat");
                }
            }

        });
    }
    $scope.listarAssinaturaAutocomplete = function (assinatura) {

        if ($scope.select2CtrlRepresentante != null)
            $scope.select2CtrlRepresentante.loader = true;
        var filtro = { assinatura: assinatura };

        formHandlerService.read($scope, {
            url: Util.getUrl("/pedido/ListarAssinaturaAutocomplete"),
            targetObjectName: 'lstAssinatura',
            responseModelName: 'lstAssinatura',
            data: filtro,
            success: function () {
                if ($scope.select2CtrlRepresentante != null)
                    $scope.select2CtrlRepresentante.loader = false;


            }

        });
    }


    $scope.criarFiltros = function () {

        $scope.filtros =
            [{
                nomeGrupo: 'Padrão',
                idGrupo: "padrao",
                filtros : [
                            {
                                label: "Id do Cliente",
                                chave: "CLI_ID",
                                ordem: 0,
                                size: 96,
                                tipo: 'texto'
                            },
                            {
                                label: "Nome do Cliente",
                                chave: "nomeCliente",
                                ordem: 1,
                                size: 170,
                                tipo: 'texto'
                            },
                            {
                                label: "CNPJ/CPF do cliente",
                                chave: "cpfCnpjCliente",
                                ordem: 2,
                                maxLength: 14,
                                size: 170,
                                tipo: 'texto'
                            },
                            {
                                label: "Id do Pedido",
                                chave: "PED_CRM_ID",
                                ordem: 0,
                                size: 100,
                                tipo: 'texto'
                            },
                            {
                                label: "Assinatura",
                                chave: "assinatura",
                                ordem: 3,
                                tipo: 'autocomplete',
                                lstAutoCompleteName: 'lstAssinatura',
                                funcaoAutoComplete: 'listarAssinaturaAutocomplete'
                            }
                ]
            },
            {
                nomeGrupo: 'Por Códigos',
                idGrupo: "codigo",
                filtros: [
                            {
                                label: "Id do Item de Pedido",
                                chave: "IPE_ID",
                                size: 100,
                                ordem: 1,
                                tipo: 'texto'
                            },
                            {
                                label: "Id da Proposta",
                                chave: "PRT_ID",
                                size: 100,
                                ordem: 2,
                                tipo: 'texto'
                            },
                            {
                                label: "Id do Item da Proposta",
                                chave: "PPI_ID",
                                size: 100,
                                ordem: 3,
                                tipo: 'texto'
                            },
                ]
            },
            {
                nomeGrupo: 'Por Status',
                idGrupo: "status",
                filtros: [
                        {
                            label: "Status",
                            chave: "status",
                            ordem: 0,
                            tipo: 'grupo',
                            grupo: []

                        },
                        {
                            label: "Pedidos Sem nota",
                            chave: "semNotaFiscal",
                            ordem: 1,
                            size: 96,
                            tipo: 'toogle'
                        }
                ]
            },
            {
                nomeGrupo: 'Por Data',
                idGrupo: "padrao",
                filtros: [
                            {
                                label: "Data de Pedido",
                                chave: "data",
                                exibirNomeToken: true,
                                nomeNoToken: "Data",
                                ordem: 0,
                                tipo: 'grupo'
                            },
                            {
                                label: "Data de Faturamento",
                                chave: "dataFat",
                                exibirNomeToken: true,
                                nomeNoToken : "Faturamento",
                                ordem: 1,
                                tipo: 'grupo'
                            },
                            {
                                label: "A partir da data",
                                chave: "dataInicial",
                                ordem: 2,
                                size: 96,
                                tipo: 'data'
                            },
                            {
                                label: "Até a data",
                                chave: "dataFinal",
                                ordem: 3,
                                size: 170,
                                tipo: 'data'
                            },
                            {
                                label: "Faturamento a Partir da Data",
                                chave: "dataFaturamentoInicial",
                                ordem: 4,
                                size: 170,
                                tipo: 'data'
                            },
                            {
                                label: "Faturamento Até a Data",
                                chave: "dataFaturamentoFinal",
                                ordem: 5,
                                size: 140,
                                tipo: 'data',
                            }
                ]   
            },
            {
                nomeGrupo: 'Outros',
                idGrupo: "regiao",
                filtros: [
                        {
                            label: "Região",
                            chave: "RG_ID",
                            ordem: 0,
                            size: 96,
                            tipo: 'select',
                            valueName: 'RG_ID',
                            labelName: 'RG_DESCRICAO',
                            renderIf: !$scope.ehFranquiado
                    },
                    {
                        label: "Tipo de Negociação",
                        chave: "TNE_ID",
                        ordem: 0,
                        size: 96,
                        tipo: 'select',
                        valueName: 'TNE_ID',
                        labelName: 'TNE_DESCRICAO'
                    },
                ]
            },
            {
                nomeGrupo: 'Número de Nota Fiscal',
                idGrupo: "nota_fiscal",
                filtros: [
                            {
                                label: "Código Nota Fiscal Inicial",
                                chave: "numeroNotaInicial",
                                ordem: 0,
                                size: 50,
                                tipo: 'texto'
                            },
                            {
                                label: "Código Nota Fiscal Final",
                                chave: "numeroNotaFinal",
                                size: 50,
                                ordem: 1,
                                tipo: 'texto'
                            }
                ]
            },
            ];
    }


    $scope.queryPedidosSelecionados = {
        selected: true
    };
        $scope.ativarModoBatchNotaFiscal = function () {

            $scope.notaBatchModal = {
                ativo: true,
                aberto: false,
                controle: {}
            };

            $scope.notaBatchModal.ListCodPedidos = [];

        };

        $scope.abrirModalCodigoPedidosSelecionados = function () {

            angular.element("#modal-pedidos-selecionados").modal();
        };

    $scope.abrirModalGerarVariasNotasFiscais = function () {

        $scope.batchResp = null;
        angular.element("#modal-batch-notaFiscal").modal();
    }

    $scope._abrirModalBatch = function (tipo) {
        $scope.notaBatchModal.tipo = tipo;
        $scope.batchResp = null;
        angular.element("#modal-batch-notaFiscal").modal();
    }

    $scope.abrirModalGerarVariasXMLNotasFiscais = function () {

        $scope._abrirModalBatch('gerar');
    }


    $scope.abrirDownloadVariasNotasFiscais = function () {

        $scope._abrirModalBatch('download')
    }

    $scope.cancelarAcao = function () {

        $scope.notaBatchModal = null;
    }

    $scope.gerarOuAtualizarVariasNotasFiscais = function () {

            if (confirm("Gerar notas fiscais dos pedidos selecionados ?")) {

                formHandlerService.submit($scope, {
                    url: Util.getUrl("/pedido/gerarOuAtualizarVariasNotasFiscais"),
                    objectName: 'notaBatchModal',
                    showAjaxLoader: true,
                    success: function (resp, status, config, message, validationMessage) {

                        $scope.message = message;
                        $scope.buttonGerarNota = 'reset';

                        if (resp.success) {

                            $scope.message = Util.createMessage("success", "Notas fiscais geradas com sucesso!");
                            $scope.batchResp = resp.result.batchResp;

                            if ($scope.batchResp.TotalExito > 0)
                                $scope.downloadDoZipDaNota($scope.batchResp.Path);
                            $timeout(function () {
                                $scope.message = null;

                            }, 1000);

                        }
                    },
                    fail: function () { 
                    }
                });
            }
            else {
                return false;
            }
    }


    $scope.downloadVariasNotasFiscais = function () {

        if (confirm("Baixar as notas dos pedidos selecionados ?")) {

            formHandlerService.submit($scope, {
                url: Util.getUrl("/pedido/downloadVariasNotas"),
                objectName: 'notaBatchModal',
                showAjaxLoader: true,
                success: function (resp, status, config, message, validationMessage) {

                    $scope.message = message;
                    $scope.buttonGerarNota = 'reset';

                    if (resp.success) {

                        $scope.message = Util.createMessage("success", "Preparando para o download!");
                        $scope.batchResp = resp.result.batchResp;

                        if ($scope.batchResp.TotalExito > 0)
                            $scope.downloadDoZipDaNota($scope.batchResp.Path);
                        $timeout(function () {
                            $scope.message = null;

                        }, 1000);

                    }
                },
                fail: function () {
                }
            });
        }
        else {
            return false;
        }
    }

    $scope.downloadDoZipDaNota = function (fileName) {

        if (fileName) {

            post(Util.getUrl("/pedido/DownloadZipXmlNfe?fileName=" + fileName), true);
        }
    };

    $scope.mostrarErrosBatchNota = function () {
        $scope.batchResp.exibirErro = true;
    }

    $scope.GerarDadosIniCancelamento = function (pedCrmId) {

        var url = Util.getUrl("/pedido/gerarDadosIniCancelamento");
        formHandlerService.read($scope, {
            url: url,
            data: {
                pedCrmId: pedCrmId
            },
            targetObjectName: 'cancelamentoDTO',
            responseModelName: 'dadosCanc',
            showAjaxLoader: true,

        });
    }

$scope.gerarHTMLEmailCanc = function () {

        formHandlerService.submit($scope, {
            url: Util.getUrl("/pedido/GerarHTMLEmailCanc"),
            objectName: 'cancelamentoDTO',
            showAjaxLoader: true,
            success: function (resp, status, config, message, validationMessage) {

                $scope.previewHtmlEmail = resp.result.htmlEmail;
                $scope.message = message;
                $scope.buttonGerarNota = 'reset';
            },
            fail: function () {
            }
        });    
}

    $scope.abrirModalPreviewEmail = function () {
        $scope.gerarHTMLEmailCanc();
        angular.element("#template-html-email").modal();

}

    $scope.abrirDetalhes = function (pedidoCRMId) {

        if (pedidoCRMId != null) {
            var url = Util.getUrl("/pedido/detalhes?PED_CRM_ID=" + pedidoCRMId);
            post(url, true);
        }
    }

    $scope.adicionarVariasNotasAoLoteNFe = function () {

        if (confirm("Gerar notas fiscais dos pedidos selecionados ?")) {

            formHandlerService.submit($scope, {
                url: Util.getUrl("/pedido/adicionarVariasNotasAoLoteNFe"),
                objectName: 'notaBatchModal',
                showAjaxLoader: true,
                success: function (resp, status, config, message, validationMessage) {

                    $scope.message = message;
                    $scope.buttonGerarNota = 'reset';

                    if (resp.success) {

                        $scope.batchResp = resp.result.batchResp;

                        if ($scope.batchResp.TotalFalha == 0 && $scope.batchResp.TotalExito > 0){
                            $scope.message = Util.createMessage("success", "Notas Fiscal Agendada com sucesso.");

                            $timeout(function () {
                                $scope.message = null;

                            }, 3000);

                            angular.element("#modal-batch-notaFiscal").modal('hide');
                        }
                        else if ($scope.batchResp.TotalFalha == 0 && $scope.batchResp.TotalExito == 0) {
                            $scope.message = Util.createMessage("warning", "Nenhuma nota foi encontrada para ser agendada.");

                            $timeout(function () {
                                $scope.message = null;

                            }, 3000);
                        }
                        else if ($scope.batchResp.TotalFalha > 0 && $scope.batchResp.TotalExito > 0) {
                            $scope.message = Util.createMessage("warning", "Algumas notas não foram agendadas.");

                            $timeout(function () {
                                $scope.message = null;

                            }, 3000);
                        }
                        else if ($scope.batchResp.TotalFalha > 0 && $scope.batchResp.TotalExito == 0) {
                            $scope.message = Util.createMessage("fail", "Nenhuma nota foi agendada.");
                            $timeout(function () {
                                $scope.message = null;

                            }, 3000);
                        }
                        

                    }
                },
                fail: function () {
                }
            });
        }
        else {
            return false;
        }
    }

    $scope.abrirProposta = function (prtId) {

        var url = Util.getUrl('/proposta/index?prtId=') + prtId;
        post(url, true);
    }

    
    $scope.retonarProgressoDoJob = function (lstCod) {

        $scope.progressPromisse = $interval(function () {
            var url = Util.getUrl("/batch/RetornarStatusDoLote");

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'lstLoteBatch',
                responseModelName: 'lstLoteBatch',
                //showAjaxLoader: true,
                data: { lstCod: lstCod },
                success: function (resp) {

                    if ($scope.lstLoteBatch) {

                        var terminou = true;
                        angular.forEach($scope.lstLoteBatch, function (value, index) {

                            terminou = terminou && (value.CodStatus == 2 || value.CodStatus == 3);
                            
                        });

                        if (terminou === true) {
                            $interval.cancel($scope.progressPromisse);
                            $scope.message = Util.createMessage('success', 'Processamento finalizado.');

                            angular.element("#modal-nfe").modal('hide');
                            $timeout(function () {
                                $scope.message = null;
                                $scope.lstLoteBatch = null;
                            }, 3000);                        
                        }
                    }
                }
            });


        }, 1000);
    };

    $scope.gerarOuAtualizarVariasNotasFiscais = function () {

        if (confirm("Gerar notas fiscais dos pedidos selecionados ?")) {

            formHandlerService.submit($scope, {
                url: Util.getUrl("/pedido/gerarOuAtualizarVariasNotasFiscais"),
                objectName: 'notaBatchModal',
                showAjaxLoader: true,
                success: function (resp, status, config, message, validationMessage) {

                    $scope.message = message;
                    $scope.buttonGerarNota = 'reset';

                    if (resp.success) {

                        $scope.message = Util.createMessage("success", "Notas fiscais geradas com sucesso!");
                        $scope.batchResp = resp.result.batchResp;

                        if ($scope.batchResp.TotalExito > 0)
                            $scope.downloadDoZipDaNota($scope.batchResp.Path);
                        $timeout(function () {
                            $scope.message = null;

                        }, 1000);

                    }
                },
                fail: function () { 
                }
            });
        }
        else {
            return false;
        }
    }


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

        $scope.obterInfoFatura = function (iffId) {

            if (iffId) {
                var url = Util.getUrl("/proposta/obterInfoFatura");

                formHandlerService.read($scope, {
                    url: url,
                    targetObjectName: 'infoFaturaModal',
                    responseModelName: 'infoFatura',
                    showAjaxLoader: true,
                    data: {
                        iffId: iffId
                    },
                    success: function () {

                        if ($scope.infoFaturaModal.IFF_ID &&
                            (!$scope.infoFaturaModal.INFO_FATURA_ITEM ||
                                $scope.infoFaturaModal.INFO_FATURA_ITEM.length < 1)) {
                            $scope.obterInfoFaturaItem($scope.infoFaturaModal.IFF_ID);
                        }
                    }
                });
            }
        };

    $scope.carregarTipoNegociacao = function () {

        var url = Util.getUrl("/pedido/listarTipoNegociacao");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstTipoNegociacao',
            responseModelName: 'lstTipoNegociacao',
            showAjaxLoader: true,
            success: function () {

                FilterService.adicionarDadoCombo($scope.filtros, $scope.lstTipoNegociacao, "TNE_ID");
            }
        });
    };



        $scope.obterInfoFaturaItem = function (iffId) {

            if (iffId) {
                var url = Util.getUrl("/proposta/obterInfoFaturaItem");

                formHandlerService.read($scope, {
                    url: url,
                    targetObjectName: 'infoFaturaModal.INFO_FATURA_ITEM',
                    responseModelName: 'infoFaturaItem',
                    showAjaxLoader: true,
                    data: {
                        iffId: iffId
                    }
                });
            }
        };

        $scope.abrirImpostosDaInfoFaturaItm = function (infoFaturaItm, infoFatura) {

            $scope.infoFaturaModalImp = angular.copy(infoFatura);
            $scope.infoFaturaItmModal = angular.copy(infoFaturaItm);

            angular.element("#modal-impostos-utilizados").modal();
        };


        $scope.abrirInfoFatura = function (infoFatura) {

            $scope.infoFaturaModal = angular.copy(infoFatura);


            if (infoFatura.IFF_ID
                &&
                (!$scope.infoFaturaModal.INFO_FATURA_ITEM ||
                    $scope.infoFaturaModal.INFO_FATURA_ITEM.length < 1)) {
                $scope.obterInfoFaturaItem(infoFatura.IFF_ID);
            }

            angular.element("#modal-impostos-utilizados").modal('hide');
            angular.element("#modal-fatura").modal();
        };


        $scope.carregarInfoFaturaEAbrirModal = function (iffId) {

            $scope.infoFaturaModal = null;
            $scope.obterInfoFatura(iffId);
            angular.element("#modal-impostos-utilizados").modal('hide');
            angular.element("#modal-fatura").modal();
        };


    if (window.ComprovanteController !== undefined) {
            
        ComprovanteController($scope, formHandlerService, $http, $timeout);
    }

    if (window.ExtornoPagamentoParcelaController !== undefined) {

        ExtornoPagamentoParcelaController($scope, formHandlerService, $timeout);
    }

    if (window.BoletoAvulsoController !== undefined) {

        BoletoAvulsoController($scope, formHandlerService, $http, conversionService, $timeout);
    }

    if (window.LoteNotaFiscalController !== undefined) {

        LoteNotaFiscalController($scope, formHandlerService, $timeout, $window);
    }

}]);