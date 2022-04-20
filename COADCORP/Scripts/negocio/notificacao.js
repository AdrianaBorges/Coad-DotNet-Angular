appModule.controller('NotificacaoController', ['$scope', 'formHandlerService', '$http', 'conversionService', '$interval', '$timeout', function ($scope, formHandlerService, $http, conversionService, $interval, $timeout) {
   
    $scope.limit = 30;
    $scope.quantidadeAtual = 0;
    $scope.initNotify = function () {
        
        $scope.qtdNotificacoes = 0;

        $scope.lstQtdNotificacoes();

        //$interval(function () {
        //    $scope.lstUsuariosLogados();
        //}, 1000);

        $interval(function () {
            $scope.lstQtdNotificacoes();
        }, 60000);

        $interval(function () {
            $scope.ContadorTimeOut();
        }, 60000);
    }


    $scope.ContadorTimeOut = function () {

        showAjaxLoader();

        $http({
            method: "Post",
            dataType: "json",
            url: "/notificacoes/ContadorTimeOut"
        }).success(function (response) {

            hideAjaxLoader();

        }).error(function (response) {

            hideAjaxLoader();

        })

    };

    
    $scope.lstUsuariosLogados = function () {

        var url = Util.getUrl("/notificacoes/ListarUsuariosLogados");
        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstusuariologado',
            responseModelName: 'lstusuariologado',
            success: function () {

            }
        });
    }


    $scope.strNgClassPanel = "{" +
    "'panel-info': notify.URG_NTF_ID == 'INFO'," +
    "'panel-warning': notify.URG_NTF_ID == 'WARN'," +
    "'panel-danger': notify.URG_NTF_ID == 'ERROR'," +
    "'panel-primary': notify.URG_NTF_ID == 'PRIORITY'," +
    "'panel-success': notify.URG_NTF_ID == 'SUCCESS'," +
    "'panel-default': !notify.URG_NTF_ID'}";

    $scope.lstQtdNotificacoes = function () {

        var url = Util.getUrl("/notificacoes/ListarQtdNotificacoesNaoLidas");
        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'qtdNotificacoes',
            responseModelName: 'qtdNotificacoes',
            success: function () {

                if ($scope.quantidadeAtual != $scope.qtdNotificacoes) {

                    $scope.quantidadeAtual = $scope.qtdNotificacoes;

                   if ($scope.quantidadeAtual != 0) { $scope.exibirNotificacaoPopup(); }

                }
            }
        });
    }

    $scope.checaResumoNotificacacoesNaoLidas = function(){

        angular.element("#notify-loader").removeClass("notify-loader-hide");

        var url = Util.getUrl("/notificacoes/ChecaResumoNotificacacoesNaoLidas");
        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstResumoNotificacoes',
            responseModelName: 'lstResumoNotificacoes',
            success: function () {

                angular.element("#notify-loader").addClass("notify-loader-hide");
            }
        });
    }

    $scope.LerEMarcarNotificacaoComoLida = function () {

        angular.element("#notify-loader").removeClass("notify-loader-hide");
      
        formHandlerService.submit($scope, {
            url: Util.getUrl("/notificacoes/MarcarTodasAsNotificacoesComoLidas"),
            objectName: 'NTF_ID',
            success: function (resp, status, config, message, validationMessage) {
                //$scope.message = message;
                //$scope.erros = validationMessage;

                if (resp.success) {
                }
                angular.element("#notify-loader").addClass("notify-loader-hide");
            }
        });

               
            
        
    }

    $scope.abreModalNotificacao = function (item, n) {

        angular.element("#notify-loader").removeClass("notify-loader-hide");

        $scope.NTF_ID = { NTF_ID: item.NTF_ID };

        formHandlerService.submit($scope, {
            url: Util.getUrl("/notificacoes/LerEMarcarNotificacaoComoLida"),
            objectName: 'NTF_ID',
            showAjaxLoader: true,
            success: function (resp, status, config, message, validationMessage) {
                $scope.message = message;
                //$scope.erros = validationMessage;
                
                if (resp.success) {

                    $scope.notify = resp.result.notify;
                    $scope.notify.NTF_VISUALIZADO = true;
                    item.NTF_VISUALIZADO = true;
                }
                angular.element("#notify-loader").addClass("notify-loader-hide");
            }
        });        

        n = n || 1;
        $scope.origem = (n > 1);
        angular.element("#modal-notify" + n).modal();
    }

    $scope.listarNotificacoes = function (pageRequest) {
        
        var url = Util.getUrl("/notificacoes/ListarNotificacoes");

        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }

        var data = null;

        if ($scope.filtro) {

            data = angular.copy($scope.filtro);
        }

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstNotificacoes',
            responseModelName: 'lstNotificacoes',
            data: data,
            showAjaxLoader: true,
            pageConfig: { pageName: 'page' },
            success: function () {

            }
        });

    }

    $scope.IrParaCliente = function(){

        if (Util.isPathValid($scope, "notify.TP_NTF_ID")) {

            if ($scope.notify.TP_NTF_ID == 1) {

                if (Util.isPathValid($scope, "notify.AGENDAMENTO.CLI_ID", true)) {

                    var CLI_ID = $scope.notify.AGENDAMENTO.CLI_ID;

                    if ($scope.origem === true) {
                        $scope.abreModalCliente(CLI_ID);
                    }
                    else {
                        $scope.abreModalClienteLayout(CLI_ID);
                    }
                    
                }
            }
            else if ($scope.notify.TP_NTF_ID == 2 || $scope.notify.TP_NTF_ID == 4) {

                if (Util.isPathValid($scope, "notify.CLI_ID", true)) {

                    var CLI_ID = $scope.notify.CLI_ID;

                    if ($scope.origem === true) {
                        $scope.abreModalCliente(CLI_ID);
                    }
                    else {
                        $scope.abreModalClienteLayout(CLI_ID);
                    }
                }
            }
        }
    }


    if (window.InfoClienteController !== undefined) {

        InfoClienteController($scope, formHandlerService, $http, conversionService, $timeout);
    }

    $scope.marcarTodasAsMotificacoesComoLida = function (REP_ID) {

        angular.element("#notify-loader").removeClass("notify-loader-hide");

        $scope.REP_ID = { REP_ID: REP_ID };

        formHandlerService.submit($scope, {
            url: Util.getUrl("/notificacoes/MarcarTodasAsNotificacoesComoLidas"),
            objectName: 'REP_ID',
            showAjaxLoader: true,
            success: function (resp, status, config, message, validationMessage) {
                $scope.message = message;
                //$scope.erros = validationMessage;

                if (resp.success) {

                    $scope.notify = resp.result.notify;
                    $scope.notify.NTF_VISUALIZADO = true;
                    item.NTF_VISUALIZADO = true;
                }
                angular.element("#notify-loader").addClass("notify-loader-hide");
            }
        });

    }

    $scope.exibirNotificacaoPopup = function () {

        var url = Util.getUrl("/notificacoes/listarNotificacoesNaoExibidas");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstNotificacoesPopup',
            responseModelName: 'lstNotificacoesPopup',
            showAjaxLoader: true,
            success: function () {

                $scope.processarFilaNotificacoesPopup();
            }
        });
    }

    $scope.marcarEExibirNotificacaoPopup = function (notify) {

        $scope.notifyPopup = null;
        angular.element("#notify-loader").removeClass("notify-loader-hide");
        $scope.marcacaoRequest = notify;
        formHandlerService.submit($scope, {
            url: Util.getUrl("/notificacoes/MarcarNotificacaoComoExibida"),
            objectName: 'marcacaoRequest',
            showAjaxLoader: true,
            success: function (resp, status, config, message, validationMessage) {
                $scope.message = message;
                //$scope.erros = validationMessage;

                if (resp.success) {

                    $scope.notifyPopup = notify;
                    $scope.lstNotificacoesPopup.splice(0, 1);

                }
                angular.element("#notify-loader").addClass("notify-loader-hide");
            }
        });
    };

    $scope.processarFilaNotificacoesPopup = function () {

        if ($scope.lstNotificacoesPopup) {

            var notify = $scope.lstNotificacoesPopup[0];
            $scope.marcarEExibirNotificacaoPopup(notify);

            if ($scope.lstNotificacoesPopup.length > 0) {
                var promise = $interval(function () {

                    if ($scope.lstNotificacoesPopup.length > 0) {

                        var notify = $scope.lstNotificacoesPopup[0];
                        $scope.marcarEExibirNotificacaoPopup(notify);
                    }
                    else {

                        $interval.cancel(promise);
                        $timeout(function () { // limpa o filtro

                            $scope.notifyPopup = null;
                        }, 3000);
                    }
                }, 4000);

            }
        }
    }

    $scope.fecharNotificacaPopup = function () {

        $scope.notifyPopup = null;
    }
    
}]);     