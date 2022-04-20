function GerenciarAssinaturaController($scope, formHandlerService, $timeout) {

    $scope.initAssinatura = function () {

        $scope.objAssinatura = {};
    }

    $scope.abrirModalCliente = function (cliId) {

        $scope.objAssinatura = { CLI_ID: cliId };
        $scope.numeroAssinatura = null;
        $scope.listarAssinaturas();
        angular.element("#modal-assinatura").modal();

    }

    $scope.listarAssinaturas = function (pagina) {

        if($scope.objAssinatura)
            $scope.objAssinatura.lstAssinatura = [];

        var cliId = $scope.objAssinatura.CLI_ID;
        
        var url = Util.getUrl("/gerenciaAssinatura/listarAssinaturas");

        if (pagina) {

            url += "?pagina=" + pagina;
        }

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'objAssinatura.lstAssinatura',
            responseModelName: 'lstAssinatura',
            showAjaxLoader: true,
            data: {
                cliId: cliId,
                registrosPorPagina: 5,
            },
            pageConfig: { pageName: 'page', pageTargetName: 'pageAssinatura' },
            success: function (resp) {

            }
        });
        
    }


    $scope.carregarAssinatura = function (numeroAssinatura, item) {

        var ipeId = null;

        if (item) {

            $scope.itemSelecionado = item;
            ipeId = item.IPE_ID;
        }        

        $scope.objAssinatura.numeroAssinatura = numeroAssinatura;
        $scope.objAssinatura.lstContratos = null;
        $scope.objAssinatura.lstParcelas = null;
        $scope.carregarDadosDaAssinatura(numeroAssinatura);
        $scope.carregarContratos(numeroAssinatura, ipeId);

        //angular.element("#modal-assinatura").modal();
    }


    $scope.carregarContratos = function (numeroAssinatura, ipe_id) {

        $scope.listarContratos(1, numeroAssinatura, ipe_id);
        $scope.listarContratosGeradosNoPedido(1, numeroAssinatura, ipe_id);
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
                IPE_ID: ipe_id
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

        var url = Util.getUrl("/gerenciaAssinatura/CarregarDadosDoContrato");

        if (pagina) {

            url += "?pagina=" + pagina;
        }

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'objAssinatura.lstContratos',
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

            var url = Util.getUrl("/gerenciaAssinatura/CarregarDadosDaParcela");
            if (pagina) {

                url += "?pagina=" + pagina;
            }

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'objAssinatura.lstParcelas',
                responseModelName: 'lstParcelas',
                showAjaxLoader: true,
                pageConfig: { pageName: 'page', pageTargetName: 'pageParcelas' },
                data: {
                    numeroContrato: numeroContrato
                }
            });
        }
    };

    $scope.carregarDadosDaAssinatura = function (numeroAssinatura) {

        if (numeroAssinatura) {

            var url = Util.getUrl("/pedido/carregarDadosDaAssinatura");

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'objAssinatura.Assinatura',
                responseModelName: 'assinatura',
                showAjaxLoader: true,
                data: {
                    numeroAssinatura: numeroAssinatura
                }
            });
        }

    };

    $scope.carregarParcelas = function (numeroContrato, idRetorno) {

        $scope.retornoParcela = idRetorno;
        $scope.carregarDadosDaParcela(1, numeroContrato);
    }

    $scope.abrirModalTransferenciaAssinatura = function (assinatura) {

        $scope.processoTransfAssiDTO = { CodAssinaturaOrigem : assinatura};
        angular.element("#modal-transferencia-assinatura").modal();
    }
    
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

    $scope.abrirModalProduto = function () {
        
        $scope.listarProdutoComposicao();
        angular.element("#modal-produto-composicao").modal();    
    }

    $scope.removerProduto = function () {

        $scope.processoTransfAssiDTO.CodProduto = null;
        $scope.processoTransfAssiDTO.ProdutoComposicao = null;
    }

    $scope.adicionarProduto = function (pro) {

        $scope.processoTransfAssiDTO.ProdutoComposicao = pro;
        $scope.processoTransfAssiDTO.CodProduto = pro.CMP_ID;
        angular.element("#modal-produto-composicao").modal('hide');
    }

    if (window.ExtornoPagamentoParcelaController !== undefined) {
        ExtornoPagamentoParcelaController($scope, formHandlerService, $timeout);
    }

    $scope.migrarAssinatura = function () {

        if (confirm("Deseja realmente gerar migrar a assinatura?")) {

            formHandlerService.submit($scope, {
                url: Util.getUrl("/gerenciaAssinatura/migrarAssinatura"),
                objectName: 'processoTransfAssiDTO',
                showAjaxLoader: true,
                success: function (resp, status, config, message, validationMessage) {

                    $scope.message = message;
                    $scope.erros = validationMessage;
                    $scope.buttonTransfe = 'reset';

                    if (resp.success) {
                        var codNovaAssinatura = resp.result.codNovaAssinatura;
                        $scope.message = Util.createMessage("success", "Assinatura migrada com sucesso! Código Gerado: '" + codNovaAssinatura + "'.");

                        $timeout(function () {
                            $scope.message = null;
                            $scope.listarAssinaturas();
                            angular.element("#modal-transferencia-assinatura").modal('hide');
                        }, 1500);
                    }

                }
            });
        }
        else {
            return false;
        }
    }


}
