appModule.controller('ContaController', ['$scope', 'formHandlerService', '$http', 'conversionService', '$interval', 'cepService', '$timeout',

    function ($scope, formHandlerService, $http, conversionService, $interval, cepService, $timeout) {

        $scope.filtro = {};
        $scope.queryCarteira = {

            Deletar: false
        };
        var now = new Date();
        var dataatual = new Date(now.getFullYear(), now.getMonth(), now.getDate());

        $scope.initEdicao = function (_ctaid) {
            
            $scope.filtroSacador = { exibirSacadorValista : true };
            $scope.carregarEmpresas();
            if (_ctaid) {
                $scope.carregarConta(_ctaid);
            }
        }


        $scope.init = function () {

            $scope.filtro = {

                cpfExato: true
            };

            $scope.BuscarConta();
            $scope.carregarEmpresas();
            $scope.criarFiltros();
        }

        $scope.criarFiltros = function () {

            var url = Util.getUrl("/Conta/retonarDadosDeFiltro");

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'filtros',
                responseModelName: 'filters',
                showAjaxLoader: true,
                success: function (resp) {

                }
            });
        }


        $scope.carregarEmpresas = function () {

            var url = Util.getUrl("/proposta/listarEmpresas");

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'lstEmpresas',
                responseModelName: 'lstEmpresas',
                showAjaxLoader: true,
                success: function () {

                    $scope.marcarEmpresaPermitidaSacadora();
                }

            });
        }


        $scope.BuscarConta = function (pagina) {

            showAjaxLoader();

            var url = "/Conta/BuscarConta";
            if (pagina) {

                url += "?pagina=" + pagina;
            }

            $http({
                url: url,
                method: "post",
                data: $scope.filtro,
            }).success(function (response) {

                hideAjaxLoader();

                if (response.success == true) {

                    $scope.lstconta = response.result.lstconta;
                    conversionService.deepConversion($scope.lstconta);

                    $scope.page = response.page;
                }
                else {

                    $scope.message = Util.createMessage("fail", response.message);
                    $scope.lstconta = null;

                    hideAjaxLoader();
                }

            }).error(function (response) {

                $scope.message = Util.createMessage("fail", response.message);
                $scope.lstconta = null;

                hideAjaxLoader();
            });
        }


        $scope.carregarConta = function (_ctaid) {

            showAjaxLoader();

            var url = "/Conta/CarregarConta";
       
            $http({
                url: url,
                method: "post",
                data: { CTA_ID: _ctaid }
            }).success(function (response) {

                hideAjaxLoader();

                if (response.success == true) {

                    $scope.conta = response.result.conta;
                    conversionService.deepConversion($scope.conta);

                    $scope.marcarEmpresaPermitidaSacadora();
                
                }
                else {

                    $scope.message = Util.createMessage("fail", response.message);
                    $scope.conta = null;

                    hideAjaxLoader();
                }

            }).error(function (response) {

                $scope.message = Util.createMessage("fail", response.message);
                $scope.conta = null;

                hideAjaxLoader();
            });
        }

        $scope.salvarConta = function () {
            $scope.conta.CTA_ID = !$scope.conta.CTA_ID ? null : $scope.conta.CTA_ID;
            formHandlerService.submit($scope, {
                url: Util.getUrl("/conta/SalvarConta"),
                objectName: 'conta',
                showAjaxLoader: true,
                success: function (resp, status, config, message, validationMessage) {
                    $scope.message = message;
                    $scope.erros = validationMessage;
                    if (resp.success) {
                        $scope.message = Util.createMessage('success', 'Salvo com sucesso.');

                        $timeout(function () {
                            $scope.message = null;
                            window.location = Util.getUrl("/Conta/index");
                        }, 1000)
                    }
                }
            });
        }

        
        $scope.dataHoraFormatada = function (dataHora) {

            if (dataHora == null)
                dataHora = new Date();


            var data = dataHora;
            var dia = data.getDate();
            if (dia.toString().length == 1)
                dia = "0" + dia;
            var mes = data.getMonth() + 1;
            if (mes.toString().length == 1)
                mes = "0" + mes;
            var ano = data.getFullYear();
            var h = data.getHours();
            var m = data.getMinutes();
            var s = data.getSeconds();

            var hora = h + ':' + m + ':' + s;

            $scope.filtro.dataatual = dia + "/" + mes + "/" + ano + " " + hora;

            return dia + "/" + mes + "/" + ano + " " + hora;
        }


        $scope.empresaSelecionada = function () {

            if ($scope.conta) {

                $scope.conta.CTA_ALOCAR_TITULO_DA_EMP_ID = $scope.conta.EMP_ID;
                $scope.conta.EMP_ID_S_AVS = null;
            }
            $scope.marcarEmpresaPermitidaSacadora();
        };

        $scope.$watch("conta.EMP_ID_S_AVS", function (value, old) {

            if (value && value != old) {

                if ($scope.conta.EMP_ID == $scope.conta.EMP_ID_S_AVS) {

                    $scope.message = Util.createMessage('fail', 'A sacadora avalista não pode ser igual a empresa da conta');

                    $timeout(function () {
                        
                        $scope.conta.EMP_ID_S_AVS = null;
                    });
                }
            }
        });

        $scope.marcarEmpresaPermitidaSacadora = function () {

            if ($scope.lstEmpresas && $scope.conta && $scope.conta.EMP_ID) {

                angular.forEach($scope.lstEmpresas, function (value, index) {

                    value.exibirSacadorValista = (value.EMP_ID != $scope.conta.EMP_ID);
                });
            }
        };
    }]);