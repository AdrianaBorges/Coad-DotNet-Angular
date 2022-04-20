appModule.controller('RetornoController', function ($scope, formHandlerService, $http, conversionService, $timeout) {


    $scope.limparFiltro = function () {

        $scope.alocacao = null;
        $scope.filtro = {};
        $scope.filtro.vencI = "";
        $scope.filtro.vencF = "";
        $scope.filtro.ctaId = null;
        $scope.filtro.bco = "";
        $scope.filtro.remessa = null;
        $scope.filtro.tpAlocacao = "padrao";
        $scope.retselect = {};

        //------

        var data = new Date();

        var dia = data.getDate();
        if (dia.toString().length == 1)
            dia = "0" + dia;
        var mes = data.getMonth() + 1;
        if (mes.toString().length == 1)
            mes = "0" + mes;
        var ano = data.getFullYear();

        $scope.filtro.vencI = 01 + "/" + mes + "/" + ano;
        $scope.filtro.vencF = dia + "/" + mes + "/" + ano;
        $scope.filtro.vlrI = null;
        $scope.filtro.vlrF = null;
        $scope.filtro.vlrT = null;

        //------

        $scope.filtro.tituloEspecifico = false;
        $scope.chkTituloEspecifico();
    }


    $scope.abriModalUploadArquivo = function (item) {

        angular.element("#modal-upload").modal();
    }

    $scope.abriModaluploadRetorno = function (item) {

        angular.element("#modal-upload-retorno").modal();
    }

    $scope.abriModalDetalharErro = function (item) {

        $scope.retselect = item;
        conversionService.deepConversion($scope.retselect);

        $scope.retselect.PAR_NUM_PARCELA = null;
        $scope.lstItemRetorno = null;
        $scope.lstItemErro = null;

        $scope.listarErroRetorno(null, 2);

    }

    $scope.abriModalDetalharRetorno = function (item) {

        $scope.retselect = item;
        conversionService.deepConversion($scope.retselect);

        $scope.retselect.PAR_NUM_PARCELA = null;
        $scope.lstItemRetorno = null;
        $scope.lstItemErro = null;

        $scope.listarParcelas(null,1);

    }

    $scope.initRetorno = function () {

        //----------------------
        var data = new Date();

        var dia = data.getDate();
        if (dia.toString().length == 1)
            dia = "0" + dia;
        var mes = data.getMonth();
        if (mes.toString().length == 1)
            mes = "0" + mes;
        var ano = data.getFullYear();

        $scope.filtro = {};
        $scope.filtro.inicial = new Date(ano, mes, 01);
        $scope.filtro.final = new Date(ano, mes, dia);
        $scope.filtro.empresa = 2;

        //-----------------------

        $scope.listarRetorno();

    }
    // upload do CNAB \\
    $scope.uploadFile = function (files) {

        if (files.length = 0) 
            return;

        if (!confirm("Confirma o upload do arquivo ?")) 
            return;

        showAjaxLoader();

        $scope.files = files ? files : $scope.files;

        var url = "/Retorno/ProcessarArquivoRetorno";
        var fd = new FormData();

        fd.append("file", files[0]);
   
        $http.post(url, fd, {
            showAjaxLoader: true,
            withCredentials: true,
            headers: { 'Content-Type': undefined },
            transformRequest: angular.identity
        }).success(function (response) {

            if (response.success === true) {

                $scope.message = response.message;

                $scope.listarRetorno();
            }
            else {

                if (response.message == null)
                    $scope.message = Util.createMessage("fail", response);
                else
                    $scope.message = response.message;

            }

            hideAjaxLoader();

        }).error(function (response) {

            $scope.message = response.message;

            hideAjaxLoader();

        });
        
    };
    $scope.listarAuditoriaRetorno = function (pagReq01) {


        if ($scope.filtro == null) 
            $scope.filtro = {}
      

        showAjaxLoader2();

        var url = "/Retorno/BuscarAuditoriaRetorno";

        $http({
            url: url,
            method: "post",
            data: {_cnq_id: $scope.filtro.retorno
                 , _rem_id: $scope.filtro.remessa
                 , _parNumParcela: $scope.filtro.parcela
                 , _parNossoNumero: $scope.filtro.nossonumero
                 , _ban_id: $scope.filtro.banco
                 , _oct_codigo: $scope.filtro.oct_codigo
                 , _ipe_id: $scope.filtro.ipe
                 , _ppi_id: $scope.filtro.ppi_id
                 , _cnqnome: $scope.filtro.cnqnome
                 , _pagina: pagReq01
            }

        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {

                $scope.lstretorno = response.result.lstretorno;
                conversionService.deepConversion($scope.lstretorno);
                $scope.totalpago = response.result.totalpago;
                $scope.totalarqu = response.result.totalarqu;

                $scope.page = response.page;

            }
            else {

                if (response.message != null)
                    $scope.message = response.message;
                else
                    $scope.message = Util.createMessage("fail", response);

                $scope.lstretorno = null;

                hideAjaxLoader2();
            }

        }).error(function (response) {

            alert(message);

            $scope.message = Util.createMessage("fail", response);
            $scope.lstretorno = null;

            hideAjaxLoader2();
        });
    }

    $scope.listarParcelas = function (pagReq01, modal) {

        showAjaxLoader2();

        var url = "/Retorno/ListarParcelasRetorno";
        $http({
            url: url,
            method: "post",
            data: {
                _cnq_id: $scope.retselect.CNQ_ID
                ,_parNumParcela: $scope.retselect.PAR_NUM_PARCELA
                , _pagina: pagReq01
            }

        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {

                $scope.lstItemErro = response.result.lstItemErro;
                $scope.lstItemRetorno = response.result.lstItemRetorno;
                conversionService.deepConversion($scope.lstItemErro);
                conversionService.deepConversion($scope.lstItemRetorno);

                $scope.pagReq01 = response.page;

                if (modal == 1)
                    angular.element("#modal-detalhar-retorno").modal();

                if (modal == 2)
                    angular.element("#modal-detalhar-erro").modal();

            }
            else {

                if (response.message != null)
                   // alert(response.message.message);
                    $scope.message = response.message;
                else
                    //alert(response);
                    $scope.message = Util.createMessage("fail", response);

                $scope.listaparcelas = null;

                hideAjaxLoader2();
            }

        }).error(function (response) {

            alert(message);

            $scope.message = Util.createMessage("fail", response);
            $scope.listaparcelas = null;

            hideAjaxLoader2();
        });

    }

    $scope.listarErroRetorno = function (pagReq02, modal) {

        showAjaxLoader2();

        var url = "/Retorno/ListarErroRetorno";
        $http({
            url: url,
            method: "post",
            data: {
                _cnq_id: $scope.retselect.CNQ_ID
                , _parNumParcela: $scope.retselect.PAR_NUM_PARCELA
                , _pagina: pagReq02
            }

        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {

                $scope.lstItemErro = response.result.lstItemErro;
                conversionService.deepConversion($scope.lstItemErro);

                $scope.pagReq02 = response.page;

                if (modal == 2)
                    angular.element("#modal-detalhar-erro").modal();

            }
            else {

                if (response.message != null)
                    //alert(response.message.message);
                    $scope.message = response.message;
                else
                   // alert(response);
                   $scope.message = Util.createMessage("fail", response);

                $scope.lstItemErro = null;

                hideAjaxLoader2();
            }

        }).error(function (response) {

            
            $scope.message = Util.createMessage("fail", response);
            $scope.lstItemErro = null;

            hideAjaxLoader2();
        });

    }


    $scope.listarOcorrenciaRetorno = function (ban_id) {

        showAjaxLoader2();
        
        if ($scope.filtro == null)
            $scope.filtro = {};

        $scope.filtro.oct_codigo = null;

        var url = "/Retorno/ListarOcorrenciaRetorno";
        $http({
            url: url,
            method: "post",
            data: { _ban_id: ban_id }

        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {

                $scope.lstOcorrenciaRetorno = response.result.lstOcorrenciaRetorno;
                conversionService.deepConversion($scope.lstOcorrenciaRetorno);
                $scope.listarAuditoriaRetorno();
            }
            else {

                if (response.message != null)
                    $scope.message = response.message;
                else
                    $scope.message = Util.createMessage("fail", response);

                $scope.lstOcorrenciaRetorno = null;

                hideAjaxLoader2();
            }

        }).error(function (response) {


            $scope.message = Util.createMessage("fail", response);
            $scope.lstOcorrenciaRetorno = null;

            hideAjaxLoader2();
        });

    }

    $scope.listarRetorno = function (paginaReq) {

        showAjaxLoader2();

        var url = "/Retorno/ListarRetorno";
        $http({
            url: url,
            method: "post",
            data: {
                _dtini: $scope.filtro.inicial,
                _dtfinal: $scope.filtro.final,
                _ban_id: $scope.filtro.banco,
                _nome: $scope.filtro.nome,
                _pagina: paginaReq
            }

        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {

                $scope.lstretorno = response.result.listaretorno;
                conversionService.deepConversion($scope.lstretorno);

                $scope.page = response.page;
            }
            else {

                if (response.message == null)
                    $scope.message = Util.createMessage("fail", response);
                else
                    $scope.message = response.messag;

                $scope.lstretorno = null;

                hideAjaxLoader2();
            }

        }).error(function (response) {

            
            $scope.message = Util.createMessage("fail", response);
            $scope.lstretorno = null;

            hideAjaxLoader2();
        });

    }
  
    $scope.processarRetorno = function (_cnqid) {

        showAjaxLoader2();

        var url = Util.getUrl("/Retorno/ProcessarRetorno");

        $http({
            url: url,
            method: "post",
            data: { _cnq_id : _cnqid}

        }).success(function (response) {

            if (response.success == true) {

                $scope.message = response.message;

                $scope.listarRetorno($scope.page.pagina);

                hideAjaxLoader2();

            } else {

                if (response.message != null)
                    $scope.message = response.message;
                else
                    $scope.message = Util.createMessage("fail", response);
                

                hideAjaxLoader2();
            }


        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);

            hideAjaxLoader2();

        });

    }

  
    if (window.BaixaManualController !== undefined) {

        BaixaManualController($scope, formHandlerService, $http, conversionService, $timeout);
    }



});