appModule.controller('TransportadorController', function ($scope, formHandlerService, $http, conversionService) {

    $scope.tra = {};
    $scope.filtro = {};

    $scope.AbrirJanelaCidade = function () {

        $('#pesquisaCidade').modal('show');

    };
    $scope.FecharJanelaCidade = function (_municipio) {

        if (_municipio.MUN_ID != "")
            $scope.tra.MUN_ID = _municipio.MUN_ID;

        if (_municipio.MUN_DESCRICAO != "")
            $scope.MUN_DESCRICAO = _municipio.MUN_DESCRICAO;

    }
    $scope.pesquisarMunicipio = function (selecionado) {

        $http({
            method: "Post",
            dataType: "json",
            url: "/Municipio/BuscarMunicipio",
            data: { _nomemunicipio: selecionado }
        }).success(function (response) {

            $scope.mostraconsulta = true;
            $scope.dbmunicipio = response;

        });
    }
    $scope.CarregarTela = function (tra_id) {

        if (tra_id) {

            formHandlerService.read($scope, {
                url: Util.getUrl("/Transportador/CarregarTela"),
                targetObjectName: 'tra',
                responseModelName: null,
                dateFieldsNames: ['DATA_CADASTRO', 'DATA_ALTERA', 'TRA_DTMOV', 'TRA_DTCAD', 'TRA_DTNASC'],
                data: { _tra_id: tra_id },
                success: function (response) {

                    if (response.Success == null) {
                        $scope.MUN_DESCRICAO = response.MUNICIPIO.MUN_DESCRICAO;
                    }
                    else {
                        alert(response.Message);
                    }
                }
            });
        };

    }

    $scope.Incluir = function () {

        showAjaxLoader();

        $http({
            url: "/Transportador/Incluir",
            method: "Post",
            dataType: 'json',
            data: JSON.stringify($scope.tra)
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success) {
                alert(response.message.message);
                location.href = "/Transportador/Index";
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
            url: "/Transportador/Salvar",
            method: "Post",
            dataType: 'json',
            data: JSON.stringify($scope.tra)
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success) {
                alert(response.message.message);
                location.href = "/Transportador/Index";
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
                url: "/Transportador/Excluir",
                method: "Post",
                dataType: 'json',
                data: JSON.stringify($scope.tra)
            }).success(function (response) {

                hideAjaxLoader();

                if (response.success) {
                    alert(response.message.message);
                    location.href = "/Transportador/Index";
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

        var url = Util.getUrl("/Transportador/Pesquisar");

        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }

        var config = {
            url: url,
            targetObjectName: '_listaTransportador',
            responseModelName: 'listaTransportador',
            showAjaxLoader: true,
            pageConfig: { pageName: 'page' }
        };

        if ($scope.filtro) {

            config.data = angular.copy($scope.filtro);
        }
        formHandlerService.read($scope, config);
    };

});