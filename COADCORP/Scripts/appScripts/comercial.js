appModule.controller('ComercialController', function ($scope, formHandlerService, $http, conversionService, $timeout, cepService, $sce) {

    $scope.filtro = {};
    $scope.filtro2 = {};

    var now = new Date();

    $scope.dataFormatada = function (jsDate) {

        var data = jsDate;
        var dia = data.getDate();
        if (dia.toString().length == 1)
            dia = "0" + dia;
        var mes = data.getMonth() + 1;
        if (mes.toString().length == 1)
            mes = "0" + mes;
        var ano = data.getFullYear();

        $scope.filtro.dataatual = dia + "/" + mes + "/" + ano;

        return $scope.filtro.dataatual;

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

        return $scope.filtro.dataatual;

    }
    $scope.trustAsHtml = function (string) {
        return $sce.trustAsHtml(string);
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
    $scope.listarCarteiras = function (pagina) {

        $scope.carteiraListada = false;
        var url = Util.getUrl("/franquia/carteiramento/buscarCarteiras");

        if (pagina) {

            url += "?pagina=" + pagina;
        }

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstCarteiras',
            responseModelName: 'lstCarteiras',
            data: $scope.filtroCart,
            showAjaxLoader: true,
            pageConfig: { pageName: 'page', pageTargetName: 'paginaCarteira' },
            success: function (resp) {
                $scope.carteiraListada = true;
            }
        });
    }
    $scope.carregaComboRegioes = function (uenId) {

        if ($scope.filtroCart)
            $scope.filtroCart.rgId = null;
        var parans = { uenId: uenId };

        var url = Util.getUrl('/regiao/ListarComboRegiao');
        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'regioesCombo',
            responseModelName: 'lstRegioes',
            data: parans,
            success: function () {

            }
        });

    }
    $scope.abrirModalCarteira = function (carsel) {
          
        // $scope.carregaComboRegioes();
        $scope.filtro.carsel = carsel
        angular.element('#modal-carteira').modal();
    }
    $scope.adicionarCarteira = function (car) {

        if ($scope.filtro.carsel == 1)
           $scope.filtro.caridori = car.CAR_ID;
        else
            $scope.filtro.cariddest = car.CAR_ID;

        angular.element('#modal-carteira').modal('hide');
    }
    $scope.carregarSemanaFat = function () {

        $scope.filtro = {};
        $scope.filtro.PEF_MES = now.getMonth();
        $scope.filtro.PEF_ANO = now.getFullYear();

        $scope.listarSemanas();

    }
    $scope.carregarTela = function () {

        $scope.filtro = {};
        $scope.filtro.dtini = new Date(now.getFullYear(), now.getMonth(), 1);
        $scope.filtro.dtfim = new Date(now.getFullYear(), now.getMonth() + 1, 0);
        $scope.filtro.grupo_id = null;
        $scope.filtro.vigencia = 0;
        $scope.filtro.diasatraso = 0;

        $scope.listarRep();
        $scope.buscarClientesAtivos();

    }
    $scope.carregarTelaApuracao = function () {

        $scope.filtro = {};
        $scope.filtro.mes = now.getMonth();
        $scope.filtro.ano = now.getFullYear();
        $scope.filtro.tipoapuracao = false;

        $scope.listarRep();
        $scope.listarApuracaoVendas();

    }
    $scope.carregarTelaMetaRep = function () {

        $scope.filtro = {};
        $scope.filtro.mes = now.getMonth();
        $scope.filtro.ano = now.getFullYear();

        $scope.listarRep();
        $scope.listarMetaRep();

    }
    $scope.addItem = function () {

        if ($scope.lstSemanas == null) {
            $scope.lstSemanas = [];
        }

        var novo = {};
        novo.PEF_MES = $scope.PEF_MES;
        novo.PEF_ANO = $scope.PEF_ANO;

        $scope.lstSemanas.push(novo);

    }
    $scope.removeItem = function (index) {

        $scope.lstSemanas.splice(index, 1);
    }
    $scope.listarApuracaoVendas = function () {
    
        var _rep_id = null;

        $scope.lstApuracaoVendas = null;

        if ($scope.filtro.mes == null || $scope.filtro.mes == "0") {
            $scope.message = Util.createMessage("fail", "Informe o mês!!");
            return
        }

        if ($scope.filtro.ano == null || $scope.filtro.ano == "") {
            $scope.message = Util.createMessage("fail", "Informe o ano!!");
            return
        }


        if ($scope.filtro.representante != null)
            _rep_id = $scope.filtro.representante.REP_ID;
        else
            _rep_id = 0;

        showAjaxLoader();

        var url = "/Comercial/ListarApuracaoVendas";
        $http({
            url: url,
            method: "post",
            data: {_mes: $scope.filtro.mes
                  ,_ano: $scope.filtro.ano
                  , _repid: _rep_id
                  , _semana: $scope.filtro.tipoapuracao
            }

        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.lstApuracaoVendas = response.result.lstApuracaoVendas;
                conversionService.deepConversion($scope.lstApuracaoVendas);

            }
            else {

                $scope.lstApuracaoVendas = null;

                $scope.message = response.message;
            }

        }).error(function (response) {

            $scope.message = response.message;

            hideAjaxLoader();
        })



    }
    $scope.listarMetaRep = function () {

        var _rep_id = null;

        $scope.lstMetaRep = null;

        if ($scope.filtro.mes == null || $scope.filtro.mes == "0") {
            $scope.message = Util.createMessage("fail", "Informe o mês!!");
            return
        }

        if ($scope.filtro.ano == null || $scope.filtro.ano == "") {
            $scope.message = Util.createMessage("fail", "Informe o ano!!");
            return
        }

        if ($scope.filtro.representante != null)
            _rep_id = $scope.filtro.representante.REP_ID;
        else
            _rep_id = 0;

        showAjaxLoader();

        
        
        var url = "/Comercial/ListarSemanaRep";
        $http({
            url: url,
            method: "post",
            data: { _semana:  $scope.filtro.semana
                  , _dtini: $scope.filtro.dtini
                  , _dtfim: $scope.filtro.dtfim
                  , _repid: _rep_id
            }

        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.lstMetaRep = response.result.lstMetaRep;
                conversionService.deepConversion($scope.lstMetaRep);

            }
            else {
                $scope.lstMetaRep = null;
                $scope.message = response.message;
            }

        }).error(function (response) {

            $scope.message = response.message;

            hideAjaxLoader();
        })

    }
    $scope.listarSemanas = function () {

        $scope.lstSemanas = null;

        if ($scope.filtro.PEF_MES == null || $scope.filtro.PEF_MES == "0") {
            $scope.message = Util.createMessage("fail", "Informe o mês!!");
            return
        }

        if ($scope.filtro.PEF_ANO == null || $scope.filtro.PEF_ANO == "") {
            $scope.message = Util.createMessage("fail", "Informe o ano!!");
            return
        }

        showAjaxLoader();

        var url = "/Comercial/ListarSemanas";
        $http({
            url: url,
            method: "post",
            data: { _PEF_MES: $scope.filtro.PEF_MES
                  , _PEF_ANO: $scope.filtro.PEF_ANO
            }

        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.lstSemanas = response.result.lstSemanas;
                conversionService.deepConversion($scope.lstSemanas);

                //angular.forEach($scope.lstSemanas, function (obj, index) {
                //    obj.PEF_DATA_INI_FAT = $scope.dataFormatada(obj.PEF_DATA_INI_FAT);
                //    obj.PEF_DATA_FIM_FAT = $scope.dataFormatada(obj.PEF_DATA_FIM_FAT);
                //});


            }
            else {
                $scope.lstSemanas = null;
                $scope.message = response.message;
            }

        }).error(function (response) {

            $scope.message = response.message;

            hideAjaxLoader();
        })

    }
    $scope.salvarSemanas = function () {

        if ($scope.filtro.PEF_MES == null || $scope.filtro.PEF_MES == "0") {
            $scope.message = Util.createMessage("fail", "Informe o mês!!");
            return
        }

        if ($scope.filtro.PEF_ANO == null || $scope.filtro.PEF_ANO == "") {
            $scope.message = Util.createMessage("fail", "Informe o ano!!");
            return
        }

        showAjaxLoader();


        var url = "/Comercial/SalvarSemanas";
        $http({
            url: url,
            method: "post",
            data: {
                _semanas: $scope.lstSemanas
            }

        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {
                $scope.message = response.message;
                $scope.listarSemanas();

            }
            else {
                $scope.lstMetaRep = null;
                $scope.message = response.message;
            }

        }).error(function (response) {

            $scope.message = response.message;

            hideAjaxLoader();
        })

    }
    $scope.transfCarteira = function (item) {

        showAjaxLoader();

        if ($scope.filtro.caridori == "" || $scope.filtro.caridori == null)
            if (!confirm("A carteira anterior não foi informada. Confira a inclusão do cliente na carteira " + $scope.filtro.cariddest + " ?"))
                return;

        
        var url = "/Comercial/TransfCarteira";
        $http({
            url: url,
            method: "post",
            data: { _car_id: $scope.filtro.cariddest
                 , _cartAssin: item
            }

        }).success(function (response) {

            hideAjaxLoader();

            if (response.success) {
                $scope.listarClientesCarteira();
            }
            else {
                $scope.lstClientesCarteira = null;
                $scope.message = response.message;
            }

        }).error(function (response) {

            $scope.message = response.message;

            hideAjaxLoader();
        })

    }
    $scope.limparCampos = function () {

        $scope.filtro.caridori = null;
        $scope.filtro.cariddest = null;

    }
    $scope.listarClientesCarteira = function () {

        showAjaxLoader();

        var url = "/Comercial/ListarClientesCarteira";
        $http({
            url: url,
            method: "post",
            data: { _car_id: $scope.filtro.caridori 
                  , _asn_num_assinatura: $scope.filtro.asn_num_assinatura
                }

        }).success(function (response) {

            hideAjaxLoader();

            if (response.success) {
                $scope.lstClientesCarteira = response.result.lstClientesCarteira;
                conversionService.deepConversion($scope.lstClientesCarteira);
            }
            else {
                $scope.lstClientesCarteira = null;
                $scope.message = response.message;
            }

        }).error(function (response) {

            $scope.message = response.message;

            hideAjaxLoader();
        })

    }
    $scope.salvarMetaRepresentante = function (_item) {

        showAjaxLoader();

        var url = "/Comercial/SalvarMetaRepresentante";
        $http({
            url: url,
            method: "post",
            data: { _meta: _item }

        }).success(function (response) {

            hideAjaxLoader();

            if (response.success != true) {
                
                $scope.message = response.message;
            }

        }).error(function (response) {

            $scope.message = response.message;

            hideAjaxLoader();
        })

    }
    $scope.expPlanilha = function () {

        showAjaxLoader2();

        $scope.export = {};

        var _url = '/Comercial/ExportarXLS';

        $http({
            url: _url,
            method: "post",
              data: { _grupo_id: $scope.filtro.grupo_id
                    , _vigencia: $scope.filtro.vigencia
                    , _atraso: $scope.filtro.diasatraso
                    , _quitado: $scope.filtro.quitado
                    , _qtdecontratos: $scope.filtro.qtdecontratos
                    , _anocoad: $scope.filtro.anocoad
                    , _uf: $scope.filtro.uf
                   
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
    $scope.buscaTelAssinatura = function (pageRequest) {

        $scope.abrirModalTelefones($scope.AssinaturaModal, false, pageRequest);

    }
    $scope.confirmarNegociacao = function () {
        angular.element("#Modal-Confirma-Negociacao").modal();
    }
    $scope.abrirModalAtendimento = function (modal) {

        angular.element("#Modal-Atendimento").modal();
    }
    $scope.buscarPesquisaPadrao = function () {

        showAjaxLoader();

        var url = "/Comercial/BuscarPesquisaPadrao";
        $http({
            url: url,
            method: "post",
            data: { _assinatura: $scope.atendimento.ASN_NUM_ASSINATURA, pagina: _pageRequest }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.AssinaturaModal = $scope.atendimento.ASN_NUM_ASSINATURA;
                $scope.listatelefone = response.result.listatelefone;
                conversionService.deepConversion($scope.listatelefone);

                $scope.pagina03 = response.page;

            }
            else {

                $scope.message = response.message;
            }

        }).error(function (response) {

            $scope.message = response.message;

            hideAjaxLoader();
        })


    }
    $scope.buscarClientesAtivos = function (_pageRequest) {

        showAjaxLoader();

        var url = "/Comercial/BuscarClientesAtivos";
        $http({
            url: url,
            method: "post",
            data: {_grupo_id: $scope.filtro.grupo_id
                  ,_vigencia: $scope.filtro.vigencia
                  ,_atraso: $scope.filtro.diasatraso
                  ,_quitado: $scope.filtro.quitado
                  ,_qtdecontratos: $scope.filtro.qtdecontratos
                  ,_anocoad: $scope.filtro.anocoad
                  ,_uf: $scope.filtro.uf
                  ,_pagina: _pageRequest
            } 
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.listaClientesAtivos = response.result.listaClientesAtivos;
                conversionService.deepConversion($scope.listaClientesAtivos);

                $scope.page = response.page;

            }
            else {

                $scope.message = response.message;
            }

        }).error(function (response) {

            $scope.message = response.message;

            hideAjaxLoader();
        })


    }
    $scope.salvarOferta = function (oferta) {

        showAjaxLoader2();

        var url = "/Comercial/GravarAgendamento";
        $http({
            url: url,
            method: "post",
            data: { _oferta: oferta }
        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {

                $scope.message = Util.createMessage("success", "Agendamento realizado com sucesso!!");

                $scope.pesquisarAgenda($scope.page.pagina);
            }
            else {

                if (response.message != null)
                    $scope.message = Util.createMessage("fail", response.message.message);
                else
                    $scope.message = Util.createMessage("fail", response);

            }

        }).error(function (response) {

            $scope.message = response.message;

            hideAjaxLoader2();
        })
    }
    if (window.BoletoAvulsoController !== undefined) {

        BoletoAvulsoController($scope, formHandlerService, $http, conversionService, $timeout);
    }


});
