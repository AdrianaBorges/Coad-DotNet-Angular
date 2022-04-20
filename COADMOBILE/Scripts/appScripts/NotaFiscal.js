
appModule.controller("NotaFiscalControler", function ($scope, $http, formHandlerService) {
   
    $scope.nf = {};
    $scope.filtro = {};
    $scope.nf.NOTA_FISCAL_ITEM = [];

    $scope.VisualizarNf = function (item) {

        var parametros = "_nf_numero=" + item.NF_NUMERO +"&"+
                         "_nf_serie=" + item.NF_SERIE +"&"+
                         "_nf_tipo=" + item.NF_TIPO 

        location.href = "/NotaFiscal/Editar?"+parametros;

    };
    $scope.BuscarFornecedor = function () {
        
        $http({
            url: "/NotaFiscal/BuscarFornecedor",
            method: "Post",
            dataType: 'json',
            data: { _for_cnpj: $scope.nf.FORNECEDOR.FOR_CNPJ }
        }).success(function (response) {
            $scope.nf.FORNECEDOR = response;
            $scope.nf.FOR_ID = $scope.nf.FORNECEDOR.FOR_ID;
            $scope.nf.FORNECEDOR.MUN_ID = response.MUN_ID;
            $scope.nf.FORNECEDOR.MUNICIPIO.MUN_DESCRICAO = response.MUNICIPIO.MUN_DESCRICAO;
        })

    };
    $scope.BuscarTransportador = function () {

        var _data = { _tra_cnpj: $scope.nf.TRANSPORTADOR.TRA_CNPJ }


        $http({
            url: "/NotaFiscal/BuscarTransportador",
            method: "Post",
            dataType: 'json',
            data: _data
        }).success(function (response) {
            $scope.nf.TRANSPORTADOR = response;
            $scope.nf.TRA_ID = $scope.nf.TRANSPORTADOR.TRA_ID;
            $scope.nf.TRANSPORTADOR.MUN_ID = response.MUN_ID;
            $scope.nf.TRANSPORTADOR.MUNICIPIO.MUN_DESCRICAO = response.MUNICIPIO.MUN_DESCRICAO;
        })

    };
    $scope.AbrejanelaProduto = function (_index) {

        $scope.indexretorno = _index;
        $('#pesquisaProduto').modal('show');
    
    };
    $scope.BuscarProduto = function (_pro_descricao) {

        var _data = { _pro_nome: _pro_descricao }

        $http({
            url: "/NotaFiscal/BuscarProduto",
            method: "Post",
            dataType: 'json',
            data: _data
        }).success(function (response) {

            if (response.length > 0) {
                $scope.mostraconsulta = true;
                $scope.dbproduto = response;
            }
        })

    };
    $scope.MostrarProduto = function (_produto_id, _index) {

        var _data = { _pro_id: _produto_id }

        $http({
            url: "/NotaFiscal/MostrarProduto",
            method: "Post",
            dataType: 'json',
            data: _data
        }).success(function (response) {

            if (response != null) {

                _produto = response;

                if (_produto.PRO_ID != "")
                    $scope.nf.NOTA_FISCAL_ITEM[_index].PRO_ID = _produto.PRO_ID;

                if (_produto.PRO_UN_COMPRA != "")
                    $scope.nf.NOTA_FISCAL_ITEM[_index].NFI_UN = _produto.PRO_UN_COMPRA;

                if (_produto.NCM_ID != "")
                    $scope.nf.NOTA_FISCAL_ITEM[_index].NCM_ID = _produto.NCM_ID;

                if (_produto.PRO_NOME != "")
                    $scope.nf.NOTA_FISCAL_ITEM[_index].NFI_PRO_NOME = _produto.PRO_NOME;
            }
        })

    };
    $scope.BuscarCfopEntrada = function (_item) {

        var _data = { _cfopsaida: $scope.nf.CFOP }

        $http({
            url: "/NotaFiscal/BuscarCfopEntrada",
            method: "Post",
            dataType: 'json',
            data: _data
        }).success(function (response) {

            if (response.CFOP != "")
                $scope.nf.CFOPENT = response.CFOP;

        })

    };
    $scope.ConsultarNotas = function () {
        
        showAjaxLoader();

        var _data = { _mesatual: $scope.filtro.mesatual, _anoatual: $scope.filtro.anoatual, _emp_id: $scope.filtro.emp_id, _nf_tipo: $scope.filtro.nf_tipo }
        var date = "";
        var formattedDate = "";
        
        $http({
            url: "/NotaFiscal/BuscarNotaFiscal",
            method: "Post",
            dataType: 'json',
            data: _data
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success) {

                $scope.page = response.page;

                var obj = response.result.ListaNotaFiscal;
                for (var ind in obj) {

                    if (obj[ind].NF_DATA_EMISSAO != null)
                        obj[ind].NF_DATA_EMISSAO = dataAtualFormatada(obj[ind].NF_DATA_EMISSAO);

                    if (obj[ind].NF_DATA_ENTRADA != null)
                        obj[ind].NF_DATA_ENTRADA = dataAtualFormatada(obj[ind].NF_DATA_ENTRADA);

                    if (obj[ind].NF_DATA_SAIDA != null)
                        obj[ind].NF_DATA_SAIDA = dataAtualFormatada(obj[ind].NF_DATA_SAIDA);
                }

                $scope.listanf = obj;
            }
            else {

                $scope.listanf = null;
                Util.alertMessage(response.message);
            }
        
        
        }).error(function () {
          
            hideAjaxLoader();
        })

    };
    $scope.BurcarNf = function (_numero,_serie,_tipo) {

        showAjaxLoader();

        var _data = { _nf_numero: _numero, _nf_serie: _serie, _nf_tipo : _tipo }
        var date = "";
        var formattedDate = "";

        $http({
            url: "/NotaFiscal/BuscarNotas",
            method: "Post",
            dataType: 'json',
            data: _data
        }).success(function (response) {

            hideAjaxLoader();

            if (response.Success == null)
            {
                response.NF_DATA_EMISSAO = dataAtualFormatada(response.NF_DATA_EMISSAO);
                $scope.dtemissao = response.NF_DATA_EMISSAO;

                if (response.NF_DATA_ENTRADA != null) {
                    $scope.mostrafornecedor = true;
                    $scope.mostracliente = false;
                    $scope.lblEntSai = "Data Entrada";
                    response.NF_DATA_ENTRADA = dataAtualFormatada(response.NF_DATA_ENTRADA);
                    $scope.dtentrada = response.NF_DATA_ENTRADA;

               }
                else {
                    $scope.mostrafornecedor = false;
                    $scope.mostracliente = true;
                    $scope.lblEntSai = "Data Saida";
                    response.NF_DATA_SAIDA = dataAtualFormatada(response.NF_DATA_SAIDA);
                    $scope.dtsaida = response.NF_DATA_SAIDA;
               }
                             
               $scope.nf = response;
               $scope.mostrarImpostos($scope.nf.NF_TIPO);
            }
            else
               alert(response.Message);

        
        }).error(function (response) {

            hideAjaxLoader();

            alert(response);
        })

    };
    $scope.Confirmar = function () {

        showAjaxLoader();

        var parametros = "_nf_numero=" + $scope.nf.NF_NUMERO + "&" +
                         "_nf_serie=" + $scope.nf.NF_SERIE + "&" +
                         "_nf_tipo=" + $scope.nf.NF_TIPO;

        $http({
            url: "/NotaFiscal/Novo",
            method: "Post",
            dataType: 'json',
            data: JSON.stringify($scope.nf)
        }).success(function (response) {

            hideAjaxLoader();

            if (response.Success) {
                alert(response.Message);
                location.href = "/NotaFiscal/Visualizar?" + parametros;
            }
            else
               alert(response.Message);

        })

    };
    $scope.ConfirmarAlteracao = function () {

        showAjaxLoader();

        var parametros = "_nf_numero=" + $scope.nf.NF_NUMERO + "&" +
                         "_nf_serie=" + $scope.nf.NF_SERIE + "&" +
                         "_nf_tipo=" + $scope.nf.NF_TIPO;

        if ($scope.nf.NF_STATUS != "ATI") {
            
            if ($scope.resolverPendencias)
                $scope.nf.NF_STATUS = "ATI";
            else
                $scope.nf.NF_STATUS = "PEN";
        }

        $http({
            url: "/NotaFiscal/EditarConfirma",
            method: "Post",
            dataType: 'json',
            data: JSON.stringify($scope.nf)
        }).success(function (response) {

            hideAjaxLoader();

            if (response.Success) {
                alert(response.Message);
                location.href = "/NotaFiscal/Visualizar?" + parametros;
            }
            else
                alert(response.Message);

        }).error(function (data) {

            hideAjaxLoader();

            alert("Erro ao atualizar o registro."); 
        })

    };
    $scope.additem = function () {

        var count = $scope.nf.NOTA_FISCAL_ITEM.length;

        if (count > 0) {
            var obj = $scope.nf.NOTA_FISCAL_ITEM[count - 1];
            if (obj.NFI_PRO_NOME != "" && obj.NFI_PRO_NOME != null) {

                $scope.nf.NOTA_FISCAL_ITEM.push({});
            }
        }
        else { $scope.nf.NOTA_FISCAL_ITEM.push({}); }

    };
    $scope.removeitem = function (index) {

        var count = $scope.nf.NOTA_FISCAL_ITEM.length;

        if (count > 0) {
            $scope.nf.NOTA_FISCAL_ITEM.splice(index, 1)
        }
 
    };
    $scope.pesquisarMunicipio = function (selecionado) {
      
        // Se a pesquisa for vazia
        if (selecionado.length < 3) {
            $scope.completing = false;
        } else {

            $http({
                method: "Post",
                dataType: "json",
                url: "/Municipio/BuscarMunicipio",
                data: { _nomemunicipio: selecionado }
            }).success(function (response) {

                $scope.completing = true;
                $scope.dbmunicipio = response;

            });
        }
    }
    $scope.seleciona = function (retorno,seleciona) {

        $scope.completing = false;
        $scope.nf.FORNECEDOR.MUN_ID = retorno.MUN_ID;
        document.getElementById(seleciona).value = retorno.MUN_DESCRICAO;

    }
    $scope.pesquisarMunicipio2 = function (selecionado) {

        // Se a pesquisa for vazia
        if (selecionado.length < 3) {
            $scope.completing = false;
        } else {

            $http({
                method: "Post",
                dataType: "json",
                url: "/Municipio/BuscarMunicipio",
                data: { _nomemunicipio: selecionado }
            }).success(function (response) {

                $scope.completing2 = true;
                $scope.dbmunicipio = response;

            });
        }
    }
    $scope.seleciona2 = function (retorno, seleciona) {

        $scope.completing2 = false;
        $scope.nf.TRANSPORTADOR.MUN_ID = retorno.MUN_ID;
        document.getElementById(seleciona).value = retorno.MUN_DESCRICAO;

    }
    $scope.fechaJanelaProduto = function (_produto) {
         
        var _index = $scope.indexretorno;
     
        if (_produto.PRO_ID != "")
            $scope.nf.NOTA_FISCAL_ITEM[_index].PRO_ID = _produto.PRO_ID;

        if (_produto.PRO_UN_COMPRA != "")
            $scope.nf.NOTA_FISCAL_ITEM[_index].NFI_UN = _produto.PRO_UN_COMPRA;

        if (_produto.NCM_ID != "")
            $scope.nf.NOTA_FISCAL_ITEM[_index].NCM_ID = _produto.NCM_ID;

        if (_produto.PRO_NOME != "")
            $scope.nf.NOTA_FISCAL_ITEM[_index].NFI_PRO_NOME = _produto.PRO_NOME;

    } 
    function dataAtualFormatada(dataHora) {

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
        return dia + "/" + mes + "/" + ano;

    }
    $scope.dataInicial = function () {

        this.motraLabel();

        var data = new Date();
        
        var dia = data.getDate();
        if (dia.toString().length == 1)
            dia = "0" + dia;
        var mes = data.getMonth() + 1;
        if (mes.toString().length == 1)
            mes = "0" + mes;
        var ano = data.getFullYear();
        
        $scope.strdtini = "01" + "/" + mes + "/" + ano;
        $scope.strdtfim = dia + "/" + mes + "/" + ano;

    }
    $scope.ExcluirNotaFiscal = function () {

        if (confirm("Deseja excluir este registro?")) {

            $http({
                url: "/NotaFiscal/Excluir",
                method: "Post",
                dataType: 'json',
                data: JSON.stringify($scope.nf)
            }).success(function (response) {

                alert(response.Message);

                location.href = "/NotaFiscal/Pesquisar";

            })
        };

    };
    $scope.mostrarImpostos = function (selecionado) {

        if (selecionado > 1 ) {
            $scope.mostraservico = true;
            $scope.mostra = false;
        }
        else {
            $scope.mostraservico = false;
            $scope.mostra = true;
        }

    }
    $scope.motraLabel = function () {

        if ($scope.nf_tipo == 1  || $scope.nf_tipo == 3)
            $scope.lblentrada = "Período (Saída)";
        else
            $scope.lblentrada = "Período (Entrada)";
    };
    $scope.mostrarData = function (item) {


        if (item.NF_TIPO == 1 || item.NF_TIPO == 3) {
            $scope.lblDataEntSai = "Saida";
            $scope.DataEntSai = item.NF_DATA_SAIDA;
            if (item.CLIENTES != null)
                $scope.lblNome = item.CLIENTES.CLI_NOME;
            else
                $scope.lblNome = "";

            $scope.lblRazaoSocial = "Cliente";
        }
        else {
            $scope.lblDataEntSai = "Entrada";
            $scope.DataEntSai = item.NF_DATA_ENTRADA;
            if (item.Fornecedor != null)
                $scope.lblNome = item.Fornecedor.FOR_RAZAO_SOCIAL;
            else
                $scope.lblNome = "";

            $scope.lblRazaoSocial = "Fornecedor";
        }

        return true;

    }
    $scope.mostrarNomeCliente = function (item) {

     
        if (item.FOR_RAZAO_SOCIAL == "" || item.FOR_RAZAO_SOCIAL == null)
            item.FOR_RAZAO_SOCIAL = item.CLI_NOME;

    }
    $scope.listar = function (pageRequest) {

        showAjaxLoader();

        var url = Util.getUrl("/NotaFiscal/BuscarNotaFiscal");

        if (pageRequest) {
            url += "?numpagina=" + pageRequest;
        }

        var config = {
            url: url,
            targetObjectName: 'listanf',
            responseModelName: 'ListaNotaFiscal',
            pageConfig: { pageName: 'page' },
            success: function () {

                hideAjaxLoader();

                var obj = $scope.listanf;

                for (var ind in obj) {

                    if (obj[ind].NF_DATA_EMISSAO != null)
                        obj[ind].NF_DATA_EMISSAO = dataAtualFormatada(obj[ind].NF_DATA_EMISSAO);

                    if (obj[ind].NF_DATA_ENTRADA != null)
                        obj[ind].NF_DATA_ENTRADA = dataAtualFormatada(obj[ind].NF_DATA_ENTRADA);

                    if (obj[ind].NF_DATA_SAIDA != null)
                        obj[ind].NF_DATA_SAIDA = dataAtualFormatada(obj[ind].NF_DATA_SAIDA);
                }


            }
        };
        
        var _data = { _mesatual: $scope.filtro.mesatual, _anoatual: $scope.filtro.anoatual, _emp_id: $scope.filtro.emp_id, _nf_tipo: $scope.filtro.nf_tipo }

        if ($scope.filtro) {

            config.data = _data;
        }

        formHandlerService.read($scope, config);
    };
    $scope.PreparaTela = function () {

        var data = new Date();

        if ($scope.filtro.mesatual == null)
            $scope.filtro.mesatual = data.getMonth() + 1;

        if ($scope.filtro.anoatual == null)
            $scope.filtro.anoatual = data.getFullYear();

        if ($scope.filtro.emp_id == null)
            $scope.filtro.emp_id = 1;

    };
  

});

