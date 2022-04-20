
appModule.controller("HomeControler", function ($scope, $http, formHandlerService, conversionService) {

    $scope.param = {};
    $scope.param.TDC_ID = "1228f7e8-b4a0-45e3-bf67-099c7fda840b";
    $scope.param.TGR_ID = 1;
    

    var now = new Date();

    $scope.LimparGrafico = function () {

        //--------
        $scope.param.chart = {};
        //---------

        //--------Grafico
        $scope.listadbgraf = [];
        $scope.dataSource = {};
        $scope.dataSource.data = [];
        $scope.dataSource00 = {};
        $scope.dataSource00.data = [];
        $scope.dataSource01 = {};
        $scope.dataSource01.data = [];
        $scope.dataSource02 = {};
        $scope.dataSource02.data = [];
        $scope.dataSource03 = {};
        $scope.dataSource03.data = [];
        $scope.dataSource04 = {};
        $scope.dataSource04.data = [];
        $scope.dataSource05 = {};
        $scope.dataSource05.data = [];
        $scope.dataSource06 = {};
        $scope.dataSource06.data = [];
        $scope.dataSource07 = {};
        $scope.dataSource07.data = [];
        $scope.dataSource08 = {};
        $scope.dataSource08.data = [];

        $scope.dataurarj = {};
        $scope.dataurarj.data = [];
        $scope.datauramg = {};
        $scope.datauramg.data = [];
        $scope.dataurapr = {};
        $scope.dataurapr.data = [];
        //--------Grafico
    }


    $scope.CarregarTela = function () {

        $scope.LimparGrafico();
        $scope.listarRep();

        $scope.param = {};
        $scope.param.dataatual = new Date();
        $scope.param.mes = now.getMonth()+1;
        $scope.param.ano = now.getFullYear().toString();
        $scope.param.emp_id = 2;
        $scope.param.grupo_id = 1;
        $scope.param.qtdeParcelas = 0;

    }
    $scope.deslogarUsuario = function (item) {

        showAjaxLoader();

        var _data = { _session_id: item.SESSION_ID };
        
        $http({
            url: "/Home/DeslogarUsuario",
            method: "Post",
            dataType: 'json',
            data: _data
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success) {

                $scope.mostrarUsuarioLogado();
            };

        }).error(function () {

            hideAjaxLoader();

        })

    };

    $scope.mostrarUsuarioLogado = function () {

        showAjaxLoader();
          
        $http({
            url: "/Home/BuscarUsuariosLogados",
            method: "Post",
            dataType: 'json'
     //       data: _data
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success) {

                $scope.listaautenticado = response.result.listaautenticado;

                for (var ind in $scope.listaautenticado) {

                    if ($scope.listaautenticado[ind].DATA_LOGIN != null)
                        $scope.listaautenticado[ind].DATA_LOGIN = dataAtualFormatada($scope.listaautenticado[ind].DATA_LOGIN);
                }

            };

        }).error(function () {

            hideAjaxLoader();

        })

    };

    $scope.preparaTela = function () {


        showAjaxLoader();

        var _data = { _numsimulador: $scope.param.TDC_ID, _dataacesso: $scope.param.dataatual }
        var date = "";
        var formattedDate = "";

        $http({
            url: "/Home/CarregarGrafico",
            method: "Post",
            dataType: 'json',
            data: _data
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success) {

                $scope.objSimuladorUFMes = response.result.grafSimuladorUFMes;

        
            };

        }).error(function () {

            hideAjaxLoader();

        })

    };
    $scope.listarRep = function () {

        showAjaxLoader();

        var url = "/RelFaturamentoRepresentante/BuscarListaRepresentantes";

        $http({
            url: url,
            method: "Post"
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {
                $scope.lstrepresentante = response.result.lstrepresentante;
                conversionService.deepConversion($scope.lstrepresentante);
            }
            else {
                $scope.lstrepresentante = null;
                alert(response.message.message);
            }

        }).error(function (response) {

            $scope.lstrepresentante = null;
            alert(response.message.message);

            hideAjaxLoader();
        })

    }
    $scope.CarregarGraficoPagamentos = function () {

        showAjaxLoader();


        var _data = {
            _mes: $scope.param.mes,
            _ano: $scope.param.ano,
            _emp_id: $scope.param.emp_id,
            _grupo_id: $scope.filtro.grupo_id,
            _qtdeParcelas: $scope.param.qtdeParcelas
        }
        var date = "";
        var formattedDate = "";

        $http({
            url: "/Home/CarregarGraficoPagamentos",
            method: "Post",
            dataType: 'json',
            data: _data
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success) {

                $scope.LimparGrafico();
                
                $scope.dataSource00 = response.result.grafPagamentosPeriodo;
                $scope.dataSource01 = response.result.grafPgtoProdutoPeriodo;
                $scope.dataSource02 = response.result.grafPgtoQtdeValor;

            };

        }).error(function () {

            hideAjaxLoader();

        })

    };
    $scope.CarregarGraficoVendas = function () {

        showAjaxLoader();
        
        var _representante = null;

        if ($scope.param.representante != null)
            _representante = $scope.param.representante.REP_ID;


        var _data = {
            _mes: $scope.param.mes,
            _ano: $scope.param.ano,
            _emp_id: $scope.param.emp_id,
            _rep_id: _representante,
            _grupo: $scope.param.grupo_id
        }
        var date = "";
        var formattedDate = "";

        $http({
            url: "/Home/CarregarGraficoVendas",
            method: "Post",
            dataType: 'json',
            data: _data
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success) {

                $scope.LimparGrafico();
                $scope.grafContratosEvolucaoAnual  = response.result.grafContratosEvolucaoAnual;
                $scope.grafContratosVendasEvolucao = response.result.grafContratosVendasEvolucao;
                $scope.grafVendastotgeral = response.result.grafVendastotgeral;
                $scope.grafVendastotcanc  = response.result.grafVendastotcanc;
                $scope.grafVendastotpago  = response.result.grafVendastotpago;
                $scope.grafVendastotpend = ($scope.grafVendastotgeral -
                                            $scope.grafVendastotcanc -
                                            $scope.grafVendastotpago);

                var grafVendasPeriodo = response.result.grafVendasPeriodo;
                var grafContratosPeriodo = response.result.grafContratosPeriodo;
                var grafContratosGrupo = response.result.grafContratosGrupo;
                var grafContratosRepresentante = response.result.grafContratosRepresentante;
                var grafContratosVendasEvolucao = response.result.grafContratosVendasEvolucao;
                var grafContratosEvolucaoAnual = response.result.grafContratosEvolucaoAnual;
                
                $scope.dataSource00 = grafVendasPeriodo;
                $scope.dataSource01 = grafContratosPeriodo;
                $scope.dataSource03 = grafContratosRepresentante;
                $scope.dataSource04 = grafContratosGrupo;
                $scope.dataSource05 = grafContratosVendasEvolucao;
                $scope.dataSource06 = grafContratosEvolucaoAnual;

            };

        }).error(function () {

            hideAjaxLoader();

        })

    };


    $scope.CarregarGrafico = function () {

        showAjaxLoader();

        var _data = { _numsimulador: $scope.param.TDC_ID, _dataacesso: $scope.param.dataatual, _grupo: $scope.param.TGR_ID }
        var date = "";
        var formattedDate = "";

        $http({
            url: "/Home/CarregarGrafico",
            method: "Post",
            dataType: 'json',
            data: _data
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success) {

                $scope.LimparGrafico();

                var objSimuladorUFMesCalc = response.result.grafSimuladorUFMesCalc;
                var objSimuladorH = response.result.grafSimuladorH;
                var objSimuladorD = response.result.grafSimuladorD;
                var objgrafUF = response.result.grafUF;
                var objgrafProduto = response.result.grafProduto;
                var objgrafTabelas = response.result.grafTabelas;
                var objgrafTabelasGruAno = response.result.grafTabelasGruAno;
                var objgrafTabelasGruMes = response.result.grafTabelasGruMes;

                var objgrafUraRamalRJ = response.result.grafUraRamalRJ;
                var objgrafUraRamalMG = response.result.grafUraRamalMG;
                var objgrafUraRamalPR = response.result.grafUraRamalPR;


                $scope.param.chart.theme = "ocean";
                $scope.param.chart.yaxisname = "Quantidade";
                $scope.param.chart.showlegend = '0';
                $scope.param.chart.showpercentvalues = '0';
                $scope.param.chart.formatNumber = '0';
                $scope.param.chart.formatNumberScale = '0';
                $scope.param.chart.paletteColors = "#0075c2";
                $scope.param.chart.valueFontColor = "#000000";

                //$scope.param.chart.refreshinterval = '5';
                //$scope.param.chart.yaxisminvalue = '5';
                //$scope.param.chart.yaxismaxvalue = '10000';
                //$scope.param.chart.numdisplaysets = '10';
                //$scope.param.chart.labeldisplay ='rotate';

                //-------------objSimuladorH
                $scope.param.chart.xaxisname = "Horas";
                $scope.param.chart.caption = objSimuladorH.Titulo;
                $scope.param.chart.subCaption = objSimuladorH.Descricao;
                $scope.param.chart.plottooltext = 'Às $label hs foram realizados $datavalue calculos no simulador';

                for (var ind in objSimuladorH.Dados) {

                    var dbgraf = {};
                    dbgraf.label = objSimuladorH.Dados[ind].label;
                    dbgraf.value = objSimuladorH.Dados[ind].data;
                    $scope.dataSource.data.push(dbgraf);

                }

                $scope.dataSource.chart = angular.copy($scope.param.chart);

                //------------- objSimuladorD
                $scope.param.chart.xaxisname = "Dias";
                $scope.param.chart.caption = objSimuladorD.Titulo;
                $scope.param.chart.subCaption = objSimuladorD.Descricao;
                $scope.param.chart.plottooltext = 'Dia $label foram realizados $datavalue calculos no simulador.';
                $scope.param.chart.paletteColors = "#0075c2";
                $scope.param.chart.bgColor= "#ffffff";
                $scope.param.chart.showBorder= "0";
                $scope.param.chart.showCanvasBorder = "0";
                $scope.param.chart.plotBorderAlpha= "10";
                $scope.param.chart.usePlotGradientColor="0";
                $scope.param.chart.plotFillAlpha= "50";
                $scope.param.chart.showXAxisLine="1";
                $scope.param.chart.axisLineAlpha= "25";
                $scope.param.chart.divLineAlpha= "10";
                $scope.param.chart.showValues="1";
                $scope.param.chart.showAlternateHGridColor="0";
                $scope.param.chart.captionFontSize="14";
                $scope.param.chart.subcaptionFontSize="14";
                $scope.param.chart.subcaptionFontBold= "0";
                $scope.param.chart.toolTipColor="#ffffff";
                $scope.param.chart.toolTipBorderThickness = "0";
                $scope.param.chart.toolTipBgColor="#000000";
                $scope.param.chart.toolTipBgAlpha= "80";
                $scope.param.chart.toolTipBorderRadius= "2";
                $scope.param.chart.toolTipPadding = "5";


                for (var ind in objSimuladorD.Dados) {
                    var dbgraf = {};
                    dbgraf.label = objSimuladorD.Dados[ind].label;
                    dbgraf.value = objSimuladorD.Dados[ind].data;
                    $scope.dataSource01.data.push(dbgraf);
                }

                $scope.dataSource01.chart = angular.copy($scope.param.chart);
                
                //------------- objgrafUF
                $scope.param.chart.xaxisname = "UF";
                $scope.param.chart.caption = objgrafUF.Titulo;
                $scope.param.chart.subCaption = objgrafUF.Descricao;
                $scope.param.chart.plottooltext = 'UF: $label ==> Quantidade: $datavalue';
                $scope.param.chart.paletteColors = "";

                for (var ind in objgrafUF.Dados) {
                    var dbgraf = {};
                    dbgraf.label = objgrafUF.Dados[ind].label;
                    dbgraf.value = objgrafUF.Dados[ind].data;
                    $scope.dataSource02.data.push(dbgraf);
                }

                $scope.dataSource02.chart = angular.copy($scope.param.chart);
                 
  
                //-------------objSimuladorUFDia

                $scope.param.chart.xaxisname = "UF";
                $scope.param.chart.caption = objSimuladorUFMesCalc.Titulo;
                $scope.param.chart.subCaption = objSimuladorUFMesCalc.Descricao;
                $scope.param.chart.plottooltext = 'Clientes de $label realizaram $datavalue calculos no simulador';
                $scope.param.chart.paletteColors = "";

                for (var ind in objSimuladorUFMesCalc.Dados) {

                    var dbgraf = {};
                    dbgraf.label = objSimuladorUFMesCalc.Dados[ind].label;
                    dbgraf.value = objSimuladorUFMesCalc.Dados[ind].data;
                    $scope.dataSource05.data.push(dbgraf);
                }

                $scope.dataSource05.chart = angular.copy($scope.param.chart);

                //------------- objgrafProduto
                $scope.param.chart.xaxisname = "Produto";
                $scope.param.chart.caption = objgrafProduto.Titulo;
                $scope.param.chart.subCaption = objgrafProduto.Descricao;
                $scope.param.chart.plottooltext = 'Produto: $label ==> Quantidade: $datavalue';
                $scope.param.chart.paletteColors = "";

                for (var ind in objgrafProduto.Dados) {

                    var dbgraf = {};
                    dbgraf.label = objgrafProduto.Dados[ind].label;
                    dbgraf.value = objgrafProduto.Dados[ind].data;
                    $scope.dataSource03.data.push(dbgraf);
                }

                $scope.dataSource03.chart = angular.copy($scope.param.chart);

                //------------- objgrafTabelas
                $scope.param.chart.xaxisname = "Tabelas";
                $scope.param.chart.caption = objgrafTabelas.Titulo;
                $scope.param.chart.subCaption = objgrafTabelas.Descricao;
                $scope.param.chart.plottooltext = 'Tabela: $label ==> Quantidade: $datavalue';
                $scope.param.chart.paletteColors = "";

                for (var ind in objgrafTabelas.Dados) {

                    var dbgraf = {};
                    dbgraf.label = objgrafTabelas.Dados[ind].label;
                    dbgraf.value = objgrafTabelas.Dados[ind].data;
                    $scope.dataSource06.data.push(dbgraf);
                }

                $scope.dataSource06.chart = angular.copy($scope.param.chart);


                //------------- objgrafTabelasGruAno
                $scope.param.chart.xaxisname = "Tabelas";
                $scope.param.chart.caption = objgrafTabelasGruAno.Titulo;
                $scope.param.chart.subCaption = objgrafTabelasGruAno.Descricao;
                $scope.param.chart.plottooltext = 'Tabela: $label ==> Quantidade: $datavalue';
                $scope.param.chart.paletteColors = "";

                for (var ind in objgrafTabelasGruAno.Dados) {

                    var dbgraf = {};
                    dbgraf.label = objgrafTabelasGruAno.Dados[ind].label;
                    dbgraf.value = objgrafTabelasGruAno.Dados[ind].data;
                    $scope.dataSource07.data.push(dbgraf);
                }

                $scope.dataSource07.chart = angular.copy($scope.param.chart);


                //------------- objgrafTabelasGruMes
                $scope.param.chart.xaxisname = "Tabelas";
                $scope.param.chart.caption = objgrafTabelasGruMes.Titulo;
                $scope.param.chart.subCaption = objgrafTabelasGruMes.Descricao;
                $scope.param.chart.plottooltext = 'Tabela: $label ==> Quantidade: $datavalue';
                $scope.param.chart.paletteColors = "";

                for (var ind in objgrafTabelasGruMes.Dados) {

                    var dbgraf = {};
                    dbgraf.label = objgrafTabelasGruMes.Dados[ind].label;
                    dbgraf.value = objgrafTabelasGruMes.Dados[ind].data;
                    $scope.dataSource08.data.push(dbgraf);
                }

                $scope.dataSource08.chart = angular.copy($scope.param.chart);

                //-------------objgrafUraRamalRJ

                $scope.param.chart.xaxisname = "UF";
                $scope.param.chart.caption = objgrafUraRamalRJ.Titulo;
                $scope.param.chart.subCaption = objgrafUraRamalRJ.Descricao;
                $scope.param.chart.plottooltext = 'Ramal $label realizaram $datavalue atendimento';
                $scope.param.chart.paletteColors = "";

                for (var ind in objgrafUraRamalRJ.Dados) {

                    var dbgraf = {};
                    dbgraf.label = objgrafUraRamalRJ.Dados[ind].label;
                    dbgraf.value = objgrafUraRamalRJ.Dados[ind].data;
                    $scope.dataurarj.data.push(dbgraf);
                }

                $scope.dataurarj.chart = angular.copy($scope.param.chart);

                //-------------objgrafUraRamalMG

                $scope.param.chart.xaxisname = "UF";
                $scope.param.chart.caption = objgrafUraRamalMG.Titulo;
                $scope.param.chart.subCaption = objgrafUraRamalMG.Descricao;
                $scope.param.chart.plottooltext = 'Ramal $label realizaram $datavalue atendimento';
                $scope.param.chart.paletteColors = "";

                for (var ind in objgrafUraRamalMG.Dados) {

                    var dbgraf = {};
                    dbgraf.label = objgrafUraRamalMG.Dados[ind].label;
                    dbgraf.value = objgrafUraRamalMG.Dados[ind].data;
                    $scope.datauramg.data.push(dbgraf);
                }

                $scope.datauramg.chart = angular.copy($scope.param.chart);

                //-------------objgrafUraRamalPR

                $scope.param.chart.xaxisname = "UF";
                $scope.param.chart.caption = objgrafUraRamalPR.Titulo;
                $scope.param.chart.subCaption = objgrafUraRamalPR.Descricao;
                $scope.param.chart.plottooltext = 'Ramal $label realizaram $datavalue atendimento';
                $scope.param.chart.paletteColors = "";

                for (var ind in objgrafUraRamalPR.Dados) {

                    var dbgraf = {};
                    dbgraf.label = objgrafUraRamalPR.Dados[ind].label;
                    dbgraf.value = objgrafUraRamalPR.Dados[ind].data;
                    $scope.dataurapr.data.push(dbgraf);
                }

                $scope.dataurapr.chart = angular.copy($scope.param.chart);

                
            };

        }).error(function () {

            hideAjaxLoader();

        })

    };

    function labelFormatter(label, series) {
        return '<div style="font-size:8pt;text-align:center;padding:2px;color:white;">' + label + '<br/>' + Math.round(series.percent) + '%</div>';
    };
    $scope.buscaDataAtual = function () {

        var data = new Date();

        var dia = data.getDate();
        if (dia.toString().length == 1)
            dia = "0" + dia;
        var mes = data.getMonth() + 1;
        if (mes.toString().length == 1)
            mes = "0" + mes;
        var ano = data.getFullYear();

        $scope.param.dataatual = dia + "/" + mes + "/" + ano;

    }

    $scope.initEditor = function () {

        $scope.modelA = "<p><div>Diego <strong>ANDRADE</strong></div></p>";
        $scope.modelB = "<p><div>Diego sssssssssssssssssssssss<strong>ANDRADE</strong><em>Silva</em></div></p>";
    }

    function dataAtualFormatada(dataHora) {

        var parseDate = new Date(parseInt(dataHora.substr(6)));
        var jsDate = new Date(parseDate);

        var data = jsDate;
        var dia = data.getDate();
        if (dia.toString().length == 1)
            dia = "0" + dia;
        var mes = data.getMonth() + 1;
        if (mes.toString().length == 1)
            mes = "0" + mes;
        var ano = data.getFullYear();

        var hora = (data.getHours() < 10) ? "0" + data.getHours() : data.getHours();
        var min = (data.getMinutes() < 10) ? "0" + data.getMinutes() : data.getMinutes();
        var formattedTime = hora + ":" + min + ":" + data.getSeconds();

        return dia + "/" + mes + "/" + ano + " - " + formattedTime;

    }


});
