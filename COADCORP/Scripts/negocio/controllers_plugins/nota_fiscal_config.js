

function NotaFiscalConfigController($scope, formHandlerService, $http, $timeout) {

    $scope.initNtConfig = function (cmp, uenId) {

        $scope.listarTiposClientes();
        $scope.listarNotaFiscalConfigTipo();
        $scope.listarImpostos();
        $scope.objControle = {
            CMP_ID: cmp.CMP_ID,
            cmp: cmp
        };

        $scope.uenId = uenId;

        $scope.objSalvar = { NotaFiscalConfigAtualizar: [], NotaFiscalConfigExcluir: [] };
    };

    $scope.listarNotaFiscalConfigPorProduto = function (cmpId, pagina) {

        $scope.paginaAtual = pagina;
        $scope.listado = true;
        var url = Util.getUrl("/notaFiscalConfig/listarNotaFiscalConfigPorProduto");

        if (pagina) {

            url += "?pagina=" + pagina;
        }

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstNotaFiscalConfig',
            responseModelName: 'lstNotaFiscalConfig',
            showAjaxLoader: true,
            data: { cmpId: cmpId},
            //pageConfig: { pageName: 'page' , pageTargetName: 'pageNotaFiscalConfig'  },
            success: function (resp) {

            }
        });
    };


    $scope.abrirEdicaoNotaFiscalConfig = function ($index, item) {


        if (!item) {

            item = {

                CONFIG_IMPOSTO: []
            };
        }


        if (!item.CMP_ID) {

            item.CMP_ID = ($scope.objControle.CMP_ID) ? ($scope.objControle.CMP_ID) : undefined;
        }

        $index = ($index == 0 || $index) ? $index : undefined;
        
        $scope.itemNfeConfig = angular.copy(item);

        $scope.itemNfeConfig.IndexAtual = $index;
        angular.element("#modal-nf-config").modal();
    };


    $scope.abrirEdicaoConfigImposto = function ($index, item) {


        if (!item) {

            item = {

                CONFIG_IMPOSTO_IMPOSTO: []
            };
        }

        $index = ($index == 0 || $index) ? $index : undefined;
        //$scope.configImposto = {}; //
        $scope.configImposto = angular.copy(item);
        $scope.configImposto.IndexAtual = $index;

        angular.element("#modal-config-imposto").modal();
    };

    $scope.listarTiposClientes = function () {

        var url = Util.getUrl("/clientes/listarTiposCliente");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstTipoCliente',
            responseModelName: 'lstTipoCliente',
            showAjaxLoader: true
           
        });
    };


    $scope.listarNotaFiscalConfigTipo = function () {

        var url = Util.getUrl("/notaFiscalConfig/listarNotaFiscalConfigTipo");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstNotaFiscalConfigTp',
            responseModelName: 'lstNotaFiscalConfigTp',
            showAjaxLoader: true

        });
    };


    $scope.listarImpostos = function () {

        var url = Util.getUrl("/notaFiscalConfig/listarImpostos");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstImpostos',
            responseModelName: 'lstImpostos',
            showAjaxLoader: true,
            success: function () {

                $scope.inicializarImpostos();

                if ($scope.configImposto) {

                    $scope.acharImpostoAdicionado($scope.configImposto.CONFIG_IMPOSTO_IMPOSTO);
                }
            }

        });
    };
    
    $scope.inicializarImpostos = function () {

        angular.forEach($scope.lstImpostos, function (value, old) {

            value.show = true;
        });

    };

    $scope.adicionarImposto = function (imposto) {

        if (imposto && $scope.configImposto && $scope.configImposto.CONFIG_IMPOSTO_IMPOSTO) {

            if ($scope.verificaDuplicacaoImposto(imposto)) {

                var empId = $scope.EMP_ID;
                $scope.configImposto.CONFIG_IMPOSTO_IMPOSTO.push({ IMPOSTO: angular.copy(imposto), IMP_ID: imposto.IMP_ID});
            }
            else {

                $scope.message = Util.createMessage("fail", "Imposto já adicionado!");
            }

        }
    };

    $scope.verificaDuplicacaoImposto = function (item) {

        var achou = true;
        if (item && $scope.configImposto && $scope.configImposto.CONFIG_IMPOSTO_IMPOSTO) {

            angular.forEach($scope.configImposto.CONFIG_IMPOSTO_IMPOSTO, function (value, index) {

                if (value.IMPOSTO.IMP_ID === item.IMP_ID) {

                    achou = false;
                    return;
                }
            });
        }

        return achou;
    };

    $scope.$watch("configImposto.CONFIG_IMPOSTO_IMPOSTO", function (value, old) {

        if (value) {
            $scope.acharImpostoAdicionado(value);
        }

    }, true);

    $scope.acharImpostoAdicionado = function (imposto) {

        $scope.inicializarImpostos();

        if (imposto) {

            angular.forEach($scope.lstImpostos, function (value, old) {

                angular.forEach(imposto, function (subValue, subOld) {

                    if (subValue.IMPOSTO && value.IMP_ID == subValue.IMPOSTO.IMP_ID) {

                        value.show = false;
                    }
                });
            });
        }

    };

    $scope.excluirImpostos = function (index) {

        if ($scope.configImposto && $scope.configImposto.CONFIG_IMPOSTO_IMPOSTO) {

            $scope.configImposto.CONFIG_IMPOSTO_IMPOSTO.splice(index, 1);

        }
    };

    $scope.salvarEdicaoConfigImposto = function () {

        if ($scope.configImposto && $scope.itemNfeConfig) {

            formHandlerService.submit($scope, {
                url: Util.getUrl("/notaFiscalConfig/validarConfigImposto"),
                objectName: 'configImposto',
                showAjaxLoader: true,
                success: function (resp, status, config, message, validationMessage) {
                    $scope.message = message;
                    $scope.erros = validationMessage;

                    if (resp.success) {

                        if ($scope.configImposto.IndexAtual || $scope.configImposto.IndexAtual >= 0) {

                            var index = $scope.configImposto.IndexAtual;
                            delete $scope.configImposto.IndexAtual;
                            $scope.itemNfeConfig.CONFIG_IMPOSTO[index] = $scope.configImposto;
                        }
                        else {
                            $scope.itemNfeConfig.CONFIG_IMPOSTO.push($scope.configImposto);
                        }

                        angular.element("#modal-config-imposto").modal('hide');
                    }

                },
                fail: function () { }
            });

            
        }
    };

    $scope.cancelarEdicaoConfigImposto = function () {

        $scope.configImposto = null;
        angular.element("#modal-config-imposto").modal('hide');
    };

    $scope.salvarEdicaoNfConfig = function () {

        if ($scope.itemNfeConfig) {

            formHandlerService.submit($scope, {
                url: Util.getUrl("/notaFiscalConfig/validarNotaFiscalConfig"),
                objectName: 'itemNfeConfig',
                showAjaxLoader: true,
                success: function (resp, status, config, message, validationMessage) {
                    $scope.message = message;
                    $scope.erros = validationMessage;

                    if (resp.success) {


                        if ($scope.itemNfeConfig.IndexAtual >= 0) {

                            var index = $scope.itemNfeConfig.IndexAtual;
                            delete $scope.itemNfeConfig.IndexAtual;
                            $scope.lstNotaFiscalConfig[index] = $scope.itemNfeConfig;
                        }
                        else {

                            $scope.lstNotaFiscalConfig.push($scope.itemNfeConfig);
                        }
                        angular.element("#modal-nf-config").modal('hide');
                    }

                },
                fail: function () {}
            });
        }
    };

    $scope.cancelarEdicaoNfConfig = function () {

        $scope.itemNfeConfig = null;
        angular.element("#modal-nf-config").modal('hide');
    };


    $scope.salvarNotaFiscalConfig = function () {

        $scope.objSalvar.NotaFiscalConfigAtualizar = $scope.lstNotaFiscalConfig;

        formHandlerService.submit($scope, {
            url: Util.getUrl("/notaFiscalConfig/salvar"),
            objectName: 'objSalvar',
            showAjaxLoader: true,
            success: function (resp, status, config, message, validationMessage) {
                $scope.message = message;
                $scope.erros = validationMessage;

                $scope.buttonSave = 'reset';
                if (resp.success) {

                    $scope.message = message;

                    $timeout(function () {

                        if (confirm("Voltar para listagem")) {

                            if ($scope.tipoComposicao == 'curso') {
                                window.open(Util.getUrl('/franquia/curso/index'), '_self');
                            } else {
                                window.open(Util.getUrl('/produtocomposicao/index'), '_self');
                            }
                        }
                        else {
                            $scope.message = null;
                            $scope.listarNotaFiscalConfigPorProduto($scope.objControle.CMP_ID);
                        }

                    }, 1000);
                }
            },
            fail: function () {
            }
        });
    };

    $scope.DeletarNotaFiscalConfig = function ($index) {

        if ($scope.lstNotaFiscalConfig) {

            var obj = angular.copy($scope.lstNotaFiscalConfig[$index]);
            $scope.lstNotaFiscalConfig.splice($index, 1);
            $scope.objSalvar.NotaFiscalConfigExcluir.push(obj);
        }
    };

    $scope.DeletarConfigImposto = function ($index) {

        if ($scope.itemNfeConfig && $scope.itemNfeConfig.CONFIG_IMPOSTO) {

            $scope.itemNfeConfig.CONFIG_IMPOSTO.splice($index, 1);
        }
    };

    $scope.tipoClienteSelecionado = function (configImposto) {

        if (configImposto) {

            if (configImposto.TIPO_CLIENTE) {

                configImposto.TIPO_CLI_ID = configImposto.TIPO_CLIENTE.TIPO_CLI_ID;
            }
            else {
                configImposto.TIPO_CLI_ID = null;
            }
        }
    };

    $scope.configImpSelecionado = function (itemNfeConfig) {

        if (itemNfeConfig) {

            if (itemNfeConfig.NOTA_FISCAL_CONFIG_TIPO) {

                itemNfeConfig.NCT_ID = itemNfeConfig.NOTA_FISCAL_CONFIG_TIPO.NCT_ID;
            }
            else {
                itemNfeConfig.NCT_ID = null;
            }
        }
    };


    $scope.abrirModalProduto = function () {

        $scope.filtro = {};
        $scope.listarProdutoComposicao();
        angular.element("#modal-produto").modal();

    };

    $scope.abrirModalClonarConfig = function () {

        $scope.modalCloneConfig = {

            ProdutoComposicaoDestino: $scope.objControle.cmp
        };

        angular.element("#modal-clonar-configs").modal();
    };

    $scope.listarProdutoComposicao = function (pageRequest) {

        $scope.listado = false;
        var url = Util.getUrl("/produtoComposicao/ListarProdutosPorUen");

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
            config.data.uenId = $scope.uenId;
            config.data.empId = $scope.objControle.cmp.EMP_ID;
        }
        formHandlerService.read($scope, config);
    };

    $scope.cancelarClonagem = function (){

        $scope.modalCloneConfig = null;
        angular.element("#modal-clonar-configs").modal('hide');
    };

    $scope.removerProdutoConfig = function () {

        $scope.modalCloneConfig.ProdutoComposicaoOrigem = null;
    };
       
    $scope.adicionarProduto = function (item) {

        if ($scope.modalCloneConfig) {

            $scope.modalCloneConfig.ProdutoComposicaoOrigem = item;
            angular.element("#modal-curso").modal('hide');
            angular.element("#modal-produto").modal('hide');
        }
    };


    $scope.clonarConfiguracao = function () {

        if ($scope.modalCloneConfig &&
            $scope.modalCloneConfig.ProdutoComposicaoOrigem &&
            $scope.modalCloneConfig.ProdutoComposicaoDestino) {

            var msg = "Deseja copiar a configuração?";

            if ($scope.lstNotaFiscalConfig && $scope.lstNotaFiscalConfig.length > 1) {

                msg += " Já existe uma configuração. A mesma será sobreescrita.";
            }
                
            if (confirm(msg)) {
                $scope.request = {
                    cmpIdOrigem: $scope.modalCloneConfig.ProdutoComposicaoOrigem.CMP_ID,
                    cmpIdDestino: $scope.modalCloneConfig.ProdutoComposicaoDestino.CMP_ID
                };
                formHandlerService.submit($scope, {
                    url: Util.getUrl("/notaFiscalConfig/clonarConfiguracao"),
                    objectName: 'request',
                    showAjaxLoader: true,
                    success: function (resp, status, config, message, validationMessage) {
                        $scope.message = message;
                        $scope.erros = validationMessage;

                        $scope.buttonCopy = 'reset';
                        if (resp.success) {

                            $scope.message = message;
                            if (resp.result && resp.result.nfConfigClonada) {

                                if ($scope.lstNotaFiscalConfig) {

                                    angular.forEach($scope.lstNotaFiscalConfig, function (value, index) {

                                        $scope.objSalvar.NotaFiscalConfigExcluir.push(value);
                                    });
                                }
                                $scope.lstNotaFiscalConfig = resp.result.nfConfigClonada;
                            };

                            $timeout(function () {
                                $scope.message = null;
                                angular.element("#modal-clonar-configs").modal('hide');

                            }, 1000);
                        }
                    },
                    fail: function () {
                    }
                });
            }
            else {
                return false;
            }
        }
    };


};
