
appModule.controller("SpedControler", function ($scope, $http) {

    $scope.filtro = {};

    $scope.GerarSped = function () {

        $scope.lnkVisible = false;

        showAjaxLoader();

        
        var _data = { mesatual: $scope.filtro.mesatual, anoatual: $scope.filtro.anoatual, emp_id: $scope.filtro.emp_id, versao: $scope.filtro.versao }

        $http({
            url: "/ObrigacoesFiscais/GerarSped",
            method: "Post",
            dataType: 'json',
            data: _data
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == false) {
                $scope.lnkVisible = false;
                alert(response.message.message);
            }
            else {
                $scope.lnkVisible = true;
                $scope.lnkPath = response.result.lnkPath;
                $scope.lnkLink = response.result.lnkLink;
                alert(response.message.message);
            }

        }).error(function () {
          
            hideAjaxLoader();
        })

    }
    $scope.CarregaTela = function (emp_id) {

        $scope.BuscarConfigSped(emp_id);
        $scope.PreparaTela();

    }

    $scope.AtualizarConfigSped = function () {

        $scope.CFG1 = angular.copy($scope.CFG);

        showAjaxLoader();

        $scope.CFG1.REG1010_IND_EXP = $scope.CFG.REG1010_IND_EXP == true ? "S" : "N";
        $scope.CFG1.REG1010_IND_CCRF = $scope.CFG.REG1010_IND_CCRF == true ? "S" : "N";
        $scope.CFG1.REG1010_IND_COMB = $scope.CFG.REG1010_IND_COMB == true ? "S" : "N";
        $scope.CFG1.REG1010_IND_USINA = $scope.CFG.REG1010_IND_USINA == true ? "S" : "N";
        $scope.CFG1.REG1010_IND_VA = $scope.CFG.REG1010_IND_VA == true ? "S" : "N";
        $scope.CFG1.REG1010_IND_EE = $scope.CFG.REG1010_IND_EE == true ? "S" : "N";
        $scope.CFG1.REG1010_IND_CART = $scope.CFG.REG1010_IND_CART == true ? "S" : "N";
        $scope.CFG1.REG1010_IND_FORM = $scope.CFG.REG1010_IND_FORM == true ? "S" : "N";
        $scope.CFG1.REG1010_IND_AER = $scope.CFG.REG1010_IND_AER == true ? "S" : "N";

        $http({
            url: "/ObrigacoesFiscais/AtualizarConfigSped",
            method: "Post",
            dataType: 'json',
            data: JSON.stringify($scope.CFG1)
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == false) {
                alert(response.message.message);
            }

        }).error(function () {

            hideAjaxLoader();
        })

    }
    $scope.BuscarConfigSped = function (emp_id) {
    
        showAjaxLoader();

        $scope.CFG = {};

        $http({
            url: "/ObrigacoesFiscais/BuscarConfigSped",
            method: "Post",
            dataType: 'json',
            data: {_emp_id : emp_id}
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == null) {
                
                $scope.CFG.EMP_ID = response.EMP_ID;
                $scope.CFG.REG1010_IND_EXP = response.REG1010_IND_EXP == 'S';
                $scope.CFG.REG1010_IND_CCRF = response.REG1010_IND_CCRF == 'S';
                $scope.CFG.REG1010_IND_COMB = response.REG1010_IND_COMB == 'S';
                $scope.CFG.REG1010_IND_USINA = response.REG1010_IND_USINA == 'S';
                $scope.CFG.REG1010_IND_VA = response.REG1010_IND_VA == 'S';
                $scope.CFG.REG1010_IND_EE = response.REG1010_IND_EE == 'S';
                $scope.CFG.REG1010_IND_CART = response.REG1010_IND_CART == 'S';
                $scope.CFG.REG1010_IND_FORM = response.REG1010_IND_FORM == 'S';
                $scope.CFG.REG1010_IND_AER = response.REG1010_IND_AER == 'S';
     
            }
            else {
                alert(response.message.message);
            }

        }).error(function () {

            hideAjaxLoader();
        })

    }
    $scope.PreparaTela = function () {

        var data = new Date();

        if ($scope.filtro.mesatual == null)
            $scope.filtro.mesatual = data.getMonth() + 1;

        if ($scope.filtro.anoatual == null)
            $scope.filtro.anoatual = data.getFullYear();

        if ($scope.filtro.emp_id == null)
            $scope.filtro.emp_id = 1;

        if ($scope.filtro.versao == null)
            $scope.filtro.versao = 8;

    };
});