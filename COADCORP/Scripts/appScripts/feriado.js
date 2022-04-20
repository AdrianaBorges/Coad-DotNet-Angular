appModule.controller('FeriadoController', function ($scope, formHandlerService, $http, conversionService) {

    $scope.filtro = {};

    $("#success-alert").fadeTo(1000, 500).slideUp(500, function () {
        $("#success-alert").alert('close');
    });


    $scope.abriModalAddItem = function (item) {

        if (item != null)
            $scope.itemselect = item;
        else
            $scope.itemselect = {};

        angular.element("#Modal-Add-Item").modal();

    }

    $scope.salvarFeriado = function (item) {

        $http({
            method: 'Post',
            dataType: 'json',
            url: '/Feriado/Salvar',
            data: { _item: item }

        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.buscarFeriados($scope.filtro.fer_id);
   
            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);

            hideAjaxLoader();
        });
    }

    $scope.excluirFeriado = function (item) {

        $http({
            method: 'Post',
            dataType: 'json',
            url: '/Feriado/Excluir',
            data: { _item: item }

        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.buscarFeriados($scope.filtro.fer_id);

            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);

            hideAjaxLoader();
        });
    }


    $scope.buscarFeriados = function (ano) {

        if ($scope.filtro.fer_id == null)
            $scope.filtro.fer_id = ano;

        showAjaxLoader();

        var url = "/Feriado/BuscarFeriado";
        $http({
            url: url,
            method: "post",
            data: { _ano: ano }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.listaferiados = response.result.listaferiados;
                conversionService.deepConversion($scope.listaferiados);
            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
                $scope.listaferiados = null;

                hideAjaxLoader();
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);
            $scope.listaferiados = null;

            hideAjaxLoader();

        });

    }
});