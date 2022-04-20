appModule.controller('PublicacaoAreaConsultoriaController', function ($scope, formHandlerService, $http, conversionService) {
    // prepara por acionar todas as inicializações necessárias...
    $scope.preparaTudo = function (publicacaoId, colecionadorId) {
        // exibir ou não blocos de informações...
        $scope.exibir = {
            editando: true,
            origem: true,
            veiculacao: true,
            revogacao: false,
            titulacao: true,
            integra: true,
            manchete: true,
            localizacao: true,
            palavraChave: true,
            publicarPortalNews: true,
            ementa: true,
            impresso: true,
            remissao: true,
            remissivo: false,
            obs: true
        };
        // inicializa tela...
        $scope.pub = {};
        $scope.inicializaRemissao();
        $scope.inicializaRemissivo();
        $scope.inicializaTitulacao();
        $scope.inicializaPalavraChave();
        $scope.inicializaUf();
        $scope.inicializaConfig();
        // acionando $scope.read() caso tenha recebido parâmetros...
        if (publicacaoId && colecionadorId) {
            $scope.read(publicacaoId, colecionadorId);
        }
    }
    //*****************************************************************

    // exibir ou não blocos de informações...
    $scope.exibirEditando = function () {
        $scope.exibir.editando = (!$scope.exibir.editando);
    }
    $scope.exibirOrigem = function () {
        $scope.exibir.origem = (! $scope.exibir.origem);
    }
    $scope.exibirVeiculacao = function () {
        $scope.exibir.veiculacao = (!$scope.exibir.veiculacao);
    }
    $scope.exibirRevogacao = function () {
        $scope.exibir.revogacao = (!$scope.exibir.revogacao);
    }
    $scope.exibirTitulacao = function () {
        $scope.exibir.titulacao = (!$scope.exibir.titulacao);
    }
    $scope.exibirIntegra = function () {
        $scope.exibir.integra = (!$scope.exibir.integra);
    }
    $scope.exibirManchete = function () {
        $scope.exibir.manchete = (!$scope.exibir.manchete);
    }
    $scope.exibirLocalizacao = function () {
        $scope.exibir.localizacao = (!$scope.exibir.localizacao);
    }
    $scope.exibirPalavraChave = function () {
        $scope.exibir.palavraChave = (!$scope.exibir.palavraChave);
    }
    $scope.exibirPublicarPortalNews = function () {
        $scope.exibir.publicarPortalNews = (!$scope.exibir.publicarPortalNews);
    }
    $scope.exibirEmenta = function () {
        $scope.exibir.ementa = (!$scope.exibir.ementa);
    }
    $scope.exibirImpresso = function () {
        $scope.exibir.impresso = (!$scope.exibir.impresso);
    }
    $scope.exibirRemissao = function () {
        $scope.exibir.remissao = (!$scope.exibir.remissao);
    }
    $scope.exibirRemissivo = function () {
        $scope.exibir.remissivo = (!$scope.exibir.remissivo);
    }
    $scope.exibirObs = function () {
        $scope.exibir.obs = (!$scope.exibir.obs);
    }
    //*****************************************************************

    // zeros à esquerda...
    $scope.pad = function (s) { return (s < 10) ? '0' + s : s; }

    // publicar na WEB
    $scope.publicarWEB = function () {
        // data da publicacao...
        var d = new Date();
        var publicado = [d.getFullYear(), $scope.pad(d.getMonth() + 1), $scope.pad(d.getDate())].join('-') + ' ' + [$scope.pad(d.getHours()), $scope.pad(d.getMinutes()), $scope.pad(d.getSeconds())].join(':');
        $scope.pub.publicadoWeb = [$scope.pad(d.getDate()), $scope.pad(d.getMonth() + 1), d.getFullYear()].join('/') + ' ' + [$scope.pad(d.getHours()), $scope.pad(d.getMinutes()), $scope.pad(d.getSeconds())].join(':');

        // lançando valores...
        $scope.pub.PUBLICACAO_CONFIG[0].ARE_CONS_ID = $scope.pub.ARE_CONS_ID;
        $scope.pub.PUBLICACAO_CONFIG[0].PCF_PUB_WEB = true;
        $scope.pub.PUBLICACAO_CONFIG[0].PCF_DATA_PUB_WEB = publicado;
        $scope.pub.PUBLICACAO_CONFIG[0].PCF_DATA_PUB_MANCHETE = publicado;

        alert("A íntegra da matéria e a manchete foram publicadas no Portal COAD com sucesso, em [ " + $scope.pub.publicadoWeb + " ]!");
    }

    // publicar na NEWS
    $scope.publicarNEWS = function () {
        // data da publicacao...
        var d = new Date();
        var publicado = [d.getFullYear(), $scope.pad(d.getMonth() + 1), $scope.pad(d.getDate())].join('-') + ' ' + [$scope.pad(d.getHours()), $scope.pad(d.getMinutes()), $scope.pad(d.getSeconds())].join(':');
        $scope.pub.publicadoNews = [$scope.pad(d.getDate()), $scope.pad(d.getMonth() + 1), d.getFullYear()].join('/') + ' ' + [$scope.pad(d.getHours()), $scope.pad(d.getMinutes()), $scope.pad(d.getSeconds())].join(':');

        // lançando valores...
        $scope.pub.PUBLICACAO_CONFIG[0].ARE_CONS_ID = $scope.pub.ARE_CONS_ID;
        $scope.pub.PUBLICACAO_CONFIG[0].PCF_PUB_NEWS = true;
        $scope.pub.PUBLICACAO_CONFIG[0].PCF_DATA_PUB_NEWS = publicado;

        alert("A matéria foi publicada na COAD NEWS com sucesso, em [ " + $scope.pub.publicadoNews + " ]!");
    }

    // publicar na EMENTA
    $scope.publicarEmenta = function () {
        // data da publicacao...
        var d = new Date();
        var publicado = [d.getFullYear(), $scope.pad(d.getMonth() + 1), $scope.pad(d.getDate())].join('-') + ' ' + [$scope.pad(d.getHours()), $scope.pad(d.getMinutes()), $scope.pad(d.getSeconds())].join(':');
        $scope.pub.publicadoEmenta = [$scope.pad(d.getDate()), $scope.pad(d.getMonth() + 1), d.getFullYear()].join('/') + ' ' + [$scope.pad(d.getHours()), $scope.pad(d.getMinutes()), $scope.pad(d.getSeconds())].join(':');

        // lançando valores...
        $scope.pub.PUBLICACAO_CONFIG[0].ARE_CONS_ID = $scope.pub.ARE_CONS_ID;
        $scope.pub.PUBLICACAO_CONFIG[0].PCF_DATA_PUB_EMENTA = publicado;

        alert("A ementa foi publicada com sucesso, em [ " + $scope.pub.publicadoEmenta + " ]!");
    }

    // texto com remissao...
    $scope.txtRemissao = function () {
        var txt = $scope.pub.PRE_REMISSAO ? $scope.pub.PRE_REMISSAO : $scope.pub.PUBLICACAO.PUB_CONTEUDO_RESENHA_TEXTO;
        for (var i = 0; i < $scope.pub.PUBLICACAO_REMISSAO.length; i++) {
            if ($scope.pub.PUBLICACAO_REMISSAO[i].PRE_REMISSAO) {                                 // <<remissaoX>>
                $scope.pub.PUBLICACAO_REMISSAO[i].PRE_NUMERO = ( i + 1 );                         // numerando a remissão...
                $scope.pub.PRE_REMISSAO = txt.replace("&lt;&lt;" + "remissao" + (i+1).toString() + "&gt;&gt;", $scope.pub.PUBLICACAO_REMISSAO[i].PRE_REMISSAO);
            }
        }
    };

    // remissao, remissivo, p.chave, titulacao, uf, config...
    $scope.MudouColecionador = function () {
        // remissão...
        for (var i = 0; i < $scope.pub.PUBLICACAO_REMISSAO.length; i++) {
            $scope.pub.PUBLICACAO_REMISSAO[i].ARE_CONS_ID = $scope.pub.ARE_CONS_ID;
        }
        // removendo caso não esteja preenchido...
        if (!$scope.pub.PUBLICACAO_REMISSAO[0].PRE_REMISSAO) {
            $scope.pub.PUBLICACAO_REMISSAO.splice(0, 1);
        }

        // remissivo...
        for (var i = 0; i < $scope.pub.PUBLICACAO_REMISSIVO.length; i++) {
            $scope.pub.PUBLICACAO_REMISSIVO[i].ARE_CONS_ID = $scope.pub.ARE_CONS_ID;
        }
        // removendo caso não esteja preenchido...
        if (!$scope.pub.PUBLICACAO_REMISSIVO[0].PRE_REMISSIVO) {
            $scope.pub.PUBLICACAO_REMISSIVO.splice(0, 1);
        }

        // palavra chave...
        for (var i = 0; i < $scope.pub.PUBLICACAO_PALAVRA_CHAVE.length; i++) {
            $scope.pub.PUBLICACAO_PALAVRA_CHAVE[i].ARE_CONS_ID = $scope.pub.ARE_CONS_ID;
        }
        // removendo caso não esteja preenchido...
        if (!$scope.pub.PUBLICACAO_PALAVRA_CHAVE[0].PPC_TEXTO) {
            $scope.pub.PUBLICACAO_PALAVRA_CHAVE.splice(0, 1);
        }

        // titulação...
        for (var i = 0; i < $scope.pub.PUBLICACAO_TITULACAO.length; i++) {
            $scope.pub.PUBLICACAO_TITULACAO[i].ARE_CONS_ID = $scope.pub.ARE_CONS_ID;
        }
        // removendo caso não esteja preenchido...
        if ((!$scope.pub.PUBLICACAO_TITULACAO[0].TIT_ID || !$scope.pub.PUBLICACAO_TITULACAO[0].TIT_ID_VERBETE || !$scope.pub.PUBLICACAO_TITULACAO[0].TIT_ID_SUBVERBETE)) {
            $scope.pub.PUBLICACAO_TITULACAO.splice(0, 1);
        }

        // ufs...
        if ($scope.pub.ARE_CONS_ID == '2') { // ICMS possui um ou mais estados...
            for (var i = 0; i < $scope.pub.PUBLICACAO_UF.length; i++) {
                $scope.pub.PUBLICACAO_UF[i].ARE_CONS_ID = $scope.pub.ARE_CONS_ID;
                $scope.pub.PUBLICACAO_UF[i].INF_NUMERO = $scope.pub.PUBLICACAO_UF[i].INF_ANO.substring(5, 10);
                $scope.pub.PUBLICACAO_UF[i].INF_ANO = $scope.pub.PUBLICACAO_UF[i].INF_ANO.substring(0, 4);
                $scope.pub.PUBLICACAO_UF[i].PUB_ATIVO = true;
            }
        } else { // diferente de ICMS, todos os estados = "TD"...
            $scope.pub.PUBLICACAO_UF[0].ARE_CONS_ID = $scope.pub.ARE_CONS_ID;
            $scope.pub.PUBLICACAO_UF[0].UF_ID = "TD";
            $scope.pub.PUBLICACAO_UF[0].INF_NUMERO = $scope.pub.PUBLICACAO_UF[0].INF_ANO.substring(5, 10);
            $scope.pub.PUBLICACAO_UF[0].INF_ANO = $scope.pub.PUBLICACAO_UF[0].INF_ANO.substring(0, 4);
            $scope.pub.PUBLICACAO_UF[0].PUB_ATIVO = true;
        }
        // removendo caso não esteja preenchido...
        if ((!$scope.pub.PUBLICACAO_UF[0].UF_ID || !$scope.pub.PUBLICACAO_UF[0].INF_ANO || !$scope.pub.PUBLICACAO_UF[0].INF_NUMERO)) {
            $scope.pub.PUBLICACAO_UF.splice(0, 1);
        }

        // config...
        for (var i = 0; i < $scope.pub.PUBLICACAO_CONFIG.length; i++) {
            $scope.pub.PUBLICACAO_CONFIG[i].ARE_CONS_ID = $scope.pub.ARE_CONS_ID;
        }
        // removendo caso não esteja preenchido...
        if ((!$scope.pub.PUBLICACAO_CONFIG[0].PCF_DATA_PUB_WEB || !$scope.pub.PUBLICACAO_CONFIG[0].PCF_DATA_PUB_NEWS || !$scope.pub.PUBLICACAO_CONFIG[0].PCF_DATA_PUB_MANCHETE)) {
            $scope.pub.PUBLICACAO_CONFIG.splice(0, 1);
        }
    }
    //*************************************************************

    // declarando config - publicando nos canais... ARRAY
    $scope.inicializaConfig = function () {
        $scope.pub.PUBLICACAO_CONFIG = [{
            ARE_CONS_ID: $scope.pub.ARE_CONS_ID,
            PCF_PUB_WEB: false,
            PCF_PUB_NEWS: false,
            PCF_DATA_PUB_WEB: '',
            PCF_DATA_PUB_NEWS: '',
            PCF_DATA_PUB_MANCHETE: '',
            PCF_DATA_PUB_EMENTA: ''
        }];
    }

    // adicionando config - publicando nos canais... ARRAY
    $scope.adicionarConfig = function () {
        $scope.pub.PUBLICACAO_CONFIG.push({
            ARE_CONS_ID: $scope.pub.ARE_CONS_ID,
            PCF_PUB_WEB: false,
            PCF_PUB_NEWS: false,
            PCF_DATA_PUB_WEB: '',
            PCF_DATA_PUB_NEWS: '',
            PCF_DATA_PUB_MANCHETE: '',
            PCF_DATA_PUB_EMENTA: ''
        });
    }

    // removendo config - publicando nos canais... ARRAY
    $scope.removerConfig = function (config, index) {
        if (confirm("Confirmar exclusão")) {
            if ($scope.pub.PUBLICACAO_CONFIG.length > 1) {
                $scope.pub.PUBLICACAO_CONFIG.splice(index, 1);
            } else {
                $scope.inicializaConfig();
            }
        }
    };
    //*************************************************************

    // declarando Estados... ARRAY
    $scope.inicializaUf = function () {
        $scope.pub.PUBLICACAO_UF = [{ UF_ID: $scope.pub.ARE_CONS_ID == 2 ? "" : "TD", ARE_CONS_ID: $scope.pub.ARE_CONS_ID, INF_ANO: "", INF_NUMERO: "", PUB_ATIVO: true }];
    }

    // adicionando Estados... ARRAY
    $scope.adicionarUf = function () {
        $scope.pub.PUBLICACAO_UF.push({ UF_ID: $scope.pub.ARE_CONS_ID == 2 ? "" : "TD", ARE_CONS_ID: $scope.pub.ARE_CONS_ID, INF_ANO: "", INF_NUMERO: "", PUB_ATIVO: true });
    }

    // removendo Estados... ARRAY
    $scope.removerUf = function (uf, index) {
        if (confirm("Confirmar exclusão")) {
            if ($scope.pub.PUBLICACAO_UF.length > 1) {
                $scope.pub.PUBLICACAO_UF.splice(index, 1);
            } else {
                $scope.inicializaUf();
            }
        }
    };
    //*************************************************************

    // declarando palavra chave... ARRAY
    $scope.inicializaPalavraChave = function () {
        $scope.pub.PUBLICACAO_PALAVRA_CHAVE = [{ PPC_TEXTO: '', ARE_CONS_ID: $scope.pub.ARE_CONS_ID }];
    }

    // adicionando palavra chave... ARRAY
    $scope.adicionarPalavraChave = function () {
        $scope.pub.PUBLICACAO_PALAVRA_CHAVE.push({ PPC_TEXTO: '', ARE_CONS_ID: $scope.pub.ARE_CONS_ID });
    }

    // removendo palavra chave... ARRAY
    $scope.removerPalavraChave = function (palavraChave, index) {
        if (confirm("Confirmar exclusão")) {
            if ($scope.pub.PUBLICACAO_PALAVRA_CHAVE.length > 1) {
                $scope.pub.PUBLICACAO_PALAVRA_CHAVE.splice(index, 1);
            } else {
                $scope.inicializaPalavraChave();
            }
        }
    };
    //*************************************************************

    // declarando remissao... ARRAY
    $scope.inicializaRemissao = function () {
        $scope.pub.PUBLICACAO_REMISSAO = [{ ARE_CONS_ID: $scope.pub.ARE_CONS_ID, PRE_REMISSAO: "", PRE_NUMERO: "" }];
    }

    // adicionando remissao... ARRAY
    $scope.adicionarRemissao = function () {
        $scope.pub.PUBLICACAO_REMISSAO.push({ ARE_CONS_ID: $scope.pub.ARE_CONS_ID, PRE_REMISSAO: "", PRE_NUMERO: "" });
    };

    // removendo remissao... ARRAY
    $scope.removerRemissao = function (remissao, index) {
        if (confirm("Confirmar exclusão")) {
            if ($scope.pub.PUBLICACAO_REMISSAO.length > 1) {
                $scope.pub.PUBLICACAO_REMISSAO.splice(index, 1);
            } else {
                $scope.adicionarRemissao();
            }
        }
    };
    //*************************************************************

    // declarando remissivo... ARRAY
    $scope.inicializaRemissivo = function () {
        $scope.pub.PUBLICACAO_REMISSIVO = [{}];
    }

    // adicionando remissivo... ARRAY
    $scope.adicionarRemissivo = function () {
        $scope.pub.PUBLICACAO_REMISSIVO.push({});
    };

    // removendo remissivo... ARRAY
    $scope.removerRemissivo = function (remissivo, index) {
        if (confirm("Confirmar exclusão")) {
            if ($scope.pub.PUBLICACAO_REMISSIVO.length > 1) {
                $scope.pub.PUBLICACAO_REMISSIVO.splice(index, 1);
            } else {
                $scope.adicionarRemissivo();
            }
        }
    };
    //*************************************************************

    // declarando titulação... ARRAY
    $scope.inicializaTitulacao = function () {
        $scope.pub.PUBLICACAO_TITULACAO = [{ PTI_PRINCIPAL: true }];
        $scope.PTI_PRINCIPAL = $scope.pub.PUBLICACAO_TITULACAO[0];
    }

    //clicou no principal... FUNCIONANDO!
    $scope.marcouPrincipal = function (titulacao) {
        angular.forEach($scope.pub.PUBLICACAO_TITULACAO, function (p) {
            p.PTI_PRINCIPAL = false;
        });
        titulacao.PTI_PRINCIPAL = true;
    };

    // adicionando titulação, verbetes e subverbetes... ARRAY
    $scope.adicionarTitulacao = function (titulacao, index) {
        $scope.pub.PUBLICACAO_TITULACAO.push({
            gg: $scope.pub.PUBLICACAO_TITULACAO[0].gg,
            ARE_CONS_ID: $scope.pub.ARE_CONS_ID,
            TIT_ID: '',
            TIT_ID_VERBETE: '',
            TIT_ID_SUBVERBETE: '',
            PTI_PRINCIPAL: false
        });
    };

    // removendo titulação, verbetes e subverbetes... ARRAY
    $scope.removerTitulacao = function (titulacao, index) {
        if (confirm("Confirmar exclusão")) {
            if ($scope.pub.PUBLICACAO_TITULACAO.length > 1) {
                $scope.pub.PUBLICACAO_TITULACAO.splice(index, 1);
            } else {
                $scope.adicionarTitulacao();
            }
        }
    };

    // leia os grandes grupos do atual colecionador...
    $scope.lerGg = function () {
        // informou colecionador?
        if ($scope.pub.ARE_CONS_ID) {
            // limpa todas as titulações...
            $scope.inicializaTitulacao();

            // pega o valor do colecionador...
            colecionadorId = $scope.pub.ARE_CONS_ID;

            // busca os seus grandes grupos...
            formHandlerService.read($scope, {
                url: Util.getUrl("/publicacaoAreaConsultoria/Gg"),
                targetObjectName: 'gg',
                responseModelName: 'gg',
                data: { colecionadorId: colecionadorId },
                success: function (retorno) {
                    if (!$scope.pub.PUBLICACAO_TITULACAO[0].gg) {
                        $scope.pub.PUBLICACAO_TITULACAO[0].gg = retorno.result.gg;
                    }
                }
            });
        }
        $scope.pub.PUBLICACAO_TITULACAO.verbetes = null;
        $scope.pub.PUBLICACAO_TITULACAO.subverbetes = null;
    }


    // leia os verbetes do grande grupo escolhido...
    $scope.lerVerbetes = function (titulacao, index) {
        // informou grande grupo?
        if (titulacao.TIT_ID) {
            // pega o valor do grande grupo...
            ggId = titulacao.TIT_ID;

            formHandlerService.read($scope, {
                url: Util.getUrl("/publicacaoAreaConsultoria/Verbetes"),
                targetObjectName: 'verbetes',
                responseModelName: 'verbetes',
                data: { ggId: ggId },
                success: function (retorno) {
                    titulacao.verbetes = retorno.result.verbetes;
                }
            });
        }
        titulacao.verbetes = null;
        titulacao.subverbetes = null;
    }

    // leia os subverbetes do verbete escolhido...
    $scope.lerSubverbetes = function (titulacao, index) {
        if (titulacao.TIT_ID_VERBETE) {
            // informando o colecionador desta titulação...
            titulacao.ARE_CONS_ID = $scope.pub.ARE_CONS_ID;

            // pega o valor do verbete...
            vbId = titulacao.TIT_ID_VERBETE;

            formHandlerService.read($scope, {
                url: Util.getUrl("/publicacaoAreaConsultoria/Subverbetes"),
                targetObjectName: 'subverbetes',
                responseModelName: 'subverbetes',
                data: { vbId: vbId },
                success: function (retorno) {
                    titulacao.subverbetes = retorno.result.subverbetes;
                }
            });
        } else {
            titulacao.subverbetes = null;
        }
    }

    // hints...
    $(document).ready(function () {
        $('[data-toggle="tooltip"]').tooltip();   
    });

    // buscar matéria...
    $scope.buscarMateria = function () {
        var mat_id = prompt("Informe o [nº da matéria] cadastrada", "");
        if (mat_id != null) {
            formHandlerService.read($scope, {
                url: Util.getUrl("/publicacaoAreaConsultoria/buscarMateria"),
                targetObjectName: 'pub',
                responseModelName: 'pub',
                dateFieldsNames: ['DATA_CADASTRO', 'DATA_ALTERA', 'DATA_EXCLUSAO'],
                data: { mat_id: mat_id, pubAreaCons: JSON.stringify($scope.pub) },
                success: function () {
                }
            });
            if ($scope.pub.PUB_ID != null) {
                //document.getElementById("PUB_ID").innerHTML = mat_id;
            }
        }
    }

    // iniciando o filtro com ativo=true...
    $scope.init = function () {
        $scope.filtro = { ativoId: 1 };
    }

    // buscando dados para o index...
    $scope.listar = function (pageRequest) {
        var url = Util.getUrl("/publicacaoAreaConsultoria/publicacoesAreaConsultoria");
        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }
        var config = {
            url: url,
            targetObjectName: 'publicacoesAreaConsultoria',
            responseModelName: 'publicacoesAreaConsultoria',
            pageConfig: { pageName: 'page' }
        };
        if ($scope.filtro) {
            config.data = angular.copy($scope.filtro);
        }
        formHandlerService.read($scope, config);
    };

    // buscando dados do ID para o index...
    $scope.read = function (publicacaoId, colecionadorId) {
        // recebeu os dois parâmetros?...
        if (publicacaoId && colecionadorId) {
            formHandlerService.read($scope, {
                url: Util.getUrl("/publicacaoAreaConsultoria/ReadpublicacaoAreaConsultoria"),
                targetObjectName: 'pub',
                responseModelName: 'pub',
                dateFieldsNames: ['DATA_CADASTRO', 'DATA_ALTERA', 'DATA_EXCLUSAO'],
                data: { publicacaoId: publicacaoId, colecionadorId: colecionadorId },
                success: function () {
                    // montando ano e remessa do informativo...
                    for (var i = 0; i < $scope.pub.PUBLICACAO_UF.length; i++) {
                        $scope.pub.PUBLICACAO_UF[i].INF_ANO = $scope.pub.PUBLICACAO_UF[i].INF_ANO + '/' + $scope.pub.PUBLICACAO_UF[i].INF_NUMERO;
                    }
                    // enchendo as titulações...
                    for (var i = 0; i < $scope.pub.PUBLICACAO_TITULACAO.length; i++) {
                        $scope.lerVerbetes($scope.pub.PUBLICACAO_TITULACAO[i]);
                        $scope.lerSubverbetes($scope.pub.PUBLICACAO_TITULACAO[i]);
                    }
                    // datando matérias já publicadas...
                    for (var i = 0; i < $scope.pub.PUBLICACAO_CONFIG.length; i++) {
                        // web...
                        var d = $scope.pub.PUBLICACAO_CONFIG[i].PCF_DATA_PUB_WEB;
                        $scope.pub.publicadoWeb = [$scope.pad(d.getDate()), $scope.pad(d.getMonth() + 1), d.getFullYear()].join('/') + ' ' + [$scope.pad(d.getHours()), $scope.pad(d.getMinutes()), $scope.pad(d.getSeconds())].join(':');
                        // news...
                        var d = $scope.pub.PUBLICACAO_CONFIG[i].PCF_DATA_PUB_NEWS;
                        $scope.pub.publicadoNews = [$scope.pad(d.getDate()), $scope.pad(d.getMonth() + 1), d.getFullYear()].join('/') + ' ' + [$scope.pad(d.getHours()), $scope.pad(d.getMinutes()), $scope.pad(d.getSeconds())].join(':');
                        // ementa...
                        var d = $scope.pub.PUBLICACAO_CONFIG[i].PCF_DATA_PUB_EMENTA;
                        $scope.pub.publicadoEmenta = [$scope.pad(d.getDate()), $scope.pad(d.getMonth() + 1), d.getFullYear()].join('/') + ' ' + [$scope.pad(d.getHours()), $scope.pad(d.getMinutes()), $scope.pad(d.getSeconds())].join(':');
                    }
                    // inicializando modelos vazios...
                    $scope.inicializaModelosVazios();
                    // texto com remissão....
                    $scope.txtRemissao();
                }
            });
        }
    }

    // inicializando modelos vazios...
    $scope.inicializaModelosVazios = function () {
        if ($scope.pub.PUBLICACAO_REMISSAO.length == 0) {
            $scope.inicializaRemissao();
        }
        if ($scope.pub.PUBLICACAO_REMISSIVO.length == 0) {
            $scope.inicializaRemissivo();
        }
        if ($scope.pub.PUBLICACAO_TITULACAO.length == 0) {
            $scope.inicializaTitulacao();
        }
        if ($scope.pub.PUBLICACAO_PALAVRA_CHAVE.length == 0) {
            $scope.inicializaPalavraChave();
        }
        if ($scope.pub.PUBLICACAO_UF.length == 0) {
            $scope.inicializaUf();
        }
        if ($scope.pub.PUBLICACAO_CONFIG) {
            $scope.inicializaConfig();
        }
    }

    // salvando os dados...
    $scope.salvarPublicacaoAreaConsultoria = function () {
        // campos obrigatórios preenchidos para salvar?...
        if ((!$scope.pub.PUBLICACAO.PUB_CONTEUDO_TEXTO) && (!pub.PUBLICACAO.PUB_CONTEUDO_RESENHA_TEXTO)) {
            alert("Por favor, insira a matéria a ser publicada no Portal ou Impresso!");
        } else if (!$scope.pub.ARE_CONS_ID) {
            alert("Por favor, escolha um Colecionador!")
        } else {
            // preparando para salvar...
            $scope.MudouColecionador();
            // salvando...
            formHandlerService.submit($scope, {
                url: Util.getUrl("/publicacaoAreaConsultoria/salvar"),
                objectName: 'pub',
                showAjaxLoader: true,
                success: function (resp, status, config, message, validationMessage) {
                    $scope.message = message;
                    $scope.erros = validationMessage;
                    if (resp.success) {
                        alert("Salvo com sucesso.");
                        window.location = Util.getUrl("/publicacaoAreaConsultoria/index");
                    }
                }
            });
        }
    }
});

/* modelo json..

var PUBLICACAO_AREAS_CONSULTORIA = {
    PUB_ID: 3,
    PUB_MANCHETE: 'TESTE',
    PUBLICACAO_CONFIG: [
		{
		    PCF_ID: 2,
		    PUB_ID: 3
		}
    ],
    PUBLICACAO: { UF_ID: 4 }
};

*/