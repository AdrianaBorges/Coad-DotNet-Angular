appModule.controller('InformativoController', function ($scope, formHandlerService, $http, conversionService) {

    $scope.initInformativo = function (numUltRemessa) {

        //----------------------
        var data = new Date();

        var dia = data.getDate();
        if (dia.toString().length == 1)
            dia = "0" + dia;
        var mes = data.getMonth();
        if (mes.toString().length == 1)
            mes = "0" + mes;
        var ano = data.getFullYear();

        $scope.etiq = {};
        $scope.filtro = {};
        $scope.filtro.inicial = new Date(ano, mes, 01);
        $scope.filtro.final = new Date(ano, mes, dia);
        $scope.filtro.empresa = 2;
        $scope.filtro.ano = ano;
        $scope.filtro.remessa = numUltRemessa;
        $scope.filtro.asn_num_assinatura = null;
        $scope.filtro.tipo = 0;

        //-----------------------

        $scope.listarPostagens();

    }
    $scope.baixarArquivos = function (item) {

        showAjaxLoader2();

        $scope.export = {};

        _url = '/Informativo/BaixarArquivo';

        $http({
            method: 'Post',
            url: _url,
            data: { ano: item.INF_ANO
                  , remessa: item.INF_REMESSA
                  , envio: item.INF_ENVIO
            }
        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {
                $scope.export.lnkPath = response.result.pathRetorno;
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


    $scope.salvarProtocolado = function (_assinatura, _prot) {

        showAjaxLoader2();

        var url = "/Informativo/SalvarProtocolado";
        $http({
            url: url,
            method: "post",
            data: {
                    _asn_num_assinatura: _assinatura
                  , _protocolado: _prot
            }
        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {

                $scope.message = response.message;

            }
            else {

                $scope.message = response.message;
            }

        });

        hideAjaxLoader2();
    }
    $scope.salvarMateriaAdicional = function (_assinatura, _adicional) {

        showAjaxLoader2();

        var url = "/Informativo/SalvarMateriaAdicional";
        $http({
            url: url,
            method: "post",
            data: {
                _asn_num_assinatura: _assinatura
               , _adicional: _adicional
            }
        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {

                $scope.message = response.message;

            }
            else {

                $scope.message = response.message;
            }

        });

        hideAjaxLoader2();
    };

    $scope.mostrarCliente = function () {


        if ($scope.etiq.ASN_NUM_ASSINATURA.length == 0)
            return;

        showAjaxLoader2();

        var _assinatura = $scope.etiq.ASN_NUM_ASSINATURA;

        var url = "/Informativo/BuscarCliente";
        $http({
            url: url,
            method: "post",
            data: { _asn_id: _assinatura }
        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {

                $scope.etiq.ASN_NUM_ASSINATURA = response.result.ASN_NUM_ASSINATURA;
                $scope.etiq.CLI_NOME = response.result.CLI_NOME;
                $scope.etiq.ASN_MATERIA_ADICIONAL = response.result.ASN_MATERIA_ADICIONAL;
                $scope.etiq.ASN_PROTOCOLADA = response.result.ASN_PROTOCOLADA;

            }
            else {

                $scope.etiq.CLI_NOME = "";

                $scope.message = response.message;
            }

        });

        hideAjaxLoader2();

    };

    $scope.listarProtocoladas = function (paginaReq) {

        showAjaxLoader2();

        var url = "/Informativo/BuscarAssinaturasProtocoladas";
        $http({
            url: url,
            method: "post",
            data: {_pagina: paginaReq}

        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {

                $scope.lstProtocoladas = response.result.lstProtocoladas;
                conversionService.deepConversion($scope.lstProtocoladas);

                $scope.paginaReq01 = response.page;
            }
            else {

                if (response.message == null)
                    $scope.message = Util.createMessage("fail", response);
                else
                    $scope.message = response.messag;

                $scope.lstProtocoladas = null;

                hideAjaxLoader2();
            }

        }).error(function (response) {


            $scope.message = Util.createMessage("fail", response);
            $scope.lstProtocoladas = null;

            hideAjaxLoader2();
        });

    }

    
    $scope.listarPostagens = function (paginaReq) {

        showAjaxLoader2();

        var url = "/Informativo/ListarPostagens";
        $http({
            url: url,
            method: "post",
            data: {
                _tipo: $scope.filtro.tipo,
                _dataini: $scope.filtro.inicial,
                _datafim: $scope.filtro.final,
                pagina: paginaReq
            }

        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {

                $scope.lstinformativo = response.result.lstinformativo;
                conversionService.deepConversion($scope.lstinformativo);

                $scope.page = response.page;
            }
            else {

                if (response.message == null)
                    $scope.message = Util.createMessage("fail", response);
                else
                    $scope.message = response.messag;

                $scope.lstinformativo = null;

                hideAjaxLoader2();
            }

        }).error(function (response) {


            $scope.message = Util.createMessage("fail", response);
            $scope.lstinformativo = null;

            hideAjaxLoader2();
        });

    }

    $scope.MontarRemessa = function () {

        showAjaxLoader2();

        var url = "/Informativo/MontarRemessa";
        $http({
            url: url,
            method: "post",
            data: {
                    ano: $scope.filtro.ano
                  , remessa: $scope.filtro.remessa
                  , MDP: $scope.filtro.MDP
                  , dtEntrega : null
            }

        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {


                $scope.listarPostagens();

                $scope.message = response.message;

            }
            else {

                if (response.message == null)
                    $scope.message = Util.createMessage("fail", response);
                else
                    $scope.message = response.messag;

                $scope.lstinformativo = null;

                hideAjaxLoader2();
            }

        }).error(function (response) {


            $scope.message = Util.createMessage("fail", response);
            $scope.lstinformativo = null;

            hideAjaxLoader2();
        });

    }

    $scope.enviarArquivo = function (item) {

        showAjaxLoader2();

        var url = "/Informativo/EnviarArquivo";
        $http({
            url: url,
            method: "post",
            data: { ano: item.INF_ANO
                  , remessa: item.INF_REMESSA
                  , MDP: null
                  , envio: item.INF_ENVIO
            }

        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {

                $scope.message = response.message;

            }
            else {

                if (response.message == null)
                    $scope.message = Util.createMessage("fail", response);
                else
                    $scope.message = response.messag;

                hideAjaxLoader2();
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);

            hideAjaxLoader2();
        });

    }

    /* ConfirmarPostagens */
    $scope.ConfirmarPostagens = function () {
        $scope.filtro.MDP = !$scope.filtro.MDP ? false : $scope.filtro.MDP;
        formHandlerService.read($scope, {
            url: Util.getUrl("/informativo/ConfirmarPostagens"),
            targetObjectName: 'informativo',
            responseModelName: 'informativo',
            showAjaxLoader: true,
            data: angular.copy($scope.filtro),
            success: function (resp, status, config, message, validationMessage) {
                $scope.message = message;
                $scope.erros = validationMessage;
                if (resp.success) {
                }
            },
            error: function (resp, status, config, message, validationMessage) {
                alert(resp);
            }
        });
    }


    /* abrirModalNovaPostagem */
    $scope.abrirModalNovaPostagem = function () {
        angular.element("#modal-nova-postagem").modal();
    }

    $scope.abrirModalProtocolado = function () {

        $scope.listarProtocoladas();

        angular.element("#modal-protocolado").modal();
    }

    $scope.abrirModalAddProtocolado = function () {

        $scope.etiq = {};

        $scope.listarProtocoladas();

        angular.element("#modal-AddProtocolado").modal();
    }

    $scope.abrirModalAddMateriaAdicional = function () {

        $scope.etiq = {};

        angular.element("#modal-AddAdicional").modal();
    }


    $scope.abrirModalMateriaAdicional = function () {

        $scope.etiq = {};

        $scope.listarMateriaAdicional();

        angular.element("#modal-Adicional").modal();
    }

    $scope.listarMateriaAdicional = function (paginaReq) {

        if ($scope.filtro.asn_num_assinatura == "")
            $scope.filtro.asn_num_assinatura = null;

        showAjaxLoader2();

        var url = "/Informativo/ListarMateriaAdicional";
        $http({
            url: url,
            method: "post",
            data: {
                _asn_num_assinatura: $scope.filtro.asn_num_assinatura,
                _pagina: paginaReq
            }

        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {

                $scope.lstMateriaAdicional = response.result.lstMateriaAdicional;
                conversionService.deepConversion($scope.lstMateriaAdicional);

                $scope.paginaReq02 = response.page;

            }
            else {

                if (response.message == null)
                    $scope.message = Util.createMessage("fail", response);
                else
                    $scope.message = response.messag;

                hideAjaxLoader2();
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);

            hideAjaxLoader2();
        });

    }



});