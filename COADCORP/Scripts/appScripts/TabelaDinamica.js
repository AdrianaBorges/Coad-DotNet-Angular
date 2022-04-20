appModule.controller('TabelaDinamicaController', function ($scope, formHandlerService, $http, $interval, conversionService, $sce) {


    $scope.pesquisa = {};
    $scope.pesquisa.tipo = 1;
    $scope.TOTAL_LINHAS = 0;
    $scope.Timer = null;
    $scope.orientacao = false;
    $scope.tabela = {};
    $scope.login = {};
    $scope.login.p = "nome";
    $scope.login.u = "usuario";
    $scope.listas = {};
    $scope.param = {};
    $scope.param.cod_ncm = null;
    $scope.param.cod_cest = null;
    $scope.possuibanner = false;

    $scope.editor = {};

    $scope.tabref = [];
    $scope.campostring = [];
    $scope.campoint = [];
    $scope.campodata = [];
    $scope.tabela.editor = [];
    $scope.ParamConsulta = [];

    $scope.param.TDC_TIPO = 0;
    $scope.param.nometabela = null;
    $scope.search = {};
    $scope.search.TAB_STRING01 = "";
    $scope.nometelapesquisa = "";
    $scope.cadastrada = false;

    $scope.confirmaEdit = false;
    $scope.confirmaNovo = false;
    $scope.isNumber = angular.isNumber;


    $scope.UFNORTE = ["AC", "AP", "AM", "PA", "RO", "RR", "TO", "DF", "GO", "MT", "MS"];
    $scope.UFSUL = ["PR", "SC", "RS", "RJ", "SP", "MG"]
    $scope.UFNORDESTE = ["AL", "BA", "CE", "MA", "PB", "PE", "PI", "RN", "SE"];

    $scope.trustAsHtml = function (string) {
        return $sce.trustAsHtml(string);
    };



    $scope.contadorProcessamento = function () {

        showAjaxLoader2();

        var url = "/TabelaDinamica/ContadorProcessamento";
        $http({
            url: url,
            method: "post",
        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {
                $scope.TOTAL_LINHAS = response.result.TOTAL_LINHAS;

                if ($scope.TOTAL_LINHAS == 0)
                    $scope.StopTimer();
            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);

                hideAjaxLoader2();
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);

            hideAjaxLoader2();
        });
    }


    $scope.filterFloat = function (value) {

        if (/^(\-|\+)?([0-9]+(\.[0-9]+)?|Infinity)$/
            .test(value))
            return Number(value);

        return value;
    }


    $scope.limpaParametros = function () {

        for (var ind in $scope.ParamConsulta) {

            $scope.ParamConsulta[ind].valor = "";
        }

        $scope.tabconfig.palavrachave = null;

    };


    $scope.buscaValorTabela = function (_parametros) {

        var _parametro = _parametros.split(":");

        var url = "/TabelaDinamica/BuscarValor";

        $http({
            url: url,
            method: "post",
            data: { _param: _parametro }
        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {

                $scope.retorno = response.result.valorRetorno;

            }
            else {

                $scope.retorno = 0;

                $scope.message = Util.createMessage("fail", response.message.message);

                hideAjaxLoader2();
            }

            return $scope.retorno;

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);
  
            hideAjaxLoader2();
      
        });

        return 0;

    }


    $scope.carregaTelaTabelaTipi = function (tipo,tipogrupo) {

        $scope.TGR_TIPO = tipogrupo;

        $scope.buscarMenuTabelas(tipo, null, tipogrupo);
    }

    $scope.carregaTabelaTipi = function (tipo) {

        showAjaxLoader2();

        if (tipo == 0 && $scope.tbtipi != null) {

            hideAjaxLoader2();
            return 0;

        }

        var url = "/TabelaDinamica/PesquisarTabelaTipi";
        $http({
            url: url,
            method: "post",
            data: { _ncm: $scope.param.cod_ncm, _cest: $scope.param.cod_cest, _tipo: tipo}
        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {

                if (tipo == 0)
                    $scope.tbtipi = response.result.tbtipi;
                else
                    $scope.tbtipiavan = response.result.tbtipi;

                $scope.tbcest = response.result.tbcest;

                $scope.page = 0;

                if (response.page != null)
                    $scope.page = response.page;

            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
                $scope.listatabela = null;

                hideAjaxLoader2();
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);
            $scope.tbtipi = null;
            $scope.tbcest = null;

            hideAjaxLoader2();
        });

    }

    $scope.carregaTabelaCest = function (tipo) {

        showAjaxLoader2();

        if (tipo == 0 && $scope.tbtcest != null) {

            hideAjaxLoader2();
            return 0;

        }

        var url = "/TabelaDinamica/PesquisarTabelaCest";
        $http({
            url: url,
            method: "post",
            data: { _ncm: $scope.param.cod_ncm, _cest: $scope.param.cod_cest, _tipo: tipo }
        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {

                if (tipo == 0)
                    $scope.tbtcest = response.result.tbtcest;
                else
                    $scope.tbtcestavan = response.result.tbtcest;
       
                $scope.page = 0;

                if (response.page != null)
                    $scope.page = response.page;

            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
                $scope.tbtcest = null;
                $scope.tbtcestavan = null;

                hideAjaxLoader2();
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);
            $scope.tbtcest = null;
            $scope.tbtcestavan = null;

            hideAjaxLoader2();
        });

    }

    $scope.carregaTelaImp = function () {

        showAjaxLoader2();

        var url = "/TabelaDinamica/CarregaTelaImp";
        $http({
            url: url,
            method: "post"
        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {

                $scope.listatabelas = response.result.listatabelas;
      
            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
                $scope.listatabelas = null;

                hideAjaxLoader2();
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);
            $scope.listatabelas = null;

            hideAjaxLoader2();
        });
    }

    $scope.buscarEmail = function () {

        var retorno = prompt("Ctrl+C para copiar o email abaixo.","desenvolvimento.tecnico@coad.com.br");

    }

    $scope.carregaTela = function (id, tipo, checa, grupo) {

        showAjaxLoader2();

        $scope.calculado = false;
        $scope.carregado = true;

        var url = "/TabelaDinamica/CarregarTela";
        $http({
            url: url,
            method: "post",
            data: { _id: id, _ChecaPublicado: checa }
        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {

                $scope.listagrupo = response.result.listagrupo;
                $scope.campostring = response.result.campostring;
                $scope.campoint = response.result.campoint;
                $scope.campodata = response.result.campodata;
                $scope.tabconfig = response.result.tabconfig;
                $scope.tabela = response.result.tabela;
                
                conversionService.deepConversion($scope.tabconfig);
                conversionService.deepConversion($scope.tabela);

                $scope.buscarItemTela(id);
  
                //---------

                $scope.buscarBanner(id);
                $scope.tabconfig.opcao = 0;

                //---------
                $scope.lstTabelasRef = response.result.lstTabelas;
                $scope.lstSimuladores = response.result.lstSimuladores;
                conversionService.deepConversion($scope.lstTabelasRef);
                conversionService.deepConversion($scope.lstSimuladores)

                $scope.buscarTabelaDinamica();
                //---------

                if (id == "" || id == null)
                    $scope.tabconfig.TDC_TIPO = tipo;

                angular.element("#Observacao").trigger("click");

                angular.element("#Configuracao").trigger("click");

                if ($scope.tabconfig.TDC_TIPO == 2) {
                    if ($scope.tabela.TAB_DINAMICA_ITEM == null || $scope.tabela.TAB_DINAMICA_ITEM.length < 1) {
                        $scope.add();
                        for (var ind in $scope.tabconfig.TAB_DINAMICA_CONFIG_ITEM) {
                            $scope.tabela.TAB_DINAMICA_ITEM[0][$scope.tabconfig.TAB_DINAMICA_CONFIG_ITEM[ind].TCI_NOME_CAMPODB] = 0;
                        }

                    }
                }

                //------------ Busca Menu
                if (grupo == 0)
                    grupo = null;

                $scope.buscarMenuTabelas(tipo, grupo);
                //------------

                $scope.montarListas();

                $scope.page = 0;

                if (response.page != null)
                    $scope.page = response.page;

            }
            else {

                if (response.success != null)
                    $scope.message = Util.createMessage("fail", response);
                else
                    $scope.message = Util.createMessage("fail", response.message.message);

                $scope.listatabela = null;

                hideAjaxLoader2();
            }

        }).error(function (response) {

            alert(response);

            $scope.message = Util.createMessage("fail", response);
            $scope.listatabela = null;

            hideAjaxLoader2();
        });

    }


    $scope.buscarItemTela = function (id, pageRequest) {

        showAjaxLoader2();

        var url = "/TabelaDinamica/BuscarItemTela";
        $http({
            url: url,
            method: "post",
            data: { _id: id, _pagina: pageRequest }
        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {

                $scope.tbitem = response.result.tbitem;
                conversionService.deepConversion($scope.tbitem);

                $scope.page = response.page;

            }
            else {

                if (response.success != null)
                    $scope.message = Util.createMessage("fail", response);
                else
                    $scope.message = Util.createMessage("fail", response.message.message);

                $scope.tbitem = null;

                hideAjaxLoader2();
            }

        }).error(function (response) {

            alert(response);

            $scope.message = Util.createMessage("fail", response);
            $scope.tbitem = null;

            hideAjaxLoader2();
        });

    }

    $scope.selCampoChave = function (itemtabcfg) {

        $scope.item1.TDC_ID_TAB_REF = itemtabcfg.TDC_ID;

        if (!itemtabcfg.TCI_CAMPO_CHAVE)
            $scope.item1.TCI_NOME_CHAVEDB = itemtabcfg.TCI_NOME_CAMPODB;
        else
            $scope.item1.TCI_NOME_CHAVEDB = null;
    }
    $scope.selCampoLista = function (itemtabcfg) {

        $scope.item1.TDC_ID_TAB_REF = itemtabcfg.TDC_ID;

        if (!itemtabcfg.TCI_CAMPO_LISTA)
            $scope.item1.TCI_NOME_LISTADB = itemtabcfg.TCI_NOME_CAMPODB;
        else
            $scope.item1.TCI_NOME_LISTADB = null;

    }

    $scope.abrejanelaAddItem = function (item) {

        $scope.confirmaEdit = false;
        $scope.confirmaNovo = false;

        if (item != null) {
            $scope.item1 = item;
            $scope.confirmaEdit = true;
            $scope.buscarConfigTabItem(item.TDC_ID_TAB_REF);
        }
        else {
            $scope.item1 = {};
            $scope.confirmaNovo = true;
        }

        angular.element("#modal-add-item").modal();
    }
    $scope.buscarBanner = function (_tdcid) {

        showAjaxLoader2();

        var url = "/TabelaDinamica/BuscarBanner";
        $http({
            url: url,
            method: "post",
            data: { _tdc_id: _tdcid }
        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {

                $scope.funcionalidade = response.result.funcionalidade;

                if($scope.funcionalidade.length>0)
                    $scope.possuibanner = true;
                else
                    $scope.possuibanner = false;


            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
                $scope.funcionalidade = null;

                hideAjaxLoader2();
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);
            $scope.funcionalidade = null;

            hideAjaxLoader2();

        });

    }

    $scope.buscarMenuTabelas = function (tipo, grupo, tipoGrupo) {

        showAjaxLoader2();

        var url = "/TabelaDinamica/BuscarMenuTabelas";
        $http({
            url: url,
            method: "post",
            data: { _tipo: tipo, _grupo: grupo, _tgr_tipo: tipoGrupo }
        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {

                $scope.lstMaisAcessadas = response.result.listaMaisAcessadas;
                $scope.lstAcessadas = response.result.listaAcessadas;
                $scope.lstTabelas = response.result.ListaTabDimGrugo;

                conversionService.deepConversion($scope.lstMaisAcessadas);
                conversionService.deepConversion($scope.lstAcessadas);
                conversionService.deepConversion($scope.lstTabelas);


            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
                $scope.lstTabelas = null;

                hideAjaxLoader2();
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);
            $scope.lstTabelas = null;

            hideAjaxLoader2();

        });


    }
    $scope.buscarSubItem = function (item) {

        $(".collapse in").collapse('hide');

        showAjaxLoader2();

        var url = "/TabelaDinamica/BuscarSubItem";
        $http({
            url: url,
            method: "post",
            data: { _node_pai: item }
        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {

                $("#" + item).collapse('show');

                $scope.tbtipin = response.result.subitem;
                $scope.tbtipin1 = response.result.subitem;
                $scope.tbtipin2 = response.result.subitem;
            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
                $scope.tbtipin = null;
                $scope.tbtipin1 = null;
                $scope.tbtipin2 = null;

                hideAjaxLoader2();
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);
            $scope.tbtipin = null;
            $scope.tbtipin1 = null;
            $scope.tbtipin2 = null;

            hideAjaxLoader2();
        });


    }
    $scope.abrirPopUpConsulta = function () {

        angular.element("#modal-consulta").modal();
    }
    $scope.buscarConfigTabItem = function (_tdcid) {

        showAjaxLoader2();

        var url = "/TabelaDinamica/BuscarConfigTabItem";
        $http({
            url: url,
            method: "post",
            data: { _tdc_id: _tdcid }
        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {

                $scope.item1.TDC_ID_TAB_REF = _tdcid;

                $scope.lstTabelasCfgItem = response.result.lstTabelasCfgItem;
            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
                $scope.lstTabelasCfgItem = null;

                hideAjaxLoader2();
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);
            $scope.lstTabelasCfgItem = null;

            hideAjaxLoader2();

        });


    }
    $scope.abrirPopUpExport = function (_item) {

        if ($scope.export == null)
            $scope.export = {};

        $scope.export.tdc_id = _item.TDC_ID;
        $scope.export.nometabela = _item.TDC_NOME_TABELA;
        $scope.export.nomearquivo = "";
        $scope.export.lnkPath = "";

        angular.element("#modal-Exportar").modal();
    }
    $scope.ordenarUp = function (item, indice) {

        if (indice > 0) {
            if ($scope.tabconfig.TAB_DINAMICA_CONFIG_ITEM == null) {
                $scope.tabconfig.TAB_DINAMICA_CONFIG_ITEM = [];
            }

            var atual = $scope.tabconfig.TAB_DINAMICA_CONFIG_ITEM[indice - 1];

            item.TCI_ORDEM_APRESENTACAO = indice - 1;
            atual.TCI_ORDEM_APRESENTACAO = indice;

            $scope.tabconfig.TAB_DINAMICA_CONFIG_ITEM[indice - 1] = item;
            $scope.tabconfig.TAB_DINAMICA_CONFIG_ITEM[indice] = atual;
        }

    }
    $scope.ordenarDown = function (item, indice) {

        if ($scope.tabconfig.TAB_DINAMICA_CONFIG_ITEM.length > (indice + 1)) {
            if ($scope.tabconfig.TAB_DINAMICA_CONFIG_ITEM == null) {
                $scope.tabconfig.TAB_DINAMICA_CONFIG_ITEM = [];
            }

            var atual = $scope.tabconfig.TAB_DINAMICA_CONFIG_ITEM[indice + 1];

            item.TCI_ORDEM_APRESENTACAO = indice + 1;
            atual.TCI_ORDEM_APRESENTACAO = indice;

            $scope.tabconfig.TAB_DINAMICA_CONFIG_ITEM[indice + 1] = item;
            $scope.tabconfig.TAB_DINAMICA_CONFIG_ITEM[indice] = atual;
        }

    }
    $scope.add = function () {

        if ($scope.tabela.TAB_DINAMICA_ITEM == null) {
            $scope.tabela.TAB_DINAMICA_ITEM = [];
        }

        var novo = {};

        for (var ind in $scope.tabconfig.TAB_DINAMICA_CONFIG_ITEM) {
            novo[$scope.tabconfig.TAB_DINAMICA_CONFIG_ITEM[ind].TCI_NOME_CAMPODB] = null;
        }

        $scope.tabela.TAB_DINAMICA_ITEM.push(novo);

    }
    $scope.remove = function (index) {

        $scope.tabela.TAB_DINAMICA_ITEM.splice(index, 1);
    }
    $scope.addItem = function (item) {

        if ($scope.tabconfig.TAB_DINAMICA_CONFIG_ITEM == null) {
            $scope.tabconfig.TAB_DINAMICA_CONFIG_ITEM = [];
        }

        var novo = {};
        novo.TCI_NOME_CAMPO = item.TCI_NOME_CAMPO;
        novo.TCI_TIPO_CAMPO = item.TCI_TIPO_CAMPO == null ? "S" : item.TCI_TIPO_CAMPO;
        novo.TCI_TAMANHO_CAMPO = item.TCI_TAMANHO_CAMPO == null ? 0 : item.TCI_TAMANHO_CAMPO;
        novo.TCI_ALINHAMENTO_CAMPO = item.TCI_ALINHAMENTO_CAMPO == null ? "D" : item.TCI_ALINHAMENTO_CAMPO;
        novo.TCI_ORDEM_APRESENTACAO = item.TCI_ORDEM_APRESENTACAO;
        novo.TCI_NOME_CAMPODB = item.TCI_NOME_CAMPODB;
        novo.TCI_VALOR_PADRAO = item.TCI_VALOR_PADRAO;
        novo.TCI_VALOR_ESPERADO = item.TCI_VALOR_ESPERADO;
        novo.TCI_FORMULA = item.TCI_FORMULA;
        novo.USU_LOGIN = item.USU_LOGIN;
        novo.TCI_TEXTO_HELP = item.TCI_TEXTO_HELP;
        novo.TDC_ID_TAB_REF = item.TDC_ID_TAB_REF;
        novo.TCI_TEXTO_HELP_LINK = item.TCI_TEXTO_HELP_LINK;
        novo.TCI_PATH_HELP_LINK = item.TCI_PATH_HELP_LINK;
        novo.TCI_TARGET = item.TCI_TARGET;


        $scope.tabconfig.TAB_DINAMICA_CONFIG_ITEM.push(novo);
        $scope.item1 = {};

        var encontrado = false;

        for (var ind in $scope.tabconfig.TAB_DINAMICA_CONFIG_ITEM) {
            $scope.tabconfig.TAB_DINAMICA_CONFIG_ITEM[ind].TCI_ORDEM_APRESENTACAO = ind;
            if (!encontrado && $scope.tabconfig.TAB_DINAMICA_CONFIG_ITEM[ind].TCI_NOME_CAMPODB != $scope.campostring[ind].Text) {
                $scope.tabconfig.TAB_DINAMICA_CONFIG_ITEM[ind].TCI_NOME_CAMPODB = $scope.campostring[ind].Text;
                encontrado = true;
            }
        }


    }
    $scope.addItemTabela = function () {

        if ($scope.tabela.TAB_DINAMICA_ITEM == null) {
            $scope.tabela.TAB_DINAMICA_ITEM = [];
        }
        else {
            $scope.tabela.TAB_DINAMICA_ITEM[0] = {};
        }

        for (var ind in $scope.tabconfig.TAB_DINAMICA_CONFIG_ITEM) {

            campo = $scope.tabconfig.TAB_DINAMICA_CONFIG_ITEM[ind].TCI_NOME_CAMPODB;

            $scope.tabela.TAB_DINAMICA_ITEM[0][campo] = "";

        }

    }
    $scope.removeItemTabela = function (index, item) {

        $scope.excluirItemTabela(item); 

        $scope.tbitem.splice(index, 1);
    }
    $scope.IncluirItemTabela = function (item) {

        var novo = {};
        var names = Object.getOwnPropertyNames(item);
        var _name = "";
      
        for (var i in names) {

            _name = names[i];

            novo[_name] = null;
        }

        $scope.tbitem.push(novo);

    }
    $scope.editarItemTabela = function (item) {

        if ($scope.tabela.TAB_DINAMICA_ITEM == null) {
            $scope.tabela.TAB_DINAMICA_ITEM = [];
        }
        else {
            $scope.tabela.TAB_DINAMICA_ITEM[0] = {};
        }

        $scope.tabela.TAB_DINAMICA_ITEM[0] = item;

        angular.element("#modal-NovoItem").modal();

    }
    $scope.removeItem = function (index) {

        $scope.tabconfig.TAB_DINAMICA_CONFIG_ITEM.splice(index, 1);

        for (var ind in $scope.tabconfig.TAB_DINAMICA_CONFIG_ITEM) {
            $scope.tabconfig.TAB_DINAMICA_CONFIG_ITEM[ind].TCI_ORDEM_APRESENTACAO = ind;
        }

    }
    $scope.listar = function (pageRequest) {

        if (($scope.param.TDC_TIPO < 1) && ($scope.param.nometabela == null || $scope.param.nometabela.length < 3)) {
            $scope.message = Util.createMessage("fail", "Informe o parte do nome da tabela para pesquisa.");
            $scope.listatabela = null;
        }
        else {

            showAjaxLoader2();

            var url = "/TabelaDinamica/Pesquisar";
            $http({
                url: url,
                method: "post",
                data: { _tab_descricao : $scope.param.nometabela,
                        _tdc_tipo: $scope.param.TDC_TIPO,
                        _publicados : $scope.param.PUBLICADO,
                        _tgr_id : $scope.param.TGR_ID,
                        _tit_id : $scope.param.TIT_ID, pagina: pageRequest }

            }).success(function (response) {

                hideAjaxLoader2();

                if (response.success == true) {

                    $scope.listatabela = response.result.listatabela;

                    $scope.page = response.page;
                }
                else {

                    $scope.message = Util.createMessage("fail", response.message.message);
                    $scope.listatabela = null;

                    hideAjaxLoader2();
                }

            }).error(function (response) {

                $scope.message = Util.createMessage("fail", response);
                $scope.listatabela = null;

                hideAjaxLoader2();
            });
        }


    }

    $scope.$watch("tabconfig.TAB_DINAMICA_CONFIG_ITEM", function (value, oldvalue) {

        if (value) {

            $scope.ParamConsulta = [];

            angular.forEach(value, function (cabec, index) {

                if (cabec.TCI_CAMPO_PESQUISA == true) {
                    var _param = {};
                    _param.label = cabec.TCI_NOME_CAMPO;
                    _param.campo = cabec.TCI_NOME_CAMPODB;
                    _param.esperado = cabec.TCI_VALOR_ESPERADO;
                    _param.valor = "";
                    $scope.ParamConsulta.push(_param);
                }

            });

            if ($scope.ParamConsulta.length == 0 && $scope.tabconfig.TDC_PALAVRA_CHAVE == true)
                $scope.tabconfig.opcao = 1;


        }

    })
    
    $scope.montaFormula = function (_item,_campodb) {

        showAjaxLoader2();

        if (_item.TCI_FORMULA== null)
            _item.TCI_FORMULA = _campodb;
        else
            _item.TCI_FORMULA += _campodb;

        hideAjaxLoader2();
  
    }
    $scope.addFormula = function (_item, _campodb) {

        showAjaxLoader2();

        if (_item.TCI_FORMULA == null)
            _item.TCI_FORMULA = _campodb;
        else
            _item.TCI_FORMULA += _campodb;

        hideAjaxLoader2();

    }

    $scope.buscarTabelaDinamica = function (_id) {

        $scope.tabref  = [];
        $scope.indices = [];
        $scope.retornos = [];

        for (var ind in $scope.tabconfig.TAB_DINAMICA_CONFIG_ITEM) {
            if ($scope.tabconfig.TAB_DINAMICA_CONFIG_ITEM[ind].TDC_ID_TAB_REF != null) {
                $scope.tabref.push($scope.tabconfig.TAB_DINAMICA_CONFIG_ITEM[ind].TDC_ID_TAB_REF);
                $scope.indices.push($scope.limpaMascara($scope.tabconfig.TAB_DINAMICA_CONFIG_ITEM[ind].TDC_ID_TAB_REF));

            }
        }

        if ($scope.tabref.length == 0)
            return;

        showAjaxLoader2();

        var url = "/TabelaDinamica/BuscarTabelaDinamica";
        $http({
            url: url,
            method: "post",
            data: { _tdc_id: $scope.tabref }
        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {
                for (var ind in response.result) {

                    $scope.retornos[ind] = response.result[ind];
                }
            }
            else
                $scope.lista01 = null;

        }).error(function (response) {

            $scope[_id] = null;

            alert(response);

            hideAjaxLoader2();

        });

    }


    $scope.buscarDescricaoMenu = function (_id) {

        var url = "/TabelaDinamica/BuscarTabelaDinamica";
        $http({
            url: url,
            method: "post",
            data: { _tdc_id: _id }
        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {

                $scope.tabelaref = response.result.tabela;
                tabelaref
                return $sce.trustAsHtml(string);

            }
            else
                return null;

        }).error(function (response) {

            alert(response);

            hideAjaxLoader2();

        });

    }



    $scope.limpaMascara = function (str) {
     
        var novoCPF = null;
        
        if (str != null) {
            var novoCPF = str.replace(/[\.-]/g, "");

            novoCPF = 'B' + novoCPF;
        }

        return novoCPF;
    }

    $scope.listarTabDinamica = function (pageRequest, idtabela) {

        showAjaxLoader2();

        var idtb = $scope.tabela.TDC_ID;

        if (idtabela != null)
            idtb = idtabela;

        var url = "/TabelaDinamica/PesquisarTabDinamica";
        $http({
            url: url,
            method: "post",
            data: { _id: idtb, _pagina: pageRequest, _registroPorPagina: null, _p: $scope.ParamConsulta, _palavrachave: $scope.tabconfig.palavrachave }
        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {

                $scope.tbitem = response.result.tbitem;

                $scope.page = 0;

                if (response.page != null)
                    $scope.page = response.page;

            }
            else {

                $scope.tbitem = null;

                if (response.message != null)
                    $scope.erro = response.message.message;
                else
                    $scope.erro = response;

                angular.element("#Modal-Info").modal();

                hideAjaxLoader2();
            }

        }).error(function (response) {

           
            $scope.tbitem = null;

            $scope.erro = response;

            angular.element("#Modal-Info").modal();

            hideAjaxLoader2();
        });

    }
    $scope.Salvar = function () {

        $http({
            method: 'Post',
            //dataType: 'json',
            url: '/TabelaDinamica/Salvar',
            data: { _id: "", _config: $scope.tabconfig, _tabela: $scope.tabela }

        }).success(function (response) {

            hideAjaxLoader2();

            $scope.erros = response.validationMessage;

            if (response.success == true) {

                $scope.message = Util.createMessage("success", "Operação realizada com sucesso!!");
                $scope.carregaTela($scope.tabconfig.TDC_ID, $scope.tabconfig.TDC_TIPO);
  
            }
            else {
                
                $scope.message = Util.createMessage("fail", response.message.message);
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);

            hideAjaxLoader2();
        });

    }
    $scope.salvarItemTabela = function (_itemTabela, _index) {

        $http({
            method: 'Post',
            //dataType: 'json',
            url: '/TabelaDinamica/SalvarItemTabela',
            data: { _id: $scope.tabela.TDC_ID, _item: _itemTabela }

        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {

                $scope.tbitem[_index] = response.result.retorno; 
                $scope.message = Util.createMessage("success", "Operação realizada com sucesso!!");

                $scope.addItemTabela();
            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);

            hideAjaxLoader2();
        });

    }
    $scope.Excluir = function () {


        if (confirm("Deseja relamente excluir este registro? ")) {

            $http({
                method: 'Post',
                //dataType: 'json',
                url: '/TabelaDinamica/ExcluirTabela',
                data: { _id: $scope.tabela.TDC_ID }

            }).success(function (response) {

                hideAjaxLoader2();

                if (response.success == true) {

                    $scope.message = Util.createMessage("success", "Operação realizada com sucesso!!");
                    window.location = "/TabelaDinamica/Index";
                }
                else {

                    $scope.message = Util.createMessage("fail", response.message.message);
                }

            }).error(function (response) {

                $scope.message = Util.createMessage("fail", response);

                hideAjaxLoader2();
            });
        }

    }
    $scope.excluirItemTabela = function (item) {

        if (confirm("Deseja relamente excluir este registro? ")) {

            $http({
                method: 'Post',
                //dataType: 'json',
                url: '/TabelaDinamica/ExcluirItemTabela',
                data: { _item: item }

            }).success(function (response) {

                hideAjaxLoader2();

                if (response.success == true) {
                    $scope.message = Util.createMessage("success", "Operação realizada com sucesso!!");
                    $scope.listarTabDinamica($scope.page.pagina);
                }
                else {

                    $scope.message = Util.createMessage("fail", response.message.message);
                }

            }).error(function (response) {

                $scope.message = Util.createMessage("fail", response);

                hideAjaxLoader2();
            });
        }

    }
    $scope.buscarusu = function () {

        showAjaxLoader2();

        $http({
            method: 'Post',
            //dataType: 'json',
            url: '/TabelaDinamica/Simuladores',
            data: { login: $scope.login }

        }).success(function (response) {

            hideAjaxLoader2();

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);

            hideAjaxLoader2();
        });

    }

    $scope.importarArquivo = function () {
   
        $scope.StartTimer();

        angular.element('#ImportarXLS').submit();

    }


    $scope.expPlanilha = function () {

        showAjaxLoader2();

        $http({
            method: 'Post',
            //dataType: 'json',
            url: '/TabelaDinamica/ExportarXLS',
            data: { _tdc_id: $scope.export.tdc_id, _nomearquivo: $scope.export.nomearquivo }

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
    $scope.checarUsuarioPortal = function () {

   
        var teste = angular.element('#sessao');
        
        showAjaxLoader2();

        var _url = 'http://portalcoadlinux.apc.intranet/home/checarusuario';

        $.ajax({
            url: _url,
            success: (function (data) {
                alert(data);
            })
        });

        hideAjaxLoader2();
    }
    $scope.calcular = function () {

        var formula = "";
        var campossoma = "";
        var campossubt = "";
        var camposdiv = "";
        var camposmult = "";
        var icms = 0;
        var uforigem = 0;
        var ufdestino = 0;
        var aliqorig = 0;
        var aliqdest = 0;
        var aliqdif = 0;


        var fcp = 0;

        for (var ind in $scope.tabconfig.TAB_DINAMICA_CONFIG_ITEM) {

            formula = $scope.tabconfig.TAB_DINAMICA_CONFIG_ITEM[ind].TCI_FORMULA;

            if (formula != null) {
                $scope.calculado = false;

                if (formula.indexOf("+") > 0)
                    campossoma = formula.split("+")
                else
                    if (formula.indexOf("+") > 0)
                        campossubt = formula.split("-")
                    else
                        if (formula.indexOf("+") > 0)
                            camposdiv = formula.split("/")
                        else
                            if (formula.indexOf("+") > 0)
                                camposmult = formula.split("*")
                            else
                                $scope.tabela.TAB_DINAMICA_ITEM[0][$scope.tabconfig.TAB_DINAMICA_CONFIG_ITEM[ind].TCI_NOME_CAMPODB] =
                                $scope.tabela.TAB_DINAMICA_ITEM[0][$scope.tabconfig.TAB_DINAMICA_CONFIG_ITEM[ind].TCI_FORMULA];

                //var serarador = ["/", "*", "(", ")", "-", "+"];
                var indserarador = [];
                if (campossoma.length > 0) {
                    for (var ind in campossoma) {
                        calculo = Number(calculo) + Number($scope.filtro[campossoma[ind]]);
                    }
                }

            }

        }

        //---------------
        var origsulsudeste = false;
        var destnortenordeste = false;
        //---------------

        //for (var ind in $scope.UFNORTE) 
        //    if ($scope.UFNORTE[ind] == $scope.tabela.TAB_DINAMICA_ITEM[0]["TAB_STRING01"].trim())
        //        orignortesul = true;

        for (var ind in $scope.UFSUL)
            if ($scope.UFSUL[ind] == $scope.tabela.TAB_DINAMICA_ITEM[0]["TAB_STRING01"].trim())
                origsulsudeste = true;

        //-----------

        for (var ind in $scope.UFNORTE)
            if ($scope.UFNORTE[ind] == $scope.tabela.TAB_DINAMICA_ITEM[0]["TAB_STRING02"].trim())
                destnortenordeste = true;

        for (var ind in $scope.UFNORDESTE)
            if ($scope.UFNORDESTE[ind] == $scope.tabela.TAB_DINAMICA_ITEM[0]["TAB_STRING02"].trim())
                destnortenordeste = true;

        if ($scope.tabela.TAB_DINAMICA_ITEM[0]["TAB_STRING02"].trim() == "ES")
            destnortenordeste = true;

        //------------

        if ($scope.tabela.TAB_DINAMICA_ITEM[0]["TAB_INT01"].trim() == "Sim")
            aliqorig = 4;
        else {
            if (origsulsudeste && destnortenordeste)
                aliqorig = 7
            else
                aliqorig = 12;
        }

        if ($scope.tabela.TAB_DINAMICA_ITEM[0]["TAB_INT03"] == null || $scope.tabela.TAB_DINAMICA_ITEM[0]["TAB_INT03"] == "") {
            alert("Informe a aliquota de destino");
            $scope.calculado = false;
            return false;
        }

        aliqdest = $scope.tabela.TAB_DINAMICA_ITEM[0]["TAB_INT03"];

        //---------- Calculo Fixo (Fundo de Combate a Pobreza)
        if ($scope.tabela.TAB_DINAMICA_ITEM[0]["TAB_INT04"] != null && $scope.tabela.TAB_DINAMICA_ITEM[0]["TAB_INT04"] != "") {
            fcp = $scope.tabela.TAB_DINAMICA_ITEM[0]["TAB_INT04"];
            fcp = fcp * Number($scope.tabela.TAB_DINAMICA_ITEM[0]["TAB_INT02"]);
            fcp = fcp / 100;
            $scope.tabela.TAB_DINAMICA_ITEM[0]["TAB_INT09"] = mascaraValor(fcp);
        }
        else
            $scope.tabela.TAB_DINAMICA_ITEM[0]["TAB_INT09"] = mascaraValor(0);
        //----------

        //---------- Calculo do valor do ICMS Origem

        var calculo = $scope.tabela.TAB_DINAMICA_ITEM[0]["TAB_INT02"] * Number(aliqorig / 100);
        $scope.tabela.TAB_DINAMICA_ITEM[0]["TAB_INT10"] = mascaraValor(calculo);

        //---------- 

        //---------- 
        aliqdif = Number(aliqdest) - Number(aliqorig);

        if (aliqdif > 0) {

            icms = aliqdif * Number($scope.tabela.TAB_DINAMICA_ITEM[0]["TAB_INT02"]);
            icms = icms / 100;
            //----------

            //---------- Calculo Fixo (Percentual Por estado)
            if ($scope.tabela.TAB_DINAMICA_ITEM[0]["TAB_INT05"] == 2016) {
                uforigem = Number(icms) * 60 / 100;
                ufdestino = Number(icms) * 40 / 100;
            }
            if ($scope.tabela.TAB_DINAMICA_ITEM[0]["TAB_INT05"] == 2017) {
                uforigem = Number(icms) * 40 / 100;
                ufdestino = Number(icms) * 60 / 100;
            }
            if ($scope.tabela.TAB_DINAMICA_ITEM[0]["TAB_INT05"] == 2018) {
                uforigem = Number(icms) * 20 / 100;
                ufdestino = Number(icms) * 80 / 100;
            }
            if ($scope.tabela.TAB_DINAMICA_ITEM[0]["TAB_INT05"] == 2019) {
                uforigem = 0;
                ufdestino = Number(icms);
            }
        }
        else {

            uforigem = 0;
            ufdestino = 0;
        }


        $scope.tabela.TAB_DINAMICA_ITEM[0]["TAB_INT07"] = mascaraValor(uforigem);
        $scope.tabela.TAB_DINAMICA_ITEM[0]["TAB_INT08"] = mascaraValor(ufdestino);
        $scope.tabela.TAB_DINAMICA_ITEM[0]["TAB_STRING03"] = mascaraValor(aliqdif);
        $scope.tabela.TAB_DINAMICA_ITEM[0]["TAB_STRING04"] = mascaraValor(icms);

        //----------

        $scope.calculado = true;

        //-----------


        var url = "/TabelaDinamica/RegistrarLogCalculo";
        $http({
            url: url,
            method: "post",
            data: { _tipoacesso: "C", _tdc_id: $scope.tabconfig.TDC_ID }
        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {

                hideAjaxLoader2();

            }
            else {

                alert(response.message.message);

                $scope.message = Util.createMessage("fail", response.message.message);

                hideAjaxLoader2();
            }

        }).error(function (response) {

            alert(response);

            $scope.message = Util.createMessage("fail", response);

            hideAjaxLoader2();

        });


    }
    $scope.calcularOpe = function () {

        showAjaxLoader2();

        $scope.erroCalc = false;

        try {

            for (var ind in $scope.tabconfig.TAB_DINAMICA_CONFIG_ITEM) {

                var _decimais = $scope.tabconfig.TAB_DINAMICA_CONFIG_ITEM[ind].TCI_DECIMAIS;
                var _frmCalculo = $scope.tabconfig.TAB_DINAMICA_CONFIG_ITEM[ind].TCI_FORMULA;
                var _campofromula = $scope.tabconfig.TAB_DINAMICA_CONFIG_ITEM[ind].TCI_NOME_CAMPODB;
                var _campotipo = $scope.tabconfig.TAB_DINAMICA_CONFIG_ITEM[ind].TCI_TIPO_CAMPO;

                if (_frmCalculo != null) {
                    for (var ind2 in $scope.tabconfig.TAB_DINAMICA_CONFIG_ITEM) {

                        var _valorcampotabela = "";
                        var _campotabela = $scope.tabconfig.TAB_DINAMICA_CONFIG_ITEM[ind2].TCI_NOME_CAMPODB;
                        _valorcampotabela = $scope.tabela.TAB_DINAMICA_ITEM[0][_campotabela];

                        //----------
                        if (_valorcampotabela == null || _valorcampotabela == "")
                            _valorcampotabela = 0;

                        if (_valorcampotabela < 0)
                            _valorcampotabela = "(" + _valorcampotabela + ")";
                        //----------
                        if (isNaN(_valorcampotabela))
                            _valorcampotabela = _valorcampotabela.trim();

                        if (_valorcampotabela != "undefined") {

                            var _strbusca = eval('/' + _campotabela + '/g');

                            _frmCalculo = _frmCalculo.replace(_strbusca, _valorcampotabela);
                        }
                    }

                    //--------------
                    var _calc = "";
                    var _calc1 = "";
                    var _arredondado  = "";
                    var _arrayformula = _frmCalculo.split(";");

                    if (_arrayformula != null) {
                        for (var ind2 in _arrayformula) {
                            if (_arrayformula[ind2] != null && _arrayformula[ind2] != "") {

                                var _vlrCalcTemp = eval(_arrayformula[ind2]);

                                _calc = $scope.filterFloat(_calc) + $scope.filterFloat(_vlrCalcTemp);

                            }
                             
                        }

                        var _valorcalc = $scope.filterFloat(_calc);
                      
                        if (_valorcalc > 0) {

                            if (_calc == null)
                                _calc = 0;

                            if (_decimais > 0)
                                _arredondado = _valorcalc.toFixed(_decimais);
                            else
                                _arredondado = _valorcalc.toFixed(2);
                       
                        }
                        else
                            _arredondado = _calc;

                    }
                    else {
                     
                        var _resultado = $scope.$eval(_frmCalculo);

                        _arredondado = _resultado;
                           
                    }

                    $scope.tabela.TAB_DINAMICA_ITEM[0][_campofromula] = _arredondado.toString();

                }
            }

            
            $scope.calculado = ($scope.erroCalc == false);
            
            var url = "/TabelaDinamica/RegistrarLogCalculo";
            $http({
                url: url,
                method: "post",
                data: { _tipoacesso: "C", _tdc_id: $scope.tabconfig.TDC_ID }
            }).success(function (response) {

                hideAjaxLoader2();

                if (response.success == true) {

                    hideAjaxLoader2();

                }
                else {

                    alert(response.message.message);

                    $scope.message = Util.createMessage("fail", response.message.message);

                    hideAjaxLoader2();
                }

            }).error(function (response) {

                alert(response);

                $scope.message = Util.createMessage("fail", response);

                hideAjaxLoader2();

            });

            hideAjaxLoader2();

        }
        catch (err) {
            //$scope.message = Util.createMessage("fail", err.message);
            alert("Erro ao realizar calculo (" + err.message + ")");
            hideAjaxLoader2();
        }



    }
    $scope.montarListas = function () {

        var lista = "";

        for (var ind in $scope.tabconfig.TAB_DINAMICA_CONFIG_ITEM) {

            lista = $scope.tabconfig.TAB_DINAMICA_CONFIG_ITEM[ind].TCI_VALOR_ESPERADO;

            if (lista != null) {
                if (lista.indexOf(";")) {
                    $scope.listas[$scope.tabconfig.TAB_DINAMICA_CONFIG_ITEM[ind].TCI_NOME_CAMPODB] = lista.split(";");
                }
            }
        }


    }
    $scope.limparCalculo = function () {
        $scope.calculado = false;
    }
    $scope.mostraOrientacao = function (tipo) {
        $scope.orientacao = tipo;
    }
    $scope.mostrarTabReferencia = function (idtabela, nomecampo) {

        showAjaxLoader2();

        $scope.ParamConsulta = [];

        var url = "/TabelaDinamica/MostrarInfAdicionais";

        $http({
            url: url,
            method: "post",
            data: { _id: idtabela }
        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {
                $scope.tabref = response.result.tabref;
                $scope.tabrefcfg = response.result.tabrefcfg;

                //--Encontrando o campo de pesquisa, caso exista algum campo selecionado.
                angular.forEach($scope.tabrefcfg.TAB_DINAMICA_CONFIG_ITEM, function (cabec, index) {

                    if (cabec.TCI_CAMPO_PESQUISA == true) {
                        var _param = {};
                        _param.label = cabec.TCI_NOME_CAMPO;
                        _param.campo = cabec.TCI_NOME_CAMPODB;
                        _param.esperado = cabec.TCI_VALOR_ESPERADO;
                        _param.valor = "";
                        $scope.ParamConsulta.push(_param);
                    }

                });


                $scope.page = 0;

                if (response.page != null)
                    $scope.page = response.page;
            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
                $scope.tabref = null;
                hideAjaxLoader2();
            }

            $scope.nometelapesquisa = nomecampo;

            $scope.listarTabDinamica(1, idtabela);

            angular.element("#modal-inf-adicional").modal();

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);
            $scope.tabref = null;
            $scope.tabrefcfg = null;

            hideAjaxLoader2();
        });

    }

    $scope.mostraInfAdicionais = function (idtabela, nomecampo, abrir, pageRequest) {

        if ($scope.tabela.TAB_DINAMICA_ITEM[0]["TAB_STRING02"] == null || $scope.tabela.TAB_DINAMICA_ITEM[0]["TAB_STRING02"] == '') {
            for (var ind in $scope.tabconfig.TAB_DINAMICA_CONFIG_ITEM) {

                if ($scope.tabconfig.TAB_DINAMICA_CONFIG_ITEM[ind].TCI_NOME_CAMPODB == "TAB_STRING02") {

                    alert("Preencha o campo (" + $scope.tabconfig.TAB_DINAMICA_CONFIG_ITEM[ind].TCI_NOME_CAMPO + "), para ter acesso a informações adicionais sobre este campo.");

                    return
                }
            }
        }

        if (idtabela == null)
            idtabela = $scope.idtabela;
        else
            $scope.idtabela =  idtabela;


        showAjaxLoader2();

        var url = "/TabelaDinamica/MostrarInfAdicionais";

        $http({
            url: url,
            method: "post",
            data: {
                _id: idtabela,
                _uf: $scope.tabela.TAB_DINAMICA_ITEM[0]["TAB_STRING02"]
               , pagina: pageRequest
            }
        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {
                $scope.tabref = response.result.tabref;
                $scope.tabrefitem = response.result.tabrefitem;
                $scope.tabrefcfg = response.result.tabrefcfg;

                $scope.search.TAB_STRING01 = $scope.tabela.TAB_DINAMICA_ITEM[0]["TAB_STRING02"].trim();

                if (nomecampo!=null)
                    $scope.nometelapesquisa = nomecampo;

                conversionService.deepConversion($scope.tabref);
                conversionService.deepConversion($scope.tabrefitem);
                conversionService.deepConversion($scope.tabrefcfg);

                if (abrir == null)
                    angular.element("#modal-consulta").modal();

                $scope.page = response.page;

            }
            else {

                if (response.message != null)
                    $scope.message = response.message;
                else
                    $scope.message = Util.createMessage("fail", response); 
                
                $scope.tabref = null;

                hideAjaxLoader2();
            }


        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);
            $scope.tabref = null;
            $scope.tabrefcfg = null;

            hideAjaxLoader2();
        });

    }
    $scope.publicar = function () {

        $http({
            method: 'Post',
            url: '/TabelaDinamica/Publicar',
            data: { _config: $scope.tabconfig }

        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {

                $scope.message = Util.createMessage("success", "Tabela/Simulador publicado com sucesso !!");

            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);

            hideAjaxLoader2();
        });

    }
    $scope.removerPublicacao = function () {

        $http({
            method: 'Post',
            url: '/TabelaDinamica/RemoverPublicacao',
            data: { _config: $scope.tabconfig }

        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {

                $scope.message = Util.createMessage("success", "Publicação removida com sucesso !!");

            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);

            hideAjaxLoader2();
        });

    }
    $scope.formatarData = function (_data) {

        var data = new Date(_data);
        var dia = data.getDate();
        if (dia.toString().length == 1)
            dia = "0" + dia;
        var mes = data.getMonth() + 1;
        if (mes.toString().length == 1)
            mes = "0" + mes;
        var ano = data.getFullYear();

        return dia + "/" + mes + "/" + ano;

    }
    function dataAtualFormatada(jsDate) {
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

    function dataHoraFormatada(jsDate) {
        var data = jsDate;
        var dia = data.getDate();
        if (dia.toString().length == 1)
            dia = "0" + dia;
        var mes = data.getMonth() + 1;
        if (mes.toString().length == 1)
            mes = "0" + mes;
        var ano = data.getFullYear();

        var hora = data.getHours();  
        var min = data.getMinutes(); 
        var seg = data.getSeconds(); 
        var hora = hora + ':' + min + ':' + seg;


        return dia + "/" + mes + "/" + ano +" "+ hora;
    }

    function mascaraValor(number) {

        var numero = (number.toFixed(2).toString().split("."));

        var v = "";
        v = numero[0] + numero[1];
        v = v.replace(/\D/g, "") // permite digitar apenas numero 
        v = v.replace(/(\d{1})(\d{15})$/, "$1.$2") // coloca ponto antes dos ultimos 15 digitos 
        v = v.replace(/(\d{1})(\d{11})$/, "$1.$2") // coloca ponto antes dos ultimos 11 digitos 
        v = v.replace(/(\d{1})(\d{8})$/, "$1.$2") // coloca ponto antes dos ultimos 8 digitos 
        v = v.replace(/(\d{1})(\d{5})$/, "$1.$2") // coloca ponto antes dos ultimos 5 digitos 
        v = v.replace(/(\d{1})(\d{1,2})$/, "$1,$2") // coloca virgula antes dos ultimos 2 digitos 

        return v;
    }

    $scope.ConvertDateJSON = function (jsondata) {

        var data = null;

        if (jsondata != null)
            data = new Date(parseInt(jsondata.substr(6)));

        return data;
    }

    //---- Calculo de Datas
    
    $scope.anoBissexto = function (ano) {
        return new Date(ano, 1, 29).getMonth() == 1
    }
    $scope.addProxMes = function (date1, diaini) {

        var dia = date1.getDate();
        var mes = date1.getMonth();
        var ano = date1.getFullYear();
        var dataretorno = new Date();
        
        mes += 1;
        
        if (mes == 12) {
            mes = 0;
            ano += 1;
        }

        if (mes != 1 && dia > 30) {
            if (mes in [3, 5, 8, 10]) {
                dataretorno = new Date(ano, mes, 30);
            }
            else {
                dataretorno = new Date(ano, mes, diaini);
            }
        } else if (mes == 1 && dia > 27) {

            if ($scope.anoBissexto(ano))
                dia = 29;
            else
                dia = 28;

            dataretorno = new Date(ano, mes, dia);

        } else {


            dataretorno = new Date(ano, mes, diaini);

        }

        return dataretorno;
    }
    $scope.dataDiff = function (dataini, datafim) {

        $scope.erroCalc = false;

        var ONE_DAY = 1000 * 60 * 60 * 24

        //-----
        var dt01 = dataini.split("/");
        var dt02 = datafim.split("/");
        //-----

        var mes = parseInt(dt01[1]) - 1;
        var mesfim = parseInt(dt02[1]) - 1;
        var date1 = new Date(dt01[2], mes, dt01[0]);
        var date2 = new Date(dt02[2], mesfim, dt02[0]);

        date2.setDate(date2.getDate() + 1);

        if (date2 < date1) {
            alert("Período inválido!! Data inicial maior que data final!!");
            $scope.erroCalc = true;
            return 0;
        }


        var diffDays = Math.abs(date2 - date1)

        diffDays = Math.round(diffDays / ONE_DAY);

        return diffDays;

    }
    $scope.buscarDataDias = function(dataini, dias) {

        //-----

        var dt01 = dataini.split("/");

        var mes = parseInt(dt01[1]) - 1;

        var date1 = new Date(dt01[2], mes, dt01[0]);

        date1.setDate(date1.getDate() + parseInt(dias));

        var dia = date1.getDate();

        if (dia.toString().length == 1)
            dia = "0" + dia;

        var mes = date1.getMonth() + 1;

        if (mes.toString().length == 1)
            mes = "0" + mes;

        var ano = date1.getFullYear();

        return dia + "/" + mes + "/" + ano;


    }
    $scope.buscarDataMeses = function(dataini, meses) {

        //-----

        var dt01 = dataini.split("/");

        var mes = parseInt(dt01[1]) - 1;

        var date1 = new Date(dt01[2], mes, dt01[0]);

        date1.setMonth(date1.getMonth() + parseInt(meses));

        var dia = date1.getDate();

        if (dia.toString().length == 1)
            dia = "0" + dia;

        var mes = date1.getMonth() + 1;

        if (mes.toString().length == 1)
            mes = "0" + mes;

        var ano = date1.getFullYear();

        return dia + "/" + mes + "/" + ano;


    }
    $scope.dataDiffMes = function(dataini, datafim) {

        $scope.erroCalc = false;

        var anos = 0;
        var meses = 0;
        var dias = 0;
        var ONE_DAY = 1000 * 60 * 60 * 24

        //-----
        var dt01 = dataini.split("/");
        var dt02 = datafim.split("/");

        //-----
        var diaini = dt01[0];
        var mes = parseInt(dt01[1]) - 1;
        var mesfim = parseInt(dt02[1]) - 1;
        var date1 = new Date(dt01[2], mes, dt01[0]);
        var date2 = new Date(dt02[2], mesfim, dt02[0]);
        var datefim = new Date(dt02[2], mesfim, dt02[0]);
        var dateant = new Date();


        if (date2 < date1) {
            alert("Período inválido!! Data inicial maior que data final!!");
            $scope.erroCalc = true;
            return 0;
        }


        ////-----
        mes += 1;

        datefim.setDate(datefim.getDate() + 1);

        while (date1 <= datefim) {

            date1 = $scope.addProxMes(date1, diaini);

            if (date1 <= datefim) {

                dateant = new Date(date1);

                meses += 1;

            }

        }

        return meses <= 0 ? 0 : meses;

    }
    $scope.dataDiffMesFolha = function(dataini, datafim, fracao) {

        $scope.erroCalc = false;

        var anos = 0;
        var meses = 0;
        var dias = 0;
        var ONE_DAY = 1000 * 60 * 60 * 24

        //-----
        var dt01 = dataini.split("/");
        var dt02 = datafim.split("/");

        //-----
        var diaini = dt01[0];
        var mes = parseInt(dt01[1]) - 1;
        var mesfim = parseInt(dt02[1]) - 1;
        var date1 = new Date(dt01[2], mes, dt01[0]);
        var date2 = new Date(dt02[2], mesfim, dt02[0]);
        var datefim = new Date(dt02[2], mesfim, dt02[0]);
        var dateant = new Date();


        if (date2 < date1) {
            alert("Período inválido!! Data inicial maior que data final!!");
            $scope.erroCalc = true;
            return 0;
        }

        
        ////-----
        mes += 1;

        datefim.setDate(datefim.getDate() + 1);

        while (date1 <= datefim) {

            date1 = $scope.addProxMes(date1, diaini);

            if (date1 <= datefim) {

                dateant = new Date(date1);

                meses += 1;

            }
        }

        if (datefim >= dateant)
            dias = Math.abs(datefim - dateant);
        else
            dias = 0;

        dias = Math.round(dias / ONE_DAY);

        if (dias >= fracao)
            meses += 1;

        return meses <= 0 ? 0 : meses;

    }
    $scope.dataDiffAno = function(dataini, datafim) {
       
        var anos = 0;
        var meses = 0;
        var dias = 0;
        var ONE_DAY = 1000 * 60 * 60 * 24

        //-----
        var dt01 = dataini.split("/");
        var dt02 = datafim.split("/");

        //-----
        var diaini = dt01[0];
        var mes = parseInt(dt01[1]) - 1;
        var mesfim = parseInt(dt02[1]) - 1;
        var date1 = new Date(dt01[2], mes, dt01[0]);
        var date2 = new Date(dt02[2], mesfim, dt02[0]);
        var datefim = new Date(dt02[2], mesfim, dt02[0]);
        var dateant = new Date();

        ////-----

        mes += 1;

        datefim.setDate(datefim.getDate() + 1);

        while (date1 <= datefim) {

 
            date1 = $scope.addProxMes(date1, diaini);

            if (date1 <= datefim) {

                dateant = new Date(date1);

                meses += 1;

                if ((meses % 12) == 0)
                    anos += 1;
            }
        }

        return anos <= 0 ? 0 : anos;

    }
    $scope.dataDiffAnoMesDia = function(dataini, datafim) {

        var anos = 0;
        var meses = 0;
        var dias = 0;
        var ONE_DAY = 1000 * 60 * 60 * 24

        //-----
        var dt01 = dataini.split("/");
        var dt02 = datafim.split("/");

        //-----
        var diaini = dt01[0];
        var mes = parseInt(dt01[1]) - 1;
        var mesfim = parseInt(dt02[1]) - 1;
        var date1 = new Date(dt01[2], mes, dt01[0]);
        var date2 = new Date(dt02[2], mesfim, dt02[0]);
        var datefim = new Date(dt02[2], mesfim, dt02[0]);
        var dateant = new Date();

        ////-----
        mes += 1;
 
        datefim.setDate(datefim.getDate() + 1);

        dateant = new Date(date1);

        while (date1 < datefim) {

  
            date1 = $scope.addProxMes(date1, diaini);

            if (date1 <= datefim) {

                dateant = new Date(date1);

                meses += 1;

                if ((meses % 12) == 0) {
                    anos += 1;
                    meses = 0;
                }
            }
        }

        if (datefim >= dateant)
            dias = Math.abs(datefim - dateant);
        else
            dias = 0;

        dias = Math.round(dias / ONE_DAY);

        var _retorno = "";

        if (anos > 0) {
            if (anos > 1)
                _retorno = anos + " anos ";
            else
                _retorno = anos + " ano ";

            if (meses > 0 )
                _retorno +=  ", ";

        }

        if (meses > 0) {
            if (meses > 1)
                _retorno += meses + " meses ";
            else
                _retorno += meses + " mês ";

        }

        if (dias > 0) {

            if (meses > 0 || anos > 0)
                _retorno += " e ";

            if (dias > 1)
                _retorno += dias + " dias ";
            else
                _retorno += dias + " dia ";

        }


        return _retorno;

    }
    $scope.dataDiffDiasUteis = function(dataini, datafim) {

        diffDays = $scope.dataDiff(dataini, datafim);

        //-----
        var ind = 0;
        var dt01 = dataini.split("/");
        var dt02 = datafim.split("/");

        //-----
        var naouteis = 0;
        var mes = parseInt(dt01[1]) - 1;
        var mesfim = parseInt(dt02[1]) - 1;
        var date1 = new Date(dt01[2], mes, dt01[0]);
        var date2 = new Date(dt02[2], mesfim, dt02[0]);
        var date3 = new Date(dt01[2], mes, dt01[0]);

        while (date3 < date2) {

            if (date3.getDay() == 0 || date3.getDay() == 6)
            {
                naouteis += 1;
            }

            date3.setDate(date3.getDate() + 1);

        }

        diffDays -= naouteis;

        return diffDays;

    }

    $scope.isNumber = function isNumber(n) {
        return !isNaN(parseFloat(n)) && isFinite(n);
    }

    $scope.checkJsonParse = function (jsonString) {

        return new Promise(function (resolve, reject) {
            try {
                var obj = JSON.parse(jsonString);
                resolve(obj);
            }
            catch (err) {
                reject(err);
            }
        });
         
    }


    $scope.dataDiffDiasUteisFeriado = function (dataini, datafim) {

        showAjaxLoader2();

        //-----
        var dt01 = dataini.split("/");
        var dt02 = datafim.split("/");

        //-----
        var diaini = dt01[0];
        var mes = parseInt(dt01[1]) - 1;
        var mesfim = parseInt(dt02[1]) - 1;
        var date1 = new Date(dt01[2], mes, dt01[0]);
        var date2 = new Date(dt02[2], mesfim, dt02[0]);
        //-----

        $scope.qtdeferiado = 0;

        var url = "/Feriado/BuscarDiasUteisFeriado";

        $http({
            url: url,
            method: "post",
            data: {
                _dataini: date1,
                _datafim: date2
            }

        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {

                $scope.qtdeferiado = response.result.qtdeferiado;

                $scope.tabela.TAB_DINAMICA_ITEM[0]["RET_FERIADOS"] = $scope.qtdeferiado;
            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
                $scope.qtdeferiado = 0;

                hideAjaxLoader2(); 
            }

            return $scope.qtdeferiado;


        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);

            $scope.qtdeferiado = 0;

            hideAjaxLoader2();

            return $scope.qtdeferiado;

        });


    }

    //---- Fim Calculo de Datas
    //----
    //---- Timer start function.
    $scope.StartTimer = function () {

        $scope.Timer = $interval(function () {
            $scope.contadorProcessamento();
        }, 1000);

    };

    //---- Timer stop function.
    $scope.StopTimer = function () {

        if (angular.isDefined($scope.Timer)) {
            $interval.cancel($scope.Timer);
        }
    };

    if (window.SlideController != undefined) {
        SlideController($scope)
    }



});
