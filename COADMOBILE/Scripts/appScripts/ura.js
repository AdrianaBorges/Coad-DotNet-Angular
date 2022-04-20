appModule.controller("URAControler" , function ($scope, formHandlerService, $http, conversionService) {

    $scope.objura = {};
    $scope.filtro = {};
    $scope.CarregarProdutos = function () {

        showAjaxLoader();

        $http({
            url: "/URA/CarregarProdutos",
            method: "Post",
            dataType: 'json',
            data: { _ura_id: $scope.filtro.URA_ID }
        }).success(function (response) {

            $scope.filtro.PRO_ID  = null;
            $scope.listaUf        = null;
            $scope.listaProdAreas = null;

            if (response.success == true) {
                $scope.listaprodutos = response.result.listaprodutos;
            }
            else {
                alert(response.message.message);
            }
        })

        hideAjaxLoader();
    };
    $scope.CarregarAreas = function () {

        $http({
            url: "/URA/CarregarAreas",
            method: "Post",
            dataType: 'json',
            data: { _prod_id: $scope.filtro.PRO_ID, _ura_id: $scope.filtro.URA_ID }
        }).success(function (response) {

            if (response.success == true) {
                $scope.listaareapesp = response.result.listaarea;
            }
            else {
                alert(response.message.message);
            }
        })
    };
    $scope.CarregarUF = function () {

        $http({
            url: "/URA/CarregarUF",
            method: "Post",
            dataType: 'json',
            data: { _prod_id: $scope.filtro.PRO_ID, _ura_id: $scope.filtro.URA_ID }
        }).success(function (response) {

            if (response.success == true) {
                $scope.listaufpesp = response.result.listauf;
            }
            else {
                alert(response.message.message);
            }
        })
    };
    $scope.listar = function () {

        showAjaxLoader();
        
        $http({
            url: "/URA/Pesquisar",
            method: "Post",
            dataType: 'json',
            data: { _ura_id: $scope.filtro.URA_ID, _pro_id: $scope.filtro.PRO_ID }
        }).success(function (response) {

            if (response.success == true) {
                $scope.filtro.URA_VIP = response.result.UraProduto.URA_VIP;
                $scope.filtro.URA_ATIVA = response.result.UraProduto.URA_ATIVA;
                $scope.listaUf = response.result.listaUfAcesso;
                $scope.listaProdAreas = response.result.listaProdAreas;
            }
            else {
                alert(response.message.message);
            }
        })

        hideAjaxLoader();
       
    };
    
    //---- ---------------------------------------------------// 
    $scope.AtualizarUra = function (cliente) {
        
        if (confirm("Deseja relamente atualizar este item na URA?") == true)
        {
            showAjaxLoader();
        
            $http({
                url: "/URA/AtualizarUra",
                method: "Post",
                dataType: 'json',
                data: JSON.stringify(cliente) 
            }).success(function (response) {
                
                alert(response.message.message);
                
                hideAjaxLoader();
            })
        }


    };
    $scope.BloqueioUra = function (cliente) {

        showAjaxLoader();

        $http({
            url: "/URA/BloqueioUra",
            method: "Post",
            dataType: 'json',
            data: JSON.stringify(cliente)
        }).success(function (response) {

            alert(response.message.message);

            $scope.ConsultarClientesURA();

            hideAjaxLoader();
        })
  
    };
    $scope.ListarConfigURA = function (pageRequest) {

        showAjaxLoader();

        $http({
            url: "/URA/ListarConfigURA",
            method: "Post",
            dataType: 'json',
            data: { _ura_id: $scope.filtro.URA_ID, pagina: pageRequest }
        }).success(function (response) {

            if (response.success == true) {
                $scope.ListaConfig = response.result.ListaConfig;
                $scope.page = response.page;
            }
            else {
                alert(response.message.message);
            }

            hideAjaxLoader();
        })


    };
    //-------------------------------------------------------// 


    //---- Funções para buscar clientes que ja estão  na URA ---// 
    $scope.ConsultarClientesURA = function (pageRequest) {

        showAjaxLoader();

        $http({
            url: "/URA/ConsultarClientesURA",
            method: "Post",
            dataType: 'json',
            data: { _ura_id: $scope.filtro.URA_ID, _asn_id: $scope.filtro.ASN_NUM_ASSINATURA, _telefone: $scope.filtro.telefone, pagina: pageRequest }
        }).success(function (response) {

            if (response.success == true) {
                $scope.ClientesURA = response.result.ClientesURA;
                $scope.page = response.page;
            }
            else {
                alert(response.message.message);
            }

            hideAjaxLoader();
        })


    };
    //-----------------------------------------------------//

    //---- Funções para buscar clientes com acesso a URA ---// 
    $scope.listarClientes = function (pageRequest) {

        showAjaxLoader();
        
        $http({
            url: "/URA/ListarClientesURA",
            method: "Post",
            dataType: 'json',
            data: { _ura_id: $scope.filtro.URA_ID, _asn_id: $scope.filtro.ASN_NUM_ASSINATURA, pagina: pageRequest }
        }).success(function (response) {

            if (response.success == true) {
                $scope.listaClientes = response.result.listaClientes;
                $scope.page = response.page;
            }
            else {
                alert(response.message.message);
            }

            hideAjaxLoader();
        })


    };
    $scope.AbreJanelaBloqueio = function (item) {

        $scope.objura = item;

        $('#JanelaBloqueio').modal('show');

    };
    $scope.AbreJanelaCliente = function (_index) {

        $scope.indexretorno = _index;
        $('#pesquisaCliente').modal('show');

    };
    $scope.BuscarClientes = function (cli_nome) {

        var _data = { _cli_nome: cli_nome }

        $http({
            url: "/URA/BuscarClientes",
            method: "Post",
            dataType: 'json',
            data: _data
        }).success(function (response) {

            if (response.success == true) {
                $scope.dbCliente = response.result.listaClientes;
                $scope.page = response.page;
            }
            else {
                alert(response.message.message);
            }
        })

    };
    $scope.fechaJanelaCliente = function (_assinatura) {

        if (_assinatura.CLI_ID != null) {
            $scope.filtro.CLI_ID = _assinatura.CLI_ID;
            $scope.filtro.CLI_NOME = _assinatura.CLIENTES.CLI_NOME;
            $scope.filtro.ASN_NUM_ASSINATURA = _assinatura.ASN_NUM_ASSINATURA;
        }
        
    }
    //---------------------------------------//

    //---- Funções para editar ura_produto ---//
    $scope.SalvarConfigURA = function () {

        showAjaxLoader();

        $http({
            url: "/URA/Editar",
            method: "Post",
            //dataType: 'json',
            data: { _uraproduto: $scope.filtro, _uraconfig: $scope.listaUf, _uraProdutoArea: $scope.listaProdAreas}
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success) {
                alert(response.message.message);
            }
            else
                alert(response.message.message);

        }).error(function (response) {

            hideAjaxLoader();

            alert(response.message.message);
        })

    };

    //-----------------------------------------//
    
    //---- Funções para incluir ou excluir UF ---//
    $scope.additem = function (item) {

        existe = false;
        existepadrao = false;
        

        for (i = 0; i < $scope.listaUf.length; i++) {
            if (item.UF_SIGLA == $scope.listaUf[i].UF_SIGLA_ACESSO) {
                existe = true;
            }
        }

        novo = {};
        novo.URA_ID = $scope.filtro.URA_ID;
        novo.PRO_ID = $scope.filtro.PRO_ID;
        novo.UF_SIGLA_ACESSO = item.UF_SIGLA;
        novo.URA_ACESSO = 1;

        if (item && existe == false) {
            if ($scope.listaUf.indexOf(novo) === -1) {
                $scope.listaUf.push(novo);
            }
        }

        //---- Gravar Item no banco de Dados
        // $scope.Incluir(novo);
        //----

    };
    $scope.apagaitem = function (item) {
        
        //---- Apaga Item no banco de Dados
        //$scope.Excluir(item);
        //----

        var ind = $scope.listaUf.indexOf(item);

        if (ind !== -1) {
            $scope.listaUf.splice(ind, 1);
        }

    };
    //-----------------------------------------//

    //---- Funções para incluir ou excluir AREA ---//
    $scope.addarea = function (item) {

        existe = false;
        existepadrao = false;


        for (i = 0; i < $scope.listaProdAreas.length; i++) {
            if (item.ACO_ID == $scope.listaProdAreas[i].ACO_ID) {
                existe = true;
            }
        }

        novo = {};
        AREA_CONSULTORIA = {};
        AREA_CONSULTORIA.ACO_DESCRICAO = item.ACO_DESCRICAO;

        novo.URA_ID = $scope.filtro.URA_ID;
        novo.PRO_ID = $scope.filtro.PRO_ID;
        novo.ACO_ID = item.ACO_ID;
        novo.AREA_CONSULTORIA = AREA_CONSULTORIA;
        novo.UPA_ATIVO = true;
        
        if (item && existe == false) {
            if ($scope.listaProdAreas.indexOf(novo) === -1) {
                $scope.listaProdAreas.push(novo);
            }
        }

        //---- Gravar Item no banco de Dados
       // $scope.Incluirarea(novo);
        //----

    };
    $scope.apagaarea = function (item) {

        //---- Apaga Item no banco de Dados
       // $scope.Excluirarea(item);
        //----

        var ind = $scope.listaProdAreas.indexOf(item);

        if (ind !== -1) {
            $scope.listaProdAreas.splice(ind, 1);
        }

    };
    //-----------------------------------------//


});