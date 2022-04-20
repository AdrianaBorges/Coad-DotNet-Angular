appModule.controller('RemessaController', function ($scope, formHandlerService, $http, conversionService, $timeout) {


    $scope.verTitulosDestaAlocacao = function () {
        $scope.date = new Date();
        angular.element('#modal-titulos-selecionados-para-remessa').modal();
    };

    $scope.carregarContratos = function () {
        if ($scope.filtro.assinatura) {
            formHandlerService.read($scope, {
                url: Util.getUrl("/Remessa/CarregarContratos"),
                targetObjectName: 'contratos',
                responseModelName: 'contratos',
                showAjaxLoader: true,
                data: { assinatura: $scope.filtro.assinatura },
                success: function (resp, status, config, message, validationMessage) {
                    $scope.message = message;
                    $scope.erros = validationMessage;
                    //                    if (resp.success) {

                    //                    }
                },
                error: function (resp, status, config, message, validationMessage) {
                    alert(resp);
                }
            });
        }
    };

    $scope.carregarTitulos = function () {
        if ($scope.filtro.contrato) {
            formHandlerService.read($scope, {
                url: Util.getUrl("/Remessa/CarregarParcelas"),
                targetObjectName: 'titulos',
                responseModelName: 'titulos',
                showAjaxLoader: true,
                data: { contrato: $scope.filtro.contrato },
                success: function (resp, status, config, message, validationMessage) {
                    $scope.message = message;
                    $scope.erros = validationMessage;
                    //                    if (resp.success) {

                    //                    }
                },
                error: function (resp, status, config, message, validationMessage) {
                    alert(resp);
                }
            });
        }
    };

    $scope.carregarItensAvulsos = function () {

        showAjaxLoader();

        var url = "/Remessa/CarregarItensAvulsos";
        $http({
            url: url,
            method: "post",
            data: {_ASN_NUM_ASSINATURA: $scope.filtro.assinatura
                  ,_CLI_NOME: $scope.filtro.nomecliente
            }

        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.lstalocacaoavulsa = response.result.listaparcelas;
                conversionService.deepConversion($scope.lstalocacaoavulsa);
            }
            else {

                alert(response.message.message);

                hideAjaxLoader();
            }

        }).error(function (response) {

            alert(message);

            hideAjaxLoader();
        });

    }

    $scope.limparFiltro = function () {

        $scope.alocacao = null;
        $scope.filtro = {};
        $scope.filtro.porvalor = false;
        $scope.filtro.avulsa = false;
        $scope.filtro.vencI = "";
        $scope.filtro.vencF = "";
        $scope.filtro.ctaId = null;
        $scope.filtro.bco = "";
        $scope.filtro.remessa = null;
        $scope.filtro.tpAlocacao = "padrao";

        //------

        var data = new Date();

        var dia = data.getDate();
        if (dia.toString().length == 1)
            dia = "0" + dia;
        var mes = data.getMonth() + 1;
        if (mes.toString().length == 1)
            mes = "0" + mes;
        var ano = data.getFullYear();

        $scope.filtro.vencI = "01" + "/" + mes + "/" + ano;
        $scope.filtro.vencF = dia + "/" + mes + "/" + ano;
        $scope.filtro.vlrI = null;
        $scope.filtro.vlrF = null;
        $scope.filtro.vlrT = null;

        $scope.limparAlocacao();
        $scope.initRemessa();

    }

    $scope.abriModalDetalharRemessa = function (item) {

        $scope.remselect = item;

        $scope.listaparcelas = null;

        $scope.listarParcelas();

        angular.element("#modal-detalhar-remessa").modal();
    }
    $scope.abriModalSelecionarRemessa = function (_valor) {

        $scope.filtro.porvalor = true;
        $scope.filtro.avulsa = false;
        $scope.filtro.tiporemessa = $scope.lstTipoRemessa[0];
     
        angular.element("#modal-selecionar-remessa").modal();
    }


    $scope.initRemessa = function () {

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
        $scope.filtro.inicial = new Date(ano, mes, 1);
        $scope.filtro.final = new Date(ano, mes, dia);
        $scope.filtro.empresa = 2;

        //-----------------------

        $scope.listarRemessa();
        $scope.listarTipoRemessa();
    }

    $scope.selecionaTipoAlocacao = function (item) {

        $scope.filtro.avulsa = item.TRE_AVULSA;
    }

    $scope.infTransmissaoArq = function (item) {

        showAjaxLoader();

        var url = "/Remessa/TransmitirArqCNAB";
        $http({
            url: url,
            method: "post",
            data: { _rem_id: item.REM_ID
                   ,_cta_id: item.CTA_ID
           }  

        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.listarRemessa($scope.paginaReq);
            }
            else {

                alert(response.message.message);

                hideAjaxLoader();
            }

        }).error(function (response) {

            alert(message);

            hideAjaxLoader();
        });

    }

    $scope.listarTipoRemessa = function () {

        showAjaxLoader();

        var url = "/Remessa/ListarTipoRemessa";
        $http({
            url: url,
            method: "post"

        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.lstTipoRemessa = response.result.lstTipoRemessa;
                conversionService.deepConversion($scope.lstTipoRemessa);

            }
            else {

                if (response.message != null)
                    alert(response.message.message);
                else
                    alert(response);

                $scope.lstTipoRemessa = null;

                hideAjaxLoader();
            }

        }).error(function (response) {

            alert(message);

            $scope.message = Util.createMessage("fail", response);
            $scope.lstTipoRemessa = null;

            hideAjaxLoader();
        });

    }
    
    $scope.listarParcelas = function (paginaReq) {

        showAjaxLoader();

        var url = "/Remessa/ListarParcelas";
        $http({
            url: url,
            method: "post",
            data: {
                _remid: $scope.remselect.REM_ID,
                _pagina: paginaReq
            }

        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.listaparcelas = response.result.listaparcelas;
                conversionService.deepConversion($scope.listaparcelas);

                $scope.page = response.page;
            }
            else {
                if (response.message!=null )
                    alert(response.message.message);
                else
                    alert(response);

                $scope.listaparcelas = null;

                hideAjaxLoader();
            }

        }).error(function (response) {

            alert(message);

            $scope.message = Util.createMessage("fail", response);
            $scope.listaparcelas = null;

            hideAjaxLoader();
        });

    }
    
    $scope.listarRemessa = function (paginaReq) {

        showAjaxLoader();

        var url = "/Remessa/ListarRemessa";
        $http({
            url: url,
            method: "post",
            data: {
                _dtini: $scope.filtro.inicial,
                _dtfinal: $scope.filtro.final,
                _emp_id: $scope.filtro.empresa,
                _ban_id: $scope.filtro.banco,
                _pagina: paginaReq
            }

        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.lstdisponivel = response.result.lstdisponivel;
                $scope.listaremessa = response.result.listaremessa;
                conversionService.deepConversion($scope.listaremessa);
                conversionService.deepConversion($scope.lstdisponivel);

                $scope.page = response.page;
            }
            else {

                if (response.message != null)
                    alert(response.message.message);
                else
                    alert(response);

                $scope.listaremessa = null;

                hideAjaxLoader();
            }

        }).error(function (response) {

            alert(message);

            $scope.message = Util.createMessage("fail", response);
            $scope.listaremessa = null;

            hideAjaxLoader();
        });

    }
    $scope.selecionarTitulos = function () {


        if  ((  $scope.filtro.ctaId == "" || $scope.filtro.ctaId == null)
            || ($scope.filtro.bco   == "" || $scope.filtro.bco   == null)
            || ($scope.filtro.vlrT  == "" || $scope.filtro.vlrT  == null)) {

            alert("Informe todos os campos para gerar a Remessa !")
            return;
        }


        showAjaxLoader();


        var url = Util.getUrl("/Remessa/SelecionarTitulos");

        $http({
            url: url,
            method: "post",
            data: {
                ctaId: $scope.filtro.ctaId,
                vlrT: $scope.filtro.vlrT,
            }

        }).success(function (response) {

            if (response.success == true) {

                $scope.janela = "Alocação - Títulos selecionados";

                var total = 0;
                for (var i = 0; i < response.result.alocacao.length; i++) {
                    total += response.result.alocacao[i].PAR_VLR_PARCELA;
                }

                $scope.listaalocacao = response.result.alocacao;
                conversionService.deepConversion($scope.listaalocacao);

                $scope.alocacao = {};
                $scope.alocacao.qAlocacao = response.result.alocacao.length;
                $scope.alocacao.vAlocacao = total;

                hideAjaxLoader();

            }
            else {

                alert(response.message.message);

                hideAjaxLoader();

            }

        }).error(function (response) {

            alert(response);

            hideAjaxLoader();

        });

    }

    $scope.titulosAlocacao = function () {

        $scope.filtro.titulo = (typeof $scope.filtro.titulo === "undefined") ? null : $scope.filtro.titulo;

        if ($scope.filtro.tpAlocacao == "padrao") {

            if (($scope.filtro.vencI == "" || $scope.filtro.vencI == null)
                || ($scope.filtro.vencF == "" || $scope.filtro.vencF == null)
                || ($scope.filtro.ctaId == "" || $scope.filtro.ctaId == null)
                || ($scope.filtro.bco == "" || $scope.filtro.bco == null)
                || ($scope.filtro.vlrI == "" || $scope.filtro.vlrI == null)
                || ($scope.filtro.vlrF == "" || $scope.filtro.vlrF == null)
                || ($scope.filtro.vlrT == "" || $scope.filtro.vlrT == null)) {

                alert("Informe todos os campos para gerar a Remessa Padrão!")
                return;
            }

        } else {

            if (
                ($scope.filtro.ctaId == "" || $scope.filtro.ctaId == null) ||
                ($scope.filtro.bco == "" || $scope.filtro.bco == null)
            ) {

                alert("Informe todos os campos para gerar a Remessa Avulsa!")
                return;
            }

        }

        showAjaxLoader();


        var url = Util.getUrl("/Remessa/lerTitulosAlocacao");

        $http({
            url: url,
            method: "post",
            data: {
                ctaId: $scope.filtro.ctaId,
				bco: $scope.filtro.bco,
                dvencimentoI: $scope.filtro.dvencimentoI,
                dvencimentoF: $scope.filtro.dvencimentoF,
                vlrI: $scope.filtro.vlrI,
                vlrF: $scope.filtro.vlrF,
                vlrT: $scope.filtro.vlrT,
                titulo: $scope.filtro.titulo
            }

        }).success(function (response) {

            if (response.success == true) {

                $scope.janela = "Alocação - Títulos selecionados";

                var total = 0;
                for (var i = 0; i < response.result.alocacao.length; i++) {
                    total += response.result.alocacao[i].PAR_VLR_PARCELA;
                }

                $scope.listaalocacao = response.result.alocacao;
                conversionService.deepConversion($scope.listaalocacao);

                $scope.alocacao = {};
                $scope.alocacao.qAlocacao = response.result.alocacao.length;
                $scope.alocacao.vAlocacao = total;

                hideAjaxLoader();

            }
            else {

                alert(response.message.message);

                hideAjaxLoader();

            }

        }).error(function (response) {

            alert(response);

            hideAjaxLoader();

        });

    }

    $scope.buscarContasBanco = function () {

        showAjaxLoader();

        var url = Util.getUrl("/Remessa/ListarContasBanco");
        $http({
            url: url,
            method: "post",
            data: { bco: $scope.filtro.bco, empid: $scope.filtro.empresa }
        }).success(function (response) {

            if (response.success == true) {

                $scope.listacontas = response.result.listacontas;

                conversionService.deepConversion($scope.listacontas);
            }
            else {

                $scope.erro = response.message.message;

                angular.element("#Modal-Erro").modal();

                hideAjaxLoader();

            }

            hideAjaxLoader();


        }).error(function (response) {

            $scope.erro = response;

            angular.element("#Modal-Erro").modal();

            hideAjaxLoader();

        });
    }

    $scope.limparAlocacao = function () {

        $scope.listaalocacao = [];
        $scope.lstalocacaoavulsa = [];
        $scope.alocacao = {};
        $scope.selecionados = false;
        $scope.filtro.assinatura = null;
        $scope.filtro.nomecliente = null;

    }

    $scope.addItemAlocacao = function (item) {


        if (($scope.filtro.bco == null || $scope.filtro.bco == "") ||
            ($scope.filtro.ctaId == null || $scope.filtro.ctaId == "")) {
            alert("Informe o banco e a agência!!");
            return
        }

        showAjaxLoader();

        if (!$scope.listaalocacao.length > 0)
            $scope.listaalocacao = [];

        if (!$scope.alocacao) {
            $scope.alocacao = {};
            $scope.alocacao.qAlocacao = $scope.listaalocacao.length;
            $scope.alocacao.vAlocacao = 0;
        }

        novo = {};
        novo.ASN_NUM_ASSINATURA = item.ASN_NUM_ASSINATURA;
        novo.CLI_NOME = item.CLI_NOME;
        novo.PAR_DATA_VENCTO = item.PAR_DATA_VENCTO;
        novo.PAR_NUM_PARCELA = item.PAR_NUM_PARCELA;
        novo.PAR_VLR_PARCELA = item.PAR_VLR_PARCELA;
        novo.BAN_ID = $scope.filtro.bco;
        novo.CTA_ID = $scope.filtro.ctaId;
        novo.EMP_ID = $scope.filtro.empid;

        $scope.listaalocacao.push(novo);

        $scope.alocacao.qAlocacao = $scope.listaalocacao.length;

        hideAjaxLoader();

    }
    $scope.removeItemAlocacao = function (index) {
        
        var count = $scope.listaalocacao.length;

        if (count > 0) {
            $scope.listaalocacao.splice(index, 1)

            if ($scope.listaalocacao.length < 1)
                $scope.selecionados = false;
        }

    }

    $scope.mostrarSelecionado = function () {

        if ($scope.selecionados)
            $scope.selecionados = false
        else
            $scope.selecionados = true;

    }
    
    $scope.efetuarAlocacaoAvulsa = function () {

        showAjaxLoader();

        var url = Util.getUrl("/Remessa/EfetuarAlocacaoAvulsa");

        $http({
            url: url,
            method: "post",
            data: { alocar: $scope.listaalocacao
                  , _tre_id: $scope.filtro.tiporemessa.TRE_ID
            }

        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                alert("Alocação efetuada com sucesso!");

                $scope.listarRemessa($scope.page.pagina);
                $scope.limparAlocacao();

                

            } else {

                alert(response.message.message);

            }


        }).error(function (response) {

            alert(response);

            hideAjaxLoader();

        });

    }
    $scope.efetuarAlocacao = function () {

        showAjaxLoader();

        var url = Util.getUrl("/Remessa/EfetuarAlocacao");

        $http({
            url: url,
            method: "post",
            data: {
                alocar: $scope.listaalocacao,
                _tre_id: $scope.filtro.tiporemessa.TRE_ID,
                sacadorAvalista: $scope.filtro.sacadorAvalista
            }

        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                alert("Alocação efetuada com sucesso!");

                $scope.listarRemessa($scope.page.pagina);
                $scope.limparAlocacao();
                
            } else {

                alert(response.message.message);

                hideAjaxLoader();
            }


        }).error(function (response) {

            alert(response);

            hideAjaxLoader();

        });

    }

    $scope.abrirJanelaGerarRemessa = function (item) {

        $scope.selecRemessa = item;

        showAjaxLoader();

        var url = Util.getUrl("/Remessa/lerCodigosRemessaDoBanco");
        $http({
            url: url,
            method: "post",
            data: { bco: item.BAN_ID }
        }).success(function (response) {

            if (response.success == true) {

                $scope.lstTipoRemessa = response.result.lstTipoRemessa;

                conversionService.deepConversion($scope.lstTipoRemessa);

                angular.element("#modal-gerar-remessa").modal();

                hideAjaxLoader();
            }
            else {

                alert(response.message.message);

                hideAjaxLoader();

            }

        }).error(function (response) {

            $scope.message = response;

            hideAjaxLoader();

        });

    }

    $scope.GerarRemessa = function (codTipoRemessa) {

        if (codTipoRemessa == null) {
            alert("Tipo de remessa não informado");
            return;
        }

        if (!confirm("Deseja realmente gerar uma nova remessa ?"))
            return;

   
        $scope.lnkPath = null;
        $scope.lnkPathZip = null;

        showAjaxLoader();

        var url = Util.getUrl("/Remessa/GerarArquivoCNAB");

        $http({
            url: url,
            method: "post",
            data: {
                cta_id: $scope.selecRemessa.CTA_ID,
                leiaute: "400",
                remessa: $scope.selecRemessa.REM_ID,
                tipoRemessa: codTipoRemessa,
                preAlocado: $scope.selecRemessa.REM_AVULSA
            }

        }).success(function (response) {

            if (response.success == true) {

                if (response.result.lnkPath) {
                    $scope.lnkPath = response.result.lnkPath;
                }

                $scope.listarRemessa($scope.paginaReq);

                hideAjaxLoader();

            }
            else {

                alert(response.message.message);

                hideAjaxLoader();

            }
        
        }).error(function (response) {

            alert(response);

            hideAjaxLoader();

        });


    }


    $scope.desAlocar = function (item) {

        if (!confirm("Deseja realmente desalocar estes titulos ?"))
            return;


        showAjaxLoader();

        var url = Util.getUrl("/Remessa/desAlocar");

        $http({
            url: url,
            method: "post",
            data: { _rem_id: item.REM_ID }

        }).success(function (response) {

            if (response.success == true) {

                alert("Desalocação realizada com sucesso!");

                $scope.listarRemessa($scope.paginaReq);

                hideAjaxLoader();

            } else {

                alert(response.message.message);

                hideAjaxLoader();
            }


        }).error(function (response) {

            alert(response);

            hideAjaxLoader();

        });

    }

    $scope.downloadNFEsRemessa = function (item) {

        $scope.selecRemessa = item;

        showAjaxLoader();

        var url = Util.getUrl("/Remessa/downloadNFesRemessa");
        $http({
            url: url,
            method: "post",
            data: { REM_ID: item.REM_ID }
        }).success(function (response) {

            if (response.success == true) {

                if (response.result.lnkPath) {
                    $scope.lnkPathZip = response.result.lnkPath;
                }

                $scope.listarRemessa($scope.paginaReq);

                hideAjaxLoader();

            }
            else {

                alert(response.message.message);

                hideAjaxLoader();

            }

        }).error(function (response) {

            $scope.message = response;

            hideAjaxLoader();

        });

    }

});