appModule.controller('RepresentanteHomeController', function ($scope, formHandlerService, $http, conversionService, $interval, $timeout) {
   
    $scope.filtro = {data : new Date()};
    $scope.urlBase = "representante";
    
    $scope.datasourceAtendimentos =  {}; 
    $scope.datasourceAtendimentosPie = {};
    $scope.datasourceAtendimentosPorRegiao = {};
    $scope.datasourceAtendimentosPiePorRegiao = {};
    $scope.relatorioFaturamentoFranquia = {};


    $scope.InitWithFranquiador = function () {
        
        //$scope.listarRepresentantes();
        $scope.urlBase = "franquiador";
        $scope.isFranquiador = true;
        $scope.loadAll();
        $scope.CarregarGraficoAtendimentosRealizadosNoMes();
        $scope.CarregarGraficoAtendimentosRealizadosNoMesPorRegiao();
        $scope.RelatorioFaturamentoFranquia();
    }

    $scope.InitWithGerente = function () {


        $scope.isFranquiador = false;
        $scope.listarRepresentantes();
        $scope.urlBase = "franquiado";
        $scope.loadAll();
    }    

    $scope.setConfig = function (value) {

        value.chart.yaxisname = "Quantidade";
        value.chart.showlegend = '0';
        value.chart.showpercentvalues = '0';
        value.chart.formatNumber = '0';
        value.chart.formatNumberScale = '0';
        value.chart.paletteColors = "#0075c2";
        value.chart.valueFontColor = "#000000";

    }
    $scope.listarRepresentantesDaRegiaoFranquiador = function (pagina) {

        var url = Util.getUrl("/franquia/franquiador/representantesDaRegiao");

        //var filtro = { RG_ID: $scope.RG_ID };
        $scope.filtro.RG_ID = $scope.RG_ID;

        if (pagina) {

            url += "?pagina=" + pagina;
        }

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'representantes',
            responseModelName: 'representantes',
            data: $scope.filtro,
            showAjaxLoader: true,
            pageConfig: { pageName: 'page' /*, pageTargetName: 'paginaPrioridades' */ },
            success: function (resp) {

            }
        });
    }


    $scope.escolherRepresentante = function (item) {

        $scope.representante = angular.copy(item);
        $scope.filtro.REP_ID = $scope.representante.REP_ID;
        $scope.loadAll();
    }

    $scope.loadAll = function (REP_ID) {

        
        $scope.listarDadosHome();
        $scope.listarAgendamentoDoDia();
        $scope.listarAgendamentoAtrasado();
        $scope.listarAgendamentoVindouro();
        $scope.lstQtdNotificacoes();
        $scope.listarQuantidadeTipoCliente();
    }
    

    $scope.$watch("filtro.data", function (value, old) {

        if (value) {

            if ($scope.isFranquiador === true) {
                $scope.CarregarGraficoAtendimentosRealizadosNoMes();
                $scope.CarregarGraficoAtendimentosRealizadosNoMesPorRegiao();
                $scope.RelatorioFaturamentoFranquia();
            }
            $scope.loadAll();
        }
    }, true);

    $scope.initHomeRepresentante = function () {
                
        $scope.loadAll();
    }
    
    $scope.listarRepresentantes = function (pagina) {

        var url = Util.getUrl("/franquiado/representantes");

        if (pagina) {

            url += "?pagina=" + pagina;
        }

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'representantes',
            responseModelName: 'representantes',
            data: $scope.filtro,
            showAjaxLoader: true,
            pageConfig: { pageName: 'page' /*, pageTargetName: 'paginaPrioridades' */},

            ccess: function (resp) {

            }
        });
    }

    $scope.listarDadosHome = function (pagina) {

       // var urlBaseOverride = ($scope.isFranquiador) ? 'franquiador' : $scope.urlBase;
        var url = Util.getUrl("/{0}/ListarDadosHome", [$scope.urlBase]);

        if (pagina) {

            url += "?pagina=" + pagina;
        }
        
        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstPrioridades',
            responseModelName: 'lstPrioridades',
            data: $scope.filtro,
            showAjaxLoader: true,
            pageConfig: { pageName: 'page', pageTargetName: 'paginaPrioridades' },
            success: function (resp) {
               
            }
        });
    }

    $scope.listarQuantidadeTipoCliente = function () {

        var url = Util.getUrl("/{0}/QuantidadeClientesPorTipo", [$scope.urlBase]);

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'resumoQuantidadeTipoCliente',
            responseModelName: 'resumoQuantidadeTipoCliente',
            showAjaxLoader: true,
            data: $scope.filtro,
            success: function (resp) {

            }
        });
    }


    $scope.listarAgendamentoDoDia = function (pagina) {

        
        var url = Util.getUrl("/{0}/ListarAgendamentoDoDia", [$scope.urlBase]);

        if (pagina) {

            url += "?pagina=" + pagina;
        }

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstAgendamentoDoDia',
            responseModelName: 'lstAgendamentoDoDia',
            showAjaxLoader: true,
            pageConfig: { pageName: 'page', pageTargetName: 'pageAgendamentoDoDia' },
            data : $scope.filtro,
            success: function (resp) {

            }
        });
    }

    $scope.listarAgendamentoAtrasado = function (pagina) {

        
        var url = Util.getUrl("/{0}/ListarAgendamentoAtrasado", [$scope.urlBase]);

        if (pagina) {

            url += "?pagina=" + pagina;
        }

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstAgendamentoAtrasado',
            responseModelName: 'lstAgendamentoAtrasado',
            showAjaxLoader: true,
            pageConfig: { pageName: 'page', pageTargetName: 'pageAgendamentoAtrasado' },
            data: $scope.filtro,
            success: function (resp) {

            }
        });
    }


    $scope.listarAgendamentoVindouro = function (pagina) {

       var url = Util.getUrl("/{0}/ListarAgendamentoVindouro", [$scope.urlBase]);

        if (pagina) {

            url += "?pagina=" + pagina;
        }

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstAgendamentoVindouro',
            responseModelName: 'lstAgendamentoVindouro',
            showAjaxLoader: true,
            pageConfig: { pageName: 'page', pageTargetName: 'pageAgendamentoVindouro' },
            data: $scope.filtro,
            success: function (resp) {

            }
        });
    }

    $scope.lstQtdNotificacoes = function () {

        var url = Util.getUrl("/notificacoes/ListarQtdNotificacoesNaoLidas");
        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'qtdNotificacoes',
            responseModelName: 'qtdNotificacoes',
            success: function () {

            }
        });
    }

    if (window.InfoClienteController !== undefined) {

        InfoClienteController($scope, formHandlerService, $http, conversionService, $timeout);
    }


    $scope.IrParaCliente = function() {

        if (Util.isPathValid($scope, "notify.TP_NTF_ID")) {

            if ($scope.notify.TP_NTF_ID == 1) {

                if (Util.isPathValid($scope, "notify.AGENDAMENTO.CLI_ID", true)) {

                    var CLI_ID = $scope.notify.AGENDAMENTO.CLI_ID;

                    var url = Util.getUrl("/franquia/clientes/editarEVisualizar?clienteId=" + CLI_ID);
                    post(url);
                }
            }
            else if ($scope.notify.TP_NTF_ID == 2) {

                if (Util.isPathValid($scope, "notify.CLI_ID", true)) {

                    var CLI_ID = $scope.notify.CLI_ID;

                    var url = Util.getUrl("/franquia/clientes/editarEVisualizar?clienteId=" + CLI_ID);
                    post(url);
                }
            }
        }
    }

    $scope.CarregarGraficoAtendimentosRealizadosNoMes = function () {

        if ($scope.filtro.data) {


            var url = Util.getUrl("/{0}/AtendimentosRealizadosNoMes", [$scope.urlBase]);

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'atendimentosRealizadosMes',
                responseModelName: 'atendimentosRealizadosMes',
                showAjaxLoader: true,
                data: $scope.filtro,
                success: function (resp) {

                    $scope.datasourceAtendimentos = angular.copy($scope.atendimentosRealizadosMes);
                    $scope.datasourceAtendimentosPie = angular.copy($scope.datasourceAtendimentos);

                    $scope.datasourceAtendimentosPie.chart

                    $scope.datasourceAtendimentosPie.chart.showBorder = 0;
                    $scope.datasourceAtendimentosPie.chart.use3DLighting = "0",
                    $scope.datasourceAtendimentosPie.chart.enableSmartLabels = "0",
                    $scope.datasourceAtendimentosPie.chart.startingAngle = "310",
                    $scope.datasourceAtendimentosPie.chart.showLabels = "0",
                    $scope.datasourceAtendimentosPie.chart.showPercentValues = "1",
                    $scope.datasourceAtendimentosPie.chart.showLegend = "1",
                    $scope.datasourceAtendimentosPie.chart.centerLabel = "Atendimentos de  $label: $value",
                    $scope.datasourceAtendimentosPie.chart.centerLabelBold = "1",
                    $scope.datasourceAtendimentosPie.chart.showTooltip = "1",
                    $scope.datasourceAtendimentosPie.chart.decimals = "0",
                    $scope.datasourceAtendimentosPie.chart.useDataPlotColorForLabels = "1",
                    $scope.datasourceAtendimentosPie.chart.theme = "carbon"

                    $scope.setConfig($scope.datasourceAtendimentos);                    
                }
            });
        }
    }

    $scope.CarregarGraficoAtendimentosRealizadosNoMesPorRegiao = function () {

        if ($scope.filtro.data) {


            var url = Util.getUrl("/{0}/AtendimentosRealizadosNoMesPorRegiao", [$scope.urlBase]);

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'atendimentosRealizadosMesPorRegiao',
                responseModelName: 'atendimentosRealizadosMesPorRegiao',
                showAjaxLoader: true,
                data: $scope.filtro,
                success: function (resp) {

                    $scope.datasourceAtendimentosPorRegiao = angular.copy($scope.atendimentosRealizadosMesPorRegiao);
                    $scope.datasourceAtendimentosPiePorRegiao = angular.copy($scope.datasourceAtendimentosPorRegiao);

                    $scope.datasourceAtendimentosPiePorRegiao.chart

                    $scope.datasourceAtendimentosPiePorRegiao.chart.showBorder = 0;
                    $scope.datasourceAtendimentosPiePorRegiao.chart.use3DLighting = "0",
                    $scope.datasourceAtendimentosPiePorRegiao.chart.enableSmartLabels = "0",
                    $scope.datasourceAtendimentosPiePorRegiao.chart.startingAngle = "310",
                    $scope.datasourceAtendimentosPiePorRegiao.chart.showLabels = "0",
                    $scope.datasourceAtendimentosPiePorRegiao.chart.showPercentValues = "1",
                    $scope.datasourceAtendimentosPiePorRegiao.chart.showLegend = "1",
                    $scope.datasourceAtendimentosPiePorRegiao.chart.centerLabel = "Atendimentos de  $label: $value",
                    $scope.datasourceAtendimentosPiePorRegiao.chart.centerLabelBold = "1",
                    $scope.datasourceAtendimentosPiePorRegiao.chart.showTooltip = "1",
                    $scope.datasourceAtendimentosPiePorRegiao.chart.decimals = "0",
                    $scope.datasourceAtendimentosPiePorRegiao.chart.useDataPlotColorForLabels = "1",
                    $scope.datasourceAtendimentosPiePorRegiao.chart.theme = "carbon"

                    $scope.setConfig($scope.datasourceAtendimentosPorRegiao);
                }
            });
        }
    }


    $scope.RelatorioFaturamentoFranquia = function () {

        if ($scope.filtro.data) {


            var url = Util.getUrl("/{0}/RelatorioFaturamentoFranquia", [$scope.urlBase]);

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'relatorioFaturamentoFranquia',
                responseModelName: 'relatorioFaturamentoFranquia',
                showAjaxLoader: true,
                data: $scope.filtro,
                success: function (resp) {

                    $scope.relatorioFaturamentoFranquia = angular.copy($scope.relatorioFaturamentoFranquia);                  
                    $scope.relatorioFaturamentoFranquia.chart.numberPrefix = "R$";
                    $scope.relatorioFaturamentoFranquia.chart.formatNumber = '1';
                    $scope.relatorioFaturamentoFranquia.chart.formatNumberScale = '0';
                    $scope.relatorioFaturamentoFranquia.chart.decimals = '2';
                    $scope.relatorioFaturamentoFranquia.chart.forceDecimals = '1';
                    $scope.relatorioFaturamentoFranquia.chart.decimalSeparator = ",";
                    $scope.relatorioFaturamentoFranquia.chart.thousandSeparator = ".";
                    $scope.relatorioFaturamentoFranquia.chart.theme = "zune";

                    
                }
            });
        }
    }
});