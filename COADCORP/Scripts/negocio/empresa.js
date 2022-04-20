appModule.controller('EmpresaController', ['$scope', 'formHandlerService', '$http', 'conversionService', '$interval', 'cepService', '$timeout',

    function ($scope, formHandlerService, $http, conversionService, $interval, cepService, $timeout) {

        $scope.filtro = {};
        $scope.queryCarteira = {

            Deletar: false
        };

        var now = new Date();
        var dataatual = new Date(now.getFullYear(), now.getMonth(), now.getDate());

        $scope.init = function () {

            $scope.filtro = {

                cpfExato: true
            };

            $scope.carregarEmpresas();
            $scope.criarFiltros();
        }

        $scope.criarFiltros = function () {

            var url = Util.getUrl("/Empresa/retonarDadosDeFiltro");

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'filtros',
                responseModelName: 'filters',
                showAjaxLoader: true,
                success: function (resp) {

                }
            });
        }

        $scope.carregarEmpresa = function (_empid) {

            showAjaxLoader();

            var url = "/Empresa/CarregarEmpresa";

            $http({
                url: url,
                method: "post",
                data: { EMP_ID: _empid }
            }).success(function (response) {

                hideAjaxLoader();

                if (response.success == true) {

                    $scope.empresa = response.result.empresa;
                    conversionService.deepConversion($scope.empresa);

                }
                else {

                    $scope.message = Util.createMessage("fail", response.message);
                    $scope.empresa = null;

                    hideAjaxLoader();
                }

            }).error(function (response) {

                $scope.message = Util.createMessage("fail", response.message);
                $scope.empresa = null;

                hideAjaxLoader();
            });
        }

        $scope.salvarEmpresa = function () {
                formHandlerService.submit($scope, {
                url: Util.getUrl("/empresa/SalvarEmpresa"),
                objectName: 'empresa',
                showAjaxLoader: true,
                success: function (resp, status, config, message, validationMessage) {
                    $scope.message = message;
                    $scope.erros = validationMessage;
                    if (resp.success) {
                        alert("Salvo com sucesso.");
                        window.location = Util.getUrl("/Empresa/index");
                    }
                }
            });
        }

        $scope.BuscarEmpresa = function (pagina) {

            showAjaxLoader();

            var url = "/empresa/BuscarEmpresa";
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

                    $scope.lstempresa = response.result.lstempresa;
                    conversionService.deepConversion($scope.lstempresa);

                    $scope.page = response.page;
                }
                else {

                    $scope.message = Util.createMessage("fail", response.message);
                    $scope.lstempresa = null;

                    hideAjaxLoader();
                }

            }).error(function (response) {

                $scope.message = Util.createMessage("fail", response.message);
                $scope.lstempresa = null;

                hideAjaxLoader();
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

    }]);