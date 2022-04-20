appModule.controller('ProdutoComposicaoController', function ($scope, formHandlerService, $http, conversionService, messageService, comparatorService, $timeout) {
    
    $scope.lstButton = [{ label: 'Incluir', show: true }, { label: 'Inclindo...', show: false }];
    $scope.lstButtonAlterar = [{ label: 'Alterar', show: true }, { label: 'Alterando...', show: false }];
    $scope.button = $scope.lstButton[0];

    $scope.tab = 1;

    $scope.query = { show: true };

    $scope.selecionarTab = function (value) {

        $scope.tab = value;
    };

    $scope.init = function () {

        $scope.getListaTipoPeriodo();
        $scope.listarTipoVenda();
        $scope.carregarEmpresas();
    };

    $scope.initList = function () {

        $scope.carregarEmpresas();
    };

    $scope.carregarEmpresas = function () {

        var url = Util.getUrl("/empresa/listarEmpresas");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstEmpresas',
            responseModelName: 'lstEmpresas',
            showAjaxLoader: true,

        });
    };


    $scope.salvarComposicao = function () {

        //$scope.buttonSave = $scope.lstButtonSave[1];
        formHandlerService.submit($scope, {
            url: Util.getUrl("/produtocomposicao/salvar"),
            objectName: 'produtocomposicao',
            deepConvertDate: true,
            showAjaxLoader: true,
            success: function (resp, status, config, message, validationMessage) {
                $scope.message = message;
                $scope.erros = validationMessage;
                $scope.buttonSave = 'reset';

                //$scope.buttonSave = $scope.lstButtonSave[0];
                if (resp.success) {

                    $scope.message = Util.createMessage("success", "Salvo com sucesso!");
                    $timeout(function () {

                        if (resp.result.produtoComposto) {

                            post(Util.getUrl("/produtocomposicao/editar?composicaoId=" + resp.result.produtoComposto.CMP_ID));
                        }
                        else {
                            $scope.message = null;
                            if ($scope.tab) {

                                $scope.tab = 2;
                            }
                        }

                    }, 1000);
                }
            },
            fail: function () {
                $scope.buttonSave = 'reset';
            }
        });
    }

    $scope.listar = function (pageRequest) {

        $scope.listado = false;

        var url = Util.getUrl("/produtocomposicao/produtoscomposicao");

        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }

        var config = {
            url: url,
            targetObjectName: 'produtosComposicao',
            responseModelName: 'produtosComposicao',
            showAjaxLoader: true,
            pageConfig: { pageName: 'page' },
            success: function () {

                $scope.listado = true;
            }
        };

        if ($scope.filtro) {

            config.data = angular.copy($scope.filtro);
        }
        formHandlerService.read($scope, config);
    };

    $scope.read = function (composicaoId) {

        $scope.getLstProdutos();
        $scope.getLstTipoPeriodo();
        if (composicaoId) {

            formHandlerService.read($scope, {
                url: Util.getUrl("/produtocomposicao/readcomposicao"),
                targetObjectName: 'produtocomposicao',
                responseModelName: 'produtocomposicao',
                data: { composicaoId: composicaoId },
                success: function () {

                    //$scope.produtocomposicao.PRODUTOS = null;

                    //if ($scope.produtocomposicao.PRODUTOS) {
                    //    $scope.select2Ctrl.stayClose = true;
                    //    $scope.produtocomposicao.PRODUTO_DESCRICAO = $scope.produtocomposicao.PRODUTOS.PRO_NOME;
                    //    $scope.produtocomposicao.PRODUTOS = null;
                    //}
                    // tratamentos para se adequar a tela                   
                    angular.forEach($scope.produtocomposicao.PRODUTO_COMPOSICAO_ITEM, function (value) {

                        value.PRODUTO_COMPOSICAO = null;
                        if (value.PRODUTOS) {
                            value.NOME_PRODUTO = value.PRODUTOS.PRO_NOME;

                        }
                    });
                }
            });
        }
        else {
            $scope.produtocomposicao = {};
            $scope.produtocomposicao.PRODUTO_COMPOSICAO_ITEM = [];
            $scope.produtocomposicao.PRODUTO_COMPOSICAO_TIPO_PERIODO = [];
        }



    }

    $scope.getLstProdutos = function () {

        formHandlerService.read($scope, {
            url: Util.getUrl("/produto/lstprodutosvenda"),
            targetObjectName: 'lstprodutos',
            responseModelName: 'produtos',
            success: function () {

            }
        });
    }

    $scope.getLstTipoPeriodo = function () {

        formHandlerService.read($scope, {
            url: Util.getUrl("/tipoperiodo/lsttipoperiodo"),
            targetObjectName: 'lsttipoperiodo',
            responseModelName: 'tiposPeriodo',
        });
    }

    $scope.addComposicaoItem = function (composicaoItem, subscope) {

        $scope.composicaoitem = composicaoItem;
        $scope.button = ($scope.EdicaoIndex == null) ? $scope.lstButton[1] : $scope.lstButtonAlterar[1];

        formHandlerService.submit($scope, {
            url: Util.getUrl("/produtocomposicaoitem/validar"),
            objectName: 'composicaoitem',
            showAjaxLoader: true,
            success: function (resp, status, config, message, validationMessage) {
                $scope.message = message;
                $scope.errosModal = validationMessage;

                if (resp.success) {// se não existe outra composição item com o produto passado

                    var sair = false;
                    angular.forEach($scope.produtocomposicao.PRODUTO_COMPOSICAO_ITEM, function (value, index) { // verifica se não existem dois ou mais itens de composição com o mesmo produto

                        if (($scope.EdicaoIndex == null || ($scope.EdicaoIndex != null && $scope.EdicaoIndex != index)) && $scope.composicaoitem.PRO_ID == value.PRO_ID) { // valida se existe o mesmo produto em outro item

                            //value.PRO_ID = null;
                            $scope.message = messageService.fail("Já existe um item de composição com o produto selecionado");
                            sair = true;
                            $scope.button = ($scope.EdicaoIndex == null) ? $scope.lstButton[0] : $scope.lstButtonAlterar[0];
                            //$scope.button = $scope.lstButton[0];
                        }
                    });

                    if (sair)
                        return;

                    //composicaoCopiado = angular.copy(composicao); // evita que modificações externas da composição modifique a lista consolidada

                    //composicaoCopiado.PRODUTO = composicaoCopiado.PRODUTOSCombo;
                    //composicaoCopiado.TIPO_PERIODO = composicaoCopiado.TIPO_PERIODOCombo;
                    //if (composicaoCopiado.PRODUTO) {
                    //    composicaoCopiado.NomeDoProduto = composicaoCopiado.PRODUTO.PRO_SIGLA;
                    //} 
                    
                    var index = $scope.EdicaoIndex;

                    //$scope.composicaoitem.PRODUTO_DESCRICAO = $scope.composicaoitem.PRODUTO_DESCRICAO;
                    if (index != null) {

                        //$scope.produtocomposicao.PRODUTO_COMPOSICAO_ITEM[index] = composicaoCopiado;
                        $scope.produtocomposicao.PRODUTO_COMPOSICAO_ITEM[index] = $scope.composicaoitem;

                        $scope.EdicaoIndex = null;
                    }
                    else {
                        //$scope.produtocomposicao.PRODUTO_COMPOSICAO_ITEM.push(composicaoCopiado);
                        $scope.produtocomposicao.PRODUTO_COMPOSICAO_ITEM.push($scope.composicaoitem);
                    }

                    angular.element("#modalComposicaoItem").modal('hide');
                    $scope.composicaoitem = {};
                }
                $scope.button = ($scope.EdicaoIndex == null) ? $scope.lstButton[0] : $scope.lstButtonAlterar[0];
                //$scope.button = $scope.lstButton[0];

            },
            fail: function () {
                //$scope.button = $scope.lstButton[0];
                $scope.button = ($scope.EdicaoIndex == null) ? $scope.lstButton[0] : $scope.lstButtonAlterar[0];
            }
        });

    }


    $scope.produtoSelecionado = function (composicao, item) {

        composicao.PRO_ID = composicao.PRODUTO.PRO_ID;
    }

    $scope.tipoPeriodoSelecionado = function (composicao, item) {

        composicao.TTP_ID = composicao.TIPO_PERIODO.TTP_ID;
    }

    $scope.removerComposicaoItem = function ($index) {

        if (confirm("Remover essa composição?")) {
            if ($scope.produtocomposicao && $scope.produtocomposicao.PRODUTO_COMPOSICAO_ITEM) {

                $scope.produtocomposicao.PRODUTO_COMPOSICAO_ITEM.splice($index, 1);
            }
        }

    }

    $scope.abrirEdicaoItem = function ($index, item) {

        // recarrega o id dos PRODUTO como se a combobox de ambos tivessem sido modificadas
        //item = angular.copy(item);
        //item.PRODUTOSCombo = item.PRODUTO;
        //item.TIPO_PERIODOCombo = item.TIPO_PERIODO;

        $scope.erros = null;
        $scope.EdicaoIndex = $index;
        $scope.composicaoitem = angular.copy(item);

        if ($scope.composicaoitem.PRODUTO) {

            $scope.composicaoitem.PRODUTO_NOME = $scope.composicaoitem.PRODUTO.PRO_NOME;
            $scope.select2CtrlItem.stayClose = true;
        }
        angular.element("#modalComposicaoItem").modal();
        $scope.button = $scope.lstButtonAlterar[0];
    }

    $scope.renovarForm = function () {

        $scope.erros = null;
        $scope.composicaoitem = {};
        $scope.EdicaoIndex = null;
    }

    

    $scope.getLstProdutosPorNome = function (nome, target) {

        if (!target) {

            target = 'lstProdutos';
        }
   
            var filtro = { nome: nome };

            formHandlerService.read($scope, {
                url: Util.getUrl("/produto/listarPorNome"),
                targetObjectName: target,
                responseModelName: 'lstProdutos',
                data: filtro,
                success: function () {

                }
            });
    }



    $scope.adicionarTipoPeriodo = function (item) {

        var tipoPerido = angular.copy(item);

        if(!Util.isPathValid($scope,'produtocomposicao.PRODUTO_COMPOSICAO_TIPO_PERIODO')){

            $scope.produtocomposicao.PRODUTO_COMPOSICAO_TIPO_PERIODO = [];
        }

        var produtoComposicaoTipoPeriodo = {

            TIPO_PERIODO: tipoPerido,
            DATA_ASSOCIACAO: new Date()
        };
        $scope.produtocomposicao.PRODUTO_COMPOSICAO_TIPO_PERIODO.push(produtoComposicaoTipoPeriodo);
        
        
    }

    $scope.inicializarTipoPeriodo = function () {

        angular.forEach($scope.lstTipoPeriodo, function (value, index) {

            value.show = true;
        });
    }

    $scope.acharTipoPeriodo = function (tiposPeriodo) {

        $scope.inicializarTipoPeriodo();
        if ($scope.lstTipoPeriodo) {

            angular.forEach($scope.lstTipoPeriodo, function (value, old) {

                angular.forEach(tiposPeriodo, function (subValue, subOld) {

                    if (value.TTP_ID == subValue.TIPO_PERIODO.TTP_ID) {

                        value.show = false;
                    }
                });
            });
        }
    }

    $scope.excluirTipoPeriodo = function (index, descricao) {

        if ($scope.produtocomposicao && $scope.produtocomposicao.PRODUTO_COMPOSICAO_TIPO_PERIODO) {

            $scope.produtocomposicao.PRODUTO_COMPOSICAO_TIPO_PERIODO.splice(index, 1);
        }
    }

    $scope.$watch("produtocomposicao.PRODUTO_COMPOSICAO_TIPO_PERIODO", function (value, old) {

        if (value) {
            $scope.acharTipoPeriodo(value);
        }

    }, true);


    $scope.getListaTipoPeriodo = function () {

        formHandlerService.read($scope, {
            url: Util.getUrl("/produtoComposicao/listarTipoPeriodo"),
            targetObjectName: 'lstTipoPeriodo',
            responseModelName: 'lstTipoPeriodo',
            success: function () {

                if ($scope.produtocomposicao && $scope.produtocomposicao.PRODUTO_COMPOSICAO_TIPO_PERIODO) {

                    $scope.acharTipoPeriodo($scope.produtocomposicao.PRODUTO_COMPOSICAO_TIPO_PERIODO);
                }

            }
        });
    }

    $scope.listarTipoVenda = function () {

        formHandlerService.read($scope, {
            url: Util.getUrl("/produtoComposicao/listarTipoVenda"),
            targetObjectName: 'lstTipoVenda',
            responseModelName: 'lstTipoVenda',
            success: function () {

            }
        });
    }


   $scope.listarProdutoComposicao = function (pageRequest) {

        $scope.listado = false;

        var url = Util.getUrl("/produtoComposicao/listarProdutosExcetoCursos");

        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }

        var config = {
            url: url,
            targetObjectName: 'lstProdutoComposicao',
            responseModelName: 'produtosComposicao',
            showAjaxLoader: true,
            pageConfig: { pageName: 'page' },
            success: function () {

                $scope.listado = true;
            }
        };

        if ($scope.filtro) {

            config.data = angular.copy($scope.filtro);
        }
        formHandlerService.read($scope, config);
    };

    $scope.abrirModalProdutoComposicao = function () {
        $scope.listarProdutoComposicao();
        angular.element("#modal-produto-composicao").modal();
    }

    $scope.adicionarProduto = function (item) {

        if ($scope.produtocomposicao) {

            $scope.produtocomposicao.PRODUTO_COMPOSICAO2 = item;
            $scope.produtocomposicao.CMP_ID_ORIGEM = item.CMP_ID;
        }
        angular.element("#modal-produto-composicao").modal('hide');
    }

    $scope.removerProdutoBase = function () {

        if (Util.isPathValid($scope, "produtocomposicao.PRODUTO_COMPOSICAO2")) {

            $scope.produtocomposicao.PRODUTO_COMPOSICAO2 = null;
            $scope.produtocomposicao.CMP_ID_ORIGEM = null;
        }
    }


    $scope.deletarProdutoComposicao = function (cmpId) {

        if (cmpId && confirm("Deseja realmente deletar esse produto?")) {
            $scope.deleteRequest = { cmpId: cmpId };

            formHandlerService.submit($scope, {
                url: Util.getUrl("/produtoComposicao/DeletarProdutoComposicao"),
                objectName: 'deleteRequest',
                showAjaxLoader: true,
                success: function (resp, status, config, message, validationMessage) {

                    if (resp.success) {

                        $scope.message = Util.createMessage("success", "Produto Deletado com Sucesso!");

                        $timeout(function () {
                            $scope.message = null;
                            $scope.listar();

                        }, 1000);
                    }
                },
                fail: function () {

                }
            });
        }
        else {
            $scope.proposta.origem = null;
        }
    };

    if (window.TabelaPrecoController !== undefined) {
        TabelaPrecoController($scope, formHandlerService, $http, conversionService, comparatorService, $timeout); // inclui o módulo de view para tabela de preço

        $scope.$watch("tab", function (value, old) {

            if (value != old && value == 2) {

                if ($scope.produtocomposicao && $scope.produtocomposicao.CMP_ID) {

                    var cmpId = $scope.produtocomposicao.CMP_ID;
                    $scope.readTabelaPreco(cmpId);
                }

            }
        });

    }
    else {
        console.info("Controlador de preco não encontrado");
    }

    if (window.NotaFiscalConfigController !== undefined) {
        NotaFiscalConfigController($scope, formHandlerService, $http, $timeout); // inclui o módulo de view para tabela de preço

        $scope.$watch("tab", function (value, old) {

            if (value != old && value == 3) {

                if ($scope.produtocomposicao && $scope.produtocomposicao.CMP_ID) {

                    var cmpId = $scope.produtocomposicao.CMP_ID;
                    $scope.listarNotaFiscalConfigPorProduto(cmpId);
                    $scope.initNtConfig($scope.produtocomposicao, 2);
                }
            }
        });

    }
    else {
        console.info("Controlador de nota fiscal config não encontrado");
    }


});