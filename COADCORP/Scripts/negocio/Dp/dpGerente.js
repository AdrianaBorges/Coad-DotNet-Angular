appModule.controller('DpController', function ($scope, formHandlerService, $http, conversionService) {

    $scope.init = function () {
        var ano = new Date().getFullYear();
        var mes = new Date().getMonth()+1;
        $scope.filtro = { dpPeriodo: "4", dpAno: ano, dpMes: mes };
    }

    $scope.isNumber = function(n) {
        return !isNaN(parseFloat(n)) && isFinite(n);
    }

    $scope.dpContracheque = function () {
        if (($scope.filtro.dpCpf) && !$scope.isNumber($scope.filtro.dpCpf)) {
            alert("Por favor, digite apenas números!");
            $scope.filtro.dpCpf = "";
        } else if (($scope.filtro.dpCpf) && $scope.filtro.dpCpf.length < 11) {
            alert("CPF faltando números!");
            $scope.filtro.dpCpf = "";
        } else if ($scope.filtro.dpCpf && $scope.filtro.dpAno && $scope.filtro.dpMes && $scope.filtro.dpPeriodo) {
            var url = Util.getUrl("/dp/contracheque/?dpEmpresa=" + $scope.filtro.dpEmpresa +
                                                    "&dpCpf=" + $scope.filtro.dpCpf +
                                                    "&dpAno=" + $scope.filtro.dpAno +
                                                    "&dpMes=" + $scope.filtro.dpMes +
                                                    "&dpPeriodo=" + $scope.filtro.dpPeriodo);
            post(url, true);
        } else {
            alert("Por favor, informe os dados necessários!");
            $scope.filtro.dpCpf = "";
        }
    }

    $scope.checaCPF = function (cpf) {
        if (($scope.filtro.dpCpf) && !$scope.isNumber($scope.filtro.dpCpf)) {
            alert("Por favor, digite apenas números!");
            $scope.filtro.dpCpf = "";
        } else if (($scope.filtro.dpCpf) && $scope.filtro.dpCpf.length < 11) {
            alert("CPF faltando números!");
            $scope.filtro.dpCpf = "";
        //} else if ($scope.filtro.dpCpf && $scope.filtro.dpCpf !== cpf) {
        //    alert("Login inválido! Por favor, evite tentativas como estas, pois seu acesso está sendo registrado para a Direção da empresa!");
        //    $scope.filtro.dpCpf = "";
        }
    }
});