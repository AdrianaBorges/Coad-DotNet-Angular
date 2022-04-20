appModule.controller('AtendimentoController', function ($scope, $sce, formHandlerService, $http, $interval, conversionService) {
      
    $scope.email = {};
    $scope.email.nome = "";
    $scope.email.assunto = "";
    $scope.email.textoemail = "";

    $scope.carregaTela = function (email) {

        $scope.buscarEmail(email);
    }
    $scope.buscarEmail = function (email) {

        showAjaxLoader();

        var url = "/Atendimento/BuscarEmail";
        $http({
            url: url,
            method: "post",
            data: { _email: email }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.listaemails = response.result.listaemails;

                for (var ind in $scope.listaemails) {
                    $scope.listaemails[ind].EAT_DATA = $scope.ConvertDateJSON($scope.listaemails[ind].EAT_DATA);
                }

                $scope.page = 0;

                if (response.page != null)
                    $scope.page = response.page;

            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
                $scope.listaemails = null;

                hideAjaxLoader();
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);
            $scope.listaemails = null;

            hideAjaxLoader();
        });

    }
    $scope.mostrarEmail = function (email) {

        showAjaxLoader();

        var url = "/Atendimento/BuscarAnexo";
        $http({
            url: url,
            method: "post",
            data: { _eat_id: email.EAT_ID }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.email.textoemail = null;

                $scope.listaanexo = response.result.listaanexo;

                $scope.email.data = email.EAT_DATA;
                $scope.email.from = email.EAT_EMAIL_FROM;
                $scope.email.nome = email.EAT_FROM_NOME;
                $scope.email.assunto = email.EAT_ASSUNTO;
                $scope.email.textoemail = $sce.trustAsHtml(email.EAT_TEXTO_EMAIL);

            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
                $scope.listaanexo = null;

                hideAjaxLoader();
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);
            $scope.listaanexo = null;

            hideAjaxLoader();
        });

    }
    $scope.ConvertDateJSON = function (jsondata) {

        var data = null;

        if (jsondata != null)
            data = new Date(parseInt(jsondata.substr(6)));

        return data;
    }

});