

function ComprovanteController($scope, formHandlerService, $http, $timeout) {

    $scope.fecharModalComprovante = function () {

        angular.element("#modal-comprovante").modal('hide');
    }


    $scope.listarItemPropostaPedidoComprovante = function (itemPropostaPedido, tipo) {

        if (itemPropostaPedido) {
            var data = {};
            if (tipo == 'proposta' && itemPropostaPedido.PPI_ID) {

                data.ppiId = itemPropostaPedido.PPI_ID;
            }
            else if (tipo == 'pedido' && itemPropostaPedido.IPE_ID) {

                data.ipeId = itemPropostaPedido.IPE_ID;
            }
            
            itemPropostaPedido.PROPOSTA_ITEM_COMPROVANTE = [];

            var url = Util.getUrl("/comprovantes/listarPropostaItemComprovante");
            formHandlerService.read(itemPropostaPedido, {
                url: url,
                targetObjectName: 'PROPOSTA_ITEM_COMPROVANTE',
                responseModelName: 'lstItemPropostaPedidoComprovante',
                //pageConfig: { pageName: 'page', pageTargetName: 'paginaHistorico' },
                data: data,
                showAjaxLoader: true,
                success: function () {
                    $scope.comprovanteCarregado = true;
                }
            });
        }

    }

    $scope.salvarComprovantes = function () {
        var url = Util.getUrl("/comprovantes/salvarPropostaItemComprovanteProposta");
        
        if($scope.modalComprovante.tipo == 'proposta')
            url = Util.getUrl("/comprovantes/salvarPropostaItemComprovanteProposta");
        else if($scope.modalComprovante.tipo == 'pedido')
            url = Util.getUrl("/comprovantes/salvarPropostaItemComprovantePedido");

        formHandlerService.submit($scope, {
            url: url,
            objectName: 'modalComprovante',
            showAjaxLoader: true,
            success: function (resp, status, config, message, validationMessage) {

                $scope.message = message;
                $scope.erros = validationMessage;

                $scope.buttonComprovante = 'reset';
                if (resp.success) {

                    $scope.message = Util.createMessage("success", "Dados do comprovante atualizados com sucesso!");

                    $timeout(function () {
                        $scope.message = null;
                        $scope.fecharModalComprovante();

                    }, 1000);

                }

            }
        });
    }


    $scope.fecharCollapseComprovante = function () {

        $timeout(function () {
            if ($scope.modalComprovante) {

                if ($scope.modalComprovante.PROPOSTA_ITEM_COMPROVANTE &&
                    $scope.modalComprovante.PROPOSTA_ITEM_COMPROVANTE.length > 0) {

                    var length = $scope.modalComprovante.PROPOSTA_ITEM_COMPROVANTE.length;

                    for (var index = 0; index <= length; index++) {

                        var selector = '#body_comprovante_' + index;
                        if (angular.element(selector).hasClass('in'))
                            angular.element(selector).collapse('hide');
                    }

                }
            }
        });
    }

    
    $scope.abrirModalComprovante = function (itemPropostaPedido, tipo) {
        $scope.modalComprovante = itemPropostaPedido;
        $scope.modalComprovante.tipo = tipo;

        $scope.listarItemPropostaPedidoComprovante(itemPropostaPedido, tipo);
        angular.element("#modal-comprovante").modal({backdrop: 'static'});
    }

    $scope.adicionarComprovante = function () {

        if ($scope.modalComprovante) {

            var codigoTipoPagamento = $scope.modalComprovante.TIPO_PAGAMENTO.CodigoPagamento;
            var tipoPagamento = { NomeTipoPagamento: $scope.modalComprovante.TIPO_PAGAMENTO.NomeTipoPagamento };

            if (!$scope.modalComprovante.PROPOSTA_ITEM_COMPROVANTE) {

                $scope.modalComprovante.PROPOSTA_ITEM_COMPROVANTE = [{ TPG_ID: codigoTipoPagamento, TIPO_PAGAMENTO : tipoPagamento}];
            }
            else
            {
                    $scope.fecharCollapseComprovante();
                    $scope.modalComprovante.PROPOSTA_ITEM_COMPROVANTE.push({ TPG_ID: codigoTipoPagamento, TIPO_PAGAMENTO: tipoPagamento });
            }

            $timeout(function () {
                // abre o último elemento
                var length = $scope.modalComprovante.PROPOSTA_ITEM_COMPROVANTE.length - 1;
                angular.element("#body_comprovante_" + (length)).collapse('show');
            });

       }
    }

    $scope.removerComprovante = function ($index, $event) {

        $event.stopPropagation();
        if ($scope.modalComprovante) {

            if ($scope.modalComprovante.PROPOSTA_ITEM_COMPROVANTE) {

                $scope.modalComprovante.PROPOSTA_ITEM_COMPROVANTE.splice($index, 1);
            }
        }
    }   

};
