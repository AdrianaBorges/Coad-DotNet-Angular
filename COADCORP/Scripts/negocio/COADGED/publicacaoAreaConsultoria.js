appModule.controller('PublicacaoAreaConsultoriaController', function ($scope, $interval, formHandlerService, $http, conversionService) {

    $scope.editorAltura = 300;
    $scope.editorLargura = 1125;

    $scope.semMenu = function () {
        $scope.editorAltura = 300;
        $scope.editorLargura = 1325;

        angular.element("#wrapper").addClass("toggled"); // expandindo a tela para editar matéria
        angular.element(document.getElementById('button-menu'))[0].disabled = true; // desabilitando o menu principal do COADCORP - para forçar usuário do COADGED a sempre fechar a tela da matéria
    }

    // preparando por acionar todas as inicializações necessárias...
    $scope.preparaTudo = function (publicacaoId, colecionadorId, cabecaMateria, revisao, colecionadorNome, cargoSigla, operacao) {
        // exibir ou não blocos de informações...
        $scope.exibir = {
            editando: true,
            origem: false,
            veiculacao: false,
            revogacao: false,
            revigoracao: false,
            alteracao: false,
            titulacao: false,
            integra: false,
            materiaPortal: false,
            materiaImpressa: false,
            manchete: false,
            manchetePt: false,
            localizacao: false,
            palavraChave: false,
            publicarPortalNews: false,
            ementa: false,
            ementaPt: false,
            impresso: false,
            remissao: false,
            remissivo: false,
            obs: false
        };
        // inicializa tela...

        $scope.ambienteProducao = false; // ambiente

        $scope.colecionadorNome = (typeof colecionadorNome === "undefined") ? $scope._colecionadorNome : colecionadorNome;  // nome do colecionador
        $scope.cargoSigla = (typeof cargoSigla === "undefined") ? $scope._cargoSigla : cargoSigla; // sigla do cargo

        $scope.publicou = false;
        $scope.cabecaMateria = cabecaMateria;
        $scope.pub = {};
        $scope.pub.PUBLICACAO = {};
        $scope.pub.salvouAuto = false;

        $scope.pub.desabilitaWeb = false;
        $scope.pub.desabilitaNews = false;
        $scope.pub.desabilitaEmenta = false;
        $scope.pub.desabilitaSalvar = false;

        $scope.pub.lIncluir = (operacao == "I");
        $scope.pub.operacao = operacao;
        $scope.operacao = operacao == "I" ? "Incluindo nova matéria" : operacao == "A" ? "Editando matéria registrada" : "Visualizando matéria registrada";

        // veio da tela de revisão?...
        $scope.pub.revisao = (revisao && !(revisao == "False")) ? revisao : "0";
        $scope.revisao = $scope.pub.revisao;

        $scope.inicializaRemissao();
        $scope.inicializaRemissivo();
        $scope.inicializaTitulacao();
        $scope.inicializaPalavraChave();
        $scope.inicializaUf();
        $scope.inicializaConfig();
        $scope.iniciazaRevogacaoAlteracao();

        // acionando $scope.read() caso tenha recebido parâmetros...
        $scope.carregouTudo = !(publicacaoId && colecionadorId && $scope.colecionadorNome);

        if (publicacaoId && colecionadorId && $scope.colecionadorNome) {
            $scope.read(publicacaoId, colecionadorId);
        }

        // editor ativo
        $scope.editorAtivo = false;
        $scope.editorAtivoOnEnter = false;
    }

    // lst
    if (typeof $scope.lst === "undefined") {
        $scope.lst = {};
    }

    // materia acessada por
    $scope.acessadaPor = function (pubId) {
        formHandlerService.read($scope, {
            url: Util.getUrl("/publicacaoAreaConsultoria/AcessadaPor"),
            showAjaxLoader: true,
            targetObjectName: 'acessada',
            responseModelName: 'acessada',
            data: { publicacaoId: pubId },
            success: function (retorno) {
                angular.element("#acessadaPor").modal();
            }
        });
    }

    // quem está editando?
    $scope.quemEdita = function (pubId) {
        formHandlerService.read($scope, {
            url: Util.getUrl("/publicacaoAreaConsultoria/QuemEstaEditando"),
            showAjaxLoader: true,
            targetObjectName: 'quemEstaEditando',
            responseModelName: 'quemEstaEditando',
            data: { publicacaoId: pubId },
            success: function (retorno) {
                //$scope.quemEstaEditando = retorno.result.quemEstaEditando;
            }
        });
    }

    // acionado ao sair da matéria do botão de edicao
    $scope.sairDaMateria = function (lVeioDeSalvar) {
        if (confirm("Deseja realmente sair da matéria?")) {
            formHandlerService.read($scope, {
                url: Util.getUrl("/publicacaoAreaConsultoria/RegistrarLiberacaoMateria"),
                showAjaxLoader: true,
                targetObjectName: '_liberar',
                responseModelName: '_liberar',
                data: { pub_id: $scope.pub.PUB_ID },
                success: function (retorno) {
                    // transportando as listas
                    if (!lVeioDeSalvar) {
                        $scope.carregouTudo = false;
                        if (!$scope.lst || $scope.lst == null) {
                            $scope.lst = {};
                        }
                        window.sessionStorage.setItem('lst', JSON.stringify($scope.lst));
                        window.history.back();
                    } else {
                        window.sessionStorage.setItem('lst', JSON.stringify($scope.lst));
                        if (revisao !== "0") {
                            window.history.back();
                        } else {
                            window.location = Util.getUrl("/publicacaoAreaConsultoria/index");
                        }
                    }
                }
            });
        }
    }

    // acionado ao entrar na matéria
    $scope.entrarNaMateria = function () {
        $scope.lst = JSON.parse(window.sessionStorage.getItem('lst'));
        if (!$scope.lst || $scope.lst == null) {
            $scope.lst = {};
        }
        window.sessionStorage.removeItem('lst');
    }

    // acionado ao sair do menu
    $scope.sairDoMenu = function () {
        if (!$scope.lst || $scope.lst == null) {
            $scope.lst = {};
        }
        window.sessionStorage.setItem('lst', JSON.stringify($scope.lst));
    }

    // acionado ao entrar no menu
    $scope.entrarNoMenu = function () {
        $scope.lst = JSON.parse(window.sessionStorage.getItem('lst'));
        if (!$scope.lst || $scope.lst == null) {
            $scope.lst = {};
        }
        window.sessionStorage.removeItem('lst');
    }

    // _dadosLogin
    $scope._dadosLoginCarregar = function (publicacaoId, colecionadorId, cabecaMateria, revisao, operacao) {
        $scope.entrarNaMateria();

        formHandlerService.read($scope, {
            url: Util.getUrl("/publicacaoAreaConsultoria/_dadosLoginCarregar"),
            showAjaxLoader: true,
            targetObjectName: '_dadosLogin',
            responseModelName: '_dadosLogin',
            data: {},
            success: function (retorno) {
                $scope._colecionadorId = retorno.result._colecionadorId;
                $scope._colecionadorNome = retorno.result._colecionadorNome;
                $scope.colecionadorNome = $scope._colecionadorNome;
                $scope._colaborador = retorno.result._colaborador;
                $scope._colaboradorNome = retorno.result._colaboradorNome;
                $scope._cargo = retorno.result._cargoEsigla[0];
                $scope._cargoSigla = retorno.result._cargoEsigla[1];
                $scope.cargoSigla = $scope._cargoSigla;

                $scope.mudouRevogacao = false;

                $scope.preparaTudo(publicacaoId, colecionadorId, cabecaMateria, revisao, $scope._colecionadorNome, $scope._cargoSigla, operacao);

                // pegando o colecionador
                $scope.pub.ARE_CONS_ID = colecionadorId ? colecionadorId : retorno.result._colecionadorId;
            }
        });
    }

    // _basicoCarregar
    $scope._basicoCarregar = function () {
        if (!$scope.lst || !$scope.lst._areas) {
            formHandlerService.read($scope, {
                url: Util.getUrl("/publicacaoAreaConsultoria/_basicoCarregar"),
                showAjaxLoader: true,
                targetObjectName: '_basico',
                responseModelName: '_basico',
                data: {},
                success: function (retorno) {
                    $scope.lst._areas = retorno.result._areas;
                    $scope.lst._informativo = retorno.result._informativo;
                    $scope.lst._tpMateria = retorno.result._tpMateria;
                    $scope.lst._ativo = retorno.result._ativo;
                    $scope.lst._uf = retorno.result._uf;
                }
            });
        }
    }

    // _origemCarregar
    $scope._origemCarregar = function () {
        if (!$scope.lst || !$scope.lst._tpAto) {
            formHandlerService.read($scope, {
                url: Util.getUrl("/publicacaoAreaConsultoria/_origemCarregar"),
                showAjaxLoader: true,
                targetObjectName: '_origem',
                responseModelName: '_origem',
                data: {},
                success: function (retorno) {
                    $scope.lst._tpAto = retorno.result._tpAto;
                    $scope.lst._orgao = retorno.result._orgao;
                }
            });
        }
    }

    // _veiculacaoCarregar
    $scope._veiculacaoCarregar = function () {
        if (!$scope.lst || !$scope.lst._veiculo) {
            formHandlerService.read($scope, {
                url: Util.getUrl("/publicacaoAreaConsultoria/_veiculacaoCarregar"),
                showAjaxLoader: true,
                targetObjectName: '_veiculacao',
                responseModelName: '_veiculacao',
                data: {},
                success: function (retorno) {
                    $scope.lst._veiculo = retorno.result._veiculo;
                }
            });
        }
    }

    // _revogacaoCarregar
    $scope._revogacaoCarregar = function () {
        if (!$scope.lst || !$scope.lst._tpAto) {
            formHandlerService.read($scope, {
                url: Util.getUrl("/publicacaoAreaConsultoria/_revogacaoCarregar"),
                showAjaxLoader: true,
                targetObjectName: '_revogacao',
                responseModelName: '_revogacao',
                data: {},
                success: function (retorno) {
                    $scope.lst._tpAto = retorno.result._tpAto;
                    $scope.lst._orgao = retorno.result._orgao;
                }
            });
        }
    }

    // _revigoracaoCarregar
    $scope._revigoracaoCarregar = function () {
        if (!$scope.lst || !$scope.lst._tpAto) {
            formHandlerService.read($scope, {
                url: Util.getUrl("/publicacaoAreaConsultoria/_revigoracaoCarregar"),
                showAjaxLoader: true,
                targetObjectName: '_revigoracao',
                responseModelName: '_revigoracao',
                data: {},
                success: function (retorno) {
                    $scope.lst._tpAto = retorno.result._tpAto;
                    $scope.lst._orgao = retorno.result._orgao;
                }
            });
        }
    }

    // _alteracaoCarregar
    $scope._alteracaoCarregar = function () {
        if (!$scope.lst || !$scope.lst._tpAto) {
            formHandlerService.read($scope, {
                url: Util.getUrl("/publicacaoAreaConsultoria/_alteracaoCarregar"),
                showAjaxLoader: true,
                targetObjectName: '_alteracao',
                responseModelName: '_alteracao',
                data: {},
                success: function (retorno) {
                    $scope.lst._tpAto = retorno.result._tpAto;
                    $scope.lst._orgao = retorno.result._orgao;
                }
            });
        }
    }

    // _titulacaoCarregar
    $scope._titulacaoCarregar = function () {
        if (!$scope.lst || !$scope.lst._capital) {
            formHandlerService.read($scope, {
                url: Util.getUrl("/publicacaoAreaConsultoria/_titulacaoCarregar"),
                showAjaxLoader: true,
                targetObjectName: '_titulacao',
                responseModelName: '_titulacao',
                data: {},
                success: function (retorno) {
                    $scope.lst._capital = retorno.result._capital;
                }
            });
        }
    }

    // _localizacaoCarregar
    $scope._localizacaoCarregar = function () {
        if (!$scope.lst || !$scope.lst._secao) {
            formHandlerService.read($scope, {
                url: Util.getUrl("/publicacaoAreaConsultoria/_localizacaoCarregar"),
                showAjaxLoader: true,
                targetObjectName: '_local',
                responseModelName: '_local',
                data: {},
                success: function (retorno) {
                    $scope.lst._secao = retorno.result._secao;
                    $scope.lst._label = retorno.result._label;
                }
            });
        }
    }

    // carregarTexto
    $scope.carregarTexto = function (editor, dado, campo, deveBuscarTexto, forcarAtualizacao) {
        if ((((alterouPublicarPortal == false && $scope.pub.PUBLICAR_PORTAL) && !forcarAtualizacao) && editor == 'Publicar') ||
            (((alterouMateriaImpressa == false && $scope.pub.MATERIA_IMPRESSA) && !forcarAtualizacao) && editor == 'Remissao')) {

            $scope.buscarTexto(editor, dado, campo);

        } else {

            var parametros = { campo: campo, _service: null, _pubArea: null, _pub: null };
            if (deveBuscarTexto) {
                parametros = { campo: campo, _service: null, _pubArea: $scope.pub, _pub: $scope.pub.PUBLICACAO };
            }

            formHandlerService.read($scope, {
                url: Util.getUrl("/publicacaoAreaConsultoria/carregarTexto"),
                showAjaxLoader: true,
                targetObjectName: 'txt',
                responseModelName: 'txt',
                data: parametros,
                success: function (retorno) {
                    if (retorno.result) {
                        if (editor == 'Integra') {
                            $scope.pub.PUBLICACAO.PUB_CONTEUDO = retorno.result.txt;

                            $scope.carregarTexto('Resenha', $scope.pub.PUBLICACAO.PUB_CONTEUDO_RESENHA, 'PUB_CONTEUDO_RESENHA');
                        } else if (editor == 'Resenha') {
                            $scope.pub.PUBLICACAO.PUB_CONTEUDO_RESENHA = retorno.result.txt;

                            $scope.carregarTexto('ManchetePt', $scope.pub.PUB_MANCHETE_PORTAL, 'PUB_MANCHETE_PORTAL');
                        } else if (editor == 'ManchetePt') {
                            $scope.pub.PUB_MANCHETE_PORTAL = retorno.result.txt;

                            $scope.carregarTexto('EmentaPt', $scope.pub.PUB_EMENTA_PORTAL, 'PUB_EMENTA_PORTAL');
                        } else if (editor == 'EmentaPt') {
                            $scope.pub.PUB_EMENTA_PORTAL = retorno.result.txt;

                            $scope.carregarTexto('Publicar', $scope.pub.PUBLICAR_PORTAL, 'PUBLICAR_PORTAL');
                        } else if (editor == 'Publicar') {
                            $scope.pub.PUBLICAR_PORTAL = retorno.result.txt;
                            if (deveBuscarTexto)
                                $scope.buscarTexto(editor, $scope.pub.PUBLICAR_PORTAL, campo);
                            else
                                $scope.carregarTexto('Manchete', $scope.pub.PUB_MANCHETE, 'PUB_MANCHETE');
                        } else if (editor == 'Manchete') {
                            $scope.pub.PUB_MANCHETE = retorno.result.txt;

                            $scope.carregarTexto('Ementa', $scope.pub.PUB_EMENTA, 'PUB_EMENTA');
                        } else if (editor == 'Ementa') {
                            $scope.pub.PUB_EMENTA = retorno.result.txt;

                            $scope.carregarTexto('Remissao', $scope.pub.MATERIA_IMPRESSA, 'MATERIA_IMPRESSA');
                        } else if (editor == 'Remissao') {
                            $scope.pub.MATERIA_IMPRESSA = retorno.result.txt;
                            if (deveBuscarTexto)
                                $scope.buscarTexto(editor, $scope.pub.MATERIA_IMPRESSA, campo);

                            if (!forcarAtualizacao)
                                $scope.carregarTexto('PUB_CONTEUDO_RESENHA_RVO', $scope.pub.PUBLICACAO.PUB_CONTEUDO_RESENHA_RVO, 'PUB_CONTEUDO_RESENHA_RVO');
                        } else if (editor == 'PUB_CONTEUDO_RESENHA_RVO') {
                            $scope.pub.PUBLICACAO.PUB_CONTEUDO_RESENHA_RVO = retorno.result.txt;

                            $scope.carregarTexto('PUB_CONTEUDO_RVO', $scope.pub.PUBLICACAO.PUB_CONTEUDO_RVO, 'PUB_CONTEUDO_RVO');
                        } else if (editor == 'PUB_CONTEUDO_RVO') {
                            $scope.pub.PUBLICACAO.PUB_CONTEUDO_RVO = retorno.result.txt;

                            $scope.carregarTexto('PUB_CONTEUDO_RDC', $scope.pub.PUBLICACAO.PUB_CONTEUDO_RDC, 'PUB_CONTEUDO_RDC');
                        } else if (editor == 'PUB_CONTEUDO_RDC') {
                            $scope.pub.PUBLICACAO.PUB_CONTEUDO_RDC = retorno.result.txt;

                            $scope.carregarTexto('PUB_CONTEUDO_RESENHA_RDC', $scope.pub.PUBLICACAO.PUB_CONTEUDO_RESENHA_RDC, 'PUB_CONTEUDO_RESENHA_RDC');
                        } else if (editor == 'PUB_CONTEUDO_RESENHA_RDC') {
                            $scope.pub.PUBLICACAO.PUB_CONTEUDO_RESENHA_RDC = retorno.result.txt;

                            $scope.carregarTexto('PUB_CONTEUDO_RESENHA_RVT', $scope.pub.PUBLICACAO.PUB_CONTEUDO_RESENHA_RVT, 'PUB_CONTEUDO_RESENHA_RVT');
                        } else if (editor == 'PUB_CONTEUDO_RESENHA_RVT') {
                            $scope.pub.PUBLICACAO.PUB_CONTEUDO_RESENHA_RVT = retorno.result.txt;

                            $scope.carregarTexto('PUB_CONTEUDO_RVT', $scope.pub.PUBLICACAO.PUB_CONTEUDO_RVT, 'PUB_CONTEUDO_RVT');
                        } else if (editor == 'PUB_CONTEUDO_RVT') {
                            $scope.pub.PUBLICACAO.PUB_CONTEUDO_RVT = retorno.result.txt;

                            $scope.carregarTexto('PUB_CONTEUDO_RESENHA_DGT', $scope.pub.PUBLICACAO.PUB_CONTEUDO_RESENHA_DGT, 'PUB_CONTEUDO_RESENHA_DGT');
                        } else if (editor == 'PUB_CONTEUDO_RESENHA_DGT') {
                            $scope.pub.PUBLICACAO.PUB_CONTEUDO_RESENHA_DGT = retorno.result.txt;
                            $scope.carregouTudo = true;
                        }
                    }
                }
            });
        }
    }

    // buscarTexto
    $scope.buscarTexto = function (editor, dado, campo) {
        var edt = objEditor(editor);
        if (edt) {
            edt = edt.editor;
            if (!(typeof edt === "undefined")) {
                edt.SetText(!dado ? "" : dado);
                edt.SetWidth($scope.editorLargura);
                edt.SetHeight($scope.editorAltura);
                edt.Focus();

                // define como editor ativo
                $scope.editorAtivo = editor;
                $scope.editorAtivoOnEnter = true; // alterei na entrada, então desconsidere
            }
        }
    }

    // converter em único parágrafo
    //selecionado = jsml.html_decode(selecionado); // limpar tags HTML
    $scope.converterEmParagrafoUnico = function (editor) {
        var edt = objEditor(editor);
        if (edt) {
            edt = edt.editor;
            if (!(typeof edt === "undefined")) {
                var textoSelecionado = edt.ExtractRangeHTML();
                if (textoSelecionado) {
                    textoSelecionado = textoSelecionado.replace(/[<]br[^>]*[>]/gi, "");
                    edt.PasteHTML(textoSelecionado);
                }
            }
        }
    }

    // atualizar matéria impressa
    $scope.atualizarMateriaImpressa = function () {
        formHandlerService.read($scope, {
            url: Util.getUrl("/publicacaoAreaConsultoria/atualizarMateriaImpressaJson"),
            showAjaxLoader: true,
            targetObjectName: 'txt',
            responseModelName: 'txt',
            data: { _pubArea: $scope.pub },
            success: function (retorno) {
                $scope.pub.MATERIA_IMPRESSA = retorno.result.txt;
            }
        });
    }

    // atualizar publicar portal
    $scope.atualizarPublicarPortal = function () {
        formHandlerService.read($scope, {
            url: Util.getUrl("/publicacaoAreaConsultoria/atualizarPublicarPortalJson"),
            showAjaxLoader: true,
            targetObjectName: 'txt',
            responseModelName: 'txt',
            data: { _pubArea: $scope.pub },
            success: function (retorno) {
                $scope.pub.PUBLICAR_PORTAL = retorno.result.txt;
            }
        });
    }

    //
    $scope.painelModal = function () {
        angular.element("#painelModal").modal();
    }

    //
    $scope.filtroModal = function () {
        angular.element("#filtroModal").modal();
    }

    //
    $scope.trocouRevogacao = function () {
        $scope.mudouRevogacao = true;
    }

    // Modal - Histórico da Matéria...
    $scope.historicoMateria = function (msg) {
        $scope.msg = msg.replace(/;/g, '<br/><br/>');
        $scope.tit = "H I S T Ó R I C O   *   D A   *   M A T É R I A";
        angular.element("#historicoMateria").modal();
    }

    // identifica se é apenas visualização...
    $scope.soVisualizar = function () { $scope.lVisual = true; }

    // Modal - Exibir Matéria do Portal...
    $scope.BotaoMateriaNoPortal = function (id, colecionadorId, html) {
        $scope.id = id;
        if (!html) {
            formHandlerService.read($scope, {
                url: Util.getUrl("/publicacaoAreaConsultoria/HtmlDaMateria"),
                showAjaxLoader: true,
                targetObjectName: 'html',
                responseModelName: 'html',
                data: { modulo: id, colecionadorId: colecionadorId },
                success: function (retorno) {
                    if (!$scope.html) {
                        $scope.html = "O conteúdo desta matéria provavelmente foi associado e publicado incorretamente e não foi encontrado!";
                    }
                }
            });
        } else {
            $scope.html = html;
        }
        angular.element("#materiaNoPortal").modal();
    }

    // checarLinkMateria...
    $scope.checarLinkMateria = function () {
        $scope.exibirEditando();
    }

    // dataMaiorHoje...
    $scope.dataMaiorHoje = function (data) {
        var d = new Date();
        if (data > d) {
            alert("Esta data não pode ser superior à data de hoje!");
        }
    }

    // paginação dos índices e sumários...
    $scope.objPaginadorMateria = function () {
        if (!$scope.pagina) {
            $scope.pagina = [{}];
            $scope.pagina.lAlterouNumero = false;
        }
    }

    //*****************************************************************

    // eventos...
    if (typeof $scope.aoAlterar === "undefined") {
        $scope.aoAlterar = 1;
    }
    if (typeof $scope.aoSair === "undefined") {
        $scope.aoSair = 2;
    }

    // operacao...
    if (typeof $scope.pub === "undefined") {
        $scope.pub = { operacao: "C" };
    }

    // exibir ou não blocos de informações...
    if (typeof $scope.exibir === "undefined") {
        $scope.exibir = { filtro: true, painel: true };
    }
    if (typeof $scope.msgPainel === "undefined") {
        $scope.msgPainel = "clique aqui para não mostrar dados";
    } else {
        $scope.msgPainel = "clique aqui para mostrar dados";
    }
    $scope.exibirPainel = function () {
        $scope.exibir.painel = (!$scope.exibir.painel);
        $scope.msgPainel = $scope.exibir.painel ? "clique aqui para não mostrar dados" : "clique aqui para mostrar dados";
    }
    $scope.exibirFiltro = function () {
        $scope.exibir.filtro = (!$scope.exibir.filtro);
    }
    $scope.exibirEditando = function () {
        $scope.exibir.editando = (!$scope.exibir.editando);
    }
    $scope.exibirOrigem = function () {
        $scope.exibir.origem = (!$scope.exibir.origem);
    }
    $scope.exibirVeiculacao = function () {
        $scope.exibir.veiculacao = (!$scope.exibir.veiculacao);
    }
    $scope.exibirRevogacao = function () {
        $scope.exibir.revogacao = (!$scope.exibir.revogacao);
        var rev = $scope.pub.TMP_REVOGACAO;
        if (rev[rev.length - 1].TIP_ATO_ID && rev[rev.length - 1].PAR_TIPO !== "R") {
            $scope.adicionarRevogacaoAlteracao();
        }
    }
    $scope.exibirRevigoracao = function () {
        $scope.exibir.revigoracao = (!$scope.exibir.revigoracao);
        var rev = $scope.pub.TMP_REVOGACAO;
        if (rev[rev.length - 1].TIP_ATO_ID && rev[rev.length - 1].PAR_TIPO !== "V") {
            $scope.adicionarRevogacaoAlteracao();
        }
    }
    $scope.exibirAlteracao = function () {
        $scope.exibir.alteracao = (!$scope.exibir.alteracao);
        var rev = $scope.pub.TMP_REVOGACAO;
        if (rev[rev.length - 1].TIP_ATO_ID && rev[rev.length - 1].PAR_TIPO !== "A") {
            $scope.adicionarRevogacaoAlteracao();
        }
    }
    $scope.exibirMateriaPortal = function () {
        //editoresCarregados();
        if (!$scope.exibir.materiaPortal || $scope.pub.salvouAuto)
            $scope.exibir.materiaPortal = (!$scope.exibir.materiaPortal);

        //if ($scope.exibir.materiaPortal)
        //    $scope.buscarTextoParaEditorRTE('Matéria do Portal', 'PUBLICAR_PORTAL', "pub.PUBLICAR_PORTAL", "Publicar"); // MUDAR
    }
    $scope.exibirMateriaImpressa = function () {
        //editoresCarregados();
        if (!$scope.exibir.materiaImpressa || $scope.pub.salvouAuto)
            $scope.exibir.materiaImpressa = (!$scope.exibir.materiaImpressa);

        //if ($scope.exibir.materiaImpressa)
        //    $scope.buscarTextoParaEditorRTE('Matéria do Impresso', 'PRE_REMISSAO', "pub.MATERIA_IMPRESSA", "Remissao"); // MUDAR
    }
    $scope.exibirTitulacao = function () {
        $scope.exibir.titulacao = (!$scope.exibir.titulacao);
    }
    $scope.exibirIntegra = function () {
        //editoresCarregados();
        if (!$scope.exibir.integra || $scope.pub.salvouAuto)
            $scope.exibir.integra = (!$scope.exibir.integra);

        //if ($scope.exibir.integra)
        //    $scope.buscarTextoParaEditorRTE('Matéria no Portal', 'PUB_CONTEUDO', "pub.PUBLICACAO.PUB_CONTEUDO", "Integra");
    }
    $scope.exibirManchete = function () {
        //editoresCarregados();
        if (!$scope.exibir.manchete || $scope.pub.salvouAuto)
            $scope.exibir.manchete = (!$scope.exibir.manchete);

        //if ($scope.exibir.manchete)
        //    $scope.buscarTextoParaEditorRTE('Manchete no Impresso', 'PUB_MANCHETE', "pub.PUB_MANCHETE", "Manchete");
    }
    $scope.exibirManchetePt = function () {
        //editoresCarregados();
        if (!$scope.exibir.manchetePt || $scope.pub.salvouAuto)
            $scope.exibir.manchetePt = (!$scope.exibir.manchetePt);

        //if ($scope.exibir.manchetePt)
        //    $scope.buscarTextoParaEditorRTE('Manchete no Portal', 'PUB_MANCHETE_PORTAL', "pub.PUB_MANCHETE_PORTAL", "ManchetePt");
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
        //editoresCarregados();
        if (!$scope.exibir.ementa || $scope.pub.salvouAuto)
            $scope.exibir.ementa = (!$scope.exibir.ementa);

        //if ($scope.exibir.ementa)
        //    $scope.buscarTextoParaEditorRTE('Ementa no Impresso', 'PUB_EMENTA', "pub.PUB_EMENTA", "Ementa");
    }
    $scope.exibirEmentaPt = function () {
        //editoresCarregados();
        if (!$scope.exibir.ementaPt || $scope.pub.salvouAuto)
            $scope.exibir.ementaPt = (!$scope.exibir.ementaPt);

        //if ($scope.exibir.ementaPt)
        //    $scope.buscarTextoParaEditorRTE('Ementa no Portal', 'PUB_EMENTA_PORTAL', "pub.PUB_EMENTA_PORTAL", "EmentaPt");
    }
    $scope.exibirImpresso = function () {
        //editoresCarregados();
        if (!$scope.exibir.impresso || $scope.pub.salvouAuto)
            $scope.exibir.impresso = (!$scope.exibir.impresso);

        //if ($scope.exibir.impresso)
        //    $scope.buscarTextoParaEditorRTE('Matéria no Impresso', 'PUB_CONTEUDO_RESENHA', "pub.PUBLICACAO.PUB_CONTEUDO_RESENHA", "Resenha");
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

    // desabilitando os botões que acionam a gravação \\
    $scope.desabilitaGravacao = function () {
        var btWeb = document.getElementById("btWeb");
        if (btWeb)
            btWeb.disabled = true;

        var btNews = document.getElementById("btNews");
        if (btNews)
            btNews.disabled = true;

        var btEmenta = document.getElementById("btEmenta");
        if (btEmenta)
            btEmenta.disabled = true;

        var btSalvar = document.getElementById("btSalvar");
        if (btSalvar)
            btSalvar.disabled = true;

        $scope.exibirPublicarPortalNews();
    }

    // habilitando os botões que acionam a gravação \\
    $scope.habilitaGravacao = function () {
        var btWeb = document.getElementById("btWeb");
        if (btWeb)
            btWeb.disabled = false;

        var btNews = document.getElementById("btNews");
        if (btNews)
            btNews.disabled = false;

        var btEmenta = document.getElementById("btEmenta");
        if (btEmenta)
            btEmenta.disabled = false;

        var btSalvar = document.getElementById("btSalvar");
        if (btSalvar)
            btSalvar.disabled = false;

        $scope.exibirPublicarPortalNews();
    }

    // zeros à esquerda...
    $scope.pad = function (s) { return (s < 10) ? '0' + s : s; }

    // publicar na WEB
    $scope.publicarWEB = function () {
        if ($scope.pub.PUBLICACAO.PUB_DATA_PUB_ATO && $scope.pub.PUBLICACAO.PUB_DATA_ATO) {
            if ($scope.pub.PUBLICACAO.PUB_DATA_PUB_ATO < $scope.pub.PUBLICACAO.PUB_DATA_ATO) {
                alert('A T E N Ç Ã O! A data do ato não pode ser maior que a data de sua publicação!');
                return;
            }
            if ($scope.alteracao && $scope.alteracao.PUB_DATA_ATO && ($scope.pub.PUBLICACAO.PUB_DATA_PUB_ATO < $scope.alteracao.PUB_DATA_ATO)) {
                alert('A T E N Ç Ã O! A data da publicação deste ato não pode ser menor que a data do ato sendo alterado!');
                return;
            }
            if ($scope.revogacao && $scope.revogacao.PUB_DATA_ATO && ($scope.pub.PUBLICACAO.PUB_DATA_PUB_ATO < $scope.revogacao.PUB_DATA_ATO)) {
                alert('A T E N Ç Ã O! A data da publicação deste ato não pode ser menor que a data do ato sendo revogado!');
                return;
            }
            if ($scope.revigoracao && $scope.revigoracao.PUB_DATA_ATO && ($scope.pub.PUBLICACAO.PUB_DATA_PUB_ATO < $scope.revigoracao.PUB_DATA_ATO)) {
                alert('A T E N Ç Ã O! A data da publicação deste ato não pode ser menor que a data do ato sendo revigorado!');
                return;
            }

            if ($scope.alteracao && $scope.alteracao.PUB_DATA_ATO && ($scope.pub.PUBLICACAO.PUB_DATA_ATO < $scope.alteracao.PUB_DATA_ATO)) {
                alert('A T E N Ç Ã O! A data deste ato não pode ser menor que a data do ato sendo alterado!');
                return;
            }
            if ($scope.revogacao && $scope.revogacao.PUB_DATA_ATO && ($scope.pub.PUBLICACAO.PUB_DATA_ATO < $scope.revogacao.PUB_DATA_ATO)) {
                alert('A T E N Ç Ã O! A data deste ato não pode ser menor que a data do ato sendo revogado!');
                return;
            }
            if ($scope.revigoracao && $scope.revigoracao.PUB_DATA_ATO && ($scope.pub.PUBLICACAO.PUB_DATA_ATO < $scope.revigoracao.PUB_DATA_ATO)) {
                alert('A T E N Ç Ã O! A data deste ato não pode ser menor que a data do ato sendo revigorado!');
                return;
            }
        }
        if ($scope.pub.PUBLICACAO.PUB_CONTEUDO.toString() == '') {
            alert('Não é possível Publicar no Portal um Conteúdo ainda não informado!');
        } else if (confirm("Confirma a Publicação do CONTEÚDO e MANCHETE desta Matéria?")) {
            // data da publicacao...
            var d = new Date();
            var publicado = [d.getFullYear(), $scope.pad(d.getMonth() + 1), $scope.pad(d.getDate())].join('-') + ' ' + [$scope.pad(d.getHours()), $scope.pad(d.getMinutes()), $scope.pad(d.getSeconds())].join(':');
            $scope.pub.publicadoWeb = [$scope.pad(d.getDate()), $scope.pad(d.getMonth() + 1), d.getFullYear()].join('/') + ' ' + [$scope.pad(d.getHours()), $scope.pad(d.getMinutes()), $scope.pad(d.getSeconds())].join(':');

            // preparando CONFIG
            if ($scope.pub.PUBLICACAO_CONFIG.length == 0) {
                $scope.inicializaConfig();
            }

            // lançando valores...
            $scope.pub.PUBLICACAO_CONFIG[0].ARE_CONS_ID = $scope.pub.ARE_CONS_ID;
            $scope.pub.PUBLICACAO_CONFIG[0].PCF_PUB_WEB = true;
            $scope.pub.PUBLICACAO_CONFIG[0].PCF_DATA_PUB_WEB = publicado;
            $scope.pub.PUBLICACAO_CONFIG[0].PCF_DATA_PUB_MANCHETE = publicado;
            $scope.pub.PUBLICACAO_CONFIG[0].publicarEmenta = false;

            // salvando a matéria...
            var msg = "A íntegra da matéria e a manchete foram publicadas no Portal COAD com sucesso, em [ " + $scope.pub.publicadoWeb + " ]!";
            $scope.acionarGravacao(false, null, "0", msg);
            $scope.publicou = true;
        }
    }

    // publicar na NEWS
    $scope.publicarNEWS = function () {
        if ($scope.pub.PUBLICACAO.PUB_DATA_PUB_ATO && $scope.pub.PUBLICACAO.PUB_DATA_ATO) {
            if ($scope.pub.PUBLICACAO.PUB_DATA_PUB_ATO < $scope.pub.PUBLICACAO.PUB_DATA_ATO) {
                alert('A T E N Ç Ã O! A data do ato não pode ser maior que a data de sua publicação!');
                return;
            }
            if ($scope.alteracao && $scope.alteracao.PUB_DATA_ATO && ($scope.pub.PUBLICACAO.PUB_DATA_PUB_ATO < $scope.alteracao.PUB_DATA_ATO)) {
                alert('A T E N Ç Ã O! A data da publicação deste ato não pode ser menor que a data do ato sendo alterado!');
                return;
            }
            if ($scope.revogacao && $scope.revogacao.PUB_DATA_ATO && ($scope.pub.PUBLICACAO.PUB_DATA_PUB_ATO < $scope.revogacao.PUB_DATA_ATO)) {
                alert('A T E N Ç Ã O! A data da publicação deste ato não pode ser menor que a data do ato sendo revogado!');
                return;
            }
            if ($scope.revigoracao && $scope.revigoracao.PUB_DATA_ATO && ($scope.pub.PUBLICACAO.PUB_DATA_PUB_ATO < $scope.revigoracao.PUB_DATA_ATO)) {
                alert('A T E N Ç Ã O! A data da publicação deste ato não pode ser menor que a data do ato sendo revigorado!');
                return;
            }

            if ($scope.alteracao && $scope.alteracao.PUB_DATA_ATO && ($scope.pub.PUBLICACAO.PUB_DATA_ATO < $scope.alteracao.PUB_DATA_ATO)) {
                alert('A T E N Ç Ã O! A data deste ato não pode ser menor que a data do ato sendo alterado!');
                return;
            }
            if ($scope.revogacao && $scope.revogacao.PUB_DATA_ATO && ($scope.pub.PUBLICACAO.PUB_DATA_ATO < $scope.revogacao.PUB_DATA_ATO)) {
                alert('A T E N Ç Ã O! A data deste ato não pode ser menor que a data do ato sendo revogado!');
                return;
            }
            if ($scope.revigoracao && $scope.revigoracao.PUB_DATA_ATO && ($scope.pub.PUBLICACAO.PUB_DATA_ATO < $scope.revigoracao.PUB_DATA_ATO)) {
                alert('A T E N Ç Ã O! A data deste ato não pode ser menor que a data do ato sendo revigorado!');
                return;
            }
        }
        if ($scope.pub.PUBLICACAO.PUB_CONTEUDO.toString() == '') {
            alert('Não é possível Publicar no Portal um Conteúdo ainda não informado!');
        } else if (confirm("Confirma a Publicação na COAD NEWS?")) {
            // data da publicacao...
            var d = new Date();
            var publicado = [d.getFullYear(), $scope.pad(d.getMonth() + 1), $scope.pad(d.getDate())].join('-') + ' ' + [$scope.pad(d.getHours()), $scope.pad(d.getMinutes()), $scope.pad(d.getSeconds())].join(':');
            $scope.pub.publicadoNews = [$scope.pad(d.getDate()), $scope.pad(d.getMonth() + 1), d.getFullYear()].join('/') + ' ' + [$scope.pad(d.getHours()), $scope.pad(d.getMinutes()), $scope.pad(d.getSeconds())].join(':');

            // preparando CONFIG
            if ($scope.pub.PUBLICACAO_CONFIG.length == 0) {
                $scope.inicializaConfig();
            }

            // lançando valores...
            $scope.pub.PUBLICACAO_CONFIG[0].ARE_CONS_ID = $scope.pub.ARE_CONS_ID;
            $scope.pub.PUBLICACAO_CONFIG[0].PCF_PUB_NEWS = true;
            $scope.pub.PUBLICACAO_CONFIG[0].PCF_DATA_PUB_NEWS = publicado;
            $scope.pub.PUBLICACAO_CONFIG[0].publicarEmenta = false;

            // salvando a matéria...
            var msg = "A matéria foi publicada na COAD NEWS com sucesso, em [ " + $scope.pub.publicadoNews + " ]!";
            $scope.acionarGravacao(false, null, "0", msg);
            $scope.publicou = true;
        }
    }

    // publicar na EMENTA
    $scope.publicarEmenta = function () {
        if ($scope.pub.PUBLICACAO.PUB_DATA_PUB_ATO && $scope.pub.PUBLICACAO.PUB_DATA_ATO) {
            if ($scope.pub.PUBLICACAO.PUB_DATA_PUB_ATO < $scope.pub.PUBLICACAO.PUB_DATA_ATO) {
                alert('A T E N Ç Ã O! A data do ato não pode ser maior que a data de sua publicação!');
                return;
            }
            if ($scope.alteracao && $scope.alteracao.PUB_DATA_ATO && ($scope.pub.PUBLICACAO.PUB_DATA_PUB_ATO < $scope.alteracao.PUB_DATA_ATO)) {
                alert('A T E N Ç Ã O! A data da publicação deste ato não pode ser menor que a data do ato sendo alterado!');
                return;
            }
            if ($scope.revogacao && $scope.revogacao.PUB_DATA_ATO && ($scope.pub.PUBLICACAO.PUB_DATA_PUB_ATO < $scope.revogacao.PUB_DATA_ATO)) {
                alert('A T E N Ç Ã O! A data da publicação deste ato não pode ser menor que a data do ato sendo revogado!');
                return;
            }
            if ($scope.revigoracao && $scope.revigoracao.PUB_DATA_ATO && ($scope.pub.PUBLICACAO.PUB_DATA_PUB_ATO < $scope.revigoracao.PUB_DATA_ATO)) {
                alert('A T E N Ç Ã O! A data da publicação deste ato não pode ser menor que a data do ato sendo revigorado!');
                return;
            }

            if ($scope.alteracao && $scope.alteracao.PUB_DATA_ATO && ($scope.pub.PUBLICACAO.PUB_DATA_ATO < $scope.alteracao.PUB_DATA_ATO)) {
                alert('A T E N Ç Ã O! A data deste ato não pode ser menor que a data do ato sendo alterado!');
                return;
            }
            if ($scope.revogacao && $scope.revogacao.PUB_DATA_ATO && ($scope.pub.PUBLICACAO.PUB_DATA_ATO < $scope.revogacao.PUB_DATA_ATO)) {
                alert('A T E N Ç Ã O! A data deste ato não pode ser menor que a data do ato sendo revogado!');
                return;
            }
            if ($scope.revigoracao && $scope.revigoracao.PUB_DATA_ATO && ($scope.pub.PUBLICACAO.PUB_DATA_ATO < $scope.revigoracao.PUB_DATA_ATO)) {
                alert('A T E N Ç Ã O! A data deste ato não pode ser menor que a data do ato sendo revigorado!');
                return;
            }
        }
        if ($scope.pub.PUBLICACAO.PUB_CONTEUDO.toString() == '') {
            alert('Não é possível Publicar no Portal um Conteúdo ainda não informado!');
        } else if (confirm("Confirma a Publicação da EMENTA desta Matéria?")) {
            // data da publicacao...
            var d = new Date();
            var publicado = [d.getFullYear(), $scope.pad(d.getMonth() + 1), $scope.pad(d.getDate())].join('-') + ' ' + [$scope.pad(d.getHours()), $scope.pad(d.getMinutes()), $scope.pad(d.getSeconds())].join(':');
            $scope.pub.publicadoEmenta = [$scope.pad(d.getDate()), $scope.pad(d.getMonth() + 1), d.getFullYear()].join('/') + ' ' + [$scope.pad(d.getHours()), $scope.pad(d.getMinutes()), $scope.pad(d.getSeconds())].join(':');

            // preparando CONFIG
            if ($scope.pub.PUBLICACAO_CONFIG.length == 0) {
                $scope.inicializaConfig();
            }

            // lançando valores...
            $scope.pub.PUBLICACAO_CONFIG[0].ARE_CONS_ID = $scope.pub.ARE_CONS_ID;
            $scope.pub.PUBLICACAO_CONFIG[0].PCF_DATA_PUB_EMENTA = publicado;
            $scope.pub.PUBLICACAO_CONFIG[0].publicarEmenta = true;

            // salvando a matéria...
            var msg = "A ementa foi publicada com sucesso, em [ " + $scope.pub.publicadoEmenta + " ]!";
            $scope.acionarGravacao(false, null, "0", msg);
            $scope.publicou = true;
        }
    }

    // Sumario
    $scope.Sumario = function () {
        formHandlerService.read($scope, {
            url: Util.getUrl("/publicacaoAreaConsultoria/Sumario"),
            showAjaxLoader: true,
            targetObjectName: 'txt',
            responseModelName: 'txt',
            data: { inf: $scope.filtro.nrInformativo, ano: $scope.filtro.anoInformativo, colecionadorId: $scope.filtro.colecionadorId, baseDados: $scope.filtro.baseDados },
            success: function (retorno) {
                $scope.txt = "Sem matéria para o Sumário!";
                if (retorno.result.txt) {
                    $scope.txt = retorno.result.txt;
                    var editor = objEditor('Publicar');
                    editor.editor._geh_htmlcode = $scope.txt;
                    editor._geh_htmlcode = $scope.txt;
                }
            }
        });
    }

    // Indice Orientacoes
    $scope.IndiceOrientacoes = function () {
        formHandlerService.read($scope, {
            url: Util.getUrl("/publicacaoAreaConsultoria/textoIndiceOrientacoes"),
            showAjaxLoader: true,
            targetObjectName: 'txt',
            responseModelName: 'txt',
            data: { ano: $scope.filtro.anoInformativo, colecionadorId: $scope.filtro.colecionadorId },
            success: function (retorno) {
                $scope.txt = "Sem matéria para o Índice!";
                if (retorno.result.txt) {
                    $scope.txt = retorno.result.txt;
                    var editor = objEditor('Publicar');
                    editor.editor._geh_htmlcode = $scope.txt;
                    editor._geh_htmlcode = $scope.txt;
                }
            }
        });
    }

    // Indice Remissivo
    $scope.IndiceRemissivo = function () {
        formHandlerService.read($scope, {
            url: Util.getUrl("/publicacaoAreaConsultoria/textoIndiceRemissivo"),
            showAjaxLoader: true,
            targetObjectName: 'txt',
            responseModelName: 'txt',
            data: { ano: $scope.filtro.anoInformativo, colecionadorId: $scope.filtro.colecionadorId },
            success: function (retorno) {
                $scope.txt = "Sem matéria para o Índice!";
                if (retorno.result.txt) {
                    $scope.txt = retorno.result.txt;
                    var editor = objEditor('Publicar');
                    editor.editor._geh_htmlcode = $scope.txt;
                    editor._geh_htmlcode = $scope.txt;
                }
            }
        });
    }

    // Indice Atos
    $scope.IndiceAtos = function () {
        formHandlerService.read($scope, {
            url: Util.getUrl("/publicacaoAreaConsultoria/textoIndiceAtos"),
            showAjaxLoader: true,
            targetObjectName: 'txt',
            responseModelName: 'txt',
            data: { ano: $scope.filtro.anoInformativo, colecionadorId: $scope.filtro.colecionadorId },
            success: function (retorno) {
                $scope.txt = "Sem matéria para o Índice!";
                if (retorno.result.txt) {
                    $scope.txt = retorno.result.txt;
                    var editor = objEditor('Publicar');
                    editor.editor._geh_htmlcode = $scope.txt;
                    editor._geh_htmlcode = $scope.txt;
                }
            }
        });
    }

    // Indice Revogacoes
    $scope.IndiceRevogacoes = function () {
        formHandlerService.read($scope, {
            url: Util.getUrl("/publicacaoAreaConsultoria/textoIndiceRevogacoes"),
            showAjaxLoader: true,
            targetObjectName: 'txt',
            responseModelName: 'txt',
            data: { ano: $scope.filtro.anoInformativo, colecionadorId: $scope.filtro.colecionadorId },
            success: function (retorno) {
                $scope.txt = "Sem matéria para o Índice!";
                if (retorno.result.txt) {
                    $scope.txt = retorno.result.txt;
                    var editor = objEditor('Publicar');
                    editor.editor._geh_htmlcode = $scope.txt;
                    editor._geh_htmlcode = $scope.txt;
                }
            }
        });
    }

    $scope.copiaIntegra = function () {
        $scope.pub.PUBLICACAO.PUB_CONTEUDO_RESENHA = $scope.pub.PUBLICACAO.PUB_CONTEUDO;
        $scope.buscarTexto('Resenha', $scope.pub.PUBLICACAO.PUB_CONTEUDO_RESENHA, 'PUB_CONTEUDO_RESENHA');
    }

    $scope.copiaManchete = function () {
        $scope.pub.PUB_MANCHETE = $scope.pub.PUB_MANCHETE_PORTAL;
        $scope.buscarTexto('Manchete', $scope.pub.PUB_MANCHETE, 'PUB_MANCHETE');
    }

    $scope.copiaEmenta = function () {
        $scope.pub.PUB_EMENTA = $scope.pub.PUB_EMENTA_PORTAL;
        $scope.buscarTexto('Ementa', $scope.pub.PUB_EMENTA, 'PUB_EMENTA');
    }

    // texto atualizado...
    $scope.atualizarTextos = function () {
        formHandlerService.read($scope, {
            url: Util.getUrl("/publicacaoAreaConsultoria/textoAtualizado"),
            showAjaxLoader: true,
            targetObjectName: 'txt',
            responseModelName: 'txt',
            data: { publicacaoId: $scope.pub.PUB_ID, colecionadorId: $scope.pub.ARE_CONS_ID, cargo: $scope.cargoSigla },
            success: function (retorno) {
                if (retorno.result.txt) {
                    $scope.pub.PUBLICAR_PORTAL = retorno.result.txt.PUBLICAR_PORTAL;
                    $scope.pub.MATERIA_IMPRESSA = retorno.result.txt.MATERIA_IMPRESSA;
                    if ($scope.cargoSigla == 'DGT') {
                        //$scope.pub.PUBLICACAO.PUB_CONTEUDO_RESENHA_DGT = $scope.pub.MATERIA_IMPRESSA;
                    }
                    // adicionando as remissões...
                    for (var i = 0; i < $scope.pub.TMP_REMISSAO.length; i++) {
                        if ($scope.pub.TMP_REMISSAO[i].PRE_REMISSAO) {                                 // <<remissaoX>>
                            $scope.pub.TMP_REMISSAO[i].PRE_NUMERO = (i + 1);                         // numerando a remissão...
                            $scope.pub.MATERIA_IMPRESSA = $scope.pub.MATERIA_IMPRESSA.replace("&lt;&lt;" + "remissao" + (i + 1).toString() + "&gt;&gt;", $scope.pub.TMP_REMISSAO[i].PRE_REMISSAO);
                        }
                    }
                }
            }
        });
    }

    // texto do portal com cabeçalho...
    $scope.txtPublicarPortal = function (adicionaCabeca, evento) {
        // adicionando a íntegra do texto...
        $scope.pub.PUBLICAR_PORTAL = $scope.pub.PUBLICACAO.CABECALHO + $scope.pub.PUBLICACAO.PUB_CONTEUDO;

        // pegando a cabeça...
        $scope.cabecaMateria = $scope.cabecaMateria ? $scope.cabecaMateria : $scope.materiaImpressa();

        // anexando a cabeça da matéria...
        if (adicionaCabeca && $scope.cabecaMateria) {
            var texto = $scope.materiaImpressa();
            if (texto) {
                var nrMateria = "";
                if ($scope.pub.PUB_ID) {
                    nrMateria = "Matéria: " + $scope.pub.PUB_ID.toString() + "<br>";
                }
                $scope.pub.PUBLICAR_PORTAL = nrMateria + texto;
            }
        }
    };

    // texto com remissao...
    $scope.txtRemissao = function (adicionaCabeca) {
        // adicionando as remissões...
        $scope.pub.MATERIA_IMPRESSA = $scope.pub.PUBLICACAO.PUB_CONTEUDO_RESENHA;
        for (var i = 0; i < $scope.pub.TMP_REMISSAO.length; i++) {
            if ($scope.pub.TMP_REMISSAO[i].PRE_REMISSAO) {                                 // <<remissaoX>>
                $scope.pub.TMP_REMISSAO[i].PRE_NUMERO = (i + 1);                         // numerando a remissão...
                $scope.pub.MATERIA_IMPRESSA = $scope.pub.MATERIA_IMPRESSA.replace("&lt;&lt;" + "remissao" + (i + 1).toString() + "&gt;&gt;", $scope.pub.TMP_REMISSAO[i].PRE_REMISSAO);
            }
        }

        // pegando a cabeça...
        $scope.cabecaMateria = $scope.cabecaMateria ? $scope.cabecaMateria : $scope.materiaImpressa();

        // anexando a cabeça da matéria...
        if (adicionaCabeca && $scope.cabecaMateria) {
            var texto = $scope.materiaImpressa();
            if (texto) {
                var nrMateria = "";
                if ($scope.pub.PUB_ID) {
                    nrMateria = "Matéria: " + $scope.pub.PUB_ID.toString() + "<br>";
                }
                $scope.pub.MATERIA_IMPRESSA = nrMateria + texto + $scope.pub.MATERIA_IMPRESSA;
            }
        }
    };

    // matéria impressa - adicionando gg/vb/svb...
    $scope.materiaImpressa = function () {
        var texto = "";

        if ($scope.nomeSVB) {
            // preparando as datas...
            var d = $scope.pub.PUBLICACAO.PUB_DATA_ATO;
            var dtAto = [$scope.pad(d.getDate()), $scope.pad(d.getMonth() + 1), d.getFullYear()].join('/'); // + ' ' + [$scope.pad(d.getHours()), $scope.pad(d.getMinutes()), $scope.pad(d.getSeconds())].join(':');

            var d = $scope.pub.PUBLICACAO.PUB_DATA_PUB_ATO;
            var dtPub = [$scope.pad(d.getDate()), $scope.pad(d.getMonth() + 1), d.getFullYear()].join('/'); // + ' ' + [$scope.pad(d.getHours()), $scope.pad(d.getMinutes()), $scope.pad(d.getSeconds())].join(':');

            // substituindo os valores...
            texto = $scope.cabecaMateria;
            texto = texto.replace("&lt;&lt;" + "gg" + "&gt;&gt;", $scope.nomeGG);
            texto = texto.replace("&lt;&lt;" + "vb" + "&gt;&gt;", $scope.nomeVB);
            texto = texto.replace("&lt;&lt;" + "svb" + "&gt;&gt;", $scope.nomeSVB);
            if ($scope.pub.PUBLICACAO.TIP_ATO_ID != null) {
                texto = texto.replace("&lt;&lt;" + "tpAto" + "&gt;&gt;", $scope.LeiaDropDown("tpAto"));
                texto = texto.replace("&lt;&lt;" + "nrAto" + "&gt;&gt;", $scope.pub.PUBLICACAO.PUB_NUMERO_ATO);
                texto = texto.replace("&lt;&lt;" + "dtAto" + "&gt;&gt;", dtAto);
                texto = texto.replace("&lt;&lt;" + "orgao" + "&gt;&gt;", $scope.LeiaDropDown("orgaoId"));
                texto = texto.replace("&lt;&lt;" + "dtPub" + "&gt;&gt;", dtPub);
                texto = texto.replace("&lt;&lt;" + "veiculo" + "&gt;&gt;", $scope.LeiaDropDown("veiculoId"));
            } else {
                texto = texto.replace("&lt;&lt;" + "tpAto" + "&gt;&gt;", $scope.LeiaDropDown("tpMateria"));
                texto = texto.replace(",", "");
                texto = texto.replace("DE", "");
                texto = texto.replace("&lt;&lt;" + "nrAto" + "&gt;&gt;", "");
                texto = texto.replace("&lt;&lt;" + "dtAto" + "&gt;&gt;", "");
                texto = texto.replace("&lt;&lt;" + "orgao" + "&gt;&gt;", "");
                texto = texto.replace("&lt;&lt;" + "dtPub" + "&gt;&gt;", "");
                texto = texto.replace("&lt;&lt;" + "veiculo" + "&gt;&gt;", "");
            }
            texto = texto.replace("&lt;&lt;" + "manchete" + "&gt;&gt;", $scope.pub.PUB_MANCHETE);
            texto = texto.replace("&lt;&lt;" + "ementa" + "&gt;&gt;", $scope.pub.PUB_EMENTA);

            // inserindo a revogação...
            var revogando = false;
            var x;
            for (var i = 0; i < $scope.pub.TMP_REVOGACAO.length; i++) {
                if ($scope.pub.TMP_REVOGACAO[i].PUB_NUMERO_ATO) {

                    //var ato = parseInt($scope.pub.TMP_REVOGACAO[i].PUB_NUMERO_ATO.match(/\d/g).join(''));
                    var ato = $scope.pub.TMP_REVOGACAO[i].PUB_NUMERO_ATO;
                    var d = $scope.pub.TMP_REVOGACAO[i].PUB_DATA_ATO;
                    var dtAto = [$scope.pad(d.getDate()), $scope.pad(d.getMonth() + 1), d.getFullYear()].join('/');

                    x = i + 1;

                    if ($scope.pub.TMP_REVOGACAO[i].PAR_TIPO == 'A') {
                        revogando = "ESTE ATO ALTEROU " + $scope.LeiaDropDown("tpAtoAlterado" + x.toString()) + " Nº " + ato + " DE " + dtAto + " - " + $scope.pub.TMP_REVOGACAO[i].PUB_ALTERACAO_ATO;
                    } else if ($scope.pub.TMP_REVOGACAO[i].PAR_TIPO == 'R') {
                        revogando = "ESTE ATO REVOGOU " + $scope.LeiaDropDown("tpAtoRevogado" + x.toString()) + " Nº " + ato + " DE " + dtAto + " - " + $scope.pub.TMP_REVOGACAO[i].PUB_ALTERACAO_ATO;
                    } else {
                        revogando = "ESTE ATO REVIGOROU " + $scope.LeiaDropDown("tpAtoRevigorado" + x.toString()) + " Nº " + ato + " DE " + dtAto + " - " + $scope.pub.TMP_REVOGACAO[i].PUB_ALTERACAO_ATO;
                    }
                }
            }
            if (revogando) {
                texto = texto.replace("&lt;&lt;" + "revogado" + "&gt;&gt;", revogando);
            }
        }

        return texto;
    }

    // remissao, remissivo, p.chave, titulacao, uf, config...
    $scope.MudouColecionador = function () {
        // remissão...
        $scope.pub.PUBLICACAO_REMISSAO = angular.copy($scope.pub.TMP_REMISSAO);
        for (var i = 0; i < $scope.pub.PUBLICACAO_REMISSAO.length; i++) {
            $scope.pub.PUBLICACAO_REMISSAO[i].ARE_CONS_ID = $scope.pub.ARE_CONS_ID;
            $scope.pub.PUBLICACAO_REMISSAO[i].PRE_NUMERO = (i + 1);
            if (!$scope.pub.PUBLICACAO_REMISSAO[i].PRE_REMISSAO) {
                $scope.pub.PUBLICACAO_REMISSAO.splice(i, 1);
            }
        }

        // remissivo...
        $scope.pub.PUBLICACAO_REMISSIVO = angular.copy($scope.pub.TMP_REMISSIVO);
        for (var i = 0; i < $scope.pub.PUBLICACAO_REMISSIVO.length; i++) {
            $scope.pub.PUBLICACAO_REMISSIVO[i].ARE_CONS_ID = $scope.pub.ARE_CONS_ID;
            if (!$scope.pub.PUBLICACAO_REMISSIVO[i].PRE_REMISSIVO) {
                $scope.pub.PUBLICACAO_REMISSIVO.splice(i, 1);
            }
        }

        // palavra chave...
        $scope.pub.PUBLICACAO_PALAVRA_CHAVE = angular.copy($scope.pub.TMP_PALAVRA_CHAVE);
        for (var i = 0; i < $scope.pub.PUBLICACAO_PALAVRA_CHAVE.length; i++) {
            $scope.pub.PUBLICACAO_PALAVRA_CHAVE[i].ARE_CONS_ID = $scope.pub.ARE_CONS_ID;
            if (!$scope.pub.PUBLICACAO_PALAVRA_CHAVE[i].PPC_TEXTO) {
                $scope.pub.PUBLICACAO_PALAVRA_CHAVE.splice(i, 1);
            }
        }

        // titulação...
        $scope.pub.PUBLICACAO_TITULACAO = angular.copy($scope.pub.TMP_TITULACAO);
        for (var i = 0; i < $scope.pub.PUBLICACAO_TITULACAO.length; i++) {
            $scope.pub.PUBLICACAO_TITULACAO[i].ARE_CONS_ID = $scope.pub.ARE_CONS_ID;
            if (!$scope.pub.PUBLICACAO_TITULACAO[i].TIT_ID || !$scope.pub.PUBLICACAO_TITULACAO[i].TIT_ID_VERBETE || !$scope.pub.PUBLICACAO_TITULACAO[i].TIT_ID_SUBVERBETE) {
                $scope.pub.PUBLICACAO_TITULACAO.splice(i, 1);
            }
        }

        // publicação ufs...
        $scope.TMP_UF = angular.copy($scope.pub.PUB_UF);

        if ($scope.pub.ARE_CONS_ID == '2') { // ICMS possui um ou mais estados...
            for (var i = 0; i < $scope.TMP_UF.length; i++) {
                $scope.TMP_UF[i].ARE_CONS_ID = $scope.pub.ARE_CONS_ID;
                $scope.TMP_UF[i].INF_NUMERO = $scope.TMP_UF[i].INF_ANO.substring(5, 10);
                $scope.TMP_UF[i].INF_ANO = $scope.TMP_UF[i].INF_ANO.substring(0, 4);
                $scope.TMP_UF[i].PUB_ATIVO = true;
            }
        } else { // diferente de ICMS, todos os estados = "TD"...
            if ($scope.TMP_UF.length == 0) {
                $scope.inicializaUf();
            }
            $scope.TMP_UF[0].ARE_CONS_ID = $scope.pub.ARE_CONS_ID;
            $scope.TMP_UF[0].UF_ID = "TD";
            $scope.TMP_UF[0].INF_NUMERO = $scope.TMP_UF[0].INF_ANO.substring(5, 10);
            $scope.TMP_UF[0].INF_ANO = $scope.TMP_UF[0].INF_ANO.substring(0, 4);
            $scope.TMP_UF[0].PUB_ATIVO = true;
        }
        // removendo caso não esteja preenchido...
        if ($scope.TMP_UF.length > 0 && (!$scope.TMP_UF[0].UF_ID || !$scope.TMP_UF[0].INF_ANO || !$scope.TMP_UF[0].INF_NUMERO)) {
            $scope.TMP_UF.splice(0, 1);
        }
        // preparando para salvar publicação uf...
        $scope.pub.PUBLICACAO_UF = $scope.TMP_UF;

        // config...
        //$scope.pub.PUBLICACAO_CONFIG = angular.copy($scope.pub.TMP_CONFIG);
        for (var i = 0; i < $scope.pub.PUBLICACAO_CONFIG.length; i++) {
            $scope.pub.PUBLICACAO_CONFIG[i].ARE_CONS_ID = $scope.pub.ARE_CONS_ID;
        }
        // removendo caso não esteja preenchido...
        if (($scope.pub.PUBLICACAO_CONFIG.length > 0) &&
            (!$scope.pub.PUBLICACAO_CONFIG[0].PCF_DATA_PUB_WEB &&
                !$scope.pub.PUBLICACAO_CONFIG[0].PCF_DATA_PUB_NEWS &&
                !$scope.pub.PUBLICACAO_CONFIG[0].PCF_DATA_PUB_MANCHETE &&
                !$scope.pub.PUBLICACAO_CONFIG[0].PCF_DATA_PUB_EMENTA)) {
            $scope.pub.PUBLICACAO_CONFIG.splice(0, 1);
        }

        // revogação e alteração...
        // removendo caso não esteja preenchido...
        $scope.pub.PUBLICACAO.PUBLICACAO_ALTERACAO_REVOGACAO = angular.copy($scope.pub.TMP_REVOGACAO);
        var i = 0;
        while (i < $scope.pub.PUBLICACAO.PUBLICACAO_ALTERACAO_REVOGACAO.length) {
            if (!$scope.pub.PUBLICACAO.PUBLICACAO_ALTERACAO_REVOGACAO[i].TIP_ATO_ID) {
                $scope.pub.PUBLICACAO.PUBLICACAO_ALTERACAO_REVOGACAO.splice(i, 1);
                i--;
            }
            i++;
        }
    }
    //*************************************************************

    // declarando revogacao e alteração de atos... ARRAY
    $scope.iniciazaRevogacaoAlteracao = function () {
        if (!$scope.pub.TMP_REVOGACAO || $scope.pub.TMP_REVOGACAO.length == 0) {
            $scope.pub.TMP_REVOGACAO = [{ PAR_TIPO: "R" }, { PAR_TIPO: "V" }, { PAR_TIPO: "A" }];
        } else {
            var lst = [];
            for (var i = 0; i < 3; i++) {
                if (i < $scope.pub.TMP_REVOGACAO.length) {
                    lst[i] = $scope.pub.TMP_REVOGACAO[i].PAR_TIPO;
                } else {
                    if (lst.indexOf("R") == -1) {
                        $scope.adicionarRevogacaoAlteracao("R");
                    }
                    if (lst.indexOf("V") == -1) {
                        $scope.adicionarRevogacaoAlteracao("V");
                    }
                    if (lst.indexOf("A") == -1) {
                        $scope.adicionarRevogacaoAlteracao("A");
                    }
                }
            }
        }
    }

    // adicionando revogacao e alteração de atos... ARRAY
    $scope.adicionarRevogacaoAlteracao = function (tipo) {
        $scope.pub.TMP_REVOGACAO.push({ PAR_TIPO: tipo });
    }

    // removendo revogacao e alteração de atos... ARRAY
    $scope.removerRevogacaoAlteracao = function (revogacao, index) {
        if (confirm("Confirmar exclusão")) {
            if ($scope.pub.TMP_REVOGACAO.length > 1) {
                $scope.pub.TMP_REVOGACAO.splice(index, 1);
            } else {
                $scope.inicializaRevogacaoAlteracao();
            }
        }
    }

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
        $scope.pub.PUB_UF = [{ UF_ID: $scope.pub.ARE_CONS_ID == 2 ? "" : "TD", ARE_CONS_ID: $scope.pub.ARE_CONS_ID, INF_ANO: "", INF_NUMERO: "", PUB_ATIVO: true }];
    }

    // adicionando Estados... ARRAY
    $scope.adicionarUf = function () {
        $scope.pub.PUB_UF.push({ UF_ID: $scope.pub.ARE_CONS_ID == 2 ? "" : "TD", ARE_CONS_ID: $scope.pub.ARE_CONS_ID, INF_ANO: "", INF_NUMERO: "", PUB_ATIVO: true });
    }

    // removendo Estados... ARRAY
    $scope.removerUf = function (uf, index) {
        if (confirm("Confirmar exclusão")) {
            if ($scope.pub.PUB_UF.length > 1) {
                $scope.pub.PUB_UF.splice(index, 1);
            } else {
                $scope.inicializaUf();
            }
        }
    };
    //*************************************************************

    // declarando palavra chave... ARRAY
    $scope.inicializaPalavraChave = function () {
        $scope.pub.TMP_PALAVRA_CHAVE = [{ PPC_TEXTO: '', ARE_CONS_ID: $scope.pub.ARE_CONS_ID }];
    }

    // adicionando palavra chave... ARRAY
    $scope.adicionarPalavraChave = function () {
        $scope.pub.TMP_PALAVRA_CHAVE.push({ PPC_TEXTO: '', ARE_CONS_ID: $scope.pub.ARE_CONS_ID });
    }

    // removendo palavra chave... ARRAY
    $scope.removerPalavraChave = function (palavraChave, index) {
        if (confirm("Confirmar exclusão")) {
            if ($scope.pub.TMP_PALAVRA_CHAVE.length > 1) {
                $scope.pub.TMP_PALAVRA_CHAVE.splice(index, 1);
            } else {
                $scope.inicializaPalavraChave();
            }
        }
    };
    //*************************************************************

    // declarando remissao... ARRAY
    $scope.inicializaRemissao = function () {
        $scope.pub.TMP_REMISSAO = [{ ARE_CONS_ID: $scope.pub.ARE_CONS_ID, MATERIA_IMPRESSA: "", PRE_NUMERO: "" }];
    }

    // adicionando remissao... ARRAY
    $scope.adicionarRemissao = function () {
        $scope.pub.TMP_REMISSAO.push({ ARE_CONS_ID: $scope.pub.ARE_CONS_ID, MATERIA_IMPRESSA: "", PRE_NUMERO: "" });
    };

    // removendo remissao... ARRAY
    $scope.removerRemissao = function (remissao, index) {
        if (confirm("Confirmar exclusão")) {
            if ($scope.pub.TMP_REMISSAO.length > 1) {
                $scope.pub.TMP_REMISSAO.splice(index, 1);
            } else {
                $scope.adicionarRemissao();
            }
        }
    };
    //*************************************************************

    // declarando remissivo... ARRAY
    $scope.inicializaRemissivo = function () {
        $scope.pub.TMP_REMISSIVO = [{}];
    }

    // adicionando remissivo... ARRAY
    $scope.adicionarRemissivo = function () {
        $scope.pub.TMP_REMISSIVO.push({});
    };

    // removendo remissivo... ARRAY
    $scope.removerRemissivo = function (remissivo, index) {
        if (confirm("Confirmar exclusão")) {
            if ($scope.pub.TMP_REMISSIVO.length > 1) {
                $scope.pub.TMP_REMISSIVO.splice(index, 1);
            } else {
                $scope.adicionarRemissivo();
            }
        }
    };
    //*************************************************************

    // declarando titulação... ARRAY
    $scope.inicializaTitulacao = function () {
        if (!$scope.pub.TMP_TITULACAO || $scope.pub.TMP_TITULACAO.length == 0) {
            $scope.pub.TMP_TITULACAO = [{ PTI_PRINCIPAL: true }];
            $scope.PTI_PRINCIPAL = $scope.pub.TMP_TITULACAO[0];
        }
    }

    //clicou no principal... FUNCIONANDO!
    $scope.marcouPrincipal = function (titulacao) {
        angular.forEach($scope.pub.TMP_TITULACAO, function (p) {
            p.PTI_PRINCIPAL = false;
        });
        titulacao.PTI_PRINCIPAL = true;
    };

    // adicionando titulação, verbetes e subverbetes... ARRAY
    $scope.adicionarTitulacao = function (titulacao, index) {
        $scope.pub.TMP_TITULACAO.push({
            gg: $scope.pub.TMP_TITULACAO[0].gg,
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
            if ($scope.pub.TMP_TITULACAO.length > 1) {
                $scope.pub.TMP_TITULACAO.splice(index, 1);
            } else {
                $scope.adicionarTitulacao();
            }
        }
    };

    // lendo o nome do gg/vb/svb...
    $scope.pegaNomeTitulacao = function (titulacao) {
        if (titulacao.gg != null) {
            for (var i = 0; i < titulacao.gg.length; i++) {
                if (titulacao.gg[i].TIT_ID == titulacao.TIT_ID) {
                    $scope.nomeGG = titulacao.gg[i].TIT_DESCRICAO;
                    break;
                }
            }
        }
        if (titulacao.verbetes != null) {
            for (var i = 0; i < titulacao.verbetes.length; i++) {
                if (titulacao.verbetes[i].TIT_ID == titulacao.TIT_ID_VERBETE) {
                    $scope.nomeVB = titulacao.verbetes[i].TIT_DESCRICAO;
                    break;
                }
            }
        }
        if (titulacao.subverbetes != null) {
            for (var i = 0; i < titulacao.subverbetes.length; i++) {
                if (titulacao.subverbetes[i].TIT_ID == titulacao.TIT_ID_SUBVERBETE) {
                    $scope.nomeSVB = titulacao.subverbetes[i].TIT_DESCRICAO;
                    break;
                }
            }
        }
    }

    // leia os grandes grupos do atual colecionador...
    $scope.lerGg = function () {
        // informou colecionador?
        if ($scope.pub.ARE_CONS_ID) {
            // array de UFs escolhidas...
            if ($scope.pub.PUB_UF && $scope.pub.PUB_UF.length > 0) {
                if ($scope.pub.PUB_UF.length == 1 && $scope.pub.PUB_UF[0].UF_ID == "TD") {
                    var ufs = null;
                } else {
                    var ufs = [];
                    for (var i = 0; i < $scope.pub.PUB_UF.length; i++) {
                        ufs[i] = $scope.pub.PUB_UF[i].UF_ID;
                    }
                }
            } else {
                var ufs = null;
            }
            // nome do colecionador...
            //$scope.colecionadorNome = $scope.LeiaDropDown("colecionadorNome");
            $scope.colecionadorNome = !$scope.colecionadorNome ? $scope.LeiaDropDown("colecionadorNome") : $scope.colecionadorNome;
            // limpa todas as titulações...
            $scope.inicializaTitulacao();
            // busca os seus grandes grupos...
            formHandlerService.read($scope, {
                url: Util.getUrl("/publicacaoAreaConsultoria/Gg"),
                showAjaxLoader: true,
                targetObjectName: 'gg',
                responseModelName: 'gg',
                data: { colecionadorId: $scope.pub.ARE_CONS_ID, ufs: ufs },
                success: function (retorno) {
                    for (var i = 0; i <= $scope.pub.TMP_TITULACAO.length - 1; i++) {
                        $scope.pub.TMP_TITULACAO[i].gg = retorno.result.gg;
                        $scope.pegaNomeTitulacao($scope.pub.TMP_TITULACAO[i]);
                    }
                }
            });
        }
        $scope.pub.TMP_TITULACAO.verbetes = null;
        $scope.pub.TMP_TITULACAO.subverbetes = null;
    }

    // Index: leia os grandes grupos do colecionador informado...
    $scope.lerGgIndex = function (colecionadorId) {
        // informou colecionador?
        $scope.filtro.ggId = null;
        $scope.filtro.vbId = null;
        $scope.filtro.svbId = null;
        $scope.filtro.gg = null;
        colecionadorId = colecionadorId == "" ? null : colecionadorId;
        // busca os seus grandes grupos...
        formHandlerService.read($scope, {
            url: Util.getUrl("/publicacaoAreaConsultoria/Gg"),
            showAjaxLoader: true,
            targetObjectName: 'gg',
            responseModelName: 'gg',
            data: { colecionadorId: colecionadorId },
            success: function (retorno) {
                if (!$scope.filtro.gg) {
                    $scope.filtro.gg = retorno.result.gg;
                }
            }
        });
        $scope.filtro.vb = null;
        $scope.filtro.svb = null;
    }

    // leia os verbetes do grande grupo escolhido...
    $scope.lerVerbetes = function (titulacao, index) {
        // informou grande grupo?
        if (titulacao.TIT_ID) {
            // busca os verbetes...
            formHandlerService.read($scope, {
                url: Util.getUrl("/publicacaoAreaConsultoria/Verbetes"),
                showAjaxLoader: true,
                targetObjectName: 'verbetes',
                responseModelName: 'verbetes',
                data: { ggId: titulacao.TIT_ID },
                success: function (retorno) {
                    titulacao.verbetes = retorno.result.verbetes;
                    $scope.pegaNomeTitulacao(titulacao);
                }
            });
        }
        titulacao.verbetes = null;
        titulacao.subverbetes = null;
    }

    // leia os verbetes do grande grupo escolhido...
    $scope.lerVerbetesIndex = function (ggId) {
        // informou grande grupo?
        if (ggId) {
            // limpando subverbetes...
            $scope.filtro.vb = null;
            $scope.filtro.ggId = ggId;
            // buscando...
            formHandlerService.read($scope, {
                url: Util.getUrl("/publicacaoAreaConsultoria/Verbetes"),
                showAjaxLoader: true,
                targetObjectName: 'verbetes',
                responseModelName: 'verbetes',
                data: { ggId: ggId },
                success: function (retorno) {
                    $scope.filtro.vb = retorno.result.verbetes;
                }
            });
        } else {
            $scope.filtro.ggId = null;
            $scope.filtro.vbId = null;
            $scope.filtro.svbId = null;
        }
        $scope.filtro.svb = null;
    }

    // leia os subverbetes do verbete escolhido...
    $scope.lerSubverbetes = function (titulacao, index) {
        if (titulacao.TIT_ID_VERBETE) {
            // informando o colecionador desta titulação...
            titulacao.ARE_CONS_ID = $scope.pub.ARE_CONS_ID;
            // buscando o subverbete...
            formHandlerService.read($scope, {
                url: Util.getUrl("/publicacaoAreaConsultoria/Subverbetes"),
                showAjaxLoader: true,
                targetObjectName: 'subverbetes',
                responseModelName: 'subverbetes',
                data: { vbId: titulacao.TIT_ID_VERBETE },
                success: function (retorno) {
                    titulacao.subverbetes = retorno.result.subverbetes;
                    $scope.pegaNomeTitulacao(titulacao);
                }
            });
        } else {
            titulacao.subverbetes = null;
        }
    }

    // leia nome do subverbete...
    $scope.lerNomeSubverbete = function (titulacao, index) {
        $scope.pegaNomeTitulacao(titulacao);
    }

    // leia os subverbetes do verbete escolhido...
    $scope.lerSubverbetesIndex = function (vbId, svbId) {
        // limpando os subverbetes...
        $scope.filtro.svb = null;
        $scope.filtro.vbId = vbId;
        $scope.filtro.svbId = svbId;
        if (vbId) {
            // carregando subverbetes...
            formHandlerService.read($scope, {
                url: Util.getUrl("/publicacaoAreaConsultoria/Subverbetes"),
                showAjaxLoader: true,
                targetObjectName: 'subverbetes',
                responseModelName: 'subverbetes',
                data: { vbId: vbId },
                success: function (retorno) {
                    $scope.filtro.svb = retorno.result.subverbetes;
                }
            });
        } else {
            $scope.filtro.vbId = null;
            $scope.filtro.svbId = null;
        }
    }

    // hints...
    $(document).ready(function () {
        $('[data-toggle="tooltip"]').tooltip();
    });

    $scope.prepararBuscarMateria = function () {
        showAjaxLoader();
        $scope.abrirAbasEditores = setInterval(function () { $scope.buscarMateria(); }, 3000);

        // abrindo abas dos editores...
        $scope.exibir.integra = true;
        $scope.exibir.materiaPortal = true;
        $scope.exibir.materiaImpressa = true;
        $scope.exibir.manchete = true;
        $scope.exibir.manchetePt = true;
        $scope.exibir.ementa = true;
        $scope.exibir.ementaPt = true;
        $scope.exibir.impresso = true;
        $scope.exibir.remissao = true;
    }

    // iniciando o filtro com ativo=true...
    $scope.init = function (colecionadorId) {
        $scope.filtro = { colecionadorId: colecionadorId, COADGED: "true" };
        $scope.painelAdm = {};
        $scope.lerGgIndex(colecionadorId);
        $scope._dadosLoginCarregar();
        $scope._basicoCarregar();
        $scope._origemCarregar();
        $scope._veiculacaoCarregar();
        $scope._titulacaoCarregar();
        $scope._localizacaoCarregar();
    }

    // calculando para o Painel...
    $scope.painelCalcular = function () {
        $scope.rdc = 0, $scope.rvt = 0, $scope.dgt = 0, $scope.rvo = 0, $scope.dia = 0, $scope.rp_rvt = 0, $scope.rp_dgt = 0, $scope.rp_rvo = 0, $scope.rp_col = 0, $scope.ed_rvt = 0, $scope.ed_dgt = 0, $scope.ed_rvo = 0;
        if ($scope.painel) {
            for (var i = 0; i < $scope.painel.length; i++) {
                $scope.rdc += reg.RDC;
            }
        }
    }

    // buscando dados para o Painel...
    $scope.painelEstatistico = function (pageRequest) {
        var url = Util.getUrl("/publicacaoAreaConsultoria/painel");
        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }
        var config = {
            url: url,
            showAjaxLoader: true,
            targetObjectName: 'painel',
            responseModelName: 'painel',
            pageConfig: { pageName: 'page' }
        };

        $scope.painelAdm.colaboradorId = $scope.filtro.pnColaboradorId;
        $scope.painelAdm.colecionadorId = $scope.filtro.pnColecionadorId;
        $scope.painelAdm.nrInformativo = $scope.filtro.pnNrInformativo;
        $scope.painelAdm.anoInformativo = $scope.filtro.pnAnoInformativo;

        config.data = angular.copy($scope.painelAdm);

        formHandlerService.read($scope, config);
        angular.element("#mostrarPainel").modal();
    };

    // buscando dados para o index...
    $scope.listar = function (pageRequest) {
        var url = Util.getUrl("/publicacaoAreaConsultoria/publicacoesAreaConsultoria");
        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }
        var config = {
            url: url,
            showAjaxLoader: true,
            targetObjectName: 'publicacoesAreaConsultoria',
            responseModelName: 'publicacoesAreaConsultoria',
            pageConfig: { pageName: 'page' }
        };
        if ($scope.filtro) {
            config.data = angular.copy($scope.filtro);
        }
        formHandlerService.read($scope, config);
    };

    // coadgedBI...
    $scope.coadgedBI = function (pageRequest, carregarMais) {

        $scope.apartirDe = $scope.apartirDe ? $scope.apartirDe : 0;
        $scope.registros = $scope.registros ? $scope.registros : 0;

        if (($scope.filtro.coadgedBI) && ($scope.filtro.coadgedBI.length < 4)) {
            alert("Por favor, informe um texto com mais de 4 caracteres!");
        } else {
            angular.element("#filtroModal").modal("hide");
            angular.element("#painelModal").modal("hide");

            var url = Util.getUrl("/publicacaoAreaConsultoria/CoadGedBI");

            if (pageRequest) {
                url += "?pagina=" + pageRequest;
            }

            if (carregarMais) {
                if (carregarMais > 0) {
                    $scope.apartirDe += $scope.registros;
                } else if ($scope.apartirDe > 0) {
                    $scope.apartirDe -= 100;
                }
                url += "?carregarMais=" + carregarMais;
                url += "&apartirDe=" + $scope.apartirDe;
            } else if (!pageRequest) {
                url += "?apartirDe=null";
            } else if (!carregarMais) {
                url += "&apartirDe=" + $scope.apartirDe;
            }

            $scope.filtro.COADGED = $scope.filtro.COADGED ? $scope.filtro.COADGED : true;
            url += "&COADGED=" + $scope.filtro.COADGED;

            $scope._coadgedBIretorno = $scope.coadgedBIretorno;

            var config = {
                url: url,
                showAjaxLoader: true,
                targetObjectName: 'coadgedBIretorno',
                responseModelName: 'coadgedBIretorno',
                pageConfig: { pageName: 'page' },
                success: function (resp, status, config, message, validationMessage) {
                    if (resp.result.coadgedBIretorno) {
                        $scope.total = (resp.result.total && resp.result.total[0]) ? resp.result.total[0].total : $scope.total;
                        if (resp.result.coadgedBIretorno.length == 0) {
                            $scope.coadgedBIretorno = $scope._coadgedBIretorno;
                            if (carregarMais > 0 || !carregarMais) {
                                $scope.apartirDe -= $scope.registros;
                            } else {
                                $scope.apartirDe += $scope.registros;
                            }
                        } else {
                            $scope.registros = resp.page.numeroRegistros;
                        }
                    }
                    if (resp.message) {
                        $scope.message = resp.message;
                    }
                },
                error: function (resp, message, validationMessage) {
                    alert(resp.message);
                }
            };
            if ($scope.filtro) {
                config.data = angular.copy($scope.filtro);
            }
            formHandlerService.read($scope, config);
        }
    }

    // buscar Ato Revogado/Alterado...
    $scope.BuscarAtoRevogado = function (indice, lImportar) {
        if ($scope.mudouRevogacao) {
            if ($scope.pub.TMP_REVOGACAO[indice].TIP_ATO_ID && $scope.pub.TMP_REVOGACAO[indice].PUB_NUMERO_ATO && $scope.pub.TMP_REVOGACAO[indice].PUB_DATA_ATO) {
                lImportar = lImportar ? lImportar : false;
                formHandlerService.read($scope, {
                    url: Util.getUrl("/publicacaoAreaConsultoria/BuscarAtoRevogado"),
                    showAjaxLoader: true,
                    targetObjectName: 'revogado',
                    responseModelName: 'revogado',
                    data: {
                        colecionadorId: $scope.pub.ARE_CONS_ID,
                        tipoR: $scope.pub.TMP_REVOGACAO[indice].PAR_TIPO,
                        tpAto: $scope.pub.TMP_REVOGACAO[indice].TIP_ATO_ID,
                        nrAto: $scope.pub.TMP_REVOGACAO[indice].PUB_NUMERO_ATO,
                        //nrAto: parseInt($scope.pub.TMP_REVOGACAO[indice].PUB_NUMERO_ATO.match(/\d/g).join('')),
                        dtAto: $scope.pub.TMP_REVOGACAO[indice].PUB_DATA_ATO,
                        importarSeNecessario: lImportar
                    },
                    success: function (resp, status, config, message, validationMessage) {
                        if (resp.result) {
                            if ((resp.result.revogado != null) && (resp.result.revogado.id)) // precisa importar? \\
                            {
                                if (confirm("Esta matéria consta apenas no PORTAL e precisa ser IMPORTADA para o COADGED antes da operação solicitada. Confirma a Importação?")) {
                                    $scope.BuscarAtoRevogado(indice, true); // importando...
                                }
                            } else if ((resp.result.revogado != null) && (resp.result.revogado.length > 0)) { // importou?...
                                $scope.mudouRevogacao = false;
                                var url = "/publicacaoAreaConsultoria/Editar?publicacaoId=" + resp.result.revogado[0] + "&colecionadorId=" + $scope.pub.ARE_CONS_ID;
                                post(url, true); // abra a matéria em outra aba para ser completada pelo usuário...
                            }
                            if ((resp.result.revogado == null) || (resp.result.revogado.length == 0)) {
                                $scope.mudouRevogacao = false;
                                if (resp.message && resp.message.message) {
                                    $scope.pub.TMP_REVOGACAO[indice].TIP_ATO_ID = null;
                                    $scope.pub.TMP_REVOGACAO[indice].PUB_NUMERO_ATO = null;
                                    $scope.pub.TMP_REVOGACAO[indice].PUB_DATA_ATO = null;
                                }
                            } else {
                                $scope.mudouRevogacao = false;
                                alert("A T E N Ç Ã O! A matéria foi importada (como ARQUIVADA) e aberta na aba ao lado. Por favor, complemente-a com as informações obrigatórias exigidas no cadastro.");
                            }
                        } else {
                            $scope.mudouRevogacao = false;
                        }
                    }
                });
            }
        }
    }

    // paginar...
    $scope.paginar = function (obj, linha, campo) {

        $scope.pagina.lAlterouNumero = true;

        if ($scope.pagina[linha].PUB_PAGINA_INDICE && (!(!isNaN(parseFloat($scope.pagina[linha].PUB_PAGINA_INDICE)) && isFinite($scope.pagina[linha].PUB_PAGINA_INDICE))) ||
            $scope.pagina[linha].PUB_PAGINA_SUMARIO && (!(!isNaN(parseFloat($scope.pagina[linha].PUB_PAGINA_SUMARIO)) && isFinite($scope.pagina[linha].PUB_PAGINA_SUMARIO)))) {

            alert("Por favor, informe apenas números para a Página!");

            if (campo == 'i') {
                $scope.pagina[linha].PUB_PAGINA_INDICE = "";
            } else {
                $scope.pagina[linha].PUB_PAGINA_SUMARIO = "";
            }
        }
    }

    // salvar paginação...
    $scope.salvarPaginacaoMateria = function () {
        formHandlerService.submit($scope, {
            url: Util.getUrl("/publicacaoAreaConsultoria/salvarPaginacaoMateria"),
            objectName: 'pagina',
            showAjaxLoader: true,
            success: function (resp, status, config, message, validationMessage) {
                $scope.message = message;
                $scope.erros = validationMessage;
                if (resp.success) {
                    alert("Salvo com sucesso.");
                    window.location = Util.getUrl("/PublicacaoAreaConsultoria/PaginarMateria");
                }
            }
        });
    }

    //
    $scope.atribuirDadosDaMateria = function () {
        // montando ano e remessa do informativo...
        $scope.pub.PUB_UF = angular.copy($scope.pub.PUBLICACAO_UF);
        for (var i = 0; i < $scope.pub.PUBLICACAO_UF.length; i++) {
            $scope.pub.PUB_UF[i].INF_ANO = $scope.pub.PUBLICACAO_UF[i].INF_ANO + '/' + $scope.pub.PUBLICACAO_UF[i].INF_NUMERO;
        }

        $scope.lerGg();

        // enchendo as titulações...
        $scope.pub.TMP_TITULACAO = angular.copy($scope.pub.PUBLICACAO_TITULACAO);
        for (var i = 0; i < $scope.pub.TMP_TITULACAO.length; i++) {
            $scope.lerVerbetes($scope.pub.TMP_TITULACAO[i]);
            $scope.lerSubverbetes($scope.pub.TMP_TITULACAO[i]);
        }
        // datando matérias já publicadas...
        for (var i = 0; i < $scope.pub.PUBLICACAO_CONFIG.length; i++) {
            $scope.publicou = true;
            // web...
            if ($scope.pub.PUBLICACAO_CONFIG[i].PCF_DATA_PUB_WEB) {
                var d = $scope.pub.PUBLICACAO_CONFIG[i].PCF_DATA_PUB_WEB;
                $scope.pub.publicadoWeb = [$scope.pad(d.getDate()), $scope.pad(d.getMonth() + 1), d.getFullYear()].join('/') + ' ' + [$scope.pad(d.getHours()), $scope.pad(d.getMinutes()), $scope.pad(d.getSeconds())].join(':');
            }
            // news...
            if ($scope.pub.PUBLICACAO_CONFIG[i].PCF_DATA_PUB_NEWS) {
                var d = $scope.pub.PUBLICACAO_CONFIG[i].PCF_DATA_PUB_NEWS;
                $scope.pub.publicadoNews = [$scope.pad(d.getDate()), $scope.pad(d.getMonth() + 1), d.getFullYear()].join('/') + ' ' + [$scope.pad(d.getHours()), $scope.pad(d.getMinutes()), $scope.pad(d.getSeconds())].join(':');
            }
            // ementa...
            if ($scope.pub.PUBLICACAO_CONFIG[i].PCF_DATA_PUB_EMENTA) {
                var d = $scope.pub.PUBLICACAO_CONFIG[i].PCF_DATA_PUB_EMENTA;
                $scope.pub.publicadoEmenta = [$scope.pad(d.getDate()), $scope.pad(d.getMonth() + 1), d.getFullYear()].join('/') + ' ' + [$scope.pad(d.getHours()), $scope.pad(d.getMinutes()), $scope.pad(d.getSeconds())].join(':');
            }
        }
        // atribui entidade para a tela...
        $scope.pub.TMP_PALAVRA_CHAVE = angular.copy($scope.pub.PUBLICACAO_PALAVRA_CHAVE);
        $scope.pub.TMP_REMISSAO = angular.copy($scope.pub.PUBLICACAO_REMISSAO);
        $scope.pub.TMP_REMISSIVO = angular.copy($scope.pub.PUBLICACAO_REMISSIVO);
        $scope.pub.TMP_REVOGACAO = angular.copy($scope.pub.PUBLICACAO.PUBLICACAO_ALTERACAO_REVOGACAO);
        // inicializando modelos vazios...
        $scope.inicializaModelosVazios();
        if ($scope.publicou && $scope.pub.PUB_ID_PORTAL) {
            if ($scope.ambienteProducao) {
                if ($scope.colecionadorNome.substring(0, 3) == "ATC") {
                    $scope.pub.linkMateria = "http://www.coad.com.br/busca/detalhe_31/" + $scope.pub.PUB_ID_PORTAL.toString();
                } else {
                    $scope.pub.linkMateria = "http://www.coad.com.br/busca/detalhe/" + $scope.pub.PUB_ID_PORTAL.toString() + "/30";
                }
            } else {
                if ($scope.colecionadorNome.substring(0, 3) == "ATC") {
                    $scope.pub.linkMateria = "http://portalcoadlinux.apc.intranet/busca/detalhe_31/" + $scope.pub.PUB_ID_PORTAL.toString();
                } else {
                    $scope.pub.linkMateria = "http://portalcoadlinux.apc.intranet/busca/detalhe/" + $scope.pub.PUB_ID_PORTAL.toString() + "/30";
                }
            }
        }

        // carregando textos
        if ($scope.operacao != "I") {
            $scope.carregarTexto('Integra', $scope.pub.PUBLICACAO.PUB_CONTEUDO, 'PUB_CONTEUDO');
        }
        $scope.pub.revisao = $scope.revisao;
    }

    // buscar matéria...
    $scope.buscarMateria = function () {
        clearInterval($scope.abrirAbasEditores);
        hideAjaxLoader();

        var mat_id = prompt("Informe o [nº da matéria] cadastrada", "");
        if (mat_id != null) {

            var incluir = $scope.pub.operacao;

            // executando a busca...
            formHandlerService.read($scope, {
                url: Util.getUrl("/publicacaoAreaConsultoria/buscarMateria"),
                showAjaxLoader: true,
                targetObjectName: 'pub',
                responseModelName: 'pub',
                data: { mat_id: mat_id, pubAreaCons: $scope.pub },
                success: function () {
                    if (!$scope.pub) {
                        alert("Ocorreu um erro! Informe imediatamente ao Setor de TI.");
                    } else {

                        $scope.pub.operacao = incluir;
                        $scope.pub.lIncluir = (incluir == "I");

                        $scope.atribuirDadosDaMateria();
                    }
                }
            });
        }
    }

    // buscando dados do ID para o index...
    $scope.read = function (publicacaoId, colecionadorId) {
        // recebeu os dois parâmetros?...
        if (publicacaoId && colecionadorId) {
            formHandlerService.read($scope, {
                url: Util.getUrl("/publicacaoAreaConsultoria/ReadpublicacaoAreaConsultoria"),
                showAjaxLoader: true,
                targetObjectName: 'pub',
                responseModelName: 'pub',
                dateFieldsNames: ['DATA_CADASTRO', 'DATA_ALTERA', 'DATA_EXCLUSAO'],
                data: { publicacaoId: publicacaoId, colecionadorId: colecionadorId },
                error: function (resp, message, validationMessage) {
                    alert(resp.message);
                },
                success: function (resp, message, validationMessage) {
                    if (!$scope.pub) {
                        alert("Ocorreu um erro! Informe imediatamente ao Setor de TI.");
                    } else {
                        $scope.atribuirDadosDaMateria();
                    }
                }
            });
        }
    }

    // inicializando modelos vazios...
    $scope.inicializaModelosVazios = function () {
        if ($scope.pub.TMP_REMISSAO.length == 0) {
            $scope.inicializaRemissao();
        }
        if ($scope.pub.TMP_REMISSIVO.length == 0) {
            $scope.inicializaRemissivo();
        }
        if ($scope.pub.TMP_TITULACAO.length == 0) {
            $scope.inicializaTitulacao();
        }
        if ($scope.pub.TMP_PALAVRA_CHAVE.length == 0) {
            $scope.inicializaPalavraChave();
        }
        if ($scope.pub.PUB_UF.length == 0) {
            $scope.inicializaUf();
        }
        if ($scope.pub.PUBLICACAO_CONFIG.length == 0) {
            $scope.inicializaConfig();
        }
        if ($scope.pub.TMP_REVOGACAO.length < 3) {
            $scope.iniciazaRevogacaoAlteracao();
        }
    }

    // processar gravação?
    $scope.devoSalvarMateria = function (lSalvamentoAutomatico, operacao, revisao, msgPublicar) {
        var processar = false;
        if ($scope.salvando == false) { // se não está salvando...
            processar = (($scope.operacao && $scope.operacao.substring(0, 1) != 'V') && $scope.pub.ARE_CONS_ID && $scope.pub.PUBLICACAO.TIP_MAT_ID && $scope.pub.PUBLICACAO.PUB_ATIVO && ($scope.pub.PUB_UF[0].INF_ANO != "")) == true;

            if (processar && ($scope.lVisual || $scope.cargoSigla == 'DIA') || (lSalvamentoAutomatico && revisao != '0'))
                processar = false;

            if (processar) {
                if (lSalvamentoAutomatico) {
                    if (($scope.cargoSigla == 'DGT' || $scope.cargoSigla == 'RVO')) {
                        processar = false;
                    }
                }
            }
        }
        return processar;
    }

    // checando datas
    $scope.checarDatas = function () {
        var processar = true;
        if ($scope.pub.PUBLICACAO.PUB_DATA_PUB_ATO && $scope.pub.PUBLICACAO.PUB_DATA_ATO) {
            if ($scope.pub.PUBLICACAO.PUB_DATA_PUB_ATO < $scope.pub.PUBLICACAO.PUB_DATA_ATO) {
                alert('A T E N Ç Ã O! A data do ato não pode ser maior que a data de sua publicação!');
                processar = false;
            }
            if ($scope.alteracao && $scope.alteracao.PUB_DATA_ATO && ($scope.pub.PUBLICACAO.PUB_DATA_PUB_ATO < $scope.alteracao.PUB_DATA_ATO)) {
                alert('A T E N Ç Ã O! A data da publicação deste ato não pode ser menor que a data do ato sendo alterado!');
                processar = false;
            }
            if ($scope.revogacao && $scope.revogacao.PUB_DATA_ATO && ($scope.pub.PUBLICACAO.PUB_DATA_PUB_ATO < $scope.revogacao.PUB_DATA_ATO)) {
                alert('A T E N Ç Ã O! A data da publicação deste ato não pode ser menor que a data do ato sendo revogado!');
                processar = false;
            }
            if ($scope.revigoracao && $scope.revigoracao.PUB_DATA_ATO && ($scope.pub.PUBLICACAO.PUB_DATA_PUB_ATO < $scope.revigoracao.PUB_DATA_ATO)) {
                alert('A T E N Ç Ã O! A data da publicação deste ato não pode ser menor que a data do ato sendo revigorado!');
                processar = false;
            }

            if ($scope.alteracao && $scope.alteracao.PUB_DATA_ATO && ($scope.pub.PUBLICACAO.PUB_DATA_ATO < $scope.alteracao.PUB_DATA_ATO)) {
                alert('A T E N Ç Ã O! A data deste ato não pode ser menor que a data do ato sendo alterado!');
                processar = false;
            }
            if ($scope.revogacao && $scope.revogacao.PUB_DATA_ATO && ($scope.pub.PUBLICACAO.PUB_DATA_ATO < $scope.revogacao.PUB_DATA_ATO)) {
                alert('A T E N Ç Ã O! A data deste ato não pode ser menor que a data do ato sendo revogado!');
                processar = false;
            }
            if ($scope.revigoracao && $scope.revigoracao.PUB_DATA_ATO && ($scope.pub.PUBLICACAO.PUB_DATA_ATO < $scope.revigoracao.PUB_DATA_ATO)) {
                alert('A T E N Ç Ã O! A data deste ato não pode ser menor que a data do ato sendo revigorado!');
                processar = false;
            }
        }
        return processar;
    }

    //
    $scope.escolherColecionador = function () {
        var processar = true;
        if ($scope.pub.PUBLICACAO) {
            if (!$scope.pub.PUB_ID) {
                if (!confirm("O colecionador escolhido não poderá ser alterado após esta confirmação. " +
                    "Confirma a gravação desta matéria no colecionador [" + $scope.LeiaDropDown("colecionadorNome") + "]?")) {
                    processar = false;
                }
            }
        }
        return processar;
    }

    //
    $scope.acionarGravacao = function (lSalvamentoAutomatico, operacao, revisao, msgPublicar) {
        processar = $scope.escolherColecionador();
        if (processar) {
            processar = $scope.checarDatas();
            if (processar)
                $scope.salvarPublicacaoAreaConsultoria(lSalvamentoAutomatico, operacao, revisao, msgPublicar);
        }
        return processar;
    }

    // salvando os dados...
    $scope.salvarPublicacaoAreaConsultoria = function (lSalvamentoAutomatico, operacao, revisao, msgPublicar) {
        // impede gravação neste momento
        $scope.desabilitaGravacao();
        $scope.tempoSalvar = 30;
        $scope.salvando = true;
        // atribuindo para o objeto publicar ao salvar...
        lPublicar = (msgPublicar != null);
        $scope.pub.lPublicar = lPublicar;
        // veio da tela de revisão?...
        $scope.pub.revisao = revisao ? revisao : "0";
        // preparando para salvar...
        $scope.MudouColecionador();
        // monta a cabeça da matéria para salvar...
        if ($scope.pub.operacao == "I") {
            $scope.pub.PUBLICACAO.CABECALHO = $scope.materiaImpressa();
        }
        //
        $scope.pub.PUBLICACAO_REVISAO = null;
        $scope.pub.PUBLICACAO_REVISAO_COLABORADOR = null;

        // salvando...
        formHandlerService.submit($scope, {
            url: Util.getUrl("/publicacaoAreaConsultoria/salvar"),
            objectName: 'pub',
            showAjaxLoader: true,
            success: function (resp, status, config, message, validationMessage) {
                $scope.message = message;
                $scope.erros = validationMessage;
                if (resp.success) {
                    $scope.pub.PUBLICACAO.PUB_ID = resp.result.PUB_ID;
                    $scope.pub.PUB_ID = resp.result.PUB_ID;
                    $scope.pub.PUB_ID_PORTAL = resp.result.PUB_ID_PORTAL;
                    $scope.pub.operacao = "A";
                    $scope.pub.lIncluir = false;
                    if ($scope.publicou && $scope.pub.PUB_ID_PORTAL && $scope.colecionadorNome) {
                        if ($scope.ambienteProducao) {
                            if ($scope.colecionadorNome.substring(0, 3) == "ATC") {
                                $scope.pub.linkMateria = "http://www.coad.com.br/busca/detalhe_31/" + $scope.pub.PUB_ID_PORTAL.toString();
                            } else {
                                $scope.pub.linkMateria = "http://www.coad.com.br/busca/detalhe/" + $scope.pub.PUB_ID_PORTAL.toString() + "/30";
                            }
                        } else {
                            if ($scope.colecionadorNome.substring(0, 3) == "ATC") {
                                $scope.pub.linkMateria = "http://portalcoadlinux.apc.intranet/busca/detalhe_31/" + $scope.pub.PUB_ID_PORTAL.toString();
                            } else {
                                $scope.pub.linkMateria = "http://portalcoadlinux.apc.intranet/busca/detalhe/" + $scope.pub.PUB_ID_PORTAL.toString() + "/30";
                            }
                        }
                    }
                    if (!lSalvamentoAutomatico) {
                        if (!lPublicar) { // quando vir do botão publicar, não mostrar mensagem nem sair da tela...
                            alert("A Matéria Nº [ " + resp.result.PUB_ID.toString() + " ] foi salva com sucesso! Favor anotar este número caso precise dele mais tarde.");

                            $scope.sairDaMateria(true, revisao);
                        } else {
                            alert(msgPublicar);
                        }
                    } else {
                        $scope.pub.salvouAuto = true;
                    }
                    // habilitando os botões que acionam a gravação \\
                    $scope.habilitaGravacao();
                }
                $scope.reativarAutoSalvar();
            },
            error: function (resp, message, validationMessage) {
                $scope.reativarAutoSalvar();
            }
        });
    }

    // segundos para salvar matéria
    if (typeof $scope.salvando == "undefined")
        $scope.salvando = false;
    if (typeof $scope.msgSalvando == "undefined")
        $scope.msgSalvando = "Clique e salve agora";
    if (typeof $scope.tempoSalvar == "undefined")
        $scope.tempoSalvar = 30;

    $interval(function () {
        if (!$scope.salvando) {
            var processar = $scope.devoSalvarMateria(true, $scope.pub.operacao, $scope.pub.revisao, null);
            if (processar) {
                if ($scope.tempoSalvar > 0) {
                    if ($scope.carregouTudo)
                        $scope.tempoSalvar--;
                } else {
                    if (!$scope.acionarGravacao(true, $scope.pub.operacao, $scope.pub.revisao, null))
                        $scope.tempoSalvar = 30;
                }
            }
        }
    }, 1000);

    $scope.$watch('tempoSalvar', function () {
        $scope.salvarEm = $scope.tempoSalvar;
    });

    // reativar o auto-salvar
    $scope.reativarAutoSalvar = function () {
        $scope.tempoSalvar = 30;
        $scope.salvando = false;
    }

    // lendo o valor de um dropdownlist...
    $scope.LeiaDropDown = function (campo, informacao) {
        var dropdown = document.getElementById(campo);
        if (dropdown && dropdown.options[dropdown.selectedIndex].text !== "" && dropdown.options[dropdown.selectedIndex].text !== "Selecione" && dropdown.options[dropdown.selectedIndex].text !== "Todos") {
            return (informacao == "valor") || (informacao == "undefined") ? dropdown.options[dropdown.selectedIndex].value : dropdown.options[dropdown.selectedIndex].text;
        }
    }

    // liberando matéria para revisão técnica...
    $scope.liberarRevisaoTecnica = function (publicacaoId, colecionadorId) {
        if (confirm("Confirma a liberação da Matéria Nº [" + publicacaoId.toString() + "] para a Revisão Técnica?")) {
            formHandlerService.read($scope, {
                url: Util.getUrl("/publicacaoAreaConsultoria/liberarRevisaoTecnica"),
                targetObjectName: 'coadgedBIretorno',
                responseModelName: 'coadgedBIretorno',
                data: { publicacaoId: publicacaoId, colecionadorId: colecionadorId },
                success: function (retorno) {
                    alert("Publicação liberada com sucesso!");
                }
            })
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////////////////////////
}).directive('toggle', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            if (attrs.toggle == "tooltip") {
                $(element).tooltip();
            }
            if (attrs.toggle == "popover") {
                $(element).popover();
            }
        }
    };
}).directive("progressBar", ["$timeout", function ($timeout) {
    return {
        restrict: "EA",
        scope: {
            total: '=total',
            complete: '=complete',
            barClass: '@barClass',
            completedClass: '=?'
        },
        transclude: true,
        link: function (scope, elem, attrs) {

            scope.label = attrs.label;
            scope.completeLabel = attrs.completeLabel;
            scope.showPercent = (attrs.showPercent) || false;
            scope.completedClass = (scope.completedClass) || 'progress-bar-danger';

            scope.$watch('complete', function () {

                var progress = scope.complete / scope.total;

                if (progress >= 1) {
                    $(elem).find('.progress-bar').addClass(scope.completedClass);
                }
                else if (progress < 1) {
                    $(elem).find('.progress-bar').removeClass(scope.completedClass);
                }

            });

        },
        template:
        "<span class='small' data-ng-show='!$parent.salvando'>" +
        "   {{label}} <span class='label label-default'> {{$parent.tempoSalvar}} </span>" +
        "</span>" +

        "<div class='progress' data-ng-show='!$parent.salvando'>" +
        "   <div class='progress-bar {{barClass}}' title='{{complete/total * 100 | number:0 }}%' style='width:{{complete/total * 100}}%;'>" +
        "      {{showPercent ? (complete/total*100) : complete | number:0}} {{completeLabel}}" +
        "   </div>" +
        "</div>"
    };
}]);


