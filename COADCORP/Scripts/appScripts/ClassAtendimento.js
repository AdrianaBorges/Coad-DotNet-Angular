appModule.controller('ClassAtendimentoController', function ($scope, formHandlerService, $http, conversionService, cepService) {

    $scope.filtro = {};
    $scope.salvar = function (item) {

        showAjaxLoader();

        var url = "/ClassificacaoAtendimento/Salvar";
        $http({
            url: url,
            method: "post",
            data: { _tipo: item }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == false) {

                alert(response.message.message);
            }

        }).error(function (response) {

            alert(response);

            hideAjaxLoader();
        })

    }
    $scope.listar = function () {

        showAjaxLoader();

        var url = "/ClassificacaoAtendimento/BuscarTipoAtendimento";
        $http({
            url: url,
            method: "post",
            data: { _classificacao: $scope.filtro.CLA_ATEND_ID }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.listaTipoAtendimento = response.result.listaTipoAtendimento;

            }
            else {

                $scope.listaAcessoTabelas = null;

                alert(response.message.message);
            }

        }).error(function (response) {

            alert(response);

            hideAjaxLoader();
        })

    }
    $scope.carregarTela = function () {

        showAjaxLoader();

        var url = "/ClassificacaoAtendimento/PreencherGrids";


        $http({
            url: url,
            method: "post",
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.listaClassificacao = response.result.listaClassificacao;

            }
            else {

                $scope.listaClassificacao = null;

                alert(response.message.message);
            }

        }).error(function (response) {

            alert(response);

            hideAjaxLoader();
        })

    }

    function dataAtualFormatadatxt(dataHora) {

        var parseDate = new Date(parseInt(dataHora.substr(6)));
        var jsDate = new Date(parseDate);

        var data = jsDate;
        var dia = data.getDate();
        if (dia.toString().length == 1)
            dia = "0" + dia;
        var mes = data.getMonth() + 1;
        if (mes.toString().length == 1)
            mes = "0" + mes;
        var ano = data.getFullYear();
        return dia + "/" + mes + "/" + ano;

    }

    function dataAtualFormatada(dataHora) {

        var parseDate = new Date(parseInt(dataHora.substr(6)));
        var jsDate = new Date(parseDate);

        return jsDate;
    }



});
