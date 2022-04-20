appModule.controller('FornecedorController', function ($scope, formHandlerService, $http, conversionService) {

    $scope.for = {};
    $scope.filtro = {};

    $scope.AbrirJanelaCidade = function () {

        $('#pesquisaCidade').modal('show');

    };
    $scope.FecharJanelaCidade = function (_municipio) {

        if (_municipio.MUN_ID != "")
            $scope.for.MUN_ID = _municipio.MUN_ID;

        if (_municipio.MUN_DESCRICAO != "")
            $scope.MUN_DESCRICAO = _municipio.MUN_DESCRICAO;

    }
    $scope.pesquisarMunicipio = function (selecionado) {

        // Se a pesquisa for vazia
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
    $scope.CarregaTela = function (for_id) {

        if (for_id) {

            formHandlerService.read($scope, {
                url: Util.getUrl("/Fornecedor/CarregaTela"),
                targetObjectName: 'for',
                responseModelName: null,
                dateFieldsNames: ['DATA_CADASTRO', 'DATA_ALTERA', 'FOR_DTMOV', 'FOR_DTCAD', 'FOR_DTNASC'],
                data: { _for_id: for_id },
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

    $scope.Salvar = function () {

        showAjaxLoader();

        $http({
            url: "/Fornecedor/Salvar",
            method: "Post",
            dataType: 'json',
            data: JSON.stringify($scope.for)
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success) {
                alert(response.message.message);
                location.href = "/Fornecedor/Index";
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
                url: "/Fornecedor/Excluir",
                method: "Post",
                dataType: 'json',
                data: JSON.stringify($scope.for)
            }).success(function (response) {

                hideAjaxLoader();

                if (response.success) {
                    alert(response.message.message);
                    location.href = "/Fornecedor/Index";
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

        var url = Util.getUrl("/Fornecedor/Pesquisar");

        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }

        var config = {
            url: url,
            targetObjectName: '_listaFornecedor',
            responseModelName: 'listaFornecedor',
            showAjaxLoader: true,
            pageConfig: { pageName: 'page' }
        };

        if ($scope.filtro) {

            config.data = angular.copy($scope.filtro);
        }
        formHandlerService.read($scope, config);
    };





});