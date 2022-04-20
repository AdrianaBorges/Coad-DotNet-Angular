appModule.controller('OrigemController', function ($scope, formHandlerService, $http, $interval, conversionService, $sce) {

    $scope._listaBairro = [];
    $scope.param = {};
    $scope.filtro = {};
    $scope.filtro.ORG_DESCRICAO = null;
    $scope.ORIGEM_ACESSO = {};
    $scope.ORIGEM_ACESSO.OAC_ID = null;

    $scope.init = function (_origem) {

        showAjaxLoader();

        var url = "/Origem/BuscarOrigem";
        $http({
            url: url,
            method: "post",
            data: { org_id: _origem }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success) {
                $scope.ORIGEM_ACESSO = response.result.origem;
                if ($scope.ORIGEM_ACESSO != null)
                    $scope.ORIGEM_ACESSO.DATA_ALTERA = $scope.dataAtualFormatada($scope.ORIGEM_ACESSO.DATA_ALTERA);
                $scope.listarFuncionalidades();
                $scope.listarFuncionalidadesSelc();

            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);

            hideAjaxLoader();
        });

    }
    $scope.listar = function (paginaReq) {

        showAjaxLoader();

        var url = "/Origem/Pesquisar";

        $http({
            url: url,
            method: "post",
            data: { _descricao: $scope.param.ORG_DESCRICAO, _pagina: paginaReq }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.listaorigem = response.result.listaorigem;

                $scope.page = response.page;
            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
                $scope.listaorigem = null;

                hideAjaxLoader();
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);
            $scope.listaorigem = null;

            hideAjaxLoader();

        });

    }
    $scope.listarFuncionalidades = function () {

        showAjaxLoader();

        var _oac_id = null;

        //if ($scope.ORIGEM_ACESSO != null)
        //    _oac_id =  $scope.ORIGEM_ACESSO.OAC_ID;

        var url = "/Funcionalidade/ListarFuncionalidades";

        $http({
            url: url,
            method: "post",
            data: { _origem: _oac_id }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.listafuncionalidade = response.result.listafuncionalidade;
            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
                $scope.listafuncionalidade = null;

                hideAjaxLoader();
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);
            $scope.listafuncionalidade = null;

            hideAjaxLoader();

        });

    }
    $scope.listarFuncionalidadesSelc = function () {

        showAjaxLoader();

        var _oac_id = null;

        if ($scope.ORIGEM_ACESSO != null)
            _oac_id = $scope.ORIGEM_ACESSO.OAC_ID;

        var url = "/Origem/ListarFuncionalidadesSelec";

        $http({
            url: url,
            method: "post",
            data: { _origem: _oac_id }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                if ($scope.result == null) {
                    $scope.listfuncselecionadas = [];
                }

                $scope.listfuncselecionadas = response.result.listafuncionalidade;

                angular.forEach($scope.listfuncselecionadas, function (obj, index) {
                    obj.OFU_DATA_ALTERA = $scope.dataAtualFormatada(obj.OFU_DATA_ALTERA);
                    if (obj.FUNCIONALIDADE!= null)
                        obj.FUNCIONALIDADE.FCI_DATA_ALTERA = $scope.dataAtualFormatada(obj.FUNCIONALIDADE.FCI_DATA_ALTERA);
                });
            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
                $scope.listfuncselecionadas = null;

                hideAjaxLoader();
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);
            $scope.listfuncselecionadas = null;

            hideAjaxLoader();

        });

    }
    $scope.addItem = function (item) {

        if ($scope.ORIGEM_ACESSO == null) {
            $scope.ORIGEM_ACESSO = {};
            $scope.ORIGEM_ACESSO.OAC_ID = 0;
        }

        if ($scope.listfuncselecionadas == null) {
            $scope.listfuncselecionadas = [];
        }

        var jaexiste = false;

        angular.forEach($scope.listfuncselecionadas, function (obj, index) {
     
            if (item.FCI_ID == obj.FCI_ID) {
                $scope.message = Util.createMessage("fail", "Item já selecionado!");
                jaexiste = true;
            }
        });

        if (jaexiste)
            return false;


        var novo = {};
        novo.OAC_ID = $scope.ORIGEM_ACESSO.OAC_ID;
        novo.FCI_ID = item.FCI_ID;
        novo.OFU_DESCRICAO = item.FCI_DESCRICAO;
	    novo.OFU_ATIVO = true;
	    novo.OFU_ORDEM = $scope.listfuncselecionadas.length;
	    //novo.OFU_DATA_ALTERA = Date();
	    //novo.USU_LOGIN = null;

        $scope.listfuncselecionadas.push(novo);

    }
    $scope.removeItem = function (index) {

        $scope.listfuncselecionadas.splice(index, 1);
        $scope.listarFuncionalidades();

    }
    $scope.trustAsHtml = function (string) {
        return $sce.trustAsHtml(string);
    }
    $scope.ordenarUp = function (item, indice) {

        if (indice > 0) {
            if ($scope.listfuncselecionadas == null) {
                $scope.listfuncselecionadas = [];
            }

            var atual = $scope.listfuncselecionadas[indice - 1];

            item.OFU_ORDEM = indice - 1;
            atual.OFU_ORDEM = indice;

            $scope.listfuncselecionadas[indice - 1] = item;
            $scope.listfuncselecionadas[indice] = atual;
        }

    }
    $scope.ordenarDown = function (item, indice) {

        if ($scope.listfuncselecionadas.length > (indice + 1)) {
            if ($scope.listfuncselecionadas == null) {
                $scope.listfuncselecionadas = [];
            }

            var atual = $scope.listfuncselecionadas[indice + 1];

            item.OFU_ORDEM = indice + 1;
            atual.OFU_ORDEM = indice;

            $scope.listfuncselecionadas[indice + 1] = item;
            $scope.listfuncselecionadas[indice] = atual;
        }

    }
    

    $scope.SalvarOrigem = function () {

        showAjaxLoader();

        var url = "/Origem/Salvar";

        $http({
            url: url,
            method: "Post",
            data: { _origem: $scope.ORIGEM_ACESSO, _origemfunc: $scope.listfuncselecionadas }
        }).success(function (response) {

            hideAjaxLoader();

            $scope.message = response.message;
            $scope.erros = response.validationMessage;

            if (response.success == true) {
                url = Util.getUrl("/Origem/Index");
                post(url);
            }


        }).error(function (response) {

            alert(response);

            hideAjaxLoader();
        })

    }
    $scope.dataAtualFormatada = function (dataHora) {

        if (dataHora == null)
            return null;

        var parseDate = new Date(parseInt(dataHora.substr(6)));
        var jsDate = new Date(parseDate);

        return jsDate;
    }
});