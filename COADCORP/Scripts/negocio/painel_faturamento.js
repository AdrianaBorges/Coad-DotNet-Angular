appModule.controller("PainelFaturamentoController", ['$scope', '$http', 'formHandlerService', 'MathService', '$timeout',
    'UploadAjax', 'Upload', 'FilterService', '$sce', '$interval',
    function ($scope, $http, formHandlerService, MathService, $timeout,
        UploadAjax, Upload, FilterService, $sce, $interval) {

        $scope.initPanel = function () {

            $scope.initPesquisa();
            $scope.carregarFiltros();
            $scope.exibirFiltroProPendCon = false;
            $scope.ativarModoBatch();

            $interval(function () {
                $scope.initPesquisa();

            }, 120000);
        }

        $scope.initPesquisa = function () {

            $scope.pesquisarPropostasPendConf();
            $scope.listarNfeLoteItmComErroOuPendente();

        }

        $scope.carregarFiltros = function () {

            $scope.criarFiltrosProPendAut();
        }

        $scope.pesquisarPropostasPendConf = function (pagina) {

            var url = Util.getUrl("/faturamento/pesquisarPropostaPendConf");

            //if (pagina) {

            //    url += "?pagina=" + pagina;
            //}

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'propostPend',
                responseModelName: 'propostPend',
                showAjaxLoader: true,
                data: $scope.filtroProPend,
                pageConfig: { pageName: 'page', pageTargetName: 'pagePropostaPend' },
                success: function (resp) {

                }
            });
        };

        $scope.criarFiltrosProPendAut = function () {
            
                $scope.filtrosProPendAut = [{
                    nomeGrupo: 'Padrão',
                    idGrupo: "padrao",
                    queryFilter: {
                        label: "Pesquisa",
                        chave: "query",
                        ordem: 0,
                        size: 96,
                        tipo: 'texto'
                    },
                    filtros: [
                        {
                            label: "Data Pag Inicial",
                            chave: "dataInicial",
                            ordem: 2,
                            size: 96,
                            tipo: 'data'
                        },
                        {
                            label: "Data Pag Final",
                            chave: "dataFinal",
                            ordem: 3,
                            size: 170,
                            tipo: 'data'
                        },
                    ]
                }]
            
    };

    $scope.ligarDesligarJob = function (jobId) {

        if ($scope.intervalPromisse) {
            $interval.cancel($scope.intervalPromisse);
            $scope.intervalPromisse = undefined;
        }

        $timeout(function () {

        if (jobId) {            
                $scope.ligarDesligarRequest = {
                    jobId: jobId
                };

                formHandlerService.submit($scope, {
                    url: Util.getUrl("/jobs/LigarDesligarJob"),
                    objectName: 'ligarDesligarRequest',
                    showAjaxLoader: true,
                    success: function (resp, status, config, message, validationMessage) {
                        $scope.initList();           
                    },
                    fail: function () {

                    }
                });
            }
        });
    }
        
        $scope.abrirDetalhes = function (prtId) {

            if (prtId != null) {
                var url = Util.getUrl("/proposta/detalhes?prtId=" + prtId);
                post(url, true);
            }
        }


        $scope.toogleFiltros = function () {

            $scope.exibirFiltroProPendCon = !$scope.exibirFiltroProPendCon; 
        }

        $scope.ativarModoBatch = function () {

            $scope.propostaBatchModal = {
                ativo: true,
                aberto: false,
                controle: {}
            };

            $scope.loteBatchModal = {

                ativo: true,
                aberto: false,
                controle: {}
            };

            $scope.loteBatchModal.ListCod = [];
            $scope.propostaBatchModal.ListCodProposta = [];
        }

        $scope.abrirModalCodigoPropostasSelecionadas = function () {

            $scope.modalCodigos = {

                label: 'Confirmar Pagamento',
                descricao: 'Códigos Propostas Selecionadas',
                lstCodigos: $scope.loteBatchModal.ListCodProposta
            };
            angular.element("#modal-codigos-selecionados").modal();
        }

        $scope.abrirModalCodigoLoteItemSelecionadas = function () {

            $scope.modalCodigos = {

                label: 'Reenviar NF',
                descricao: 'Códigos Itens de Lote Selecionados',
                lstCodigos: $scope.loteBatchModal.ListCod
            };
            angular.element("#modal-codigos-selecionados").modal();
        }

        $scope.abrirModalConfirmarPagamento = function () {

            $scope.batchResp = null;
            angular.element("#modal-batch-pag-proposta").modal();
        }

        $scope.confirmarPropostasComoPaga = function () {
            
            if (confirm("Deseja realmente alterar o status da proposta?")) {

                $scope.propostaBatchModal.lstPrtId = [];

                angular.forEach($scope.propostaBatchModal.ListCodProposta, function (value, index) {

                    $scope.propostaBatchModal.lstPrtId.push(value.CodProposta);
                });

                formHandlerService.submit($scope, {
                    url: Util.getUrl("/faturamento/marcarManualmenteVariasPropostasComoPaga"),
                    objectName: 'propostaBatchModal',
                    showAjaxLoader: true,
                    success: function (resp, status, config, message, validationMessage) {

                        $scope.message = message;
                        $scope.erros = validationMessage;

                        $scope.buttonGerarNota = 'reset';
                        if (resp.success) {

                            $scope.batchResp = resp.result.batchResp;

                            if ($scope.batchResp.TotalFalha == 0 && $scope.batchResp.TotalExito > 0) {
                                $scope.message = Util.createMessage("success", "Pedidos Marcados com sucesso.");

                                $timeout(function () {
                                    $scope.message = null;
                                    $scope.pesquisarPropostasPendConf();
                                    $scope.propostaBatchModal.ListCodProposta = [];
                                    angular.element("#modal-batch-pag-proposta").modal('hide');

                                }, 3000);

                            }
                            else if ($scope.batchResp.TotalFalha == 0 && $scope.batchResp.TotalExito == 0) {
                                $scope.message = Util.createMessage("warning", "Nenhum Pedido foi marcado.");

                                $timeout(function () {
                                    $scope.message = null;

                                }, 3000);
                            }
                            else if ($scope.batchResp.TotalFalha > 0 && $scope.batchResp.TotalExito > 0) {
                                $scope.message = Util.createMessage("warning", "Alguns Pedidos não foram marcadas.");

                                $timeout(function () {
                                    $scope.message = null;

                                }, 3000);
                            }
                            else if ($scope.batchResp.TotalFalha > 0 && $scope.batchResp.TotalExito == 0) {
                                $scope.message = Util.createMessage("fail", "Nenhum Pedido foi marcado.");
                                $timeout(function () {
                                    $scope.message = null;

                                }, 3000);
                            }

                        }

                    }
                });
            }
            else {
                return false;
            }
        }

        $scope.abrirPopover = function (element, $event) {
            
            if ($event)
                $event.stopPropagation();

            if (!$scope.popoverAberto) {
                $scope.popoverAberto = true;
                angular.element("#" + element).popover('show');
            }
        }

        $scope.fecharPopover = function ($event, excetoIndex, subSelector) {

            //if ($event)
            //    $event.stopPropagation();

            $scope.popoverAberto = false;
            excetoIndex = (excetoIndex || excetoIndex == 0) ? excetoIndex : 111111111;
            subSelector = (subSelector) ? subSelector : 'sub';

            var selector = "#pop_over_" + subSelector + excetoIndex;
            angular.element(".popover-a").not(selector).popover('hide');
        }

        $scope.abrirProposta = function (prtId) {

            var url = Util.getUrl('/proposta/index?prtId=') + prtId;
            post(url, true);
        }

        $scope.mostrarErrosBatch = function () {
            $scope.batchResp.exibirErro = true;
        }

        $scope.listarNfeLoteItmComErroOuPendente = function (pagina) {

            var url = Util.getUrl("/faturamento/listarNfeLoteItmComErroOuPendente");
            
            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'lotePend',
                responseModelName: 'lotePend',
                showAjaxLoader: true,
                data: $scope.filtroLotePend,
                pageConfig: { pageName: 'page', pageTargetName: 'pageLotePend' },
                success: function (resp) {

                }
            });
        };

        $scope.abrirDetalhesLoteItem = function (loteItem) {

            $scope.loteItem = loteItem;
            angular.element("#modal-msg-lote").modal();
        }
        
        $scope.abrirPropostaPorItem = function (ppiId) {

            var url = Util.getUrl('/proposta/index?ppiId=') + ppiId;
            post(url, true);
        }

        $scope.abrirPedidoPorItem = function (ipeId) {

            var url = Util.getUrl('/pedido/index?ipeId=') + ipeId;
            post(url, true);
        }


        $scope.cancelarProcessamentoItem = function () {

            if (confirm("Deseja realmente cancelar o processamento desses lotes?")) {

                $scope.loteBatchModal.lstCodEnv = [];

                angular.forEach($scope.loteBatchModal.ListCod, function (value, index) {

                    $scope.loteBatchModal.lstCodEnv.push(value.Cod);
                });
                formHandlerService.submit($scope, {
                    url: Util.getUrl("/faturamento/CancelarProcessamentoItem"),
                    objectName: 'loteBatchModal.lstCodEnv',
                    showAjaxLoader: true,
                    success: function (resp, status, config, message, validationMessage) {
                        $scope.loteBatchModal.ListCod = [];
                        $scope.listarNfeLoteItmComErroOuPendente();
                    },
                    fail: function () {

                    }
                });
            }
        }

        $scope.abrirModalRegerarNota = function () {
            
            angular.element("#modal-batch-notaFiscal").modal();
        }


        $scope.adicionarVariasNotasAoLoteNFe = function () {

            if (confirm("Gerar notas fiscais dos pedidos selecionados ?")) {

                $scope.loteBatchModal.ListCodLoteItm = [];

                angular.forEach($scope.loteBatchModal.ListCod, function (value, index) {

                    $scope.loteBatchModal.ListCodLoteItm.push(value.Cod);
                });

                formHandlerService.submit($scope, {
                    url: Util.getUrl("/faturamento/adicionarVariasNotasAoLoteNFe"),
                    objectName: 'loteBatchModal.ListCodLoteItm',
                    showAjaxLoader: true,
                    success: function (resp, status, config, message, validationMessage) {

                        $scope.message = message;
                        $scope.buttonGerarNota = 'reset';
                        $scope.loteBatchModal.ListCod = [];
                        $scope.listarNfeLoteItmComErroOuPendente();

                        if (resp.success) {

                            $scope.batchResp = resp.result.batchResp;

                            if ($scope.batchResp.TotalFalha == 0 && $scope.batchResp.TotalExito > 0) {
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
}]);