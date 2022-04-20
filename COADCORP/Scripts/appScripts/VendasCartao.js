appModule.controller("VendasCartaoControler", function ($scope, $http, formHandlerService) {

    $scope.venda = {};
    $scope.filtro = {};

    $scope.Pesquisar = function () {

        showAjaxLoader();

        var _data = { _mesatual: $scope.filtro.mesatual, _anoatual: $scope.filtro.anoatual, _emp_id: $scope.filtro.emp_id }
        var date = "";
        var formattedDate = "";

        $http({
            url: "/VendasCartao/Pesquisar",
            method: "Post",
            dataType: 'json',
            data: _data
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success) {

                $scope.page = response.page;

                var obj = response.result.ListaVendas;

                $scope.lista = obj;
            }
            else {

                $scope.listanf = null;
                Util.alertMessage(response.message);
            }


        }).error(function () {

            hideAjaxLoader();
        })

    };
    $scope.Incluir = function () {

        showAjaxLoader();

        $http({
            url: "/VendasCartao/Incluir",
            method: "Post",
            dataType: 'json',
            data: JSON.stringify($scope.venda)
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success) {
                alert(response.message.message);
                location.href = "/VendasCartao/Index";
            }
            else
                alert(response.message.message);

        }).error(function (response) {

            hideAjaxLoader();

            alert(response.message.message);
        })

    };
    $scope.Salvar = function () {

        showAjaxLoader();

        $http({
            url: "/VendasCartao/Salvar",
            method: "Post",
            dataType: 'json',
            data: JSON.stringify($scope.venda)
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success) {
                alert(response.message.message);
                location.href = "/VendasCartao/Index";
            }
            else
                alert(response.message.message);

        }).error(function (response) {

            hideAjaxLoader();

            alert(response.message.message);
        })

    };
    $scope.Excluir = function () {

        if (confirm("Deseja excluir este registro?")) {

            $http({
                url: "/VendasCartao/Excluir",
                method: "Post",
                dataType: 'json',
                data: JSON.stringify($scope.venda)
            }).success(function (response) {

                hideAjaxLoader();

                if (response.success) {
                    alert(response.message.message);
                    location.href = "/VendasCartao/Index";
                }
                else
                    alert(response.message.message);

            }).error(function (response) {

                hideAjaxLoader();

                alert(response.message.message);
            })
        };

    };
    $scope.listar = function (pageRequest) {

        showAjaxLoader();

        var url = Util.getUrl("/VendasCartao/Pesquisar");

        if (pageRequest) {
            url += "?numpagina=" + pageRequest;
        }

        var config = {
            url: url,
            targetObjectName: 'lista',
            responseModelName: 'ListaVendas',
            pageConfig: { pageName: 'page' },
            success: function () {

                hideAjaxLoader();

                var obj = $scope.listanf;

            }
        };

        var _data = { _mesatual: $scope.filtro.mesatual, _anoatual: $scope.filtro.anoatual, _emp_id: $scope.filtro.emp_id }

        if ($scope.filtro) {

            config.data = _data;
        }

        formHandlerService.read($scope, config);
    };
    $scope.PreparaTela = function () {

        var data = new Date();

        if ($scope.filtro.mesatual == null)
            $scope.filtro.mesatual = data.getMonth() + 1;

        if ($scope.filtro.anoatual == null)
            $scope.filtro.anoatual = data.getFullYear();

        if ($scope.filtro.emp_id == null)
            $scope.filtro.emp_id = 1;

    };
    $scope.CarregaTela = function (emp_id, for_id, mesatual, anoatual) {

        showAjaxLoader();

        var _data = { _emp_id: emp_id, _for_id: for_id, _mesatual: mesatual, _anoatual : anoatual }

        $http({
            url: "/VendasCartao/CarregaTela",
            method: "Post",
            dataType: 'json',
            data: _data
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {
                $scope.venda = response.result.VendasCartao;
            }
            else
                alert(response.message);

        })

    };


});






















