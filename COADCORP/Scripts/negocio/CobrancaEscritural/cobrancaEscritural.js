appModule.controller('CobrancaEscrituralController', function ($scope, formHandlerService, $http, conversionService) {

    // meses por extenso...
    $scope.mes = ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'];
    
    // zeros à esquerda...
    $scope.pad = function (s) { return (s < 10) ? '0' + s : s; }

    $scope.init = function () {

        $scope.filtro = {};

        $scope.buscarCNAB();
    }
    $scope.buscarCNAB = function () {

        showAjaxLoader();

        var url = "/CobrancaEscritural/BuscarCNAB";

        $http({
            url: url,
            method: "post"

        }).success(function (response) {


            hideAjaxLoader();

            if (response.success == true) {

                $scope.lstcnab = response.result.lstcnab;
                conversionService.deepConversion($scope.lstcnab);
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

    $scope.buscarDetalheCNAB = function () {

        showAjaxLoader();

        var url = "/CobrancaEscritural/BuscarDetalheCNAB";

        $http({
            url: url,
            method: "post",
            data: {_referencia: $scope.filtro.cnab}
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.lstdetalhecnab = response.result.lstdetalhecnab;
                conversionService.deepConversion($scope.lstdetalhecnab);
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


    $scope.salvarCnab = function (item) {

        showAjaxLoader();

        var url = "/CobrancaEscritural/SalvarCnab";

        $http({
            url: url,
            method: "post",
            data: { _CNAB: item }

        }).success(function (response) {

            hideAjaxLoader();

            if (!response.success) {

                alert(response.message.message);

                hideAjaxLoader();
            }

        }).error(function (response) {

            alert(message);

            hideAjaxLoader();
        });

    }


    $scope.CarregarPainelFinanceiroSintetico = function () {


        $scope.ttDisp = 0;
        $scope.ttDispDocIrreg = 0;
        $scope.ttDispRejBco = 0;
        $scope.ttDispDocIrregRejBco = 0;
        $scope.ttAlocadoAReceber = 0;
        $scope.ttAlocadoRecebido = 0;

        $scope.docIrregular = 0;
        $scope.titRejeitado = 0;
        $scope.titRejeitadoIrregular = 0;

        if (!$scope.emp_id)
            $scope.emp_id = 2;
        if (!$scope.parcIni)
            $scope.parcIni = 1;
        if (!$scope.parcFim)
            $scope.parcFim = 99999;
        if (!$scope.dvencimentoI)
            $scope.dvencimentoI = new Date();
        if (!$scope.dvencimentoF) {
            $scope.dvencimentoF = new Date()
            $scope.dvencimentoF.setMonth($scope.dvencimentoF.getMonth() + 12);
        }

        formHandlerService.read($scope, {
            url: Util.getUrl("/CobrancaEscritural/CarregarPainelFinanceiroSintetico"),
            targetObjectName: 'painel',
            responseModelName: 'painel',
            showAjaxLoader: true,
            data: { emp_id: $scope.emp_id, parcIni: $scope.parcIni, parcFim: $scope.parcFim, dtIni: $scope.dvencimentoI, dtFim: $scope.dvencimentoF },
            success: function (retorno) {
                for (var i = 0; i < $scope.painel.length; i++) {
                    $scope.ttDisp += $scope.painel[i].T_DISP;
                    $scope.ttAlocadoAReceber += $scope.painel[i].T_ALOCADO_ARECEBER;
                    $scope.ttAlocadoRecebido += $scope.painel[i].T_ALOCADO_RECEBIDO;
                    $scope.ttDispRejBco += $scope.painel[i].T_DISP_REJ_BCO;
                    $scope.ttDispDocIrreg += $scope.painel[i].T_DISP_IRREG;
                    $scope.ttDispDocIrregRejBco += $scope.painel[i].T_DISP_IRREG_REJ_BCO;

                    if ($scope.painel[i].Q_DISP_REJ_BCO > 0) {
                        $scope.titRejeitado = 1;
                    }
                    if ($scope.painel[i].Q_DISP_IRREG > 0) {
                        $scope.docIrregular = 1;
                    }
                    if ($scope.painel[i].Q_DISP_IRREG_REJ_BCO > 0) {
                        $scope.titRejeitadoIrregular = 1;
                    }
                }
            }
        });
    }

    $scope.CarregarPainelFinanceiroDocInvalido = function () {

        if (!$scope.emp_id)
            $scope.emp_id = 2;
        if (!$scope.parcIni)
            $scope.parcIni = 1;
        if (!$scope.parcFim)
            $scope.parcFim = 99999;
        if (!$scope.dvencimentoI)
            $scope.dvencimentoI = new Date();
        if (!$scope.dvencimentoF)
            $scope.dvencimentoF = new Date();

        $scope.janela = 'Painel Financeiro - títulos de clientes com CPF/CNPJ inválido';

        formHandlerService.read($scope, {
            url: Util.getUrl("/CobrancaEscritural/CarregarPainelFinanceiroDocInvalido"),
            targetObjectName: 'painelDocInvalido',
            responseModelName: 'painelDocInvalido',
            showAjaxLoader: true,
            data: { emp_id: $scope.emp_id, parcIni: $scope.parcIni, parcFim: $scope.parcFim, dtIni: $scope.dvencimentoI, dtFim: $scope.dvencimentoF },
            success: function () {
                $scope.dadosModal = $scope.painelDocInvalido;
                angular.element("#painel").modal();
            }
        });
    }

    $scope.CarregarPainelFinanceiroRejeicaoBancaria = function () {

        if (!$scope.emp_id)
            $scope.emp_id = 2;
        if (!$scope.parcIni)
            $scope.parcIni = 1;
        if (!$scope.parcFim)
            $scope.parcFim = 99999;
        if (!$scope.dvencimentoI)
            $scope.dvencimentoI = new Date();
        if (!$scope.dvencimentoF)
            $scope.dvencimentoF = new Date();

        $scope.janela = 'Painel Financeiro - títulos a menos de 21 dias para rejeição bancária';

        formHandlerService.read($scope, {
            url: Util.getUrl("/CobrancaEscritural/CarregarPainelFinanceiroRejeicaoBancaria"),
            targetObjectName: 'painelRejeicaoBancaria',
            responseModelName: 'painelRejeicaoBancaria',
            showAjaxLoader: true,
            data: { emp_id: $scope.emp_id, parcIni: $scope.parcIni, parcFim: $scope.parcFim, dtIni: $scope.dvencimentoI, dtFim: $scope.dvencimentoF },
            success: function () {
                $scope.dadosModal = $scope.painelRejeicaoBancaria;
                angular.element("#painel").modal();
            }
        });
    }

    $scope.ImprimirAnalitico = function () {
        showAjaxLoader();

        var url = "/CobrancaEscritural/ImprimirAnalitico";
        var fd = new FormData();

        fd.append("html", $scope.linhas);
        fd.append("arquivo", $scope.arquivo);

        $http.post(url, fd, {
            showAjaxLoader: true,
            withCredentials: true,
            headers: { 'Content-Type': undefined },
            transformRequest: angular.identity
        }).success(function (response) {
            if (response) {
                $scope.arquivo = "LIQUIDACAO";
            }
        }).error(function (response) {
            if (response)
                $scope.linhas = response.result.linhas;
        }).then(function (response) {
            var headers = response.headers();
            var blob = new Blob([response.data], { type: headers['content-type'] });
            var link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);
            link.download = $scope.arquivo + ".DOC";
            link.click();
        });
        hideAjaxLoader();
    };

    $scope.abrirModalRetornoCNAB = function () {
        //angular.element("#TelaModalRetornoCNAB").modal();
    }

    $scope.limparSelecao = function () {
        $('#fileID').val('');
        angular.element('fileID').val(null);
        this.files = null;
        $scope.linhas = null;
        $scope.arquivo = "LIQUIDACAO";
        $scope.status = null;
        $scope.jaProcessou = null;
    }

    $scope.adicionarDoc = function () {
        $scope.baixar.push({ PAR_NUM_PARCELA: $scope.PAR_NUM_PARCELA });
    }

    $scope.removerDoc = function (index) {
        if (confirm("Confirmar exclusão")) {
            if ($scope.baixar.length > 1) {
                $scope.baixar.splice(index, 1);
            } else {
                $scope.baixar = [{ PAR_NUM_PARCELA: $scope.PAR_NUM_PARCELA }];
            }
            $scope.calcularTDocLiq();
        }
    }

    $scope.calcularTDocLiq = function () {
        $scope.tDocLiq = 0;
        for (var i = 0; i < $scope.baixar.length; i++) {
            $scope.tDocLiq += $scope.baixar[i].PLI_VALOR;
        }
    }

    $scope.baixarManualmente = function () {
        formHandlerService.read($scope, {
            url: Util.getUrl("/CobrancaEscritural/baixarManualmente"),
            targetObjectName: 'bxManual',
            responseModelName: 'bxManual',
            showAjaxLoader: true,
            data: angular.copy($scope.baixar),
            success: function (r) {
                if (!r.message && r.result.bxManual) {
                    var d = $scope.vencto;
                    $scope.dtVenc = [d.getFullYear(), $scope.pad(d.getMonth() + 1), $scope.pad(d.getDate())].join('-'); // + ' ' + [$scope.pad(d.getHours()), $scope.pad(d.getMinutes()), $scope.pad(d.getSeconds())].join(':');

                    $scope.arquivo = "LIQUIDACAO";

                    $scope.linhas = $scope.linhas ? $scope.linhas : "";
                    $scope.linhas += "<html><head><style>table { border-collapse: collapse;}table, td, th {border: 1px solid black;}</style></head><body>";
                    $scope.linhas += "<table style=\"font-size:18\">";
                    $scope.linhas += "<tr><td><strong>Baixa Manual Efetivada</strong></td></tr>";
                    $scope.linhas += "<tr>";
                    $scope.linhas += "<td> Titulo </td>";
                    $scope.linhas += "<td> Vencto </td>";
                    $scope.linhas += "<td style=\"text-align:right\"> VALOR </td>";
                    $scope.linhas += "<td> Assinatura </td>";
                    $scope.linhas += "<td> CNPJ </td>";
                    $scope.linhas += "<td> Cliente </td>";
                    $scope.linhas += "</tr>";

                    $scope.linhas += "<tr>";
                    $scope.linhas += "<td> " + $scope.PAR_NUM_PARCELA + " </td>";
                    $scope.linhas += "<td> " + $scope.dtVenc + " </td>";
                    $scope.linhas += "<td style=\"text-align:right\"> <strong>" + "R$ " + $scope.titulo.PAR_VLR_PARCELA.toFixed(2).replace('.', ',').replace(/(\d)(?=(\d{3})+\,)/g, "$1.") + "</strong> </td>";
                    $scope.linhas += "<td> " + $scope.assiCliente + " </td>";
                    $scope.linhas += "<td> " + $scope.cnpjCliente + " </td>";
                    $scope.linhas += "<td> " + $scope.nomeCliente + " </td>";
                    $scope.linhas += "</tr>";
                    $scope.linhas += "</table>";

                    $scope.linhas += "<table style=\"font-size:18\">";
                    $scope.linhas += "<tr>";
                    $scope.linhas += "<td> # </td>";
                    $scope.linhas += "<td> DOC </td>";
                    $scope.linhas += "<td style=\"text-align:right\"> VALOR </td>";
                    $scope.linhas += "<td> Numero </td>";
                    $scope.linhas += "<td> Data </td>";
                    $scope.linhas += "</tr>";

                    for (var i = 0; i < $scope.baixar.length; i++) {
                        d = $scope.baixar[i].PLI_DATA_BAIXA;
                        $scope.dtVenc = [d.getFullYear(), $scope.pad(d.getMonth() + 1), $scope.pad(d.getDate())].join('-'); // + ' ' + [$scope.pad(d.getHours()), $scope.pad(d.getMinutes()), $scope.pad(d.getSeconds())].join(':');

                        $scope.linhas += "<tr>";
                        $scope.linhas += "<td>" + (i + 1).toString() + " </td>";
                        $scope.linhas += "<td> " + $scope.baixar[i].PLI_TIPO_DOC + " </td>";
                        $scope.linhas += "<td style=\"text-align:right\"> <strong> " + "R$ " + $scope.baixar[i].PLI_VALOR.toFixed(2).replace('.', ',').replace(/(\d)(?=(\d{3})+\,)/g, "$1.") + "</strong> </td>";
                        $scope.linhas += "<td> " + $scope.baixar[i].PLI_NUMERO + " </td>";
                        $scope.linhas += "<td> " + $scope.dtVenc + " </td>";
                        $scope.linhas += "</tr>";
                    }

                    $scope.linhas += "</table></body></html>";

                    $scope.PAR_NUM_PARCELA = "";

                    alert("Baixa realizada com sucesso!");

                    angular.element("#TelaModalBaixaManual").modal('hide');
                }
            }
        });
    }

    $scope.a = function () {
        alert("Ação inválida neste módulo! Por favor, não clique neste botão.");
    }

    $scope.buscarParcela = function () {
        formHandlerService.read($scope, {
            url: Util.getUrl("/CobrancaEscritural/BuscarParcela"),
            targetObjectName: 'bxManual',
            responseModelName: 'bxManual',
            showAjaxLoader: true,
            data: { PAR_NUM_PARCELA: $scope.PAR_NUM_PARCELA },
            success: function (r) {
                if (!r.message && r.result) {
                    $scope.titulo = r.result.titulo;
                    $scope.baixar = r.result.docLiq;
                    $scope.baixado = $scope.baixar.length > 0 ? "(Já baixado)" : "";
                    $scope.nomeCliente = r.result.nomeCliente;
                    $scope.cnpjCliente = r.result.cnpjCliente;
                    $scope.assiCliente = r.result.assiCliente;

                    $scope.vencto = new Date($scope.titulo.PAR_DATA_VENCTO.match(/\d+/)[0] * 1);

                    for (var i = 0; i < $scope.baixar.length; i++){
                        $scope.baixar[i].PLI_DATA_BAIXA = new Date($scope.baixar[i].PLI_DATA_BAIXA.match(/\d+/)[0] * 1);
                    }

                    if ($scope.baixar.length === 0)
                        $scope.baixar = [{ PAR_NUM_PARCELA: $scope.PAR_NUM_PARCELA }];

                    $scope.calcularTDocLiq();

                    if (!$scope.baixado)
                        alert("Título em aberto!");

                    angular.element("#TelaModalBaixaManual").modal();
                }
            }
        });
    }

    // estornar baixa de títulos \\
    $scope.estornarBaixa = function () {
        formHandlerService.read($scope, {
            url: Util.getUrl("/CobrancaEscritural/EstornarBaixa"),
            targetObjectName: 'bxEstorno',
            responseModelName: 'bxEstorno',
            showAjaxLoader: true,
            data: { PAR_NUM_PARCELA: $scope.PAR_NUM_PARCELA },
            success: function (r) {
                if (!r.message && r.result && r.result.bxEstorno) {
                    alert("Estorno de baixa realizado com sucesso!");
                } else {
                    alert("Erro durante o estorno!");
                }
                angular.element("#TelaModalBaixaManual").modal('hide');
            }
        });
    }

    // upload do CNAB \\
    $scope.uploadFile = function (files, deveProcessar, deveEstornar) {
        showAjaxLoader();
        if (files.length > 0) {
            if (deveProcessar) {
                if (!confirm("Confirma o processamento dos títulos deste arquivo?")) {
                    hideAjaxLoader();
                    return;
                }
            }

            $scope.files = files ? files : $scope.files;

            var url = "/CobrancaEscritural/ProcessarArquivoRetornoCNAB";
            var fd = new FormData();

            fd.append("file", files[0]);
            fd.append("processar", deveProcessar);
            //fd.append("estornar", deveEstornar);

            $http.post(url, fd, {
                showAjaxLoader: true,
                withCredentials: true,
                headers: { 'Content-Type': undefined },
                transformRequest: angular.identity
            }).success(function (response) {
                hideAjaxLoader();

                if (response.success === true) {
                    $scope.status = response.result.status ? " : OK!" : " : ERRO!";
                    $scope.processou = response.result.processou;
                    //$scope.estornou = response.result.estornou;

                    $scope.jaProcessou = "";

                    if ($scope.processou)
                        $scope.jaProcessou = response.result.jaProcessado ? " reprocessado." : " processado.";
                    else
                        $scope.jaProcessou = response.result.jaProcessado ? " já processado." : " não processado.";

                    if (response.result.linhas) {

                        $scope.arquivo = "LIQUIDACAO";

                        //if (!$scope.linhas)
                        //    $scope.linhas = response.result.linhas;
                        //else
                            $scope.linhas = response.result.linhas;

                        //// abrir a modal \\
                        //$scope.abrirModalRetornoCNAB();
                    }
                    if (deveProcessar) {
                        alert(response.message.message);
                    }

                } else {
                    alert(response.message.message);
                }
                
            }).error(function (response) {
                
                alert(response);

                hideAjaxLoader();
            });
        }
    };

    $scope.imprimirArqRetorno = function (files, deveProcessar, imprimir) {
        showAjaxLoader();
        $scope.files = files ? files : $scope.files;

        var url = "/CobrancaEscritural/ImprimirCNABRetorno";
        var fd = new FormData();

        fd.append("file", files[0]);
        fd.append("processar", deveProcessar);

        $http.post(url, fd, {
            showAjaxLoader: true,
            withCredentials: true,
            headers: { 'Content-Type': undefined },
            transformRequest: angular.identity
        }).success(function (response) {
            if (response) {
                $scope.arquivo = "LIQUIDACAO";
            }
        }).error(function (response) {
            if (response)
                $scope.linhas = response.result.linhas;
        }).then(function (response) {
            var headers = response.headers();
            var blob = new Blob([response.data],{type:headers['content-type']});
            var link = document.createElement('a');
            link.href = window.URL.createObjectURL(blob);
            link.download = "RetornoCNAB.DOC";
            link.click();
        });
        hideAjaxLoader();
    };

    $scope.listarConta = function (pageRequest) {
        var url = Util.getUrl("/conta/listarConta");
        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }
        var config = {
            url: url,
            targetObjectName: 'conta',
            responseModelName: 'conta',
            showAjaxLoader: true,
            success: function (retorno) {
                if (retorno.result.conta) {
                    $scope.conta = retorno.result.conta;
                }
            },
            pageConfig: { pageName: 'page' }
        };
        if ($scope.filtro) {
            config.data = angular.copy($scope.filtro);
        }
        formHandlerService.read($scope, config);
    };
    $scope.readConta = function (ctaId) {
        if (ctaId) {
            formHandlerService.read($scope, {
                url: Util.getUrl("/conta/Readconta"),
                targetObjectName: 'conta',
                responseModelName: 'conta',
                showAjaxLoader: true,
                dateFieldsNames: ['DATA_CADASTRO', 'DATA_ALTERA'],
                data: { ctaId: ctaId },
                success: function (r) {
                    $scope.conta.CTA_CEDENTE_EMITE_BOLETO = r.result.conta.CTA_CEDENTE_EMITE_BOLETO ? "1" : "0";
                }
            });
        };
    }
    $scope.salvarConta = function () {
        $scope.conta.CTA_ID = !$scope.conta.CTA_ID ? null : $scope.conta.CTA_ID;
        $scope.conta.CTA_CEDENTE_EMITE_BOLETO = parseInt($scope.conta.CTA_CEDENTE_EMITE_BOLETO);
        formHandlerService.submit($scope, {
            url: Util.getUrl("/conta/salvar"),
            objectName: 'conta',
            showAjaxLoader: true,
            success: function (resp, status, config, message, validationMessage) {
                $scope.message = message;
                $scope.erros = validationMessage;
                if (resp.success) {
                    alert("Salvo com sucesso.");
                    window.location = Util.getUrl("/Conta/index");
                }
            }
        });
    }


    //******************************************************************************\\
    // buscando registro \\
    $scope.buscarRegistro = function (pageRequest) {
        if ($scope.cnab.EMP_ID && $scope.cnab.BAN_ID && $scope.cnab.CNB_CNAB && $scope.cnab.CNB_REGISTRO && $scope.cnab.CNB_ARQUIVO) {
            $scope.filtro = { empresa: $scope.cnab.EMP_ID, banco: $scope.cnab.BAN_ID, leiaute: $scope.cnab.CNB_CNAB, arquivo: $scope.cnab.CNB_ARQUIVO, registro: $scope.cnab.CNB_REGISTRO, itensPorPagina: 999999 };
            var url = Util.getUrl("/cobrancaEscritural/lerCNAB");
            if (pageRequest) {
                url += "?pagina=" + pageRequest;
            }
            var config = {
                url: url,
                targetObjectName: 'cnb',
                responseModelName: 'cnb',
                showAjaxLoader: true,
                success: function (retorno) {
                    if (retorno.result.cnb.length === 0) {
                        $scope.cnab.CNB_INICIO = $scope.cnab.CNB_REGISTRO === "0" ? 10 : 1;
                        $scope.cnab.CNB_FINAL = $scope.cnab.CNB_INICIO;
                        $scope.cnab.CNB_TAMANHO = 1;
                        $scope.cnab.CNB_DECIMAL = 0;

                        // abrir modal para adicionar...
                        $scope.editarCampo($scope.cnab, 0);
                    }
                },
                pageConfig: { pageName: 'page' }
            };
            if ($scope.filtro) {
                config.data = angular.copy($scope.filtro);
            }
            formHandlerService.read($scope, config);
        }
    }

    //--------------------------------------------------------------------------\\

    $scope.removerCampo = function (obj, indice) {
        if (confirm("Confirma a EXCLUSÃO do campo [" + obj.CNB_CAMPO + "]?")) {
            // se já foi gravado, exclui do banco de dados \\
            if (obj.CNB_ID) {
                formHandlerService.read($scope, {
                    url: Util.getUrl("/cobrancaEscritural/removerCampo"),
                    targetObjectName: 'cnab',
                    responseModelName: 'cnab',
                    showAjaxLoader: true,
                    dateFieldsNames: ['DATA_CADASTRO', 'DATA_ALTERA'],
                    data: { cnabId: obj.CNB_ID },
                    success: function (retorno) {
                    }
                });
            }
            // eliminando a linha na tela \\
            $scope.cnb.splice(indice, 1);
        }
    }

    $scope.editarCampo = function (obj, indice) {
        $scope.janela = "Editando campo";
        $scope.cnab = angular.copy(obj);
        $scope.indice = indice;
        $scope.incluir = false;

        angular.element("#adicionarCampo").modal();
    }

    $scope.adicionarCampo = function (obj, indice) {
        $scope.janela = "Adicionando novo campo";
        $scope.cnab = angular.copy($scope.cnb[indice]);
        $scope.cnab.CNB_ID = null;
        $scope.cnab.CNB_CAMPO = null;
        $scope.cnab.CNB_TIPO = null;
        $scope.cnab.CNB_CONTEUDO = null;
        $scope.cnab.CNB_INICIO = $scope.cnab.CNB_FINAL + 1;
        $scope.cnab.CNB_FINAL = $scope.cnab.CNB_FINAL + 1;
        $scope.cnab.CNB_TAMANHO = 1;
        $scope.cnab.CNB_DECIMAL = 0;
        $scope.indice = indice + 1;
        $scope.incluir = true;

        angular.element("#adicionarCampo").modal();
    }

    $scope.confirmarCampo = function () {
        if ($scope.incluir)
            $scope.cnb.push(angular.copy($scope.cnab));
        else
            $scope.cnb[$scope.indice] = angular.copy($scope.cnab);

        angular.element("#adicionarCampo").modal('hide');
    }

    //--------------------------------------------------------------------------\\

    $scope.minMaxInicio = function () {
        if ($scope.cnab.CNB_TIPO === "D")
            $scope.cnab.CNB_FINAL = ($scope.cnab.CNB_INICIO + 5);

        if (typeof $scope.cnab.CNB_INICIO === 'undefined')
            alert('Por favor, informe um valor inicial entre 10 e 400.');
        else if (typeof $scope.cnab.CNB_FINAL !== 'undefined') {
            if (parseInt($scope.cnab.CNB_INICIO) > parseInt($scope.cnab.CNB_FINAL))
                $scope.cnab.CNB_FINAL = $scope.cnab.CNB_INICIO;
            $scope.cnab.CNB_TAMANHO = ($scope.cnab.CNB_FINAL - $scope.cnab.CNB_INICIO) + 1;
        }
    }

    $scope.minMaxFinal = function () {
        if (typeof $scope.cnab.CNB_FINAL === 'undefined')
            alert('Por favor, informe um valor inicial entre 10 e 400.');
        else if (typeof $scope.cnab.CNB_INICIO !== 'undefined') {
            if (parseInt($scope.cnab.CNB_INICIO) > parseInt($scope.cnab.CNB_FINAL))
                $scope.cnab.CNB_FINAL = $scope.cnab.CNB_INICIO;
            $scope.cnab.CNB_TAMANHO = ($scope.cnab.CNB_FINAL - $scope.cnab.CNB_INICIO) + 1;
        }
    }

    //--------------------------------------------------------------------------\\

    $scope.funcaoConfirmada = function (confirmado) {
        $scope.cnab.CNB_CONTEUDO = confirmado;
        angular.element("#selecionaFuncao").modal('hide');
    }

    $scope.funcoes = function () {
        $scope.janela = "Funções";
        angular.element("#selecionaFuncao").modal();
    }

    //--------------------------------------------------------------------------\\

    $scope.campoConfirmado = function (confirmado) {
        $scope.cnab.CNB_CONTEUDO = "#" + confirmado;
        angular.element("#selecionaCampo").modal('hide');
    }

    $scope.camposConta = function () {
        formHandlerService.read($scope, {
            url: Util.getUrl("/cobrancaEscritural/lerCamposConta"),
            targetObjectName: 'conta',
            responseModelName: 'conta',
            showAjaxLoader: true,
            success: function (retorno) {
                $scope.janela = "Campos da Conta";
                $scope.campos = retorno.result.conta;
                angular.element("#selecionaCampo").modal();
            }
        });
    }

    $scope.camposEmpresa = function () {
        formHandlerService.read($scope, {
            url: Util.getUrl("/cobrancaEscritural/lerCamposEmpresa"),
            targetObjectName: 'empresa',
            responseModelName: 'empresa',
            showAjaxLoader: true,
            success: function (retorno) {
                $scope.janela = "Campos da Empresa";
                $scope.campos = retorno.result.empresa;
                angular.element("#selecionaCampo").modal();
            }
        });
    }

    $scope.camposCliente = function () {
        formHandlerService.read($scope, {
            url: Util.getUrl("/cobrancaEscritural/lerCamposCliente"),
            targetObjectName: 'cliente',
            responseModelName: 'cliente',
            showAjaxLoader: true,
            success: function (retorno) {
                $scope.janela = "Campos do Cliente";
                $scope.campos = retorno.result.cliente;
                angular.element("#selecionaCampo").modal();
            }
        });
    }

    $scope.camposClienteEndereco = function () {
        formHandlerService.read($scope, {
            url: Util.getUrl("/cobrancaEscritural/lerCamposClienteEndereco"),
            targetObjectName: 'clienteEndereco',
            responseModelName: 'clienteEndereco',
            showAjaxLoader: true,
            success: function (retorno) {
                $scope.janela = "Campos do Endereço do Cliente";
                $scope.campos = retorno.result.clienteEndereco;
                angular.element("#selecionaCampo").modal();
            }
        });
    }

    $scope.camposParcela = function () {
        formHandlerService.read($scope, {
            url: Util.getUrl("/cobrancaEscritural/lerCamposParcela"),
            targetObjectName: 'parcela',
            responseModelName: 'parcela',
            showAjaxLoader: true,
            success: function (retorno) {
                $scope.janela = "Campos da Parcela";
                $scope.campos = retorno.result.parcela;
                angular.element("#selecionaCampo").modal();
            }
        });
    }

    $scope.listar = function (pageRequest) {
        var url = Util.getUrl("/cobrancaEscritural/lerCNAB");
        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }
        var config = {
            url: url,
            targetObjectName: 'cnb',
            responseModelName: 'cnb',
            showAjaxLoader: true,
            success: function (retorno) {
                if (retorno.result.cnb) {
                    $scope.cnabCampos = retorno.result.cnb;
                }
            },
            pageConfig: { pageName: 'page' }
        };
        if ($scope.filtro) {
            config.data = angular.copy($scope.filtro);
        }
        formHandlerService.read($scope, config);
    };

    $scope.read = function (cnbId) {
        if (cnbId) {
            formHandlerService.read($scope, {
                url: Util.getUrl("/cobrancaEscritural/Readcnab"),
                targetObjectName: 'cnab',
                responseModelName: 'cnab',
                showAjaxLoader: true,
                dateFieldsNames: ['DATA_CADASTRO', 'DATA_ALTERA'],
                data: { cnbId: cnbId },
                success: function () {
                }
            });
        };
    }
});