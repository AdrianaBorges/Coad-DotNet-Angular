appModule.controller('LoginController', function ($scope, formHandlerService, $http, conversionService, $timeout) {

    $scope.filtro = {};
    
    $scope.validarLogin = function () {

        showAjaxLoader2();

        $http({
            method: "Post",
            dataType: "json",
            url: "/Login/ValidarLogin",
            data: {
                _login: $scope.filtro.login,
                _senha: $scope.filtro.senha
                }
        }).success(function (response) {

            if (response.success == true) {

                window.location = Util.getUrl("/Home/index");

            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);

            }

            hideAjaxLoader2();

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);

            hideAjaxLoader2();

        });

    }

    $scope.gerarSenhaTemporaria = function () {

        showAjaxLoader2();

        $http({
            method: "Post",
            dataType: "json",
            url: "/Login/GerarSenhaTemporaria",
            data: {
                _login: $scope.filtro.login,
                _email: $scope.filtro.email
            }
        }).success(function (response) {

            if (response.success == true) {

                alert(response.message.message);

                window.location = Util.getUrl("/Login/Login");

            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);

            }

            hideAjaxLoader2();

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);

            hideAjaxLoader2();

        });

    }

});