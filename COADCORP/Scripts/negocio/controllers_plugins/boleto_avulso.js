function BoletoAvulsoController($scope, formHandlerService, $http, conversionService, $timeout) {

    var now = new Date();
    var dataatual = new Date(now.getFullYear(), now.getMonth(), now.getDate());

    $scope.AbrirModalBoletoAvulso = function (_numparcela, ehcurso) {

        $scope.curso = (ehcurso == true);

        showAjaxLoader2();

        var url = "/Parcelas/BuscarParcela";
        $http({
            url: url,
            method: "post",
            data: { _parcela: _numparcela }
        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {

                var mes = now.getMonth() + 1;
                var dia = now.getDate() + 1;
                var mesStr = (mes < 10) ? "0" + mes : mes;
                var diaStr = (dia < 10) ? "0" + dia : dia;

                $scope.parcelaProrrog = response.result.parcelaProrrog;
                conversionService.deepConversion($scope.parcelaProrrog);
                $scope.parcelaProrrog.PAR_VENC_BOLETOStr = null;
                $scope.parcelaProrrog.AEM_EMAIL = null;

                $timeout(function () {
                    $scope.parcelaProrrog.PAR_VENC_BOLETO = new Date(now.getFullYear(), now.getMonth(), dia);
                    $scope.buscarEmailPorBoleto($scope.parcelaProrrog.PAR_NUM_PARCELA);
                });

                angular.element("#modal-BoletoAvulso").modal();
            }
            else {

                $scope.message = response.message;
            }

        }).error(function (response) {

            $scope.message = response.message;

            hideAjaxLoader2();
        })

    }

    $scope.buscarEmailPorBoleto = function (_numparcela) {

        showAjaxLoader2();

        var url = "/Cliente/BuscarEmailPorBoleto";
        $http({
            url: url,
            method: "post",
            data: { _parcela: _numparcela }
        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {

                $scope.listaemailBoleto = response.result.listaemail;
                conversionService.deepConversion($scope.listaemailBoleto);

            }
            else {

                $scope.message = response.message;
            }

        }).error(function (response) {

            $scope.message = response.message;

            hideAjaxLoader2();
        })

    }

    $scope.enviarBoletoAvulso = function (_parcela) {


        if (_parcela.PAR_VENC_BOLETO < dataatual) {
            alert("A data de vencimento informada não é Válida ! Verifique!");
            return
        }

        if ($scope.curso == false) {
            if (_parcela.PAR_VLR_BOLETO < _parcela.PAR_VLR_PARCELA) {
                alert("Valor do boleto é menor que o valor da parcela ! Verifique!");
                return
            }
        }

        if (_parcela.AEM_EMAIL == "" || _parcela.AEM_EMAIL == null)
        {
            alert("Email não informado ! Verifique!");
            return
        }


        showAjaxLoader2();

        var url = "/CobrancaEscritural/EnviarBoletoAvulso";

        $http({
            url: url,
            method: "post",
            data: {
                idTitulo: _parcela.PAR_NUM_PARCELA,
                dtVencimento: _parcela.PAR_VENC_BOLETO,
                vlrBoleto: _parcela.PAR_VLR_BOLETO,
                email: _parcela.AEM_EMAIL
            }
        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {

                $scope.message = Util.createMessage("success", "Boleto enviado com sucesso !!");

                $timeout(function () {
                    $scope.message = null;
                    angular.element("#modal-BoletoAvulso").modal("hide");

                }, 1000);

            }
            else {

                $scope.message = response.message;

            }

        }).error(function (response) {

            $scope.message = response.message;

            hideAjaxLoader2();

        })
    }

    $scope.prorrogarDocVencimento = function (parcela) {

        if (parcela.PAR_VENC_BOLETO <= dataatual) 
        {
            alert("A data de vencimento informada não é Válida ! Verifique!");
            return
        }

        parcela.PAR_DATA_VENCTO = parcela.PAR_VENC_BOLETO;
        parcela.PAR_VENC_BOLETO = null;


        if (confirm("Confirmar o prorrogação ?")) {

            showAjaxLoader2();

            var url = "/Parcelas/Salvar";
            $http({
                url: url,
                method: "post",
                data: { _parcela: parcela, _tipo: "P" }
            }).success(function (response) {

                hideAjaxLoader2();

                if (response.success === true) {

                    $scope.message = Util.createMessage("success", "Parcela prorrogada com sucesso !!");

                    angular.element("#modal-BoletoAvulso").modal("hide");
                }
                else {
                    $scope.message = Util.createMessage("fail", response.message.message);
                }

            }).error(function (response) {

                $scope.message = Util.createMessage("fail", response);

                hideAjaxLoader2();
            })

        }

    }

    $scope.abrirFuncionalidadeBoletoAvulso = function (ipe) {

        var url = Util.getUrl("/pedido/retornarParcelaParaPagamento");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'parcelaItem',
            responseModelName: 'parcela',
            data: { IPE_ID: ipe },
            showAjaxLoader: true,
            success: function () {

                if ($scope.parcelaItem)
                    $scope.AbrirModalBoletoAvulso($scope.parcelaItem.PAR_NUM_PARCELA, true);
                else
                    $scope.message = Util.createMessage('fail', 'Nenhuma parcela em aberta foi encontrada no pedido.');
            }
        });
    }

}