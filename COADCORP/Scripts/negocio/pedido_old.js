appModule.controller("PedidoController", function ($scope, $http, messageService, /** $location ,*/ mask, clearMask, formHandlerService) {
    $scope.init = function (usuario) {

        $scope.prepedido = {};
        $scope.prepedido.lstDadosPedido = [{ lstDeTelefones: [{}], lstDeEmails: [{}] }];
        $scope.prepedido.lstFormaPagamento = [];
        $scope.prepedido.lstDeEmails = [{}];
        $scope.prepedido.cliente = { CLI_ID: null, ENDERECO_ENTREGA: { CLI_ID: null, END_TIPO: 1 }, ENDERECO_FATURAMENTO: { CLI_ID: null, END_TIPO: 2 } };
        $scope.prepedido.lstDeTelefones = [{}];
        $scope.listaTiposDeTelefone = [{}];
        $scope.lstTiposDeCliente = [];
        $scope.lstSetorDeTelefone = [];
        $scope.lstSetorDeEmails = [];
        $scope.lstProdutos = [];
        $scope.queryTelEmail = { show: true }
        $scope.prepedido.prospectClienteOpt = 'Cliente'; // deletar em outra oportunidade
        $scope.codrepoperador = usuario.USU_LOGIN;
        $scope.operadora = usuario.USU_NOME;


        $scope.prepedido.anobase = new Date().getFullYear() - 1966; // ano coad = data atual - 1966
        
    };

    $scope.listar = function (pageRequest) {

        var url = Util.getUrl("/prepedido/prepedidos");

        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }

        var config = {
            url: url,
            targetObjectName: 'prepedidos',
            responseModelName: 'prepedidos',
            pageConfig: { pageName: 'page' },
            showAjaxLoader : true,
        };

        if ($scope.filtro) {

            config.data = angular.copy($scope.filtro);
        }
        formHandlerService.read($scope, config);
    };

    $scope.getLstClientes = function (pageRequest) {

        
       // showAjaxLoader();
        var url = Util.getUrl("/cliente/clientes");

        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }

        var config = {
            url: url,
            targetObjectName: 'lstclientes',
            responseModelName: 'clientes',
            pageConfig: { pageName: 'page' },
            showAjaxLoader : true,
        };

        if ($scope.filtro) {

            config.data = angular.copy($scope.filtro);
        }
        formHandlerService.read($scope, config);

    }
    $scope.MascaraValor = function (element) {
        var value = "";
        
        value = String(element);
        value = value.replace(/\D/g, "");
        value = value.replace(/(\d)(\d{8})$/, "$1.$2");//coloca o ponto dos milhões
        value = value.replace(/(\d)(\d{5})$/, "$1.$2");//coloca o ponto dos milhares

        value = value.replace(/(\d)(\d{2})$/, "$1,$2");//coloca a virgula antes dos 2 últimos dígitos

        element = value;
        return value;
    }

    $scope.DesmascararValor = function(value){

        value = String(value);
        value = value.replace(".", "").replace(",", ".");

        return Number(value);
    }

    $scope.produtoSelecionado = function ($index) {

        var dadosDoPedido = $scope.prepedido.lstDadosPedido[$index];
        if (dadosDoPedido.prodDto != null && dadosDoPedido.prodDto.CMP_ID) {

            dadosDoPedido.produtoProd = dadosDoPedido.prodDto.CMP_ID;
            dadosDoPedido.CMP_ID = dadosDoPedido.prodDto.CMP_ID;
        }
            

        dadosDoPedido.valor = dadosDoPedido.prodDto.CMP_VLR_VENDA;
        dadosDoPedido.total = dadosDoPedido.prodDto.CMP_VLR_VENDA;
        dadosDoPedido.desconto = null;
    }

    $scope.CalcularDesconto = function (dadosPedido) {
        if (dadosPedido && dadosPedido.valorMask && dadosPedido.desconto) {

            var desconto = dadosPedido.desconto;

            if (typeof desconto == 'string') {

                desconto = Number(String(desconto).replace(/\D/g, ""));
            }
            var value = clearMask('mask_dinheiro')(dadosPedido.valorMask);

            var calcular = (value - (value * (desconto / 100))).toFixed(2);
            dadosPedido.total = calcular;
        }
        else if (dadosPedido && dadosPedido.valorMask) {
            dadosPedido.total = clearMask('mask_dinheiro')(dadosPedido.valorMask);
        }
        else if (dadosPedido) {
            dadosPedido.total = 0;
        }
    }

    $scope.carregarProdutos = function () {
        var url = "/PrePedido/ProdutosComposicao";
        $http({
            url: url,
            method: "post"
        }).success(function (retorno) {
            $scope.lstProdutos = retorno.result.produtos;
        });
    }

    $scope.selecionarTipoDeCliente = function () {
        var url = "/PrePedido/RetornarTiposDeClientes";
        var data = { tipoPagamento: "0" };
        $http({
            url: url,
            method: "post",
            data: data
        }).success(function (retorno) {
            $scope.lstTiposDeCliente = retorno;
        });
    }

    $scope.carregarTiposDeTelefone = function () {
        var url = "/PrePedido/RetornarTiposDeTelefone";
        $http({
            url: url,
            method: "post"
        }).success(function (retorno) {
            $scope.listaTiposDeTelefone = retorno;
        });
    }

    $scope.carregarEmailDeCliente = function () {
        var url = "/PrePedido/RecuperarSetorDeEmail";
        var data = { identificador: 0 };
        $http({
            url: url,
            method: "post",
            data: data
        }).success(function (retorno) {
            $scope.lstSetorDeEmails = retorno;

            // inicializa o atributo show que irá ser usada para mostrar ou não o tipo de setor do email
            if ($scope.lstSetorDeEmails) {

                angular.forEach($scope.lstSetorDeEmails, function (value) {
                    
                    value.show = true;
                });
            }
        });
    }

    $scope.carregarSetorDeTelefone = function () {
        var url = "/PrePedido/RecuperarSetorDeTelefone";
        var data = { identificador: 0 };
        $http({
            url: url,
            method: "post",
            data: data
        }).success(function (retorno) {
            $scope.lstSetorDeTelefone = retorno;

            // inicializa o atributo show que irá ser usada para mostrar ou não o tipo de setor do telefone
            if ($scope.lstSetorDeTelefone) {

                angular.forEach($scope.lstSetorDeTelefone, function (value) {

                    value.show = true;
                });
            }
        });
    }

    

    $scope.iniciarComboBox = function () {
        $scope.carregarProdutos();
        $scope.carregarTiposDeTelefone();
        $scope.selecionarTipoDeCliente();
        $scope.carregarEmailDeCliente();
        $scope.carregarSetorDeTelefone();
    }

    $scope.selecionarEmpresa = function (selecionado) {
        $scope.empresaEscolhida = selecionado;
        angular.element("#empresaModal").modal("hide");
    }

    $scope.novoEmail = function (pedido) {

       if (pedido.lstDeEmails.length < 6) {

            angular.forEach(pedido.lstDeEmails, function (value) {

                console.info(value);
            });
            pedido.lstDeEmails.push({});
        }
    }

    // busca o tipo de email baseado no id do setor
    $scope.achaTipoEmail = function (search) {

        if ($scope.lstSetorDeEmails) {

            var objResult = {};
            angular.forEach($scope.lstSetorDeEmails, function (obj) {

                if (obj.Value == search) {

                    objResult = obj;
                    return;
                }
            });
            return objResult;
        }
    }

    
    /*
    * valida o tipo de email baseado nas regras
    * 1- Emails de consulta podem ser no máximo 3
    * 2- Demais tipos de emails só podem ser 1 de cada
    * 3- No Total devem haver no máximo 6 emails
    */
    $scope.validaEmail = function (pedido, emailCliente) {

        var tipo = emailCliente.idtipo;
        var qtdEmailPagar = 0;
        var qtdEmailConsultar = 0;
        var qtdEmailNewsletter = 0;
        var qtdEmailRenovar = 0;

        angular.forEach(pedido.lstDeEmails, function (value) {

            if (value && value.idtipo) {

               if (value.idtipo == "3" && value != emailCliente) {

                    qtdEmailConsultar++;
                }
               
               if (value.idtipo == "2" && value != emailCliente) {
                    
                    qtdEmailPagar++;
                }

               if (value.idtipo == "4" && value != emailCliente) {

                    qtdEmailNewsletter++;
                }
               if (value.idtipo == "5" && value != emailCliente) {

                    qtdEmailRenovar++;                    
                }            
                
            }
        });


        if (tipo == "3" && qtdEmailConsultar >= 3) {

            $scope.message = messageService.fail("Só podem existir no máximo 3 emails para consulta.");
           // $location.hash('mensagem');
            //$anchorScroll();
            emailCliente.idtipo = null;
            return;
        }

        if (tipo == "2" && qtdEmailPagar >= 1) {

            $scope.message = messageService.fail("Só podem existir até no máximo 1 email do setor pagar.");
            //$location.hash('mensagem');
            //$anchorScroll();
            emailCliente.idtipo = null;
            return;
        }

        if (tipo == "4" && qtdEmailNewsletter >= 1) {

            $scope.message = messageService.fail("Só podem existir 1 email do setor newsletter.");
            //$location.hash('mensagem');
            //$anchorScroll();
            emailCliente.idtipo = null;
            return;
        }

        if (tipo == "5" && qtdEmailRenovar >= 1) {

            $scope.message = messageService.fail("Só podem existir 1 email do setor renovar.");
           // $location.hash('mensagem');
            //$anchorScroll();
            emailCliente.idtipo = null;
            return;
        }
    }

    /*
    * valida o tipo de telefone baseado nas regras
    * 1- Telefones de consulta podem ser no máximo 3
    * 2- Demais tipos de emails só podem ser 1 de cada
    * 3- No Total devem haver no máximo 5 telefones
    */
    $scope.validaTelefone = function (pedido, telefoneCliente) {

        var tipo = telefoneCliente.idsetor;
        var qtdTelPagar = 0;
        var qtdTelConsultar = 0;       
        var qtdTelRenovar = 0;

        angular.forEach(pedido.lstDeTelefones, function (value) {

            if (value && value.idsetor) {

                if (value.idsetor == "3" && value != telefoneCliente) {

                    qtdTelConsultar++;
                }

                if (value.idsetor == "2" && value != telefoneCliente) {

                    qtdTelPagar++;
                }

                if (value.idsetor == "5" && value != telefoneCliente) {

                    qtdTelRenovar++;
                }

            }
        });


        if (tipo == "3" && qtdTelConsultar >= 3) {

            $scope.message = messageService.fail("Só podem existir no máximo 3 telefones para consulta.");
            //$location.hash('mensagem');
            //$anchorScroll();
            telefoneCliente.idsetor = null;
            return;
        }

        if (tipo == "2" && qtdTelPagar >= 1) {

            $scope.message = messageService.fail("Só podem existir até no máximo 1 telefone do setor pagar.");
            //$location.hash('mensagem');
            //$anchorScroll();
            telefoneCliente.idsetor = null;
            return;
        }


        if (tipo == "5" && qtdTelRenovar >= 1) {

            $scope.message = messageService.fail("Só podem existir 1 telefone do setor renovar.");
            //$location.hash('mensagem');
            //$anchorScroll();
            telefoneCliente.idsetor = null;
            return;
        }
    }

    $scope.novoTelefone = function (pedido) {

        if (pedido.lstDeTelefones.length < 5) {

            pedido.lstDeTelefones.push({});
        }
        
    }

    $scope.novoPedido = function (selecionado) {
        $scope.prepedido.lstDadosPedido.push({ lstDeTelefones: [{}], lstDeEmails: [{}]});
        $scope.prepedido.lstFormaPagamento.push({});
    }

    $scope.novaFormaDePagamento = function (selecionado) {
        //$scope.lstFormaPagamento.push({});
    }

    $scope.recuperarDadosProspect = function (valor) {
        var url = "/PrePedido/RecuperarProspect";
        var urlinfadicionais = "/PrePedido/RecuperarProspectInformacoesAdicionais";
        var urlemail = "/PrePedido/RecuperarEmailProspect";
        var urltel = "/PrePedido/RecuperarTelefoneProspect";
        var complementoEntrega = "";
        var complementoFaturamento = "";
        var data = { idProspect: valor };
        $http({
            url: url,
            method: "post",
            data: data
        }).success(function (retorno) {
            if (retorno) {
                complementoEntrega = retorno.TIPO_COMPL + retorno.COMPL + retorno.TIPO_COMPL2 + retorno.COMPL2 + retorno.TIPO_COMPL3 + retorno.COMPL3;
                complementoFaturamento = retorno.TIPO_COMPL + retorno.COMPL + retorno.TIPO_COMPL2 + retorno.COMPL2 + retorno.TIPO_COMPL3 + retorno.COMPL3;

                if (!$scope.prepedido.cliente) {

                    $scope.prepedido.cliente = {};
                }
                if (!$scope.prepedido.cliente.ENDERECO_ENTREGA) {
                    $scope.prepedido.cliente.ENDERECO_ENTREGA = {};
                }
                if (!$scope.prepedido.cliente.ENDERECO_FATURAMENTO) {

                    $scope.prepedido.cliente.ENDERECO_FATURAMENTO = {};
                }

                $scope.prepedido.cliente.CLI_NOME = retorno.NOME;
                $scope.prepedido.tel1 = retorno.DDD_TEL + retorno.TELEFONE;
                $scope.prepedido.cliente.CLI_A_C = retorno.A_C;
                $scope.prepedido.email1 = retorno.E_MAIL;
                $scope.prepedido.tel2 = retorno.DDD_FAX + retorno.FAX;
                $scope.prepedido.cliente.ENDERECO_ENTREGA.END_LOGRADOURO = retorno.TIPO + retorno.LOGRAD;
                $scope.prepedido.cliente.ENDERECO_ENTREGA.END_NUMERO = retorno.NUMERO;
                $scope.prepedido.cliente.ENDERECO_ENTREGA.END_COMPLEMENTO = complementoEntrega;
                $scope.prepedido.cliente.ENDERECO_ENTREGA.END_CEP = retorno.CEP;
                $scope.prepedido.cliente.ENDERECO_ENTREGA.END_BAIRRO = retorno.BAIRRO;
                $scope.prepedido.cliente.ENDERECO_ENTREGA.END_MUNICIPIO = retorno.MUNIC;
                $scope.prepedido.ufentrega = retorno.UF;
                $scope.prepedido.cliente.ENDERECO_FATURAMENTO.END_LOGRADOURO = retorno.TIPO + retorno.LOGRAD;
                $scope.prepedido.cliente.ENDERECO_FATURAMENTO.END_NUMERO = retorno.NUMERO;
                $scope.prepedido.cliente.ENDERECO_FATURAMENTO.END_COMPLEMENTO = complementoFaturamento;
                $scope.prepedido.cliente.ENDERECO_FATURAMENTO.END_CEP = retorno.CEP;
                $scope.prepedido.cliente.ENDERECO_FATURAMENTO.END_BAIRRO = retorno.BAIRRO;
                $scope.prepedido.cliente.ENDERECO_FATURAMENTO.END_MUNICIPIO = retorno.MUNIC;
                $scope.prepedido.cliente.ENDERECO_FATURAMENTO.uffaturamento = retorno.UF;
                
                $http({
                    url: urltel,
                    method: "post",
                    data: data
                }).success(function (retornotel) {
                    $scope.prepedido.celular = retornotel.DDD_TEL + retornotel.TELEFONE;
                });

                $http({
                    url: urlemail,
                    method: "post",
                    data: data
                }).success(function (retorno) {
                    $scope.email2 = retorno.E_MAIL;
                });

                $http({
                    url: urlinfadicionais,
                    method: "post",
                    data: data
                }).success(function (retorno) {
                    $scope.cpfcnpj = retorno.CPF_CNPJ;
                    $scope.inscestadual = retorno.INSCRICAO;
                });
            } else {
                $scope.message = messageService.fail("Não existem prospects com esse código!");
                //$location.hash('mensagem');
               // $anchorScroll();
            }
            

        }).
        error(function (data, status, headers, config) {
            
            // called asynchronously if an error occurs
            // or server returns response with an error status.
        });        
    }

    $scope.recuperarDadosCliente = function(valor) {

        formHandlerService.read($scope, {
            url: Util.getUrl("/cliente/recuperardadosdocliente"),
            targetObjectName: 'prepedido.cliente',
            responseModelName: 'cliente',
            dateFieldsNames: ['DATA_CADASTRO', 'DATA_ALTERA', 'DATA_EXCLUSAO'],
            data: { idCliente: valor },
            success: function () {
                
                $scope.prepedido.cliente.TIPO_CLI_ID = String($scope.prepedido.cliente.TIPO_CLI_ID);
                $scope.prepedido.CLI_ID = $scope.prepedido.cliente.CLI_ID;

                var cliente = $scope.prepedido.cliente;

                if (!cliente.ENDERECO_ENTREGA || !cliente.ENDERECO_ENTREGA.END_TIPO) {

                    $scope.prepedido.cliente.ENDERECO_ENTREGA = { END_TIPO: 1 };
                }

                if (!cliente.ENDERECO_FATURAMENTO || !cliente.ENDERECO_FATURAMENTO.END_TIPO) {

                    $scope.prepedido.cliente.ENDERECO_FATURAMENTO = { END_TIPO: 2};
                }
            }
        });
    }

    $scope.read = function (empId, prepedidoId) {

        if (empId && prepedidoId) {

            formHandlerService.read($scope, {
                url: Util.getUrl("/prepedido/load"),
                targetObjectName: 'prepedido',
                responseModelName: 'prepedido',
                dateFieldsNames: ['predata'],
                data: { empId: empId, prepedidoId: prepedidoId },
                success: function () {

                    var cliente = $scope.prepedido.cliente;
                    cliente.TIPO_CLI_ID = String(cliente.TIPO_CLI_ID);

                    var lstPedido = $scope.prepedido.lstDadosPedido;

                    var index = 0;
                    angular.forEach(lstPedido, function (value) {

                        value.prodDto = value.PRODUTO_COMPOSICAO;
                        value.valorMask = Number(value.valor).toFixed(2);
                        value.descontoMask = value.desconto;
                        value.totalMask = Number(value.total).toFixed(2);

                        index++;
                    });

                }
            });
        }
            
    }

   /** $scope.recuperarDadosCliente = function (valor) {
        var url = "/PrePedido/RecuperarDadosDoCliente";
        var urlemail = "/PrePedido/RecuperarEmailsDoCliente";
        var urltel = "/PrePedido/RecuperarTelefonesDoCliente";
        var urlendereco = "/PrePedido/RecuperarEnderecoDoCliente";
        var complementoEntrega = "";
        var complementoFaturamento = "";
        var data = { idCliente: valor};

        $http({
            url: url,
            method: "post",
            data: data
        }).success(function (retorno) {
            $scope.erros = null;
            if (retorno) {
                $scope.prepedido.cliente.CLI_ID = retorno.clienteId;
                $scope.prepedido.cliente.CLI_NOME = retorno.nomeRazaoSocial;
                $scope.prepedido.cliente.CLI_A_C = retorno.aosCuidados;
                $scope.prepedido.cliente.CLI_CPF_CNPJ = retorno.cpfCnpj;
                $scope.prepedido.cliente.CLI_INSCRICAO = retorno.inscricaoEstadual;
                $scope.prepedido.cliente.CLI_TP_PESSOA = String(retorno.tipoDeCliente);
                $scope.prepedido.cliente.ENDERECO_ENTREGA.END_TIPO = 1;
                $scope.prepedido.cliente.ENDERECO_ENTREGA.END_LOGRADOURO = retorno.enderecoEntrega;
                $scope.prepedido.cliente.ENDERECO_ENTREGA.END_NUMERO = retorno.numeroEntrega;
                $scope.prepedido.cliente.ENDERECO_ENTREGA.END_COMPLEMENTO = retorno.complementoEntrega;
                $scope.prepedido.cliente.ENDERECO_ENTREGA.END_CEP = retorno.cepEntrega;
                $scope.prepedido.cliente.ENDERECO_ENTREGA.END_BAIRRO = retorno.bairroEntrega;
                $scope.prepedido.cliente.ENDERECO_ENTREGA.END_MUNICIPIO = retorno.municipioEntrega;
                $scope.prepedido.cliente.ENDERECO_ENTREGA.UF = retorno.ufEntrega;
                $scope.prepedido.cliente.ENDERECO_ENTREGA.END_TIPO = 2;
                $scope.prepedido.cliente.ENDERECO_FATURAMENTO.END_LOGRADOURO = retorno.enderecoFaturamento;
                $scope.prepedido.cliente.ENDERECO_FATURAMENTO.END_NUMERO = retorno.numeroFaturamento;
                $scope.prepedido.cliente.ENDERECO_FATURAMENTO.END_COMPLEMENTO = retorno.complementoFaturamento;
                $scope.prepedido.cliente.ENDERECO_FATURAMENTO.END_CEP = retorno.cepFaturamento;
                $scope.prepedido.cliente.ENDERECO_FATURAMENTO.END_BAIRRO = retorno.bairroFaturamento;
                $scope.prepedido.cliente.ENDERECO_FATURAMENTO.END_MUNICIPIO = retorno.municipioFaturamento;
                $scope.prepedido.cliente.ENDERECO_FATURAMENTO.UF = retorno.ufFaturamento;
                $scope.prepedido.CLI_ID = retorno.clienteId;

                $scope.prepedido.lstDeEmails = retorno.emails;
                $scope.prepedido.lstDeTelefones = retorno.telefones;
            } else {
                $scope.message = messageService.fail("Não existe cliente com esse código!");
                $location.hash('mensagem');
                $anchorScroll();
            }
        }).
        error(function (data, status, headers, config) {
            // alert("F");
            // called asynchronously if an error occurs
            // or server returns response with an error status.
        });


    }
    */
    $scope.limparCampos = function () {
        $scope.prepedido.codigoProspect = "";
        $scope.prepedido.nome = "";
        $scope.prepedido.tel1 = "";
        $scope.prepedido.aoscuidados = "";
        $scope.prepedido.email1 = "";
        $scope.prepedido.tel2 = "";
        $scope.prepedido.enderecoentrega = "";
        $scope.prepedido.numeroentrega = "";
        $scope.prepedido.complementoentrega = "";
        $scope.prepedido.cepentrega = "";
        $scope.prepedido.bairroentrega = "";
        $scope.prepedido.municipioentrega = "";
        $scope.prepedido.ufentrega = "";
        $scope.prepedido.enderecofaturamento = "";
        $scope.prepedido.numerofaturamento = "";
        $scope.prepedido.complementofaturamento = "";
        $scope.prepedido.cepfaturmento = "";
        $scope.prepedido.bairrofaturamento = "";
        $scope.prepedido.municipiofaturamento = "";
        $scope.prepedido.uffaturamento = "";
        $scope.prepedido.cpfcnpj = "";
        $scope.prepedido.inscestadual = "";
        $scope.prepedido.email2 = "";
        $scope.prepedido.celular = "";
    }

    $scope.validarDadosIniciaisFormulario = function () {
        var url = "/PrePedido/ValidarInformacoesIniciais";
        $scope.prepedido.lstFormaPagamento = [];
        var data = JSON.stringify($scope.prepedido);
        $http({
            url: url,
            method: "post",
            data: data,
            headers: {
                "Content-Type": "application/json"
            }
        }).success(function (retorno) {

            $scope.erros = retorno.validationMessage;
            $scope.message = retorno.message;

            if (retorno.success) {

                $scope.prepedido.lstFormaPagamento = [];
                angular.forEach($scope.prepedido.lstDadosPedido, function (value) {
                    $scope.prepedido.lstFormaPagamento.push({ totalMask: value.totalMask, totalFixo : value.total });
                });
                angular.element("#empresaModal").modal();
            }
            else {
                $scope.message = retorno.message;
                $location.hash('mensagem');
                //$anchorScroll();
            }
        });
    }

    $scope.quickTest = function () {
        $scope.prepedido = { "lstDadosPedido": [{ "lstDeTelefones": [{ "idsetor": "2", "tipo": "CELULAR ", "telefone": "2134434343" }], "lstDeEmails": [{ "email": "dx_diego@hotmail.com", "idtipo": "3" }], "valor": 500, "total": 475, "tiponegocioprod": "1", "prodDto": { "CMP_ID": 255, "PRO_ID": 546, "CMP_DESCRICAO": "Composição de teste", "CMP_NOME_ESTRANGEIRO": "Composição de teste", "UEN_ID": "ED", "AREA_ID": null, "TIPO_PRO_ID": 1, "TIPO_ENVIO_ID": 1, "CMP_VLR_VENDA": 500, "PEDIDO": [], "PRODUTO_COMPOSICAO_ITEM": [], "PRODUTOS": { "PRO_ID": 546, "PRO_SIGLA": "PASTA PLA", "PRO_NOME": "PASTA PLAST.\" ADV\"", "PRO_ID_DERVADO": null, "PRO_RECEBE_MALA": 0, "PRO_STATUS": 0, "NCM_ID": "39261000", "GRUPO_ID": 6, "DATA_CADASTRO": "/Date(1430924409543)/", "DATA_ALTERA": null, "DATA_EXCLUSAO": null, "USU_LOGIN_EXCLUSAO": null, "PRO_UN_COMPRA": "PC", "PRO_UN_VEND": "PC", "PRO_PRECO_COMPRA": 4.5, "PRO_PRECO_CUSTO": 4.5, "AREA_ID": 0, "PRO_PRECO_VENDA": 0, "TIPO_PRO": 7, "TPC_ID": 2, "AREAS": { "AREA_ID": 0, "AREA_NOME": "X", "PRODUTOS": [] }, "GRUPO": { "GRUPO_ID": 6, "GRU_DESCRICAO": "CONSUMO", "PRODUTOS": [] }, "PRODUTO_COMPOSICAO": [], "PRODUTO_COMPOSICAO_ITEM": [], "TIPO_PRODUTO": { "TIPO_PRO": 7, "TIPO_DESCRICAO": "MaterialConsumo", "PRODUTOS": [] }, "UNIDADE_MEDIDA": { "UND_ID": "PC", "UND_DESCRICAO": "PACOTE", "PRODUTOS": [], "PRODUTOS1": [] }, "UNIDADE_MEDIDA1": { "UND_ID": "PC", "UND_DESCRICAO": "PACOTE", "PRODUTOS": [], "PRODUTOS1": [] }, "TIPO_PROD_COMPORTAMENTO": { "TPC_ID": 2, "TPC_DESCRICAO": "NORMAL", "PRODUTOS": [] } }, "TIPO_ENVIO": { "TIPO_ENVIO_ID": 1, "TIPO_ENVIO_DESCRICAO": "Mensal", "PRODUTO_COMPOSICAO": [] }, "TIPO_PRODUTO_COMPOSICAO": { "TIPO_PRO_ID": 1, "TIPO_PRO_DESCRICAO": "Online", "PRODUTO_COMPOSICAO": [] }, "UNIDADE_NEGOCIO": { "UND_NEGOCIO_ID": "ED", "UND_NEGOCIO_DESCR": "Educação", "PRODUTO_COMPOSICAO": [] }, "DATA_EXCLUSAO": null, "USU_LOGIN_EXCLUSAO": null }, "produtoProd": 255, "CMP_ID": 255, "desconto": "5", "valorMask": "500,00", "totalMask": "475,00", "inicioVigenciaprod2": "19/06/2015", "inicioVigenciaprod": "2015-06-19T03:00:00.000Z", "fimVigenciaprod2": "18/06/2015", "fimVigenciaprod": "2015-06-18T03:00:00.000Z" }], "lstFormaPagamento": [{ "totalMask": "475,00", "totalFixo": 475, "entrada": null, "valorparcelas": null, "total": 475 }], "lstDeEmails": [{ "assinatura": "30A00514", "idemail": "467159", "email": "tonziro@tonziro.com.br", "idtipo": null, "tipo": null }, { "assinatura": "30A00514", "idemail": "467159", "email": "tonziro@tonziro.com.br", "idtipo": null, "tipo": null }], "cliente": { "CLI_ID": 401286, "ENDERECO_ENTREGA": { "CLI_ID": null, "END_TIPO": 2, "END_LOGRADOURO": "RUA", "END_NUMERO": "374", "END_COMPLEMENTO": null, "END_CEP": "35010160", "END_BAIRRO": "CENTRO", "END_MUNICIPIO": "Governador Valadares", "UF": "MG" }, "ENDERECO_FATURAMENTO": { "CLI_ID": null, "END_TIPO": 2, "END_LOGRADOURO": "RUA", "END_NUMERO": "374", "END_COMPLEMENTO": null, "END_CEP": "35010160", "END_BAIRRO": "CENTRO", "END_MUNICIPIO": "Governador Valadares", "UF": "MG" }, "CLI_NOME": "TONZIRO ORG.TEC.CONTABIL LTDA", "CLI_A_C": "Diego", "CLI_CPF_CNPJ": "20625141000107", "CLI_INSCRICAO": "ISENTO", "CLI_TP_PESSOA": "1" }, "lstDeTelefones": [{ "assinatura": "30A00514", "idtelefone": "1935024", "telefone": "3333501016", "tipo": "FAX ", "idsetor": "", "setor": null }, { "assinatura": "30A00514", "idtelefone": "1935024", "telefone": "3333501016", "tipo": "FAX ", "idsetor": "", "setor": null }, { "assinatura": "30A00514", "idtelefone": "1935024", "telefone": "3333501016", "tipo": "FAX ", "idsetor": "", "setor": null }], "prospectClienteOpt": "Cliente", "anobase": 49, "periodo": "3", "semana": "6", "empresa": "2", "CLI_ID": 401286 };
        
        };
        
    $scope.removeTelefone = function (dadosDoPedido,telefone) {

        var index = dadosDoPedido.lstDeTelefones.indexOf(telefone);
        dadosDoPedido.lstDeTelefones.splice(index, 1);
    }

    $scope.removeEmail = function (dadosDoPedido, email) {

        var index = dadosDoPedido.lstDeEmails.indexOf(telefone);
        dadosDoPedido.lstDeEmails.splice(index, 1);
    }

    $scope.removePedido = function (dadosDoPedido) {

        var index = $scope.prepedido.lstDadosPedido.indexOf(dadosDoPedido);
        $scope.prepedido.lstDadosPedido.splice(index, 1);
    }

    $scope.abreModalClientes = function (prepedido) {
        $scope.prepedidoSelecionado = prepedido;
        angular.element("#modal_clientes").modal();
    }

    $scope.incluirCliente = function (item) {

        if ($scope.prepedidoSelecionado) {

            $scope.recuperarDadosCliente(item.CLI_ID)
        }

        angular.element("#modal_clientes").modal('hide');
    }


});
appModule.controller("FormaPagamentoController", function ($scope, $http,/** $location, */ clearMask) {
    $scope.lstTipoPagamento = [];
    
    $scope.selecionarFormaDePagamento = function (opcao, formaDePagamento) {

        formaDePagamento.formapagtoprod = null;
        formaDePagamento.total = formaDePagamento.totalFixo;
        if (opcao && (Number(opcao) === 0 || Number(opcao) === 1)) {

            formaDePagamento.entrada = $scope.formaDePagamento.total;
            if (opcao && Number(opcao) === 1) {

                formaDePagamento.qteparcelas = 0;
                formaDePagamento.valorparcelasMask = null;
                
            }
        }       
        else {
            formaDePagamento.entradaMask = null;
            formaDePagamento.qteparcelas = null;
            formaDePagamento.valorparcelasMask = null;
        }

        var url = "/PrePedido/RetornarFormasDePagamento";
        var data = { tipoPagamento: opcao };
        $http({
            url: url,
            method: "post",
            data: data
        }).success(function (retorno) {
            $scope.lstTipoPagamento = retorno;
        });
    }

    $scope.SalvarPedido = function () {
        var url = "/PrePedido/CadastrarPedido";
        var data = JSON.stringify($scope.prepedido);
        $http({
            url: url,
            method: "post",
            data: data,
            headers: {
                "Content-Type": "application/json"
            }
        }).success(function (retorno) {
            $scope.messageFormPagamento = retorno.message;
            $scope.errosFormaPagamento = retorno.validationMessage;

            if (retorno.success) {

                alert("Pedido salvo com sucesso");
                window.open("/PrePedido","_self");
            }

            
           // $location.hash('messageFormPagamento');
            //$anchorScroll();
        });
    }



    $scope.validarFormasDePagamentoFormulario = function () {
        var x = 0;
        return false;
    }

    $scope.calcularParcelas = function () {
        
        var total =  BigDecimal.toFixed($scope.formaDePagamento.totalFixo);
        var entrada = BigDecimal.toFixed($scope.formaDePagamento.entradaFixo);
        var qteparcelas = $scope.formaDePagamento.qteparcelas;


        if (total && entrada && qteparcelas) {


            var valorParcelas = BigDecimal.toFixed((total - entrada) / qteparcelas);
            $scope.formaDePagamento.valorparcelas = valorParcelas;

            //// ------------- converto para int para que a soma não seja confundida com uma concatenação---------------
            valorParcelas = Number(valorParcelas);
            qteparcelas = Number(qteparcelas);
            entrada = Number(entrada);
            ///----------------------------------------------------------------------------------------------------------
            var totalCalculado = BigDecimal.toFixed((valorParcelas * qteparcelas) + entrada);
            var total = $scope.formaDePagamento.total;

            var sobra = (total - totalCalculado).toFixed(2);
            $scope.formaDePagamento.entradaMask = ($scope.formaDePagamento.entradaFixo + Number(sobra)).toFixed(2);

        }
        else {
            $scope.formaDePagamento.valorparcelasMask = null;
            $scope.formaDePagamento.entradaMask = BigDecimal.toFixed($scope.formaDePagamento.entradaFixo);
        }
    }

    $scope.calculoParcelaChange = function () {


        ///--------------------- valida se a entrada é maior que o total -------------------------
        var opcao = $scope.formaDePagamento.condicaoDePagamento;
        var valorEntrada = Number(clearMask('mask_dinheiro')($scope.formaDePagamento.entradaMask));
        $scope.formaDePagamento.entradaFixo = valorEntrada;
        var total = Number($scope.formaDePagamento.totalFixo);

        if (valorEntrada > total) {

            $scope.formaDePagamento.entradaMask = total.toFixed(2);
        }
        //-----------------------------------------------------------------------------------------
        
        if (opcao && Number(opcao) === 1) {

            $scope.formaDePagamento.qteparcelas = 0;
            $scope.formaDePagamento.valorparcelasMask = null;
            $scope.formaDePagamento.total = $scope.formaDePagamento.totalFixo;

        }
    }
   
});

appModule.controller("PedidoIndexController", function ($scope, $http, messageService) {
    $scope.excluirRegistro = function (idPedido, idEmp) {
        var url = "/PrePedido/ExcluirPedido";
        var data = { idPedido: idPedido, idEmp: idEmp };
        if (confirm("Tem certeza que quer excluir o registro de id igual á " + idPedido + "?") == true) {
            $http({
                url: url,
                method: "post",
                data: data,
                headers: {
                    "Content-Type": "application/json"
                }
            }).success(function (retorno) {

                //if (retorno.success) {

                    $scope.message = retorno.message;
                    //$location.hash('mensagem');
                    //$anchorScroll();

                //} else {
                //    $scope.message = retorno.message;
                //    $location.hash('mensagem');
                //    $anchorScroll();
                //}
                //location.reload();
            });
        } else {
            return false;
        }
        
    }
});