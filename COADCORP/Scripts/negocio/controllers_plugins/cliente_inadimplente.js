

function ClienteInadimplenteController($scope, formHandlerService) {

    $scope.abrirModalChecarInadimplecia = function (cliId) {

        $scope.checarInadimplencia(cliId);
        angular.element("#modal-validacao-inadimplencia").modal();
    }

    $scope.checarInadimplencia = function (cliId) {

        $scope.ValidacaoInadimplencia = null;
        $scope.listado = true;
        var url = Util.getUrl("/clientes/ChecarInadimplencia");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'ValidacaoInadimplencia',
            responseModelName: 'ValidacaoInadimplencia',
            showAjaxLoader: true,
            data: { cliId: cliId },
            //pageConfig: { pageName: 'page' /*, pageTargetName: 'pageAgendamentoDoDia' */ },
            success: function (resp) {

            }
        });
    };

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
};
