function LoteNotaFiscalController($scope, formHandlerService, $timeout, $window) {

    
    $scope.listarLoteItens = function (ipeID) {

        if (ipeID) {

            var url = Util.getUrl("/pedido/listarItensDeLote");

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'lstLoteItem',
                responseModelName: 'lstLoteItem',
                data: { ipeID: ipeID },
                showLoader: true,
                success: function (response) {

                }
            });
        }
    }

    $scope.listarLoteItensServico = function (ipeID) {

        if (ipeID) {

            var url = Util.getUrl("/pedido/listarItensDeLoteServico");

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'lstLoteItemServico',
                responseModelName: 'lstLoteItemServico',
                data: { ipeID: ipeID },
                showLoader: true,
                success: function (response) {

                }
            });
        }
    }

    $scope.listarLoteItensDaProposta = function (ppiID) {

        if (ppiID) {

            var url = Util.getUrl("/proposta/listarItensDeLote");

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'lstLoteItem',
                responseModelName: 'lstLoteItem',
                data: { ppiID: ppiID },
                showLoader: true,
                success: function (response) {

                }
            });
        }
    }


    $scope.listarLoteItensDaPropostaServico = function (ppiID) {

        if (ppiID) {

            var url = Util.getUrl("/proposta/listarItensDeLoteServico");

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'lstLoteItemServico',
                responseModelName: 'lstLoteItemServico',
                data: { ppiID: ppiID },
                showLoader: true,
                success: function (response) {

                }
            });
        }
    }


    $scope.listarNotasDaProposta = function (ppiID) {

        if (ppiID) {

            var url = Util.getUrl("/proposta/ListarNotasDaProposta");

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'lstNFe',
                responseModelName: 'lstNFe',
                data: { ppiID: ppiID },
                showLoader: true,
                success: function (response) {

                }
            });
        }
    }

    $scope.listarNotasServicoDaProposta = function (ppiID) {

        if (ppiID) {

            var url = Util.getUrl("/proposta/ListarNotasServicoDaProposta");

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'lstNFse',
                responseModelName: 'lstNFse',
                data: { ppiID: ppiID },
                showLoader: true,
                success: function (response) {

                }
            });
        }
    }


    $scope.listarNotasServicoDoPedido = function (ipeID) {

        if (ipeID) {

            var url = Util.getUrl("/pedido/ListarNotasServicoDoPedido");

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'lstNFse',
                responseModelName: 'lstNFse',
                data: { ipeID: ipeID },
                showLoader: true,
                success: function (response) {

                }
            });
        }
    }

    $scope.listarNotasDoPedido = function (ipeID) {

        if (ipeID) {

            var url = Util.getUrl("/pedido/ListarNotasDoPedido");

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'lstNFe',
                responseModelName: 'lstNFe',
                data: { ipeID: ipeID },
                showLoader: true,
                success: function (response) {

                }
            });
        }
    }

    $scope.abrirModalNFe = function (IPE_ID) {

        $scope.ipeID = IPE_ID;
        $scope.tipoModal = 'pedido';

        $scope.tab = 1;
        angular.element("#modal-nfe").modal();
        $scope.listarNotasDoPedido(IPE_ID);
        $scope.listarNotasServicoDoPedido(IPE_ID);
    }

    $scope.abrirModalNFeProposta = function (PPI_ID) {

        $scope.ppiID = PPI_ID;
        $scope.tab = 1;
        $scope.tipoModal = 'proposta';
        angular.element("#modal-nfe").modal();
        $scope.listarNotasDaProposta(PPI_ID);
        $scope.listarNotasServicoDaProposta(PPI_ID);
    }

    $scope.mudarTabNotaFiscal = function () {

        $scope.tab = 1;

        if ($scope.tipoModal == 'pedido') {

            $scope.listarNotasDoPedido($scope.ipeID);
            $scope.listarNotasServicoDoPedido(IPE_ID);

        }
        else if ($scope.tipoModal == 'proposta') {

            $scope.listarNotasDaProposta($scope.ppiID);
        }
    }

    $scope.mudarTabInfLote = function () {

        $scope.tab = 2;

        if ($scope.tipoModal == 'pedido') {
            $scope.listarLoteItens($scope.ipeID);
            $scope.listarLoteItensServico($scope.ipeID);

        }
        else if ($scope.tipoModal == 'proposta') {

            $scope.listarLoteItensDaProposta($scope.ppiID);
            $scope.listarLoteItensDaPropostaServico($scope.ppiID);
        }
    }

    $scope.baixarNotaFiscal = function (nfID) {

        if (nfID) {
            var url = Util.getUrl("/notafiscal/DownloadNFe?nfID=" + nfID);
            post(url, true);
        }
    }

    $scope.baixarNotaFiscalEvento = function (nfEveID) {

        if (nfEveID) {
            var url = Util.getUrl("/notafiscal/DownloadEventoNFe?nfEveID=" + nfEveID);
            post(url, true);
        }
    }

    $scope.baixarNotaFiscalLoteItem = function (loteItemID) {

        if (loteItemID) {
            var url = Util.getUrl("/notafiscal/DownloadNFeDoLote?loteItemID=" + loteItemID);
            post(url, true);
        }
    }

    $scope.abrirModalMsg = function (msg) {

        $scope.mensagemLote = msg;
        angular.element("#erro-descricao").modal();
    };

    $scope.gerarLinkDanfe = function (nfeId) {

        if (nfeId) {

            var url = Util.getUrl("/notaFiscal/gerarLinkDanfe");

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'link',
                responseModelName: 'link',
                data: { nfeId: nfeId },
                showLoader: true,
                success: function (response) {

                    if (response.success) {

                        if ($scope.link && $scope.link.Link) {

                            $window.open($scope.link.Link, '_new');
                        }
                    }
                }
            });
        }
    };


    $scope.executarcallbackLoteItem = function (nflId) {

        if (confirm("Executar novamente os callbacks não executados?")) {

            $scope.objSubmit = { nflId: nflId };
            formHandlerService.submit($scope, {
                url: Util.getUrl("/notaFiscal/ExecutarCallBacksLoteItem"),
                objectName: 'objSubmit',
                showAjaxLoader: true,
                success: function (resp, status, config, message, validationMessage) {

                    $scope.message = message;
                    $scope.erros = validationMessage;
                    $scope.buttonForcarBaixa = 'reset';

                    if (resp.success) {                       
                        $scope.mudarTabInfLote();
                        $timeout(function () {
                            $scope.message = null;
                        }, 1000);
                    }


                }
            });
        }
        else {
            return false;
        }
    }
}
