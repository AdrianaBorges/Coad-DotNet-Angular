appModule.controller("ItemMenuControler", function ($scope, $http, formHandlerService, $timeout) {

    $scope.m = {};
    $scope.filtro = {};
    $scope.menu = {ITM_ATIVO: 0, ITM_EXTERNO: 0 };


    $scope.$watch('m.SIS_ID', function (value, oldvalue) {
        var nivel = 0;
        if (value != oldvalue) {
            if (value.ITM_MENU_NIVEL > 0)
                nivel = m.ITM_MENU_NIVEL - 1;
            $scope.BuscarMenuCombo  (value, 0);
        }

    });
    $scope.$watch('m.ITM_MENU_NIVEL', function (value, oldvalue) {
        var nivel = 0;
        if (value != oldvalue) {
            if (value > 0)
                nivel = value - 1;

            $scope.BuscarMenu($scope.m.SIS_ID, nivel);
        }

    });

    //$scope.$watch('menu.ITM_MENU_NIVEL', function (value, oldvalue) {
    //    var nivel = 0;
    //    if (value != oldvalue) {
    //        if (value > 0)
    //            nivel  - 1;

    //        $scope.BuscarMenuCombo($scope.menu.SIS_ID, nivel);
    //    }

    //});


    $scope.NivelMenuSelecionado = function () {

        var nivel = $scope.menu.ITM_MENU_NIVEL;
        if (nivel > 0)
            nivel--;

        $scope.BuscarMenuCombo($scope.menu.SIS_ID, nivel);
        
    }
    $scope.CarregarDadosTela = function (itm_id) {

        //$scope.BuscarItemMenu(itm_id);
        $scope.read(itm_id);
        
    };

    $scope.CarregarDadosTelaCons = function (sis_id, itm_id) {

        $scope.BuscarListaMenu(sis_id, itm_id);

    };

    $scope.EditarItemMenu = function () {

        showAjaxLoader();
        var url = ($scope.menu.ITM_ID) ? "/ItemMenu/EditarSalvar" : "/ItemMenu/Novo"; 
        $http({
            url: url,
            method: "Post",
            dataType: 'json',
            data: JSON.stringify($scope.menu)
        }).success(function (response) {

            hideAjaxLoader();

            if (response.Success) {
                alert(response.Message);
                location.href = "/ItemMenu/Index";
            }
            else
                alert(response.Message);

        }).error(function (response) {
            alert(response.Message);
        });

    };

    $scope.SalvarItemMenu = function () {

        showAjaxLoader();

        $http({
            url: "/ItemMenu/Novo",
            method: "Post",
            dataType: 'json',
            data: JSON.stringify($scope.menu)
        }).success(function (response) {

            hideAjaxLoader();

            if (response.Success) {
                alert(response.Message);
                location.href = "/ItemMenu/Index";
            }
            else
                alert(response.Message);

        })

    };

    $scope.BuscarItemMenu = function (itm_id) {

        var _data = { _itm_id: itm_id }

        $http({
            url: "/ItemMenu/BuscarItemMenu",
            method: "Post",
            dataType: 'json',
            data: _data
        }).success(function (response) {

            if (response.Success == null) {
                $scope.listaitemmenu = response;
            }
            else
                alert(response.Message);
        })

    };

    $scope.BuscarMenu = function (sis_id, itm_memu_nivel) {

        showAjaxLoader();

        var _data = { _sis_id: sis_id, _itm_memu_nivel: itm_memu_nivel }

        $http({
            url: "/ItemMenu/BuscarMenu",
            method: "Post",
            dataType: 'json',
            data: _data
        }).success(function (response) {

            hideAjaxLoader();
            if (response.Success == null) {
                $scope.listaMenu = response.result.listaitemmenu;
                $scope.page = response.page;
                $scope.listar();
            }
            else
                alert(response.Message);
        })

    };

    $scope.BuscarMenuCombo = function (sis_id, itm_memu_nivel) {

        showAjaxLoader();

        var _data = { _sis_id: sis_id, _itm_memu_nivel: itm_memu_nivel }

        $http({
            url: "/ItemMenu/BuscarMenuCombo",
            method: "Post",
            dataType: 'json',
            data: _data
        }).success(function (response) {

            hideAjaxLoader();
            if (response.Success == null) {
                $scope.listaMenu = response.result.listaitemmenu;
                $scope.listar();
            }
            else
                alert(response.Message);
        })

    };

    $scope.BuscarListaItemMenu = function (sis_id, itm_id, pagina) {

        $scope.listar();
        //showAjaxLoader();

        //var _data = { _itm_nivel: 1, _sis_id: sis_id, _itm_id_node: itm_id }

        //var url = "/ItemMenu/BuscarListaItemMenu";

        //if (pagina) {

        //    url += "?pagina=" + pagina;
        //}
        //$http({
        //    url: url,
        //    method: "Post",
        //    dataType: 'json',
        //    data: _data
        //}).success(function (response) {

        //    hideAjaxLoader();
        //    if (response.Success == null) {
        //        $scope.listaitemmenu = response.result.listaitemmenu;
        //        $scope.page = response.page;
        //    }
        //    else
        //        alert(response.Message);
        //})

    };
    $scope.BuscarListaSubMenu = function (sis_id, itm_id) {

        showAjaxLoader();

        var _data = { _itm_nivel: 2, _sis_id: sis_id, _itm_id_node: itm_id }

        $http({
            url: "/ItemMenu/BuscarListaItemMenu",
            method: "Post",
            dataType: 'json',
            data: _data
        }).success(function (response) {

            hideAjaxLoader();
            if (response.Success == null) {
                $scope.listasubitemmenu = response.result.listaitemmenu;
                $scope.page = response.page;
            }
            else
                alert(response.Message);
        })

    };

    $scope.listar = function (pageRequest) {

        showAjaxLoader();       
     
        var url = Util.getUrl("/ItemMenu/BuscarListaItemMenu");

        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }

        var config = {
            url: url,
            targetObjectName: 'listaitemmenu',
            responseModelName: 'listaitemmenu',
            pageConfig: { pageName: 'page' },
            success: function () {
                hideAjaxLoader();
            }
        };

        var _data = { _sis_id: $scope.m.SIS_ID,  _itm_id_node: $scope.m.ITM_NODE_ID};
      
        if ($scope.m) {

            config.data = _data;
        }

        formHandlerService.read($scope, config);

        hideAjaxLoader();
    };

    $scope.read = function (ITM_ID, ultimoCodigo) {

        if (ITM_ID) {
            
            var url = Util.getUrl("/ItemMenu/RecuperarDadosDoItemMenu");
            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'menu',
                responseModelName: 'menu',
                data: { ITM_ID: ITM_ID },
                success: function () {

                    if ($scope.menu) {                        
                        $scope.NivelMenuSelecionado();
                    }
                }
            });
        }
        else {
            $scope.menu = { ITM_ATIVO: 0, ITM_EXTERNO: 0, ITM_ID : ultimoCodigo };
        }
    }

    $scope.listarItensDeMenu = function (pagina,alvo, item) {

        $scope.listado = false;
        var url = Util.getUrl("/itemmenu/ItensDeMenu");

        if (pagina) {

            url += "?pagina=" + pagina;
        }

        var config =
        {
            url: url,
            targetObjectName: 'itensDeMenu',
            responseModelName: 'itensDeMenu',
            showAjaxLoader: true,
            data : $scope.filtro,
            pageConfig: { pageName: 'page' /*, pageTargetName: 'paginaPrioridades' */ },
            success: function (resp) {
                $scope.listado = true;
                if (!alvo) {

                    $scope.itensDeMenuGrid = angular.copy($scope.itensDeMenu);
                    $scope.paginaItemMenuGrid = angular.copy($scope.page);
                }
            }
        }

        if (alvo == 'grid') {

            config.targetObjectName = 'itensDeMenuGrid';
            config.pageConfig.pageTargetName = 'paginaItemMenuGrid';
        }
               
        if (item) {

            $scope.setItemPai(item);
            var id = item.ITM_ID;
            var nivel = item.ITM_MENU_NIVEL;

            nivel++;
            if (nivel < 0) {
                nivel = 0;
            }
            config.data = {
                ITM_NODE_ID: id,
                ITM_MENU_NIVEL: nivel
            };

        }
        else {

            $scope.setItemPai(null);
        }
        

        formHandlerService.read($scope, config);
    }

    $scope.obterSubMenus = function ($event, ITM_ID, item, selector) {

        $event.stopPropagation();
        $scope.listado = false;

        var url = Util.getUrl("/itemmenu/ObterSubMenus");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'subMenus',
            responseModelName: 'subMenus',
            data: { ITM_ID: ITM_ID },
            showAjaxLoader: true,
            success: function (resp) {

                if ($scope.subMenus && item) {

                    if ($scope.subMenus.length > 0) {

                        // Existe dois tipos de animação presentes.
                        //1º Do próprio angular .animate-repeat.
                        //2° Do boostrap 'collapse'
                        //Para não haver comflito. o padrão é deixar a classe colapse que mantém o elemento invisível
                        // quando é detectado que o elemente será preenchido então jogo a classe in para que o collpse
                        // torne o elemente visível. Assim o angular realiza a animação inicial. Depois as animaçoes de mostrar
                        // e ocultar fica a cardo do boostrap
                        angular.element(selector).addClass('in');
                    }
                    item.subMenus = angular.copy($scope.subMenus);
                }
            }
        });
        
    }   

    $scope.collapse = function (selector) {

        angular.element(selector).collapse('toggle');
    }

    $scope.dispararAcaoMenuClicado = function ($event, ITM_ID, item, selector) {
        
        if (item.aberto == undefined) {
            item.aberto = true;
        }
        else {
            item.aberto = !item.aberto;
        }

       
        if (item && (!item.subMenus || item.subMenus.length < 1)) {
            $scope.obterSubMenus($event, ITM_ID, item, selector);
        }
        else {
            $scope.collapse(selector);
        }      

    }

    $scope.setItemPai = function (item) { 
    
        $scope.itemPai = (item) ? angular.copy(item) : item;
    };

    $scope.salvar = function () {

        var url = Util.getUrl("/itemmenu/salvar");

        formHandlerService.submit($scope, {
            url: url,
            objectName: 'menu',
            success: function (resp, status, config, message, validationMessage) {
                $scope.message = message;
                $scope.erros = validationMessage;

                if (resp.success) {

                    $scope.message = Util.createMessage("success", "Salvo com sucesso!");
                    $timeout(function () {

                        $scope.message = null;
                        window.location = Util.getUrl("/itemmenu/index");

                    }, 1000);
                }
                else {
                    $scope.button = 'reset';
                }

            }
        });
    }

    $scope.abrirSubMenusNoGrid = function ($event, item) {

        $event.stopPropagation();
        $scope.listarItensDeMenu(null, 'grid', item);
    }


    $scope.CarregarItensMenuPerfil = function () {

        //$event.stopPropagation();
        $scope.listado = false;

        if (!$scope.filtroItensMenuPerfil) {

            $scope.filtroItensMenuPerfil = {};
        }

        var url = Util.getUrl("/itemmenu/CarregarItensMenuPerfil");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'itemMenuPerfil',
            responseModelName: 'itemMenuPerfil',
            data: $scope.filtroItensMenuPerfil,
            showAjaxLoader: true,
            success: function (resp) {
               

                $scope.itemMenuPerfilSelecionado = null;
                //if ($scope.subMenus) {

                //    if ($scope.subMenus.length > 0) {

                //        // Existe dois tipos de animação presentes.
                //        //1º Do próprio angular .animate-repeat.
                //        //2° Do boostrap 'collapse'
                //        //Para não haver conflito. o padrão é deixar a classe colapse que mantém o elemento invisível
                //        // quando é detectado que o elemente será preenchido então jogo a classe in para que o collapse
                //        // torne o elemente visível. Assim o angular realiza a animação inicial. Depois as animaçoes de mostrar
                //        // e ocultar fica a cardo do boostrap
                //        //angular.element(selector).addClass('in');
                //    }
                //    item.subMenus = angular.copy($scope.subMenus);
                //}
            }
        });

    }

    $scope.carregaPerfis = function (nome) {

        
        var parans = {};
        if ($scope.filtroItensMenuPerfil && $scope.filtroItensMenuPerfil._sis_id) {

            parans.SIS_ID = $scope.filtroItensMenuPerfil._sis_id;
        }

        if (nome) {
            parans.nome = nome;
        }

        var url = Util.getUrl("/perfil/ListarPerfis");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: "perfis",
            responseModelName: "perfis",
            data: parans,
            showAjaxLoader: false,
            success: function (resp) {
            }
        });
    }
    $scope.sistemaSelecionado = function () {

        $scope.list = [];
    }

    $scope.selecionaItemMenuPerfil = function ($event, item) {

        $event.stopPropagation();
        $scope.itemMenuPerfilSelecionado = item;
    }


    $scope.selecionaAcesso = function (itemMenuPerfil, campo, valor) {

        if (!valor && valor != 0) {

            valor = null;
        }

        if (itemMenuPerfil) {
            
            switch (campo) {

                case "acesso": {

                    if (valor == null) {
                        valor = itemMenuPerfil.NIV_ACESSO;
                        
                    }
                    else {
                        itemMenuPerfil.NIV_ACESSO = valor;
                    }

                    itemMenuPerfil.NIV_INSERT = valor;
                    itemMenuPerfil.NIV_EDIT = valor;
                    itemMenuPerfil.NIV_DELETE = valor;
                    break;
                }
                case "insert": {

                    if (valor == null) {
                        valor = itemMenuPerfil.NIV_INSERT;

                    }
                    else {
                        itemMenuPerfil.NIV_INSERT = valor;
                    }
                    break;
                }
                case "edit": {
                    
                    if (valor == null) {
                        valor = itemMenuPerfil.NIV_EDIT;

                    }
                    else {
                        itemMenuPerfil.NIV_EDIT = valor;
                    }
                    break;
                }
                case "delete": {

                    if (valor == null) {
                        valor = itemMenuPerfil.NIV_DELETE;

                    }
                    else {
                        itemMenuPerfil.NIV_DELETE = valor;
                    }
                    break;
                }
            }

            if (itemMenuPerfil.SUB_ITEM_MENU) {

                angular.forEach(itemMenuPerfil.SUB_ITEM_MENU, function (value, index) {

                    $scope.selecionaAcesso(value, campo, valor);
                });

            }
        }
    }


    $scope.nivelAcessoClicado = function (itemMenuPerfil) {

        $scope.selecionaAcesso(itemMenuPerfil, "acesso");
    }

    $scope.nivelInsertClicado = function (itemMenuPerfil) {

        $scope.selecionaAcesso(itemMenuPerfil, "insert");
    }
    $scope.nivelEditClicado = function (itemMenuPerfil) {

        $scope.selecionaAcesso(itemMenuPerfil, "edit");
    }
    $scope.nivelDeleteClicado = function (itemMenuPerfil) {

        $scope.selecionaAcesso(itemMenuPerfil, "delete");
    }

    $scope.salvarItemMenuPerfil = function () {

        var url = Util.getUrl("/itemmenu/SalvarItemMenuPerfil");

        formHandlerService.submit($scope, {
            url: url,
            objectName: 'itemMenuPerfil',
            success: function (resp, status, config, message, validationMessage) {
                $scope.message = message;
                $scope.erros = validationMessage;

                if (resp.success) {
                    $scope.CarregarItensMenuPerfil();
                    $scope.message = Util.createMessage("success", "Salvo com sucesso!");
                    $timeout(function () {

                        $scope.message = null;
                        $scope.button = 'reset'; //window.location = Util.getUrl("/perfil/configurar");

                    }, 1000);
                }
                else {
                    $scope.button = 'reset';
                }

            }
        });
    }

});