///---------------------------------TransferirSuspectsController---------------------------------------------------
appModule.controller("FilaController", function ($scope, $http, formHandlerService, messageService, $timeout) {
    
    $scope.listado = false;
    $scope.listar = function (pageRequest) {


        var url = Util.getUrl("/franquia/fila/ListarPorRegiao");

        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }

        var config = {
            url: url,
            targetObjectName: 'lstFila',
            responseModelName: 'lstFila',
            showAjaxLoader: true,
            data: $scope.filtro,
            pageConfig: { pageName: 'page' },
            success: function (response) {
                $scope.listado = true;

            }
        };

        formHandlerService.read($scope, config);
    };

    $scope.removerFila = function (FLC_ID) {

        if (confirm("Deseja realmente remover?")) {

            var url = Util.getUrl("/franquia/fila/RemoverFila");

            $scope.removerDTO = { FLC_ID : FLC_ID};

            formHandlerService.submit($scope, {
                url: url,
                objectName: 'removerDTO',
                success: function (resp, status, config, message, validationMessage) {

                    $scope.removerDTO = null;
                    $scope.message = message;
                    $scope.erros = validationMessage;
                    $scope.listar();
                }
            });
        }
    }

    $scope.moverFila = function (FLC_ID, tipo) {

        var url = Util.getUrl("/franquia/fila/MoverFila");

        $scope.moverFilaDTO = { FLC_ID: FLC_ID, tipo: tipo };

        formHandlerService.submit($scope, {
            url: url,
            objectName: 'moverFilaDTO',
            success: function (resp, status, config, message, validationMessage) {
                $scope.message = message;
                $scope.erros = validationMessage;
                $scope.listar();
                $scope.moverFilaDTO = null;
            }
        });
    }
});

