function BaixaManualController($scope, formHandlerService, $http, conversionService, $timeout) {

    var now = new Date();
    var dataatual = new Date(now.getFullYear(), now.getMonth(), now.getDate());

    $scope.abriModalBaixaManual = function () {

        $scope.parcelaProrrog = {};

        angular.element("#modal-baixa-manual").modal();
    }
    $scope.buscarParcelaNossoNum = function (_nossonumero) {


        if (_nossonumero.length == 0)
            $scope.parcelaProrrog = {};

        if (_nossonumero.length < 7)
            return;

        showAjaxLoader2();

        var url = "/Parcelas/BuscarParcelaNossoNum";
        $http({
            url: url,
            method: "post",
            data: { _par_nosso_numero: _nossonumero }
        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success) {

                var mes = now.getMonth() + 1;
                var dia = now.getDate();
                var mesStr = (mes < 10) ? "0" + mes : mes;
                var diaStr = (dia < 10) ? "0" + dia : dia;

                $scope.parcelaProrrog = response.result.parcelaProrrog;
                conversionService.deepConversion($scope.parcelaProrrog);

                if ($scope.parcelaProrrog.PAR_DATA_PAGTO != null) {
                    $scope.message = Util.createMessage("fail", "Titulo ja baixado !!");
                    $scope.parcelaProrrog = {};
                    return;
                }

                $scope.parcelaProrrog.PAR_VLR_PAGO = $scope.parcelaProrrog.PAR_VLR_PARCELA +
                                                     $scope.parcelaProrrog.PAR_VLR_DESP_ADM +
                                                     $scope.parcelaProrrog.PAR_VLR_JUROS;

                $scope.parcelaProrrog.PAR_DATA_BAIXAr = null;
                $scope.parcelaProrrog.PAR_DATA_PAGTOr = null;

                $timeout(function () {
                    $scope.parcelaProrrog.PAR_DATA_BAIXA = new Date(now.getFullYear(), now.getMonth(), dia);
                    $scope.parcelaProrrog.PAR_DATA_PAGTO = new Date(now.getFullYear(), now.getMonth(), dia);
                });

            }
            else {

                if (response.message == null)
                    $scope.message = Util.createMessage("fail", response);
            }

        }).error(function (response) {

            $scope.message = response.message;

            hideAjaxLoader2();
        })

    }
    $scope.buscarParcelaBaixa = function (_numparcela, _baixa) {

        if (_numparcela.length == 0)
            $scope.parcelaProrrog = {};

        if (_numparcela.length < 7)
            return;


        showAjaxLoader2();

        var url = "/Parcelas/BuscarParcela";
        $http({
            url: url,
            method: "post",
            data: {
                _parcela: _numparcela
              , _baixamanual: _baixa
            }
        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {

                var mes = now.getMonth() + 1;
                var dia = now.getDate();
                var mesStr = (mes < 10) ? "0" + mes : mes;
                var diaStr = (dia < 10) ? "0" + dia : dia;

                $scope.parcelaProrrog = response.result.parcelaProrrog;
                conversionService.deepConversion($scope.parcelaProrrog);
                
                if ($scope.parcelaProrrog.PAR_DATA_PAGTO != null) {
                    $scope.message = Util.createMessage("fail", "Titulo ja baixado !!");
                    $scope.parcelaProrrog = {};
                    return;
                }

                $scope.parcelaProrrog.PAR_VLR_PAGO = $scope.parcelaProrrog.PAR_VLR_PARCELA +
                                                     $scope.parcelaProrrog.PAR_VLR_DESP_ADM +
                                                     $scope.parcelaProrrog.PAR_VLR_JUROS;
                
                $scope.parcelaProrrog.PAR_DATA_BAIXAr = null;
                $scope.parcelaProrrog.PAR_DATA_PAGTOr = null;
                
                $timeout(function () {
                    $scope.parcelaProrrog.PAR_DATA_BAIXA = new Date(now.getFullYear(), now.getMonth(), dia);
                    $scope.parcelaProrrog.PAR_DATA_PAGTO = new Date(now.getFullYear(), now.getMonth(), dia);
                });

            }
            else {

                if (response.message == null)
                    $scope.message = Util.createMessage("fail", response);
            }

        }).error(function (response) {

            $scope.message = response.message;

            hideAjaxLoader2();
        })

    }
    $scope.baixaManual = function (_parcelaProrrog, _baixa) {

        if (_parcelaProrrog == null)
            return;

        if (_parcelaProrrog.PAR_NUM_PARCELA.length < 7)
            return;

        showAjaxLoader2();

        var url = "/Parcelas/BaixaManual";
        $http({
            url: url,
            method: "post",
            data: {
                 _parcela: _parcelaProrrog
               , _baixamanual: _baixa
            }
        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {

                $scope.message = response.message;
                $scope.parcelaProrrog = {};
            }
            else {
             
                if (response.message == null)
                    $scope.message = Util.createMessage("fail", response);
                else
                    $scope.message = response.message;

                $scope.parcelaProrrog = {};
            }

        }).error(function (response) {

            $scope.message = response.message;

            hideAjaxLoader2();
        })

    }
}