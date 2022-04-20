appModule.controller('RegiaoController', function ($scope, formHandlerService, $http, conversionService, messageService, comparatorService, $timeout, $parse) {

    $scope.query = { show: true };
    $scope.queryMunicipio = { show: true };

    $scope.initEdit = function () {

        $scope.carregarUENs();
        $scope.listarUfs();
    }

    $scope.inicializarUfs = function () {

        angular.forEach($scope.listUfs, function (value, index) {

            value.show = true;
        });
    }

    $scope.init = function () {

        $scope.carregarUENs();
    }

    $scope.dropRef = {};

    $scope.carregarUENs = function () {

        var url = Util.getUrl('/UEN/ListarUENs');
        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstUEN',
            responseModelName: 'lstUEN',
            success: function () {

            }
        });

    }
    $scope.salvar = function () {

        formHandlerService.submit($scope, {
            url: Util.getUrl("/franquia/regiao/salvar"),
            objectName: 'regiao',
            deepConvertDate: true,
            showAjaxLoader: true,
            success: function (resp, status, config, message, validationMessage) {
                $scope.message = message;
                $scope.erros = validationMessage;
                $scope.button = 'reset';

                //$scope.buttonSave = $scope.lstButtonSave[0];
                if (resp.success) {

                    $scope.message = Util.createMessage("success", "Salvo com sucesso!");
                    $timeout(function () {

                        window.location = Util.getUrl("/franquia/regiao/index");
                        
                    }, 1000);
                }
            },
            fail: function () {
                $scope.buttonSave = 'reset';
            }
        });
    }

    $scope.adicionarUfOuMunicipio = function (idDrag, idDrop, value) {

        if ($scope.regiao) {
            var value = $scope.objUfMunicipio;
            if (idDrag == 'drag_uf') {

                $scope.adicionarUf(value);
            }
            else if (idDrag == 'drag_municipio') {
                
                if(value){
                    $scope.adicionarMunicipio(value);
                }
            }
        }
    }

    $scope.collapse = function (selector) {

        angular.element(selector).collapse('toggle');
    }

    $scope.dispararAcaoMenuClicado = function ($event, item, selector) {

        if (item.aberto == undefined) {
            item.aberto = true;
        }
        else {
            item.aberto = !item.aberto;
        }

        $scope.collapse(selector);

    }

    $scope.inicializarMunicipio = function (item) {

        angular.forEach(item.MUNICIPIO, function (value, index) {

            var ufPai = angular.copy(item);
            ufPai.MUNICIPIO = null;

            value.UF1 = ufPai;
            value.show = true;
        });
    }

    $scope.returnMunicipios = function ($event, UF, item, selector) {

        $event.stopPropagation();
        $scope.listado = false;

        var url = Util.getUrl("/endereco/municipiosporuf");

        formHandlerService.read(item, {
            url: url,
            targetObjectName: 'MUNICIPIO',
            responseModelName: 'municipios',
            data: { uf: UF },
            showAjaxLoader: true,
            success: function (resp) {

                if (item && item.MUNICIPIO && item.MUNICIPIO.length > 0) {

                    angular.forEach(item.MUNICIPIO, function (value, index) {

                        value.show = true;
                    });
                    // Existe dois tipos de animação presentes.
                    //1º Do próprio angular .animate-repeat.
                    //2° Do boostrap 'collapse'
                    //Para não haver comflito. o padrão é deixar a classe colapse que mantém o elemento invisível
                    // quando é detectado que o elemente será preenchido então jogo a classe in para que o collpse
                    // torne o elemente visível. Assim o angular realiza a animação inicial. Depois as animaçoes de mostrar
                    // e ocultar fica a cardo do boostrap

                    angular.element(selector).addClass('in');
                }
            }
        });

    }

    $scope.dispararAcaoMenuClicadoEBuscarMunicipios = function ($event, item, selector) {

        if (item.aberto == undefined) {
            item.aberto = true;
        }
        else {
            item.aberto = !item.aberto;
        }

        if (item && (!item.MUNICIPIO || item.MUNICIPIO.length < 1)) {
            $scope.returnMunicipios($event, item.UF, item, selector);
        }
        else {
            $scope.collapse(selector);
        }

    }

    $scope.listarRegiao = function (pageRequest) {

        $scope.listado = false;

        var url = Util.getUrl("/regiao/listarRegiao");

        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }

        var config = {
            url: url,
            targetObjectName: 'lstRegioes',
            responseModelName: 'lstRegioes',
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

    $scope.listarUfs = function () {

        $scope.listado = false;

        var url = Util.getUrl("/regiao/ListarUfs");

        var config = {
            url: url,
            targetObjectName: 'listUfs',
            responseModelName: 'listUfs',
            showAjaxLoader: true,
            pageConfig: { pageName: 'page' },
            success: function () {

                $scope.listado = true;
                $scope.inicializarUfs();
            }
        };

        if ($scope.filtro) {

            config.data = angular.copy($scope.filtro);
        }
        formHandlerService.read($scope, config);
    };

    $scope.getMunicipioPorUf = function () {

        $scope.municipios = null;
        $scope.filtro.munId = null;

        if ($scope.filtro.uf) {

            formHandlerService.read($scope, {
                url: Util.getUrl("/endereco/municipiosporuf"),
                targetObjectName: 'municipios',
                responseModelName: 'municipios',
                data: { uf: $scope.filtro.uf },
                success: function () {

                }
            });
        }

    }

    $scope.read = function (regiaoId) {

        $scope.carregarUENs();
        $scope.listarUfs();

        $scope.Uf1 = "A";
        $scope.Uf2 = "B";
        $scope.Uf3 = "C";
        $scope.Uf4 = "D";
        $scope.Uf5 = "E";
        
        if (regiaoId) {

            formHandlerService.read($scope, {
                url: Util.getUrl("/regiao/RecuperarDadosDaRegiao"),
                targetObjectName: 'regiao',
                responseModelName: 'regiao',
                data: { regiaoId: regiaoId },
                success: function () {

                }
            });
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

    $scope.verificaDuplicacaoUF = function (uf) {

        var podeAdicionar = true;
        if (uf && $scope.regiao && $scope.regiao.UF) {

            angular.forEach($scope.regiao.UF, function (value, index) {

                if (value.UF_SIGLA === uf.UF_SIGLA) {
                    podeAdicionar = false;
                }
            });
        }

        return podeAdicionar;
    }


    $scope.adicionarUf = function (uf) {

        if (uf && $scope.regiao && $scope.regiao.UF) {

            if ($scope.verificaDuplicacaoUF(uf)) {
                $scope.regiao.UF.push(uf);
            }
            else {

                $scope.message = Util.createMessage("fail", "Região já adicionado!");
            }

        }
    }


    $scope.acharUf = function (ufs) {
        
        $scope.inicializarUfs();
        if ($scope.listUfs) {

            angular.forEach($scope.listUfs, function (value, old) {

                angular.forEach(ufs, function (subValue, subOld) {

                    if (value.UF_SIGLA == subValue.UF_SIGLA) {

                        value.show = false;
                    }
                });
            });
        }
    }

    $scope.excluirUfs = function (index, descricao) {

        if ($scope.regiao && $scope.regiao.UF) {

            $scope.regiao.UF.splice(index, 1);
        }
    }

    $scope.$watch("regiao.UF", function (value, old) {

        if (value) {
            $scope.acharUf(value);
        }

    }, true);


        
    $scope.verificaDuplicacaoMuncipio = function (municipio) {

        var podeAdicionar = true;
        if (municipio && $scope.regiao && $scope.regiao.MUNICIPIO) {

            angular.forEach($scope.regiao.MUNICIPIO, function (value, index) {

                if (value.MUN_ID === municipio.MUN_ID) {
                    podeAdicionar = false;
                }
            });
        }

        return podeAdicionar;
    }


    $scope.adicionarMunicipio = function (municipio) {

        if (municipio && $scope.regiao) {

            if ($scope.verificaDuplicacaoMuncipio(municipio)) {
                $scope.regiao.MUNICIPIO.push(municipio);
            }
            else {

                $scope.message = Util.createMessage("fail", "Municipio já adicionado!");
            }

        }
    }


    $scope.acharMunicipio = function (uf, municipios) {
        $scope.inicializarMunicipio(uf);
        if (uf.MUNICIPIO) {

            angular.forEach(uf.MUNICIPIO, function (value, old) {

                angular.forEach(municipios, function (subValue, subOld) {

                    if (value.MUN_ID == subValue.MUN_ID) {

                        value.show = false;
                    }
                });
            });
        }
    }

    $scope.excluirMunicipio = function (index, descricao) {

        if ($scope.regiao && $scope.regiao.MUNICIPIO) {

            $scope.regiao.MUNICIPIO.splice(index, 1);
        }
    }

    $scope.$watch("regiao.UF", function (value, old) {

        if (value) {
            $scope.acharUf(value);
        }

    }, true);


    $scope.returnObjetoUf = function (id_uf) {

        var objeto = null;
        var achou = false;
        angular.forEach($scope.listUfs, function (value, index) {

            if (achou == false && value.UF_SIGLA == id_uf) {

                objeto = value;
                achou = true;
            }
        });

        return objeto;
    }

    $scope.removeUfOuMunicipio = function (idDrag, idDrop) {

        
        var item = $scope.objRemoveUf;
        if (item && idDrop == "drop_remove") {

            var index = item.$index;
            if (idDrag == "drag_remove_uf") {

                $scope.regiao.UF.splice(index, 1);
            }
            else if (idDrag == "drag_remove_municipio") {

                var obj = $scope.regiao.MUNICIPIO[index];
                $scope.ufDoMunicipio = obj.UF1;
                $scope.regiao.MUNICIPIO.splice(index, 1);
            }

        }
    }

    $scope.abrirModalEmpresa = function () {

        $scope.listarEmpresa();
        angular.element("#modal-empresa").modal();
    }

    $scope.removerEmpresa = function () {

        if ($scope.regiao) {

            $scope.regiao.EMP_ID = null;
            $scope.regiao.EMPRESA = null;
        }
    }
    $scope.adicionarEmpresa = function (emp) {

        if ($scope.regiao) {

            $scope.regiao.EMP_ID = emp.EMP_ID;
            $scope.regiao.EMPRESA = angular.copy(emp);
        }

        angular.element("#modal-empresa").modal("hide");
    }

    $scope.listarEmpresa = function (pageRequest) {

        $scope.listado = false;

        var url = Util.getUrl("/regiao/listarEmpresa");

        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }

        var config = {
            url: url,
            targetObjectName: 'lstEmpresa',
            responseModelName: 'lstEmpresa',
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
});