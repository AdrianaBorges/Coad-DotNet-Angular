

function HistoricoClienteController($scope, formHandlerService, $http, conversionService, $timeout) {

    $scope.readCliente = function (CLI_ID) {

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
                    $scope.listarHistoricosDoCliente();
                }
            });
        }

    }

   $scope.listarHistoricosDoCliente = function () {

        var CLI_ID = ($scope.clienteModal && $scope.clienteModal.CLI_ID);
        var url = Util.getUrl("/historico/listarhistoricoclientecompleto");
       

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
            data : data,
            showAjaxLoader: true,
            success: function () {
            }
        });

    }
};
