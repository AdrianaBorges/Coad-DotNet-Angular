
appModule.controller("NotaFiscalControler", ['$scope', '$http', 'formHandlerService', 'conversionService', '$timeout', '$window',
    function ($scope, $http, formHandlerService, conversionService, $timeout, $window) {
   
    $scope.nf = {};
    $scope.nf.NOTA_FISCAL_ITEM = [];
    $scope.VisualizarNf = function (item) {

        var parametros = "_nf_id=" + item.NF_ID 

        location.href = "/NotaFiscal/Editar?" + parametros;

    };


    $scope.carregarEmpresas = function () {

        var url = Util.getUrl("/empresa/listarEmpresas");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstEmpresas',
            responseModelName: 'lstEmpresas',
            showAjaxLoader: true,

        });
    }

    $scope.initNfAvulsa = function (nfId) {

        $scope.carregarEmpresas();

        if (nfId) {
            
            var url = Util.getUrl("/notaFiscal/RecuperarDadosDaNotaFiscal");

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'nf',
                responseModelName: 'nf',
                showAjaxLoader: true,
                data: { nfId: nfId },
                success: function () {
                    $scope.obterDadosDoCliente();
                }
            });
        }
        else {

            $scope.nf = {

                NF_TIPO: 3,
                NOTA_FISCAL_ITEM: [],
                NF_AVULSA: true,
            };
        }
    }

    $scope.expPlanilha = function (_url) {

        showAjaxLoader2();

        $scope.export = {};
      
        if (_url == null)
            _url = '/NotaFiscal/ExportarXLS';

        $http({
            method: 'Post',
            //dataType: 'json',
            url: _url,
            data: {
                _nfnumero: $scope.filtro.NF_NUMERO,
                _mesatual: $scope.filtro.mesatual,
                _anoatual: $scope.filtro.anoatual,
                _emp_id: $scope.filtro.emp_id,
                _nf_tipo: $scope.filtro.nf_tipo
            }

        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {
                $scope.export.lnkPath = response.result.retorno;
                $scope.message = Util.createMessage("success", "Operação realizada com sucesso!!");
            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);

            hideAjaxLoader2();
        });

    }
    $scope.listarRelatorio = function (pageRequest) {

        showAjaxLoader2();

        var url = "/NotaFiscal/ListarNotasPeriodo";
        $http({
            url: url,
            method: "post",
            data: {
                _mesatual: $scope.filtro.mesatual,
                _anoatual: $scope.filtro.anoatual,
                _emp_id: $scope.filtro.emp_id,
                _nf_tipo: $scope.filtro.nf_tipo,
                numpagina: pageRequest
            }
        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success) {

                $scope.page = response.page;
                $scope.listaNotaFiscalSintetico = response.result.listaNotaFiscalSintetico;
                $scope.listaNotaFiscal = response.result.listaNotaFiscal;

            }
            else {

                $scope.listaNotaFiscal = null;

                alert(response.message.message);
            }

        }).error(function (response) {

            alert(response);

            hideAjaxLoader2();
        })

    }
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

            if (response.success) {

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

        var _data = {
            _nfnumero: $scope.filtro.NF_NUMERO,
            _cpfCnpj: $scope.filtro.CPF_CNPJ,
            _mesatual: $scope.filtro.mesatual,
            _anoatual: $scope.filtro.anoatual,
            _emp_id: $scope.filtro.emp_id,
            _nf_tipo: $scope.filtro.nf_tipo,
            antecipada : $scope.filtro.antecipada
        }
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

                $scope.listanf = response.result.ListaNotaFiscal;

                $scope.message = response.message;
            }
            else {

                $scope.listanf = null;

                if (response.message != null)
                    $scope.message = response.message;
                else
                    $scope.message = Util.createMessage("fail", response);
            }
        
        
        }).error(function () {
          
            hideAjaxLoader();
        })

    };
    $scope.BurcarNf = function (nf_id) {

        showAjaxLoader();

        var _data = { _nf_id: nf_id}
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
                       
               conversionService.deepConversion(response);
               $scope.nf = response;
               $scope.mostrarImpostos($scope.nf.NF_TIPO);
                
               angular.element("#selDadosAdicionais").trigger("click");

               angular.element("#selTransportador").trigger("click");

            }
            else
               alert(response.Message);

        
        }).error(function (response) {

            hideAjaxLoader();

            alert(response);
        })

    };

    $scope.buscarEmailNotaFiscal = function (_ctr) {

        showAjaxLoader2();

        var url = "/Cliente/BuscarEmailsNF";
        $http({
            url: url,
            method: "post",
            data: {
                _contrato: _ctr.CTR_NUM_CONTRATO
                   ,_cli_id: _ctr.CLI_ID
            }
        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {

                $scope.listaemail = response.result.listaemail;
                conversionService.deepConversion($scope.listaemail);

            }
            else {

                $scope.message = response.message;
            }

        }).error(function (response) {

            $scope.message = response.message;

            hideAjaxLoader2();
        })

    }

    $scope.abrirModalEnviarNotaFiscal = function (_item) {

        $scope.nfenviar = _item;
        conversionService.deepConversion($scope.nfenviar);

        $scope.buscarEmailNotaFiscal($scope.nfenviar);

        $('#modal-enviar-nota').modal('show');

    };

    $scope.gerarDanfe = function (nfid) {

     //   showAjaxLoader();

        if (nfid) {
            var url = Util.getUrl("/NotaFiscal/GerarDanfe?id=" + nfid);
            post(url, true);
        }


        //$http({
        //    url: "/NotaFiscal/GerarDanfe",
        //    method: "Post",
        //    dataType: 'json',
        //    data: { _nf_id: nfid }
        //}).success(function (response) {

        //    hideAjaxLoader();

        //    if (!response.success) {

        //        if (response.message != null)
        //            $scope.message = response.message;
        //        else
        //            $scope.message = Util.createMessage("fail", response);
        //    }


        //}).error(function () {

        //    hideAjaxLoader();
        //})

    };

    $scope.enviarNotaFiscal = function () {

        showAjaxLoader();

        var _data = {
            _notafiscal: $scope.nfenviar
        }

        $http({
            url: "/NotaFiscal/EnviarNotaFiscal",
            method: "Post",
            dataType: 'json',
            data: _data
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success) {

                $('#modal-enviar-nota').modal('hide');

                $scope.message = response.message;

                $scope.listar($scope.page.pagina);
            }
            else {
  
                if (response.message != null)
                    $scope.message = response.message;
                else
                    $scope.message = Util.createMessage("fail", response);
           }


        }).error(function () {

            hideAjaxLoader();
        })

    };

    $scope.Confirmar = function () {

        showAjaxLoader();

        conversionService.deepConversion($scope.nf);
        
        $http({
            url: "/NotaFiscal/Salvar",
            method: "Post",
            dataType: 'json',
            data: { _nota_fiscal: $scope.nf}
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success) {
                if (confirm(response.message.message + ". Deseja incluir uma nova nota fiscal?"))
                    location.href = "/NotaFiscal/Editar";
                else
                    location.href = "/NotaFiscal/Index";
            }
            else
                $scope.message = response.message;

      
            hideAjaxLoader();


        }).error(function (response) {

            hideAjaxLoader();

            $scope.message = Util.createMessage("fail", response.message);
      
        })

    };
    $scope.ConfirmarAlteracao = function () {
            
        showAjaxLoader();

        var parametros = "_nf_id=" + $scope.nf_Id
                     

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
                location.href = "/NotaFiscal/Pesquisar";
            }
            else
                alert(response.Message);

        }).error(function (data) {

            hideAjaxLoader();

            alert("Erro ao atualizar o registro."); 
        })

    };
    $scope.additem = function () {

        if ($scope.nf.NOTA_FISCAL_ITEM == null)
            $scope.nf.NOTA_FISCAL_ITEM = [];

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

        showAjaxLoader();

        if (confirm("Deseja excluir este registro?")) {

            $http({
                url: "/NotaFiscal/Excluir",
                method: "Post",
                dataType: 'json',
                data: JSON.stringify($scope.nf)
            }).success(function (response) {

                if (response.success) {
                    location.href = "/NotaFiscal/Index";
                }
                else
                    $scope.message = Util.createMessage("fail", response.message.message);

                hideAjaxLoader();

            }).error(function (response) {

                hideAjaxLoader();

                $scope.message = Util.createMessage("fail", response.message.message);

            });
        };

        hideAjaxLoader();

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

    $scope.mostrarNomeCliente = function (item) {

     
        if (item.FOR_RAZAO_SOCIAL == "" || item.FOR_RAZAO_SOCIAL == null)
            item.FOR_RAZAO_SOCIAL = item.CLI_NOME;

    }
    $scope.listar = function (pageRequest) {

        showAjaxLoader2();
        
        if ($scope.filtro == null)
            $scope.filtro = {}

        var url = "/NotaFiscal/BuscarNotaFiscal";

        $http({
            url: url,
            method: "post",
            data: { _nfnumero: $scope.filtro.NF_NUMERO
                  , _cpfCnpj: $scope.filtro.CPF_CNPJ  
                  , _mesatual: $scope.filtro.mesatual
                  , _anoatual: $scope.filtro.anoatual
                  , _emp_id: $scope.filtro.emp_id
                  , _nf_tipo: $scope.filtro.nf_tipo
                  , antecipada: $scope.filtro.antecipada
                  , avulsa: $scope.filtro.avulsa
                  , numpagina: pageRequest
            }

        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success == true) {

                $scope.listanf = response.result.listanf;
                conversionService.deepConversion($scope.listanf);

                $scope.page = response.page;

            }
            else {

                if (response.message != null)
                    $scope.message = response.message;
                else
                    $scope.message = Util.createMessage("fail", response);

                $scope.lstretorno = null;

                hideAjaxLoader2();
            }

        }).error(function (response) {

            alert(message);

            $scope.message = Util.createMessage("fail", response);
            $scope.lstretorno = null;

            hideAjaxLoader2();
        });


     
    };
    $scope.PreparaTela = function () {

        var data = new Date();

        $scope.filtro = {}
        $scope.filtro.NF_NUMERO = null;
        $scope.filtro.CPF_CNPJ  = null;
        $scope.filtro.mesatual  = data.getMonth() + 1;
        $scope.filtro.anoatual  = data.getFullYear();
        $scope.filtro.emp_id    = 2;
        $scope.filtro.nf_tipo   = 1;

        $scope.criarFiltros()
        $scope.listarNotaFiscalTipo();
    };

    $scope.cancelarNotaFiscal = function (nfID) {

        if (nfID && confirm("Deseja realmente cancelar a Nota Fiscal ?")) {

            $scope.requisicaoCan = { nfID: nfID };
            formHandlerService.submit($scope, {
                url: Util.getUrl("/notafiscal/CancelarNotaFiscal"),
                objectName: 'requisicaoCan',
                showAjaxLoader: true,
                success: function (resp, status, config, message, validationMessage) {

                    $scope.message = message;
                    if (resp.success) {

                        $scope.message = Util.createMessage("success", "Notas Fiscal Cancelada com Sucesso!");
                        $scope.listar();
                        $timeout(function () {
                            $scope.message = null;

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

    $scope.gerarDevolucaoNotaFiscal = function (nfID) {

        if (nfID && confirm("Deseja realmente devolver a Nota Fiscal ?")) {

            $scope.requisicaoCan = { nfID: nfID };
            formHandlerService.submit($scope, {
                url: Util.getUrl("/notafiscal/gerarDevolucaoNotaFiscal"),
                objectName: 'requisicaoCan',
                showAjaxLoader: true,
                success: function (resp, status, config, message, validationMessage) {

                    $scope.message = message;
                    if (resp.success) {

                        $scope.message = Util.createMessage("success", "Nota Fiscal agendada para devolução com sucesso!!");
                        $scope.listar();
                        $timeout(function () {
                            $scope.message = null;

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

    $scope.abrirModalCartaCorrecao = function (nfID) {

        $scope.cartaCorrecaoModal = { NotaFiscalID: nfID };
        angular.element("#modal-carta-correcao").modal();
    }

    $scope.gerarCartaDeCorrecao = function () {

        if (confirm("Deseja realmente enviar a carta de correção a Nota Fiscal ?")) {

            formHandlerService.submit($scope, {
                url: Util.getUrl("/notafiscal/gerarCartaCorrecao"),
                objectName: 'cartaCorrecaoModal',
                showAjaxLoader: true,
                success: function (resp, status, config, message, validationMessage) {

                    $scope.message = message;
                    $scope.buttonGerar = 'reset';
                    if (resp.success) {

                        $scope.message = Util.createMessage("success", "A carta de correção foi anexada a Nota Fiscal com sucesso!!");
                        angular.element("#modal-carta-correcao").modal('hide');
                        $scope.listar();
                        $timeout(function () {
                            $scope.message = null;

                        }, 1000);
                    }
                },
                fail: function () {

                    $scope.buttonGerar = 'reset';
                }
            });
        }
        else {
            return false;
        }
    }


    $scope.abrirModalEventosNFe = function (nfID) {

        $scope.lstEvento = null;
        $scope.evento = null;

        if (nfID) {

            $scope.nfID = nfID;
            angular.element("#modal-eventos").modal();
            $scope.listarEventosDaNFe();
        }
    }

    $scope.listarEventosDaNFe = function (pagina) {

        $scope.listado = false;
        var url = Util.getUrl("/notafiscal/ListarEventosNotaFiscal");

        if (pagina) {

            url += "?pagina=" + pagina;
        }

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstEventos',
            responseModelName: 'lstEventos',
            data: { nfID: $scope.nfID },
            showAjaxLoader: true,
            pageConfig: { pageName: 'page' , pageTargetName: 'paginaEventos' },
            success: function (resp) {
                $scope.listado = true;
            }
        });
    }

    $scope.listarItensDeLote = function (pagina) {

        $scope.listado = false;
        var url = Util.getUrl("/notafiscal/ListarItensDeLote");

        if (pagina) {

            url += "?pagina=" + pagina;
        }

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstLoteItem',
            responseModelName: 'lstLoteItem',
            data: { nfID: $scope.nfID },
            showAjaxLoader: true,
            //pageConfig: { pageName: 'page', pageTargetName: 'paginaEventos' },
            success: function (resp) {
                $scope.listado = true;
            }
        });
    }


    $scope.listarItensDeLoteService = function (pagina) {

        $scope.listado = false;
        var url = Util.getUrl("/notafiscal/ListarItensDeLoteService");

        if (pagina) {

            url += "?pagina=" + pagina;
        }

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstLoteItemServico',
            responseModelName: 'lstLoteItemService',
            data: { nfID: $scope.nfID },
            showAjaxLoader: true,
            //pageConfig: { pageName: 'page', pageTargetName: 'paginaEventos' },
            success: function (resp) {
                $scope.listado = true;
            }
        });
    }


    $scope.visualizarEvento = function (evento) {

        if (evento) {
            $scope.evento = evento;
        }
    }

    $scope.baixarNotaFiscal = function (nfID) {

        if (nfID) {
            var url = Util.getUrl("/notafiscal/DownloadNFe?nfID=" + nfID);
            post(url, true);
        }
    }

    $scope.baixarNotaFiscalEvento = function (nfEveID) {

        if (nfEveID) {
            var url = Util.getUrl("/notafiscal/DownloadEventoNFe?nfEveID=" + nfEveID);
            post(url, true);
        }
    }

    $scope.abrirModalLoteNFe = function (nfID) {
        
        if (nfID) {

            $scope.nfID = nfID;
            angular.element("#modal-nfe-lote").modal();
            $scope.listarItensDeLote();
            $scope.listarItensDeLoteService();
        }
    }

    $scope.criarFiltros = function () {

        var url = Util.getUrl("/notafiscal/retonarDadosDeFiltro");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'filtros',
            responseModelName: 'filters',
            showAjaxLoader: true,
            success: function (resp) {

            }
        });
    }

    $scope.listarNotaFiscal = function (pagina) {

        $scope.listado = false;
        var url = Util.getUrl("/notafiscal/representantes");

        if (pagina) {

            url += "?pagina=" + pagina;
        }

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'representantes',
            responseModelName: 'representantes',
            data: $scope.filtro,
            showAjaxLoader: true,
            pageConfig: { pageName: 'page' /*, pageTargetName: 'paginaPrioridades' */ },
            success: function (resp) {
                $scope.listado = true;
            }
        });
    }

    $scope.listarNotaFiscalTipo = function () {

        var url = Util.getUrl("/notafiscal/listarNotaFiscalTipo");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstNfTipo',
            responseModelName: 'lstNfTipo',
            showAjaxLoader: true,
            success: function (resp) {
            }
        });
    }

    $scope._carregarDadosDoCliente = function (CLI_ID) {

        $scope.nf.origem = 'cli';
        $scope.nf.INFO_CLIENTE = null;

        if (CLI_ID) {
            var url = Util.getUrl("/franquia/clientes/RecuperarDadosDoCliente");

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'nf.INFO_CLIENTE',
                responseModelName: 'cliente',
                data: { clienteId: CLI_ID },
                showAjaxLoader: true,
                success: function (response) {

                    if (!Util.isPathValid($scope, "nf.INFO_CLIENTE")) {
                        $scope.nf.origem = null;
                    }
                    
                },
                fail: function () {
                    $scope.nf.origem = null;
                }
            });
        }
        else {
            $scope.nf.origem = null;
        }
    }

    $scope.obterDadosDoCliente = function () {

        if (Util.isPathValid($scope, "nf.CLI_ID")) {

            var cliId = $scope.nf.CLI_ID;
            $scope._carregarDadosDoCliente(cliId);
        }
    }

    $scope.abrirModalBuscarCliente = function () {

        $scope.filtro = { pesquisaCpfCnpjPorIqualdade: true, uenLogada: false, exibirFiltroUen: false };
        angular.element("#modal-buscar-cliente").modal();
    }


    $scope.buscarClientes = function (pageRequest) {

        $scope.message = null;
        $scope.listado = false;
        var url = Util.getUrl("/franquia/clientes/BuscarClienteGlobal");

        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }
        else {
            pageRequest = 1;
        }

        var config = {
            url: url,
            targetObjectName: 'clientes',
            responseModelName: 'clientes',
            showAjaxLoader: true,
            pageConfig: { pageName: 'page' },
            success: function (response) {
                $scope.listado = true;

            }
        };

        if ($scope.filtro) {

            $scope.filtro.registroPorPagina = 15;
            $scope.filtro.pagina = pageRequest;
            $scope.filtro.lstClaCliId = [2, 3];

            if ($scope.filtro.cpf_cnpj || $scope.filtro.nome ||
                $scope.filtro.email || $scope.filtro.telefone || $scope.filtro.codigoAssinatura) {

                if ($scope.filtro.dddTelefone && !$scope.filtro.telefone) {

                    $scope.message = Util.createMessage("fail", "Preencha o telefone ao preencher o ddd");
                    return;
                }
                config.data = angular.copy($scope.filtro);
            }
            else {

                $scope.message = Util.createMessage("fail", "Preenche pelo menos um filtro");
                return;
            }


            formHandlerService.read($scope, config);
        }
        else {
            $scope.message = Util.createMessage("fail", "Preenche pelo menos um filtro");

        }
    };

    $scope.selecionarCliente = function (item) {

        if (item && item.CLI_ID) {

            $scope.nf.CLI_ID = item.CLI_ID;
            $scope.obterDadosDoCliente();
        }

        angular.element("#modal-buscar-cliente").modal('hide');
    }


    $scope.dispararAcaoAdicaoProduto = function () {

        angular.element("#modal-produto-composicao").modal();
        
    }


    $scope.listarProdutoComposicao = function (pageRequest) {

        $scope.listado = false;
        var url = Util.getUrl("/curso/ListarCursos");

        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }

        var config = {
            url: url,
            targetObjectName: 'lstProdutoComposicao',
            responseModelName: 'lstCursos',
            showAjaxLoader: true,
            pageConfig: { pageName: 'page' },
            success: function () {

                $scope.listado = true;
            }
        };

        if ($scope.filtro) {

            config.data = angular.copy($scope.filtro);
            config.data.uenId = 1;
        }
        formHandlerService.read($scope, config);
    };


    $scope.adicionarProduto = function (item) {

        if (item != null && Util.isPathValid($scope, "nf")) {
            if ($scope.nf.NOTA_FISCAL_ITEM) {


                var discriminacao = "";

                if ($scope.nf.NOTA_FISCAL_ITEM &&
                    $scope.nf.NOTA_FISCAL_ITEM.length > 0) {

                    discriminacao = $scope.nf.NOTA_FISCAL_ITEM[0].NFI_DISCRIMINACAO_SERVICO;
                    discriminacao = discriminacao.replace($scope.nf.NOTA_FISCAL_ITEM[0].PRODUTO_COMPOSICAO.CMP_DESCRICAO, "");
                }
                var obj = {

                    PRODUTO_COMPOSICAO: item,
                    CMP_ID: item.CMP_ID,
                    NF_NUMERO: 0,
                    NFI_QTDE: 1,
                    NFI_DISCRIMINACAO_SERVICO: item.CMP_DESCRICAO + " " + discriminacao
                };

                if ($scope.nf.NOTA_FISCAL_ITEM.length == 0)
                    $scope.nf.NOTA_FISCAL_ITEM.push(obj);
                else
                    $scope.nf.NOTA_FISCAL_ITEM[0] = obj;

            }
            $scope.nf.NF_VLR_SERVICO = item.CMP_VLR_VENDA;
            
            $scope.calcularDescontoDosImpostos();

            angular.element("#modal-curso").modal('hide');
            angular.element("#modal-produto-composicao").modal('hide');
        }
    }

    $scope.calcularDescontoDosImpostos = function () {

        var url = Util.getUrl("/notafiscal/calcularDescontoDosImpostos");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'notaFiscalResp',
            responseModelName: 'notaFiscal',
            showAjaxLoader: true,
            data: { notaFiscal: $scope.nf },
            success: function (resp) {

                if ($scope.notaFiscalResp && $scope.nf) {

                    $scope.nf.NF_VLR_COFINS = $scope.notaFiscalResp.NF_VLR_COFINS;
                    $scope.nf.NF_VLR_CSLL = $scope.notaFiscalResp.NF_VLR_CSLL;
                    $scope.nf.NF_VLR_INSS = $scope.notaFiscalResp.NF_VLR_INSS;
                    $scope.nf.NF_VLR_IR = $scope.notaFiscalResp.NF_VLR_IR;
                    $scope.nf.NF_VLR_PIS = $scope.notaFiscalResp.NF_VLR_PIS;
                    $scope.nf.NF_VLR_OUTRAS = $scope.notaFiscalResp.NF_VLR_OUTRAS;
                    $scope.nf.NF_VLR_ISS = $scope.notaFiscalResp.NF_VLR_ISS;
                    $scope.nf.NF_VLR_NOTA = $scope.notaFiscalResp.NF_VLR_NOTA;
                }
            }
        });
    }

    $scope.calcularTotais = function () {

        if ($scope.nf) {

            if ($scope.totalNfPromisse) {

                $timeout.cancel($scope.totalNfPromisse);
            }

            $scope.totalNfPromisse = $timeout(function () {

                var valorBruto = $scope.nf.NF_VLR_SERVICO;

                valorBruto -= ($scope.nf.NF_VLR_COFINS) ? $scope.nf.NF_VLR_COFINS : 0;
                valorBruto -= ($scope.nf.NF_VLR_CSLL) ? $scope.nf.NF_VLR_CSLL : 0;
                valorBruto -= ($scope.nf.NF_VLR_INSS) ? $scope.nf.NF_VLR_INSS : 0;
                valorBruto -= ($scope.nf.NF_VLR_IR) ? $scope.nf.NF_VLR_IR : 0;
                valorBruto -= ($scope.nf.NF_VLR_PIS) ? $scope.nf.NF_VLR_PIS : 0;
                valorBruto -= ($scope.nf.NF_VLR_OUTRAS) ? $scope.nf.NF_VLR_OUTRAS : 0;
                //valorBruto -= ($scope.nf.NF_VLR_ISS) ? $scope.nf.NF_VLR_ISS : 0;

                $scope.nf.NF_VLR_NOTA = valorBruto;

            }, 1000);
        }
    }


    $scope.salvarNotaFiscalAvulsa = function () {

        formHandlerService.submit($scope, {
            url: Util.getUrl("/notaFiscal/SalvarNotaFiscalAvulsa"),
            objectName: 'nf',
            showAjaxLoader: true,
            success: function (resp, status, config, message, validationMessage) {

                $scope.message = message;
                $scope.erros = validationMessage;

                $scope.buttonSave = 'reset';

                if (resp.success) {

                    $scope.message = Util.createMessage("success", "Proposta emitida com sucesso!");

                    $timeout(function () {

                        $scope.message = null;
                        window.open(Util.getUrl('/notaFiscal/index'), '_self');

                    }, 1000);

                }
            }

        });
    }

    $scope.editarNotaAvulsa = function (item) {

        if (item && item.NF_ID && item.NF_ID != 0) {
            var url = Util.getUrl("/notaFiscal/avulsa?nfId=" + item.NF_ID);
            post(url);
        }

    }


    $scope.gerarLinkDanfe = function (nfeId) {

        if (nfeId) {

            var url = Util.getUrl("/notaFiscal/gerarLinkDanfe");

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'link',
                responseModelName: 'link',
                data: { nfeId: nfeId },
                showLoader: true,
                success: function (response) {

                    if (response.success) {

                        if ($scope.link && $scope.link.Link) {

                            $window.open($scope.link.Link, '_new');
                        }
                    }
                }
            });
        }
    };

}]);

