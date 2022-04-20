appModule.controller("FilaEmailController", ['$scope', '$http', 'formHandlerService', 'MathService', '$timeout',
    'UploadAjax', 'Upload', 'FilterService', '$sce', '$interval',
function ($scope, $http, formHandlerService, MathService, $timeout,
    UploadAjax, Upload, FilterService, $sce, $interval) {
    
    $scope.initList = function () {

        $scope.criarFiltros();
    }


    $scope.criarFiltros = function () {

        $scope.filtros =
            [{
                nomeGrupo: 'Padrão',
                idGrupo: "padrao",
                filtros: [
                    {
                        label: "Código da Fila",
                        chave: "FilaId",
                        ordem: 0,
                        size: 75,
                        tipo: 'texto'
                    },
                    {
                        label: "E-Mail",
                        chave: "Email",
                        ordem: 1,
                        size: 96,
                        tipo: 'texto'
                    },
                    {
                        label: "Assunto",
                        chave: "Assunto",
                        ordem: 2,
                        size: 170,
                        tipo: 'texto'
                    },
                    {
                        label: "Usuário",
                        chave: "Usuario",
                        ordem: 3,
                        maxLength: 14,
                        size: 170,
                        tipo: 'texto'
                    }
                ]
            },
            {
                nomeGrupo: 'Detalhes',
                idGrupo: "cod",
                filtros: [
                    {
                        label: "Exibir Enviados",
                        chave: "ExibirEnviados",
                        ordem: 0,
                        size: 96,
                        tipo: 'toogle'
                    },
                    {
                        label: "Exibir Cancelados",
                        chave: "ExibirCancelados",
                        ordem: 1,
                        size: 96,
                        tipo: 'toogle'
                    },
                    {
                        label: "Data Criação Inicial",
                        chave: "DataCriacaoInicial",
                        ordem: 2,
                        size: 170,
                        tipo: 'data'
                    },
                    {
                        label: "Data Criacao Final",
                        chave: "DataCriacaoFinal",
                        ordem: 3,
                        size: 96,
                        tipo: 'data'
                    },
                    {
                        label: "Data Envio Inicial",
                        chave: "DataEnvioInicial",
                        ordem: 4,
                        size: 170,
                        tipo: 'data'
                    },
                    {
                        label: "Data Envio Final",
                        chave: "DataEnvioFinal",
                        ordem: 5,
                        size: 96,
                        tipo: 'data'
                    }

                ]
            },
        ];
    };

    $scope.pesquisarFilaEmail = function (pagina) {

        $scope.paginaAtual = pagina;
        $scope.listado = true;
        var url = Util.getUrl("/filaEmail/pesquisarFilaEmail");

        if (pagina) {

            url += "?pagina=" + pagina;
        }

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstFilaEmail',
            responseModelName: 'lstFilaEmail',
            showAjaxLoader: true,
            data: $scope.filtro,
            pageConfig: { pageName: 'page' /*, pageTargetName: 'pageAgendamentoDoDia' */ },
            success: function (resp) {

            }
        });
    };

    $scope.trustHTML = function (value) {

        var trustedContent = $sce.trustAsHtml(value);
        return trustedContent;
    }
    
    $scope.abrirModalCorpoEmail = function (fleId) {

        $scope.recuperarDadosDaFilaEmail(fleId);
        angular.element("#corpo-email").modal();
    };

    $scope.recuperarDadosDaFilaEmail = function (fleId) {

        var url = Util.getUrl("/filaEmail/recuperarDadosDaFilaEmail");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'filaEmail',
            responseModelName: 'filaEmail',
            showAjaxLoader: false,
            data: { fleId : fleId },
            success: function (resp) {

            }
        });
    };

    $scope.abrirModalNotificacao = function (fleId) {

        $scope.recuperarNotificacaoSistema(fleId);
        angular.element("#notificacao-erro").modal();
    };

    $scope.recuperarNotificacaoSistema = function (fleId) {

        var url = Util.getUrl("/jobs/ListarNotificacaoSistema");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'notifySistema',
            responseModelName: 'notifySistema',
            showAjaxLoader: false,
            data: { 
                tnsID: 3,
                codRefInt: fleId
            },
            success: function (resp) {

            }
        });
    }

    $scope.cancelarFilaEmail = function (fleId) {

        var url = Util.getUrl("/filaEmail/cancelarEmailNaFila");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'filaEmail',
            responseModelName: 'filaEmail',
            showAjaxLoader: false,
            data: { fleId: fleId },
            success: function (resp) {

            }
        });
    };

    $scope.cancelarFilaEmail = function (fleId) {

        if (confirm("Cancelar a fila de E-Mail?")) {

            $scope.cancelamentoRequest = { fleId: fleId };

            formHandlerService.submit($scope, {
                url: Util.getUrl("/filaEmail/cancelarEmailNaFila"),
                objectName: 'cancelamentoRequest',
                showAjaxLoader: true,
                success: function (resp, status, config, message, validationMessage) {

                    $scope.message = message;

                    if (resp.success) {

                        $scope.message = Util.createMessage("success", "E-Mail cancelado com sucesso!");
                        $scope.pesquisarFilaEmail();

                        $timeout(function () {
                            $scope.message = null;

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


    $scope.abrirPopOverEmail = function (selector, item, $event) {

        $event.stopPropagation();
        if ($scope.popoverSelector && $scope.popoverSelector != selector) {

            angular.element("#" + $scope.popoverSelector).popover('hide');
        }

        $scope.popoverSelector = selector;
        item.enderecoEmail = item.Email;

        angular.element("#" + $scope.popoverSelector).popover('show');
    }


    $scope.fecharPopOvers = function () {
        if ($scope.popoverSelector) {

            angular.element("#" + $scope.popoverSelector).popover('hide');
        }
    }

    $scope.salvarEmail = function (item, valor) {

            if (confirm("Confirmar Alteração?")) {

                formHandlerService.submit($scope, {
                    url: Util.getUrl("/filaEmail/alterarEmail"),
                    objectName: 'email',
                    showAjaxLoader: true,
                    success: function (resp, status, config, message, validationMessage) {

                        $scope.message = message;

                        if (resp.success) {

                            $scope.message = Util.createMessage("success", "E-Mail alterado com sucesso!");
                            $scope.pesquisarFilaEmail();
                            $scope.fecharPopOvers();

                            $timeout(function () {
                                $scope.message = null;

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

    $scope.evitarProgacao = function ($event) {

        $event.stopPropagation();
    }

    
}]);