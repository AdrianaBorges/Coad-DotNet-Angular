function ImportacaoClienteController($scope, formHandlerService, $http, conversionService, $timeout) {
    
    $scope.query = { show: true };


    $scope.carregaRegioes = function () {

        var parans = {};

        if ($scope.representante && $scope.representante.UEN_ID) {

            parans.UEN_ID = angular.copy($scope.representante.UEN_ID);
        }


        var url = Util.getUrl('/regiao/regioes');
        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'regioes',
            responseModelName: 'lstRegiao',
            data: parans,
            success: function () {

            }
        });

    }


    $scope.carregaRegioes();

    $scope.alterarPasso = function (value) {

        $scope.passoImport = value;
    }

    $scope.prosseguirPasso = function () {

        $scope.passoImport++;
    }

    $scope.regredirPasso = function () {

        $scope.passoImport--;
    }

    $scope.abreModalImportarClienteParaAgenda = function () {

        $scope.modalImportacao = {

            CLI_ID : $scope.clienteModal.CLI_ID,
            ASSINATURA_TELEFONE: [],
            ASSINATURA_EMAIL: []
        };

        $scope.lstEmail = [];
        $scope.lstTelefone = [];
        $scope.passoImport = 1;
        $scope.listarAssinaturas();
        //$scope.abreModal("#modalImportacao");
        angular.element("#modalImportacao").modal();
    }

    $scope.listarAssinaturas = function () {

        $scope.lstAssinatura = [];
        var data = {
            cliId: $scope.clienteModal.CLI_ID
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

        $scope.telefoneListado = false;
        $scope.emailListado = false;
        $scope.lstTelefone = [];
        $scope.lstEmail = [];

        if ($scope.objAssinatura) {

            $scope.listarEmails();
            $scope.listarTelefones();
        }
    }

    $scope.listarEmails = function () {

        $scope.lstEmail = [];
        var data = {
            asnNumAssinatura: $scope.objAssinatura.ASN_NUM_ASSINATURA
        };

        var url = Util.getUrl("/clientes/listarEmailDasAssinaturas");


        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstEmail',
            responseModelName: 'lstEmails',
            showAjaxLoader: true,
            //pageConfig: { pageName: 'page' /*, pageTargetName: 'pageAgendamentoDoDia' */ },
            data: data,
            success: function (resp) {
                $scope.emailListado = true;

                if (Util.isPathValid($scope, 'modalImportacao.ASSINATURA_EMAIL')) {

                    $scope.acharEmail($scope.modalImportacao.ASSINATURA_EMAIL);
                }
            }
        });
    }

    $scope.listarTelefones = function () {

        $scope.lstTelefone = [];

        var data = {
            asnNumAssinatura: $scope.objAssinatura.ASN_NUM_ASSINATURA
        };

        var url = Util.getUrl("/clientes/listarTelefoneDasAssinaturas");


        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstTelefone',
            responseModelName: 'lstTelefone',
            showAjaxLoader: true,
            //pageConfig: { pageName: 'page' /*, pageTargetName: 'pageAgendamentoDoDia' */ },
            data: data,
            success: function (resp) {
                $scope.telefoneListado = true;

                if (Util.isPathValid($scope, 'modalImportacao.ASSINATURA_TELEFONE')) {

                    $scope.acharTelefone($scope.modalImportacao.ASSINATURA_TELEFONE);
                }
            }
        });
    }

    $scope.adicionarTelefoneOuEmailImportacao = function (idDrag, idDrop) {

        if (idDrop == "drop_tel" || idDrop == "drop_email") {

            var objTelEmail = angular.copy($scope.objTelEmail);
            if (idDrag == "drag_telefone") {
                $scope.modalImportacao.ASSINATURA_TELEFONE.push(objTelEmail);
            }

            if (idDrag == "drag_email") {
                $scope.modalImportacao.ASSINATURA_EMAIL.push(objTelEmail);
            }

        }
    }

    $scope.inicializarTel = function () {

        angular.forEach($scope.lstTelefone, function (value, index) {

            value.show = true;
        });
    }

    $scope.inicializarEmails = function () {

        angular.forEach($scope.lstEmail, function (value, index) {

            value.show = true;
        });
    }
    
    $scope.acharTelefone = function (tels) {

        $scope.inicializarTel();
        if ($scope.lstTelefone) {

            angular.forEach($scope.lstTelefone, function (value, old) {

                angular.forEach(tels, function (subValue, subOld) {

                    if (value.ATE_DDD == subValue.ATE_DDD && value.ATE_TELEFONE == subValue.ATE_TELEFONE) {

                        value.show = false;
                    }
                });
            });
        }
    }

    $scope.acharEmail = function (emails) {

        $scope.inicializarEmails();
        if ($scope.lstEmail) {

            angular.forEach($scope.lstEmail, function (value, old) {

                angular.forEach(emails, function (subValue, subOld) {

                    if (value.AEM_EMAIL == subValue.AEM_EMAIL) {

                        value.show = false;
                    }
                });
            });
        }
    }

    $scope.excluirTelefone = function (index, descricao) {

        if ($scope.modalImportacao && $scope.modalImportacao.ASSINATURA_TELEFONE) {

            $scope.modalImportacao.ASSINATURA_TELEFONE.splice(index, 1);
        }
    }

    $scope.excluirEmail = function (index, descricao) {

        if ($scope.modalImportacao && $scope.modalImportacao.ASSINATURA_EMAIL) {

            $scope.modalImportacao.ASSINATURA_EMAIL.splice(index, 1);
        }
    }
    $scope.$watch("modalImportacao.ASSINATURA_TELEFONE", function (value, old) {

        if (value) {
            $scope.acharTelefone(value);
        }

    }, true);

    $scope.$watch("modalImportacao.ASSINATURA_EMAIL", function (value, old) {

        if (value) {
            $scope.acharEmail(value);
        }

    }, true);

    $scope.removerTelefoneOuEmailImportacao = function (idDrag, idDrop) {

        if (idDrop == "drop_remove_tel" || idDrop == "drop_remove_email") {

            var objRemoveTelEmail = angular.copy($scope.objRemoveTelEmail);
            var index = $scope.objRemoveTelEmail.index;

            if (idDrag == "drag_remove_tel") {

                $scope.modalImportacao.ASSINATURA_TELEFONE.splice(index, 1);
            }

            if (idDrag == "drag_remove_email") {
                $scope.modalImportacao.ASSINATURA_EMAIL.splice(index, 1);
            }
        }
    };

    $scope.$watch("modalImportacao.ASSINATURA_TELEFONE", function (lstTelefones, old) {

        angular.forEach(lstTelefones, function (value, index) {

            value.index = index;
        });

    }, true);

    $scope.$watch("modalImportacao.ASSINATURA_EMAIL", function (lstEmails, old) {

        angular.forEach(lstEmails, function (value, index) {

            value.index = index;
        });

    }, true);

    $scope.importarClienteParaAgenda = function () {

        if (confirm("Confirmar importação do cliente?")) {
            url = Util.getUrl("/franquia/clientes/ImportarClienteParaAgenda");
            var cliId = $scope.modalImportacao.CLI_ID;

            formHandlerService.submit($scope, {

                url: url,
                objectName: 'modalImportacao',
                showLoader: true,
                success: function (resp, status, config, message, validationMessage) {


                    $scope.message = message;
                    $scope.erros = validationMessage;


                    if (resp.success) {

                        $scope.message = Util.createMessage("success", "Importação realizada com sucesso!");

                        $timeout(function () {
                            $scope.buttonImport = 'reset';
                            $scope._carregarDadosDoCliente(cliId);
                            angular.element("#modalImportacao").modal('hide');
                            $scope.message = null;

                        }, 2000);
                    }
                    else {
                        $scope.buttonImport = 'reset';
                    }
                }
            });
        }
        else {
            return false;
        }
    }
}