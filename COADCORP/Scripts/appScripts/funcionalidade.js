appModule.controller('FuncionalidadeController', function ($scope, formHandlerService, $http, $interval, conversionService, $sce) {

    $scope._listaBairro = [];
    $scope.param = {};
    $scope.filtro = {};
    $scope.filtro.FCI_DESCRICAO = null;

    $scope.init = function (funcionalidade) {

        showAjaxLoader();

        var url = "/Funcionalidade/BuscarFuncionalidade";
        $http({
            url: url,
            method: "post",
            data: { _fci_id: funcionalidade }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success) {
                $scope.funcionalidade = response.result.funcionalidade;
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);

            hideAjaxLoader();
        });

    }
    $scope.listar = function (paginaReq) {

        showAjaxLoader();

        var url = "/Funcionalidade/Pesquisar";

        $http({
            url: url,
            method: "post",
            data: { _descricao: $scope.filtro.FCI_DESCRICAO, _pagina: paginaReq }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.listafuncionalidade = response.result.listafuncionalidade;

                $scope.page = response.page;
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
    $scope.trustAsHtml = function (string) {
        return $sce.trustAsHtml(string);
    }
    $scope.excluirImagem = function (opcao) {
        
        if (opcao == 0)
            $scope.funcionalidade.FCI_URL = null;
        if (opcao == 1)
            $scope.funcionalidade.FCI_IMG01 = null;
        if (opcao == 2)
            $scope.funcionalidade.FCI_IMG02 = null;
        if (opcao == 3)
            $scope.funcionalidade.FCI_IMG03 = null;
        if (opcao == 4)
            $scope.funcionalidade.FCI_IMG04 = null;

    }

    $scope.excluir = function () {

        showAjaxLoader();

        if (confirm("Deseja excluir este registro?")) {

            $http({
                url: "/Funcionalidade/Excluir",
                method: "Post",
                dataType: 'json',
                data: JSON.stringify($scope.funcionalidade)
            }).success(function (response) {

                if (response.success) {
                    location.href = "/Funcionalidade/Index";
                }
                else
                    $scope.message = Util.createMessage("fail", response.message.message);

                hideAjaxLoader();

            }).error(function (response) {

                hideAjaxLoader();

                $scope.message = Util.createMessage("fail", response.message.message);

            });
        };

        hideAjaxLoader();

    };

    $scope.listar = function (paginaReq) {

        showAjaxLoader();

        var url = "/Funcionalidade/Pesquisar";

        $http({
            url: url,
            method: "post",
            data: { _descricao: $scope.filtro.FCI_DESCRICAO, _pagina: paginaReq }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.listafuncionalidade = response.result.listafuncionalidade;

                $scope.page = response.page;
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

});