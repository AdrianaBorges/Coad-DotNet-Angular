appModule.controller('SqlDinamicoController', function ($scope, formHandlerService, $http, conversionService, $interval, $timeout, $compile) {
    
    $scope.init = function (relId) {

        $scope.tab = 1;
        $scope.open = false;
        $scope.listarTabelas();
        $scope.listarTipoJoins();
        $scope.listarOperadorCondicional();
        $scope.listarOperadoresLogicos();

        if (relId && relId != null) {

            $scope.recuperarDadosDoRelatorio(relId);
        }
        else {
            $scope.relPersonalizado = {

                IgnoreValidation : true,
                RELATORIO_TABELA_COLUNAS: [],
                RELATORIO_TABELAS: [],
                RELATORIO_JOIN: [],
                RELATORIO_CONDICAO : [],
            };
        }

        $interval(function () {
            var c = angular.element('connection');
                c.connections('update');
            
        }, 500);
    }

    $scope.abrirTab = function (numTab)
    {
        $scope.tab = numTab;
    }

    //$scope.abrirTabTabelas = function () {

    //}

    $scope.abrirPainelUen = function () {

        $scope.open = !$scope.open;
    }

    $scope.iniciarMarcacaoJoin = function (tipo, tabela, nomeCampo, index) {

        if (tipo && tabela) {

            var nomeTabela = tabela.RET_NOME_TABELA;
            $scope.joinMarkDTO = {
                Passo: 0,
                TPJ_ID: tipo,
                RELATORIO_TABELAS: tabela,
                REJ_NOME_CAMPO1: nomeCampo,
                Seletor1: 'campo_' + nomeTabela + '_X_' + nomeCampo,
            };

            $scope.message = Util.createMessage('info', 'Escolha uma tabela para associar.');

            $timeout(function () {
                $scope.message = null;
            }, 900);
        }
        else 
        {
            $scope.message = Util.createMessage('fail', 'Alguma coisa deu errado. Algumas informações importantes não foram encontradas.');
        }
    }

    $scope.criarJoin = function (tabela, nomeCampo, index) {

        if (tabela) {

            var nomeTabela = tabela.RET_NOME_TABELA;

            var seletor = 'campo_' + nomeTabela + '_X_' + nomeCampo;
            $scope.joinMarkDTO.Passo = 1;
            $scope.joinMarkDTO.RELATORIO_TABELAS1 = tabela;
            $scope.joinMarkDTO.REJ_NOME_CAMPO2 = nomeCampo;
            $scope.joinMarkDTO.Seletor2 = seletor;
            $scope.joinMarkDTO.Id = $scope.joinMarkDTO.Seletor2 + 'x' + $scope.joinMarkDTO.Seletor2;

            angular.element("#" + $scope.joinMarkDTO.Seletor1).connections({

                to: "#" + seletor,
                'class': $scope.joinMarkDTO.Id + ", line-jquery-connections",
                //within: '#tabela-container'
            });

            $scope.criarAcaoLinha($scope.joinMarkDTO);
            $scope.relPersonalizado.RELATORIO_JOIN.push($scope.joinMarkDTO);

            delete $scope.joinMarkDTO;
        }
        else
        {
            $scope.message = Util.createMessage('fail', 'Alguma coisa deu errado. Algumas informações importantes não foram encontradas.');
        }
    }

    $scope.criarAcaoLinha = function (joinMarkDTO) {

        angular.element($scope.joinMarkDTO.Id).on('click', function () {
            var joinMark = angular.copy(joinMarkDTO);

            if (!$scope.$phase) {

                $scope.$apply(function () {

                    value = '33';
                    $scope.message = Util.createMessage("info", "Elemento[" + joinMark.Id + "]");
                });
            }
        });
    }


    $scope.listarTabelas = function () {

        $scope.listado = false;
        var url = Util.getUrl('/sqlDinamico/listarTabelasDoSistema');

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstTabelas',
            responseModelName: 'lstTabelas',
            showAjaxLoader: true,
           // pageConfig: { pageName: 'page' },
            success: function () {
                $scope.listado = true;
            }
        });
    }

    $scope.listarEDescreverColunas = function (nomeTabela) {

        $scope.nomeTabela = nomeTabela;
        $scope.descreverColunas();
    }

    $scope.descreverColunas = function (tabela, nomeTabela, selector) {

        var url = Util.getUrl('/sqlDinamico/descreverColunasDaTabela');

        formHandlerService.read(tabela, {
            url: url,
            targetObjectName: 'Colunas',
            responseModelName: 'lstColunas',
            showAjaxLoader: true,
            data: {nomeTabela: nomeTabela},
            //pageConfig: { pageName: 'page', pageTargetName: 'paginaPrioridades'},
            success: function () {

                $scope.listado = true;

                //Existe dois tipos de animação presentes.
                //1º Do próprio angular .animate-repeat.
                //2° Do boostrap 'collapse'
                //Para não haver comflito. o padrão é deixar a classe colapse que mantém o elemento invisível
                // quando é detectado que o elemente será preenchido então jogo a classe in para que o collpse
                // torne o elemente visível. Assim o angular realiza a animação inicial. Depois as animaçoes de mostrar
                // e ocultar fica a cardo do boostrap

                angular.element(selector).addClass('in');
            }
        });
    }

    $scope.preencherColunas = function (tabela, callback) {

        if (tabela && (!tabela.Colunas || tabela.Colunas.length <= 0)) {
            var url = Util.getUrl('/sqlDinamico/descreverColunasDaTabela');

            formHandlerService.read(tabela, {
                url: url,
                targetObjectName: 'Colunas',
                responseModelName: 'lstColunas',
                showAjaxLoader: true,
                data: { nomeTabela: tabela.RET_NOME_TABELA },
                //pageConfig: { pageName: 'page', pageTargetName: 'paginaPrioridades'},
                success: function () {

                    if (callback && typeof callback == 'function') {
                        callback();
                    }
                }
            });
        }
    }

    $scope.trocarUen = function () {

        var url = "/franquia/uen/TrocarUen";

        formHandlerService.submit($scope, {
            url: url,
            objectName: 'uen',
            success: function (resp, status, config, message, validationMessage) {
                $scope.message = message;
                $scope.erros = validationMessage;

                if (resp.success) {

                    $scope.button = 'reset';
                    $scope.message = Util.createMessage("success", "Salvo com sucesso!");
                    $scope.uenAtual = angular.copy(resp.result.uen);

                    $timeout(function () {

                        $scope.message = null;
                        $scope.open = false;
                        location.reload();
                        
                    }, 1000);
                }
                else {
                    $scope.button = 'reset';
                    $scope.open = false;
                }

            }
        });
    }

    $scope.collapse = function (selector) {

        angular.element(selector).collapse('toggle');
    }

    $scope.dispararAcaoMenuClicadoTabelas = function ($event, item, nomeTabela, selector) {

        $event.stopPropagation();

        if (item.aberto == undefined) {
            item.aberto = true;
        }
        else {
            item.aberto = !item.aberto;
        }

        if (item && (!item.Colunas || item.Colunas.length < 1)) {
            $scope.descreverColunas(item, nomeTabela, selector);
        }
        else {
            $scope.collapse(selector);
        }

    }


    $scope.salvarRelatorioPersonalizado = function () {

        var relId = $scope.relPersonalizado.REL_ID;

            formHandlerService.submit($scope, {
                url: Util.getUrl("/sqlDinamico/SalvarRelatorioPersonalizado"),
                objectName: 'relPersonalizado',
                showAjaxLoader: true,
                success: function (resp, status, config, message, validationMessage) {

                    $scope.message = message;
                    $scope.erros = validationMessage;

                    $scope.button = 'reset';
                    if (resp.success) {

                        $scope.message = Util.createMessage("success", "Relatório personalizado salvo com sucesso!");

                        $timeout(function () {
                            
                            $scope.message = null;
                            if (relId) {

                                if (confirm("Voltar para a listagem de relatórios?")) {

                                    window.open(Util.getUrl('/sqlDinamico/relatorioBase'), '_self');
                                }
                                else{
                                    $scope.recuperarDadosDoRelatorio(relId);
                                }
                            }
                            else {
                                window.open(Util.getUrl('/sqlDinamico/relatorioBase'), '_self');
                            }

                        }, 1000);

                    }
                }
            });
    }


    $scope.excluirRelatorioPersonalizado = function (base) {

        if (confirm("Deseja realmente excluir?")) {

            
            formHandlerService.submit($scope, {
                url: Util.getUrl("/sqlDinamico/ExcluirRelatorioPersonalizado"),
                objectName: 'relPersonalizado',
                showAjaxLoader: true,
                success: function (resp, status, config, message, validationMessage) {

                    $scope.message = message;
                    $scope.erros = validationMessage;

                    $scope.buttonDel = 'reset';
                    if (resp.success) {

                        $scope.message = Util.createMessage("success", "Relatório excluído com com sucesso!");

                        $timeout(function () {

                            $scope.message = null;

                            if (base == true) {
                                window.open(Util.getUrl('/sqlDinamico/relatorioBase'), '_self');
                            }
                            else {
                                window.open(Util.getUrl('/sqlDinamico/index'), '_self');
                            }

                        }, 1000);

                    }
                }
            });
        }
        else {

            return false;
        }
    }


    $scope.salvarRelatorioPersonalizadoDerivado = function () {

        var relId = $scope.relPersonalizado.REL_ID;

        formHandlerService.submit($scope, {
            url: Util.getUrl("/sqlDinamico/SalvarRelatorioPersonalizadoDerivado"),
            objectName: 'relPersonalizado',
            showAjaxLoader: true,
            success: function (resp, status, config, message, validationMessage) {

                $scope.message = message;
                $scope.erros = validationMessage;

                $scope.button = 'reset';
                if (resp.success) {

                    $scope.message = Util.createMessage("success", "Relatório salvo com sucesso!");

                    $timeout(function () {
                        //window.open(Util.getUrl('/pedido/index'), '_self');
                        $scope.message = null;
                        if (relId) {
                            if (confirm("Voltar para a listagem de relatórios?")) {

                                window.open(Util.getUrl('/sqlDinamico/index'), '_self');
                            }
                            else {
                                $scope.recuperarDadosDoRelatorio(relId);
                            }
                        }
                        else {
                            window.open(Util.getUrl('/sqlDinamico/index'), '_self');
                        }

                    }, 1000);

                }
            }
        });
    }

    $scope.converterTabela = function (objTabela) {

        var obj = {
            RET_NOME_TABELA : objTabela.nome,
            RET_SCHEMA: objTabela.schema
        };

        return obj;
    }

    $scope.converterColuna = function (colunaInfo)
    {
        var obj = {
                
            COR_DESCRICAO: colunaInfo.COLUMN_NAME,

        };
        return obj;
    }

    $scope.checarEAdicionarTabelas = function (relatorioTabela) {

        var achou = false;
        angular.forEach($scope.relPersonalizado.RELATORIO_TABELAS, function (value, index) {

            if (!achou) {

                if (value.RET_NOME_TABELA == relatorioTabela.RET_NOME_TABELA) {
                    achou = true;
                    return;
                }
            }            
        });

        if (!achou) {
            $scope.relPersonalizado.RELATORIO_TABELAS.push(relatorioTabela);
        }
    }

    $scope.adicionarAgrupamento = function (idDrag, idDrop, value) {

        if (idDrag == 'drag_consulta_coluna') {

            var length = $scope.relPersonalizado.Agrupamento.length;
            value.COR_AGRUPAMENTO_ORDEM = (length - 1);

            $scope.relPersonalizado.Agrupamento.push(value);
        }
    }

    $scope.adicionarItemConsulta = function (idDrag, idDrop, value) {

        if ($scope.relPersonalizado) {
            //var value = $scope.objUfMunicipio;
            if (idDrag == 'drag_consulta_coluna') {

                var coluna = angular.copy(value);
                $scope.relPersonalizado.RELATORIO_TABELA_COLUNAS.push(coluna);

                $scope.preencherColunas(coluna.RELATORIO_TABELAS, function () {
                    
                    $scope.checarEAdicionarTabelas(coluna.RELATORIO_TABELAS);
                });                
            }
            else if (idDrag == 'drag_consulta_tabela') {

                if (value) {

                    $scope.preencherColunas(value, function () {

                        $scope.relPersonalizado.RELATORIO_TABELAS.push(value);
                    });                    
                }
            }
        }
    }

    $scope.preencherColunasDasTabelasRelatorio = function () {
        
        if (Util.isPathValid($scope, 'relPersonalizado.RELATORIO_TABELAS')) {

            angular.forEach($scope.relPersonalizado.RELATORIO_TABELAS, function (value, index) {

                $scope.preencherColunas(value, function () {  

                });
            });
        }
    }

    $scope.recuperarDadosDoRelatorio = function (relId) {

        if (relId) {
            formHandlerService.read($scope, {
                url: Util.getUrl("/sqlDinamico/RecuperarDadosDoRelatorioPersonalizado"),
                targetObjectName: 'relPersonalizado',
                responseModelName: 'relPersonalizado',
                showAjaxLoader: true,
                data: { relId: relId },
                success: function () {
                    $timeout(function () {
                        $scope.criarMarcacaoJoin();
                        if (Util.isPathValid($scope, 'relPersonalizado.REL_ID_PAI')) {

                            $scope.listarColunasDoRelatorioFormatado($scope.relPersonalizado.REL_ID_PAI);
                        }
                    })
                }
            });
        }
    }

    $scope.criarMarcacaoJoin = function () {

        if (Util.isPathValid($scope, 'relPersonalizado.RELATORIO_JOIN')) {

            angular.forEach($scope.relPersonalizado.RELATORIO_JOIN, function (value, index) {

                var tab1 = value.RELATORIO_TABELAS.RET_NOME_TABELA;
                var tab2 = value.RELATORIO_TABELAS1.RET_NOME_TABELA;

                var coluna1 = value.REJ_NOME_CAMPO1;
                var coluna2 = value.REJ_NOME_CAMPO2;

                var seletor1 = "campo_" + tab1 + "_X_" + coluna1;
                var seletor2 = "campo_" + tab2 + "_X_" + coluna2;

                angular.element("#" + seletor1).connections({

                    to: "#" + seletor2,
                    'class': seletor1 + "x" + seletor2 + ", line-jquery-connections"
                });
            });
        }
    }

    $scope.abrirModalPreviewQuery = function () {

        $scope.mostrarPreviewSQL($scope.relPersonalizado.REL_ID);
        angular.element("#modal-preview-query").modal();
    }

    $scope.abrirModalPreviewQueryPorId = function (relId) {

        $scope.relPersonalizado = {REL_ID : relId};
        $scope.mostrarPreviewSQL(relId);
        angular.element("#modal-preview-query").modal();
    }

    $scope.mostrarPreviewSQL = function (relId) {

        if (relId) {
            formHandlerService.read($scope, {
                url: Util.getUrl("/sqlDinamico/mostrarPreviewQuery"),
                targetObjectName: 'montagemQuery',
                responseModelName: 'montagemQuery',
                showAjaxLoader: true,
                data: { relId: relId },
                success: function () {

                    angular.element("#preview-query").addClass('in');
                    $scope.resultado = null;
                }
            });
        }
    }

    $scope.listarOperadoresLogicos = function () {

            formHandlerService.read($scope, {
                url: Util.getUrl("/sqlDinamico/ListarOperadorLogico"),
                targetObjectName: 'lstOperadoresLogicos',
                responseModelName: 'lstOperadoresLogicos',
                success: function () {
                }
            });
        
    }


    $scope.listarOperadorCondicional = function () {

        formHandlerService.read($scope, {
            url: Util.getUrl("/sqlDinamico/ListarOperadorCondicional"),
            targetObjectName: 'lstOperadoresCondicionais',
            responseModelName: 'lstOperadoresCondicionais',
            success: function () {
            }
        });        
    }


    $scope.adicionarCondicaoConsulta = function (idDrag, idDrop, value) {

        if ($scope.relPersonalizado) {
            //var value = $scope.objUfMunicipio;
            if ((idDrag == 'drag_consulta_coluna' || idDrag == 'drag_colunas') && idDrop == 'drop-condicao') {

                var coluna = null;

                if (idDrag == 'drag_colunas') {
                    coluna = angular.copy(value.item);
                }
                else {
                    coluna = angular.copy(value);
                }
                
                var objCondicao = {
                    
                    RELATORIO_TABELAS: coluna.RELATORIO_TABELAS,
                    REC_CAMPO: coluna.COR_DESCRICAO,
                    REC_NOME_TIPO: coluna.COR_TYPE_NAME
                };

                $scope.relPersonalizado.RELATORIO_CONDICAO.push(objCondicao);

                $scope.preencherColunas(coluna.RELATORIO_TABELAS, function () {

                    $scope.checarEAdicionarTabelas(coluna.RELATORIO_TABELAS);
                });            
           }
        }
    }

    $scope.abrirModalOperadorCondicional = function (relatorioCondicao) {

        if(!Util.isPathValid(relatorioCondicao, 'RELATORIO_CONDICAO_RELATORIO_OPERADOR_CONDICIONAL')){

            relatorioCondicao.RELATORIO_CONDICAO_RELATORIO_OPERADOR_CONDICIONAL = [];
        }

        var lstCondicionais = angular.copy(relatorioCondicao.RELATORIO_CONDICAO_RELATORIO_OPERADOR_CONDICIONAL);

        $scope.modalOperadorCondicional = {
            lstCondicionais: lstCondicionais,
            relatoricoCondicao : relatorioCondicao
        };
        angular.element("#modal-operadores-condicionais").modal();
    }

    
    $scope.adicionarOperadorCondicional = function () {

        if(Util.isPathValid($scope, 'modalOperadorCondicional.OPERADOR_CONDICIONAL')){

            var operador = $scope.modalOperadorCondicional.OPERADOR_CONDICIONAL;
            $scope.modalOperadorCondicional.lstCondicionais.push({ RELATORIO_OPERADOR_CONDICIONAL: operador, ROC_ID: operador.ROC_ID });
        }
        else{

            $scope.message = Util.createMessage("fail", "Selecione um operador para adicionar");
        }
    }

    $scope.confirmarOperadoresCondicionais = function () {

        $scope.modalOperadorCondicional.relatoricoCondicao.RELATORIO_CONDICAO_RELATORIO_OPERADOR_CONDICIONAL =
            $scope.modalOperadorCondicional.lstCondicionais;

        delete $scope.modalOperadorCondicional;

        angular.element("#modal-operadores-condicionais").modal('hide');
    }

    $scope.removerOperadorCondicional = function (index) {

        if (index || index == 0)
        {
            $scope.modalOperadorCondicional.lstCondicionais.splice(index, 1);
        }
    }

    $scope.removerRelatorioCondicao = function($index) {

        $scope.relPersonalizado.RELATORIO_CONDICAO.splice($index, 1);
    }

    
    $scope.utilizarRelatorio = function (relId)
    {
        $scope.relPersonalizado = { REL_ID: relId };
        angular.element("#resultado-query").collapse('hide');
        $scope.obterMetaDadoDoRelatorio(relId);
        //$scope.rodarResultadoRelatorio(relId)
    }

    $scope.mostrarPreviewResultado = function () {

        $scope.listado = true;
        if ($scope.relPersonalizado.REL_ID) {
            formHandlerService.read($scope, {
                url: Util.getUrl("/sqlDinamico/mostrarPreviewResultado"),
                showAjaxLoader: true,
                targetObjectName: 'resultado',
                responseModelName: 'resultado',
                data: { relId: $scope.relPersonalizado.REL_ID },
                success: function () {
                    angular.element("#preview-query").removeClass('in');
                }
            });
        }
    }

    $scope.rodarResultadoRelatorio = function () {

        $scope.listado = true;

        if ($scope.relPersonalizado.REL_ID) {

            var params = {
                relId: $scope.relPersonalizado.REL_ID,
            };

            if ($scope.metadado) {

                params.lstFiltros = $scope.metadado.Filtros;
            }

            formHandlerService.read($scope, {
                url: Util.getUrl("/sqlDinamico/rodarResultadoRelatorio"),
                showAjaxLoader: true,
                targetObjectName: 'resultado',
                responseModelName: 'resultado',
                data: params,
                success: function () {
                    angular.element("#preview-query").removeClass('in');
                }
            });
        }
    }

    $scope.removerColunas = function (index) {

        $scope.relPersonalizado.RELATORIO_TABELA_COLUNAS.splice(index, 1);
    }

    $scope.removerTabelas = function (index) {

        var relTabela = $scope.relPersonalizado.RELATORIO_TABELAS[index];
        var indexJoin = null;
        var indexColuna = null;
        var indexCondicao = null;

        // removendo o join da tabela
        angular.forEach($scope.relPersonalizado.RELATORIO_JOIN, function (value, loopIndex) {

            var achou = false;
            if (value.RELATORIO_TABELAS.RET_NOME_TABELA == relTabela.RET_NOME_TABELA ||
                value.RELATORIO_TABELAS1.RET_NOME_TABELA == relTabela.RET_NOME_TABELA) {

                if (achou == false) {
                    indexJoin = loopIndex;
                    achou = true;
                }
            }
        });
        
        // removendo as colunas da tabela
        angular.forEach($scope.relPersonalizado.RELATORIO_TABELA_COLUNAS, function (value, loopIndex) {

            var achou = false;
            if (value.RELATORIO_TABELAS.RET_NOME_TABELA == relTabela.RET_NOME_TABELA) {

                if (achou == false) {
                    indexColuna = loopIndex;
                    achou = true;
                }
            }
        });

        // removendo as condições da tabela
        angular.forEach($scope.relPersonalizado.RELATORIO_CONDICAO, function (value, loopIndex) {

            var achou = false;
            if (value.RELATORIO_TABELAS.RET_NOME_TABELA == relTabela.RET_NOME_TABELA) {

                if (achou == false) {
                    indexCondicao = loopIndex;
                    achou = true;
                }
            }
        });

        $scope.relPersonalizado.RELATORIO_JOIN.splice(indexJoin, 1);
        $scope.relPersonalizado.RELATORIO_TABELA_COLUNAS.splice(indexColuna, 1);
        $scope.relPersonalizado.RELATORIO_CONDICAO.splice(indexCondicao, 1);
        $scope.relPersonalizado.RELATORIO_TABELAS.splice(index, 1);
    }

   
    $scope.evitarProgacao = function ($event) {

        $event.stopPropagation();
    }

    $scope.abrirPopOverAlias = function (selector, item, $event) {

        $event.stopPropagation();
        if ($scope.popoverSelector && $scope.popoverSelector != selector) {

            angular.element("#" + $scope.popoverSelector).popover('hide');
        }

        $scope.popoverSelector = selector;
        $scope.aliasColuna = item.COR_ALIAS;

        angular.element("#" + $scope.popoverSelector).popover('show');
    }

    
    $scope.fecharPopOvers = function ()
    {
        if ($scope.popoverSelector) {

            angular.element("#" + $scope.popoverSelector).popover('hide');
        }
    }

    $scope.confirmarEdicaoAliasColuna = function (item, valor) {

        item.COR_ALIAS = valor;
        $scope.fecharPopOvers();
    }


    $scope.limparColuna = function(col){

        return col.replace(/[-_]/g, " ");
    }

    $scope.listarRelatorioPersonalizadoBase = function (pagina) {

        $scope.listado = false;
        var url = Util.getUrl("/sqlDinamico/listarRelatorioPersonalizadoBase");

        if (pagina) {

            url += "?pagina=" + pagina;
        }

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstRelatorios',
            responseModelName: 'lstDadosRelatorio',
            data: $scope.filtro,
            showAjaxLoader: true,
            pageConfig: { pageName: 'page' /*, pageTargetName: 'paginaPrioridades' */ },
            success: function (resp) {
                $scope.listado = true;
            }
        });
    }

    $scope.listarRelatorioPersonalizado = function (pagina) {

        $scope.listado = false;
        var url = Util.getUrl("/sqlDinamico/listarRelatorioPersonalizado");

        if (pagina) {

            url += "?pagina=" + pagina;
        }

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstRelatorios',
            responseModelName: 'lstRelatorios',
            data: $scope.filtro,
            showAjaxLoader: true,
            pageConfig: { pageName: 'page' /*, pageTargetName: 'paginaPrioridades' */ },
            success: function (resp) {
                $scope.listado = true;
            }
        });
    }

    $scope.abrirModalRelatorioBase = function () {

        $scope.listarRelatorioPersonalizadoBase();
        angular.element("#modal-dados-relatorio").modal();
    }

    $scope.removerRelatorioBase = function () {

        $scope.relPersonalizado.RelatorioPai = null;
        $scope.relPersonalizado.REL_ID_PAI = null;
        $scope.relPersonalizado.RELATORIO_TABELA_COLUNAS = [];
        $scope.relPersonalizado.RELATORIO_CONDICAO = [];
        $scope.lstColunasFormatada = null;
    }

    $scope.adicionarRelatorioBase = function (item) {

        $scope.relPersonalizado.RelatorioPai = item;
        $scope.relPersonalizado.RelatorioPai.IgnoreValidation = true;
        $scope.listarColunasDoRelatorioFormatado(item.REL_ID);
        $scope.relPersonalizado.RELATORIO_TABELA_COLUNAS = [];
        $scope.relPersonalizado.RELATORIO_CONDICAO = [];
        

        angular.element("#modal-dados-relatorio").modal('hide');
    }

    $scope.listarColunasDoRelatorioFormatado = function (relId) {

        $scope.lstColunasFormatada = null;
        formHandlerService.read($scope, {
            url: Util.getUrl("/sqlDinamico/listarColunasDoRelatorioFormatado"),
            showAjaxLoader: true,
            targetObjectName: 'lstColunasFormatada',
            responseModelName: 'lstColunasFormatada',
            data: {relId : relId},
            success: function () {
            }
        });
    }

    $scope.obterMetaDadoDoRelatorio = function (relId) {

        $scope.lstColunasFormatada = null;
        formHandlerService.read($scope, {
            url: Util.getUrl("/sqlDinamico/obterMetaDadoDoRelatorio"),
            showAjaxLoader: true,
            targetObjectName: 'metadado',
            responseModelName: 'metadado',
            data: { relId: relId },
            success: function () {
            }
        });
    }

    $scope.rodarResultadoRelatorioPlanilha = function () {

        $scope.listado = true;

        if ($scope.relPersonalizado.REL_ID) {

            var params = {
                relId: $scope.relPersonalizado.REL_ID,
            };

            if ($scope.metadado) {

                params.lstFiltros = $scope.metadado.Filtros;
            }

            formHandlerService.read($scope, {
                url: Util.getUrl("/sqlDinamico/RodarResultadoRelatorioPlanilha"),
                showAjaxLoader: true,
                targetObjectName: 'fileName',
                responseModelName: 'fileName',
                data: params,
                success: function () {


                    if ($scope.fileName)
                        $scope.baixarPlanilha($scope.fileName);
                    angular.element("#preview-query").removeClass('in');
                }
            });
        }
    }

    $scope.baixarPlanilha = function (fileName) {

        if (fileName) {
            var url = Util.getUrl("/sqlDinamico/BaixarPlanilha");
            post(url + "?fileName=" + fileName, true);
        }
    }


    $scope.adicionarCondicaoConsulta = function (idDrag, idDrop, value) {

        if ($scope.relPersonalizado) {
            //var value = $scope.objUfMunicipio;
            if ((idDrag == 'drag_consulta_coluna' || idDrag == 'drag_colunas') && idDrop == 'drop-condicao') {

                var coluna = null;

                if (idDrag == 'drag_colunas') {
                    coluna = angular.copy(value.item);
                }
                else {
                    coluna = angular.copy(value);
                }

                var objCondicao = {

                    RELATORIO_TABELAS: coluna.RELATORIO_TABELAS,
                    REC_CAMPO: coluna.COR_DESCRICAO,
                    REC_NOME_TIPO: coluna.COR_TYPE_NAME
                };

                $scope.relPersonalizado.RELATORIO_CONDICAO.push(objCondicao);

                $scope.preencherColunas(coluna.RELATORIO_TABELAS, function () {

                    $scope.checarEAdicionarTabelas(coluna.RELATORIO_TABELAS);
                });
            }
        }
    }

    $scope.listarTipoJoins = function () {

        $scope.lstColunasFormatada = null;
        formHandlerService.read($scope, {
            url: Util.getUrl("/sqlDinamico/listarTipoJoin"),
            showAjaxLoader: true,
            targetObjectName: 'lstTipoJoin',
            responseModelName: 'lstTipoJoin',
            success: function () {
            }
        });
    }

    $scope.removerRelatorioJoin = function ($index) {
        

        $scope.relPersonalizado.RELATORIO_JOIN.splice($index, 1);
        angular.element("." + "line-jquery-connections").connections('remove');
        $scope.criarMarcacaoJoin();
    }

})