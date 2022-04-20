/// <reference path="controllers_plugins/endereco.js" />
appModule.controller('ParcelasController', function ($scope, formHandlerService, $http, conversionService, $timeout) {
   

    $scope.filtro = {};
    $scope.filtro2 = {};
    $scope.filtro.Logradouro = " ";
    $scope.filtro.Email = " ";
    $scope.filtro.Telefone = " ";
    $scope.filtro.URA_ID = null;
    $scope.filtro.representante = {};
    $scope.mostragrupo = true;
    $scope.filtro.tipo = 0;
    $scope.filtro.tipodata = 0;
    $scope.filtro.tipobaixa = 0;
    $scope.filtro.tiposelecao = 0;
    $scope.filtro.ordalfabetica = false;
    

    var _data = new Date();
    $scope.filtro.mes = _data.getMonth() + 1;
    $scope.filtro.ano = _data.getFullYear();
    $scope.filtro.regiao = "MG";

    
    $scope.initAlocados = function () {

        $scope.filtro = {};
        $scope.filtro.dtini = new Date(now.getFullYear(), now.getMonth(), 1);
        $scope.filtro.dtfim = new Date(now.getFullYear(), now.getMonth(), now.getDate());
        $scope.filtro.banco = 0;
        $scope.filtro.mes = now.getMonth();
        $scope.filtro.ano = now.getFullYear();
        $scope.listarTitulosAlocados();

    }

    $scope.initClienteFranquia = function () {

        $scope.filtro = { pesquisaCpfCnpjPorIqualdade: true };

    }    
    
    $scope.assinturaSelect = null;
    $scope.totassSelect = null;
    $scope.consassSelect = null;
    $scope.atendimento = {};
 
    var now = new Date();
    var dataatual = new Date(now.getFullYear(), now.getMonth(), now.getDate());
    
    $scope.iniRelContratoRep = function (_rep_id,_rep_nome) {

        $scope.filtro = {};
        $scope.filtro.dataatual = new Date();
        $scope.filtro.mes = now.getMonth();
        $scope.filtro.ano = now.getFullYear();
        $scope.filtro.emp_id = 2;
        $scope.filtro.qtdeParcelas = 0;
        $scope.filtro.dtini = new Date(now.getFullYear(), now.getMonth(),1);
        $scope.filtro.dtfim = new Date();
        $scope.filtro.ordalfabetica = false;
        $scope.filtro.tipo = 0;

        if (_rep_id != null) {

            if ($scope.filtro.representante == null) {
                $scope.filtro.representante = {};
                $scope.filtro.representante.REP_ID = null;
            }

            $scope.filtro.representante.REP_ID = _rep_id;
            $scope.filtro.representante.REP_NOME = _rep_nome.toUpperCase();

            $scope.listarContratoRepData();
        }
        else
            $scope.listarRep();
        

    }
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

 
    $scope.iniRelLiquidacao = function (_dados) {

        $scope.filtro.dtini = new Date(now.getFullYear(), now.getMonth(), 1);
        $scope.filtro.dtfim = new Date(now.getFullYear(), now.getMonth(), now.getDate());
        $scope.filtro.empid = 2;
        $scope.filtro.banid = null;
        $scope.filtro.par_num_parcela = null;
    }

    $scope.buscarPordata = function (_dados) {

        showAjaxLoader();

        var url = "/Parcelas/BuscarPordata";

        var _data = _dados.DATA_LIQUIDACAO_TXT;
        $scope.filtro.TOTAL_DIA = _dados.TOTAL_DIA;

        $http({
            url: url,
            method: "Post",
            data: { _dtini: _data, _emp_id: $scope.filtro.empid, _ban_id: $scope.filtro.banid }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {
                $scope.listaparcelas = response.result.listaparcelas;
                $scope.page = response.page;
            }
            else {
                $scope.listaparcelas = null;
                $scope.message = Util.createMessage("fail", response.message.message);
            }

        }).error(function (response) {

            $scope.listaparcelas = null;
            $scope.message = Util.createMessage("fail", response.message.message);

            hideAjaxLoader();
        })
    }


    $scope.listarEmpresa = function () {

        showAjaxLoader();

        var url = "/RelFaturamentoRepresentante/BuscarListaEmpresa";

        $http({
            url: url,
            method: "Post"
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {
                $scope.lstempresa = response.result.lstempresa;
                conversionService.deepConversion($scope.lstempresa);
            }
            else {
                $scope.lstempresa = null;
                alert(response.message.message);
            }

        }).error(function (response) {

            $scope.lstempresa = null;
            alert(response.message.message);

            hideAjaxLoader();
        })

    }


    $scope.listarContratoRepData = function () {

        showAjaxLoader();

        $scope.dataHoraFormatada();

        if ($scope.filtro.representante == null) {
            $scope.filtro.representante = {};
            $scope.filtro.representante.REP_ID = null;
        }


        var url = "/RelFaturamentoRepresentante/BuscarContratoRepData";

        $http({
            url: url,
            method: "Post",
            data: {
                _dtini: $scope.filtro.dtini,
                _dtfim: $scope.filtro.dtfim,
                _emp_id: $scope.filtro.emp_id,
                _rep_id: $scope.filtro.representante.REP_ID,
                _grupo_id: $scope.filtro.grupo_id,
                _ordalfabetica: $scope.filtro.ordalfabetica,
                _tipo: $scope.filtro.tipo
            }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {
                $scope.listacontratorep = response.result.listacontratorep;
                $scope.listacontratorepSint = response.result.listacontratorepSint;
                conversionService.deepConversion($scope.listacontratorep);
                conversionService.deepConversion($scope.listacontratorepSint);
                $scope.tolvenda = response.result.tolvenda;
                $scope.tolcontratos = response.result.tolcontratos;
                $scope.tolrenovacao = response.result.tolrenovacao;
            }
            else {
                $scope.listaparcelas = null;
                alert(response.message.message);
            }

        }).error(function (response) {

            $scope.listacontratorep = null;
            alert(response.message.message);

            hideAjaxLoader();
        })

    }

    $scope.listarContratoRep = function () {

        showAjaxLoader();

        $scope.dataHoraFormatada();

        if ($scope.filtro.representante == null) {
            $scope.filtro.representante = {};
            $scope.filtro.representante.REP_ID = null;
        }


        var url = "/RelFaturamentoRepresentante/BuscarContratoRep";

        $http({
            url: url,
            method: "Post",
            data: {
                _mes: $scope.filtro.mes,
                _ano: $scope.filtro.ano,
                _emp_id: $scope.filtro.emp_id,
                _rep_id: $scope.filtro.representante.REP_ID,
                _grupo_id: $scope.filtro.grupo_id,
                _ordalfabetica: $scope.filtro.ordalfabetica,
                _tipo: $scope.filtro.tipo
            }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {
                $scope.listacontratorep = response.result.listacontratorep;
                $scope.listacontratorepSint = response.result.listacontratorepSint;
                conversionService.deepConversion($scope.listacontratorep);
                conversionService.deepConversion($scope.listacontratorepSint);
                $scope.tolvenda = response.result.tolvenda;
                $scope.tolcontratos = response.result.tolcontratos;
                $scope.tolrenovacao = response.result.tolrenovacao;
            }
            else {
                $scope.listaparcelas = null;
                alert(response.message.message);
            }

        }).error(function (response) {

            $scope.listacontratorep = null;
            alert(response.message.message);

            hideAjaxLoader();
        })

    }

    $scope.listarTitulosAlocados = function () {

        showAjaxLoader();

        var url = "/RelAlocados/ListarTitulosAlocados";

        $http({
            url: url,
            method: "Post",
            data: {
                _dtini: $scope.filtro.dtini,
                _dtfim: $scope.filtro.dtfim,
                _banid: $scope.filtro.banid
            }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {
                $scope.lstalocados = response.result.lstalocados;
                conversionService.deepConversion($scope.lstalocados);

            }
            else {

                $scope.lstalocados = null;

                alert(response.message.message);
            }

        }).error(function (response) {

            $scope.listaparcelas = null;
            alert(response.message.message);

            hideAjaxLoader();
        })
        
    }

    $scope.listar = function (pageRequest) {

        if ($scope.filtro) {
            if ($scope.filtro.empid < 1) {
                $scope.message = Util.createMessage("fail", "Selecione uma empresa");
                return;
            }
        }

        showAjaxLoader();

        var url = "/Parcelas/BuscarParcelasConciliacao";
                
        $http({
            url: url,
            method: "Post",
            data: {
                _dtini: $scope.filtro.dtini,
                _dtfim: $scope.filtro.dtfim,
                _emp_id: $scope.filtro.empid,
                _ban_id: $scope.filtro.banid,
                _parcela: $scope.filtro.par_num_parcela,
                _tipodata: $scope.filtro.tipodata,
                _tipoBaixa: $scope.filtro.tipobaixa,
                _pagina: pageRequest
            }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {
                $scope.listaparcelas = response.result.listaparcelas;
                $scope.listadatas = response.result.listadatas;
                conversionService.deepConversion($scope.listadatas);
                conversionService.deepConversion($scope.listaparcelas);

                $scope.page = response.page;
                
            }
            else {
                $scope.listaparcelas = null;

                alert(response.message.message);
            }

        }).error(function (response) {

            $scope.listaparcelas = null;
            alert(response.message.message);
            
            hideAjaxLoader();
        })

    }

    $scope.abriModalComprovante = function (item) {

        $scope.iselect = {};
        $scope.iselect.CLI_ID = item.CLI_ID;
        $scope.iselect.ASN_NUM_ASSINATURA = item.ASN_NUM_ASSINATURA;
        $scope.iselect.CLI_NOME = item.CLI_NOME;
        $scope.iselect.PAR_NUM_PARCELA = item.PAR_NUM_PARCELA;
        $scope.iselect.BAN_ID = item.BAN_ID;
        $scope.iselect.PLI_DATA = item.PLI_DATA;
        $scope.iselect.PLI_VALOR = item.PLI_VALOR;
        $scope.iselect.DATA_EMISSAO = new Date(now.getFullYear(), now.getMonth(), now.getDate());
        $scope.iselect.PLI_TIPO_DOC = item.PLI_TIPO_DOC;
        $scope.iselect.REGIAO_UF = item.REGIAO_UF;
        $scope.iselect.REP_ID = item.REP_ID;
        $scope.iselect.REP_NOME = item.REP_NOME;

        
        showAjaxLoader();

        var url = "/Parcelas/BuscarEndereco";

        $http({
            url: url,
            method: "Post",
            data: { _cli_id: $scope.iselect.CLI_ID }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.endereco = response.result.endereco;
                $scope.iselect.END_NUMERO = $scope.endereco.END_NUMERO;
                $scope.iselect.END_BAIRRO = $scope.endereco.END_BAIRRO;
                $scope.iselect.END_CEP = $scope.endereco.END_CEP;
                $scope.iselect.END_UF = $scope.endereco.END_UF;
                $scope.iselect.END_MUNICIPIO = $scope.endereco.END_MUNICIPIO;
                $scope.iselect.END_COMPLEMENTO = $scope.endereco.END_COMPLEMENTO;
                $scope.iselect.END_LOGRADOURO = $scope.endereco.END_LOGRADOURO;

                angular.element("#modal-comprovante").modal();
            }
            else {
                $scope.listaparcelas = null;
                alert(response.message.message);
            }

        }).error(function (response) {

            $scope.listaparcelas = null;
            alert(response.message.message);

            hideAjaxLoader();
        });

    }
    $scope.expPlanilha = function () {

        showAjaxLoader2();

        $scope.export = {};
               
        var _url = '/Parcelas/ExportarXLS';

        $http({
            method: 'Post',
            //dataType: 'json',
            url: _url,
            data: { _dtini: $scope.filtro.dtini,
                    _dtfim: $scope.filtro.dtfim,
                    _emp_id: $scope.filtro.empid,
                    _ban_id: $scope.filtro.banid,
                    _parcela: $scope.filtro.par_num_parcela,
                    _tipoBaixa: $scope.filtro.tipobaixa
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
    $scope.expPlanilhaRep = function (_listaexport, _listaexport1) {

        showAjaxLoader2();

        $scope.export = {};
        $scope.export.lista = _listaexport;
        $scope.export.lista1 = _listaexport1;

        $http({
            method: 'Post',
            //dataType: 'json',
            url: '/RelFaturamentoRepresentante/ExportarXLS',
            data: {
                   _lista: $scope.export.lista
                  ,_lista1: $scope.export.lista1
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


    function dataAtualFormatadatxt(dataHora) {

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
        return dia + "/" + mes + "/" + ano;

    }

    $scope.dataAtualFormatada = function(dataHora) { 

        if (dataHora == null)
            return null;

        var parseDate = new Date(parseInt(dataHora.substr(6)));
        var jsDate = new Date(parseDate);

        return jsDate;
    }

    if (window.InfoMarketingController !== undefined) {

        InfoMarketingController($scope, formHandlerService, $http, conversionService);
    }

    if (window.TelefoneController !== undefined) {

        TelefoneController($scope, formHandlerService, $http, conversionService);
    }

    if (window.InfoClienteController !== undefined) {

        InfoClienteController($scope, formHandlerService, $http, conversionService, $timeout);
    }

    if (window.HistoricoClienteController !== undefined) {

        HistoricoClienteController($scope, formHandlerService, $http, conversionService, $timeout);
    }

    if (window.CarteiramentoPluginController !== undefined) {

        CarteiramentoPluginController($scope, formHandlerService, $http, conversionService, $timeout);
    }    
        
    if (window.EnderecoController !== undefined) {

        EnderecoController($scope, formHandlerService, $http, $timeout);
    }
    
    $scope.dataHoraFormatada = function(dataHora) { 

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
    


  
});

