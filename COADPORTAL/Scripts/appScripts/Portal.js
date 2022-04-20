appModule.controller('PortalController', function ($scope, formHandlerService, $http, $interval, conversionService, $sce, mask) {

    $scope.usuario = {};
    $scope.usuario.login = "";
    $scope.usuario.senha = "";
    $scope.MES = {};
    $scope.TDC_ID = {};
    $scope.filtro = {};
    $scope.filtro.TIPO = 0;
    $scope.export = {};
    $scope.cadastro = {};
    $scope.cartao = {};
    $scope.cartao.formapgto = 2;
    $scope.tipoperiodo = 1;
    $scope.cartao.numeroParcelas = null;


    $scope.trustAsHtml = function (string) {
        return $sce.trustAsHtml(string);
    };

    $scope.initDadosConclusao = function (_nome, _boleto) {

        $scope.boleto = _boleto;
        $scope.comprador = {};
        $scope.comprador.nome = _nome;

    }

    $scope.mask = function (valor) {
        valor = mask('mask_dinheiro')(valor);
        return valor;
    }

    $scope.ImpEtiqueta = function () {

        var url = Util.getUrl("/AcessoTabelas/Imprimir");

        post(url);

    }

    $scope.carregarTela = function () {

        $scope.filtro.mes = data.getMonth() + 1;
        $scope.filtro.ano = data.getFullYear();

    }
    $scope.buscarNoticia = function (id) {

        showAjaxLoader();

        var url = "/Noticia/CarregarTela";
        $http({
            url: url,
            method: "post",
            data: { _id: id }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.noticia = response.result.noticia;
                //---------
                if ($scope.noticia.DATA_ALTERA != null)
                    $scope.noticia.DATA_ALTERA = $scope.ConvertDateJSON($scope.noticia.DATA_ALTERA);
                if ($scope.noticia.DATA_CADASTRO != null)
                    $scope.noticia.DATA_CADASTRO = $scope.ConvertDateJSON($scope.noticia.DATA_CADASTRO);
                if ($scope.noticia.DATA_PUBLICACAO != null)
                    $scope.noticia.DATA_PUBLICACAO = $scope.ConvertDateJSON($scope.noticia.DATA_PUBLICACAO);

            }
            else {


                if (response.message != null)
                    $scope.erro = response.message.message;
                else
                    $scope.erro = response;

                angular.element("#Modal-Erro").modal();

                $scope.noticia = null;

                hideAjaxLoader();
            }

        }).error(function (response) {

            $scope.erro = response;

            angular.element("#Modal-Erro").modal();

            $scope.noticia = null;

            hideAjaxLoader();
        });

    }
    $scope.buscarTabela = function (id) {

        showAjaxLoader();

        var url = "/Tabela/CarregarTela";
        $http({
            url: url,
            method: "post",
            data: { _id: id}
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.tabconfig = response.result.tabconfig;
                $scope.tabela = response.result.tabela;
                $scope.tbitem = response.result.tbitem;
                //---------
                $scope.tabela.TAB_DATA_ALTERA = $scope.ConvertDateJSON($scope.tabela.TAB_DATA_ALTERA);
                $scope.tabela.TAB_DATA_INCLUSAO = $scope.ConvertDateJSON($scope.tabela.TAB_DATA_INCLUSAO);
                $scope.tabconfig.TDC_DATA_ALTERA = $scope.ConvertDateJSON($scope.tabconfig.TDC_DATA_ALTERA);
                $scope.tabconfig.TDC_DATA_INCLUSAO = $scope.ConvertDateJSON($scope.tabconfig.TDC_DATA_INCLUSAO);
                $scope.tabconfig.TDC_DATA_PUBLICACAO = $scope.ConvertDateJSON($scope.tabconfig.TDC_DATA_PUBLICACAO);

                for (var ind in $scope.tabela.TAB_DINAMICA_ITEM) {
                    $scope.tabela.TAB_DINAMICA_ITEM[ind].TAB_DATA_ALTERA = $scope.ConvertDateJSON($scope.tabela.TAB_DINAMICA_ITEM[ind].TAB_DATA_ALTERA);
                    $scope.tabela.TAB_DINAMICA_ITEM[ind].TAB_DATA_INCLUSAO = $scope.ConvertDateJSON($scope.tabela.TAB_DINAMICA_ITEM[ind].TAB_DATA_INCLUSAO);
                }

                for (var ind in $scope.tbitem) {
                    $scope.tbitem[ind].TAB_DATA_ALTERA = $scope.ConvertDateJSON($scope.tbitem[ind].TAB_DATA_ALTERA);
                    $scope.tbitem[ind].TAB_DATA_INCLUSAO = $scope.ConvertDateJSON($scope.tbitem[ind].TAB_DATA_INCLUSAO);
                }


                if (response.page != null)
                    $scope.page = response.page;

            }
            else {

                if (response.message != null)
                    $scope.erro = response.message.message;
                else
                    $scope.erro = response;

                angular.element("#Modal-Erro").modal();

                $scope.listatabela = null;

                hideAjaxLoader();
            }

        }).error(function (response) {

            $scope.erro = response;

            angular.element("#Modal-Erro").modal();

            $scope.listatabela = null;

            hideAjaxLoader();
        });

    }
    $scope.buscarTipoPagamento = function (_tpg_id) {

        showAjaxLoader();

        var url = "/Cadastro/BuscarTipoPagamento";
        $http({
            url: url,
            method: "post",
            data: { cmp_id: $scope.comprador.cmp_id , tpg_id: _tpg_id }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.tipopagamento = response.result.tipopagamento;
                var _item = $scope.tipopagamento[0]
                $scope.cartao.valortela = _item.ValorParcela;
                $scope.cartao.numeroParcela = _item.Parcela;
            }
            else {

                if (response.message != null)
                    $scope.erro = response.message.message;
                else
                    $scope.erro = response;

                angular.element("#Modal-Erro").modal();

                $scope.tipopagamento = null;

                hideAjaxLoader();
            }

        }).error(function (response) {

            $scope.erro = response;

            angular.element("#Modal-Erro").modal();

            $scope.listatabela = null;

            hideAjaxLoader();
        });

    }
    $scope.mostrarParcelamento = function () {

        angular.element("#Modal-Parcelamento").modal();
    }

    $scope.enviarPagamento = function () {

        showAjaxLoader();

        var url = "/Cadastro/EnviarDadosPagamento";
        $http({
            url: url,
            method: "post",
            data: { _comprador: $scope.comprador, _cartao: $scope.cartao }
        }).success(function (response) {
            hideAjaxLoader();

            if (response.success == true) {

                //var _urlpagamento = response.result.urlpagamento;

                url = Util.getUrl('/Cadastro/Pagamento');
                post(url);

            }
            else {

                $scope.erro = response.message.message;

                angular.element("#Modal-Erro").modal();
                
                hideAjaxLoader();

                $scope.button = "reset";
            }

        }).error(function (response) {

            $scope.erro = response;

            angular.element("#Modal-Erro").modal();

            hideAjaxLoader();

            $scope.button = "reset";

        });

    }

    $scope.listarTabDinamica = function (pageRequest, idtabela) {

        showAjaxLoader();

        var idtb = $scope.tabela.TDC_ID;

        if (idtabela != null)
            idtb = idtabela;

        var url = "/Tabela/PesquisarTabDinamica";
        $http({
            url: url,
            method: "post",
            data: { _id: idtb, _pagina: pageRequest, _registroPorPagina: null, _p: $scope.ParamConsulta }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.tbitem = response.result.tbitem;

                $scope.page = 0;

                if (response.page != null)
                    $scope.page = response.page;

            }
            else {

                $scope.erro = response.message.message;

                angular.element("#Modal-Erro").modal();

                $scope.listatabela = null;

                hideAjaxLoader();
            }

        }).error(function (response) {

            $scope.erro = response;

            angular.element("#Modal-Erro").modal();

            $scope.listatabela = null;

            hideAjaxLoader();
        });

    }
    $scope.listar = function (pageRequest) {

        showAjaxLoader();

        var idtabela = $scope.filtro.TDC_ID;

        if ($scope.filtro.TIPO == 2) {
            idtabela = null;
        }


        var url = "/Tabela/Pesquisar";
        $http({
            url: url,
            method: "post",
            data: { _mes: $scope.filtro.mes, _ano: $scope.filtro.ano, _tdc_id: idtabela, _tipo: $scope.filtro.TIPO }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {
                $scope.listaAcessoTabelas = response.result.listaAcessoTabelas;
            }
            else {

                $scope.erro = response.message.message;

                angular.element("#Modal-Erro").modal();

                $scope.listaAcessoTabelas = null;

            }

        }).error(function (response) {

            $scope.erro = response;

            angular.element("#Modal-Erro").modal();

            hideAjaxLoader();
        })

    }
    $scope.exportar = function () {

        showAjaxLoader();

        var url = "/AcessoTabelas/ExportarResumo";
        var idtabela = $scope.filtro.TDC_ID;

        if ($scope.filtro.TIPO == 1) {
            url = "/AcessoTabelas/ExportarLista";
        }

        if ($scope.filtro.TIPO == 2) {
            url = "/AcessoTabelas/ExportarLista";
            _idtabela = null;
        }

        $http({
            url: url,
            method: "post",
            data: { _mes: $scope.filtro.mes, _ano: $scope.filtro.ano, _tdc_id: _idtabela }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.export.lnkPath = response.result.retorno;

            }
            else {

                $scope.erro = response.message.message;

                angular.element("#Modal-Erro").modal();

                $scope.export.lnkPath = null;

            }

        }).error(function (response) {

            $scope.erro = response;

            angular.element("#Modal-Erro").modal();

            hideAjaxLoader();
        })

    }
    $scope.init = function (_oac_id) {

        showAjaxLoader();

        var url = "/Portal/BuscarWidgets";
  
        //if (origem == 1) {
        //    url = "/AcessoTabelas/ExportarLista";
        //}

        //if (origem == 2) {
        //    url = "/AcessoTabelas/ExportarLista";
        //    _idtabela = null;
        //}

        $http({
            url: url,
            method: "post",
            data: { _origem: _oac_id}
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.lstwidgetsd = response.result.lstwidgetsd;
                $scope.lstwidgetse = response.result.lstwidgetse;
                $scope.lstwidgetsc = response.result.lstwidgetsc;

                //-----------Carregando Widgets
                $scope.lstAcessoTabelas = response.result.lstAcessoTabelas;

            }
            else {

                $scope.erro = response.message.message;

                angular.element("#Modal-Erro").modal();

                $scope.export.lnkPath = null;

            }

        }).error(function (response) {

            $scope.erro = response;

            angular.element("#Modal-Erro").modal();

            hideAjaxLoader();
        })

    }
    $scope.initPlanos = function (tipo) {

        $scope.tabela01 = null;

        var url = "/Cadastro/BuscarTabelaPreco";

        $scope.mask = function (valor) {

            valor = mask('mask_dinheiro')(valor);
            return valor;
        };

        if (tipo == null)
            tipo = 1;

        $http({
            url: url,
            method: "post",
            data: { _ttp_id: tipo }
        }).success(function (response) {

            if (response.success == true) {

                $scope.tabela01 = response.result.tabela01;
                $scope.tabela02 = response.result.tabela02;
                $scope.tabela03 = response.result.tabela03;

                //-----------Selecionando a Combo;

                $scope.tabelaPreco  = $scope.tabela01[0];
                $scope.tabelaPreco1 = $scope.tabela02[0];
                $scope.tabelaPreco2 = $scope.tabela03[0];

                $scope.tipoperiodo = tipo;
   
            }
            else {

                $scope.erro = response.message.message;

                angular.element("#Modal-Erro").modal();

           }

        }).error(function (response) {

            $scope.erro = response;

            angular.element("#Modal-Erro").modal();

        })

    }
    $scope.initDadosFaturamento = function () {

        showAjaxLoader();

        var url = "/Cadastro/DadosFaturamentoInit";
         

        $http({
            url: url,
            method: "post",
            //data: { _origem: _oac_id }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.tabelaSelecionada = response.result.tabelaSelecionada;
                $scope.comprador = response.result.comprador;

                for (var ind in $scope.tabelaSelecionada) {
                    if ($scope.comprador.cmp_id == $scope.tabelaSelecionada[ind].CMP_ID) {
                        $scope.produto = $scope.tabelaSelecionada[ind];
                    }
                }

                $scope.buscarPreco($scope.produto);

            }
            else {

                $scope.erro = response.message.message;

                angular.element("#Modal-Erro").modal();
            }

        }).error(function (response) {

            $scope.erro = response;

            angular.element("#Modal-Erro").modal();

            hideAjaxLoader();
        })

    }
    $scope.buscarPreco = function (item) {

        $scope.comprador.valor = item.RTP_PRECO_VENDA;
        $scope.comprador.cmp_id = item.CMP_ID;
        $scope.cartao.cmpDescricao = item.CMP_DESCRICAO;
        $scope.cartao.cmpQuantidade = 1;

    }
    $scope.initDadosPagamento = function () {

        showAjaxLoader();

        var url = "/Cadastro/DadosPagamentoInit";

        $http({
            url: url,
            method: "post",
            //data: { _origem: _oac_id }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.comprador = response.result.comprador;
                $scope.cartao = response.result.cartao;

                //$scope.cartao = {};
                $scope.cartao.formapgto = 2;
                $scope.cartao.valor = $scope.comprador.valor;
                $scope.cartao.endereco = $scope.comprador.endereco;
                $scope.cartao.numero = $scope.comprador.numero;
                $scope.cartao.complemento = $scope.comprador.complemento;
                $scope.cartao.bairro = $scope.comprador.bairro;
                $scope.cartao.CEP = $scope.comprador.CEP;
                $scope.cartao.cidade = $scope.comprador.cidade;
                $scope.cartao.UF = $scope.comprador.UF;
                $scope.cartao.email = $scope.comprador.email;
                $scope.cartao.numeroParcelas = null;
                


                $scope.buscarTipoPagamento(9);

            }
            else {

                $scope.erro = response.message.message;

                angular.element("#Modal-Erro").modal();

            }

        }).error(function (response) {

            $scope.erro = response;

            angular.element("#Modal-Erro").modal();

            hideAjaxLoader();
        })

    }
    $scope.buscarCep = function () {

        if ($scope.comprador) {
            var _cep_id = $scope.comprador.CEP;

            if (_cep_id && _cep_id.length >= 8) {

                showAjaxLoader();

                var url = "/Cadastro/BuscarCep";
                $http({
                    url: url,
                    method: "post",
                    data: { _cep_numero: _cep_id }
                }).success(function (retorno) {

                    hideAjaxLoader();
                    
                    if (retorno.success) {
                        if (retorno.result.cep.CEP_NUMERO == null) {
                            $scope.comprador.bairro = null;
                            $scope.comprador.cidade = null;
                            $scope.comprador.UF = null;
                            $scope.comprador.munid = null;
                            $scope.comprador.tipologradouro = null;
                            $scope.comprador.endereco = null;
                            $scope.comprador.numero = null;
                            $scope.comprador.complemento = null;
                        }
                        else {
                            if (retorno.result.cep.CEP_BAIRRO != null) {
                                $scope.comprador.bairro = retorno.result.cep.CEP_BAIRRO.BAR_DESCRICAO;
                            }
                            if (retorno.result.cep.MUNICIPIO != null) {
                                $scope.comprador.cidade = retorno.result.cep.MUNICIPIO.MUN_DESCRICAO;
                            }
                            //$scope.comprador.CEP = retorno.result.CEP_NUMERO;
                            $scope.comprador.UF = retorno.result.cep.CEP_UF;
                            $scope.comprador.munid = retorno.result.cep.MUN_ID;
                            $scope.comprador.tipologradouro = retorno.result.cep.CEP_TIPO_LOGRADOURO;
                            $scope.comprador.endereco = retorno.result.cep.CEP_LOG;
                            $scope.comprador.numero = null;
                            $scope.comprador.complemento = null;
                        }
                    }
               

                }).error(function (response) {

                    $scope.erro = response;

                    angular.element("#Modal-Erro").modal();

                    hideAjaxLoader();
                });
            }
        }
    }
    $scope.trustAsHtml = function (string) {
        return $sce.trustAsHtml(string);
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
    function dataAtualFormatada(dataHora) {

        var parseDate = new Date(parseInt(dataHora.substr(6)));
        var jsDate = new Date(parseDate);

        return jsDate;
    }
    $scope.ConvertDateJSON = function (jsondata) {

        var data = null;

        if (jsondata != null)
            data = new Date(parseInt(jsondata.substr(6)));

        return data;
    }
    $scope.realizarLogin = function (login) {

        showAjaxLoader();

        var url = "/Cadastro/Login";
        $http({
            url: url,
            method: "post",
            data: { _login: $scope.usuario.login, _senha: $scope.usuario.senha }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                if (login)
                    url = Util.getUrl("/Portal");
                else
                    url = Util.getUrl("/Cadastro/DadosFaturamento");

                post(url);

            }
            else {

                $scope.bolinha = Util.createMessage("fail", response.message.message);

                hideAjaxLoader();

                $scope.button = "reset";
            }

        }).error(function (response) {

            $scope.erro = response;

            angular.element("#Modal-Erro").modal();
     
            hideAjaxLoader();

            $scope.button = "reset";
        });


    }
    $scope.realizarCadastro = function () {

        showAjaxLoader();

        var url = "/Cadastro/Cadastrar";
        $http({
            url: url,
            method: "post",
            data: { _usuario: $scope.usuario}
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {
                url = Util.getUrl("/Cadastro/Pagamento");
                post(url);

            }
            else {

                $scope.bolina2 = Util.createMessage("fail", response.message.message);

                hideAjaxLoader();
            }

        }).error(function (response) {

            $scope.erro = response;

            angular.element("#Modal-Erro").modal();

            hideAjaxLoader();
        });


    }
    $scope.validarEmail = function () {

        showAjaxLoader();

        var url = "/Cadastro/VerificaEmail";
        $http({
            url: url,
            method: "post",
            data: { _email: $scope.usuario.email}
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {
                url = Util.getUrl("/Cadastro/DadosFaturamento");
                post(url);

            }
            else {

                $scope.bolinha2 = Util.createMessage("fail", response.message.message);

                hideAjaxLoader();

                $scope.button = "reset";
            }

        }).error(function (response) {

            $scope.erro = response;

            angular.element("#Modal-Erro").modal();

            hideAjaxLoader();

            $scope.button = "reset";
        });


    }
    $scope.realizarPagamento = function () {

        showAjaxLoader();

        var url = "/Cadastro/RealizarPagamento";
        $http({
            url: url,
            method: "post",
            data: { _cartao: $scope.cartao, _comprador: $scope.comprador }
        }).success(function (response) {

            if (response.success == true) {

                url = Util.getUrl("/Cadastro/Conclusao?_nome=" + $scope.comprador.nome + "&_formapgto=" + $scope.cartao.formapgto);
                post(url);

            }
            else {

                $scope.erro = response.message.message;

                angular.element("#Modal-Erro").modal();

                hideAjaxLoader();

                $scope.button = "reset";
            }

            hideAjaxLoader();


        }).error(function (response) {

            $scope.erro = response;

            angular.element("#Modal-Erro").modal();

            hideAjaxLoader();

            $scope.button = "reset";

        });

    }

});
