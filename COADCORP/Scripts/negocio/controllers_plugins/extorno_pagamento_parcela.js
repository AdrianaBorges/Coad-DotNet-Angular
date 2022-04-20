function ExtornoPagamentoParcelaController($scope, formHandlerService, $timeout) {
    $scope.tab = 1;

    $scope.selecionarTab = function (tab) {

        $scope.modalAssinatura = {

            controle: {},
            assinatura: null,
            LstParcelasSelecionadas: [],
            lstContrato: [],
            lstParcelas: []
        };
        $scope.tab = tab;

        if (tab == 1) {

            $scope.carregarAssinaturaDoExtorno($scope.objControle.numeroAssinatura, $scope.objControle.item);
        }
        if (tab == 2) {

            $scope.carregarParcelasExtorno(null, $scope.objControle.item);
        }
    }

    $scope.abrirModalExtorno = function (numeroAssinatura, item) {

        $scope.objControle = {

            numeroAssinatura : numeroAssinatura,
            item : item
        };

        $scope.selecionarTab(1);
    }

    $scope.fecharModalComprovante = function () {

        angular.element("#modal-comprovante").modal('hide');
    }

    $scope.carregarAssinaturaDoExtorno = function (numeroAssinatura, item) {
          
        $scope.carregarDadosDaAssinaturaParaExtorno(numeroAssinatura);


        var dados = {
            numeroAssinatura: numeroAssinatura
        };

        if (item) {
            dados.IPE_ID = item.IPE_ID;
            dados.PPI_ID = item.PPI_ID;
        }

        $scope.carregarContratosDoExtorno(dados);
        $scope.itemSelecionado = item;

        angular.element("#modal-extorno-parcela").modal();
    }
    $scope.carregarDadosDaAssinaturaParaExtorno = function (numeroAssinatura) {

        if (numeroAssinatura) {

            var url = Util.getUrl("/dadosParcelas/carregarDadosDaAssinatura");

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'modalAssinatura.assinatura',
                responseModelName: 'assinatura',
                showAjaxLoader: true,
                data: {
                    numeroAssinatura: numeroAssinatura
                }
            });
        }

    };
    $scope.listarContratosDaAssinatura = function (dados) {

        $scope.listarContratosDoExtorno(1, dados);
    }

    $scope.carregarParcelasExtorno = function (numeroContrato, item, abrirModal) {

        $scope.dadosConsultaParcela = {

            contrato: numeroContrato            
        };

        if (item) {

            $scope.dadosConsultaParcela.ppiId = item.PPI_ID;
            $scope.dadosConsultaParcela.ipeId = item.IPE_ID;
        }
        $scope.carregarDadosDaParcelaExtorno(1, $scope.dadosConsultaParcela, abrirModal);
    }   

    $scope.listarContratosDoExtorno = function (pagina, dadosPesquisaDeContrato) {


        if (dadosPesquisaDeContrato) {

            $scope.dadosPesquisaDeContrato = dadosPesquisaDeContrato;
        }
        else {
            dadosPesquisaDeContrato = $scope.dadosPesquisaDeContrato;
        }

        var url = Util.getUrl("/dadosParcelas/CarregarDadosDoContrato");

        if (pagina) {

            url += "?pagina=" + pagina;
        }

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'modalAssinatura.lstContratos',
            responseModelName: 'lstContratos',
            showAjaxLoader: true,
            data: dadosPesquisaDeContrato,
            pageConfig: { pageName: 'page', pageTargetName: 'pageContratos' },
            success: function (resp) {

            }
        });

    };


    $scope.carregarDadosDaParcelaExtorno = function (pagina, dadosConsultaParcela, abrirModal) {

        if (dadosConsultaParcela) {

            $scope.dadosConsultaParcela = dadosConsultaParcela;
        }
        else {
            dadosConsultaParcela = $scope.dadosConsultaParcela;
        }

        if (dadosConsultaParcela &&
            dadosConsultaParcela.contrato || 
            dadosConsultaParcela.ppiId ||
            dadosConsultaParcela.ipeId) {

            var url = Util.getUrl("/dadosParcelas/ListarParcelasPagas");
            if (pagina) {

                url += "?pagina=" + pagina;
            }

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'modalAssinatura.lstParcelas',
                responseModelName: 'lstParcelas',
                showAjaxLoader: true,
                pageConfig: { pageName: 'page', pageTargetName: 'pageParcelas' },
                data: dadosConsultaParcela
            });
        }

        if(abrirModal == true)
        angular.element("#modal-extorno-parcela-parcelas").modal();
    };

    $scope.carregarContratosDoExtorno = function (dados) {

        $scope.listarContratosDoExtorno(1, dados);
    }

    $scope.abrirModalParcelasSelecionadas = function () {

        angular.element("#modal-parcelas-selecionadas").modal();
    }

    $scope.abrirModalExtornarParcela = function() {

        if (!Util.isPathValid($scope, "modalAssinatura.LstParcelasSelecionadas")) {
            $scope.message = Util.createMessage('fail', 'Selecione ao menos uma parcela antes de continuar.');
            return false;
        }
        if ($scope.modalAssinatura.LstParcelasSelecionadas.length <= 0) {

            $scope.message = Util.createMessage('fail', 'Selecione ao menos uma parcela antes de continuar.');
            return false;
        }
        angular.element("#modal-executar-extorno").modal();
    }

    $scope.extornarParcela = function () {

        if (Util.isPathValid($scope, "modalAssinatura.LstParcelasSelecionadas")) {

            if ($scope.modalAssinatura.LstParcelasSelecionadas.length <= 0) {

                $scope.message = Util.createMessage('fail', 'Selecione ao menos uma parcela antes de continuar.');
                return false;
            }
            if (confirm("Extornar?")) {


                formHandlerService.submit($scope, {
                    url: Util.getUrl("/dadosParcelas/ExtornarVariasParcelas"),
                    objectName: 'modalAssinatura.LstParcelasSelecionadas',
                    showAjaxLoader: true,
                    success: function(resp, status, config, message, validationMessage) {

                        $scope.message = message;
                        $scope.erros = validationMessage;

                        $scope.buttonSave = 'reset';
                        if (resp.success) {

                            $scope.message = Util.createMessage("success", "Parcela extornada com sucesso!");

                            $timeout(function() {
                                $scope.message = null;
                                angular.element("#modal-executar-extorno").modal('hide');
                                angular.element("#modal-extorno-parcela").modal('hide');
                                angular.element("#modal-extorno-parcela-parcelas").modal('hide');
                                $scope.selecionarTab($scope.tab);
                            }, 1000);

                        }

                    }
                });
            }
            else {
                return false;
            }

        }
        else {
            $scope.message = Util.createMessage('fail', 'O objeto de envio não está presente. Recarregue a página e tente novamente.');
            return false;
        }
    }

};
