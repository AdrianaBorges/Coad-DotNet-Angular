appModule.controller('GerenciaTitulosController', function ($scope, formHandlerService, $http, conversionService, $sce) {

    $scope.inicio = function (idTitulo) {
        if (!$scope.filtro)
            $scope.filtro = {};

        if (idTitulo) {
            $scope.filtro.idTitulo = idTitulo;
            $scope.buscarContas();
        }
    }


    $scope.trustAsHtml = function (string) {
        return $sce.trustAsHtml(string);
    };

    $scope.obterTitulosDaAssinatura = function (idAssinatura) {
        $scope.now = new Date();
        $scope.now.toISOString();

        if (!$scope.filtro) {
            $scope.filtro = {};
            $scope.filtro.idAssinatura = '';
        }

        if (idAssinatura) {
            $scope.filtro.idAssinatura = idAssinatura;
        }

        if ($scope.filtro.idAssinatura) {
            formHandlerService.read($scope, {
                url: Util.getUrl("/CobrancaEscritural/obterTitulosDaAssinatura"),
                targetObjectName: 'titulos',
                responseModelName: 'titulos',
                showAjaxLoader: true,
                data: angular.copy($scope.filtro),
                success: function (retorno) {
                },
                fail: function (retorno) {

                    $scope.message = retorno.message;
                }
            });
        }
    }

    // imprimir MODAL \\
    $scope.imprimirModal = function (tela) {
        var printContent = document.getElementById(tela);
        var windowUrl = 'about:blank';
        var uniqueName = new Date();
        var windowName = 'Print' + uniqueName.getTime();
        var printWindow = window.open(windowUrl, windowName, 'left=50000,top=50000,width=0,height=0');

        printWindow.document.write(printContent.innerHTML);
        printWindow.document.close();
        printWindow.focus();
        printWindow.print();
        printWindow.close();
    }

    // cliente para boleto avulso \\
    $scope.buscarClientePorAssinatura = function () {
        if ($scope.filtro.idAssinatura) {
            formHandlerService.read($scope, {
                url: Util.getUrl("/CobrancaEscritural/buscarClientePorAssinatura"),
                targetObjectName: 'cliente',
                responseModelName: 'cliente',
                showAjaxLoader: true,
                data: angular.copy($scope.filtro),
                success: function (retorno) {
                }
            });
        }
    }

    // contas para boleto avulso \\
    $scope.buscarContas = function () {
        if ($scope.filtro.idTitulo) {
            formHandlerService.read($scope, {
                url: Util.getUrl("/CobrancaEscritural/buscarContas"),
                targetObjectName: 'contas',
                responseModelName: 'contas',
                showAjaxLoader: true,
                data: angular.copy($scope.filtro),
                success: function (retorno) {
                    $scope.emails = retorno.result.emails;
                }
            });
        }
    }

    // zeros à esquerda...
    $scope.pad = function (s) { return (s < 10) ? '0' + s : s; }

    // tem letras?...
    $scope.temLetra = function (texto) {
        var letras = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        for (i = 0; i < texto.length; i++) {
            if (letras.indexOf(texto.charAt(i), 0) != -1)
                return texto.substring(i, 1);
        }
        return false;
    }
        
    // lendo o valor de um dropdownlist...
    $scope.LeiaDropDown = function (campo, informacao) {
        var dropdown = document.getElementById(campo);
        if (dropdown && dropdown.options[dropdown.selectedIndex].text !== "" && dropdown.options[dropdown.selectedIndex].text !== "Selecione" && dropdown.options[dropdown.selectedIndex].text !== "Todos") {
            return (informacao == "valor") || (informacao == "undefined") ? dropdown.options[dropdown.selectedIndex].value : dropdown.options[dropdown.selectedIndex].text;
        }
    }

    // zerando a seleção de um dropdownlist...
    $scope.ZerarSelecaoDropDown = function (campo) {
        var dropdown = document.getElementById(campo);
        dropdown.selectedIndex = 0;
    }

    // setando o index de um dropdownlist...
    $scope.SetarIndiceDropDown = function (campo, informacao) {
        var dropdown = document.getElementById(campo);
        for (var i = 0, j = dropdown.options.length; i < j; ++i) {
            if (dropdown.options[i].value === informacao) {
                dropdown.selectedIndex = i;
                break;
            }
        }
    }

    // ANALÍTICOS   ================================================================================================================== \\

    // tabs \\
    $scope.AtivarTab = function (tabAtiva) {
        $scope.tabAtiva = tabAtiva;
        $scope.filtro.ctaId = "";
        //$scope.ZerarSelecaoDropDown('cta');
        //$scope.SetarIndiceDropDown('cta', $scope.filtro.ctaId);
    }

    // tab da alocação \\
    $scope.TabAlocacao = function (id) {
        $scope.TabAtiva = id;
        $scope.limparFiltro();

        if (id == 3)
            $scope.titulosSintetico();

    }

    // checar conta da empresa \\
    $scope.checarCtaEmp = function () {
        $scope.SetarIndiceDropDown('cta', $scope.filtro.ctaId);
        var cta = $scope.LeiaDropDown('cta');
        if (cta.indexOf('Empresa: ' + $scope.tabAtiva.substring(0, 1)) == -1) {
            alert("Por favor, escolha uma Conta da Empresa " + $scope.tabAtiva);
            $scope.filtro.ctaId = "";
            //$scope.ZerarSelecaoDropDown('cta');
        }
    }

    // disponíveis: 1.Disp 2.Transm 3.Rej 4.Aloc 5.Vencido Receber 6.Vincendo Receber 7.Receb 8.Expirando 9.Expirado 
    $scope.listarAnalitico = function (item, rl) {
        formHandlerService.read($scope, {
            url: Util.getUrl("/cobrancaEscritural/LerTitulosAnaliticos"),
            targetObjectName: 'analitico',
            responseModelName: 'analitico',
            showAjaxLoader: true,
            data: { bco: item.banco.substring(0, 3), rel: rl },
            success: function (retorno) {
                if (retorno.result && retorno.result.analitico && retorno.result.analitico.length > 0) {

                    $scope.ope = 0; // define 0=nada; 1=expirando(alocar); 2=expirado(redefinir vencimentos)
                    $scope.qtd = 0; // expirandos - permitir alocar; e expirados - redefinir vencimentos
                    $scope.vlr = 0; // expirandos - permitir alocar; e expirados - redefinir vencimentos

                    if (rl == 1) {
                        $scope.cabecalho = "Titulos Disponíveis [" + item.banco + "]";
                        $scope.arquivo = "disponiveis";
                        var qtd = item.qDisponiveis;
                        var vlr = item.vDisponiveis;
                    }
                    if (rl == 2){
                        $scope.cabecalho = "Titulos Transmitidos [" + item.banco + "]";
                        $scope.arquivo = "transmitidos";
                        var qtd = item.qTransmitidos;
                        var vlr = item.vTransmitidos;
                    }
                    if (rl == 3){
                        $scope.cabecalho = "Titulos Rejeitados [" + item.banco + "]";
                        $scope.arquivo = "rejeitados";
                        var qtd = item.qRejeitados;
                        var vlr = item.vRejeitados;
                    }
                    if (rl == 4){
                        $scope.cabecalho = "Titulos Alocados [" + item.banco + "]";
                        $scope.arquivo = "alocados";
                        var qtd = item.qAlocados;
                        var vlr = item.vAlocados;
                    }
                    if (rl == 5){
                        $scope.cabecalho = "Titulos Vencidos a Receber [" + item.banco + "]";
                        $scope.arquivo = "vencidosReceber";
                        var qtd = item.qVencidosReceber;
                        var vlr = item.vVencidosReceber;
                    }
                    if (rl == 6){
                        $scope.cabecalho = "Titulos Vincendos a Receber [" + item.banco + "]";
                        $scope.arquivo = "vincendosReber";
                        var qtd = item.qVincendosReceber;
                        var vlr = item.vVincendosReceber;
                    }
                    if (rl == 7){
                        $scope.cabecalho = "Titulos Recebidos [" + item.banco + "]";
                        $scope.arquivo = "recebidos";
                        var qtd = item.qRecebidos;
                        var vlr = item.vRecebidos;
                    }
                    if (rl == 8){
                        $scope.cabecalho = "Titulos Expirando [" + item.banco + "]";
                        $scope.arquivo = "expirando";
                        var qtd = item.qExpirando;
                        var vlr = item.vExpirando;
                        $scope.qtd = qtd;
                        $scope.vlr = vlr;
                        $scope.ope = 1;
                    }
                    if (rl == 9){
                        $scope.cabecalho = "Titulos Expirados [" + item.banco + "]";
                        $scope.arquivo = "expirados";
                        var qtd = item.qExpirados;
                        var vlr = item.vExpirados;
                        $scope.qtd = qtd;
                        $scope.vlr = vlr;
                        $scope.ope = 2;
                    }

                    // preparando a impressão \\
                    var d = new Date();
                    var data = [d.getFullYear(), $scope.pad(d.getMonth() + 1), $scope.pad(d.getDate())].join('-') + ' ' + [$scope.pad(d.getHours()), $scope.pad(d.getMinutes()), $scope.pad(d.getSeconds())].join(':');

                    var tPago = 0;
                    var tVlr = 0;

                    $scope.linhas = "<html><head><style>table { border-collapse: collapse;}table, td, th {border: 1px solid black;}</style></head><body>";
                    $scope.linhas += "<table style=\"font-size:18\">";
                    $scope.linhas += "<tr><td><strong>" + $scope.analitico[0].empresa + "</strong></td></tr>";
                    $scope.linhas += "<tr><td><strong>" + $scope.cabecalho + "</strong> - Emissao: " + data + "</td></tr>";
                    $scope.linhas += "</table>";

                    $scope.linhas += "<table style=\"font-size:18\">";
                    $scope.linhas += "<tr>";
                    $scope.linhas += "<td> Nome </td>";
                    $scope.linhas += "<td> Assinatura </td>";
                    $scope.linhas += "<td> Contrato </td>";
                    $scope.linhas += "<td> Vigencia </td>";
                    $scope.linhas += "<td> Titulo </td>";
                    $scope.linhas += "<td> Banco </td>";
                    $scope.linhas += "<td> Conta </td>";
                    $scope.linhas += "<td> Vencimento </td>";
                    $scope.linhas += "<td style=\"text-align:right\"> Vlr.Titulo </td>";
                    $scope.linhas += "<td> Dia Pagto </td>";
                    $scope.linhas += "<td style=\"text-align:right\"> Vlr.Pagto </td>";
                    $scope.linhas += "</tr>";

                    $scope.empresas = [];

                    $scope.aQtd = [];
                    $scope.aVlr = [];
                    $scope.aPgt = [];

                    // soma e total por empresa \\
                    $scope.aQtd[$scope.analitico[0].empresa] = 0;
                    $scope.aVlr[$scope.analitico[0].empresa] = tVlr.toFixed(2).replace('.', ',').replace(/(\d)(?=(\d{3})+\,)/g, "$1.");
                    $scope.aPgt[$scope.analitico[0].empresa] = tPago.toFixed(2).replace('.', ',').replace(/(\d)(?=(\d{3})+\,)/g, "$1.");

                    for (var i = 0; i < $scope.analitico.length; i++) {
                        // identificando as empresas dos títulos \\
                        if ($scope.empresas.indexOf($scope.analitico[i].empresa) == -1) {
                            tPago = 0;
                            tVlr = 0;

                            $scope.empresas.push($scope.analitico[i].empresa);

                            $scope.aQtd[$scope.analitico[i].empresa] = 0;

                            if ($scope.empresas.length == 1)
                                $scope.tabAtiva = $scope.analitico[i].empresa;
                            else {
                                // totais \\
                                $scope.linhas += "<tr>";
                                $scope.linhas += "<td></td>";
                                $scope.linhas += "<td></td>";
                                $scope.linhas += "<td></td>";
                                $scope.linhas += "<td></td>";
                                $scope.linhas += "<td></td>";
                                $scope.linhas += "<td></td>";
                                $scope.linhas += "<td></td>";
                                $scope.linhas += "<td>Qtd: <strong>" + $scope.aQtd[$scope.analitico[i - 1].empresa] + "</strong></td>";
                                $scope.linhas += "<td style=\"text-align:right\"><strong> " + "R$ " + $scope.aVlr[$scope.analitico[i - 1].empresa] + " </strong></td>";
                                $scope.linhas += "<td></td>";
                                $scope.linhas += "<td style=\"text-align:right\"><strong> " + "R$ " + $scope.aPgt[$scope.analitico[i - 1].empresa] + " </strong></td>";
                                $scope.linhas += "</tr>";
                                $scope.linhas += "</table>"; // fechando a tabela totalizada \\

                                $scope.linhas += "<table style=\"font-size:18\">";
                                $scope.linhas += "<tr></tr>";
                                $scope.linhas += "<tr><td><strong>" + $scope.analitico[i].empresa + "</strong></td></tr>";
                                $scope.linhas += "</table>"; // fechando o cabeçalho \\

                                $scope.linhas += "<table style=\"font-size:18\">";
                                $scope.linhas += "<tr>";
                                $scope.linhas += "<td> Nome </td>";
                                $scope.linhas += "<td> Assinatura </td>";
                                $scope.linhas += "<td> Contrato </td>";
                                $scope.linhas += "<td> Vigencia </td>";
                                $scope.linhas += "<td> Titulo </td>";
                                $scope.linhas += "<td> Banco </td>";
                                $scope.linhas += "<td> Conta </td>";
                                $scope.linhas += "<td> Vencimento </td>";
                                $scope.linhas += "<td style=\"text-align:right\"> Vlr.Titulo </td>";
                                $scope.linhas += "<td> Dia Pagto </td>";
                                $scope.linhas += "<td style=\"text-align:right\"> Vlr.Pagto </td>";
                                $scope.linhas += "</tr>";
                            }
                        }

                        var d = $scope.analitico[i].CTR_DATA_INI_VIGENCIA;
                        var dIniV = "";
                        if (d != null)
                            dIniV = [d.getFullYear(), $scope.pad(d.getMonth() + 1), $scope.pad(d.getDate())].join('-');

                        var d = $scope.analitico[i].CTR_DATA_FIM_VIGENCIA;
                        var dFimV = "";
                        if (d != null)
                            dFimV = [d.getFullYear(), $scope.pad(d.getMonth() + 1), $scope.pad(d.getDate())].join('-');

                        var d = $scope.analitico[i].PAR_DATA_VENCTO;
                        var dVenc = "";
                        if (d != null)
                            dVenc = [d.getFullYear(), $scope.pad(d.getMonth() + 1), $scope.pad(d.getDate())].join('-');

                        var d = $scope.analitico[i].PAR_DATA_PAGTO;
                        var dPgto = "";
                        if (d != null)
                            dPgto = [d.getFullYear(), $scope.pad(d.getMonth() + 1), $scope.pad(d.getDate())].join('-');

                        var vlrPgto = "";
                        if ($scope.analitico[i].PAR_VLR_PAGO != null) {
                            vlrPgto = $scope.analitico[i].PAR_VLR_PAGO.toFixed(2).replace('.', ',').replace(/(\d)(?=(\d{3})+\,)/g, "$1.");
                            tPago += $scope.analitico[i].PAR_VLR_PAGO;
                        }

                        var vlrParc = "";
                        if ($scope.analitico[i].PAR_VLR_PARCELA != null) {
                            vlrParc = $scope.analitico[i].PAR_VLR_PARCELA.toFixed(2).replace('.', ',').replace(/(\d)(?=(\d{3})+\,)/g, "$1.");
                            tVlr += $scope.analitico[i].PAR_VLR_PARCELA;
                        }

                        var ban = ($scope.analitico[i].BAN_ID != null) ? $scope.analitico[i].BAN_ID : "";
                        var cta = ($scope.analitico[i].CTA_ID != null) ? $scope.analitico[i].CTA_ID : "";

                        $scope.linhas += "<tr>";
                        $scope.linhas += "<td> " + $scope.analitico[i].CLI_NOME + " </td>";
                        $scope.linhas += "<td> " + $scope.analitico[i].ASN_NUM_ASSINATURA + " </td>";
                        $scope.linhas += "<td> " + $scope.analitico[i].CTR_NUM_CONTRATO + " </td>";
                        $scope.linhas += "<td> " + dIniV + " a " + dFimV + " </td>";
                        $scope.linhas += "<td> " + $scope.analitico[i].PAR_NUM_PARCELA + " </td>";
                        $scope.linhas += "<td> " + ban + " </td>";
                        $scope.linhas += "<td> " + cta + " </td>";
                        $scope.linhas += "<td> " + dVenc + " </td>";
                        $scope.linhas += "<td style=\"text-align:right\"> " + "R$ " + vlrParc + " </td>";
                        $scope.linhas += "<td> " + dPgto + " </td>";

                        if (!$scope.analitico[i].PAR_VLR_PAGO || ($scope.analitico[i].PAR_VLR_PAGO == $scope.analitico[i].PAR_VLR_PARCELA)) {
                            $scope.linhas += "<td style=\"text-align:right\"> " + "R$ " + vlrPgto + " </td>";
                        } else {
                            $scope.linhas += "<td style=\"text-align:right\"><strong> " + "R$ " + vlrPgto + " </strong></td>";
                        }

                        $scope.linhas += "</tr>";

                        // soma e total por empresa \\
                        $scope.aQtd[$scope.analitico[i].empresa] += 1;
                        $scope.aVlr[$scope.analitico[i].empresa] = tVlr.toFixed(2).replace('.', ',').replace(/(\d)(?=(\d{3})+\,)/g, "$1.");;
                        $scope.aPgt[$scope.analitico[i].empresa] = tPago.toFixed(2).replace('.', ',').replace(/(\d)(?=(\d{3})+\,)/g, "$1.");;
                    }

                    $scope.linhas += "<tr>";
                    $scope.linhas += "<td></td>";
                    $scope.linhas += "<td></td>";
                    $scope.linhas += "<td></td>";
                    $scope.linhas += "<td></td>";
                    $scope.linhas += "<td></td>";
                    $scope.linhas += "<td></td>";
                    $scope.linhas += "<td></td>";
                    $scope.linhas += "<td>Qtd: <strong>" + $scope.aQtd[$scope.analitico[i - 1].empresa] + "</strong></td>";
                    $scope.linhas += "<td style=\"text-align:right\"><strong> " + "R$ " + $scope.aVlr[$scope.analitico[i - 1].empresa] + " </strong></td>";
                    $scope.linhas += "<td></td>";
                    $scope.linhas += "<td style=\"text-align:right\"><strong> " + "R$ " + $scope.aPgt[$scope.analitico[i - 1].empresa] + " </strong></td>";
                    $scope.linhas += "</tr>";

                    // totais \\
                    $scope.linhas += "<tr>";
                    $scope.linhas += "<td></td>";
                    $scope.linhas += "<td></td>";
                    $scope.linhas += "<td></td>";
                    $scope.linhas += "<td></td>";
                    $scope.linhas += "<td></td>";
                    $scope.linhas += "<td></td>";
                    $scope.linhas += "<td></td>";
                    $scope.linhas += "<td>Qtd: <strong>" + qtd + "</strong></td>";
                    $scope.linhas += "<td style=\"text-align:right\"><strong> " + "R$ " + vlr.toFixed(2).replace('.', ',').replace(/(\d)(?=(\d{3})+\,)/g, "$1.") + " </strong></td>";
                    $scope.linhas += "<td></td>";
                    $scope.linhas += "<td style=\"text-align:right\"><strong> " + "R$ " + tPago.toFixed(2).replace('.', ',').replace(/(\d)(?=(\d{3})+\,)/g, "$1.") + " </strong></td>";
                    $scope.linhas += "</tr>";

                    $scope.linhas += "</table></body></html>";

                    angular.element("#modalAnalitico").modal();

                } else if (retorno.result && retorno.result.analitico && retorno.result.analitico.length == 0) {
                    alert("Sem títulos a serem exibidos!");
                } else {
                    alert("A quantidade de títulos é muito grande para ser exibida!");
                }
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

    // ================================================================================================================== \\

    // gerar os boleto avulso \\
    $scope.gerarBoletoAvulso = function () {

        $scope.lnkPath = null;
        $scope.lnkLink = null;

        formHandlerService.read($scope, {
            url: Util.getUrl("/cobrancaEscritural/VisualizarBoleto"),
            targetObjectName: 'lnk',
            responseModelName: 'lnk',
            showAjaxLoader: true,
            data:
            {
                idTitulo: $scope.filtro.idTitulo,
                dtVencimento: $scope.filtro.dtVencimento.toISOString(),
                vlrBoleto: $scope.filtro.vlrBoleto,
                idConta: $scope.filtro.idConta,
                msg: $scope.filtro.msg
            },
            success: function (response) {
                if (response.success == true) {
                    $scope.lnkPath = response.result.lnkPath;
                    $scope.lnkLink = response.result.lnkLink;
                }
                else {
                    alert(response.message.message);
                }
                hideAjaxLoader();
            }
        });
    }

    // gerar os boletos desta alocação \\
    $scope.gerarBoletos = function () {
        var vlrRemessa = angular.copy($scope.filtro.remessa.substring(0, $scope.filtro.remessa.length - 1));
        formHandlerService.read($scope, {
            url: Util.getUrl("/cobrancaEscritural/gerarBoletosPDF"),
            targetObjectName: 'boletoGerado',
            responseModelName: 'boletoGerado',
            showAjaxLoader: true,
            data: { remessa: vlrRemessa },
            success: function (retorno) {
                if (retorno.result.boletoGerado) {
                    alert("Boleto gerado com sucesso!");
                } else {
                    alert("Boletos não gerados!. Ocorreu um problema durante o processo.");
                }
            }
        });
    }

    // lendo codigos de remessa do banco escolhido \\

    $scope.carregarBoletoHtml = function (idTitulo) {


        showAjaxLoader();

        var url = Util.getUrl("/CobrancaEscritural/CarregarBoletoHtml");
        $http({
            url: url,
            method: "post",
            data: { idparcela: idTitulo }
        }).success(function (response) {

            if (response.success == true) {

                $scope.boleto = response.result.boleto;

                conversionService.deepConversion($scope.boleto);
            }
            else {
                                
                $scope.message = Util.createMessage("fail", response.message.message);
            }

            hideAjaxLoader();


        }).error(function (response) {

            
            $scope.message = Util.createMessage("fail", response);

            hideAjaxLoader();

        });
    }

    $scope.buscarTipoRemessa = function () {

        showAjaxLoader();

        var url = Util.getUrl("/cobrancaEscritural/lerCodigosRemessaDoBanco");
        $http({
            url: url,
            method: "post",
            data: { bco: $scope.filtro.bco }
        }).success(function (response) {

            if (response.success == true) {

                $scope.listacontas = response.result.listacontas;

                conversionService.deepConversion($scope.listacontas);
            }
            else {

                $scope.message = response.message.message;

            }

            hideAjaxLoader();


        }).error(function (response) {

            $scope.message = response;

            hideAjaxLoader();

        });
    }

 

    // abri modal remessas enviadas \\
    $scope.abrirModalRemEnviada = function () {
        angular.element("#modalRemEnviada").modal();
    }

    // fechar modal remessas enviadas \\
    $scope.fecharModalRemEnviada = function () {
        angular.element("#modalRemEnviada").modal("hide");
    }

    // informar a transmissão do arquivo \\
    $scope.informarRemessaTransmitida = function () {
        $scope.filtro.remessa = $scope.filtro.Remessa.REM_ID;
        formHandlerService.read($scope, {
            url: Util.getUrl("/cobrancaEscritural/TransmitirArqCNAB"),
            targetObjectName: 'transmitido',
            responseModelName: 'transmitido',
            showAjaxLoader: true,
            data: angular.copy($scope.filtro),
            success: function (retorno) {
                if (retorno.result.transmitiu) {
                    alert("Transmissão de arquivo realizada com sucesso! Títulos transmitidos.");
                    window.history.go(0);
                } else {
                    alert("Títulos não transmitidos!. Ocorreu um problema durante o processo.");
                }
            }
        });
    }

    // efetuar desAlocação \\
    $scope.desAlocar = function () {
        $scope.filtro.remessa = $scope.filtro.Remessa.REM_ID;
        formHandlerService.read($scope, {
            url: Util.getUrl("/cobrancaEscritural/desAlocar"),
            targetObjectName: 'sintetico',
            responseModelName: 'sintetico',
            showAjaxLoader: true,
            data: angular.copy($scope.filtro),
            success: function (retorno) {
                if (retorno.result.desAlocou) {
                    alert("Desalocação realizada com sucesso! Títulos liberados.");
                    window.history.go(0);
                } else {
                    alert("Títulos não desalocados!. Ocorreu um problema durante o processo.");
                }
            }
        });
    }

    // limpar filtro \\
    $scope.limparFiltro = function () {

        $scope.filtro = {};
        $scope.filtro.dvencimentoI = "";
        $scope.filtro.dvencimentoF = "";
        $scope.filtro.vencI = "";
        $scope.filtro.vencF = "";
        $scope.filtro.vlrI = "";
        $scope.filtro.vlrT = "";
        $scope.filtro.vlrF = "";
        $scope.filtro.ctaId = null;
        $scope.filtro.bco = "";
        $scope.filtro.remessa = null;
        $scope.filtro.Remessa = null;

        $scope.podeGerarCNAB = false;
        $scope.podeGerarCNABTeste = false;
        $scope.podeDesalocar = false;
        $scope.podeGerarBoleto = false;
    }

    $scope.buscarContasBanco = function () {
        
        showAjaxLoader();

        var url = Util.getUrl("/cobrancaEscritural/BuscarContasBanco");
        $http({
            url: url,
            method: "post",
            data: { bco: $scope.filtro.bco }
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
    $scope.InitGerenciaTitulos = function () {

        $scope.TabAtiva = 1;

        showAjaxLoader();

        angular.element("#selTitulosAlocados").trigger("click");
        angular.element("#selAlocar").trigger("click");

        var url = Util.getUrl("/cobrancaEscritural/BuscarRemessa");
        $http({
            url: url,
            method: "post"
           // data: {}
        }).success(function (response) {

            if (response.success == true) {

                $scope.listaremessa = response.result.listaremessa;

                conversionService.deepConversion($scope.listaremessa);
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


    $scope.abrirJanelaGerarRemessa = function (_oficial, item) {

        $scope.selecRemessa = item;

        showAjaxLoader();

        var url = Util.getUrl("/cobrancaEscritural/lerCodigosRemessaDoBanco");
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

    $scope.GerarRemessa = function (_oficial, TipoRemessa) {

        if (TipoRemessa == null) {
            alert("Tipo de remessa não informado");
            return;
        }

        if (_oficial != null)
            if (!confirm("Deseja realmente gerar uma nova remessa ?"))
                return;

        if (_oficial == null)
            _oficial = false;


        $scope.lnkPath = null;
        $scope.lnkLink = null;

        showAjaxLoader();

        //var intervalPromisse = $scope.acompanharStatusBatch();

        var url = Util.getUrl("/cobrancaEscritural/GerarArquivoCNAB");
        $http({
            url: url,
            method: "post",
            data: {
                cta_id: $scope.selecRemessa.CTA_ID,
                leiaute: "400",
                remessa: $scope.selecRemessa.REM_ID,
                oficial: _oficial,
                codRemessa: TipoRemessa
            }

        }).success(function (response) {

            if (response.success == true) {

                $scope.lnkPath = response.result.lnkPath;
                $scope.lnkLink = response.result.lnkLink;

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

    $scope.acompanharStatusBatch = function () {

        var intervalPromise =
            $interval(function () {
                var url = Util.getUrl("/batch/RetornarStatusDeBatchImportacaoSuspect");

                $http({
                    url: url,
                    method: 'POST'
                })
                .then(function (response) {

                    if (response.data.result != null) {

                        $scope.batchStatus = response.data.result.batchStatus;
                    }
                }, function (response) {

                });
                //formHandlerService.read($scope, {
                //    url: url,
                //    targetObjectName: 'batchStatus',
                //    responseModelName: 'batchStatus',
                //    showAjaxLoader: false,
                //    //pageConfig: { pageName: 'page' /*, pageTargetName: 'pageAgendamentoDoDia' */ },
                //    success: function (resp) {

                //    }
                //});
            }, 300,
            0,
            false);

        return intervalPromise;
    }


    // preenche o painel \\
    $scope.titulosSintetico = function () {
        $scope.filtro = {};
        formHandlerService.read($scope, {
            url: Util.getUrl("/cobrancaEscritural/lerTitulosSintetico"),
            targetObjectName: 'sintetico',
            responseModelName: 'sintetico',
            showAjaxLoader: true,
            data: angular.copy($scope.filtro),
            success: function (retorno) {
            }
        });
    }

    $scope.titulosAlocacao = function () {


        $scope.filtro.remessa = $scope.filtro.Remessa ? $scope.filtro.Remessa.REM_ID : $scope.filtro.remessa;
        $scope.filtro.ctaId = parseInt($scope.filtro.ctaId);

        var url = Util.getUrl("/cobrancaEscritural/lerTitulosAlocacao");

        $http({
            url: url,
            method: "post",
            data: {
                remessa: $scope.filtro.remessa,
                ctaId: $scope.filtro.ctaId,
                dvencimentoI: $scope.filtro.dvencimentoI,
                dvencimentoF: $scope.filtro.dvencimentoF,
                vlrI: $scope.filtro.vlrI,
                vlrF: $scope.filtro.vlrF,
                vlrT: $scope.filtro.vlrT
            }

        }).success(function (response) {

            if (response.success == true) {

                $scope.janela = "Alocação - Títulos selecionados";

                var total = 0;
                for (var i = 0; i < response.result.alocacao.length; i++) {
                    total += response.result.alocacao[i].PAR_VLR_PARCELA;
                }

                $scope.alocacao.qAlocacao = retorno.result.alocacao.length;
                $scope.alocacao.vAlocacao = total;
                $scope.filtro.CTA_ID = $scope.filtro.ctaId;
                $scope.gerarCNAB = ($scope.filtro.remessa ? true : false);

                hideAjaxLoader();

                angular.element("#exibirTitulosAlocacao").modal();

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

    // efetuando alocação \\
    $scope.alocacaoConfirmada = function (tipoTitulo) {

        if (tipoTitulo == 'expirando/expirado') {
            alert("Esta operação ainda está sendo implementada! Obrigado.");
            return;
        }

        var d = new Date();
        var dtAloc = [d.getFullYear(), $scope.pad(d.getMonth() + 1), $scope.pad(d.getDate())].join('-') + ' ' + [$scope.pad(d.getHours()), $scope.pad(d.getMinutes()), $scope.pad(d.getSeconds())].join(':');

        for (var i = 0; i < $scope.alocacao.length; i++) {
            $scope.alocacao[i].CTA_ID = $scope.filtro.ctaId;
            $scope.alocacao[i].PAR_DATA_ALOC = dtAloc;
        }

        //$scope.filtro.remessa = $scope.filtro.Remessa.REM_ID != null ? $scope.filtro.Remessa.REM_ID : $scope.filtro.remessa;

        formHandlerService.read($scope, {
            url: Util.getUrl("/cobrancaEscritural/EfetuarAlocacao"),
            targetObjectName: 'alocacao',
            responseModelName: 'alocacao',
            showAjaxLoader: true,
            data: angular.copy($scope.alocacao),
            success: function (retorno) {
                if (retorno.success == true) {
                    $scope.gerarCNAB = true;
                    $scope.filtro.remessa = retorno.result.alocacao[0].PAR_REMESSA;
                    $scope.message = Util.createMessage("success", "Alocação efetuada com sucesso!");
                    $scope.limparFiltro();
                    angular.element("#exibirTitulosAlocacao").modal('hide');
                    window.history.go(0);
                } else {
                    alert(retorno.message.message);
                }
            },
            error: function (retorno) {

                alert(retorno.message.message);

            }
        });
    }

    // gerarCNAB \\
    $scope.gerarArquivoCNAB = function () {
        $scope.filtro.remessa = $scope.filtro.Remessa.REM_ID;
        formHandlerService.read($scope, {
            url: Util.getUrl("/cobrancaEscritural/GerarArquivoCNAB"),
            targetObjectName: 'cnab',
            responseModelName: 'cnab',
            showAjaxLoader: true,
            data: { cta_id: $scope.filtro.ctaId, leiaute: "400", remessa: $scope.filtro.remessa },
            success: function (retorno) {
                if (retorno.result.alocacao) {
                    $scope.gerarCNAB = true;
                    //$scope.filtro.remessa = retorno.result.alocacao[0].PAR_REMESSA;
                    alert("Arquivo CNAB gerado com sucesso! Por favor, verifique e teste o arquivo no sistema bancário.");
                }
            }
        });
    }

    // teste \\
    $scope.processaTitulos = function (pageRequest) {
        $scope.filtro = { empresa: 2, remessa: "teste", de: Date.now(), ate: Date.now(), dataBase: Date.now() };
        $scope.filtro.remessa = $scope.filtro.Remessa.REM_ID;
        formHandlerService.read($scope, {
            url: Util.getUrl("/cobrancaEscritural/processarTitulos"),
            targetObjectName: 'titulos',
            responseModelName: 'titulos',
            showAjaxLoader: true,
            data: angular.copy($scope.filtro),
            success: function (retorno) {
            }
        });
    }

});
