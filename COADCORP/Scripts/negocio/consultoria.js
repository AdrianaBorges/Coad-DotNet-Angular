appModule.controller('ConsultoriaController', function ($scope, formHandlerService, $http, conversionService, $timeout) {
    $scope.param = {};
    $scope.filtro = {};
    $scope.message = {};
    var now = new Date();
    var dataatual = new Date(now.getFullYear(), now.getMonth(), now.getDate());

    $scope.listar = function (paginaReq) {

        showAjaxLoader();

        var url = "/Consultoria/Pesquisar";

        $http({
            url: url,
            method: "post",
            data: {
                _id: $scope.param.id,
                _codigo: $scope.param.codigo,
                _status: $scope.param.status,
                _perini: $scope.param.perini,
                _perfim: $scope.param.perfim,
                _uf: $scope.param.uf,
                _pagina: paginaReq
            }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {
                $scope.listaconsulta = response.result.listaconsulta;
                $scope.page = response.page;
            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
                $scope.listaconsulta = null;

                hideAjaxLoader();
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);
            $scope.listaconsulta = null;

            hideAjaxLoader();

        });

    }

    $scope.fillform = function (identificador) {
        showAjaxLoader();
        var url = "/Consultoria/BuscarConsulta";
        $http({
            url: url,
            method: "post",
            data: {
                _id: identificador
            }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {
                $scope.consulta = response.result.consulta;
                $scope.consultor = response.result.consultor;
                $scope.dataCadastro = response.result.dataCadastro;
                $scope.dataResposta = response.result.dataResposta;
                $scope.dataUltimoAcesso = response.result.dataUltimoAcesso;
                $scope.descricaocolecionador = response.result.descricaocolecionador;
                $scope.exibirBtnSalvar = response.result.exibirBtnSalvar;
                $scope.consultorUltimoAcesso = response.result.consultorUltimoAcesso;
            }
            else {
                $scope.message = Util.createMessage("fail", response.message.message);
                $scope.listaconsulta = null;
                hideAjaxLoader();
            }

        }).error(function (response) {
            $scope.message = Util.createMessage("fail", response);
            $scope.listaconsulta = null;
            hideAjaxLoader();
        });
    }

    $scope.EnviarResposta = function () {
        showAjaxLoader();

        var url = "/Consultoria/SalvarEnviarResposta";
        $http({
            url: url,
            method: "post",
            data: {
                _id: $scope.consulta.id,
                _resposta: $scope.consulta.resposta_supervisor
            }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {
                $scope.message = Util.createMessage("success", "Resposta salva e enviada com sucesso!");
                $timeout(function () {
                    $scope.message = null;
                    window.location = Util.getUrl("/Consultoria/consultoriaEmail");
                }, 1000);
            }
            else {
                $scope.message = Util.createMessage("fail", response.message.message);
                hideAjaxLoader();
            }

        }).error(function (response) {
            $scope.message = Util.createMessage("fail", response);
            hideAjaxLoader();
        });
    }


});