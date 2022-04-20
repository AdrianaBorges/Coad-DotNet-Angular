appModule.controller('HistAtendController', function ($scope, formHandlerService, $http, conversionService, cepService) {

    $scope.filtro = {};
    $scope.quebra = {};

    $scope.preparaTela = function () {

        var data = new Date();
        var now = new Date();
        $scope.filtro.dataini = new Date(now.getFullYear(), now.getMonth(), 1);
        $scope.filtro.datafim = new Date(now.getFullYear(), now.getMonth(), now.getDate());

        $scope.filtro.mes = data.getMonth() + 1;
        $scope.filtro.ano = data.getFullYear();

    }

    $scope.ImpEtiqueta = function () {

        var url = Util.getUrl("/Etiquetas/Imprimir");
            
        post(url);

    }
    
    $scope.listar = function (pageRequest) {

        showAjaxLoader();

        var url = "/Etiquetas/Pesquisar";
        $http({
            url: url,
            method: "post",
            data: { _asn_id : $scope.filtro.assinatura, _dtini: $scope.filtro.dtinicial, _dtfim: $scope.filtro.dtfinal, etiqueta:$scope.filtro.etiqueta, pagina: pageRequest }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.listahistatend = response.result.listahistatend;

                $scope.page = response.page;

            }
            else {

                $scope.listahistatend = null;

                alert(response.message.message);
            }

        }).error(function (response) {

            alert(response);

            hideAjaxLoader();
        })

    }
    $scope.buscarHistorico = function (assinatura) {

        showAjaxLoader();

        var url = "/Etiquetas/BuscarHistorico";
        $http({
            url: url,
            method: "post",
            data: { _assinatura: assinatura }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.listaagenda = response.result.listaagenda;
                $scope.listaatend = response.result.listaatend;

            }
            else {

                alert(response.message.message);
            }

        }).error(function (response) {

            alert(response);

            hideAjaxLoader();
        })

    }
    $scope.buscarHistAten = function () {

        showAjaxLoader();

        var url = "/HistoricoAtendimento/BuscarAtendimentoPorPeriodo";
        $http({
            url: url,
            method: "post",
            data: { _dtini: $scope.filtro.dataini, _dtfim: $scope.filtro.datafim }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.listaHistAten = response.result.listahistatend;
                $scope.quebra = null;

            }
            else {

                alert(response.message.message);
            }

        }).error(function (response) {

            alert(response);

            hideAjaxLoader();
        })

    }

    $scope.quebraRelatorio = function (item) {
      
        if ($scope.quebra != item)
        {
            $scope.quebra = item;
            return true;
        }
        else
            return false;

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
