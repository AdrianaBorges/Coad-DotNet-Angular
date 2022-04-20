function InfoClienteController($scope, formHandlerService, $http, conversionService, $timeout) {

    $scope.listarAgendamento = true;

    $scope.initInfoCliente = function (abreModal, CLI_ID) {

        $scope.tab = 1;        
        $scope.listarHorarios();
        $scope.carregarTiposDeCliente();
        if (abreModal == 'True' && CLI_ID) {
            
            var executa = true;
            $scope.CLI_ID = CLI_ID;
            $scope.$watch("inited", function (value) {

                if (value === true && executa === true) {

                    executa = false;
                    $scope.abreModalCliente(CLI_ID);
                }
            });          
        }

    }

    $scope.selecionarTab = function (value) {

        $scope.tab = value;
    }

    $scope.listarHorarios = function () {

            var url = Util.getUrl("/franquia/agendamento/listarhorarios");          

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'horarios',
                responseModelName: 'horarios',
                showLoader: true,
                success: function () {

                }
            });

    }

    $scope.abreModalCliente = function (CLI_ID, modalId) {

        $scope.tab = 1;
        $scope.clienteModal = {};
        $scope.modalId = (modalId != null) ? modalId : "#modal-cliente";

        if (!$scope.horarios) {

            $scope.listarHorarios();
        }

        if (CLI_ID) {
            
            $scope.CLI_ID = CLI_ID;
            $scope._carregarDadosDoCliente(CLI_ID);
            angular.element($scope.modalId).modal();
        } 

    }


    $scope.abreModalClienteLayout = function (CLI_ID) {

        $scope.abreModalCliente(CLI_ID, "#modal-cliente-layout");
    }

    $scope._carregarDadosDoCliente = function (CLI_ID) {

        if (CLI_ID) {
            
            var url = Util.getUrl("/franquia/clientes/RecuperarDadosDoClienteParaConfiguracao");

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'clienteModal',
                responseModelName: 'cliente',
                data: { clienteId: CLI_ID },
                showLoader: true,
                success: function (response) {

                    if ($scope.clienteModal) {

                        var label = null;
                        
                        $scope.podeEditar = response.result.podeEditar;
                        switch ($scope.clienteModal.CLA_CLI_ID) {

                            case 1: { label = "Prospect"; break; }
                            case 2: { label = "Cliente"; break; }
                        }

                        if ($scope.clienteModal.ClienteExisteNaAgenda == false) {

                            $scope.message = Util.createMessage("warning", "O cliente selecionado não pertece a essa agenda. Para importa-lo, clique em 'Importar Cliente'");
                        }
                        $scope.listarRepresentantesDoClienteInformacao();
                        $scope.buttonEvoluirConfig = [{ label: 'Evoluir para ' + label, state: 'evo' }, { label: 'Alterando...', state: 'alt', disabled: true }];
                    }
                }
            });
        }
    }

    $scope.abreAbaAgendarContato = function () {
        
        var CLI_ID = ($scope.clienteModal && $scope.clienteModal.CLI_ID)
        $scope.tab = 2;
        $scope.agendamento = { CLI_ID: $scope.clienteModal.CLI_ID, HISTORICO_ATENDIMENTO: [{HAT_DESCRICAO : null}] };
    }

    $scope.agendarContato = function () { 
    
        $scope.objEnvio = angular.copy($scope.agendamento);

        
        if (!$scope.agendamento.AGE_DATA_AGENDAMENTOMask) {
            $scope.buttonAgendar = 'agendar';
            $scope.objEnvio.AGE_DATA_AGENDAMENTO = null;            
        }

        if (!$scope.agendamento.AGE_DATA_AGENDAMENTOTime) {
                      
            $scope.buttonAgendar = 'agendar';
            $scope.objEnvio.AGE_DATA_AGENDAMENTO = null;            
        }
               
        formHandlerService.submit($scope, {
            url: Util.getUrl("/franquia/agendamento/criaragendamento"),
            objectName: 'objEnvio',
            showLoader: true,
            success: function (resp, status, config, message, validationMessage) {

                $scope.objEnvio = null;
                $scope.message = message;
                $scope.erros = validationMessage;

                $scope.buttonAgendar = 'agendar';
                if (resp.success) {

                    if ($scope.loadAll) {

                        $scope.loadAll();
                    }
                    $scope._carregarDadosDoCliente($scope.clienteModal.CLI_ID);

                    $scope.message = Util.createMessage("success", "Salvo com sucesso!");
                    $scope.agendamento = {};
                    $timeout(function () {
                        
                        $scope.message = null;                        
                        if ($scope.tab) {

                            $scope.tab = 1;
                        }

                    }, 1000);
                   
                }

            }
        });
    }

    $scope.abreAbaHistoricoCliente = function () {
               
        $scope.tab = 3;      
        $scope.listarHistoricos();
    }



    $scope.listarHistoricos = function (paginaReq) {

        $scope.lstHistoricos = [];
        var CLI_ID = ($scope.clienteModal && $scope.clienteModal.CLI_ID);
        var url = Util.getUrl("/historico/listarhistoricocliente");        
       
        if (paginaReq) {

            url += "?pagina=" + paginaReq;
        }

        var data = null;
        if ($scope.filtro && ($scope.filtro.dataInicial || $scope.filtro.dataInicial)) {

            data = angular.copy($scope.filtro);
            data.CLI_ID = CLI_ID;
        }
        else {
            data = { CLI_ID: CLI_ID };
        }
       

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstHistoricos',
            responseModelName: 'lstHistoricos',
            pageConfig: { pageName: 'page', pageTargetName: 'paginaHistorico' },
            data : data,
            showAjaxLoader: true,
            success: function () {
            }
        });


    }

    $scope.mostrarHistoricoGeral = function () {
               
        if ($scope.clienteModal && $scope.clienteModal.CLI_ID) {

            var CLI_ID = $scope.clienteModal.CLI_ID;
            var url = Util.getUrl("/franquia/historico/mostrartudo?CLI_ID=" + CLI_ID);
            post(url, true);
        }
        
    }

    $scope.abreAbaListaDeAgendamentos = function () {

        $scope.listarAgendamento = true;
        $scope.tab = 4;
        $scope.listarAgendamentos();
    }

    $scope.listarAgendamentos = function (pagina) {

        $scope.lstHistoricos = [];
        var CLI_ID = $scope.clienteModal.CLI_ID;

        var data = null;
        $scope.dataAtual = new Date();
        if ($scope.filtro && ($scope.filtro.dataInicial || $scope.filtro.dataInicial)) {

            data = angular.copy($scope.filtro);
            data.CLI_ID = CLI_ID;
        }
        else {
            data = { CLI_ID: CLI_ID };
        }

        var url = Util.getUrl("/agendamento/ListarAgendamentosPorClienteEOperadora");

        if (pagina) {

            url += "?pagina=" + pagina;
        }

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstAgendamento',
            responseModelName: 'lstAgendamento',
            showAjaxLoader: true,
            pageConfig: { pageName: 'page' /*, pageTargetName: 'pageAgendamentoDoDia' */},
            data: data,
            success: function (resp) {

            }
        });
    }

    $scope.abreFormConfirmacaoAgendamento = function(item){
       
        $scope.abreModal("#modalformAgendamento");
        $scope.acaoAgendar = "confirmação";
        $scope.agendamentoForm = item;
        $scope.agendamentoForm.HISTORICO_ATENDIMENTO = [{}];
        
    }

    $scope.abreFormReagendar = function (item) {

        $scope.erros = null;
        $scope.abreModal("#modalformAgendamento");
        $scope.agendamentoForm = angular.copy(item);

        //item.AGE_DATA_AGENDAMENTO = null;
        $scope.acaoAgendar = "reagendar";
        $scope.agendamentoForm.HISTORICO_ATENDIMENTO = [{}];

    }

    $scope.atualizarAgendamento = function () {

        var url = null;
        var messageSuccess = null;


        if ($scope.acaoAgendar == "confirmação") {
            url = Util.getUrl("/franquia/agendamento/ConfirmarAgendamento");
            messageSuccess = "Agendamento confirmado com sucesso!";
        }
        else {
            url = Util.getUrl("/franquia/agendamento/Reagendar");
            messageSuccess = "Reagendamento realizado com sucesso!";
        }         

        //$scope.agendamentoF = angular.copy($scope.agendamentoForm);

        //if (!$scope.agendamento.AGE_DATA_AGENDAMENTOMask) {
        //    $scope.buttonConfirmAgen = 'agendar';
        //    $scope.objEnvio.AGE_DATA_AGENDAMENTO = null;
        //}

        //if (!$scope.agendamento.AGE_DATA_AGENDAMENTOTime) {

        //    $scope.buttonConfirmAgen = 'agendar';
        //    $scope.objEnvio.AGE_DATA_AGENDAMENTO = null;
        //}

        formHandlerService.submit($scope, {

            url: url,
            objectName: 'agendamentoForm',
            showLoader: true,
            success: function (resp, status, config, message, validationMessage) {

                $scope.objEnvio = null;
                $scope.message = message;
                $scope.erros = validationMessage;

                $scope.buttonConfirmAgen = 'reset';
                if (resp.success) {

                    $scope.message = Util.createMessage("success", messageSuccess);
                    $scope.agendamento = {};

                    $scope._carregarDadosDoCliente($scope.clienteModal.CLI_ID);

                    if ($scope.loadAll) {

                        $scope.loadAll();
                    }
                    $timeout(function () {

                        $scope.fechaModal("#modalformAgendamento");
                        $scope.message = null;
                        if ($scope.tab) {

                            $scope.tab = 4;
                            $scope.listarAgendamentos();
                        }

                    }, 1000);

                }

            }
        });
    }

    $scope.informarContato = function () {

        url = Util.getUrl("/franquia/clientes/InformarContato");

        var objEnvio = angular.copy($scope.contato);

        //if (!objEnvio.DataMask) {
        //    $scope.buttonInformarContato = 'reset';
        //    objEnvio.Data = null;
        //}

        //if (!objEnvio.DataMaskTime) {

        //    $scope.buttonInformarContato = 'reset';
        //    objEnvio.Data = null;
        //}
                 
        $scope.objEnvio = objEnvio;

        formHandlerService.submit($scope, {

            url: url,
            objectName: 'objEnvio',
            showLoader: true,
            success: function (resp, status, config, message, validationMessage) {

               
                $scope.message = message;
                $scope.erros = validationMessage;

                $scope.buttonInformarContato = 'reset';
                if (resp.success) {

                    $scope.message = Util.createMessage("success", "Contato informado com sucesso!");
                    $scope.agendamento = {};

                    $scope._carregarDadosDoCliente($scope.clienteModal.CLI_ID);
                    if ($scope.loadAll) {

                        $scope.loadAll();
                    }
                    $timeout(function () {
                        $scope.buttonInformarContato = 'reset';
                        $scope.fechaModal("#modalFormContato");
                        $scope.abreAbaHistoricoCliente();
                        $scope.message = null;                        

                    }, 2000);
                }
            }
        });
    }

    $scope.abreModal = function (selector) {

        angular.element($scope.modalId).find(selector).modal();
    }

    $scope.fechaModal = function (selector) {

        angular.element($scope.modalId).find(selector).modal('hide');
    }
    $scope.abreFormContato = function (item) {

        $scope.erros = null;
        var CLI_ID = $scope.clienteModal.CLI_ID;
        $scope.abreModal("#modalFormContato");
        $scope.contato = {Data : new Date(), CLI_ID : CLI_ID};
    }


    $scope.listarRepresentantes = function (pagina) {

        var url = Util.getUrl("/franquia/representante/representantes");
        
        if (pagina) {

            url += "?pagina=" + pagina;
        }

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'representantes',
            responseModelName: 'representantes',
            showAjaxLoader: true,
            pageConfig: { pageName: 'page' /*, pageTargetName: 'paginaPrioridades' */ },
            success: function (resp) {

            }
        });
    }

    $scope.listarRepresentantesDoCliente = function (pagina) {

        var url = Util.getUrl("/franquia/representante/RepresentantesDoClienteExcetoOLogado");

        if (!$scope.filtroEncaminhar) {

            $scope.filtroEncaminhar = {};
        }
        
        if (pagina) {

            url += "?pagina=" + pagina;
        }

        if($scope.clienteModal){

            $scope.filtroEncaminhar.CLI_ID = $scope.clienteModal.CLI_ID;
        }

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstRepresentanteDoCliente',
            responseModelName: 'lstRepresentanteDoCliente',
            showAjaxLoader: true,
            data: $scope.filtroEncaminhar,
            pageConfig: { pageName: 'page' , pageTargetName: 'paginaRepresentanteDoCliente'},
            success: function (resp) {

            }
        });
    }


    $scope.listarRepresentantesDoClienteInformacao = function (pagina) {

        var url = Util.getUrl("/franquia/representante/RepresentantesDoCliente");

        if (!$scope.filtroInfo) {

            $scope.filtroInfo = {};
        }

        if (pagina) {

            url += "?pagina=" + pagina;
        }

        if ($scope.clienteModal) {

            $scope.filtroInfo.CLI_ID = $scope.clienteModal.CLI_ID;
        }

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstRepresentanteDoClienteInformacao',
            responseModelName: 'lstRepresentanteDoCliente',
            showAjaxLoader: true,
            data: $scope.filtroInfo,
            pageConfig: { pageName: 'page', pageTargetName: 'paginaRepresentanteDoClienteInformacao' },
            success: function (resp) {

            }
        });
    }

    $scope.listarRepresentantesDaRegiao = function (pagina) {

        var url = Util.getUrl("/franquia/representante/representantesDaRegiao");

        if (pagina) {

            url += "?pagina=" + pagina;
        }

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'representantesDaRegiao',
            responseModelName: 'representantes',
            data: $scope.filtroRepresentante,
            showAjaxLoader: true,
            pageConfig: { pageName: 'page' , pageTargetName: 'paginaRepDaRegiao'  },
            success: function (resp) {

            }
        });
    }

    $scope.carregaRegioes = function (REP_ID) {

        if (!$scope.regioes) {
            var url = Util.getUrl('/regiao/regioes');
            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'regioes',
                responseModelName: 'lstRegiao',
                success: function ( ) {

                }
            });
        }
    }


    $scope.carregaRegioesExcluindoRegiaoDoRepresentante = function () {

        if (!$scope.regioes) {
            var url = Util.getUrl('/regiao/regioesExcluindoRegiaoDoRepresentante');
            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'regioes',
                responseModelName: 'lstRegiao',
                success: function () {

                }
            });
        }
    }
    $scope.abreAbaEncaminhamento = function () {

        $scope.tab = 5;
        $scope.lstRepresentanteDoCliente = null;
        $scope.carregaRegioes();
        $scope.listarRepresentantesDaRegiao();
    }

    $scope.abreFormEncaminhar = function (item) {
                   
        $scope.erros = null;
        var CLI_ID = $scope.clienteModal.CLI_ID;
        $scope.abreModal("#modalFormEncaminhar");
        $scope.encaminhamentoDTO = { REP_ID: item.REP_ID, CLI_ID : CLI_ID, CLIENTE : $scope.clienteModal, REPRESENTANTE: item };
    }

    $scope.encaminharCliente = function () {

        url = Util.getUrl("/franquia/clientes/EncaminharCliente");

        formHandlerService.submit($scope, {

            url: url,
            objectName: 'encaminhamentoDTO',
            showLoader: true,
            success: function (resp, status, config, message, validationMessage) {


                $scope.message = message;
                $scope.erros = validationMessage;

                $scope.buttonEncaminhar = 'reset';
                if (resp.success) {

                    $scope.tab = 1;
                    $scope.message = Util.createMessage("success", "Encaminhado com sucesso!");
                    $scope.encaminhamentoDTO = {};

                    $scope._carregarDadosDoCliente($scope.clienteModal.CLI_ID);
                    if ($scope.loadAll) {

                        $scope.loadAll();
                    }
                    $timeout(function () {
                        $scope.buttonEncaminhar = 'reset';
                        $scope.fechaModal("#modalFormEncaminhar");

                    }, 2000);

                }

            }
        });
    }

    $scope.editarCliente = function (CLI_ID) {

        if (CLI_ID) {
            var url = Util.getUrl("/franquia/clientes/Editar?clienteId=" + CLI_ID);

            post(url, true);
        }

    }

    $scope.abreFormVendaEfetuada = function () {

        $scope.erros = null;
        var CLI_ID = $scope.clienteModal.CLI_ID;
        $scope.abreModal("#modalInformarVenda");
        $scope.descricaoProdutoComposto = null;
        $scope.vendaCRMDTO = {CLI_ID: CLI_ID};
    }

    $scope.informarVendaEfetuada = function () {
        
        formHandlerService.submit($scope, {
            url: Util.getUrl("/franquia/pedido/InformarVendaEfetuada"),
            objectName: 'vendaCRMDTO',
            showAjaxLoader: true,
            success: function (resp, status, config, message, validationMessage) {

                $scope.message = message;
                $scope.erros = validationMessage;

                $scope.buttonInformarVenda = 'reset';
                if (resp.success) {

                    $scope.vendaCRMDTO = null;
                    if ($scope.loadAll) {

                        $scope.loadAll();
                    }
                    $scope._carregarDadosDoCliente($scope.clienteModal.CLI_ID);

                    $scope.message = Util.createMessage("success", "Salvo com sucesso!");
                    
                    $timeout(function () {

                        $scope.message = null;
                        if ($scope.tab) {

                            $scope.buttonInformarVenda = 'reset';
                            $scope.fechaModal("#modalInformarVenda");
                            $scope.abreAbaListaDePedidos();
                        }

                    }, 1000);

                }

            }
        });
    }

    $scope.listarPedidosEmitidos = function (pagina) {

        $scope.lstPedidos = [];
        var data = {
            CLI_ID: $scope.clienteModal.CLI_ID,
            vendaInformada : true
        };
        
        var url = Util.getUrl("/pedido/ListarPedidosVendaInformada");

        if (pagina) {

            url += "?pagina=" + pagina;
        }

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstPedidos',
            responseModelName: 'lstPedidos',
            showAjaxLoader: true,
            pageConfig: { pageName: 'page' /*, pageTargetName: 'pageAgendamentoDoDia' */ },
            data: data,
            success: function (resp) {

            }
        });
    }

    $scope.abreAbaListaDePedidos = function () {

        $scope.tab = 6;
        $scope.listarPedidosEmitidos();
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
            showAjaxLoader: true,
            success: function (resp) {
            }
        });
    }

    $scope.emitirPedido = function (CLI_ID) {
        
        if (CLI_ID) {

            var url = Util.getUrl("/pedido/emitir?CLI_ID=" + CLI_ID);
            post(url, true);
        }        
    }

    $scope.emitirPedidoMesmaJanela = function (CLI_ID) {

        if (CLI_ID) {

            var url = Util.getUrl("/pedido/emitir?CLI_ID=" + CLI_ID);
            post(url);
        }
    }

    $scope.carregarTiposDeCliente = function () {



        var url = Util.getUrl("/franquia/clientes/ListarTiposCliente");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: "lstTipoCliente",
            responseModelName: "lstTipoCliente",
            showAjaxLoader: true,
            success: function (resp) {
            }
        });
    }



    if (window.ImportacaoClienteController !== undefined) {

        ImportacaoClienteController($scope, formHandlerService, $http, conversionService, $timeout);
    }
}
