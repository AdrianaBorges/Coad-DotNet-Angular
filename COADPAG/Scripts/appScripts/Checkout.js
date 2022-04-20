appModule.controller('PortalController', function ($scope, formHandlerService, $http, $interval, conversionService, $sce, mask ) {

    $scope.usuario = {};
    $scope.usuario.login = "";
    $scope.usuario.senha = "";
    $scope.MES = {};
    $scope.TDC_ID = {};
    $scope.filtro = {};
    $scope.filtro.TIPO = 0;
    $scope.export = {};
    $scope.cadastro = {};
    $scope.cartao = {};

    $scope.trustAsHtml = function (string) {
        return $sce.trustAsHtml(string);
    };


    $scope.initDadosConclusao = function (_nome,_boleto) {

        $scope.boleto = _boleto;
        $scope.comprador = {};
        $scope.comprador.nome = _nome;

    }

    $scope.mask = function (valor) {
        valor = mask('mask_dinheiro')(valor);
        return valor;
    }


    $scope.ImpEtiqueta = function () {

        var url = Util.getUrl("/AcessoTabelas/Imprimir");

        post(url);

    }

    $scope.carregarTela = function () {

        $scope.filtro.mes = data.getMonth() + 1;
        $scope.filtro.ano = data.getFullYear();

    }
    $scope.realizarPagamento = function () {

        showAjaxLoader();

        var url = "/Checkout/RealizarPagamento";
        $http({
            url: url,
            method: "post",
            data: { _cartao: $scope.cartao, _comprador: $scope.comprador }
        }).success(function (response) {
           
            if (response.success == true) {

                var urlboleto = response.result.urlboleto;

                if (urlboleto == "" || urlboleto == null) {
                    url = Util.getUrl("/Conclusao/Index?_nome=" + $scope.comprador.nome + "&_formapgto=" + $scope.cartao.formapgto);
                    post(url);
                }
                else
                    window.location.href = urlboleto;

            }
            else {

                $scope.erro = response.message.message;

                angular.element("#Modal-Erro").modal();

                hideAjaxLoader();

                $scope.button = "reset";
            }

            hideAjaxLoader();


        }).error(function (response) {

            $scope.erro = response;

            angular.element("#Modal-Erro").modal();

            hideAjaxLoader();

            $scope.button = "reset";

        });

    }

    $scope.mostraLoader = function () {

        showAjaxLoader();

    }

    $scope.buscarPreco = function (item) {

        $scope.comprador.valor = item.CMP_VLR_VENDA;
        $scope.comprador.cmp_id = item.CMP_ID;
    }
    $scope.initDadosPagamento = function (ipeid, ipehash) {


        if (ipeid == null || ipehash == null || ipeid == "" || ipehash == "") {

            alert("Pedido não informado ou inválido!!");

            url = Util.getUrl("http://www.coad.com.br/");
            post(url);

            return
        }


        showAjaxLoader();


        var url = "/Checkout/DadosPagamentoInit";

        $http({
            url: url,
            method: "post",
            data: { _ipe_id: ipeid, _ipe_hash: ipehash }
        }).success(function (response) {

            if (response.success == true) {

                $scope.comprador = response.result.comprador;
                $scope.cartao = response.result.cartao;

            }
            else {

                alert(response.message.message);

                url = Util.getUrl("http://www.coad.com.br/");
                post(url);

            }

            hideAjaxLoader();

        }).error(function (response) {

            alert(response);

            url = Util.getUrl("http://www.coad.com.br/");
            post(url);

            hideAjaxLoader();
        })


    }
    $scope.trustAsHtml = function (string) {
        return $sce.trustAsHtml(string);
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
    $scope.ConvertDateJSON = function (jsondata) {

        var data = null;

        if (jsondata != null)
            data = new Date(parseInt(jsondata.substr(6)));

        return data;
    }

     //$scope.count = 0;

     //$scope.increment = function () {

     //    showAjaxLoader();

     //     $scope.count++;
     //     if ($scope.count < 20) {
     //         $timeout($scope.increment, 1000);
     //     }
     //  //   hideAjaxLoader();


     //};
     
      
     //$scope.increment();


});
