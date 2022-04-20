appModule.controller("PropostaController", ['$scope', '$http', 'formHandlerService', 'MathService', '$timeout', 'UploadAjax', 'Upload', 'FilterService', 'UenService',
function ($scope, $http, formHandlerService, MathService, $timeout, UploadAjax, Upload, FilterService, UenService, $window) {
    
    $scope.initList = function (ehGerente, ppiId, prtId, isFranquiado) {
        $scope.dispararPesquisa = null;
        $scope.listado = false;
        $scope.ehGerente = (ehGerente === true || ehGerente === "True");
        $scope.ehFranquiado = (isFranquiado === true || isFranquiado === "True");
        $scope.carregaCombos();
        $scope.carregarTipoNegociacao();
        //$scope.carregarTipoProposta();
        $scope.carregarPedidoStatus();
        $scope.criarFiltros();
        $scope.obterGruposDeFiltroDoPedido();
        $scope.listarUens();
        $scope.filtro = {
            pesquisaCpfCnpjPorIqualdade: true,
            PRT_ID: (prtId) ? prtId : null,
            PPI_ID: (ppiId) ? ppiId : null
        };

        if (ppiId || prtId)
            $scope.dispararPesquisa = true;


        $scope.retornarUenAtual(function () {

            $scope.$watch('uen', function () {

                if($scope.listado === true)
                    $scope.listarPropostas();

                $scope.carregaCombos();
            });
        });
    }

    $scope.listarLocalizacoesDeCurso = function () {

        var url = Util.getUrl("/curso/listarLocalizacoesDeCurso");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstLocalizacoes',
            responseModelName: 'lstLocalizacoes',
            showAjaxLoader: true,
        });
    };

    $scope._executarInit = function (prtId, empId, ehGerente, repId) {

        $scope.cpfObrigatorio = false;
        $scope.queryPropostaVal = { EhValido: false };
        var ehGerenteEmitindo = (ehGerente === "1");
        $scope.ehGerenteEmitindo = ehGerenteEmitindo;
        $scope.listarLocalizacoesDeCurso();

        if (prtId) {
            $scope.read(prtId, function () {

                if ($scope.proposta) {
                    $scope.proposta.EhGerenteEmitindo = ehGerenteEmitindo;
                    $scope.objPedido = $scope.proposta;
                }
            });
        }
        else {
            $scope.proposta = {
                PRT_QTD_PARCELAS: 1,
                PROPOSTA_ITEM: [],
                REP_ID_EMITENTE: repId,
                EhGerenteEmitindo: ehGerenteEmitindo
            };

            if (empId) {
                $scope.proposta.EMP_ID = Number(empId);
            }

            if (!ehGerenteEmitindo) {

                $scope.proposta.REP_ID = repId;
                $scope.recuperarDadosDoRepresentante(repId);
                $scope.listarCarteirasDoRepresentante();
            }

            $scope.objPedido = $scope.proposta;

        }

        if ($scope.uen && $scope.uen.UEN_ID) {

            if ($scope.proposta) {

                if (!$scope.proposta.PRT_ID && !$scope.proposta.UEN_ID) {

                    $scope.proposta.UEN_ID = $scope.uen.UEN_ID;
                }

                if ($scope.proposta.PRT_ID &&
                    $scope.proposta.UEN_ID &&
                    $scope.uen &&
                    $scope.proposta.UEN_ID !== $scope.uen.UEN_ID) {

                    if (confirm("Essa proposta pertence a outra UEN. Se você confirmar você será direcionada a listagem de proposta.")) {

                        window.location = Util.getUrl("/proposta/index");
                    }
                    else {
                        UenService.trocarUen($scope, $scope.proposta.UEN_ID);
                    }
                }


                if ($scope.uen.UEN_ID === 1) {

                    $scope.proposta.TPP_ID = 1;
                }
            }
        }

        $scope.$watch("proposta.PROPOSTA_ITEM", function (value, old) {

            if ($scope.proposta)
                $scope.proposta.Itens = value;
        });

        $scope.carregarTipoNegociacao();
        $scope.carregarTipoProposta();
        $scope.carregaCombos();
        $scope.carregarEmpresas();
        $scope.retornarPeriodoFaturamento();

    };

    $scope.init = function (prtId, empId, ehGerente, repId) {

        $scope.dadosInit = {

            prtId: prtId,
            empId: empId,
            ehGerente: ehGerente,
            repId : repId
        };

        $scope.retornarUenAtual(function () {
            $scope.$watch('uen', function () {

                $scope.reinicializar();
            });
        });
        $scope._executarInit(prtId, empId, ehGerente, repId);
    };

    $scope.reinicializar = function () {

        $scope._executarInit($scope.dadosInit.prtId, $scope.dadosInit.empId, $scope.dadosInit.ehGerente, $scope.dadosInit.repId);
    };



    $scope.obterDadosDoProspect = function () {

        if (Util.isPathValid($scope, "proposta.CLI_ID")) {

            var cliId = $scope.proposta.CLI_ID;
            $scope._carregarDadosDoCliente(cliId);
            $scope.listarAssinaturas();

            if ($scope.ehGerenteEmitindo) {
                $scope.proposta.REPRESENTANTE = null;
                $scope.proposta.REP_ID = null;
                $scope.proposta.CAR_ID = null;
            }
        }   
    }

    $scope.carregarTipoProposta = function () {

        var url = Util.getUrl("/proposta/listarTipoProposta");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstTipoProposta',
            responseModelName: 'lstTipoProposta',
            showAjaxLoader: true,
            success: function () {
                FilterService.adicionarDadoCombo($scope.filtros, $scope.lstTipoProposta, "TPP_ID");
            }
            
        });
    }

    $scope.listarUens = function () {

        var url = Util.getUrl("/UEN/listarUens");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstUEN',
            responseModelName: 'lstUEN',
            showAjaxLoader: true,
            success: function () {
                FilterService.adicionarDadoCombo($scope.filtros, $scope.lstUEN, "UEN_ID");
            }

        });
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


    $scope.retornarPeriodoFaturamento = function () {

        var url = Util.getUrl("/proposta/retornarPeriodoFaturamento");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstDatasFaturamento',
            responseModelName: 'lstDatasFaturamento',
            showAjaxLoader: true,

        });
    }

    $scope.getLstProdutosPorNome = function (nome) {
        
        if ($scope.select2CtrlProduto != null)
            $scope.select2CtrlProduto.loader = true;
        var filtro = { nome: nome };

        formHandlerService.read($scope, {
            url: Util.getUrl("/produto/listarPorNomeAutocomplete"),
            targetObjectName: 'lstProdutos',
            responseModelName: 'lstProdutos',
            data: filtro,
            success: function () {
                if ($scope.select2CtrlProduto != null)
                    $scope.select2CtrlProduto.loader = false;
            }

        });
    }

    $scope.listarRepresentanteAutocomplete = function (nome) {

        if ($scope.select2CtrlRepresentante != null)
            $scope.select2CtrlRepresentante.loader = true;
        var filtro = { nome: nome };

        formHandlerService.read($scope, {
            url: Util.getUrl("/representante/listarRepresentanteAutocomplete"),
            targetObjectName: 'lstRepresentante',
            responseModelName: 'lstRepresentante',
            data: filtro,
            success: function () {
                if ($scope.select2CtrlRepresentante != null)
                    $scope.select2CtrlRepresentante.loader = false;
            }

        });
    };

    $scope.carregarEmpresas = function () {

        var url = Util.getUrl("/proposta/listarEmpresas");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstEmpresas',
            responseModelName: 'lstEmpresas',
            showAjaxLoader: true,

        });
    };

    $scope.read = function (prtId, onSuccess) {

        if (prtId != null) {
            var url = Util.getUrl("/proposta/RecuperarDadosDaProposta");

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'proposta',
                responseModelName: 'proposta',
                showAjaxLoader: true,
                data: { prtId: prtId },
                success: function () {

                    if (Util.isPathValid($scope, "proposta.CLI_ID")) {

                        $scope._carregarDadosDoCliente($scope.proposta.CLI_ID, false);
                        $scope.listarAssinaturas($scope.proposta.CLI_ID);
                        /// comentário
                    }

                    if(Util.isPathValid($scope, 'proposta.PRT_DATA_FATURAMENTO_AGENDADA'))
                    {
                        
                        var date = $scope.proposta.PRT_DATA_FATURAMENTO_AGENDADA;
                        var dateStr = date.getFullYear() + '/' + 
                            (date.getMonth()) +  '/' + 
                            date.getDate()

                        $scope.proposta.DATA_FATURAMENTO_STR = dateStr;
                    }

                    if (onSuccess && typeof (onSuccess) == 'function') {

                        onSuccess();
                    }
                }
            });
        }
    }

    $scope.initDetalhes = function (PED_CRM_ID) {

        if (PED_CRM_ID) {

            $scope.carregarDadosDoPedido(PED_CRM_ID);
        }
    }

    $scope.listarPropostas = function (pagina) {
        
        $scope.paginaAtual = pagina;
        $scope.listado = true;
        var url = Util.getUrl("/proposta/listarPropostas");

        if (pagina) {

            url += "?pagina=" + pagina;
        }

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstPropostas',
            responseModelName: 'lstPropostas',
            showAjaxLoader: true,
            data: $scope.filtro,
            pageConfig: { pageName: 'page' /*, pageTargetName: 'pageAgendamentoDoDia' */ },
            success: function (resp) {

            }
        });
    };

    $scope._carregarDadosDoCliente = function (CLI_ID, calcular) {

        $scope.proposta.origem = 'cli';
        $scope.proposta.INFO_CLIENTE = null;
        
        if (CLI_ID) {
            var url = Util.getUrl("/proposta/checarERetornarDadosDeCliente");

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'proposta.INFO_CLIENTE',
                responseModelName: 'cliente',
                data: { clienteId: CLI_ID },
                showAjaxLoader: true,
                success: function (response) {

                    if (!Util.isPathValid($scope, "proposta.INFO_CLIENTE")) {
                        $scope.proposta.origem = null;
                        $scope.proposta.ASN_NUM_ASSINATURA = null;
                    }

                    if (!$scope.ehGerenteEmitindo) {
                        $scope.listarCarteirasDoRepresentanteECliente(CLI_ID, $scope.proposta.REP_ID);
                    }

                    if(calcular !== false)
                        $scope.calcularImpostoDoPedido();
                },
                fail: function () {
                    $scope.proposta.origem = null;
                }
            });
        }
        else {
            $scope.proposta.origem = null;
        }
    }

    $scope.abreModalEnviarEmailParaCliente = function (ppiId, cliId, boleto) {

        if (!boleto && boleto != false)
            boleto = true;

        $scope.modalEnvioEmail = {
            PPI_ID: ppiId,
            CLI_ID: cliId,
            boleto: boleto,
            LstEmail : []
        };   

        $scope.listarEmailsDoCliente(cliId);
        $scope.listarAssinaturas(cliId);
        angular.element("#modal-enviar-email").modal();
    }


    $scope.listarAssinaturas = function (cliId) {

        $scope.lstAssinatura = [];        
        if (cliId || Util.isPathValid($scope, "proposta.CLI_ID")) {
            
            var paramCliId = (cliId) ? cliId : $scope.proposta.CLI_ID;
            
            var url = Util.getUrl("/CLIENTES/listarAssinaturas");

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'lstAssinatura',
                responseModelName: 'lstAssinatura',
                showAjaxLoader: true,
                data: {
                    cliId: paramCliId
                },
                success: function (resp) {

                }
            });
        }
    }


    $scope.listarEmailsDoCliente = function (cliId) {

        $scope.lstEmailsDoCliente = [];

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


    $scope.enviarEmailPagamento = function (email, cadastrarEmail, clidId) {

        var pagamentoGateway = $scope.modalEnvioEmail.pagamentoGateway;

        if (pagamentoGateway == true) {

            $scope.enviarEmailPagamentoGateway(email);
        }
        else {
            if ($scope.modalEnvioEmail.boleto == true) {

                $scope.enviarBoletoPorEmail(email, cadastrarEmail);
            }
            else {
                $scope.EnviarResumoDaProposta(email, cadastrarEmail);
            }
        }
    }

    $scope.habilitarEmailCustomizado = function () {
        
        if (!$scope.modalEnvioEmail.emailCustomizadoHabilitado)
            $scope.modalEnvioEmail.emailCustomizadoHabilitado = true;
        else {
            $scope.modalEnvioEmail.emailCustomizadoHabilitado = !$scope.modalEnvioEmail.emailCustomizadoHabilitado;
        }

        $scope.emailCustom = ($scope.modalEnvioEmail.EmailCustom) ? $scope.modalEnvioEmail.EmailCustom : null;
        
    }

    $scope.adicionarEmailCustom = function () {

        if ($scope.modalEnvioEmail.emailValido === false) {

            $scope.message = Util.createMessage('fail', 'O E-Mail não é válido.');
            return;
        }
        $scope.modalEnvioEmail.EmailCustom = $scope.emailCustom;

        $scope.checarEmailValido($scope.modalEnvioEmail.EmailCustom);
    }

    $scope.removerEmailCustomizado = function () {

        $scope.modalEnvioEmail.EmailCustom = null;
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


    $scope.gerarStatusDTO = function (propostaItem) {

        var cliId = null;
        var ppiId = propostaItem.PPI_ID;

        if (Util.isPathValid(propostaItem, "PROPOSTA.CLI_ID")) {

            cliId = propostaItem.PROPOSTA.CLI_ID;
        }
        return { CLI_ID: cliId, PPI_ID: ppiId };
    }

   $scope.abrirModalMarcarComoPago = function (propostaItem) {

        $scope.alteracaoModal = $scope.gerarStatusDTO(propostaItem);
        $scope.alteracaoModal.header = "Informar pagamento manualmente.";
        $scope.alteracaoModal.label = "Informações sobre o pagamento.";
        $scope.alteracaoModal.buttonConfig = [{ label: 'Confirmar', state: 'cnf' }, { label: 'Processando...', state: 'process', disabled: true }];
        $scope.alteracaoModal.buttonVar = "buttonAlt";
        $scope.alteracaoModal.acaoDoBotao = function () {

            return $scope.InformarPedidoPagoComPendenciaDeConferencia();
        };

        angular.element("#modal-alterar-status").modal();
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

   $scope.abrirModalCancelarProposta = function (propostaItem) {

       $scope.alteracaoModal = $scope.gerarStatusDTO(propostaItem);
       $scope.alteracaoModal.header = "Cancelamento.";
       $scope.alteracaoModal.label = "Motivos do Cancelamento.";
       $scope.alteracaoModal.buttonConfig = [{ label: 'Cancelar', state: 'cnf' }, { label: 'Cancelando...', state: 'process', disabled: true }];
       $scope.alteracaoModal.buttonVar = "buttonAlt";
       $scope.alteracaoModal.buttonClass = 'btn-danger';
       $scope.alteracaoModal.acaoDoBotao = function () {

           return $scope.cancelarProposta();
       };

       angular.element("#modal-alterar-status").modal();
   }

   $scope.InformarPedidoPagoComPendenciaDeConferencia = function () {

       if (confirm("Deseja realmente alterar o status da proposta?")) {

           formHandlerService.submit($scope, {
               url: Util.getUrl("/proposta/informarPedidoPagoComPendenciaDeConferencia"),
               objectName: 'alteracaoModal',
               showAjaxLoader: true,
               success: function (resp, status, config, message, validationMessage) {

                   $scope.message = message;
                   $scope.erros = validationMessage;

                   $scope.buttonAlt = 'reset';
                   if (resp.success) {

                       $scope.message = Util.createMessage("success", "Status alterado com sucesso!");

                       $timeout(function () {
                           angular.element("#modal-alterar-status").modal('hide');
                           angular.element("#modal-opcoes-pagamento").modal('hide');
                           $scope.message = null;
                           $scope.recarregarListarPropostas();
                           $scope.listarPropostaItemPorProposta();

                       }, 1000);
                   }

               }
           });
       }
       else {
           return false;
       }
   }

   $scope.cancelarProposta = function () {

       if (confirm("Deseja realmente cancelar a proposta ?")) {

           formHandlerService.submit($scope, {
               url: Util.getUrl("/proposta/cancelarPropostaItem"),
               objectName: 'alteracaoModal',
               showAjaxLoader: true,
               success: function (resp, status, config, message, validationMessage) {

                   $scope.message = message;
                   $scope.erros = validationMessage;

                   $scope.buttonAlt = 'reset';
                   if (resp.success) {

                       $scope.message = Util.createMessage("success", "Proposta cancelada com sucesso!");

                       $timeout(function () {
                           angular.element("#modal-alterar-status").modal('hide');
                           angular.element("#modal-opcoes-pagamento").modal('hide');
                           $scope.message = null;
                           $scope.recarregarListarPropostas();
                           $scope.listarPropostaItemPorProposta();

                       }, 1000);
                   }

               }
           });
       }
       else {
           return false;
       }
   }
   $scope.recusarPagamento = function () {

       if (confirm("Deseja realmente alterar o status do pedido?")) {

           formHandlerService.submit($scope, {
               url: Util.getUrl("/proposta/RecusarPagamentoDoPedido"),
               objectName: 'alteracaoModal',
               showAjaxLoader: true,
               success: function (resp, status, config, message, validationMessage) {

                   $scope.message = message;
                   $scope.erros = validationMessage;

                   $scope.buttonAlt = 'reset';
                   if (resp.success) {

                       $scope.message = Util.createMessage("success", "Status alterado com sucesso!");

                       $timeout(function () {
                           angular.element("#modal-alterar-status").modal('hide');
                           angular.element("#modal-opcoes-pagamento").modal('hide');
                           $scope.message = null;
                           $scope.recarregarListarPropostas();
                           $scope.listarPropostaItemPorProposta();

                       }, 1000);
                   }

               }
           });
       }
       else {
           return false;
       }
   }


   $scope.confirmarPagamento = function (propostaItem) {

       $scope.alteracaoModal = $scope.gerarStatusDTO(propostaItem);
       if (confirm("Deseja realmente alterar o status da proposta?")) {

           formHandlerService.submit($scope, {
               url: Util.getUrl("/proposta/MarcarPropostaComoPaga"),
               objectName: 'alteracaoModal',
               showAjaxLoader: true,
               success: function (resp, status, config, message, validationMessage) {

                   $scope.message = message;
                   $scope.erros = validationMessage;

                   $scope.buttonAlt = 'reset';
                   if (resp.success) {

                       $scope.message = Util.createMessage("success", "Status alterado com sucesso!");

                       $timeout(function () {
                           $scope.message = null;
                           $scope.recarregarListarPropostas();
                           $scope.listarPropostaItemPorProposta();
                           
                           angular.element("#modal-opcoes-pagamento").modal('hide');

                       }, 1000);
                   }

               }
           });
       }
       else {
           return false;
       }
   }

   $scope.recarregarListarPropostas = function () {

       if ($scope.paginaAtual) {

           var paginaAtual = $scope.paginaAtual;
           $scope.listarPropostas(paginaAtual);
       }
       else {
           $scope.listarPropostas();
       }
   }


    $scope.enviarLinkBoletoPorEmail = function (email) {

        if (confirm("Deseja enviar o email para o cliente?")) {

            var PPI_ID = $scope.modalEnvioEmail.PPI_ID;

            $scope.envioEmailDTO = {
                PPI_ID: PPI_ID,
                email: email
            };

            formHandlerService.submit($scope, {
                url: Util.getUrl("/proposta/EnviarLinkBoletoPorEmail"),
                objectName: 'modalEnvioEmail',
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

    $scope.adicionarEmail = function (email, cadastrar) {

        if (email) {
            
            var achou = false;

            angular.forEach($scope.modalEnvioEmail.LstEmail, function (value) {

                if (achou == false) {
                    if (value.Email == email) {
                        achou = true;
                    }
                }
            });
            if(achou == false)
                $scope.modalEnvioEmail.LstEmail.push({ Email: email, CadastrarEmail : cadastrar });
        }
        else {
            $scope.message = Util.createMessage('fail', 'Informe o E-Mail')
        }
    }

    $scope.removerEmail = function ($index) {

        $scope.modalEnvioEmail.LstEmail.splice($index, 1);
    }
    $scope.enviarBoletoAutomatico = function (email, ppiId, cliId) {

        if (email) {

            $scope.modalEnvioEmail = {
                PPI_ID: ppiId,
                CLI_ID: cliId,
                boleto: true,
                LstEmail: [
                    {
                        Email: email,
                        CadastrarEmail: true,
                    }
                ]
            };
            $scope.enviarBoletoPorEmail();

        }
    }

    $scope.enviarBoletoPorEmail = function () {

        if (confirm("Deseja enviar o email para o cliente?")) {

            $scope.objEnvio = angular.copy($scope.modalEnvioEmail);
            
            formHandlerService.submit($scope, {
                url: Util.getUrl("/proposta/EnviarBoletoPorEmail"),
                objectName: 'objEnvio',
                showAjaxLoader: true,
                success: function (resp, status, config, message, validationMessage) {

                    $scope.buttonEnviarEmail = 'reset';
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
        else {
            return false;
        }
    }


    $scope.EnviarResumoDaProposta = function (email, cadastrarEmail) {

        if (confirm("Deseja enviar o email para o cliente?")) {


            formHandlerService.submit($scope, {
                url: Util.getUrl("/proposta/EnviarResumoDaProposta"),
                objectName: 'modalEnvioEmail',
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

    $scope.recuperarClientePorAssinatura = function () {

        if (Util.isPathValid($scope, "proposta.ASN_NUM_ASSINATURA") &&
            $scope.codAssinaturaAnterior != $scope.proposta.ASN_NUM_ASSINATURA) {

            $scope.codAssinaturaAnterior = $scope.proposta.ASN_NUM_ASSINATURA;
            $scope.carregarClientePorAssinatura($scope.proposta.ASN_NUM_ASSINATURA);
        }
    }

    $scope.carregarClientePorAssinatura = function (codAssinatura) {

        if (codAssinatura) {
            $scope.proposta.origem = 'ass';

            var url = Util.getUrl("/franquia/CLIENTES/RecuperarDadosDoClientePorAssinatura");

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'proposta.INFO_CLIENTE',
                responseModelName: 'cliente',
                data: { codAssinatura: codAssinatura },
                showAjaxLoader: true,
                success: function (response) {

                    if (Util.isPathValid($scope, "proposta.INFO_CLIENTE.CLI_ID")) {

                        $scope.proposta.CLI_ID = $scope.proposta.INFO_CLIENTE.CLI_ID;
                    }
                    if (Util.isPathValid($scope, "proposta.ASN_NUM_ASSINATURA") &&
                        Util.isPathValid($scope, "proposta.TPP_ID") &&
                        $scope.proposta.TPP_ID == 2) {

                        $scope.adicionarProdutoRenovacao(codAssinatura);
                    }
                }
            });
        }
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

            FilterService.adicionarDadoCombo($scope.filtros, $scope.lstRegioes, "RG_ID");
            FilterService.adicionarDadoCombo($scope.filtros, $scope.lstTipoPagamento, "TPG_ID");
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

            if ($scope.uen)
                config.data.uenId = $scope.uen.UEN_ID;

            if ($scope.proposta.EMP_ID)
                config.data.empId = $scope.proposta.EMP_ID;
        }
        formHandlerService.read($scope, config);
    };

    $scope.adicionarLinha = function () {

        $scope.passo = 2;

        if (Util.isPathValid($scope, "proposta.PROPOSTA_ITEM")) {
            $scope.proposta.PROPOSTA_ITEM.push({

                PPI_GERA_NOTA: true
            });
        }

        $scope.calcularTotais();

    }

    $scope.adicionarProduto = function (item) {

        //$scope.limparPedidoPagamento();
        //$scope.limparParticipantes();
        $scope.passo = 2;

        if (item != null && Util.isPathValid($scope, "proposta.PROPOSTA_ITEM")) {

            if ($scope.proposta.TPP_ID == 3 && !Util.isPathValid($scope, "modalProposta.ASN_NUM_ASS_CANC")) {

                $scope.message = Util.createMessage('fail', 'Selecione a assinatura a ser transferida.');
                return;
            }

            var data = new Date();
            data.setDate(data.getDate() + 2);
                        
            var propostaItem = {

                PRODUTO_COMPOSICAO: angular.copy(item),
                CMP_ID: item.CMP_ID,
                PPI_QTD: item.QTD,
                PPI_QTD_PARCELAS: 1,
                PRT_IDENTIFICACAO_TURMA: item.PRT_IDENTIFICACAO_TURMA,
                PPI_VALOR_UNITARIO: item.CMP_VLR_VENDA,
                PPI_TOTAL: item.QTD * item.CMP_VLR_VENDA,
                PPI_DATA_VENCIMENTO: data,
                PPI_GERA_NOTA: true,
                PPI_VALOR_UNITARIO_PRODUTO: item.CMP_VLR_VENDA,
                Participantes: ($scope.proposta.UEN_ID === 1 ) ? [{ PED_EH_O_COMPRADOR: false }] : null
            };

            if ($scope.proposta.TPP_ID === 3) {

                propostaItem.PPI_ASN_NUM_ASS_CANC = $scope.modalProposta.ASN_NUM_ASS_CANC;
            }

            $scope.proposta.PROPOSTA_ITEM.push(propostaItem);
        }

        $scope.calcularTotais();

        angular.element("#modal-curso").modal('hide');
        angular.element("#modal-produto-composicao").modal('hide');
    }

    $scope.alterarProduto = function (item, infoProdutoComposto) {

        $scope.passo = 2;

        if (item !== null &&
            infoProdutoComposto &&
            infoProdutoComposto.ProdutoComposicao) {

            var produtoComposicao = infoProdutoComposto.ProdutoComposicao;

            var data = new Date();
            data.setDate(data.getDate() + 2);

            var valorVenda = (infoProdutoComposto.ValorDaVenda) ? infoProdutoComposto.ValorDaVenda : produtoComposicao.CMP_VLR_VENDA;

            item.PRODUTO_COMPOSICAO = angular.copy(produtoComposicao);
            item.CMP_ID = produtoComposicao.CMP_ID;
            item.CmpId = produtoComposicao.CMP_ID;
            item.PPI_QTD = produtoComposicao.QTD;
            item.PRT_IDENTIFICACAO_TURMA = produtoComposicao.PRT_IDENTIFICACAO_TURMA;
            item.PPI_VALOR_UNITARIO = valorVenda;
            item.PPI_TOTAL = produtoComposicao.QTD * valorVenda;
            item.PPI_DATA_VENCIMENTO = data;
            item.PPI_QTD_CONSULTA = infoProdutoComposto.QtdConsultas;
            item.PPI_GERA_NOTA = true;
            item.PPI_VALOR_UNITARIO_PRODUTO = valorVenda;
            if(!item.PPI_QTD_PARCELAS){
                item.PPI_QTD_PARCELAS = 1;
            }
            2
        }

        $scope.calcularTotais();
        
    }

    $scope.calcularPrecoItem = function (item) {

        if (item) {

            var qtd = item.PPI_QTD;
            var valorUnitario = item.PPI_VALOR_UNITARIO;
            var totalDoItem = qtd * valorUnitario;

            item.PPI_TOTAL = totalDoItem;
        }

        $scope.calcularTotais();
    }

    //$scope.calcularTotais = function () {

    //    if ($scope.proposta.UEN_ID && $scope.proposta.UEN_ID == 1) {
    //        $scope.calcularImpostoDoPedido();
    //    }
    //    else {
    //        $scope._calcularTotais();
    //    }
    //}

    $scope.calcularTotais = function () {

        if (Util.isPathValid($scope, "proposta.PROPOSTA_ITEM")) {
            var total = 0;

            angular.forEach($scope.proposta.PROPOSTA_ITEM, function(value, index){

                total += value.PPI_TOTAL;
                $scope.calcularParcela(value);
            });

            $scope.proposta.PRT_VALOR_UNITARIO = total;
            $scope.proposta.PRT_VALOR_TOTAL = total;

        }
    }

    $scope.recalcularTotais = function () {

        if (Util.isPathValid($scope, "proposta.PROPOSTA_ITEM")) {
            var total = 0;

            angular.forEach($scope.proposta.PROPOSTA_ITEM, function (value, index) {

                total += Number(value.PPI_TOTAL);
            });

            $scope.proposta.PRT_VALOR_UNITARIO = total;
            $scope.proposta.PRT_VALOR_TOTAL = total;

        }
    }

    $scope.deletarPropostaItem = function ($index) {

        if ($index != null && Util.isPathValid($scope, "proposta.PROPOSTA_ITEM")) {
            
            $scope.proposta.PROPOSTA_ITEM.splice($index, 1);
        }

        if ($scope.proposta.PROPOSTA_ITEM.length <= 0) {

            $scope.passo = 1;
        }

        $scope.calcularTotais();
    }

    $scope.recalcularSomaParcelas = function (item) {

        if (item) {

            var entrada = item.PPI_VALOR_ENTRADA;
            var valorParcela = item.PPI_VALOR_PARCELA;
            var qtdParcelas = item.PPI_QTD_PARCELAS;

            if (item.TIPO_PAGAMENTO &&
                item.TIPO_PAGAMENTO.TPG_ID !== 9) {

                item.ProvaRealTotal = (qtdParcelas * valorParcela) + entrada;
                item.PPI_TOTAL = item.ProvaRealTotal.toFixed(2);
            }
            else {
                item.ProvaRealTotal = valorParcela;
                item.PPI_TOTAL = valorParcela;
            }

            $scope.recalcularTotais();
        }
    }

    $scope.calcularParcela = function (item) {

        if (item) {
            
            var entrada = item.PPI_VALOR_BRUTO_ENTRADA;
            var total = item.PPI_VALOR_UNITARIO * item.PPI_QTD;
            var qtdParcelas = item.PPI_QTD_PARCELAS;

            item.PPI_VALOR_ENTRADA = entrada;
            var restante = total;

            if (Util.isPathValid(item, "TIPO_PAGAMENTO") && item.TIPO_PAGAMENTO.TPG_TIPO == 1) {
                if (entrada > 0) {

                    restante = Number(Number(total - entrada).toFixed(6));
                }
            }
            else {
                item.PPI_VALOR_ENTRADA = null;
            }

            if (item.TIPO_PAGAMENTO &&
                item.TIPO_PAGAMENTO.TPG_ID !== 9) {
                var valorParcela = restante / qtdParcelas;
                valorParcela = Number(Number(valorParcela).toFixed(2));

                //valorParcela = MathService.truncarDecimal(valorParcela, 2);

                item.PPI_VALOR_PARCELA = valorParcela;
                item.PPI_VALOR_BRUTO_PARCELA = valorParcela;
                item.ProvaRealTotal = (qtdParcelas * valorParcela) + entrada;
            }
            else {

                var valorParcela = Number(Number(restante).toFixed(2));

                valorParcela = MathService.truncarDecimal(valorParcela, 2);

                item.PPI_VALOR_PARCELA = valorParcela;
                item.PPI_VALOR_BRUTO_PARCELA = valorParcela;
                item.ProvaRealTotal = valorParcela;
            }

            //if ($scope.proposta.UEN_ID == 1) {

                $scope.calcularDescontoDoItem(item);
            //}
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

    $scope.abrirModalPropostaItem = function (prtId) {

        //$scope.filtro = {};
        $scope.lstPropostaItemModal = null;

        $scope.listarPropostaItemPorProposta(prtId);
        angular.element("#modal-detalhes-proposta").modal();
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

    $scope.gerarFormParticipantes = function () {

        if ($scope.validarVenda() && Util.isPathValid($scope, "pedido.ITEM_PEDIDO")) {

            $scope.limparParticipantes();
            $scope.limparPedidoPagamento();
            $scope.passo = 3;
            angular.forEach($scope.pedido.ITEM_PEDIDO, function (value, index) {
                
                if (value.PRODUTO_COMPOSICAO && value.PRODUTO_COMPOSICAO.EhCurso === true) {
                    if (Util.isPathValid(value, "IPE_QTD")) {

                        var index = value.IPE_QTD;
                        var lstParticipantes = [];

                        for (var i = 0; i < index; i++) {

                            lstParticipantes.push({PED_EH_O_COMPRADOR : false});
                        }

                        value.PEDIDO_PARTICIPANTE = lstParticipantes;
                    }
                }
            });
        }
    }

    $scope.calcularDesconto = function (itemPedido) {

        //$scope.limparPedidoPagamento(2);
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
    
    $scope.emitirProposta = function (sair, forcar) {

        if (forcar === true) {

            if (!confirm("Salvar assim mesmo?"))
                return;
        }
        $scope.button1 = 'save';
        $scope.button2 = 'save';
        
        if (Util.isPathValid($scope, 'proposta.PROPOSTA_ITEM') ) {

            $scope.proposta.Forcar = forcar;
            var valorTotalErrado = false;
            angular.forEach($scope.proposta.PROPOSTA_ITEM, function (value, old) {

                if (valorTotalErrado !== true && value.TIPO_PAGAMENTO && value.TIPO_PAGAMENTO.TPG_ID !== 9) {

                    var valorParcela = (isNaN(value.PPI_VALOR_PARCELA)) ? 0 : value.PPI_VALOR_PARCELA;
                    var qtdParcela = (isNaN(value.PPI_QTD_PARCELAS)) ? 0 : value.PPI_QTD_PARCELAS;
                    var valorEntrada = value.PPI_VALOR_ENTRADA;
                    var valorTotal = Number(value.PPI_TOTAL);

                    var totalReal = (qtdParcela * valorParcela) + valorEntrada;
                    totalReal = totalReal.toFixed(2);
                    valorTotal = valorTotal.toFixed(2);
                    if (valorTotal !== totalReal) {
                        $scope.message = Util.createMessage('fail', 'Não é possível emitir a proposta. O total da proposta não corresponde ao \'Valor Total Parcelas\'.');
                        valorTotalErrado = true;
                        return false;
                    }
                }

            });

            if (valorTotalErrado) {
                $scope.button1 = 'reset';
                $scope.button2 = 'reset';
                return false;
            }
        }
        else {
            $scope.message = Util.createMessage('fail', 'A proposta está com dados incompletos. Recarregue a tela e refaça a proposta.');
            return false;
        }
        formHandlerService.submit($scope, {
            url: Util.getUrl("/proposta/SalvarProposta"),
            objectName: 'proposta',
            showAjaxLoader: true,
            success: function (resp, status, config, message, validationMessage) {

                $scope.message = message;
                $scope.erros = validationMessage;

                $scope.button1 = 'reset';
                $scope.button2 = 'reset';
                $scope.button3 = 'reset';
                if (resp.success) {

                    $scope.propostaSalvaResult = resp.result.propostaSalvaResult;

                    if ($scope.propostaSalvaResult &&
                        $scope.propostaSalvaResult.EhValido === true || $scope.proposta.Forcar == true) {

                        $scope.message = Util.createMessage("success", "Proposta emitida com sucesso!");

                        if (sair === true) {
                            $timeout(function () {

                                window.open(Util.getUrl('/proposta/index'), '_self');

                            }, 1000);
                        }
                        else {
                            if (Util.isPathValid(resp, 'result.prtId')) {

                                var ehGerenteEmitindo = $scope.proposta.EhGerenteEmitindo;

                                $scope.codigoDaProposta = resp.result.prtId;
                                if ($scope.codigoDaProposta) {
                                    $scope.read($scope.codigoDaProposta, function () {

                                        if ($scope.proposta) {
                                            $scope.proposta.EhGerenteEmitindo = ehGerenteEmitindo;
                                        }
                                    });
                                }

                                $scope.lstPropostaItemBoleto = null;
                                $scope.listarPropostaItemBoletoPorProposta($scope.codigoDaProposta);

                            }
                            angular.element("#modal-post-salvamento").modal();
                            $timeout(function () {

                                $scope.message = null;
                            }, 2000);
                        }
                    }
                    else {
                        angular.element("#modal-validacao-pendendencia").modal();
                    }
                }

            }
        });
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

    $scope.calcularImpostoDoPedido = function () {

        if($scope.callPromise)
            $timeout.cancel($scope.callPromise);


        $scope.callPromise = $timeout(function () {
            if (/*$scope.proposta.UEN_ID == 1 &&*/ Util.isPathValid($scope, "proposta.PROPOSTA_ITEM")) {

                angular.forEach($scope.proposta.PROPOSTA_ITEM, function (value, index) {

                    $scope.calcularImpostosNosItens(value);
                });
            }
        }, 1000);
        
    }

    $scope.calcularDescontoDoItem = function (item) {

        if ($scope.callPromiseDescontoDoItem)
            $timeout.cancel($scope.callPromiseDescontoDoItem);

        $scope.callPromiseDescontoDoItem = $timeout(function () {
                $scope.calcularImpostosNosItens(item);
             
        }, 1000);
    };

    $scope.calcularImpostosNosItens = function (item) {

        var propostaItem = angular.copy(item);

        if (!propostaItem.PPI_ID) {

            var proposta = angular.copy($scope.proposta);
            proposta.PROPOSTA_ITEM = null;

            propostaItem.PROPOSTA = proposta;
        }

       // $scope.limparPedidoPagamento();
        if (Util.isPathValid($scope, "proposta.INFO_CLIENTE.TIPO_CLI_ID")) {
            var url = Util.getUrl("/proposta/CalcularDescontoDosImpostos");

            if (propostaItem /*&& propostaItem.PRODUTO_COMPOSICAO.EhCurso == true*/) {

                item.lock = true;
                formHandlerService.read($scope, {
                    url: url,
                    targetObjectName: 'resultadoCalc',
                    responseModelName: 'resultadoCalc',
                    showAjaxLoader: true,
                    data: {
                        propostaItem: propostaItem,
                        proposta : $scope.proposta
                    },
                    success: function (resp) {
                        item.lock = false;
                        if (resp.success && Util.isPathValid($scope, "resultadoCalc")) {


                            var infoFaturaEntrada = $scope.resultadoCalc.ResultadoEntrada;
                            var infoFaturaParcela = $scope.resultadoCalc.ResultadoParcela;

                            if (infoFaturaEntrada != null) {

                                var valorFatura = infoFaturaEntrada.IFF_TOTAL_LIQUIDO;
                                item.PPI_VALOR_ENTRADA = valorFatura;
                                item.IFF_ID_ENTRADA = infoFaturaEntrada.IFF_ID;
                                item.INFO_FATURA1 = infoFaturaEntrada;
                            }
                            else {
                                item.IFF_ID_ENTRADA = null;
                                item.INFO_FATURA1 = null;
                            }

                            if (infoFaturaParcela != null) {

                                var valorFatura = infoFaturaParcela.IFF_TOTAL_LIQUIDO;
                                item.PPI_VALOR_PARCELA = valorFatura;
                                item.IFF_ID = infoFaturaParcela.IFF_ID;
                                item.INFO_FATURA = infoFaturaParcela;
                            }
                            else {
                                
                                item.IFF_ID = null;
                                item.INFO_FATURA = null;
                            }
                            $scope.recalcularSomaParcelas(item);

                        }
                        else {

                            item.INFO_FATURA = null;
                            item.INFO_FATURA1 = null;
                            item.PPI_VALOR_ENTRADA = item.PPI_VALOR_BRUTO_ENTRADA;
                            item.PPI_VALOR_PARCELA = item.PPI_VALOR_BRUTO_PARCELA;
                            item.PPI_TOTAL = item.PPI_VALOR_UNITARIO * item.PPI_QTD;
                        }
                    },
                    fail: function () {
                        item.lock = false;
                    }

                });
            }
        }

    };

    $scope.listarPropostaItemPorProposta = function (prtId) {

        if (prtId)
            $scope.PRT_ID = prtId;
       
        if ($scope.PRT_ID) {

            var url = Util.getUrl("/proposta/listarPropostaItem");

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'lstPropostaItemModal',
                responseModelName: 'lstPropostaItem',
                showAjaxLoader: true,
                data: {
                    prtId: $scope.PRT_ID
                }
            });
        }
    };

    $scope.abrirModalHistorico = function (PPI_ID) {

        $scope.listarHistoricos(null, PPI_ID);
        angular.element("#modal-historico").modal();
    }
    $scope.abrirModalDescricao = function (descricao) {

        $scope.descricaoHistorico = descricao;
        angular.element("#descricao-historico-pedido").modal();
    }

    $scope.listarHistoricos = function (paginaReq, PPI_ID) {

        $scope.lstHistoricos = [];
        var url = Util.getUrl("/proposta/listarHistoricoDaPropostaItem");

        if (PPI_ID) {

            $scope.PPI_ID = PPI_ID;
        }
        else {

            PPI_ID = $scope.PPI_ID;
        }

        if (paginaReq) {

            url += "?pagina=" + paginaReq;
        }

        if (!$scope.filtro)
            $scope.filtro = {};

        $scope.filtro.PPI_ID = PPI_ID;


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
    
    $scope.dispararAcaoAdicaoProduto = function () {

        if (!Util.isPathValid($scope, 'proposta.TPP_ID')) {
            $scope.message = Util.createMessage('fail', 'Infome o tipo de proposta antes de adicionar o produto.');
            return;

        }

        if ($scope.proposta && $scope.proposta.TPP_ID == 2) {
            $scope.adicionarLinha();

        } else {

            $scope.filtro = {};
            $scope.listarProdutoComposicao();

            if ($scope.proposta.TPP_ID == 3 && !Util.isPathValid($scope, 'proposta.CLI_ID')) {

                $scope.message = Util.createMessage('fail', 'Selecione primeiro o Cliente/Prospect antes de adicionar o produto.');
                return;
            }
            angular.element("#modal-produto-composicao").modal();
        }
    }

    $scope.abrirModalOpcoesDePagamento = function (propostaItem) {

        $scope.modalPropostaItem = propostaItem;
        angular.element("#modal-opcoes-pagamento").modal();
    }

    $scope.dispararAcaoPagamentoSelecionado = function (item) {

        if (item && item.TIPO_PAGAMENTO && item.TIPO_PAGAMENTO.TPG_ID != null)
        {
            item.TPG_ID = item.TIPO_PAGAMENTO.TPG_ID;
            if (item.TIPO_PAGAMENTO.TPG_TIPO == 0) {

                if (item.TIPO_PAGAMENTO.TPG_ID != 9) {
                    item.PPI_QTD_PARCELAS = 1;
                }
                item.PPI_VALOR_ENTRADA = null;
                item.PPI_VALOR_ENTRADASTr = null;
                item.PPI_VALOR_BRUTO_ENTRADA = null;
                item.PPI_VALOR_BRUTO_ENTRADASTr = null;
                item.PPI_DATA_VENCIMENTO_SEG_PARCELAStr = null;
            }            
        }
        else if (!item.TIPO_PAGAMENTO)
        {
            item.TPG_ID = null;
        }

        $scope.calcularParcela(item);
    }


    $scope.emitirPedidoDaProposta = function (prtId) {

        if (confirm("Deseja realmente gerar o pedido a partir dessa proposta?")) {

            $scope.objSubmit = { prtId: prtId };
            formHandlerService.submit($scope, {
                url: Util.getUrl("/proposta/emitirPedidoDaProposta"),
                objectName: 'objSubmit',
                showAjaxLoader: true,
                success: function (resp, status, config, message, validationMessage) {

                    $scope.message = message;
                    $scope.erros = validationMessage;

                    if (resp.success) {

                        $scope.message = Util.createMessage("success", "Pedido emitido com sucesso!");

                        $timeout(function () {
                            $scope.message = null;
                            $scope.recarregarListarPropostas();
                            $scope.listarPropostaItemPorProposta();

                        }, 1000);
                    }

                }
            });
        }
        else {
            return false;
        }
    }

    $scope.abrirModalBuscarCliente = function () {

        $scope.filtro = { pesquisaCpfCnpjPorIqualdade: true, uenLogada : true };
        angular.element("#modal-buscar-cliente").modal();
    }

    $scope.buscarClientes = function (pageRequest) {

        $scope.message = null;
        $scope.listado = false;
        var url = Util.getUrl("/franquia/clientes/BuscarClienteGlobal");

        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }
        else {
            pageRequest = 1;
        }

        var config = {
            url: url,
            targetObjectName: 'clientes',
            responseModelName: 'clientes',
            showAjaxLoader: true,          
            pageConfig: { pageName: 'page'},
            success: function (response) {
                $scope.listado = true;

            }
        };

        if ($scope.filtro) {

            $scope.filtro.registroPorPagina = 15;
            $scope.filtro.pagina = pageRequest;
            $scope.filtro.lstClaCliId = [2, 3];

            if ($scope.filtro.cpf_cnpj || $scope.filtro.nome ||
                $scope.filtro.email || $scope.filtro.telefone || $scope.filtro.codigoAssinatura || !$scope.ehGerenteEmitindo) {

                if ($scope.filtro.dddTelefone && !$scope.filtro.telefone) {

                    $scope.message = Util.createMessage("fail", "Preencha o telefone ao preencher o ddd");
                    return;
                }

                if (!$scope.ehGerenteEmitindo) {

                    if (!Util.isPathValid($scope, 'proposta.REP_ID')) {
                        $scope.message = Util.createMessage("fail", "Não é possível consultar. O representante não está presente.");
                        return;
                    }

                    $scope.filtro.REP_ID = $scope.proposta.REP_ID;
                }
                config.data = angular.copy($scope.filtro);
            }
            else {

                $scope.message = Util.createMessage("fail", "Preenche pelo menos um filtro");
                return;
            }


            formHandlerService.read($scope, config);
        }
        else {
            $scope.message = Util.createMessage("fail", "Preenche pelo menos um filtro");

        }
    };

    $scope.selecionarCliente = function (item) {

        if (item && item.CLI_ID) {

            $scope.proposta.CLI_ID = item.CLI_ID;
            $scope.obterDadosDoProspect();
        }

        angular.element("#modal-buscar-cliente").modal('hide');
    }

    $scope.semanaFatSelecionada = function () {

        if ($scope.proposta.DATA_FATURAMENTO_STR) {

            var date = $scope.proposta.DATA_FATURAMENTO_STR.split("/");
            var ano = date[0];
            var mes = date[1];
            var dia = date[2];

            $scope.proposta.PRT_DATA_FATURAMENTO_AGENDADA = new Date(ano, mes, dia);
        }
    }

    $scope.adicionarProdutoRenovacao = function (item) {

        if (item && item.ASN_NUM_ASSINATURA) {
            var url = Util.getUrl("/proposta/retornarProdutoRenovacao");
            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'infoProdutoRenovacao',
                responseModelName: 'infoProdutoRenovacao',
                //pageConfig: { pageName: 'page', pageTargetName: 'paginaHistorico' },
                data: {
                    codAssinatura: item.ASN_NUM_ASSINATURA,
                    empId: $scope.proposta.EMP_ID
                },
                showAjaxLoader: true,
                success: function (resp) {

                    if (resp.success) {

                        if (Util.isPathValid($scope, 'infoProdutoRenovacao.ProdutoComposicao')) {
                            $scope.infoProdutoRenovacao.ProdutoComposicao.QTD = 1;
                            $scope.alterarProduto(item, $scope.infoProdutoRenovacao);
                            $scope.proposta.liberarListagemProduto = true;

                        }
                    }
                    else {

                        if (item !== null) {
                            item.PRODUTO_COMPOSICAO = null;
                            item.CMP_ID = null;
                            item.CmpId = null;
                            item.PPI_QTD = null;
						    item.PRT_IDENTIFICACAO_TURMA = null;
                            item.PPI_VALOR_UNITARIO = null;
                            item.PPI_TOTAL = null;
                            item.PPI_DATA_VENCIMENTO = null;
                            item.PPI_QTD_CONSULTA = null;
                            item.PPI_GERA_NOTA = false;
                            item.PPI_VALOR_UNITARIO_PRODUTO = null;
                        }
                    }
                },
                fail: function () {
                    if (item !== null) {
                        item.PRODUTO_COMPOSICAO = null;
                        item.CMP_ID = null;
                        item.CmpId = null;
                        item.PPI_QTD = null;
						item.PRT_IDENTIFICACAO_TURMA = null;
                        item.PPI_VALOR_UNITARIO = null;
                        item.PPI_TOTAL = null;
                        item.PPI_DATA_VENCIMENTO = null;
                        item.PPI_QTD_CONSULTA = null;
                        item.PPI_GERA_NOTA = false;
                        item.PPI_VALOR_UNITARIO_PRODUTO = null;
                    }
                }
            });
        }

    }

    $scope.checarCortesia = function (item) {

        if (item && item.PPI_CORTESIA == true) {

            item.PPI_VALOR_UNITARIO = 0;
            item.PPI_TOTAL = 0;
            item.PPI_VALOR_ENTRADA = 0;
            item.PPI_QTD_PARCELAS = 0;
            item.PPI_VALOR_PARCELA = 0;
        }

        $scope.calcularTotais();
    }


    $scope.abrirModalRepresentante = function () {
        if ($scope.proposta.CLI_ID) {

            $scope.filtro = { CLI_ID: $scope.proposta.CLI_ID, uenLogada: true };

            if (!Util.isPathValid($scope, 'proposta.EMP_ID')) {

                $scope.message = Util.createMessage('fail', 'Selecione a empresa da proposta.');
                return;
            }
            $scope.buscarTodosOsRepresentanteDoCliente();

            angular.element('#modal-busca-representante').modal();
        }
        else {
            $scope.message = Util.createMessage('fail', 'Selecione o Cliente/Prospect antes de pesquisar');
        }
    }

    $scope.removerRepresentante = function () {

        $scope.proposta.REP_ID = null;
        $scope.proposta.REPRESENTANTE = null;
    }

    $scope.adicionarRepresentante = function (rep) {

        $scope.proposta.REPRESENTANTE = rep.Representante;
        $scope.proposta.REP_ID = rep.Representante.REP_ID;
        $scope.proposta.CAR_ID = rep.CarId;
        angular.element("#modal-busca-representante").modal('hide');
    }

    $scope.buscarTodosOsRepresentanteDoCliente = function (pageRequest) {

        $scope.message = null;
        $scope.listado = false;

        if ($scope.filtro.CLI_ID) {
            
            var url = Util.getUrl("/franquia/representante/listarTodosOsRepresentantesDoCliente");

            if (pageRequest) {
                url += "?pagina=" + pageRequest;
            }

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'representantes',
                responseModelName: 'representantes',
                showAjaxLoader: true,
                pageConfig: { pageName: 'page', /*pageTargetName: 'paginaProspect' */},
                data: $scope.filtro,
                success: function (response) {
                    $scope.listado = true;

                }
            });
        }
        else {
            $scope.message = Util.createMessage('fail', 'Selecione o Cliente/Prospect antes de pesquisar');
        }
    }



    $scope.abrirModalForcarBaixa = function (ppiId) {

        $scope.codigoPropostaItem = ppiId;
        $scope.PassoOperacao = "Verificando se a parcela já foi paga...";
        delete $scope.existeParcelaPaga;
        angular.element("#modal-forcar-baixa").modal();
        $scope.checarSeAhParcelaPaga(ppiId);
    }


    $scope.checarSeAhParcelaPaga = function (ppiId) {

        var url = Util.getUrl("/proposta/ChecarSeAhParcelaPaga");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'existeParcelaPaga',
            responseModelName: 'existeParcelaPaga',
            showAjaxLoader: true,
            data: { ppiId: ppiId },
            success: function () {

                if ($scope.existeParcelaPaga == true) {

                    $scope.PassoOperacao = "Existem parcelas pagas. O pedido pode ser baixado";
                }
                else {
                    $scope.PassoOperacao = "Não éxistem parcelas pagas. O pedido não pode ser baixado";
                }
            }

        });
    }

    $scope.forcarBaixaAutomatica = function (prtId) {

        if (confirm("Deseja realmente forçar a baixa de pagamento do pedido ?")) {

            $scope.objSubmit = { prtId: prtId };
            formHandlerService.submit($scope, {
                url: Util.getUrl("/proposta/forcarBaixaAutomatica"),
                objectName: 'objSubmit',
                showAjaxLoader: true,
                success: function (resp, status, config, message, validationMessage) {

                    $scope.message = message;
                    $scope.erros = validationMessage;
                    $scope.buttonForcarBaixa = 'reset';

                    if (resp.success) {

                        $scope.message = Util.createMessage("success", "Baixa de pagamento realizada com sucesso!");

                        $timeout(function () {
                            $scope.message = null;
                            $scope.recarregarListarPropostas();
                            $scope.listarPropostaItemPorProposta();
                            angular.element("#modal-forcar-baixa").modal('hide');

                        }, 1000);
                    }


                }
            });
        }
        else {
            return false;
        }
    }

    // Evita que o período bônus seja superior a 12
    $scope.bloquearPeriodoBonus = function (item) {

        if (item && item.PPI_PERIODO_MES_BONUS) {
            
            if (item.PPI_PERIODO_MES_BONUS > 12) {

                item.PPI_PERIODO_MES_BONUS = 12;
            }

            if (item.PPI_PERIODO_MES_BONUS < 1) {

                item.PPI_PERIODO_MES_BONUS = 1;
            }
        }
    }

    $scope.listarAssinaturaAutocomplete = function (assinatura) {

        if ($scope.select2CtrlRepresentante != null)
            $scope.select2CtrlRepresentante.loader = true;
        var filtro = { assinatura: assinatura };

        formHandlerService.read($scope, {
            url: Util.getUrl("/proposta/ListarAssinaturaAutocomplete"),
            targetObjectName: 'lstAssinatura',
            responseModelName: 'lstAssinatura',
            data: filtro,
            success: function () {
                if ($scope.select2CtrlRepresentante != null)
                    $scope.select2CtrlRepresentante.loader = false;


            }

        });
    }

    $scope.obterGruposDeFiltroDoPedido = function () {

        var url = Util.getUrl("/proposta/obterGruposDeFiltroDaProposta");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'gruposDeFiltro',
            responseModelName: 'gruposDeFiltro',
            showAjaxLoader: true,
            success: function () {

                if ($scope.gruposDeFiltro) {

                    var grupos = $scope.gruposDeFiltro.Grupos;
                    var status = grupos.status;
                    var tipoProposta = grupos.tipoProposta;
                    //var data = grupos.data;
                    //var dataFaturamento = grupos.dataFaturamento;

                    FilterService.adicionarGrupoDeFiltro($scope.filtros, status, "status");
                    FilterService.adicionarGrupoDeFiltro($scope.filtros, tipoProposta, "tipoProposta")
                    //FilterService.adicionarGrupoDeFiltro($scope.filtros, data, "data");
                    //FilterService.adicionarGrupoDeFiltro($scope.filtros, dataFaturamento, "dataFat");
                }
            }

        });
    }

    $scope.criarFiltros = function () {

        $scope.filtros =
            [{
                nomeGrupo: 'Padrão',
                idGrupo: "padrao",
                filtros: [
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
                        chave: "cpfCnpj",
                        ordem: 2,
                        maxLength: 14,
                        size: 170,
                        tipo: 'texto'
                    },
                    {
                        label: "Id da Proposta",
                        chave: "PRT_ID",
                        ordem: 0,
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
                nomeGrupo: 'Detalhes',
                idGrupo: "cod",
                filtros: [
                    {
                        label: "Representante",
                        chave: "REP_ID",
                        ordem: 3,
                        tipo: 'autocomplete',
                        lstAutoCompleteName: 'lstRepresentante',
                        funcaoAutoComplete: 'listarRepresentanteAutocomplete'
                    },
                    {
                        label: "Produtos",
                        chave: "PRO_ID",
                        ordem: 3,
                        tipo: 'autocomplete',
                        lstAutoCompleteName: 'lstProdutos',
                        funcaoAutoComplete: 'getLstProdutosPorNome'
                    }
                ]
            },
            {
                nomeGrupo: 'Por Código',
                idGrupo: "cod",
                filtros: [

                    {
                        label: "Id do Item da Proposta",
                        chave: "PPI_ID",
                        ordem: 0,
                        tipo: 'texto'
                    }
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
                ]
            },
            {
                nomeGrupo: 'Por Tipo',
                idGrupo: "status",
                filtros: [
                    {
                        label: "Tipo de Proposta",
                        chave: "tipoProposta",
                        ordem: 0,
                        tipo: 'grupo',
                        grupo: []

                    },
                ]
            },
            {
                nomeGrupo: 'Por Data',
                idGrupo: "padrao",
                filtros: [
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
                        label: "Data Pagamento",
                        chave: "dataPagamento",
                        ordem: 3,
                        size: 170,
                        tipo: 'data'
                    },
                    {
                        label: "Data de Vencimento Inicial",
                        chave: "dataVencimentoInicial",
                        ordem: 4,
                        size: 96,
                        tipo: 'data'
                    },
                    {
                        label: "Data de Vencimento Inicial",
                        chave: "dataVencimentoFinal",
                        ordem: 5,
                        size: 170,
                        tipo: 'data'
                    },

                ]
            },
            {
                nomeGrupo: 'Outros',
                idGrupo: "other",
                filtros: [
                    {
                        label: "Região",
                        chave: "RG_ID",
                        ordem: 0,
                        size: 96,
                        tipo: 'select',
                        valueName: 'RG_ID',
                        labelName: 'RG_DESCRICAO',
                        renderIf: ($scope.ehGerente && !$scope.ehFranquiado)
                    },
                    //{
                    //    label: "UEN",
                    //    chave: "UEN_ID",
                    //    ordem: 0,
                    //    size: 96,
                    //    tipo: 'select',
                    //    valueName: 'UEN_ID',
                    //    labelName: 'UEN_DESCRICAO',
                    //    renderIf: $scope.ehGerente
                    //},
                    {
                        label: "Tipo de Pagamento",
                        chave: "TPG_ID",
                        ordem: 0,
                        size: 96,
                        tipo: 'select',
                        valueName: 'TPG_ID',
                        labelName: 'TPG_DESCRICAO'
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
                //{
                //    nomeGrupo: 'Número de Nota Fiscal',
                //    idGrupo: "nota_fiscal",
                //    filtros: [
                //                {
                //                    label: "Código Nota Fiscal Inicial",
                //                    chave: "numeroNotaInicial",
                //                    ordem: 0,
                //                    size: 50,
                //                    tipo: 'texto'
                //                },
                //                {
                //                    label: "Código Nota Fiscal Final",
                //                    chave: "numeroNotaFinal",
                //                    size: 50,
                //                    ordem: 1,
                //                    tipo: 'texto'
                //                }
                //    ]
                //},
            ];
    };

    $scope.tipoPropostaAlterado = function () {

        if (Util.isPathValid($scope, "proposta")) {
            $scope.proposta.PROPOSTA_ITEM = [];
        }
    }

    $scope.listarPropostaItemBoletoPorProposta = function (prtId) {

        $scope.lstPropostaItemBoleto = null;
        var url = Util.getUrl("/proposta/ListarPropostaItemDeBoleto");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstPropostaItemBoleto',
            responseModelName: 'lstPropostaItemBoleto',
            showAjaxLoader: true,
            data: {
                prtId: prtId
            }
        });
    };

    $scope.recuperarDadosDoRepresentante = function (REP_ID) {

        if (REP_ID) {
            var url = Util.getUrl("/representante/RecuperarDadosDoRepresentante");


            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'proposta.REPRESENTANTE',
                responseModelName: 'representante',
                data: { REP_ID: REP_ID },
                success: function () {

                }
            });
        }
    }


    $scope.listarCarteirasDoRepresentanteECliente = function (cliId, repId) {

        var url = Util.getUrl("/franquia/carteiramento/ListarCarteirasDoRepreClienteEAssi");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstCarteiramento',
            responseModelName: 'lstCarteiramento',
            showAjaxLoader: true,
            data: {
                cliId: cliId,
                repId: repId
            },
            success: function (resp) {

            }
        });
    }

    $scope.listarCarteirasDoRepresentante = function () {

        var url = Util.getUrl("/franquia/carteiramento/CarteiramentosDaRepresentante");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstCarteiramento',
            responseModelName: 'carteiramentos',
            showAjaxLoader: true,
            success: function (resp) {

            }
        });
    }

    $scope.checarEmailValido = function (email) {
        
        if (email) {
            $scope.emailRequest = {
                AEM_EMAIL: email
            };

            formHandlerService.submit($scope, {
                url: Util.getUrl("/franquia/clientes/ChecarCliente"),
                objectName: 'emailRequest',
                showAjaxLoader: true,
                success: function (resp, status, config, message, validationMessage) {

                    $scope.erros = validationMessage;
                    $scope.message = message;
                    $scope.modalEnvioEmail.emailValido = resp.success;

                    if (resp.success) {
                        $scope.adicionarEmail(email, true);
                        $scope.emailCustom = null;
                    }
                },
                fail: function () {

                }
            });
        }
    }

    $scope.cancelarModalSalvamento = function () {

        angular.element("#modal-validacao-pendendencia").modal('hide');
    }

    if (window.ComprovanteController !== undefined) {

        ComprovanteController($scope, formHandlerService, $http, $timeout);
    }

    $scope.abrirDetalhes = function (prtId) {

        if (prtId != null) {
            var url = Util.getUrl("/proposta/detalhes?prtId=" + prtId);
            post(url, true);
        }
    }

    $scope.recuperarRegrasCampanhaDeVenda = function(){

        if (Util.isPathValid($scope, 'proposta.TPP_ID')) {

            $scope.buscarCampanhaVenda($scope.proposta.TPP_ID);
        }
    }

    $scope.buscarCampanhaVenda = function (tppId) {

        var url = Util.getUrl("/registroLiberacao/BuscarCampanhaVenda");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'proposta.CAMPANHA_VENDA',
            responseModelName: 'campanhaVenda',
            showAjaxLoader: true,
            data: {tppId : tppId},
            success: function (resp) {
                $scope.proposta.CVE_ID = $scope.proposta.CAMPANHA_VENDA.CVE_ID;
            }
        });
    }

    $scope.abrirPedidoDaProposta = function (prtId) {

            var url = Util.getUrl("/proposta/RetornarCodPedidoDaProposta");

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'pedCrmId',
                responseModelName: 'pedCrmId',
                showAjaxLoader: true,
                data: { prtId: prtId },
                success: function (resp) {

                    var url = Util.getUrl('/pedido/index?pedCrmId=') + $scope.pedCrmId;
                    post(url, true);
                }
            });
    }


    $scope.abrirPropostaPorItem = function (ppiId) {

        var url = Util.getUrl('/proposta/index?ppiId=') + ppiId;
        post(url, true);
    }

    $scope.abrirPedidoPorItem = function (ipeId) {

        var url = Util.getUrl('/pedido/index?ipeId=') + ipeId;
        post(url, true);
    }

    $scope.abrirModalDetalhesParcelas = function (lstParcelas) {

        $scope.lstParcelasModal = null;

        if (lstParcelas != null)
            $scope.lstParcelasModal = lstParcelas;

        angular.element("#modal-validacao-inadimplencia-parcelas").modal();
    }


    $scope.adicionarVariasNotasAoLoteNFe = function (ppiID) {

        if (confirm("Gerar nota fiscal antecipada?")) {

            $scope.notaBatchModal = { ppiID: ppiID };
            formHandlerService.submit($scope, {
                url: Util.getUrl("/proposta/adicionarVariasNotasAoLoteNFe"),
                objectName: 'notaBatchModal',
                showAjaxLoader: true,
                success: function (resp, status, config, message, validationMessage) {

                    $scope.message = message;
                    $scope.buttonGerarNota = 'reset';

                    if (resp.success) {

                        $scope.message = Util.createMessage("success", "Notas Fiscal Agendada com sucesso !!");
                        
                        $timeout(function () {
                            $scope.message = null;
                            angular.element("#modal-batch-notaFiscal").modal('hide');
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

    $scope.abrirModalDeEmails = function (cliId, campo) {

        $scope.modalEnvioEmail = {
            CLI_ID: cliId,
            campo: campo,
        };

        $scope.listarEmailsDoCliente(cliId);
        $scope.listarAssinaturas(cliId);
        angular.element("#modal-emails-cli").modal();
    }

    $scope.adicionarEmailProposta = function (email, campo) {

        if (email) {

            $scope.proposta[campo] = email;
            angular.element("#modal-emails-cli").modal('hide');
        }
        else {
            $scope.message = Util.createMessage('fail', 'Selecione um E-Mail');
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
    }


    $scope.quantidadeAlterada = function (item) {

        if ($scope.proposta && $scope.proposta.UEN_ID)
            var uen = $scope.proposta.UEN_ID;
        else {
            uen = $scope.uen.UEN_ID;
        }

        if (uen == 1) {

            item.Participantes = [];

            if (item.PPI_QTD && item.PPI_QTD > 0) {

                for (var i = 0; i < item.PPI_QTD; i++) {

                    item.Participantes.push({ PED_EH_O_COMPRADOR: true})
                }
            }
        }
    }

    $scope.abrirSubModalParticipantes = function (PPI_ID) {

        $scope.lstPedidoParticipante = null;

        $scope.pesquisarPorPropostaItem(PPI_ID);
        angular.element("#modal-participante").modal();
    }

    $scope.pesquisarPorPropostaItem = function (PPI_ID) {

        if (PPI_ID) {
            var url = Util.getUrl("/proposta/PesquisarPorPropostaItem");

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'lstPedidoParticipante',
                responseModelName: 'lstPedidoParticipante',
                showAjaxLoader: true,
                data: {
                    PPI_ID: PPI_ID
                }
            });
        }
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

    $scope.calcularProvaReal = function (proItm) {

        var parcelas = proItm.PPI_QTD_PARCELAS;
        if (proItm.TIPO_PAGAMENTO && proItm.TIPO_PAGAMENTO.TPG_ID == 9) {
            parcelas = 1;
        }
        proItm.ProvaRealTotal = (parcelas * proItm.PPI_VALOR_PARCELA) + proItm.PPI_VALOR_ENTRADA
    };


    $scope.carregarTipoNegociacao = function () {

        var url = Util.getUrl("/proposta/listarTipoNegociacao");

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

    $scope.confirmarAceiteVendaAPrazo = function (ppiId) {
        
        if (ppiId && confirm("Deseja realmente alterar o status da proposta?")) {

            $scope.alteracaoModal = { PPI_ID: ppiId };
            formHandlerService.submit($scope, {
                url: Util.getUrl("/proposta/InformarAceiteVendaAPrazo"),
                objectName: 'alteracaoModal',
                showAjaxLoader: true,
                success: function (resp, status, config, message, validationMessage) {

                    $scope.message = message;
                    $scope.erros = validationMessage;

                    $scope.buttonAlt = 'reset';
                    if (resp.success) {

                        $scope.message = Util.createMessage("success", "Status alterado com sucesso!");

                        $timeout(function () {
                            $scope.message = null;
                            $scope.recarregarListarPropostas();
                            $scope.listarPropostaItemPorProposta();

                            angular.element("#modal-opcoes-pagamento").modal('hide');

                        }, 1000);
                    }

                }
            });
        }
        else {
            return false;
        }
    }

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
                },
                success: function () {

                }
            });
        }
    };

    $scope.abrirImpostosDaInfoFaturaItm = function (infoFaturaItm, infoFatura) {

        $scope.infoFaturaModalImp = angular.copy(infoFatura);
        $scope.infoFaturaItmModal = angular.copy(infoFaturaItm);

        angular.element("#modal-impostos-utilizados").modal();
    };

    $scope.empresaAlterada = function () {

        $scope.proposta.PROPOSTA_ITEM = [];
    };

    if (window.ExtornoPagamentoParcelaController !== undefined) {

        ExtornoPagamentoParcelaController($scope, formHandlerService, $timeout);
    }

    if (window.LoteNotaFiscalController !== undefined) {
        LoteNotaFiscalController($scope, formHandlerService, $timeout, $window);
    }
}]);