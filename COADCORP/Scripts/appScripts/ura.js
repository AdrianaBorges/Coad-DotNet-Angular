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
            $scope.filtro.UF_SIGLA_ACESSO = null;
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
            data: { _prod_id: $scope.filtro.PRO_ID, _ura_id: $scope.filtro.URA_ID, _uf_sigla: $scope.filtro.UF_SIGLA_ACESSO }
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
    $scope.listarConfig = function () {

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
                $scope.filtro.UF_SIGLA_ACESSO = null;
                $scope.listaProdAreas = null;
            }
            else {
                alert(response.message.message);
            }
        })

        hideAjaxLoader();
       
    };

    $scope.buscarProdAreas = function (item) {

        $scope.filtro.UF_SIGLA_ACESSO = item.UF_SIGLA_ACESSO;

        showAjaxLoader();

        $http({
            url: "/URA/BuscarProdAreas",
            method: "Post",
            dataType: 'json',
            data: { _ura_id: $scope.filtro.URA_ID, _pro_id: $scope.filtro.PRO_ID, _uf_sigla: $scope.filtro.UF_SIGLA_ACESSO }
        }).success(function (response) {

            if (response.success == true) {
                $scope.listaProdAreas = response.result.listaProdAreas;
            }
            else {
                $scope.listaProdAreas = [];
            }
        })

        hideAjaxLoader();

    };




    //---------------------------------------------------------//

    $scope.mostrarDetConsultas = function (_asn) {

        showAjaxLoader();

        var url = "/Cliente/BuscarListaApuracaoConsulta";
        $http({
            url: url,
            method: "post",
            data: { _assinatura: _asn.codigo, _contrato: null }
        }).success(function (retorno) {

            hideAjaxLoader();

            if (retorno.success) {

                $scope.listaQtdeConsEmail = retorno.result.listaQtdeConsEmail;

                angular.forEach($scope.listaQtdeConsEmail, function (obj, index) {
                    obj.contratadas = _asn.contratadas;
                    obj.realizadoura = _asn.qtdtotal;
                });

                angular.element("#modal-det-consultas").modal();
            }
            else {

                alert(response.message.message);
            }

        }).error(function (response) {

            alert(response);

            hideAjaxLoader();
        });

    }
    $scope.mostrarListaConsultas = function (ConsEmail, uraid) {

        showAjaxLoader();

        var url = "/URA/ListaConsultasUra";
        $http({
            url: url,
            method: "post",
            data: { _asn_id: ConsEmail.codigo, _ura_id: uraid }
        }).success(function (retorno) {

            hideAjaxLoader();

            if (retorno.success) {

                $scope.detconsultas = retorno.result.detconsultas;

                angular.forEach($scope.detconsultas, function (obj, index) {
                    obj.HAU_DATA_CADASTRO = $scope.dataHoraFormatada(obj.HAU_DATA_CADASTRO);
                });


                angular.element("#modal-det-cons-analitico").modal();
            }
            else {

                alert(response.message.message);
            }

        }).error(function (response) {

            alert(response);

            hideAjaxLoader();
        });

    }
    $scope.mostrarDetConsultaEmail = function (ConsEmail) {

        showAjaxLoader();

        var url = "/Cliente/BuscarDetConsultaEmail";
        $http({
            url: url,
            method: "post",
            data: { _asn_id: ConsEmail.codigo }
        }).success(function (retorno) {

            hideAjaxLoader();

            if (retorno.success) {

                $scope.detconsultas = retorno.result.detconsultas;

                angular.forEach($scope.detconsultas, function (obj, index) {
                    obj.HAE_DTCADASTRO = $scope.dataHoraFormatada(obj.HAE_DTCADASTRO);
                });

                angular.element("#modal-det-consultas-email").modal();
            }
            else {

                alert(response.message.message);
            }

        }).error(function (response) {

            alert(response);

            hideAjaxLoader();
        });

    }
    $scope.preparaTela = function () {

        var now = new Date();
        $scope.filtro.dtinicial = new Date(now.getFullYear(), now.getMonth(), 1);
        $scope.filtro.dtfinal = new Date(now.getFullYear(), now.getMonth(), now.getDate());
    }
    $scope.listar = function (pageRequest) {

        showAjaxLoader();

        var url = "/URA/ListarAssinaturaConsultas";
        $http({
            url: url,
            method: "post",
            data: { _asn_id: $scope.filtro.assinatura, _dtini: $scope.filtro.dtinicial, _dtfim: $scope.filtro.dtfinal, pagina: pageRequest }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.listaQtdeConsultas = response.result.listaQtdeConsultas;

                $scope.page = response.page;

            }
            else {

                alert(response.message.message);
            }

        }).error(function (response) {

            alert(response);

            hideAjaxLoader();
        })

    }
    //---------------------------------------------------------//
    
    //---------------------------------------------------------// 
    $scope.atualizarURA = function () {

        if ($scope.updateura.ativo == null) {
            alert("Situação não informada.");
            return
        }

        if ($scope.updateura.qtde == null || $scope.updateura.qtde == 0) {
            alert("Quantidade informada é inválida");
            return
        }

        if (confirm("Estas informações serão atualizadas na URA. Confirma ?")) {

            showAjaxLoader();

            var url = "/URA/AtualizarUra";
            $http({
                url: url,
                method: "post",
                data: { _assinatura: $scope.updateura.assinatura, _qtde: $scope.updateura.qtde, _ativo: $scope.updateura.ativo }
            }).success(function (response) {

                hideAjaxLoader();

                if (response.success == true) {
                    alert(response.message.message);
                }
                else {
                    $scope.message = Util.createMessage("fail", response.message.message);
                }

            }).error(function (response) {

                $scope.message = Util.createMessage("fail", response);

                hideAjaxLoader();
            })

        }

    }
    $scope.abrirModalAtualizarURA = function (ConsEmail) {

        $scope.updateura = {};
        $scope.updateura.ativo = true;
        $scope.updateura.qtde = ConsEmail.contratadas;
        if (ConsEmail.contratadas > 0)
            $scope.updateura.qtde3x = (ConsEmail.contratadas * 3);
        else
            $scope.updateura.qtde3x = 0;

        $scope.updateura.assinatura = ConsEmail.codigo;

        angular.element("#Modal-Atualizar-URA").modal();

    }
    $scope.multiplicaqtdeURA = function () {

        if ($scope.updateura.qtde != null && $scope.updateura.qtde > 0)
            $scope.updateura.qtde3x = ($scope.updateura.qtde * 3);
        else
            $scope.updateura.qtde3x = 0;

    }
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
    $scope.listarClientes = function (pageRequest) {

        showAjaxLoader();

        $http({
            url: "/URA/ListarClientesURA",
            method: "Post",
            dataType: 'json',
            data: { _ura_id: $scope.filtro.URA_ID, _asn_id: $scope.filtro.ASN_NUM_ASSINATURA, _telefone: $scope.filtro.telefone, pagina: pageRequest }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {
                $scope.listaClientes = response.result.listaClientes;
                $scope.page = response.page;
            }
            else {
                alert(response.message.message);
            }

        }).error(function (response) {

            alert(response);

            hideAjaxLoader();

        });


    };
    //--------------------------------------------------------// 

    
    //---- Funções para buscar clientes com acesso a URA ---// 

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
    //----------------------------------------------------//

    //---- Funções para editar ura_produto -----//
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
        $scope.editarUraConfig(novo, "I");
        //----

    };
    $scope.apagaitem = function (item) {
        
        //---- Apaga Item no banco de Dados
        $scope.editarUraConfig(item, "E");
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

        if ($scope.listaProdAreas == null)
            $scope.listaProdAreas = [];


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
        novo.UF_SIGLA_ACESSO = $scope.filtro.UF_SIGLA_ACESSO;
        novo.ACO_ID = item.ACO_ID;
        novo.AREA_CONSULTORIA = AREA_CONSULTORIA;
        novo.UPA_ATIVO = true;
      
      

        
        if (item && existe == false) {
            if ($scope.listaProdAreas.indexOf(novo) === -1) {
                $scope.listaProdAreas.push(novo);
            }
        }

        //---- Gravar Item no banco de Dados
        $scope.incluirArea(novo);
        //----

    };
    $scope.apagaarea = function (item) {

        //---- Apaga Item no banco de Dados
        $scope.excluirArea(item);
        //----

        var ind = $scope.listaProdAreas.indexOf(item);

        if (ind !== -1) {
            $scope.listaProdAreas.splice(ind, 1);
        }

    };
    $scope.incluirArea = function (item) {
    
        showAjaxLoader();

        $http({
            url: "/URA/SalvarProdArea",
            method: "Post",
            dataType: 'json',
            data: { _uraProdutoArea: item }
        }).success(function (response) {

            if (response.success != true) {
                alert(response.message.message);
            }
        })

        hideAjaxLoader();

    }
    $scope.excluirArea = function (item) {

        showAjaxLoader();

        $http({
            url: "/URA/ExcluirProdArea",
            method: "Post",
            dataType: 'json',
            data: { _uraProdutoArea: item }
        }).success(function (response) {

            if (response.success != true) {
                alert(response.message.message);
            }
        })

        hideAjaxLoader();

    }

    $scope.editarUraConfig = function (item, tipo) {

        showAjaxLoader();

        $http({
            url: "/URA/EditarUraConfig",
            method: "Post",
            dataType: 'json',
            data: { _uraconfig: item, _tipo: tipo }
        }).success(function (response) {

            if (response.success != true) {
                alert(response.message.message);
            }
        })

        hideAjaxLoader();

    }
    $scope.editarUraProduto = function (item, tipo) {

        showAjaxLoader();

        $http({
            url: "/URA/EditarUraProduto",
            method: "Post",
            dataType: 'json',
            data: { _uraproduto: item, _tipo: tipo }
        }).success(function (response) {

            if (response.success != true) {
                alert(response.message.message);
            }
        })

        hideAjaxLoader();
    }


    $scope.incluirUf = function (item) {

        $scope.filtro.UF_SIGLA_ACESSO = item.UF_SIGLA_ACESSO;

        showAjaxLoader();

        $http({
            url: "/URA/SalvarProdArea",
            method: "Post",
            dataType: 'json',
            data: { _uraProdutoArea: item }
        }).success(function (response) {

            if (response.success != true) {
                alert(response.message.message);
            }
        })

        hideAjaxLoader();

    }

    //-------------------------------------------//

    $scope.dataHoraFormatada = function (dataHora) {

        if (dataHora == null)
            return "";

        var parseDate = new Date(parseInt(dataHora.substr(6)));
        var jsDate = new Date(parseDate);

        var data = jsDate;
        var dia = data.getDate();
        if (dia.toString().length == 1)
            dia = "0" + dia;
        var mes = data.getMonth() + 1;
        if (mes.toString().length == 1)
            mes = "0" + mes;
        var ano = data.getFullYear();
        var h = data.getHours();
        var m = data.getMinutes();
        var s = data.getSeconds();

        var hora = h + ':' + m + ':' + s;

        return dia + "/" + mes + "/" + ano + " " + hora;
    }

});