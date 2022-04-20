appModule.controller("RegistroLiberacaoController", ['$scope', '$http', 'formHandlerService', 'MathService', '$timeout', 'UploadAjax', 'Upload', 'FilterService',
function ($scope, $http, formHandlerService, MathService, $timeout, UploadAjax, Upload, FilterService) {
    
    $scope.popoverAberto = false;
    $scope.initList = function (ehGerente) {
        
        $scope.listarTemplateGrupo();
        $scope.filtro = {pesquisaCpfCnpjPorIqualdade : true};
    }

    $scope.init = function (tplId) {
        if (tplId) {
            $scope.read(tplId, function () {
                 
            });
        }
        else {
            $scope.templateHTML = {};

        }
        $scope.listarTemplateGrupo();
        
    };

    $scope.pesquisarRegistrosLiberacao = function (pagina) {

        $scope.paginaAtual = pagina;
        $scope.listado = true;
        var url = Util.getUrl("/registroLiberacao/pesquisarRegistrosLiberacao");

        if (pagina) {

            url += "?pagina=" + pagina;
        }
        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstRegistroLiberacao',
            responseModelName: 'lstRegistroLiberacao',
            showAjaxLoader: true,
            data: $scope.filtro,
            pageConfig: { pageName: 'page' /*, pageTargetName: 'pageAgendamentoDoDia' */ },
            success: function (resp) {

            }
        });
    };
    
    $scope.listarTemplateGrupo = function () {

        var url = Util.getUrl("/templates/listarTemplateGrupo");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstTemplateGrupo',
            responseModelName: 'lstTemplateGrupo',
            showAjaxLoader: true,

        });
    }

    $scope.read = function (tplId, onSuccess) {

        if (tplId != null) {
            var url = Util.getUrl("/templates/RecuperarDadosDoTemplate");

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'templateHTML',
                responseModelName: 'templateHTML',
                showAjaxLoader: true,
                data: { tplId: tplId },
                success: function () {
                    
                    if (onSuccess && typeof (onSuccess) == 'function') {

                        onSuccess();
                    }
                }
            });
        }
    }

    $scope.abreModalEnviarEmailParaCliente = function (ppiId, cliId) {

        $scope.modalEnvioEmail = {
            PPI_ID: ppiId,
            CLI_ID: cliId,
        };

        $scope.listarEmailsDoCliente(cliId);
        $scope.listarAssinaturas(cliId);
        angular.element("#modal-enviar-email").modal();
    }

       
    $scope.salvarTemplates = function (sair) {

        formHandlerService.submit($scope, {
            url: Util.getUrl("/templates/SalvarTemplate"),
            objectName: 'templateHTML',
            showAjaxLoader: true,
            success: function (resp, status, config, message, validationMessage) {

                $scope.message = message;
                $scope.erros = validationMessage;

                $scope.button = 'reset';
                if (resp.success) {

                    $timeout(function () {

                        window.open(Util.getUrl('/templates/index'), '_self');

                    }, 1000);

                }
            }
        });
    }

    $scope.abrirClienteEmProspect = function (cliId) {

        var url = Util.getUrl("/prospect/Editar?clienteId=" + cliId + "&tipo=cliente");
        post(url, true);
    }

    $scope.abrirClienteNaPosicaoDeCliente = function (cliId) {

        var url = Util.getUrl("/Cliente/Editar?clienteId=" + cliId);
        post(url, true);
    }
    $scope.listarRegistroLiberacaoItem = function (rliId) {

        $scope.lstRegistroLiberacaoItems = null;
        $scope.listado = true;
        var url = Util.getUrl("/registroLiberacao/ListarRegistroLiberacaoItemAtivo");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstRegistroLiberacaoItems',
            responseModelName: 'lstRegistroLiberacaoItems',
            showAjaxLoader: true,
            data: { rliId: rliId },
            //pageConfig: { pageName: 'page' /*, pageTargetName: 'pageAgendamentoDoDia' */ },
            success: function (resp) {

            }
        });
    };

    $scope.abrirModalRegistroLiberacaoItem = function (rliId) {

        $scope.listarRegistroLiberacaoItem(rliId);
        angular.element("#modal-detalhes-proposta").modal();
    }

    $scope.aprovarPendencia = function (rliId) {

        if (confirm("Confirma aprovação?")) {

            formHandlerService.submit($scope, {
                url: Util.getUrl("/registroLiberacao/AprovarPendencia"),
                objectName: 'alteracaoModal',
                showAjaxLoader: true,
                success: function (resp, status, config, message, validationMessage) {

                    $scope.message = message;
                    $scope.erros = validationMessage;

                    $scope.alteracaoModal.buttonVar = 'reset';
                    if (resp.success) {

                        $timeout(function () {

                            angular.element("#modal-alterar-status").modal('hide');
                            $scope.pesquisarRegistrosLiberacao();
                            $scope.message = null;

                        }, 1000);

                    }
                }
            });
        }
    }

    $scope.rejeitarPendencia = function () {

        if (confirm("Confirma rejeição?")) {

            formHandlerService.submit($scope, {
                url: Util.getUrl("/registroLiberacao/RejeitarPendencia"),
                objectName: 'alteracaoModal',
                showAjaxLoader: true,
                success: function (resp, status, config, message, validationMessage) {

                    $scope.message = message;
                    $scope.erros = validationMessage;

                    $scope.alteracaoModal.buttonVar = 'reset';
                    if (resp.success) {

                        $timeout(function () {

                            angular.element("#modal-alterar-status").modal('hide');
                            $scope.pesquisarRegistrosLiberacao();
                            $scope.message = null;

                        }, 1000);

                    }
                }
            });
        }
    }

    $scope.abrirModalAprovarPendencia = function (rliId) {

        $scope.alteracaoModal = { RLI_ID: rliId };
        $scope.alteracaoModal.header = "Aprovar Pendência.";
        $scope.alteracaoModal.label = "Observações.";
        $scope.alteracaoModal.buttonConfig = [{ label: 'Confirmar', state: 'cnf' }, { label: 'Processando...', state: 'process', disabled: true }];
        $scope.alteracaoModal.buttonVar = "button";
        $scope.alteracaoModal.acaoDoBotao = function () {

            return  $scope.aprovarPendencia(rliId);
        };

        angular.element("#modal-alterar-status").modal();
    }

    $scope.abrirModalRejeitarPendencia = function (rliId) {

        $scope.alteracaoModal = { RLI_ID: rliId };
        $scope.alteracaoModal.header = "Rejeitar Pendência.";
        $scope.alteracaoModal.label = "Observações.";
        $scope.alteracaoModal.buttonConfig = [{ label: 'Confirmar', state: 'cnf' }, { label: 'Processando...', state: 'process', disabled: true }];
        $scope.alteracaoModal.buttonVar = "button";
        $scope.alteracaoModal.acaoDoBotao = function () {

            return $scope.rejeitarPendencia(rliId);
        };

        angular.element("#modal-alterar-status").modal();
    }

    $scope.abrirPopover = function (element) {

        if (!$scope.popoverAberto) {
            $scope.popoverAberto = true;
            angular.element("#" + element).popover('show');
        }
    }

    $scope.fecharPopover = function ($event, excetoIndex) {

        if ($event)
            $event.stopPropagation();

        $scope.popoverAberto = false;
        if ($scope.lstRegistroLiberacao != null) {

            var size = $scope.lstRegistroLiberacao.length;
            for (var index = 0; index < size; index++) {

                if ((!excetoIndex || excetoIndex < 0) || excetoIndex != index) {

                    angular.element("#" + 'pop_over_' + index).popover('hide');
                }
            }
        }        
    }

    $scope.abrirDetalhesProposta = function (prtId) {

        if (prtId != null) {
            var url = Util.getUrl("/proposta/detalhes?prtId=" + prtId);
            post(url, true);
        }
    }

    if (window.ClienteInadimplenteController !== undefined) {

        ClienteInadimplenteController($scope, formHandlerService);
    }
}]);