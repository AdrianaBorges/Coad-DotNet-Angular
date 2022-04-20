appModule.controller('ManualdpController', function ($scope, formHandlerService, $http, $interval, conversionService, $sce) {

    $scope.param = {};
    $scope.editor = {};
    $scope.manual = {};
    $scope.open = false;
    $scope.open1 = false;
    $scope.open2 = false;
    $scope.mostrar = true;
    $scope.filtro = {};
    $scope.trustAsHtml = function (string) {
        return $sce.trustAsHtml(string);
    };
    $scope.initModulo = function (modulo) {

        showAjaxLoader();

        var url = "/Modulo/Init";
        $http({
            url: url,
            method: "post",
            data: {_mod_id : modulo}

        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.item = response.result.item;
                conversionService.deepConversion($scope.item);

            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
                $scope.item = null;

                hideAjaxLoader();
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);
            $scope.item = null;

            hideAjaxLoader();
        });
    }

    $scope.listarItensAlterados = function (paginaReq) {
        
        showAjaxLoader();

        var url = "/Manualdp/ListarItensAlterados";
        $http({
            url: url,
            method: "post",
            data: { _pagina: paginaReq }

        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.ItensAlterados = response.result.ItensAlterados;
                conversionService.deepConversion($scope.ItensAlterados);
                $scope.page = response.page;
            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
                $scope.ItensAlterados = null;

                hideAjaxLoader();
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);
            $scope.ItensAlterados = null;

            hideAjaxLoader();
        });
    }

    $scope.addLink = function () {

        if ($scope.item.MANUAL_DP_LINK == null) {
            $scope.item.MANUAL_DP_LINK = [];
        }

        var novo = {};

        for (var ind in $scope.item.LINKS) {
            novo[$scope.item.MANUAL_DP_LINK[ind].LNK_TAG] = null;
            novo[$scope.item.MANUAL_DP_LINK[ind].LNK_DESCRICAO] = null;
            novo[$scope.item.MANUAL_DP_LINK[ind].LNK_LINK] = null;
        }

        $scope.item.MANUAL_DP_LINK.push(novo);

    }
 
    $scope.add = function () {

        if ($scope.item.FUNDAMENTACAO == null) {
            $scope.item.FUNDAMENTACAO = [];
        }

        var novo = {};

        for (var ind in $scope.item.FUNDAMENTACAO) {
            novo[$scope.item.FUNDAMENTACAO[ind].TCI_NOME_CAMPODB] = null;
            novo[$scope.item.FUNDAMENTACAO[ind].TIP_ATO_ID] = null;
            novo[$scope.item.FUNDAMENTACAO[ind].MAI_NUMERO_ATO] = null;
            novo[$scope.item.FUNDAMENTACAO[ind].MAI_DATA_ATO] = null;
            novo[$scope.item.FUNDAMENTACAO[ind].ORG_ID] = null;
            novo[$scope.item.FUNDAMENTACAO[ind].MAI_NUMERO_ARTIGO] = null;
        }

        $scope.item.FUNDAMENTACAO.push(novo);

    }
    $scope.removeLINK = function (index) {

        $scope.item.MANUAL_DP_LINK.splice(index, 1);
    }
    $scope.remove = function (index) {

        $scope.item.FUNDAMENTACAO.splice(index, 1);
    }
    $scope.AddItemNovo = function (novo) {

        $scope.novo = novo

    }
    $scope.abriModalAddItem = function () {

        angular.element("#Modal-Add-Item").modal();

    }
    $scope.abriModalAssunto = function (novo) {

        if (novo == 1)
            $scope.manual = {};

        $scope.listarModulos();

        angular.element("#Modal-Incluir-Assunto").modal();

    }
    $scope.abriModalExcluirAssunto = function () {

        $scope.listarModulos();

        angular.element("#Modal-Excluir-Assunto").modal();

    }
    $scope.abrirPopUpReferencia = function (item) {

        $scope.LINK_SELECT = item;

        $scope.listarModulos();

        angular.element("#Modal-Referencia").modal();

    }
    $scope.selecAssunto = function (mod_id,man_id) {

        $scope.LINK_SELECT.LNK_LINK = '<a title="' + $scope.LINK_SELECT.LNK_DESCRICAO + '" href="/Manualdp/AbrirDocDocumento?param01=' + mod_id + '&param02=' + man_id + '">' + $scope.LINK_SELECT.LNK_DESCRICAO + '</a>';

    }
    $scope.selecItem = function (mai_id) {

        $scope.LINK_SELECT.LNK_LINK = '<a title="' + $scope.LINK_SELECT.LNK_DESCRICAO + '" href="/Manualdp/AbrirDocDocumento?param01=' + mai_id + '" >' + $scope.LINK_SELECT.LNK_DESCRICAO + '</a>';

    }
    $scope.abriModalEditarAssunto = function (item) {

        $scope.itemselect = {};

        if (item != null)
            $scope.itemselect = item;

        angular.element("#Modal-Incluir-Assunto").modal();

        $scope.listarItensPorAssunto(item.MAN_ID);

    }
    $scope.detalharModulo = function (item) {

        $scope.item = {}
        $scope.item.MOD_ID = item.MOD_ID;

        showAjaxLoader();

        var url = "/Manualdp/PesquisarAssuntoPorModulo";
        $http({
            url: url,
            method: "post",
            data: { _assunto: null, _mod_id: $scope.item.MOD_ID }

        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.listaassunto = response.result.listaassunto;
                conversionService.deepConversion($scope.listaassunto);

            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
                $scope.listaassunto = null;

                hideAjaxLoader();
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);
            $scope.listaassunto = null;

            hideAjaxLoader();
        });
       
    }
    $scope.detalharAssunto = function (item) {

        $scope.detalhe = {};
        $scope.itemselect = item;

        $scope.detalhe.MAN_ASSUNTO = item.MAN_ASSUNTO;
        //$scope.detalhe.MAI_TITULO = "Sumário";

        $scope.listarItensPorAssunto(item.MAN_ID);

    }
    $scope.detalharItem = function (item) {

        $scope.detalhe = {};

        $scope.detalhe.MOD_DESCRICAO = item.MOD_DESCRICAO;
        $scope.detalhe.MAN_ASSUNTO = item.MAN_ASSUNTO;
        $scope.detalhe.MAI_TITULO = item.MAI_TITULO;
        $scope.detalhe.USU_LOGIN = item.USU_LOGIN;
        $scope.detalhe.USU_LOGIN_ALT = item.USU_LOGIN_ALT;
        $scope.detalhe.DATA_INSERT = item.DATA_INSERT;
        $scope.detalhe.DATA_ALTERA = item.DATA_ALTERA;
        $scope.detalhe.MAI_DATA_PUBLICACAO = item.MAI_DATA_PUBLICACAO;
        $scope.detalhe.MAI_ID = item.MAI_ID;
        $scope.detalhe.MOD_ID = item.MOD_ID;
        $scope.detalhe.MAI_DATA_PUBLICACAO = item.MAI_DATA_PUBLICACAO;

        $scope.detalhe.MAI_DESCRICAO = item.MAI_DESCRICAO;

        angular.element("#Modal-Detalhe").modal();

    }
    $scope.carregaTela = function (id) {

        showAjaxLoader();

        $scope.listarModulos();


        var url = "/Manualdp/CarregarTela";
        $http({
            url: url,
            method: "post",
            data: { _mai_id: id }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.item = response.result.item;
                $scope.item.MOD_ID = $scope.item.MANUAL_DP.MOD_ID;
                $scope.listarAssuntoPorModulo();
                conversionService.deepConversion($scope.item);
            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
                $scope.item = null;

                hideAjaxLoader();
            }

        }).error(function (response) {

            alert(message);

            $scope.message = Util.createMessage("fail", response);
            $scope.item = null;

            hideAjaxLoader();
        });

    }
    
    $scope.initSumario = function (mai_id, man_id, mod_id) {

        $scope.listarModulos();
        $scope.listarItensAlterados();

        var item = {};
        item.MAI_ID = mai_id;
        item.MOD_DESCRICAO = "";
        item.MAI_TITULO = "";
        item.MAN_ASSUNTO = "";

        $scope.abrirDocumento(item);

        //if (mai_id == null)
        //    $scope.abrirDocumento(item);
        
        if (mai_id != null && mai_id > 0)
            $scope.abrirItem(mai_id);

        if ( (mod_id != null && mod_id > 0) && (man_id != null && man_id > 0) )
            $scope.abrirAssunto(mod_id, man_id);


    }
    $scope.abrejanelaAddItem = function (item) {

        $scope.confirmaEdit = false;
        $scope.confirmaNovo = false;

        if (item != null) {
            $scope.item1 = item;
            $scope.confirmaEdit = true;
        }
        else {
            $scope.item1 = {};
            $scope.confirmaNovo = true;
        }

        angular.element("#modal-add-item").modal();
    }
    $scope.abrirRecente = function (item, id) {

        if (id == null)
            id = item.MAI_ID;

        showAjaxLoader();

        if ($scope.detalhe == null)
            $scope.detalhe = {};

        $scope.item = null;
        $scope.filtro.pesquisa = null;
        $scope.listaporassunto = null;


        var url = "/Manualdp/AbrirDoc";
        $http({
            url: url,
            method: "post",
            data: { _mai_id: id }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.item = response.result.item;
                $scope.item.MOD_ID = $scope.item.MANUAL_DP.MOD_ID;
                conversionService.deepConversion($scope.item);

                ///----
                $scope.filtro.pesquisa = null;
                $scope.detalhe.MOD_ID = $scope.item.MOD_ID
                $scope.detalhe.MOD_DESCRICAO = item.MOD_DESCRICAO
                $scope.detalhe.MAN_ID = item.MAN_ID
                $scope.detalhe.MAN_ASSUNTO = item.MAN_ASSUNTO
  
                //------

                //$scope.listarAssuntoPorModulo();
            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
                $scope.item = null;

                hideAjaxLoader();
            }

        }).error(function (response) {

            alert(message);

            $scope.message = Util.createMessage("fail", response);
            $scope.item = null;

            hideAjaxLoader();
        });
    }

    $scope.abrirItem = function (id) {

        $scope.abrirRecente(null, id);
   
    }
    $scope.abrirAssunto = function (mod_id, man_id) {

        showAjaxLoader();
         
        var url = "/Manualdp/BuscarModuloAssunto";
        $http({
            url: url,
            method: "post",
            data: { _mod_id: mod_id, _man_id: man_id }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {
                
                $scope.modulo = response.result.modulo;
                $scope.assunto = response.result.assunto;
                conversionService.deepConversion($scope.modulo);
                conversionService.deepConversion($scope.assunto);

                ///----
                $scope.abrirDocumento($scope.modulo, $scope.assunto);
                ///----
            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
                $scope.item = null;

                hideAjaxLoader();
            }

        }).error(function (response) {

            alert(message);

            $scope.message = Util.createMessage("fail", response);
            $scope.item = null;

            hideAjaxLoader();
        });

    }
    $scope.abrirDocumento = function (item, item1) {


        $scope.item = null;
        $scope.filtro.pesquisa = null;
        $scope.listaporassunto = null;

        if ($scope.detalhe == null)
            $scope.detalhe = {};

        $scope.filtro.pesquisa = null;

        $scope.detalhe.MOD_ID = item.MOD_ID
        $scope.detalhe.MOD_DESCRICAO = item.MOD_DESCRICAO

        if (item1 != null) {
            $scope.detalhe.MAN_ID = item1.MAN_ID
            $scope.detalhe.MAN_ASSUNTO = item1.MAN_ASSUNTO
        }

        $scope.listarItensPorAssunto($scope.detalhe.MAN_ID);


    }
    $scope.publicarAssunto = function (item, tipo) {

        $scope.detalhe = {};
        $scope.itemselect = item;
        $scope.detalhe.MAN_ASSUNTO = item.MAN_ASSUNTO;
        
        $http({
            method: 'Post',
            //dataType: 'json',
            url: '/Manualdp/PublicarAssunto',
            data: { _manual: $scope.itemselect, _tipo: tipo }

        }).success(function (response) {

            hideAjaxLoader();
                        
            if (response.success == true) {

                $scope.listarAssuntoPorModulo();

            //       $scope.message = Util.createMessage("success", "Operação realizada com sucesso!!");
   
            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);

            hideAjaxLoader();
        });

    }   
    $scope.publicarAssuntoGeral = function (tipo) {

        $http({
            method: 'Post',
            //dataType: 'json',
            url: '/Manualdp/PublicarAssuntoGeral',
            data: { _manual: $scope.itemselect, _tipo: tipo }

        }).success(function (response) {

            hideAjaxLoader();

            //     $scope.erros = response.validationMessage;

            if (response.success == true) {

                $scope.listaassunto = response.result.listaassunto;

                $scope.message = Util.createMessage("success", "Operação realizada com sucesso!!");
           
            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);

            hideAjaxLoader();
        });

    }
    $scope.restaurarIndice = function (tipo) {

        $http({
            method: 'Post',
            //dataType: 'json',
            url: '/Manualdp/RestaurarIndice',
            data: { _manual: $scope.itemselect, _tipo: tipo }

        }).success(function (response) {

            hideAjaxLoader();

            //     $scope.erros = response.validationMessage;

            if (response.success == true) {

                $scope.listarItensPorAssunto($scope.itemselect.MAN_ID);

                $scope.message = Util.createMessage("success", "Operação realizada com sucesso!!");
                
            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);

            hideAjaxLoader();
        });

    }
    $scope.ordenarUp = function (lista,item, indice) {

        if (indice > 0) {
            if (lista == null) {
                lista = [];
            }

            var atual = lista[indice - 1];

            item.MAI_INDEX = indice - 1;
            atual.MAI_INDEX = indice;

            lista[indice - 1] = item;
            lista[indice] = atual;

            $scope.salvarOrdenado(item, atual);
        }

    }
    $scope.ordenarDown = function (lista,item, indice) {

        if (lista.length > (indice + 1)) {
            if (lista == null) {
                lista = [];
            }

            var atual = lista[indice + 1];

            item.MAI_INDEX = indice + 1;
            atual.MAI_INDEX = indice;

            lista[indice + 1] = item;
            lista[indice] = atual;

            $scope.salvarOrdenado(item, atual);
        }

    }
    $scope.ordenarAssuntoUp = function (lista,item, indice) {

        if (indice > 0) {
            if (lista == null) {
                lista = [];
            }

            var atual = lista[indice - 1];

            item.MAN_INDEX = indice - 1;
            atual.MAN_INDEX = indice;

            lista[indice - 1] = item;
            lista[indice] = atual;

            $scope.salvarAssuntoOrdenado(item, atual);
        }

    }
    $scope.ordenarAssuntoDown = function (lista, item, indice) {

        if (lista.length > (indice + 1)) {
            if (lista == null) {
                lista = [];
            }

            var atual = lista[indice + 1];

            item.MAN_INDEX = indice + 1;
            atual.MAN_INDEX = indice;

            lista[indice + 1] = item;
            lista[indice] = atual;

            $scope.salvarAssuntoOrdenado(item, atual);
        }

    }
    $scope.salvarOrdenado = function (regitem, regatual) {

        $http({
            method: 'Post',
            //dataType: 'json',
            url: '/Manualdp/SalvarOrdenado',
            data: { _item: regitem, _atual: regatual }

        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.listarItensPorAssunto($scope.itemselect.MAN_ID);

            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);

            hideAjaxLoader();
        });
    }
    $scope.salvarAssuntoOrdenado = function (regitem, regatual) {

        $http({
            method: 'Post',
            //dataType: 'json',
            url: '/Manualdp/SalvarAssuntoOrdenado',
            data: { _item: regitem, _atual: regatual }

        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.listarAssuntoPorModulo();

            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);

            hideAjaxLoader();
        });
    }
    $scope.listarItensRelacionados = function (titulo) {

        showAjaxLoader();

        var url = "/Manualdp/ListarItensRelacionados";
        $http({
            url: url,
            method: "post",
            data: { _titulo: titulo }

        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.listaitensrel = response.result.listaitensrel;
                conversionService.deepConversion($scope.listaitensrel);

            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
                $scope.listaitensrel = null;

                hideAjaxLoader();
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);
            $scope.listaitensrel = null;

            hideAjaxLoader();
        });

    }
    $scope.listarItensPorAssunto = function (idassunto) {

        showAjaxLoader();

        var url = "/Manualdp/ListarItensPorAssunto";
        $http({
            url: url,
            method: "post",
            data: { _man_id: idassunto }

        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.listaporassunto = response.result.listaporassunto;

                conversionService.deepConversion($scope.listaporassunto);
                
            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
                $scope.listaitensrel = null;

                hideAjaxLoader();
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);
            $scope.listaitensrel = null;

            hideAjaxLoader();
        });

    }

    $scope.buscarModulo = function (idmodulo) {

        showAjaxLoader();

        var url = "/Manualdp/BuscarModulo";
        $http({
            url: url,
            method: "post",
            data: { _mod_id: idmodulo }

        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.modulo = response.result.modulo;

            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
                $scope.modulo = null;

                hideAjaxLoader();
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);
            $scope.modulo = null;

            hideAjaxLoader();
        });

    }
    $scope.listarModulos = function () {

        showAjaxLoader();

        var url = "/Manualdp/ListarModulos";
        $http({
            url: url,
            method: "post"
            //,data: { _man_id: idassunto }

        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.listamodulo = response.result.listamodulo;

            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
                $scope.listamodulo = null;

                hideAjaxLoader();
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);
            $scope.listaitensrel = null;

            hideAjaxLoader();
        });

    }
    $scope.buscarPalavraChave = function (palavraChave,pageRequest) {

        showAjaxLoader();

        var url = "/Manualdp/BuscarPalavraChave";
        $http({
            url: url,
            method: "post",
            data: { _mai_descricao: palavraChave, _pagina: pageRequest }

        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.listaitens = response.result.listaitens;

                conversionService.deepConversion($scope.listaitens);

                
                $scope.page = response.page;
            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
                $scope.listaitens = null;

                hideAjaxLoader();
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);
            $scope.listaitens = null;

            hideAjaxLoader();
        });

    }

    $scope.identarItem = function (item, nomeul) {

        showAjaxLoader();

        var _maiid = item.MAI_ID;

	    var url = "/Manualdp/IdentarItem";
        $http({
            url: url,
            method: "post",
            data: { _item: item }

        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.listaporassunto = response.result.listaporassunto;
                conversionService.deepConversion($scope.listaporassunto);

                //for (var ind in $scope.listaporassunto) {
                //    $scope.listaporassunto[ind].DATA_INSERT = $scope.ConvertDateJSON($scope.listaporassunto[ind].DATA_INSERT);
                //    $scope.listaporassunto[ind].DATA_ALTERA = $scope.ConvertDateJSON($scope.listaporassunto[ind].DATA_ALTERA);
                //    for (var ind0 in $scope.listaporassunto[ind].MANUAL_DP_ITEM1) {
                //        $scope.listaporassunto[ind].MANUAL_DP_ITEM1[ind0].DATA_INSERT = $scope.ConvertDateJSON($scope.listaporassunto[ind].MANUAL_DP_ITEM1[ind0].DATA_INSERT);
                //        $scope.listaporassunto[ind].MANUAL_DP_ITEM1[ind0].DATA_ALTERA = $scope.ConvertDateJSON($scope.listaporassunto[ind].MANUAL_DP_ITEM1[ind0].DATA_ALTERA);

                //        var item03 = $scope.listaporassunto[ind].MANUAL_DP_ITEM1[ind0].MANUAL_DP_ITEM1;
                //        for (var ind1 in item03) {
                //            item03[ind1].DATA_INSERT = $scope.ConvertDateJSON(item03[ind1].DATA_INSERT);
                //            item03[ind1].DATA_ALTERA = $scope.ConvertDateJSON(item03[ind1].DATA_ALTERA);

                //        }

                //    }
                //}



            }
            else {


                $scope.message = Util.createMessage("fail", response.message.message);
                
                hideAjaxLoader();
            }

        }).error(function (response) {

            alert(response);

            $scope.message = Util.createMessage("fail", response);

            hideAjaxLoader();
        });

    }
    $scope.recuarItem = function (item, nomeul) {

        showAjaxLoader();

        var _maiid = item.MAI_ID;

        var url = "/Manualdp/RecuarItem";
        $http({
            url: url,
            method: "post",
            data: { _item: item }

        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.listaporassunto = response.result.listaporassunto;
                conversionService.deepConversion($scope.listaporassunto);

                //for (var ind in $scope.listaporassunto) {
                //    $scope.listaporassunto[ind].DATA_INSERT = $scope.ConvertDateJSON($scope.listaporassunto[ind].DATA_INSERT);
                //    $scope.listaporassunto[ind].DATA_ALTERA = $scope.ConvertDateJSON($scope.listaporassunto[ind].DATA_ALTERA);
                //    for (var ind0 in $scope.listaporassunto[ind].MANUAL_DP_ITEM1) {
                //        $scope.listaporassunto[ind].MANUAL_DP_ITEM1[ind0].DATA_INSERT = $scope.ConvertDateJSON($scope.listaporassunto[ind].MANUAL_DP_ITEM1[ind0].DATA_INSERT);
                //        $scope.listaporassunto[ind].MANUAL_DP_ITEM1[ind0].DATA_ALTERA = $scope.ConvertDateJSON($scope.listaporassunto[ind].MANUAL_DP_ITEM1[ind0].DATA_ALTERA);

                //        var item03 = $scope.listaporassunto[ind].MANUAL_DP_ITEM1[ind0].MANUAL_DP_ITEM1;
                //        for (var ind1 in item03) {
                //            item03[ind1].DATA_INSERT = $scope.ConvertDateJSON(item03[ind1].DATA_INSERT);
                //            item03[ind1].DATA_ALTERA = $scope.ConvertDateJSON(item03[ind1].DATA_ALTERA);

                //        }


                //    }
                //}



            }
            else {


                $scope.message = Util.createMessage("fail", response.message.message);

                hideAjaxLoader();
            }

        }).error(function (response) {

            alert(response);

            $scope.message = Util.createMessage("fail", response);

            hideAjaxLoader();
        });

    }
    $scope.initConfigurar = function () {

        $scope.listarModulos();

    }
    $scope.listarAssuntoPorModulo = function (assunto) {

        if ($scope.item == null)
            $scope.item = {};

        if ($scope.manual.MOD_ID != null)
            $scope.item.MOD_ID = $scope.manual.MOD_ID;

        if ($scope.param.MOD_ID != null)
            $scope.item.MOD_ID = $scope.param.MOD_ID;

        $scope.param.MAN_ID = null;

        showAjaxLoader();

        var url = "/Manualdp/PesquisarAssuntoPorModulo";
        $http({
            url: url,
            method: "post",
            data: { _assunto: assunto, _mod_id: $scope.item.MOD_ID }

        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.listaassunto = response.result.listaassunto;
                for (var ind in $scope.listaassunto) {
                    $scope.listaassunto[ind].MAN_DATA_PUBLICACAO = $scope.ConvertDateJSON($scope.listaassunto[ind].MAN_DATA_PUBLICACAO);
                }
            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
                $scope.listaassunto = null;

                hideAjaxLoader();
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);
            $scope.listaassunto = null;

            hideAjaxLoader();
        });

    }
    $scope.listarAssunto = function (assunto) {

        showAjaxLoader();

        var url = "/Manualdp/PesquisarAssunto";
        $http({
            url: url,
            method: "post",
            data: { _assunto: assunto}

        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.listaassunto = response.result.listaassunto;
                for (var ind in $scope.listaassunto) {
                    $scope.listaassunto[ind].MAN_DATA_PUBLICACAO = $scope.ConvertDateJSON($scope.listaassunto[ind].MAN_DATA_PUBLICACAO);
                }
            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
                $scope.listaassunto = null;

                hideAjaxLoader();
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);
            $scope.listaassunto = null;

            hideAjaxLoader();
        });

    }
    $scope.listarReferencia = function (MAI_TITULO) {

        showAjaxLoader();

        var url = "/Manualdp/PesquisarReferencia";
        $http({
            url: url,
            method: "post",
            data: {
                _mai_titulo: MAI_TITULO
            }

        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.listaitens = response.result.listaitens;

                conversionService.deepConversion($scope.listaitens);
            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
                $scope.listaitens = null;

                hideAjaxLoader();
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);
            $scope.listaitens = null;

            hideAjaxLoader();
        });

    }
    $scope.listar = function (pageRequest) {

        showAjaxLoader();

        var url = "/Manualdp/Pesquisar";
        $http({
            url: url,
            method: "post",
            data: {
                _param: $scope.param,
                _pagina: pageRequest
            }

        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.listaitens = response.result.listaitens;

                conversionService.deepConversion($scope.listaitens);

                $scope.page = response.page;
            }
            else {

                $scope.message = Util.createMessage("fail", response.message);
                $scope.listaitens = null;

                hideAjaxLoader();
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response.message);
            $scope.listaitens = null;

            hideAjaxLoader();
        });
        
    }
    $scope.salvarModulo = function () {

        $http({
            method: 'Post',
            //dataType: 'json',
            url: '/Modulo/Salvar',
            data: { _modulo: $scope.item }

        }).success(function (response) {

            hideAjaxLoader();

            //     $scope.erros = response.validationMessage;

            if (response.success == true) {

                if (confirm("Operação realizada com sucesso. Incluir um novo módulo?"))
                    window.location = "/Modulo/Editar?_mai_id=";
                else
                    window.location = "/Modulo/Index";

            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);

            hideAjaxLoader();
        });

    }
    $scope.excluirModulo = function (item) {

        if (!confirm("Deseja realmente excluir este módulo?"))
            return;


        $http({
            method: 'Post',
            //dataType: 'json',
            url: '/Modulo/Excluir',
            data: { _modulo: item }

        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                alert(response.message.message);

                window.location = "/Modulo/Index";
              

            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);

            hideAjaxLoader();
        });

    }

    $scope.salvar = function () {

        $http({
            method: 'Post',
            //dataType: 'json',
            url: '/Manualdp/Salvar',
            data: { _manual: $scope.item }

        }).success(function (response) {

            hideAjaxLoader();

       //     $scope.erros = response.validationMessage;

            if (response.success == true) {

                if (confirm("Operação realizada com sucesso. Incluir um novo assunto?"))
                    window.location = "/Manualdp/Editar?_mai_id=";
                else
                    window.location = "/Manualdp/Index";

            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);

            hideAjaxLoader();
        });

    }
    $scope.publicar = function (item, tipo) {

        $http({
            method: 'Post',
            //dataType: 'json',
            url: '/Manualdp/Publicar',
            data: { _mai_id: item, _tipo:tipo }

        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.listar($scope.paginaReq);

            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);

            hideAjaxLoader();
        });

    }
    $scope.salvarAssunto = function (assunto) {

        $http({
            method: 'Post',
            //dataType: 'json',
            url: '/Manualdp/SalvarAssunto',
            data: { _manual: assunto }

        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.item = {}
                $scope.item.MOD_ID = $scope.itemselect.MOD_ID;

                $scope.listarAssuntoPorModulo();

           //     $scope.message = Util.createMessage("success", "Operação realizada com sucesso!!");
                $scope.manual = {};
                $scope.manual.MAN_ASSUNTO = null;
            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);

            hideAjaxLoader();
        });

    }
    $scope.excluirAssunto = function () {

        $http({
            method: 'Post',
            //dataType: 'json',
            url: '/Manualdp/ExcluirAssunto',
            data: { _manual: $scope.itemselect }

        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.item.MOD_ID = $scope.itemselect.MOD_ID;

                $scope.listarAssuntoPorModulo();

                $scope.message = Util.createMessage("success", "Operação realizada com sucesso!!");
                
            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);

            hideAjaxLoader();
        });

    }

    $scope.Excluir = function () {


        if (confirm("Deseja relamente excluir este registro? ")) {

            $http({
                method: 'Post',
                //dataType: 'json',
                url: '/Manualdp/Excluir',
                data: { _mai_id: $scope.item.MAI_ID }

            }).success(function (response) {

                hideAjaxLoader();

                if (response.success == true) {

                    $scope.message = Util.createMessage("success", "Operação realizada com sucesso!!");
                    window.location = "/Manualdp/Index";
                }
                else {

                    $scope.message = Util.createMessage("fail", response.message.message);
                }

            }).error(function (response) {

                $scope.message = Util.createMessage("fail", response);

                hideAjaxLoader();
            });
        }

    }

    $scope.formatarData = function (_data) {

        var data = new Date(_data);
        var dia = data.getDate();
        if (dia.toString().length == 1)
            dia = "0" + dia;
        var mes = data.getMonth() + 1;
        if (mes.toString().length == 1)
            mes = "0" + mes;
        var ano = data.getFullYear();

        return dia + "/" + mes + "/" + ano;

    }

    function dataAtualFormatada(jsDate) {
        var data = jsDate;
        var dia = data.getDate();
        if (dia.toString().length == 1)
            dia = "0" + dia;
        var mes = data.getMonth() + 1;
        if (mes.toString().length == 1)
            mes = "0" + mes;
        var ano = data.getFullYear();
        return dia + "/" + mes + "/" + ano;
    }

    function dataHoraFormatada(jsDate) {
        var data = jsDate;
        var dia = data.getDate();
        if (dia.toString().length == 1)
            dia = "0" + dia;
        var mes = data.getMonth() + 1;
        if (mes.toString().length == 1)
            mes = "0" + mes;
        var ano = data.getFullYear();

        var hora = data.getHours();
        var min = data.getMinutes();
        var seg = data.getSeconds();
        var hora = hora + ':' + min + ':' + seg;


        return dia + "/" + mes + "/" + ano + " " + hora;
    }

    function mascaraValor(number) {

        var numero = (number.toFixed(2).toString().split("."));

        var v = "";
        v = numero[0] + numero[1];
        v = v.replace(/\D/g, "") // permite digitar apenas numero 
        v = v.replace(/(\d{1})(\d{15})$/, "$1.$2") // coloca ponto antes dos ultimos 15 digitos 
        v = v.replace(/(\d{1})(\d{11})$/, "$1.$2") // coloca ponto antes dos ultimos 11 digitos 
        v = v.replace(/(\d{1})(\d{8})$/, "$1.$2") // coloca ponto antes dos ultimos 8 digitos 
        v = v.replace(/(\d{1})(\d{5})$/, "$1.$2") // coloca ponto antes dos ultimos 5 digitos 
        v = v.replace(/(\d{1})(\d{1,2})$/, "$1,$2") // coloca virgula antes dos ultimos 2 digitos 

        return v;
    }

    $scope.ConvertDateJSON = function (jsondata) {

        var data = null;

        if (jsondata != null)
            data = new Date(parseInt(jsondata.substr(6)));

        return data;
    }


});
