appModule.controller('DpController', function ($scope, formHandlerService, $http, conversionService) {

     $scope.init = function (_cpf) {

        $scope.dpPeriodo = 4 ;
        $scope.cpf = _cpf;
        $scope.lista = true;

        $scope.buscarContraCheque(_cpf);
    }

    $scope.buscarContraCheque = function (_numcpf) {

        showAjaxLoader();

        if (_numcpf == null)
            _numcpf = $scope.cpf;

        var url = "/Dp/BuscarContraCheque";
        $http({
            url: url,
            method: "post",
            data: { _cpf: _numcpf }

        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {
                $scope.lista = true;
                $scope.lstchlst = response.result.chlst;
            }
            else {

                alert(response.message.message);

                hideAjaxLoader();
            }

        }).error(function (response) {

            alert(message);

            hideAjaxLoader();
        });

    }


    $scope.isNumber = function(n) {
        return !isNaN(parseFloat(n)) && isFinite(n);
    }

    $scope.emitirContracheque = function (item) {
        
        showAjaxLoader();

        var url = "/Dp/EmitirContracheque";
        $http({
            url: url,
            method: "post",
            data: {  _dpCpf: item.CCH_NUM_CPF
                   , _dpEmpresa: item.CODCOLIGADA
                   , _dpAno: item.ANOCOMP
                   , _dpMes: item.MESCOMP
                   , _dpPeriodo: item.NROPERIODO
                 }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {
                $scope.lista = false;
                $scope.empresa = response.result.empresa;
                $scope.lsteventos = response.result.cotracheque.Lista;
                $scope.CCH_TOT_VLR_PROVENTO = response.result.cotracheque.CCH_TOT_VLR_PROVENTO;
                $scope.CCH_TOT_VLR_DESCONTOS = response.result.cotracheque.CCH_TOT_VLR_DESCONTOS;
                
            }
            else {

                alert(response.message.message);

                hideAjaxLoader();
            }

        }).error(function (response) {

            alert(message);

            hideAjaxLoader();
        });


    }

    $scope.checaCPF = function (cpf) {
        if (($scope.filtro.dpCpf) && !$scope.isNumber($scope.filtro.dpCpf)) {
            alert("Por favor, digite apenas números!");
            $scope.filtro.dpCpf = "";
        } else if (($scope.filtro.dpCpf) && $scope.filtro.dpCpf.length < 11) {
            alert("CPF faltando números!");
            $scope.filtro.dpCpf = "";
        } else if ($scope.filtro.dpCpf && $scope.filtro.dpCpf !== cpf) {
            alert("Login inválido! Por favor, evite tentativas como estas, pois seu acesso está sendo registrado para a Direção da empresa!");
            $scope.filtro.dpCpf = "";
        }
    }
});