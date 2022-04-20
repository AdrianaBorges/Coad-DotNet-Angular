appModule.controller('MunicipioController', function ($scope, formHandlerService, $http, conversionService, cepService) {

    $scope.filtro = {};
    $scope.param = {};
    $scope.export = {};


    $scope.preparaTela = function () {

        var now = new Date();
        $scope.filtro.dtinicial = new Date(now.getFullYear(), now.getMonth(), 1);
        $scope.filtro.dtfinal = new Date(now.getFullYear(), now.getMonth(), now.getDate());
    }

    $scope.buscarMunicipio = function () {


        if ($scope.param) {
            if ($scope.param.mun_descricao != null && $scope.param.mun_descricao != "" && $scope.param.mun_descricao.length < 3) {
              //  $scope.message = Util.createMessage("fail", "Informe um nome com ao menos 3 caracteres.");
                return;
            }
        }

        showAjaxLoader2();

        var url = "/Municipio/BuscarMunicipio";
        $http({
            url: url,
            method: "post",
            data: { _nomemunicipio: $scope.param.mun_descricao }
        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {

                $scope.lstmunicipio = response.result.dbMunicipio;

            }
            else {

                $scope.message = response.message;

            }

        }).error(function (response) {

            alert(response);

            hideAjaxLoader2();
        })

    }
    $scope.atualizarMunicipio = function (_mun) {

        showAjaxLoader2();

        var url = "/Municipio/Salvar";
        $http({
            url: url,
            method: "post",
            data: { _municipio: _mun }
        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {

                $scope.message = response.message;

                $timeout(function () {
                    $scope.message = null;
                }, 1000);
               
            }
            else {

                $scope.message = response.message;

            }

        }).error(function (response) {

            alert(response);

            hideAjaxLoader2();
        })

    }


});
