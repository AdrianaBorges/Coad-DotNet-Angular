

function InfoClienteController($scope, formHandlerService, $http, conversionService, $timeout) {

    $scope.initInfoCliente = function () {

        $scope.tab = 1;        
        $scope.listarHorarios();

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

    $scope.abreModalCliente = function (CLI_ID) {

        $scope.tab = 1;
        $scope.clienteModal = {};
        if (CLI_ID) {

             var url = Util.getUrl("/franquia/clientes/recuperardadosdocliente");          

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'clienteModal',
                responseModelName: 'cliente',
                data: { clienteId: CLI_ID },
                showLoader: true,
                success: function () {

                }
            });

            angular.element("#modal-cliente").modal();
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

    $scope.listarHistoricos = function () {

        var CLI_ID = ($scope.clienteModal && $scope.clienteModal.CLI_ID);
        var url = Util.getUrl("/historico/listarhistoricocliente");        
       

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
};