/////////////////////////////////////////////////

var alterouPublicarPortal = false;
var alterouMateriaImpressa = false;

function objEditor(nomeDoEditor) {
    return document.getElementById(nomeDoEditor);
}

function RichTextEditor_OnTextChanged(editor) {
    if (angular.element(document.getElementById('idMateria')).scope().editorAtivo == editor._config.containerid) {
        if (angular.element(document.getElementById('idMateria')).scope().editorAtivoOnEnter == true) {
            angular.element(document.getElementById('idMateria')).scope().editorAtivoOnEnter = false;
        } else {
            if (editor._config.containerid in ('Integra', 'EmentaPt', 'ManchetePt')) {
                alterouPublicarPortal = true;
            } else if (editor._config.containerid in ('Resenha', 'Ementa', 'Manchete')) {
                alterouMateriaImpressa = true;
            }
        }
    }

    // Atualizando o texto do Digitador...
    if (angular.element(document.getElementById('idMateria')).scope().cargoSigla == 'DGT') {
        if (angular.element(document.getElementById('idMateria')).scope().editorAtivo == editor._config.containerid)
            angular.element(document.getElementById('idMateria')).scope().pub.PUBLICACAO.PUB_CONTEUDO_RESENHA_DGT = angular.element(document.getElementById('idMateria')).scope().pub.MATERIA_IMPRESSA;
    }
}

