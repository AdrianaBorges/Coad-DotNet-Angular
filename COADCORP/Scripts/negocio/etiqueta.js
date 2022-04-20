appModule.controller('EtiquetaController', function ($scope, formHandlerService, $http, conversionService, cepService) {

    $scope.filtro = {};
    $scope.param = {};
    $scope.export = {};
    

    $scope.preparaTela = function () {
    
        var now = new Date();
        $scope.filtro.dtinicial = new Date(now.getFullYear(), now.getMonth(), 1);
        $scope.filtro.dtfinal = new Date(now.getFullYear(), now.getMonth(), now.getDate());
    }

    $scope.Listar = function () {

        showAjaxLoader2();

        $scope.param.patharquivo = "";

        $http({
            method: 'Post',
            dataType: 'json',
            url: '/Etiquetas/ListarArquivos',
            data: { _patharquivo: $scope.param.patharquivo }
        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {

                $scope.listaarquivos = response.result.listaarquivos;
            }
            else
                $scope.message = response.message;

        }).error(function (response) {

            $scope.message = response.message;

            hideAjaxLoader2();
        })


    }

    $scope.ImprimirEtiquetaSolicitacao = function () {

        showAjaxLoader2();

        var url = "/Etiquetas/ImprimirEtiquetaSolicitacao";
        $http({
            url: url,
            method: "post",
            data: { _impresso: $scope.impresso }

        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {
                
                $scope.etiquetaavulsa = null;
                $scope.export.lnkPath = response.result.retorno;

            }
            else {

                $scope.message = response.message;

            }

        }).error(function (response) {

            alert(response);

            hideAjaxLoader2();
        })

    }


    $scope.ImpEtiquetaAvulsa = function () {

        showAjaxLoader2();

        var url = "/Etiquetas/ImprimirAvulsa";
        $http({
            url: url,
            method: "post",
            data: { _etiquetas: $scope.etiquetaavulsa }
        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {

                $scope.etiquetaavulsa = null;
                $scope.export.lnkPath = response.result.retorno;

            }
            else {

                $scope.message = response.message;
                
            }

        }).error(function (response) {

            alert(response);

            hideAjaxLoader2();
        })

    }
    $scope.add = function () {

        if ($scope.etiquetaavulsa == null)
            $scope.etiquetaavulsa = [];

        var count = $scope.etiquetaavulsa.length;

        if (count > 0) {
            var obj = $scope.etiquetaavulsa[count - 1];
            if (obj.assinatura != "" && obj.assinatura != null) {
                $scope.etiquetaavulsa.push({});
            }
        }
        else { $scope.etiquetaavulsa.push({}); }

    };
    $scope.remove = function (index) {

        var count = $scope.etiquetaavulsa.length;

        if (count > 0) {
            $scope.etiquetaavulsa.splice(index, 1)
        }

    };

    $scope.mostrarCliente = function (index) {
           
        showAjaxLoader2();

        var _assinatura = $scope.etiquetaavulsa[index].assinatura;

        var url = "/Etiquetas/BuscarCliente";
        $http({
            url: url,
            method: "post",
            data: { _asn_id: _assinatura }
        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {

                $scope.etiquetaavulsa[index].nome_cliente = response.result.cliente.CLI_NOME;

                $scope.page = response.page;

            }
            else {

                $scope.etiquetaavulsa[index].nome_cliente = "";

                $scope.message = response.message;
            }

        });

        hideAjaxLoader2();
        
    };
    $scope.mostrarProduto = function (index) {

        showAjaxLoader2();

        var _produto = $scope.etiquetaavulsa[index].produto;

        var url = "/Etiquetas/BuscarProduto";
        $http({
            url: url,
            method: "post",
            data: { _pro_id: _produto }
        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {

                $scope.etiquetaavulsa[index].nome_produto = response.result.produto.PRO_SIGLA;

                $scope.page = response.page;

            }
            else {

                $scope.etiquetaavulsa[index].nome_produto = "";

                $scope.message = response.message;
            }

        });

        hideAjaxLoader2();
 
    };

});
