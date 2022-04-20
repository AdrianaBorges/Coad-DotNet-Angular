appModule.controller('CursoController', function ($scope, formHandlerService, $http, conversionService, messageService, comparatorService, $timeout, $parse) {
    
    $scope.lstButton = [{ label: 'Incluir', show: true }, { label: 'Inclindo...', show: false }];
    $scope.lstButtonAlterar = [{ label: 'Alterar', show: true }, { label: 'Alterando...', show: false }];
    //$scope.lstButtonSave = [{ label: 'Salvar', show: true }, { label: 'Salvando...', show: false }];
    $scope.button = $scope.lstButton[0];
   // $scope.buttonSave = $scope.lstButtonSave[0];


    $scope.carregarEmpresas = function () {

        var url = Util.getUrl("/empresa/listarEmpresas");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstEmpresas',
            responseModelName: 'lstEmpresas',
            showAjaxLoader: true,

        });
    };

    $scope.init = function () {

        $scope.matrizGrandeGrupo = [[]];

        $scope.listarColecionador();
        $scope.carregarEmpresas();
    };

    $scope.initList = function () {

        $scope.carregarEmpresas();
    };

    $scope.tab = 1;
    $scope.selecionarTab = function (value) {

        $scope.tab = value;
    }

    $scope.limparProdutoItemProduto = function (curso) {
        
    }

    $scope.salvarCurso = function () {

        //$scope.buttonSave = $scope.lstButtonSave[1];
        $scope.curso.EhCurso = true;

        if ($scope.curso.valido == false) {

            $scope.message = Util.createMessage("fail", "O preço não pode ser inferior ao valor dos itens de produto");
            return false;
        }

        formHandlerService.submit($scope, {
            url: Util.getUrl("/franquia/curso/salvar"),
            objectName: 'curso',
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

                        if (resp.result.curso) {

                            post(Util.getUrl("/franquia/curso/editar?composicaoId=" + resp.result.curso.CMP_ID));
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
    };

    $scope.listarCurso = function (pageRequest) {

        $scope.listado = false;

        var url = Util.getUrl("/curso/ListarCursos");

        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }

        var config = {
            url: url,
            targetObjectName: 'lstCursos',
            responseModelName: 'lstCursos',
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
                url: Util.getUrl("/curso/recuperarDadosDoCurso"),
                targetObjectName: 'curso',
                responseModelName: 'curso',
                data: { composicaoId: composicaoId },
                success: function () {

                    $scope.valorVendaAlterado();
                    //$scope.curso.PRODUTOS = null;

                    //if ($scope.curso.PRODUTOS) {
                    //    $scope.select2Ctrl.stayClose = true;
                    //    $scope.curso.PRODUTO_DESCRICAO = $scope.curso.PRODUTOS.PRO_NOME;
                    //    $scope.curso.PRODUTOS = null;
                    //}
                    // tratamentos para se adequar a tela                   
                    //angular.forEach($scope.curso.PRODUTO_COMPOSICAO_ITEM, function (value) {

                    //    value.PRODUTO_COMPOSICAO = null;
                    //    if (value.PRODUTOS) {
                    //        value.NOME_PRODUTO = value.PRODUTOS.PRO_NOME;

                    //    }
                    //});
                }
            });
        }
        else {
            $scope.curso = {
                AREA_CONSULTORIA_CURSO_PROXY: [],
                PRODUTO_COMPOSICAO_ITEM: [{

                    CMI_PRECO_UNIT: 0,
                    CMI_QTDE: 1,
                    TTP_ID: null,
                    CMI_QTDE_PERIODO : 1
                }]
            };

            $scope.obterProdutoCurso($scope.curso.PRODUTO_COMPOSICAO_ITEM[0]);
            //$scope.curso.PRODUTO_COMPOSICAO_ITEM = [];
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



    $scope.obterProdutoCurso = function (composicaoItem) {

        formHandlerService.read($scope, {
            url: Util.getUrl("/produto/ReadProduto"),
            targetObjectName: 'produto',
            responseModelName: 'produto',
            data : {produtoId : 40},
            success: function () {

                composicaoItem.PRODUTOS = $scope.produto;
                composicaoItem.PRO_ID = $scope.produto.PRO_ID;
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
        $scope.produtoTemp = $scope.composicaoitem.PRODUTOS;
        $scope.composicaoitem.PRODUTOS = null;


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
                    angular.forEach($scope.curso.PRODUTO_COMPOSICAO_ITEM, function (value, index) { // verifica se não existem dois ou mais itens de composição com o mesmo produto

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

                    $scope.composicaoitem.PRODUTOS = $scope.produtoTemp;

                    //composicaoCopiado = angular.copy(composicao); // evita que modificações externas da composição modifique a lista consolidada

                    //composicaoCopiado.PRODUTO = composicaoCopiado.PRODUTOSCombo;
                    //composicaoCopiado.TIPO_PERIODO = composicaoCopiado.TIPO_PERIODOCombo;
                    //if (composicaoCopiado.PRODUTO) {
                    //    composicaoCopiado.NomeDoProduto = composicaoCopiado.PRODUTO.PRO_SIGLA;
                    //} 
                    
                    var index = $scope.EdicaoIndex;

                    //$scope.composicaoitem.PRODUTO_DESCRICAO = $scope.composicaoitem.PRODUTO_DESCRICAO;
                    if (index != null) {

                        //$scope.curso.PRODUTO_COMPOSICAO_ITEM[index] = composicaoCopiado;
                        $scope.curso.PRODUTO_COMPOSICAO_ITEM[index] = $scope.composicaoitem;

                        $scope.EdicaoIndex = null;
                    }
                    else {
                        //$scope.curso.PRODUTO_COMPOSICAO_ITEM.push(composicaoCopiado);
                        $scope.curso.PRODUTO_COMPOSICAO_ITEM.push($scope.composicaoitem);
                    }

                    $scope.valorVendaAlterado();

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
            if ($scope.curso && $scope.curso.PRODUTO_COMPOSICAO_ITEM) {

                $scope.curso.PRODUTO_COMPOSICAO_ITEM.splice($index, 1);
                $scope.valorVendaAlterado();
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

    $scope.tipoComposicao = 'curso';
    $scope.renovarForm = function () {

        $scope.erros = null;
        $scope.composicaoitem = {};
        $scope.EdicaoIndex = null;
    }

    if (window.TabelaPrecoController !== undefined) {
        TabelaPrecoController($scope, formHandlerService, $http, conversionService, comparatorService, $timeout); // inclui o módulo de view para tabela de preço

        $scope.$watch("tab", function (value, old) {

            if (value != old && value == 2) {

                if ($scope.curso && $scope.curso.CMP_ID) {

                    var cmpId = $scope.curso.CMP_ID;
                    $scope.readTabelaPreco(cmpId);
                }

            }
        });

    }
    else {
        console.info("Controlador de preco não encontrado");
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

    $scope.listarProdutoAnexoPorNome = function (nome, target) {

        if (!target) {

            target = 'lstProdutos';
        }


        var filtro = { nome: nome };

        formHandlerService.read($scope, {
            url: Util.getUrl("/produto/ListarProdutoAnexoPorNome"),
            targetObjectName: target,
            responseModelName: 'lstProdutosAnexos',
            data: filtro,
            success: function () {

            }
        });
    }
        
    $scope.listarColecionador = function () {

        var url = Util.getUrl("/franquia/professor/LstColecionadores");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstColecionadores',
            responseModelName: 'lstColecionadores',
            showAjaxLoader: true,
            success: function (resp) {
            }
        });
    }

    $scope.listarGrandeGrupo = function (value, index) {

        var filter = { areConsId: value };
        var targetName = 'matrizGrandeGrupo[' + index + '].lstGrandeGrupo';

        if (value) {

            var url = Util.getUrl("/franquia/professor/LstGrandeGrupo");

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: targetName,
                responseModelName: 'lstGrandeGrupo',
                showAjaxLoader: true,
                data: filter,
                success: function (resp) {
                }
            });
        }
        else {
            var setter = $parse(targetName);
            setter.assign($scope, []);
        }
    }

    $scope.colecionadorSelecionado = function (value, index) {

        $scope.listarGrandeGrupo(value, index);
    }

    $scope.IncluirAreas = function () {

        if (Util.isPathValid($scope, 'curso.AREA_CONSULTORIA_CURSO_PROXY')) {

            var lstLength = $scope.curso.AREA_CONSULTORIA_CURSO_PROXY.length;

            if (lstLength > 0) {
                var lastObj = $scope.curso.AREA_CONSULTORIA_CURSO_PROXY[lstLength - 1];

                if (!lastObj.ARE_CONS_ID) {

                    $scope.message = Util.createMessage("fail", "Selecine pelo menos o colecionador antes de adicionar uma nova linha.");
                    return;
                }
            }
            var cmpId = $scope.curso.CMP_ID;
            $scope.curso.AREA_CONSULTORIA_CURSO_PROXY.push({ CMP_ID: cmpId });
        }
    }

    $scope.ExcluirAreas = function (index) {

        if (Util.isPathValid($scope, 'curso.AREA_CONSULTORIA_CURSO_PROXY')) {

            $scope.curso.AREA_CONSULTORIA_CURSO_PROXY.splice(index, 1);
        }

        angular.forEach($scope.curso.AREA_CONSULTORIA_CURSO_PROXY, function (value, index) {

            if (value && value.ARE_CONS_ID) {
                $scope.colecionadorSelecionado(value.ARE_CONS_ID, index)
            }
        });
    }

    $scope.valorVendaAlterado = function () {

        if ($scope.curso && $scope.curso.PRODUTO_COMPOSICAO_ITEM && $scope.curso.CMP_VLR_VENDA) {

            if ($scope.curso.PRODUTO_COMPOSICAO_ITEM.length == 1) {

                var obj = $scope.curso.PRODUTO_COMPOSICAO_ITEM[0];

                if (Util.isPathValid(obj, "PRODUTOS.GRUPO_ID") && obj.PRODUTOS.GRUPO_ID == 2) {

                    obj.CMI_PRECO_UNIT = $scope.curso.CMP_VLR_VENDA;
                }
            }
            else if ($scope.curso.PRODUTO_COMPOSICAO_ITEM.length > 1) {

                var valorCurso = 0;
                var somaLivros = 0;
                var objetoItemCurso = null;


                angular.forEach($scope.curso.PRODUTO_COMPOSICAO_ITEM, function (obj, index) {

                    if (Util.isPathValid(obj, "PRODUTOS.GRUPO_ID") && obj.PRODUTOS.GRUPO_ID == 2) {

                        objetoItemCurso = obj;
                    }
                    else {
                        var qtd = (obj.CMI_QTDE) ? obj.CMI_QTDE : 1;
                        somaLivros += (obj.CMI_PRECO_UNIT * (qtd));
                    }
                });

                if (objetoItemCurso != null) {

                    if ($scope.curso.CMP_VLR_VENDA <= somaLivros) {

                        $scope.message = Util.createMessage("warning", "O preço não pode ser inferior ao valor dos itens de produto");
                        objetoItemCurso.CMI_PRECO_UNIT = null;
                        $scope.curso.valido = false;
                        return;
                    }
                    $scope.curso.valido = true;
                    objetoItemCurso.CMI_PRECO_UNIT = ($scope.curso.CMP_VLR_VENDA - somaLivros);
                }
            }
        }
    };


    $scope.deletarCurso = function (cmpId) {

        if (cmpId && confirm("Deseja realmente deletar esse curso?")) {
            $scope.deleteRequest = { cmpId: cmpId };

            formHandlerService.submit($scope, {
                url: Util.getUrl("/produtoComposicao/DeletarProdutoComposicao"),
                objectName: 'deleteRequest',
                showAjaxLoader: true,
                success: function (resp, status, config, message, validationMessage) {

                    if (resp.success) {

                        $scope.message = Util.createMessage("success", "Curso Deletado com Sucesso!");

                        $timeout(function () {
                            $scope.message = null;
                            $scope.listarCurso();

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
    }

    if (window.NotaFiscalConfigController !== undefined) {
        NotaFiscalConfigController($scope, formHandlerService, $http, $timeout); 

        $scope.$watch("tab", function (value, old) {

            if (value != old && value == 3) {

                if ($scope.curso && $scope.curso.CMP_ID) {

                    var cmpId = $scope.curso.CMP_ID;
                    $scope.listarNotaFiscalConfigPorProduto(cmpId);
                    $scope.initNtConfig($scope.curso, 1);
                }
            }
        });

    }
    else {
        console.info("Controlador de nota fiscal config não encontrado");
    }


});