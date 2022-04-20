appModule.controller('AgendaCobrancaController', function ($scope, formHandlerService, $http, conversionService, $timeout, cepService, $sce) {

    $scope.filtro = {};
    $scope.filtro2 = {};
    
    var now = new Date();
  
    $scope.trustAsHtml = function (string) {
        return $sce.trustAsHtml(string);
    };

    $scope.selecionaAba = function (value) {

        if (value === 1) {
            angular.element("#tab1").trigger("click");
            //$scope.listarAgendaCobranca(pageRequest);

            $scope.tab = "tab1";
        }

        if (value === 2) {
            angular.element("#tab2").trigger("click");
            //$scope.listarAgendamento(pageRequest);
            $scope.tab = "tab2";
        }

        if (value === 3) {
            angular.element("#tab3").trigger("click");
            //$scope.listarAtrasoPrimeiraParcela(pageRequest);
            $scope.tab = "tab3";
        }

        if (value === 4) {
            angular.element("#tab4").trigger("click");
            //$scope.listarAtrasoPrimeiraParcela(pageRequest);
            $scope.tab = "tab4";
        }
    };

    $scope.carregarTela = function () {

        $scope.filtro = {};
        $scope.filtro.opcao = 0;
        $scope.filtro.aberto = true;
        $scope.filtro.dtini = new Date(now.getFullYear(), now.getMonth() - 3, 1);
        $scope.filtro.dtfim = new Date(now.getFullYear(), now.getMonth() + 1, 0);
        $scope.filtro.emp_id = 2;
        $scope.filtro.ordalfabetica = false;
        $scope.filtro.mes = now.getMonth();
        $scope.filtro.ano = now.getFullYear();

        $scope.pesquisarAgenda();


        //angular.element("#clientesematraso").trigger("click");       // 04 - ABA        


        //angular.element("#clienteparcelaliberada").trigger("click"); // 01 - ABA             

        angular.element("#clienteagendado").trigger("click");        // 02 - ABA   

       // angular.element("#clienteparcelaliberada").trigger("click"); // 01 - ABA    

       //angular.element("#clienteatrasoentrada").trigger("click");   // 03 - ABA

       // angular.element("#clientesematraso").trigger("click");       // 01 - ABA        

       angular.element("#tab01").trigger("click"); // 01 - ABA             

    };
       
    $scope.buscaTelAssinatura = function (pageRequest) {

        $scope.abrirModalTelefones($scope.AssinaturaModal, false, pageRequest);

    };

    $scope.confirmarNegociacao = function () {
        angular.element("#Modal-Confirma-Negociacao").modal();
    };

    $scope.abrirModalAtendimento = function (modal) {

        $scope.atendimento = {};
        $scope.atendimento.HAT_ORIGEM_ATEND = "TEL";
        $scope.atendimento.TIP_ATEND_GRUPO = "COB";
        $scope.atendimento.ASN_NUM_ASSINATURA = modal.ASN_NUM_ASSINATURA;
        $scope.atendimento.PAR_NUM_PARCELA = modal.PAR_NUM_PARCELA;
        $scope.atendimento.CLI_ID = modal.CLI_ID;
        $scope.atendimento.CLI_NOME = modal.CLI_NOME;
        $scope.atendimento.HAT_DATA_RESOLUCAO = new Date();
        $scope.atendimento.ACA_ID = 1;
        $scope.atendimento.HAT_IMP_ETIQUETA = 0;
        $scope.atendimento.reagendar = true;

        ///---------------------------------------------------------------------

        $scope.agenda = {};
        $scope.agenda.CLI_ID = modal.CLI_ID;
        $scope.agenda.AGC_DATA_AGENDA = new Date();
        $scope.agenda.AGC_HORA_AGENDA = "8:00";
        $scope.agenda.PAR_NUM_PARCELA = modal.PAR_NUM_PARCELA;
        $scope.agenda.ASN_NUM_ASSINATURA = modal.ASN_NUM_ASSINATURA;
        $scope.agenda.PAR_NUM_PARCELA = modal.PAR_NUM_PARCELA;

        ///---------------------------------------------------------------------

        $scope.buscarTipoAtendimento();
        $scope.buscarDadosTelefone();
        $scope.BuscarTitulosVencidos(modal.CLI_ID, modal.ASN_NUM_ASSINATURA);

        angular.element("#Modal-Atendimento").modal();
    };

    $scope.abrirModalEmail = function (_asn_id) {

        showAjaxLoader();

        var url = "/Cliente/BuscarEmails";
        $http({
            url: url,
            method: "post",
            data: { _assinatura: _asn_id }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success === true) {

                $scope.AssinaturaModal = _asn_id;
                $scope.listaemail = response.result.listaemail;
                conversionService.deepConversion($scope.listaemail);

                angular.element("#Modal-Atualizar-Email").modal();

            }
            else {

                alert(response.message.message);
            }

        }).error(function (response) {

            alert(response);

            hideAjaxLoader();
        });

    };

    $scope.buscarDadosTelefone = function (_pageRequest) {

        showAjaxLoader();

        var url = "/Cliente/BuscarTelefones";
        $http({
            url: url,
            method: "post",
            data: { _assinatura: $scope.atendimento.ASN_NUM_ASSINATURA, pagina: _pageRequest }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success === true) {

                $scope.AssinaturaModal = $scope.atendimento.ASN_NUM_ASSINATURA;
                $scope.listatelefone = response.result.listatelefone;
                conversionService.deepConversion($scope.listatelefone);

                $scope.pagina03 = response.page;

            }
            else {

                $scope.message = response.message;
            }

        }).error(function (response) {

            $scope.message = response.message;

            hideAjaxLoader();
        });

    };

    $scope.buscarTipoAtendimento = function () {

        showAjaxLoader();

        var url = "/Cliente/BuscarTipoAtendimento";
        $http({
            url: url,
            method: "post",
            data: { _grupo: $scope.atendimento.TIP_ATEND_GRUPO }
        }).success(function (retorno) {

            hideAjaxLoader();

            if (retorno.success) {

                $scope.lstTipoAtendimento = retorno.result.lstTipoAtendimento;
                conversionService.deepConversion($scope.lstTipoAtendimento);

                $scope.atendimento.TIP_ATEND_ID = 111;
            }
            else {

                $scope.message = response.message;
            }

        }).error(function (response) {

            $scope.message = response.message;

            hideAjaxLoader();
        });

    }

    $scope.buscarContasBanco = function () {

        showAjaxLoader();

        var url = Util.getUrl("/Remessa/BuscarContasBanco");
        $http({
            url: url,
            method: "post",
            data: { bco: $scope.filtro.bco, cta_emite_boleto: true }
        }).success(function (response) {

            if (response.success === true) {

                $scope.listacontas = response.result.listacontas;
                conversionService.deepConversion($scope.listacontas);
            }
            else {

                $scope.erro = response.message.message;

                angular.element("#Modal-Erro").modal();

                hideAjaxLoader();

            }

            hideAjaxLoader();


        }).error(function (response) {

            $scope.erro = response;

            angular.element("#Modal-Erro").modal();

            hideAjaxLoader();

        });
    };

    $scope.abrirModalTelefones = function (modal, abrir, pageRequest) {

        showAjaxLoader2();

        var url = "/Cliente/BuscarTelefones";
        $http({
            url: url,
            method: "post",
            data: { _assinatura: modal.ASN_NUM_ASSINATURA, pagina: pageRequest }
        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success === true) {

                $scope.listatelefone = response.result.listatelefone;
                conversionService.deepConversion($scope.listatelefone);

                if (abrir === true)
                    angular.element("#Modal-Telefones").modal();

                $scope.pagina01 = response.page;

            }
            else {

                $scope.message = response.message;
            }

        }).error(function (response) {

            $scope.message = response.message;

            hideAjaxLoader2();
        })
        
    }

    $scope.abrirModalReagendamento = function (modal) {

        $scope.agendaANT = null;

        if (modal.AGC_ID !== null) {
            $scope.agendaANT = modal;
        }

        else {
            $scope.agendaANT = {};
            $scope.agendaANT.CLI_ID = modal.CLI_ID;
            $scope.agendaANT.AGC_ID = modal.AGC_ID;
            $scope.agendaANT.ASN_NUM_ASSINATURA = modal.ASN_NUM_ASSINATURA;
            $scope.agendaANT.AGC_ASSUNTO = modal.AGC_ASSUNTO;
            $scope.agendaANT.CLI_NOME = modal.CLI_NOME;
            $scope.agendaANT.AGC_DATA_AGENDA = modal.AGC_DATA_AGENDA;

        }

        $scope.agenda = {};
        $scope.agenda.CLI_ID = modal.CLI_ID;
        $scope.agenda.AGC_HORA_AGENDA = "8:00";
        $scope.agenda.AGC_DATA_AGENDA = new Date();
        $scope.agenda.PAR_NUM_PARCELA = modal.PAR_NUM_PARCELA;
        $scope.agenda.ASN_NUM_ASSINATURA = modal.ASN_NUM_ASSINATURA;
        $scope.agenda.AGC_REAGENDAMENTO = modal.AGC_ID;
        ///--------------
        $scope.BuscarTitulosVencidos(modal.CLI_ID, modal.ASN_NUM_ASSINATURA);
        ///--------------

        angular.element("#Modal-Reagendamento").modal();

    };

    $scope.abrirModalHistorico = function (_clienteid, pageRequest) {

        showAjaxLoader();

        var url = "/AgendaCobranca/BuscarHistorico";
        $http({
            url: url,
            method: "post",
            data: { _cli_id: _clienteid, _pagina: pageRequest, _registroPorPagina: 6 }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success === true) {

                $scope.listaatend = response.result.listaatend;
                conversionService.deepConversion($scope.listaatend);

                $scope.pagina02 = response.page;

                angular.element("#Modal-Historico").modal();

            }
            else {

                $scope.message = response.message;
            }

        }).error(function (response) {

            $scope.message = response.message;

            hideAjaxLoader();
        });

    };

    $scope.listaDebitoDetalhadamente = function (pageRequest) {

        showAjaxLoader2();

        var url = "/AgendaCobranca/BuscarDebitoDetalhadamente";
        $http({
            url: url,
            method: "post",
            data: {
                assinatura: $scope.filtro.assinatura
                , cliente: $scope.filtro.cliente
                , pagina: pageRequest
            }
        }).success(function (response) {

            if (response.success) {

                $scope.lstDetalhesDebito = response.result.lstDetalhesDebito;
                conversionService.deepConversion($scope.lstDetalhesDebito);
                $scope.page = response.page;
            }
            else {

                $scope.lstDetalhesDebito = null;

                $scope.message = response.message;
            }

            hideAjaxLoader2();

        }).error(function (response) {

            $scope.message = response.message;

            hideAjaxLoader2();
        });

    };

    $scope.listarAgendaCobranca = function (pageRequest) {

        showAjaxLoader2();
        $scope.filtro.primeiraParcela = 0;

        var url = "/AgendaCobranca/BuscarTitulosAtraso";
        $http({
            url: url,
            method: "post",
            data: {
                assinatura: $scope.filtro.assinatura
                , cliente: $scope.filtro.cliente
                , atendente: $scope.filtro.atendente
                , cnpj: $scope.filtro.cnpj
                , dataini: $scope.filtro.dtini
                , datafim: $scope.filtro.dtfim
                , pagina: pageRequest
                , primeiraParcela: $scope.filtro.primeiraParcela
            }
        }).success(function (response) {

            if (response.success) {

                $scope.lstagendacobranca = response.result.lstagendacobranca;
                conversionService.deepConversion($scope.lstagendacobranca);
                $scope.pagina01 = response.page;
            }
            else {

                $scope.lstagendacobranca = null;

                $scope.message = response.message;
            }

            hideAjaxLoader2();

        }).error(function (response) {

            $scope.message = response.message;

            hideAjaxLoader2();
        });

    };

    $scope.ParcelaLiberadaSitIrregular = function (_clienteid, pageRequest) {

        showAjaxLoader();

        var url = "/AgendaCobranca/ParcelaLiberadaSitIrregular";
        $http({
            url: url,
            method: "post",
            data: { _cli_id: _clienteid, _pagina: pageRequest, _registroPorPagina: 6 }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success === true) {

                $scope.lstliberacaoindevida = response.result.lstliberacaoindevida;
                conversionService.deepConversion($scope.lstliberacaoindevida);

                $scope.pagina02 = response.page;

            }
            else {

                $scope.message = response.message;
            }

        }).error(function (response) {

            $scope.message = response.message;

            hideAjaxLoader();
        });

    };

    $scope.BuscarHistorico = function (_clienteid, pageRequest) {

        showAjaxLoader();

        var url = "/AgendaCobranca/BuscarHistorico";
        $http({
            url: url,
            method: "post",
            data: { _cli_id: _clienteid, _pagina: pageRequest, _registroPorPagina: 6 }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success === true) {

                $scope.listaatend = response.result.listaatend;
                conversionService.deepConversion($scope.listaatend);

                $scope.pagina02 = response.page;

            }
            else {

                $scope.message = response.message;
            }

        }).error(function (response) {

            $scope.message = response.message;

            hideAjaxLoader();
        });

    };
        
    $scope.salvarReagendamento = function (agenda) {

        showAjaxLoader2();
        agenda.PAR_NUM_PARCELA = $scope.agenda.PAR_NUM_PARCELA;

        var url = "/AgendaCobranca/GravarAgendamento";
        $http({
            url: url,
            method: "post",
            data: { _agenda: agenda}
        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success === true) {

                $scope.message = Util.createMessage("success", "Agendamento realizado com sucesso!!");

                $scope.pesquisarAgenda();
            }
            else {

                if (response.message !== null)
                    $scope.message = Util.createMessage("fail", response.message.message);
                else
                    $scope.message = Util.createMessage("fail", response);

            }

        }).error(function (response) {

            $scope.message = response.message;

            hideAjaxLoader2();
        });

    };

    $scope.pesquisarAgenda = function (pageRequest, opcao) {

        if (opcao !== null)
            $scope.filtro.opcao = opcao;

        $scope.lstagendamento = null;
        $scope.lstagendacobranca = null;
        $scope.lstCobrancaPrimeiraParcela = null;

        $scope.listarAgendaCobranca(pageRequest);
        $scope.listarAgendamento(pageRequest);
        $scope.listarAtrasoPrimeiraParcela(pageRequest);
        $scope.listarParcelaLiberada(pageRequest);

    };

    // Retirada de parcela do código 9 em lote ---------------Início

    $scope.ativarModoBatchParcela = function () {

        $scope.parcelaBatchModal = {
            ativo: true,
            aberto: false,
            controle: {}
        };

        $scope.parcelaBatchModal.lstparcelaliberada = [];

    };

    $scope.alteraSituacaoParcelas = function () { // Antigo código nove

        if ($scope.parcelaBatchModal.lstparcelaliberada.length <= 0) {
            $scope.message = Util.createMessage('fail', 'Selecione ao menos uma parcela antes de continuar.');
            return false;

        }

        if (confirm("Alterar a situação da(s) parcela(s) 9?")) {

            formHandlerService.submit($scope, {
                url: Util.getUrl("/Parcelas/AlteraSituacaoParcelas"),
                objectName: 'parcelaBatchModal.lstparcelaliberada',
                showAjaxLoader: true,
                success: function (resp, status, config, message, validationMessage) {

                    $scope.message = message;
                    $scope.erros = validationMessage;

                    $scope.buttonSave = 'reset';
                    if (resp.success) {

                        $scope.message = Util.createMessage("success", "Parcela extornada com sucesso!");

                        $timeout(function () {
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

    };

    // Fim da retirada de parcela do código 9 ----------------------

    $scope.BuscarTitulosVencidos = function (_cliente, _assinatura) {

        showAjaxLoader2();

        if (_cliente === undefined)
            _cliente = null;

        var url = "/AgendaCobranca/BuscarTitulosVencidos";
        $http({
            url: url,
            method: "post",
            data: {
                _cli_id: _cliente
                , _asn_num_assinatura: _assinatura
            }
        }).success(function (response) {

            if (response.success === true) {

                $scope.totaldebito = response.result.totaldebito;
                $scope.lstTitulosVencidos = response.result.lstTitulosVencidos;
                $scope.lstNegociacao = response.result.lstNegociacao;

                conversionService.deepConversion($scope.lstTitulosVencidos);
                conversionService.deepConversion($scope.lstNegociacao);
                $scope.page = response.page;
            }
            else {

                $scope.lstagendacobranca = null;

                $scope.message = response.message;
            }

            hideAjaxLoader2();

            //angular.element("#tab1").trigger("click");


        }).error(function (response) {

            $scope.message = response.message;

            hideAjaxLoader2();
        });

    };

    $scope.BuscarDebitoDetalhadamente = function (_assinatura, _cliente) {

        showAjaxLoader2();

        if (_cliente === undefined)
            _cliente = null;

        var url = "/AgendaCobranca/BuscarTitulosVencidos";
        $http({
            url: url,
            method: "post",
            data: {
                _cli_id: _cliente
                , _asn_num_assinatura: _assinatura
            }
        }).success(function (response) {

            if (response.success === true) {

                $scope.totaldebito = response.result.totaldebito;
                $scope.lstTitulosVencidos = response.result.lstTitulosVencidos;
                $scope.lstNegociacao = response.result.lstNegociacao;

                conversionService.deepConversion($scope.lstTitulosVencidos);
                conversionService.deepConversion($scope.lstNegociacao);
                $scope.page = response.page;
            }
            else {

                $scope.lstagendacobranca = null;

                $scope.message = response.message;
            }

            hideAjaxLoader2();

            angular.element("#tab1").trigger("click");


        }).error(function (response) {

            $scope.message = response.message;

            hideAjaxLoader2();
        });

    };

    $scope.listarAtrasoPrimeiraParcela = function (pageRequest) {

        showAjaxLoader2();
        $scope.filtro.primeiraParcela = 1;

        var url = "/AgendaCobranca/BuscarTitulosAtrasoPrimeiraParcela";
        $http({
            url: url,
            method: "post",
            data: {
                assinatura: $scope.filtro.assinatura
                , cliente: $scope.filtro.cliente
                , atendente: $scope.filtro.atendente
                , cnpj: $scope.filtro.cnpj
                , dataini: $scope.filtro.dtini
                , datafim: $scope.filtro.dtfim
                , pagina: pageRequest
                , primeiraParcela: $scope.filtro.primeiraParcela
            }
        }).success(function (response) {

            if (response.success) {

                $scope.lstprimeiraparcela = response.result.lstprimeiraparcela;
                conversionService.deepConversion($scope.lstprimeiraparcela);
                $scope.pagina03 = response.page;
            }
            else {

                $scope.lstprimeiraparcela = null;

                $scope.message = response.message;
            }

            hideAjaxLoader2();

        }).error(function (response) {

            $scope.message = response.message;

            hideAjaxLoader2();
        });

    };

    $scope.listarParcelaLiberada = function (pageRequest) {

        showAjaxLoader2();

        var url = "/AgendaCobranca/BuscarTitulosComParcelaLiberada";
        $http({
            url: url,
            method: "post",
            data: {
                assinatura: $scope.filtro.assinatura
                , cliente: $scope.filtro.cliente
                , atendente: $scope.filtro.atendente
                , cnpj: $scope.filtro.cnpj
                , dataini: $scope.filtro.dtini
                , datafim: $scope.filtro.dtfim
                , pagina: pageRequest
            }
        }).success(function (response) {

            if (response.success) {

                $scope.lstparcelaliberada = response.result.lstparcelaliberada;
                conversionService.deepConversion($scope.lstparcelaliberada);
                $scope.pagina04 = response.page;
            }
            else {

                $scope.lstparcelaliberada = null;

                $scope.message = response.message;
            }

            hideAjaxLoader2();

        }).error(function (response) {

            $scope.message = response.message;

            hideAjaxLoader2();
        });

    };

    $scope.listarAgendamento = function (pageRequest) {

        showAjaxLoader2();

        var url = "/AgendaCobranca/BuscarAgendamento";
        $http({
            url: url,
            method: "post",
            data: {
                assinatura: $scope.filtro.assinatura
                , cliente: $scope.filtro.cliente
                , atendente: $scope.filtro.atendente
                , cnpj: $scope.filtro.cnpj
                , dataini: $scope.filtro.dtini
                , datafim: $scope.filtro.dtfim
                , pendente: $scope.filtro.aberto
                , pagina: pageRequest
                , registroPorPagina: null
            }
        }).success(function (response) {

            if (response.success) {

                $scope.lstagendamento = response.result.lstagendamento;
                conversionService.deepConversion($scope.lstagendamento);
                $scope.pagina02 = response.page;


            }
            else {

                $scope.lstagendamento = null;

                $scope.message = response.message;
            }

            hideAjaxLoader2();

        }).error(function (response) {

            $scope.message = response.message;

            hideAjaxLoader2();
        });

    };

    $scope.emitirBoleto = function () {

        showAjaxLoader2();

        var url = "/cobrancaEscritural/EmitirBoleto";
        $http({
            url: url,
            method: "post",
            data: {
                _parcela: $scope.avulso,
                idConta: $scope.avulso.ctaId,
                msg: $scope.avulso.msg
            }
        }).success(function (response) {

            if (response.success === true) {

                $scope.message = response.message;
            }
            else {

                $scope.message = response.message;
            }

            hideAjaxLoader2();

        }).error(function (response) {

            $scope.message = response.message;

            hideAjaxLoader2();
        });

    };
  
    $scope.listar = function (pageRequest) {

        showAjaxLoader2();

        var idtabela = $scope.filtro.TDC_ID;

        if ($scope.filtro.TIPO === 2) {
            idtabela = null;
        }

        var url = "/AcessoTabelas/Pesquisar";
        $http({
            url: url,
            method: "post",
            data: { _mes: $scope.filtro.mes, _ano: $scope.filtro.ano, _tdc_id: idtabela, _tipo: $scope.filtro.TIPO }
        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success === true) {

                $scope.listaAcessoTabelas = response.result.listaAcessoTabelas;
                conversionService.deepConversion($scope.listaAcessoTabelas);

            }
            else {

                $scope.listaAcessoTabelas = null;

                $scope.message = response.message;
            }

        }).error(function (response) {

            $scope.message = response.message;

            hideAjaxLoader2();
        });

    };
  
    if (window.BoletoAvulsoController !== undefined) {

        BoletoAvulsoController($scope, formHandlerService, $http, conversionService, $timeout);
    }

    $scope.listarParcelas = function (pagina) {

        $scope.paginaAtual = pagina;

        var url = Util.getUrl("/agendacobranca/ListarParcelasSituacaoIndevida");

    };

    $scope.abrirModalParcelaIrregular = function (pageRequest) {

        showAjaxLoader();

        var url = "/AgendaCobranca/ParcelaLiberadaSitIrregular";
        $http({
            url: url,
            method: "post",
            data: { _pagina: pageRequest, _registroPorPagina: 6 }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success === true) {

                $scope.lstliberacaoindevida = response.result.lstliberacaoindevida;
                conversionService.deepConversion($scope.lstliberacaoindevida);

                $scope.pagina02 = response.page;

                angular.element("#Modal-ParcelaSituacaoIrregular").modal();

            }
            else {

                $scope.message = response.message;
            }

        }).error(function (response) {

            $scope.message = response.message;

            hideAjaxLoader();
        });

    };

    $scope.cancelarAcao = function () {

        $scope.parcelaBatchModal = null;
    };

    $scope.queryParcelasSelecionadas = {
        selected: true
    };


    $scope.abrirModalCodigoParcelasSelecionadas = function () {

        angular.element("#modal-parcelas-selecionadas").modal();
    };

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



  
});
