appModule.controller("HomeControler", function ($scope, $http, formHandlerService) {

    $scope.AtualizarGraficos = function () {

        var data = new Date();

        if ($scope.mesatual == null)
            $scope.mesatual = data.getMonth()-1;

        if ($scope.anoatual == null)
            $scope.anoatual = data.getFullYear();

        if ($scope.regiao_uf == null)
            $scope.regiao_uf = "RJ";

        $scope.GerarGraficoRep($scope.anoatual, $scope.mesatual, $scope.regiao_uf);

    };

    $scope.GerarGraficoRep = function (ano, mes, regiao_uf) {

        showAjaxLoader();

        var _data = { _ano: ano, _mes : mes, _regiao_uf : regiao_uf }
      
        $http({
            url: "/Home/GraficoRepresentante",
            method: "Post",
            dataType: 'json',
            data: _data
        }).success(function (response) {

            hideAjaxLoader();

            if (response != null) {
                $scope.CarregarGrafico(response);
                $scope.CarregarGrafico2(response);

            }
        })

    };

    $scope.CarregarGrafico = function (_dadosgrafico) {

        showAjaxLoader();

        var placeholder = $("#flot-line-chart");

        placeholder.unbind();

        $("#title").text(_dadosgrafico.Titulo);
        $("#description").text(_dadosgrafico.Descricao);

        var data = [];
        
        for (var i = 0; i < _dadosgrafico.Dados.length; i++) {
            data[i] = [_dadosgrafico.Dados[i].label, _dadosgrafico.Dados[i].data];
        }


        $.plot(placeholder, [data], {
            series: {
                bars: {
                    show: true,
                    barWidth: 0.6,
                    align: "center"
                }
            },
            legend: {
                show: false
            },
            tooltip: true,
            tooltipOpts: {
                content: " %x,  %y"
            },
            grid: {
                hoverable: true
            },
            xaxis: {
                mode: "categories",
                tickLength: 10
            }
        });
        $(placeholder).bind("plothover", pieHover);
        //$(placeholder).bind("plotclick", pieClick);

        hideAjaxLoader();

    };

    $scope.CarregarGrafico2 = function (_dadosgrafico) {
        //Flot Pie Chart

        showAjaxLoader();

        var placeholder = $("#flot-pie-chart");

        placeholder.unbind();

        $("#title").text(_dadosgrafico.Titulo);
        $("#description").text(_dadosgrafico.Descricao);

        var data = _dadosgrafico.Dados;

        $.plot(placeholder, data, {
            series: {
                pie: {
                    show: true
                }
            },
            grid: {
                hoverable: true,
                clickable: true
            },
            tooltip: true,
            tooltipOpts: {
                content: "%p.0%, %s", // show percentages, rounding to 2 decimal places
                shifts: {
                    x: 20,
                    y: 0
                },
                defaultTheme: false
            }
        });
        $(placeholder).bind("plothover", pieHover);
        $(placeholder).bind("plotclick", pieClick);

        hideAjaxLoader();

    };

    function labelFormatter(label, series) {
        return '<div style="font-size:8pt;text-align:center;padding:2px;color:white;">' + label + '<br/>' + Math.round(series.percent) + '%</div>';
    };

    function pieHover(event, pos, obj) {
        if (!obj)
            return;
        percent = parseFloat(obj.series.percent).toFixed(2);
        $("#hover").html('<span style="font-weight: bold; color: ' + obj.series.color + '">' + obj.series.label + ' (' + percent + '%)</span>');
    }

    function pieClick(event, pos, obj) {
        if (!obj)
            return;
        percent = parseFloat(obj.series.percent).toFixed(2);
        alert('' + obj.series.label + ': ' + percent + '%');
    }




});
