
appModule.controller('MainController', ['$scope', '$window', '$interval', 'formHandlerService', function ($scope, $window, $interval, formHandlerService) {

    $scope.inHomol = inHomologation;
    $scope.isHomologation = function () {

        return $scope.inHomol;
    }


    $scope.pesquisarJobAgendamento = function (pagina) {

        $scope.paginaAtual = pagina;
        $scope.listado = true;
        var url = Util.getUrl("/jobs/PesquisarNotificacoesJobsAtivos");

        if (pagina) {

            url += "?pagina=" + pagina;
        }

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstJobs',
            responseModelName: 'lstJobs',
            showAjaxLoader: false,
            data: $scope.filtro,
            pageConfig: { pageName: 'page' /*, pageTargetName: 'pageAgendamentoDoDia' */ },
            success: function (resp) {

            }
        });
    };

    $scope.initList = function () {

        $scope.dot = "";
        $scope.intervalPromisse = $interval(function () {

            $scope.pesquisarJobAgendamento($scope.paginaAtual);

        }, 5000);

        $scope.intervalPromisse = $interval(function () {

            if($scope.dot == "...")
                $scope.dot = "";
            else {
                $scope.dot += ".";
            }
        }, 700);

        $scope.filtro = { pesquisaCpfCnpjPorIqualdade: true };

    }


    $scope.cancelarNotificacao = function (jnfID, $event) {

        $event.stopPropagation();
        $scope.canc = { jnfID: jnfID };

        formHandlerService.submit($scope, {
            url: Util.getUrl("/jobs/CancelarJobNotificacao"),
            objectName: 'canc',
            showAjaxLoader: true,
            success: function (resp, status, config, message, validationMessage) {

                $scope.buttonCancel = 'reset';
                if (resp.success) {

                }
                else {

                    $scope.message = message;
                    $scope.erros = validationMessage;

                }

            }
        });
    }

    $scope.abrirModalSchedulerNotify = function (notify) {
        
        $scope.notify = notify;

        if (notify && notify.JNF_ID) {

            $scope.listarJobNotificacaoMsgItem(notify.JNF_ID);
        }
        angular.element("#modal-scheduler-notify").modal();
    }


    $scope.listarJobNotificacaoMsgItem = function (jnf) {

        var url = Util.getUrl("/jobs/listarJobNotificacaoMsgItem");
        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstMsg',
            responseModelName: 'lstMsg',
            showAjaxLoader: false,
            data: {jnf : jnf},
            //pageConfig: { pageName: 'page' /*, pageTargetName: 'pageAgendamentoDoDia' */ },
            success: function (resp) {

            }
        });
    };



}]);
