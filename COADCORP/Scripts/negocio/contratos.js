appModule.controller('ContratosController', function ($scope, formHandlerService, $http, conversionService, $timeout) {

    $scope.filtro = {};
    $scope.filtro2 = {};
    var now = new Date();


    $scope.initAReceber = function () {

        $scope.filtro = {};
        $scope.filtro.dtini = new Date(now.getFullYear(), now.getMonth(), 1);
        $scope.filtro.dtfim = new Date(now.getFullYear(), now.getMonth(), now.getDate());
        $scope.filtro.emp_id = 2;
        $scope.filtro.data = 1;
        $scope.filtro.tipo = 0;
        $scope.filtro.banco = 0;
        $scope.filtro.mes = now.getMonth();
        $scope.filtro.ano = now.getFullYear();
        $scope.filtro.tipodata = true;
        $scope.filtro.tipobaixa = 0;

    }

    $scope.iniRelContratoTipoPgto = function (_dados) {

        $scope.filtro = {};
        $scope.filtro.dtini = new Date(now.getFullYear(), now.getMonth(), 1);
        $scope.filtro.dtfim = new Date(now.getFullYear(), now.getMonth(), now.getDate());
        $scope.filtro.emp_id = 2;
        $scope.filtro.mes = now.getMonth();
        $scope.filtro.ano = now.getFullYear();
        $scope.filtro.tipodata = true;

    }

    $scope.iniRelContratoCancelado = function (_dados) {

        $scope.filtro = {};
        $scope.filtro.dtini = new Date(now.getFullYear(), now.getMonth(), 1);
        $scope.filtro.dtfim = new Date(now.getFullYear(), now.getMonth(), now.getDate());
        $scope.filtro.emp_id = 2;
        $scope.filtro.tipo = 0;
        $scope.filtro.mes = now.getMonth();
        $scope.filtro.ano = now.getFullYear();
        $scope.filtro.tipodata = true;

    }

    $scope.iniRelFaturamentoUF = function (_dados) {

        $scope.filtro = {};
        $scope.filtro.dtini = new Date(now.getFullYear(), now.getMonth(), 1);
        $scope.filtro.dtfim = new Date(now.getFullYear(), now.getMonth(), now.getDate());
        $scope.filtro.emp_id = 2;
        $scope.filtro.ordalfabetica = false;
        $scope.filtro.mes = now.getMonth();
        $scope.filtro.ano = now.getFullYear();
        $scope.filtro.tipodata = true;

    }

    $scope.iniRelFaturamentoProd = function (_dados) {

        $scope.filtro = {};
        $scope.filtro.dtini = new Date(now.getFullYear(), now.getMonth(), 1);
        $scope.filtro.dtfim = new Date(now.getFullYear(), now.getMonth(), now.getDate());
        $scope.filtro.emp_id = 2;
        $scope.filtro.tipodata = true;
        $scope.filtro.mes = now.getMonth();
        $scope.filtro.ano = now.getFullYear();


    }

    $scope.iniRelResumoCReceber = function (_dados) {

        $scope.filtro = {};
        $scope.filtro.dtini = new Date(now.getFullYear(), now.getMonth(), 1);
        $scope.filtro.dtfim = new Date(now.getFullYear(), now.getMonth(), now.getDate());
        $scope.filtro.emp_id = 2;
        $scope.filtro.ordalfabetica = false;
        $scope.filtro.mes = now.getMonth();
        $scope.filtro.ano = now.getFullYear();
        $scope.filtro.tipodata = true;

    }

    $scope.initApuraRecebimento = function (_dados) {

        $scope.filtro = {};
        $scope.filtro.dtini = new Date(now.getFullYear(), now.getMonth(), 1);
        $scope.filtro.dtfim = new Date(now.getFullYear(), now.getMonth(), now.getDate());
        $scope.filtro.emp_id = 2;
        $scope.filtro.ordalfabetica = false;
        $scope.filtro.mes = now.getMonth();
        $scope.filtro.ano = now.getFullYear();
        $scope.filtro.tipodata = true;
        $scope.filtro.tipobaixa = 0;

    }

    $scope.iniRelFaturamentoContrato = function (_dados) {

        $scope.filtro = {};
        $scope.filtro.tipo = 0;
        $scope.filtro.dtini = new Date(now.getFullYear(), now.getMonth(), 1);
        $scope.filtro.dtfim = new Date(now.getFullYear(), now.getMonth(), now.getDate());
        $scope.filtro.emp_id = 2;
        $scope.filtro.analitico = true;
        $scope.filtro.financeiro = true;
        $scope.filtro.mes = now.getMonth();
        $scope.filtro.ano = now.getFullYear();
        $scope.filtro.tipodata = true;

    }
    $scope.iniRelClientesProduto = function (_ultimodia) {

        $scope.filtro = {};
        $scope.filtro.emp_id = 2;
        $scope.filtro.mes = now.getMonth();
        $scope.filtro.ano = now.getFullYear();
        $scope.filtro.tipodata = true;

    }
    $scope.iniPrevisaoReceita = function (_dados) {

        $scope.filtro = {};
        $scope.filtro.ano = now.getFullYear();
        $scope.filtro.tipodata = true;

    }


    $scope.dataAtualFormatada = function () {

        var parseDate = new Date();
        var jsDate = new Date(parseDate);

        var data = jsDate;
        var dia = data.getDate();
        if (dia.toString().length == 1)
            dia = "0" + dia;
        var mes = data.getMonth() + 1;
        if (mes.toString().length == 1)
            mes = "0" + mes;
        var ano = data.getFullYear();

        $scope.filtro.dataatual = dia + "/" + mes + "/" + ano;

    }

    $scope.listarApuraRecebimento = function () {

        $scope.dataAtualFormatada();

        showAjaxLoader2();

        var url = "/RelApuracaoRecebimento/BuscarApuraRecebimento";
        $http({
            url: url,
            method: "post",
            data: { _dtini: $scope.filtro.dtini
                  , _dtfim: $scope.filtro.dtfim
                  , _emp_id: $scope.filtro.emp_id
                  , _ban_id: $scope.filtro.banco
                  , _gru_id: $scope.filtro.grupo_id
                  , _tipobaixa: $scope.filtro.tipobaixa
            }

        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.lstTotal = response.result.lstTotal;
                $scope.lstGeral = response.result.lstGeral;

                conversionService.deepConversion($scope.lstTotal);
                conversionService.deepConversion($scope.lstGeral);

            }
            else {

                alert(response.message.message);

                $scope.ConferenciaFinanceira = null;

                hideAjaxLoader();
            }

        }).error(function (response) {

            alert(message);

            $scope.message = Util.createMessage("fail", response);
            $scope.ConferenciaFinanceira = null;

            hideAjaxLoader();
        });
    }


    $scope.listarConferenciaFinanceira = function () {

        $scope.dataAtualFormatada();

        showAjaxLoader2();

        var url = "/RelConferenciaFinanceira/BuscarConferenciaFinanceira";
        $http({
            url: url,
            method: "post",
            data: {
                _dtini: $scope.filtro.dtini
                  , _dtfim: $scope.filtro.dtfim
                  , _emp_id: $scope.filtro.emp_id
                  , _grupo_id: $scope.filtro.grupo_id
                  , _tipodata: $scope.filtro.tipo
            }

        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.lstConferenciaFinanceira = response.result.ConferenciaFinanceira.RELATORIO;
                $scope.lstTotalEntrada = response.result.ConferenciaFinanceira.TOTAL_ENTRADA;
                $scope.lstTotalBaixado = response.result.ConferenciaFinanceira.TOTAL_BAIXADO;
                $scope.lstTotalfaturamento = response.result.ConferenciaFinanceira.TOTAL_FATURAMENTO;
                $scope.lstTotalAprazo = response.result.ConferenciaFinanceira.TOTAL_APRAZO;

                conversionService.deepConversion($scope.lstConferenciaFinanceira);

            }
            else {

                alert(response.message.message);

                $scope.ConferenciaFinanceira = null;

                hideAjaxLoader();
            }

        }).error(function (response) {

            alert(message);

            $scope.message = Util.createMessage("fail", response);
            $scope.ConferenciaFinanceira = null;

            hideAjaxLoader();
        });

    }
    $scope.listarPrevisaoReceita = function () {

        $scope.dataAtualFormatada();

        showAjaxLoader2();

        var url = "/RelPrevisaoReceita/PrevisaoReceita";
        $http({
            url: url,
            method: "post",
            data: {
                _mes: $scope.filtro.mes
                   , _ano: $scope.filtro.ano
                   , _emp_id: $scope.filtro.emp_id
                   , _grupo_id: $scope.filtro.grupo_id

            }

        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.lstPrevisaoReceita = response.result.lstPrevisaoReceita;

                conversionService.deepConversion($scope.lstPrevisaoReceita);

            }
            else {

                alert(response.message.message);

                $scope.lstPrevisaoReceita = null;

                hideAjaxLoader();
            }

        }).error(function (response) {

            alert(message);

            $scope.message = Util.createMessage("fail", response);
            $scope.lstPrevisaoReceita = null;

            hideAjaxLoader();
        });

    }



    $scope.listarTitulosAreceber = function (pageRequest) {

        $scope.dataAtualFormatada();

        showAjaxLoader2();

        var url = "/RelAreceber/ListarTitulosAreceber";
        $http({
            url: url,
            method: "post",
            data: {
                _dtini: $scope.filtro.dtini
               , _dtfim: $scope.filtro.dtfim
               , _emp_id: $scope.filtro.emp_id
               , _tipodata: $scope.filtro.data
               , _tiporel: $scope.filtro.tipo
               , _tipobanco: $scope.filtro.banco
               , _banid: $scope.filtro.banid
               , _grupoid: $scope.filtro.grupo_id
               , _tipobaixa: $scope.filtro.tipobaixa
               , _rem_id: $scope.filtro.remid
               , _numpagina: pageRequest
               , _grupoid: $scope.filtro.grupo_id
            }


        }).success(function (response) {

            hideAjaxLoader();

            if (response.success) {

                $scope.vlrprevisto = response.result.vlrprevisto;
                $scope.vlrpago = response.result.vlrpago;
                $scope.lstareceber = response.result.lstareceber;

                conversionService.deepConversion($scope.lstareceber);

                $scope.page = response.page;

            }
            else {

                alert(response.message.message);

                $scope.lstareceber = null;

                hideAjaxLoader();
            }

        }).error(function (response) {

            alert(message);

            $scope.message = Util.createMessage("fail", response);
            $scope.lstareceber = null;

            hideAjaxLoader();
        });

    }


    $scope.listarFaturamentoContrato = function (pageRequest) {

        $scope.dataAtualFormatada();

        showAjaxLoader2();


        var url = "/RelFaturamentoContrato/ListarFaturamentoContrato";
        $http({
            url: url,
            method: "post",
            data: {_dtini: $scope.filtro.dtini
                  , _dtfim: $scope.filtro.dtfim
                  , _emp_id: $scope.filtro.emp_id
                  , _ctr: $scope.filtro.ctr_num_contrato
                  , _asn: $scope.filtro.asn_num_assinatura
                  , _grupo_id: $scope.filtro.grupo_id
                  , _analitico: $scope.filtro.analitico
                  , _tipodata: $scope.filtro.tipodata
                  , _numpagina: pageRequest
            }

        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.listaFaturamentoContrato = response.result.listaFaturamentoContrato;
                $scope.tolcontratos = response.result.tolcontratos;

                conversionService.deepConversion($scope.listaFaturamentoContrato);

                $scope.page = response.page;

            }
            else {

                alert(response.message.message);
                $scope.listaclientesproduto = null;

                hideAjaxLoader();
            }

        }).error(function (response) {

            alert(message);

            $scope.message = Util.createMessage("fail", response);
            $scope.listaclientesproduto = null;

            hideAjaxLoader();
        });

    }

    $scope.listarClientesProduto = function (id) {

        $scope.dataAtualFormatada();

        showAjaxLoader2();


        var url = "/RelClientesProduto/ListarClientesProduto";
        $http({
            url: url,
            method: "post",
            data: {
                _mes: $scope.filtro.mes,
                _ano: $scope.filtro.ano
            }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.listaclientesproduto = response.result.listaclientesproduto;
                $scope.totqtde = response.result.totqtde;
                $scope.totvalor = response.result.totvalor;

                conversionService.deepConversion($scope.listaclientesproduto);

            }
            else {

                alert(response.message.message);
                $scope.listaclientesproduto = null;

                hideAjaxLoader();
            }

        }).error(function (response) {

            alert(message);

            $scope.message = Util.createMessage("fail", response);
            $scope.listaclientesproduto = null;

            hideAjaxLoader();
        });

    }

    $scope.listarContratosCancelados = function () {

        $scope.dataAtualFormatada();

        showAjaxLoader2();


        var url = "/RelContratosCancelados/BuscarContratosCancelados";
        $http({
            url: url,
            method: "post",
            data: {
                _mes: $scope.filtro.mes,
                _ano: $scope.filtro.ano,
                _emp_id: $scope.filtro.emp_id,
                _grupo_id: $scope.filtro.grupo_id,
                _tipo: $scope.filtro.tipo,
                _rep_id: $scope.filtro.representante
            }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.lstContratosCancelados = response.result.lstContratosCancelados;
                $scope.total = response.result.total;
                conversionService.deepConversion($scope.lstContratosCancelados);

            }
            else {

                $scope.message = response.message;

                $scope.lstContratosCancelados = null;

                hideAjaxLoader();
            }

        }).error(function (response) {

            $scope.message = response.message;
            $scope.lstContratosCancelados = null;

            hideAjaxLoader();
        });

    }
    $scope.listarContratoTipoPgto = function () {

        $scope.dataAtualFormatada();

        showAjaxLoader2();

        var url = "/RelFaturamentoTipoPgto/ListarContratoTipoPgto";
        $http({
            url: url,
            method: "post",
            data: {
                _dtini: $scope.filtro.dtini
                  , _dtfim: $scope.filtro.dtfim
                  , _emp_id: $scope.filtro.emp_id
                  , _tipodata: $scope.filtro.tipodata
                  , _qtdeParcelas : 0
                  , _grupo_id: $scope.filtro.grupo_id
            }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.lstContratosTipoPgto = response.result.lstContratosTipoPgto;
                $scope.tolcontratos = response.result.tolcontratos;
                $scope.qtdecontratos = response.result.qtdecontratos;
                conversionService.deepConversion($scope.lstContratosTipoPgto);

            }
            else {

                $scope.message = response.message;

                $scope.lstContratosTipoPgto = null;

                hideAjaxLoader();
            }

        }).error(function (response) {

            $scope.message = response.message;
            $scope.lstContratosTipoPgto = null;

            hideAjaxLoader();
        });

    }
    $scope.listarFaturamentoProduto = function () {

        $scope.dataAtualFormatada();

        showAjaxLoader2();

        var url = "/RelFaturamentoProduto/BuscarFaturamentoProduto";
        $http({
            url: url,
            method: "post",
            data: {
                _dtini: $scope.filtro.dtini
                  , _dtfim: $scope.filtro.dtfim
                  , _emp_id: $scope.filtro.emp_id
                  , _grupo_id: $scope.filtro.grupo_id
                  , _tipodata: $scope.filtro.tipodata
            }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.lstFaturamentoProduto = response.result.lstFaturamentoProduto;
                $scope.total = response.result.total;
                conversionService.deepConversion($scope.lstFaturamentoProduto);

            }
            else {

                $scope.message = response.message;

                $scope.lstContratosCancelados = null;

                hideAjaxLoader();
            }

        }).error(function (response) {

            $scope.message = response.message;
            $scope.lstContratosCancelados = null;

            hideAjaxLoader();
        });

    }
    $scope.listarBuscarFaturamentoProdutoUF = function () {

        $scope.dataAtualFormatada();

        showAjaxLoader2();

        var url = "/RelFaturamentoUF/BuscarFaturamentoProdutoUF";
        $http({
            url: url,
            method: "post",
            data: {
                _dtini: $scope.filtro.dtini
                  , _dtfim: $scope.filtro.dtfim
                  , _emp_id: $scope.filtro.emp_id
                  , _grupo_id: $scope.filtro.grupo_id
                  , _tipodata: $scope.filtro.tipodata
            }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.lstFaturamentoProdUF = response.result.lstFaturamentoProdUF;
                $scope.total = response.result.total;
                $scope.totalcanc = response.result.totalcanc;
                conversionService.deepConversion($scope.lstFaturamentoProdUF);

            }
            else {

                $scope.message = response.message;

                $scope.lstFaturamentoUF = null;

                hideAjaxLoader();
            }

        }).error(function (response) {

            $scope.message = response.message;
            $scope.lstFaturamentoUF = null;

            hideAjaxLoader();
        });

    }
    $scope.listarBuscarFaturamentoUF = function () {

        $scope.dataAtualFormatada();

        showAjaxLoader2();

        var url = "/RelFaturamentoUF/BuscarFaturamentoUF";
        $http({
            url: url,
            method: "post",
            data: {
                _dtini: $scope.filtro.dtini
                  , _dtfim: $scope.filtro.dtfim
                  , _emp_id: $scope.filtro.emp_id
                  , _grupo_id: $scope.filtro.grupo_id
                  , _ordalfabetica: $scope.filtro.ordalfabetica
                  , _tipodata: $scope.filtro.tipodata
            }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success) {

                $scope.lstFaturamentoUF = response.result.lstFaturamentoUF;
                $scope.total = response.result.total;
                $scope.totalcanc = response.result.totalcanc;
                conversionService.deepConversion($scope.lstFaturamentoUF);

            }
            else {

                $scope.message = response.message;

                $scope.lstFaturamentoUF = null;

                hideAjaxLoader();
            }

        }).error(function (response) {

            $scope.message = response.message;
            $scope.lstFaturamentoUF = null;

            hideAjaxLoader();
        });

    }
    $scope.listarBuscarResumoCReceber = function () {

        $scope.dataAtualFormatada();

        showAjaxLoader2();

        var url = "/RelResumoCReceber/BuscarResumoCReceber";
        $http({
            url: url,
            method: "post",
            data: {
                _dtini: $scope.filtro.dtini
                  , _dtfim: $scope.filtro.dtfim
                  , _emp_id: $scope.filtro.emp_id
                  , _tipodata: $scope.filtro.tipodata
            }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {
                $scope.lstResumoCReceber = response.result.lstResumoCReceber;
                $scope.totalcanc = response.result.totalcanc;
                $scope.totalfat = response.result.totalfat;
                $scope.totalpago = response.result.totalpago;
                $scope.totalreceber = response.result.totalreceber;
                conversionService.deepConversion($scope.lstResumoCReceber);
            }
            else {

                $scope.message = response.message;

                $scope.lstResumoCReceber = null;

                hideAjaxLoader();
            }

        }).error(function (response) {

            $scope.message = response.message;
            $scope.lstResumoCReceber = null;

            hideAjaxLoader();
        });

    }
    $scope.expPlanilha = function (_listaexport, _url) {

        showAjaxLoader2();

        $scope.export = {};
        $scope.export.lista = _listaexport;

        if (_url == null)
            _url = '/RelClientesProduto/ExportarXLS';

        $http({
            method: 'Post',
            //dataType: 'json',
            url: _url,
            data: { _lista: $scope.export.lista }

        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {
                $scope.export.lnkPath = response.result.retorno;
                $scope.message = Util.createMessage("success", "Operação realizada com sucesso!!");
            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);

            hideAjaxLoader2();
        });

    }
    $scope.expPlanilhaUf = function (_listaexport) {

        showAjaxLoader2();

        $scope.export = {};
        $scope.export.lista = _listaexport;
        $scope.export.nomearquivo = "Faturamento_UF";


        $http({
            method: 'Post',
            //dataType: 'json',
            url: '/RelFaturamentoUF/ExportarXLS',
            data: { _lista: $scope.export.lista }

        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {
                $scope.export.lnkPath = response.result.retorno;
                $scope.message = Util.createMessage("success", "Operação realizada com sucesso!!");
            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);

            hideAjaxLoader2();
        });

    }
    $scope.expPlanilhaContrato = function () {

        showAjaxLoader2();

        $scope.export = {};

        var url = "/RelFaturamentoContrato/ExportarXLS";
        $http({
            url: url,
            method: "post",
            data: {
                _dtini: $scope.filtro.dtini
                  , _dtfim: $scope.filtro.dtfim
                  , _emp_id: $scope.filtro.emp_id
                  , _grupo_id: $scope.filtro.grupo_id
                  , _tipodata: $scope.filtro.tipodata

            }

        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {
                $scope.export.lnkPath = response.result.retorno;
                $scope.message = Util.createMessage("success", "Operação realizada com sucesso!!");
            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
            }

        }).error(function (response) {

            $scope.message = response.message;

            hideAjaxLoader2();
        });

    }


    $scope.expPlanilhaRelAreceber = function () {

        showAjaxLoader2();

        $scope.export = {};

        var url = "/RelAreceber/ExportarXLS";
        $http({
            url: url,
            method: "post",
            data: {
                _dtini: $scope.filtro.dtini
               , _dtfim: $scope.filtro.dtfim
               , _emp_id: $scope.filtro.emp_id
               , _tipodata: $scope.filtro.data
               , _tiporel: $scope.filtro.tipo
               , _tipobanco: $scope.filtro.banco
               , _banid: $scope.filtro.banid
               , _grupoid: $scope.filtro.grupo_id
            }

        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {
                $scope.export.lnkPath = response.result.retorno;
                $scope.message = Util.createMessage("success", "Operação realizada com sucesso!!");
            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);

            hideAjaxLoader2();
        });

    }

});

   