function RichTextEditor_OnBlur(editor) {
    if (editor._config.containerid == 'Integra') {
        if (angular.element(document.getElementById('idMateria')).scope().editorAtivo == editor._config.containerid) {
            var txt = angular.element(document.getElementById('idMateria')).scope().pub.PUBLICACAO.PUB_CONTEUDO;
            if (txt != editor._geh_htmlcode.trim()) {
                angular.element(document.getElementById('idMateria')).scope().pub.PUBLICACAO.PUB_CONTEUDO = editor._geh_htmlcode.trim();
                angular.element(document.getElementById('idMateria')).scope().atualizarPublicarPortal();
            }
            if (angular.element(document.getElementById('idMateria')).scope().pub.PUBLICACAO.PUB_CONTEUDO_RESENHA.length == 0) {
                angular.element(document.getElementById('idMateria')).scope().pub.PUBLICACAO.PUB_CONTEUDO_RESENHA = editor._geh_htmlcode.trim();
                angular.element(document.getElementById('idMateria')).scope().atualizarMateriaImpressa();
            }
        }
    }
    if (editor._config.containerid == 'Resenha') {
        if (angular.element(document.getElementById('idMateria')).scope().editorAtivo == editor._config.containerid) {
            var txt = angular.element(document.getElementById('idMateria')).scope().pub.PUBLICACAO.PUB_CONTEUDO_RESENHA;
            if (txt != editor._geh_htmlcode.trim()) {
                angular.element(document.getElementById('idMateria')).scope().pub.PUBLICACAO.PUB_CONTEUDO_RESENHA = editor._geh_htmlcode.trim();
                angular.element(document.getElementById('idMateria')).scope().atualizarMateriaImpressa();
            }
        }
    }
    if (editor._config.containerid == 'EmentaPt') {
        if (angular.element(document.getElementById('idMateria')).scope().editorAtivo == editor._config.containerid) {
            var txt = angular.element(document.getElementById('idMateria')).scope().pub.PUB_EMENTA_PORTAL;
            if (txt != editor._geh_htmlcode.trim()) {
                angular.element(document.getElementById('idMateria')).scope().pub.PUB_EMENTA_PORTAL = editor._geh_htmlcode.trim();
                angular.element(document.getElementById('idMateria')).scope().atualizarPublicarPortal();
            }
        }
    }
    if (editor._config.containerid == 'Ementa') {
        if (angular.element(document.getElementById('idMateria')).scope().editorAtivo == editor._config.containerid) {
            var txt = angular.element(document.getElementById('idMateria')).scope().pub.PUB_EMENTA;
            if (txt != editor._geh_htmlcode.trim()) {
                angular.element(document.getElementById('idMateria')).scope().pub.PUB_EMENTA = editor._geh_htmlcode.trim();
                angular.element(document.getElementById('idMateria')).scope().atualizarMateriaImpressa();
            }
        }
    }
    if (editor._config.containerid == 'ManchetePt') {
        if (angular.element(document.getElementById('idMateria')).scope().editorAtivo == editor._config.containerid) {
            var txt = angular.element(document.getElementById('idMateria')).scope().pub.PUB_MANCHETE_PORTAL;
            if (txt != editor._geh_htmlcode.trim()) {
                angular.element(document.getElementById('idMateria')).scope().pub.PUB_MANCHETE_PORTAL = editor._geh_htmlcode.trim();
                angular.element(document.getElementById('idMateria')).scope().atualizarPublicarPortal();
            }
        }
    }
    if (editor._config.containerid == 'Manchete') {
        if (angular.element(document.getElementById('idMateria')).scope().editorAtivo == editor._config.containerid) {
            var txt = angular.element(document.getElementById('idMateria')).scope().pub.PUB_MANCHETE;
            if (txt != editor._geh_htmlcode.trim()) {
                angular.element(document.getElementById('idMateria')).scope().pub.PUB_MANCHETE = editor._geh_htmlcode.trim();
                angular.element(document.getElementById('idMateria')).scope().atualizarMateriaImpressa();
            }
        }
    }
    if (editor._config.containerid == 'Publicar') {
        if (angular.element(document.getElementById('idMateria')).scope().editorAtivo == editor._config.containerid)
            angular.element(document.getElementById('idMateria')).scope().pub.PUBLICAR_PORTAL = editor._geh_htmlcode.trim();
    }
    if (editor._config.containerid == 'Remissao') {
        if (angular.element(document.getElementById('idMateria')).scope().editorAtivo == editor._config.containerid)
            angular.element(document.getElementById('idMateria')).scope().pub.MATERIA_IMPRESSA = editor._geh_htmlcode.trim();
    }
    if (editor._config.containerid == 'Digitador') {
        if (angular.element(document.getElementById('idMateria')).scope().editorAtivo == editor._config.containerid)
            angular.element(document.getElementById('idMateria')).scope().pub.PUBLICACAO.PUB_CONTEUDO_RESENHA_DGT = editor._geh_htmlcode.trim();
    }
    if (editor._config.containerid == 'EditorRapido') {
        if (angular.element(document.getElementById('idMateria')).scope().editorAtivo == editor._config.containerid)
            angular.element(document.getElementById('idMateria')).scope().pub.PUBLICACAO.PUB_CONTEUDO_RESENHA_RVO = editor._geh_htmlcode.trim();
    }
}
