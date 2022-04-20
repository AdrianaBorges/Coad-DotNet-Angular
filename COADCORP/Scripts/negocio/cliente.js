 /// <reference path="controllers_plugins/endereco.js" />
appModule.controller('ClienteController', function ($scope, formHandlerService, $http, conversionService, $timeout) {

    $scope.tab = 1;
    $scope.tabAtiva = 0;
    $scope.filtro = {};
    $scope.filtro.Logradouro = " ";
    $scope.filtro.Email = " ";
    $scope.filtro.Telefone = " ";
    $scope.filtro.URA_ID = null;
    $scope.modalcli = false;


    $scope.initClienteFranquia = function () {
        $scope.filtro = { pesquisaCpfCnpjPorIqualdade: true };
    }

    $scope.assinturaSelect = null;
    $scope.totassSelect = null;
    $scope.consassSelect = null;
    $scope.atendimento = {};


    /// Pega data do sistema.

    var now = new Date();
    var dataatual = new Date(now.getFullYear(), now.getMonth(), now.getDate());

    /*
    *-------------------------------------------------------- Inicializações ----------------------------------------------------------------------
    */
    $scope.initAssinatura = function (_numassinatura) {

        $scope.NUM_ASSINATURA = _numassinatura;

        $scope.pesquisarAssinatura($scope.NUM_ASSINATURA);
    }

    $scope.init = function (RG_ID, validacaoTotalEndereco) {

        $scope.validacaoTotalEndereco = validacaoTotalEndereco;

        $scope.TipoAcaoTel = { label: 'Incluir', valor: 0 };
        $scope.prospect = { PROSPECTS_TELEFONE: [] };

        //if ($scope.validacaoTotalEndereco == false) {

        //    $scope.end = {validacaoTotal: false};
        //}

        $scope.iniciarCombos();

        $scope.carregaRegioesParaCadastroSuspect(RG_ID);

        $scope.validarPermissao('/Cliente/Index', 'Incluir');

        if ($scope.initEnd) {
            $scope.initEnd(validacaoTotalEndereco);
        }
    }

    $scope.iniciarCombos = function () {

        var url = "/endereco/combosendereco";
        $http({
            url: url,
            method: "post"
        }).success(function (retorno) {

            $scope.tipoLogradouro = retorno.result.tipoLogradouro;
            $scope.tipoEnderecos = retorno.result.tipoEnderecos;
            $scope.ListaClassificacao = retorno.result.ListaClassificacao;

        });

    }
    $scope.multiplicaqtdeURA = function () {

        if ($scope.updateura.qtde != null && $scope.updateura.qtde > 0)
            $scope.updateura.qtde3x = ($scope.updateura.qtde * 2);
        else
            $scope.updateura.qtde3x = 0;

    }

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
                    url = Util.getUrl("/Cliente/Editar?clienteId=" + $scope.cliente.CLI_ID + "&assinatura=" + $scope.assinturaSelect);
                    post(url);
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
    $scope.TestarEnvioEmail = function (_assinatura) {

        showAjaxLoader();

        var url = "/Cliente/TestarEnvioEmail";


        $http({
            url: url,
            method: "post",
            data: { _par_num_parcela: _assinatura }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.message = response.message;

            }
            else {

                if (response.message != null)
                    $scope.message = Util.createMessage("fail", response.message.message);
                else
                   $scope.message = Util.createMessage("fail", response);
                
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response.message.message);

            hideAjaxLoader();
        })

    }


    $scope.buscarClassificacaoAtend = function () {

        showAjaxLoader();

        var url = "/ClassificacaoAtendimento/PreencherGrids";


        $http({
            url: url,
            method: "post",
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.listaClassificacao = response.result.listaClassificacao;

            }
            else {

                $scope.listaClassificacao = null;

                alert(response.message.message);
            }

        }).error(function (response) {

            alert(response);

            hideAjaxLoader();
        })

    }

    $scope.atualizarAssinatura = function () {

        if (confirm("Estas informações serão atualizadas na Assinatura. Confirma ?")) {

            showAjaxLoader();

            var url = "/Cliente/AtualizarAssinatura";
            $http({
                url: url,
                method: "post",
                data: { _assinatura: $scope.updateassinatura }
            }).success(function (response) {

                hideAjaxLoader();

                if (response.success == true) {
                    alert(response.message.message);
                    url = Util.getUrl("/Cliente/Editar?clienteId=" + $scope.cliente.CLI_ID + "&assinatura=" + $scope.assinturaSelect);
                    post(url);
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

    $scope.atualizarAssinaturaURA = function () {

        if (confirm("Estas informações serão atualizadas na Assinatura. Confirma ?")) {

            showAjaxLoader();

            _ativoBool = ($scope.assinatura.ativo == 1);

            var url = "/Cliente/AtualizarAssinaturaURA";
            $http({
                url: url,
                method: "post",
                data: { _assinatura: $scope.assinatura, _ativo: _ativoBool }
            }).success(function (response) {

                hideAjaxLoader();

                if (response.success == true) {

                    alert(response.message.message);

                    url = Util.getUrl("/Cliente/Editar?clienteId=" + $scope.cliente.CLI_ID + "&assinatura=" + $scope.assinturaSelect);
                    post(url);

                    //$scope.NUM_ASSINATURA = null;

                    //$scope.pesquisarAssinatura($scope.NUM_ASSINATURA);

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




    $scope.pesquisarAssinatura = function (_numassinatura) {

        showAjaxLoader();

        var url = "/URA/PesquisarAssinatura";
        $http({
            url: url,
            method: "post",
            data: { _assinatura: _numassinatura }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.assinatura = response.result.assinatura;
                $scope.assinatura.ativo = 0;

                $scope.totassSelect = $scope.assinatura.ASN_QTDE_CONS_CONTRATO;
                $scope.consassSelect = $scope.assinatura.ASN_QTDE_CONS_UTILIZADA;

                if ($scope.assinatura.ASN_ATIVA == true && $scope.assinatura.ASN_QTDE_CONS_CONTRATO > 0)
                    $scope.assinatura.ativo = 1;

            }


        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);

            hideAjaxLoader();
        })


    }

    $scope.gerarSenhaAssinatura = function () {

        if (confirm("Deseja gerar uma nova senha para usuário?")) {

            showAjaxLoader();

            var url = "/Cliente/GerarNovaSenha";
            $http({
                url: url,
                method: "post",
                data: { _assinatura: $scope.assinturaSelect, _cli_id: $scope.cliente.CLI_ID }
            }).success(function (response) {

                hideAjaxLoader();

                if (response.success == true) {
                    alert(response.message.message);
                    $scope.buscarApuracaoConsulta($scope.assinturaSelect);
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


    $scope.initProspect = function (cpf_cnpj, nome, email, validarCPF_CNPJ) {

        validarCPF_CNPJ = (validarCPF_CNPJ == "1");
        var cliente = {
            ASSINATURA: [{
                ASSINATURA_EMAIL: [],
                ASSINATURA_TELEFONE: [],
                UEN_ID: 1
            }],
            CLIENTES_ENDERECO: [],
            ASSINATURA_EMAIL: [],
            ASSINATURA_TELEFONE: [],
            ValidarCPF_CNPJ : validarCPF_CNPJ
        };

        if (cpf_cnpj) {

            cliente.CLI_CPF_CNPJ = cpf_cnpj;
        }

        if (nome) {

            cliente.CLI_NOME = nome;
        }

        if (email) {

            cliente.ASSINATURA_EMAIL.push({ AEM_EMAIL: email });
        }


        $scope.cliente = cliente;
    }

    $scope.initBuscaCliente = function () {

        $scope.carregaRepresentanteAutoCompleteDTO();
    }

    $scope.sd = function () {

        $scope.cliente = {};
    }

    $scope.verificarEtiqueta = function (_tipo) {

        var url = "/Cliente/VerificarEtiqueta";
        $http({
            url: url,
            method: "post",
            data: { _id: _tipo }
        }).success(function (retorno) {

            $scope.emiteetiqueta = (retorno.result.TipoAtendimento == 1);

        });

    }

    $scope.LiberarDocVencido = function (parcela) {

        if (confirm("Confirmar esta operação ?")) {

            showAjaxLoader();

            if (parcela.PAR_SITUACAO == "NOR")
                parcela.PAR_SITUACAO = "LIB";
            else
                parcela.PAR_SITUACAO = "NOR";

            var url = "/Parcelas/Salvar";
            $http({
                url: url,
                method: "post",
                data: { _parcela: parcela, _tipo: "L" }
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

    

    $scope.buscarAssinatura = function (cliente) {

        var url = "/Cliente/BuscarAssinatura";
        $http({
            url: url,
            method: "post",
            data: { _cli_id: cliente }
        }).success(function (retorno) {

            if (retorno.success) {
                $scope.listaassinatura = retorno.result.listaassinatura.lista;
            }

        });

    }
    $scope.abrirModal = function (modal) {

        if (modal==1)
            angular.element("#Modal-CPF").modal();
        else
            angular.element("#Modal-CNPJ").modal();
    }

    $scope.abrirModalCancelamento = function (_contrato) {

        $scope.contrato = null;
        $scope.contrato = _contrato;
        conversionService.deepConversion($scope.contrato);

        $timeout(function () {
            $scope.message = null;
        }, 1000);

        angular.element("#Modal-Cancelar-Contrato").modal();

    }

    $scope.abrirModalEditarContrato = function (_contrato) {

        $scope.contrato = null;
        $scope.contrato = _contrato;
        conversionService.deepConversion($scope.contrato);

        angular.element("#Modal-Editar-Contrato").modal();

    }


    $scope.abrirModalPesquisarCliente = function (modal) {

        $scope.modalcli = true;

        angular.element("#Modal-Pesquisar-Cliente").modal();

    }

    $scope.buscaTelAssinatura = function (pageRequest) {

        $scope.abrirModalTelefone($scope.AssinaturaModal, false, pageRequest);

    }
    $scope.abrirModalTelefone = function (_asn_id, abrir, pageRequest) {

        showAjaxLoader();

        var url = "/Cliente/BuscarTelefones";
        $http({
            url: url,
            method: "post",
            data: { _assinatura: _asn_id, pagina: pageRequest  }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success === true) {

                $scope.AssinaturaModal = _asn_id;
                $scope.listatelefone = response.result.listatelefone;
                conversionService.deepConversion($scope.listatelefone);

                if (abrir === null)
                    angular.element("#Modal-Atualizar-Telefones").modal();
                
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
    $scope.abrirModalEmail = function (_asn_id, abrir) {

        showAjaxLoader();

        var url = "/Cliente/BuscarEmails";
        $http({
            url: url,
            method: "post",
            data: { _assinatura: _asn_id }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.AssinaturaModal = _asn_id;
                $scope.listaemail = response.result.listaemail;
                conversionService.deepConversion($scope.listaemail);

                if (abrir == null)
                    angular.element("#Modal-Atualizar-Email").modal();

            }
            else {

                alert(response.message.message);
            }

        }).error(function (response) {

            alert(response);

            hideAjaxLoader();
        })

    }

    $scope.abrirModalAtualizarAssinatura = function (_assinatura) {

        $scope.initAssinatura(_assinatura);

        angular.element("#Modal-Atualizar-Assinatura").modal();

    }

    $scope.abrirModalAtualizarURA = function (_contratadas) {

        $scope.updateura = {};
        $scope.updateura.ativo = true;
        $scope.updateura.qtde = _contratadas;
        if (_contratadas > 0)
            $scope.updateura.qtde3x = (_contratadas * 2);
        else
            $scope.updateura.qtde3x = 0;

        $scope.updateura.assinatura = $scope.assinturaSelect;

        angular.element("#Modal-Atualizar-URA").modal();

    }
    
    $scope.mostrarNossoNumero = function (_par_num_parcela) {

        showAjaxLoader();

        var url = "/Cliente/MostrarNossoNumero";
        $http({
            url: url,
            method: "post",
            data: { _idTitulo: _par_num_parcela}
        }).success(function (retorno) {

            hideAjaxLoader();

            if (retorno.success) {

                $scope.nossonumero = retorno.result.nossonumero;

                alert($scope.nossonumero);
            }
            else {

                alert(response.message.message);
            }

        }).error(function (response) {

            alert(response);

            hideAjaxLoader();
        });

    }
    $scope.cancelarContrato = function (_contrato) {

        showAjaxLoader();

        var url = "/Contratos/Cancelamento";
        $http({
            url: url,
            method: "post",
            data: { _ctr: _contrato }
        }).success(function (retorno) {

            hideAjaxLoader();

            if (retorno.success) {

                $scope.buscarContratosAssinatura();

                $scope.buscarHistorico($scope.cliente.CLI_ID);

                $scope.message = response.message;

            }
            else {

                if (response.message != null)
                    $scope.message = response.message;
                else
                    $scope.message = Util.createMessage("fail", response);
                
            }

        }).error(function (response) {

            alert(response);

            hideAjaxLoader();
        });

    }

    $scope.mostrarDetConsulta = function (uraid) {

        showAjaxLoader();

        var url = "/Cliente/BuscarDetConsultaUra";
        $http({
            url: url,
            method: "post",
            data: { _asn_id: $scope.assinturaSelect, _ura_id: uraid }
        }).success(function (retorno) {

            hideAjaxLoader();

            if (retorno.success) {

                $scope.detconsultas = retorno.result.detconsultas;

                angular.forEach($scope.detconsultas, function (obj, index) {
                    obj.HAU_DATA_CADASTRO = $scope.dataAtualFormatada(obj.HAU_DATA_CADASTRO);
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
    $scope.mostrarDetConsultaEmail = function () {

        showAjaxLoader();

        var url = "/Cliente/BuscarDetConsultaEmail";
        $http({
            url: url,
            method: "post",
            data: { _asn_id: $scope.assinturaSelect }
        }).success(function (retorno) {

            hideAjaxLoader();

            if (retorno.success) {

                $scope.detconsultas = retorno.result.detconsultas;

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
    $scope.buscarTipoAtendimento = function () {

        showAjaxLoader();

        $scope.emiteetiqueta = false;
        $scope.atendimento.TIP_ATEND_ID = null;

        var url = "/Cliente/BuscarTipoAtendimento";
        $http({
            url: url,
            method: "post",
            data: { _grupo: $scope.atendimento.TIP_ATEND_GRUPO
                  , _classificacao: $scope.atendimento.CLA_ATEND_ID
            }
        }).success(function (retorno) {

            hideAjaxLoader();

            if (retorno.success) {

                $scope.lstTipoAtendimento = retorno.result.lstTipoAtendimento;
            }
            else {

                alert(response.message.message);
            }

        }).error(function (response) {

            alert(response);

            hideAjaxLoader();
        });

    }

    $scope.realizarAtendimento = function (ass) {

        $scope.assinturaSelect = ass.ASN_NUM_ASSINATURA;
        $scope.totassSelect = ass.ASN_QTDE_CONS_CONTRATO;
        $scope.consassSelect = ass.ASN_QTDE_CONS_UTILIZADA;
        $scope.BuscarInformativo(ass.PRO_ID);

        $scope.preencherGrids(ass);
        $scope.buscarClassificacaoAtend();
        
        angular.element("#selecionarHist").trigger("click");

        angular.element("#selecionartab").trigger("click");

        angular.element("#modal-atendimento").modal();

    }
    $scope.histAtendDetalhe = function (atendselect) {

        $scope.histAtendSelect = atendselect;

        $scope.histAtendSelect.HAT_DATA_HIST = atendselect.HAT_DATA_HIST;

        if (atendselect.HAT_DATA_RESOLUCAO != null)
            $scope.histAtendSelect.HAT_DATA_RESOLUCAO = atendselect.HAT_DATA_RESOLUCAO;

        angular.element("#modal-Hist-Atend").modal();
    }
    $scope.histAgendaDetalhe = function (agendaSelect) {

        $scope.histAgendaSelect = agendaSelect;

        angular.element("#modal-Hist-Agenda").modal();
    }
    $scope.buscarSequenciaProd = function (assinatura) {



        if ($scope.transferencia) {
            if ($scope.transferencia.PRODLETRA != null && $scope.transferencia.PRODLETRA != "" && $scope.transferencia.PRODLETRA.length < 3) {
                //$scope.message = Util.createMessage("fail", "Informe um nome com ao menos 3 caracteres.");
                return;
            }
        }

        showAjaxLoader();

        var _meses = ['A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L'];
        var _dias = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

        var url = "/Cliente/BuscarSequenciaProd";
        $http({
            url: url,
            method: "post",
            data: { _prodletra: $scope.transferencia.PRODLETRA }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.novaassinatura = response.result.novaassinatura;
                conversionService.deepConversion($scope.novaassinatura);

                $scope.transferencia.ASN_NUM_ASSINATURA = $scope.novaassinatura;
                $scope.transferencia.CTR_DATA_FIM_VIGENCIA = $scope.transferencia.CTR_DATA_FIM_VIGENCIA_AUX;

                if ($scope.transferencia.CTR_ANO_VIGENCIA != "9999") {
                   
                    var _anoVigencia = 0;
                    var _letranova = $scope.transferencia.ASN_NUM_ASSINATURA.substring(3,2);
                    var _mesvigencia = _meses.indexOf(_letranova);
                    var _diasvigencia = _dias[_mesvigencia];

                    if ($scope.transferencia.ASN_NUM_ASSINATURA_ANT.substring(3, 2) <= $scope.transferencia.ASN_NUM_ASSINATURA.substring(3, 2)) {

                        _anoVigencia = $scope.transferencia.CTR_ANO_VIGENCIA;

                        var _dtfimvigencia = new Date(($scope.transferencia.CTR_DATA_FIM_VIGENCIA.getFullYear()),
                                                      _mesvigencia, _diasvigencia);

                        $scope.transferencia.CTR_DATA_FIM_VIGENCIA = _dtfimvigencia;
                    }
                    else {

                        var _dtfimvigencia = new Date(($scope.transferencia.CTR_DATA_FIM_VIGENCIA.getFullYear() + 1),
                                                       _mesvigencia, _diasvigencia);

                        $scope.transferencia.CTR_DATA_FIM_VIGENCIA = _dtfimvigencia;
              

                        if ($scope.transferencia.CTR_ANO_VIGENCIA >= now.getFullYear())
                            _anoVigencia = $scope.transferencia.CTR_ANO_VIGENCIA;
                        else
                            _anoVigencia = IntToStr($scope.transferencia.CTR_ANO_VIGENCIA + 1);
                    }

                    $scope.transferencia.CTR_ANO_VIGENCIA = _anoVigencia;

                }

            }
            else {

                $scope.message = response.message;
            }

        }).error(function (response) {

            $scope.message = response.message;

            hideAjaxLoader();
        })


    }
    $scope.mostrarTransfAssinatura = function (assinatura) {

        showAjaxLoader();

        $scope.assinturaSelect = assinatura.ASN_NUM_ASSINATURA;
        $scope.totassSelect = assinatura.ASN_QTDE_CONS_CONTRATO;
        $scope.consassSelect = assinatura.ASN_QTDE_CONS_UTILIZADA;
        $scope.BuscarInformativo(assinatura.PRO_ID);

        $scope.preencherGrids(assinatura);


        var url = "/Cliente/BuscarUltContrato";
        $http({
            url: url,
            method: "post",
            data: { _assinatura: assinatura.ASN_NUM_ASSINATURA }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.transferencia = {};

                $scope.ultimocontrato = response.result.ultimocontrato;
                conversionService.deepConversion($scope.ultimocontrato);

                $scope.transferencia.CTR_NUM_CONTRATO = $scope.ultimocontrato.CTR_NUM_CONTRATO;
                $scope.transferencia.CTR_DATA_INI_VIGENCIA = $scope.ultimocontrato.CTR_DATA_INI_VIGENCIA;
                $scope.transferencia.CTR_DATA_FIM_VIGENCIA = $scope.ultimocontrato.CTR_DATA_FIM_VIGENCIA;
                $scope.transferencia.ASN_NUM_ASSINATURA_ANT = assinatura.ASN_NUM_ASSINATURA;
                $scope.transferencia.CTR_ANO_VIGENCIA = $scope.ultimocontrato.CTR_ANO_VIGENCIA;
                $scope.transferencia.CTR_DATA_FIM_VIGENCIA_AUX = $scope.ultimocontrato.CTR_DATA_FIM_VIGENCIA;

                angular.element("#Modal-Transf-Assinatura").modal();
            }
            else {

                $scope.message = response.message;
            }

        }).error(function (response) {

            $scope.message = response.message;

            hideAjaxLoader();
        })


      
    }
    $scope.mostrarMotivoCancelamento = function (motivo) {

        $scope.motivoCancelamento = {};
        $scope.motivoCancelamento = motivo;

        angular.element("#Modal-Motivo-Cancel").modal();
    }
    $scope.selAssinatura = function (ass) {

        $scope.tabAtiva = 0;
        $scope.assinturaSelect = ass.ASN_NUM_ASSINATURA;
        $scope.totassSelect = ass.ASN_QTDE_CONS_CONTRATO;
        $scope.consassSelect = ass.ASN_QTDE_CONS_UTILIZADA;

        $scope.preencherGrids();
        $scope.buscarSituacaoCliente();

        angular.element("#selagendatab").trigger("click");

        angular.element("#selecionarContrato").trigger("click");


    }
    $scope.carregarAssinatura = function (assinatura) {

        if (assinatura != null && assinatura != "") {
            $scope.tabAtiva = 0;
            $scope.assinturaSelect = assinatura;
            $scope.buscarSituacaoCliente();
        }
        else
            $scope.assinturaSelect = null;


        $scope.preencherGrids();

        angular.element("#selecionarContrato").trigger("click");

        angular.element("#selecionartab").trigger("click");

    }
    $scope.transferirVigencia = function () {

        showAjaxLoader();

        var url = "/Cliente/TransferirVigencia";
        $http({
            url: url,
            method: "post",
            data: { _transf: $scope.transferencia }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                alert(response.message.message);

                url = Util.getUrl("/Cliente/Editar?clienteId=" + $scope.cliente.CLI_ID + "&assinatura=" + $scope.transferencia.ASN_NUM_ASSINATURA);
                post(url);

            }
            else {

                $scope.message = response.message;
            }

        }).error(function (response) {

            $scope.message = response.message;

            hideAjaxLoader();
        })
    }

    $scope.buscarContratosAssinatura = function () {

        showAjaxLoader();

        var url = "/Cliente/BuscarContratosAssinatura";
        $http({
            url: url,
            method: "post",
            data: { _assinatura: $scope.assinturaSelect }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.listacontrato = response.result.listacontrato;

            }
            else {

                alert(response.message.message);
            }

        }).error(function (response) {

            alert(response);

            hideAjaxLoader();
        })

    }
    $scope.listarNfeCliente = function (tab) {

        showAjaxLoader2();

        $scope.tabAtiva = tab;

        var url = "/NotaFiscal/ListarNfeCliente";
        $http({
            url: url,
            method: "post",
            data: {
                _cli_id: $scope.cliente.CLI_ID
            }
        }).success(function (response) {

            hideAjaxLoader2();

            if (response.success) {

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

    $scope.preencherGrids = function (pageRequest) {

        showAjaxLoader();

        var url = "/Cliente/PreencherGrids";
        $http({
            url: url,
            method: "post",
            data: { _assinatura: $scope.assinturaSelect, pagina: pageRequest }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.buscarContratosAssinatura();

                //----------
                $scope.assinatura = response.result.assinatura;
                $scope.assinatura.ativo = 0;

                $scope.totassSelect = $scope.assinatura.ASN_QTDE_CONS_CONTRATO;
                $scope.consassSelect = $scope.assinatura.ASN_QTDE_CONS_UTILIZADA;

                if ($scope.assinatura.ASN_ATIVA == true && $scope.assinatura.ASN_QTDE_CONS_CONTRATO > 0)
                    $scope.assinatura.ativo = 1;
                //----------

                $scope.AssinaturaSenha = response.result.AssinaturaSenha;
                $scope.listaQtdeConsEmail = response.result.listaQtdeConsEmail;

                angular.forEach($scope.listaQtdeConsEmail, function (obj, index) {
                    obj.contratadas = $scope.totassSelect;
                    obj.realizadoura = $scope.consassSelec;
                });

 
                if (response.result.listauras != null)
                    $scope.listauras = response.result.listauras.lista;

                $scope.listaQtdeConsEmail = response.result.listaQtdeConsEmail;
                $scope.listaChequeDevolvido = response.result.listaChequeDevolvido;
                $scope.listaRepresentante = response.result.listaRepresentante;

                angular.forEach($scope.listaQtdeConsEmail, function (obj, index) {
                    obj.contratadas = $scope.totassSelect;
                    obj.realizadoura = $scope.consassSelec;
                });

                angular.forEach($scope.buscarAssinatura, function (obj, index) {
                    obj.CTR_DATA_INI_VIGENCIA = $scope.dataAtualFormatada(obj.CTR_DATA_INI_VIGENCIA);
                    obj.CTR_DATA_FIM_VIGENCIA = $scope.dataAtualFormatada(obj.CTR_DATA_FIM_VIGENCIA);

                    if (obj.CTR_DATA_INI_VIGENCIA <= dataatual && obj.CTR_DATA_FIM_VIGENCIA >= dataatual)
                        obj.SITUACAO = "Vigente";
                    else if (obj.CTR_DATA_INI_VIGENCIA > dataatual)
                        obj.SITUACAO = "Futuro";
                    else
                        obj.SITUACAO = "Encerrado";
                });

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
    $scope.buscarApuracaoConsulta = function () {

        showAjaxLoader();

        var url = "/Cliente/BuscarApuracaoConsulta";
        $http({
            url: url,
            method: "post",
            data: { _assinatura: $scope.assinturaSelect }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.AssinaturaSenha = response.result.AssinaturaSenha;
                $scope.listaQtdeConsEmail = response.result.listaQtdeConsEmail;

                angular.forEach($scope.listaQtdeConsEmail, function (obj, index) {
                    obj.contratadas = $scope.totassSelect;
                    obj.realizadoura = $scope.consassSelec;
                });

            }
            else {

                alert(response.message.message);
            }

        }).error(function (response) {

            alert(response);

            hideAjaxLoader();
        })

    }
    $scope.buscarListaApuracaoConsulta = function (tab) {

        showAjaxLoader();

        $scope.tabAtiva = tab;

        var now = new Date();
        $scope.filtro.mes = now.getMonth();
        $scope.filtro.ano = now.getFullYear();

        var url = "/Cliente/BuscarListaApuracaoConsulta";
        $http({
            url: url,
            method: "post",
            data: { _assinatura: $scope.assinturaSelect}
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.listaApuracaoConsulta = response.result.listaQtdeConsEmail;

                angular.forEach($scope.listaApuracaoConsulta, function (obj, index) {
                    obj.contratadas = $scope.totassSelect;
                    obj.realizadoura = $scope.consassSelec;
                });

            }
            else {

                alert(response.message.message);
            }

        }).error(function (response) {

            alert(response);

            hideAjaxLoader();
        })

    }

    $scope.preencherGridTel = function (pageRequest) {

        showAjaxLoader();

        var url = "/Cliente/preencherGridTel";
        $http({
            url: url,
            method: "post",
            data: { _assinatura: $scope.assinturaSelect, pagina: pageRequest }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.listatelefone = response.result.listatelefone;

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
    $scope.buscarHistorico = function (_clienteid) {

        showAjaxLoader();

        var url = "/Cliente/BuscarHistorico";
        $http({
            url: url,
            method: "post",
            data: { _cli_id: _clienteid }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.listaagenda = response.result.listaagenda;
                $scope.listaatend = response.result.listaatend;

            }
            else {

                alert(response.message.message);
            }

        }).error(function (response) {

            alert(response);

            hideAjaxLoader();
        })

    }
    $scope.BuscarInformativo = function (_produto) {

        showAjaxLoader();

        var url = "/Cliente/BuscarInformativo";

        $http({
            url: url,
            method: "post",
            data: { _pro_id: _produto }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {
                $scope.listainformativos = response.result.listainformativos;
            }
            else {

                alert(response.message.message);
            }

        }).error(function (response) {

            alert(response);

            hideAjaxLoader();
        })

    }
    $scope.initRepId = function (repId) {

        $scope.REP_ID = repId;
    }

    $scope.buscarSituacaoCliente = function () {

        showAjaxLoader();

        var url = Util.getUrl("/cliente/BuscarSituacaoCliente");

        $http({
            url: url,
            method: "post",
            data: { _asn_id: $scope.assinturaSelect }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {
                $scope.SituacaoCliente = response.result.SituacaoCliente;
            }
            else {

                alert(response.message.message);
            }

        }).error(function (response) {

            alert(response);

            hideAjaxLoader();
        });

    }


    $scope.read = function (clienteId, infoMarketing, edicao, validarCPF_CNPJ) {

        if (validarCPF_CNPJ)
            validarCPF_CNPJ = (validarCPF_CNPJ == "1");

        showAjaxLoader();
        $scope.edicao = edicao;
        if (clienteId) {
            var url = ""

            // a url de franquia traz além dos dados de cliente os dados de suas informações de marketing
            if (infoMarketing) {

                url = Util.getUrl("/franquia/clientes/recuperardadosdocliente");
            }
            else {
                url = Util.getUrl("/cliente/recuperardadosdocliente");
            }

            $http({
                url: url,
                method: "post",
                data: { clienteId: clienteId }
            }).success(function (response) {

                hideAjaxLoader();

                if (response.success == true) {
                    $scope.cliente = response.result.cliente;
                    //  $scope.SituacaoCliente = response.result.SituacaoCliente;
                    $scope.cliente.ValidarCPF_CNPJ = validarCPF_CNPJ;
                    $scope.cliente.DATA_ALTERA = $scope.dataAtualFormatada($scope.cliente.DATA_ALTERA);
                    $scope.cliente.DATA_CADASTRO = $scope.dataAtualFormatada($scope.cliente.DATA_CADASTRO);
                    $scope.cliente.DATA_ULTIMO_HISTORICO = $scope.dataAtualFormatada($scope.cliente.DATA_ULTIMO_HISTORICO);

                    angular.forEach($scope.cliente.CLIENTES_ENDERECO, function (obj, index) {
                        obj.DATA_ALTERA = $scope.dataAtualFormatada(obj.DATA_ALTERA);
                        obj.DATA_CADASTRO = $scope.dataAtualFormatada(obj.DATA_CADASTRO);
                    });

                    if (!infoMarketing) {
                        $scope.listaagenda = response.result.listaagenda;
                        $scope.listaatend = response.result.listaatend;
                    }
                }
                else {

                    alert(response.message.message);
                }

            }).error(function (response) {

                alert(response);

                hideAjaxLoader();
            });

        }
        else {
            $scope.cliente = {
                ValidarCPF_CNPJ: validarCPF_CNPJ
            };

            hideAjaxLoader();
        }


    }

    $scope.acaoEnd = { acao: 0, label: 'Incluir' };

    //-------------------------------------------------------Fim Inicializações ---------------------------------------------

    /*
    *-------------------------------------------------------- Ações --------------------------------------------------------
    */

    $scope.GravarAtendimento = function (_atendimento) {


        _atendimento.CLI_ID = $scope.cliente.CLI_ID;
        _atendimento.ASN_NUM_ASSINATURA = $scope.assinturaSelect;
        _atendimento.HAT_IMP_ETIQUETA = $scope.emiteetiqueta;

        var lbletiqueta = "";

        var dtatual = new Date();
        var colecionador = "";
        var prefixo = "";

        showAjaxLoader();

        var url = "/cliente/GravarAtendimento";
        $http({
            url: url,
            method: "post",
            data: JSON.stringify(_atendimento)
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.buscarHistorico($scope.cliente.CLI_ID);

                alert(response.message.message);

                angular.element("#selecionarHist").trigger("click");

            }
            else {

                alert(response.message.message);
            }

        }).error(function (response) {

            alert(response);
            hideAjaxLoader();
        })
    }

    $scope._salvar = function (url, emitirPedido) {

        var cliente = angular.copy($scope.cliente);
        cliente.ValidarTelefoneEEMail = true;

        if (cliente && cliente.INFO_MARKETING) {

            if (cliente.INFO_MARKETING.AREA_INFO_MARKETING) {

                angular.forEach(cliente.INFO_MARKETING.AREA_INFO_MARKETING, function (value, index) {

                    value.AREAS = null;
                });
            }

            if (cliente.INFO_MARKETING.PRODUTO_COMPOSICAO_INFO_MARKETING) {

                angular.forEach(cliente.INFO_MARKETING.PRODUTO_COMPOSICAO_INFO_MARKETING, function (value, index) {

                    value.PRODUTO_COMPOSICAO = null;
                });
            }
        }

        $scope.clienteParaSalvar = cliente;

        formHandlerService.submit($scope, {
            url: Util.getUrl(url),
            objectName: 'clienteParaSalvar',
            //defaultSuccess: {
            //    successMessage: 'Salvo com sucesso!!!',
            //    redirectUrl: Util.getUrl("/franquia/clientes/buscarClientes"),
            //},
            //onunsuccess: function () {

            //    $scope.button = 'save';
            //},
            success: function (resp, status, config, message, validationMessage) {
                $scope.message = message;
                $scope.erros = validationMessage;

                if (resp.success) {

                    if (Util.isPathValid($scope, "clienteParaSalvar.CLI_ID") && emitirPedido == true) {

                        $scope.emitirPedidoMesmaJanela($scope.clienteParaSalvar.CLI_ID);
                        return;
                    }

                    //alert("Salvo com sucesso.");
                    window.location = Util.getUrl("/franquia/clientes/BuscarClientes");
                }
                else {
                    $scope.button = 'save';
                }

            }
        });
    }

    $scope.salvarClienteFranquia = function (salvar) {

        if (salvar == 1) {

            var url = "/franquia/suspect/salvar";
        }
        else {
            var url = "/franquia/clientes/salvar";
        }

        $scope._salvar(url);
    }
    $scope.salvarClienteEEmitirPedido = function () {

        var url = "/franquia/clientes/salvar";
        $scope._salvar(url, true);
    }
    $scope.salvarClienteFranquiaGerente = function () {

        var url = "/franquia/franquiado/salvar";
        $scope._salvar(url);
    }

    $scope.salvarTelefone = function (telefone) {

        showAjaxLoader();

        var url = "/Cliente/SalvarTelefone";

        $http({
            url: url,
            method: "Post",
            data: { _telefone: telefone }
        }).success(function (response) {

            hideAjaxLoader();

            $scope.message = response.message;
            $scope.erros = response.validationMessage;
            //$scope.abrirModalTelefone(telefone.ASN_NUM_ASSINATURA, false);
            $scope.buscaTelAssinatura($scope.page.pagina);

            //if (response.success == true) {
            //    url = Util.getUrl("/Cliente/Editar?clienteId=" + $scope.cliente.CLI_ID + "&assinatura=" + $scope.assinturaSelect);
            //    post(url);
            //}


        }).error(function (response) {

            alert(response);

            hideAjaxLoader();
        })
    }


    $scope.salvarEmail = function (email,) {

        showAjaxLoader();

        var url = "/Cliente/SalvarEmail";

        email.ASN_NUM_ASSINATURA = $scope.AssinaturaModal;
        email.CLI_ID = $scope.cliente.CLI_ID;

        $http({
            url: url,
            method: "Post",
            data: { _email: email }
        }).success(function (response) {

            hideAjaxLoader();

            $scope.message = response.message;
            $scope.erros = response.validationMessage;
            $scope.abrirModalEmail(email.ASN_NUM_ASSINATURA, false);

            //if (response.success == true) {
            //    url = Util.getUrl("/Cliente/Editar?clienteId=" + $scope.cliente.CLI_ID + "&assinatura=" + $scope.assinturaSelect);
            //    post(url);
            //}


        }).error(function (response) {

            alert(response);

            hideAjaxLoader();
        })
    }
    $scope.SalvarCliente = function () {

        showAjaxLoader();

        var url = "/Cliente/Salvar";

        $http({
            url: url,
            method: "Post",
            data: { _cliente: $scope.cliente, _assinatura: $scope.assinturaSelect, _telefones: $scope.listatelefone, _emails: $scope.listaemail }
        }).success(function (response) {

            hideAjaxLoader();

            $scope.message = response.message;
            $scope.erros = response.validationMessage;

            if (response.success == true) {
                url = Util.getUrl("/Cliente/Editar?clienteId=" + $scope.cliente.CLI_ID + "&assinatura=" + $scope.assinturaSelect);
                post(url);
            }


        }).error(function (response) {

            alert(response);

            hideAjaxLoader();
        })

    }
    $scope.listar = function (pageRequest) {

        if ($scope.filtro) {
            if ($scope.filtro.nome != null && $scope.filtro.nome != "" && $scope.filtro.nome.length < 3) {
                $scope.message = Util.createMessage("fail", "Informe um nome com ao menos 3 caracteres.");
                return;
            }

            if (!$scope.filtro.cliente &&
                !$scope.filtro.contrato &&
                !$scope.filtro.pedido &&
                !$scope.filtro.cnpj &&
                !$scope.filtro.nome &&
                !$scope.filtro.assinatura &&
                !$scope.filtro.logradouro) {
                $scope.message = Util.createMessage("fail", "Escolha pelo menos um filtro antes de pesquisar.");
                return;
            }
        }
        else {

            $scope.message = Util.createMessage("fail", "Escolha pelo menos um filtro antes de pesquisar.");
            return;
        }

        showAjaxLoader();

        var url = "/Cliente/Clientes";

        $http({
            url: url,
            method: "Post",
            data: { cliente: $scope.filtro.cliente, contrato: $scope.filtro.contrato, pedido: $scope.filtro.pedido, assinatura: $scope.filtro.assinatura, cnpj: $scope.filtro.cnpj, nome: $scope.filtro.nome, pagina: pageRequest, somenteativos: $scope.filtro.somenteativos }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {
                $scope.clientes = response.result.clientes;
                conversionService.deepConversion($scope.clientes);
                $scope.page = response.page;
            }
            else {
                $scope.clientes = null;
                $scope.message =  response.message;
            }

        }).error(function (response) {

            $scope.clientes = null;
            $scope.message = response.message;

            hideAjaxLoader();
        })

    };

    $scope.abrirEdicaoItem = function (index, item) {

        $scope.TipoAcaoTel = { label: 'Alterar', valor: 1, index: index };
        var telefoneObj = angular.copy(item);
        $scope.telefone = telefoneObj;
    }

    // ---- Manutenção Lista Telefones
    $scope.IncluirTelefone = function (tel) {

        if (tel) {
            if (tel.TIPO_TEL_ID == null || tel.TIPO_TEL_ID == "") {
                $scope.message = Util.createMessage("fail", "Preencha o tipo de telefone!");
                return;
            }

            if (tel.ATE_TELEFONE == null || tel.ATE_TELEFONE == "") {
                $scope.message = Util.createMessage("fail", "Preencha o telefone!");
                return;
            }
        }

        if ($scope.listatelefone) {
            novo = {};
            novo.ASN_NUM_ASSINATURA = $scope.assinturaSelect;
            $scope.listatelefone.push(novo);
        }
    }
    $scope.excluirTelefoneAssinatura = function (telefone, index) {

        if (!confirm("Deseja realamente excluir este telefone?"))
            return;

        showAjaxLoader();

        var url = "/Cliente/ExcluirTelefone";

        $http({
            url: url,
            method: "Post",
            data: { _telefone: telefone }
        }).success(function (response) {

            hideAjaxLoader();

            $scope.message = response.message;
            $scope.erros = response.validationMessage;
            $scope.abrirModalTelefone(telefone.ASN_NUM_ASSINATURA, false);
            $scope.listatelefone.splice(index, 1);

            //if (response.success == true) {
            //    url = Util.getUrl("/Cliente/Editar?clienteId=" + $scope.cliente.CLI_ID + "&assinatura=" + $scope.assinturaSelect);
            //    post(url);
            //}


        }).error(function (response) {

            alert(response);

            hideAjaxLoader();
        })


        

    }
    // ---- Manutenção Lista Emails
    $scope.IncluirEmail = function (email) {

        if (email && (email.AEM_EMAIL == null || email.AEM_EMAIL == "")) {

            $scope.message = Util.createMessage("fail", "Preencha corretamente o email antes de adicionar mais uma linha.");
            return;
        }
        //if ($scope.listatelefone && $scope.prospect.PROSPECTS_TELEFONE) {
        if ($scope.listaemail) {
            novo = {};
            novo.ASN_NUM_ASSINATURA = $scope.assinturaSelect;
            $scope.listaemail.push(novo);
        }
    }
    $scope.excluirEmailAssinatura = function (email,index) {

        if (!confirm("Deseja realamente excluir este email?"))
            return;

        showAjaxLoader();

        var url = "/Cliente/ExcluirEmail";

        $http({
            url: url,
            method: "Post",
            data: { _email: email }
        }).success(function (response) {

            hideAjaxLoader();

            $scope.message = response.message;
            $scope.erros = response.validationMessage;
            $scope.abrirModalEmail(email.ASN_NUM_ASSINATURA, false);
            $scope.listaemail.splice(index, 1);

            //if (response.success == true) {
            //    url = Util.getUrl("/Cliente/Editar?clienteId=" + $scope.cliente.CLI_ID + "&assinatura=" + $scope.assinturaSelect);
            //    post(url);
            //}


        }).error(function (response) {

            alert(response);

            hideAjaxLoader();
        })

       
    }
    //---------------------------------
    $scope.AbrirModalParcelas = function (parcela) {

        showAjaxLoader();

        var url = "/Cliente/BuscarParcelas";

        $http({
            url: url,
            method: "post",
            data: { _numcontrato: parcela.CTR_NUM_CONTRATO }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.listaparcelas = response.result.listaparcelas;

                angular.forEach($scope.listaparcelas, function (obj, index) {

                    obj.PAR_DATA_VENCTO = $scope.dataAtualFormatada(obj.PAR_DATA_VENCTO);

                    if (obj.PAR_DATA_ALOC != null) {
                        obj.PAR_DATA_ALOC = $scope.dataAtualFormatada(obj.PAR_DATA_ALOC);
                    }

                    if (obj.DATA_ALTERA != null) {
                        obj.DATA_ALTERA = $scope.dataAtualFormatada(obj.DATA_ALTERA);
                    }
                    if (obj.DATA_PRORROGACAO != null) {
                        obj.DATA_PRORROGACAO = $scope.dataAtualFormatada(obj.DATA_PRORROGACAO);
                    }

                    if (obj.PAR_DATA_PAGTO != null) {
                        obj.PAR_DATA_PAGTO = $scope.dataAtualFormatada(obj.PAR_DATA_PAGTO);
                        obj.SITUACAO = "Pago";
                    }
                    else if (obj.PAR_DATA_VENCTO > dataatual)
                        obj.SITUACAO = "A Vencer";
                    else if (obj.PAR_DATA_VENCTO <= dataatual)
                        obj.SITUACAO = "Vencida";
                });

                angular.element("#modal-parcelas").modal();
            }
            else {

                $scope.message = response.message;
            }

        }).error(function (response) {

            $scope.message = response.message;

            hideAjaxLoader();
        })

    }


    $scope.AbrirModalLiquidacao = function (parcela) {

        showAjaxLoader();

        var url = "/Cliente/BuscarLiqParcelas";

        $http({
            url: url,
            method: "post",
            data: { _numparcela: parcela.PAR_NUM_PARCELA }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.listaliqparcelas = response.result.listaliqparcelas;

                angular.element("#modal-Liquidacao").modal();
            }
            else {

                $scope.message = response.message;

            }

        }).error(function (response) {

            $scope.message = response.message;

            hideAjaxLoader();

        })

    }
    $scope.AbrirModalCHDevolvido = function (parcela) {

        showAjaxLoader();

        var url = "/Cliente/BuscarCHDevolvido";

        $http({
            url: url,
            method: "post",
            data: { _numassinatura: parcela.ASN_NUM_ASSINATURA }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.listachdevolvido = response.result.listachdevolvido;

                angular.element("#modal-Cheque-Devolvido").modal();
            }
            else {

                $scope.message = response.message;
            }

        }).error(function (response) {

            alert(response);

            hideAjaxLoader();
        })

    }
    $scope.validarPermissao = function (url, acesso) {

        $http({
            url: "/Home/ValidaPermissao",
            method: "post",
            data: { _url: url, _acesso: acesso }
        }).success(function (response) {

            $scope.Incluir = response.success;

        }).error(function (response) {

            $scope.Editar = false;

        })

    }


    //---- Funções para buscar clientes que ja estão  na URA ---// 
    $scope.mostrarHistAgenda = function (_assinatura,tab) {

        showAjaxLoader();

        $scope.tabAtiva = tab;

        $http({
            url: "/Cliente/BuscarHistAgenda",
            method: "Post",
            dataType: 'json',
            data: { _asn_id: _assinatura }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.listaagenda = response.result.listaagenda;
 
            }
            else {
                alert(response.message.message);
            }

        }).error(function (response) {

            alert(response);

            hideAjaxLoader();
        })

    };

    $scope.listarClientesRepresentante = function (pageRequest) {

        $scope.message = null;

        var url = Util.getUrl("/franquia/clientes/clientesporrepresentante");

        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }

        $scope.listado = false;
        var config = {
            url: url,
            targetObjectName: 'clientes',
            responseModelName: 'clientes',
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

    $scope.buscarClientes = function (pageRequest) {

        $scope.message = null;
        $scope.listado = false;
        var url = Util.getUrl("/franquia/clientes/BuscarClientes");

        if (pageRequest) {
            url += "?pagina=" + pageRequest;
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
            if ($scope.filtro.cpf_cnpj || $scope.filtro.nome ||
                    $scope.filtro.classificacaoClienteId || $scope.filtro.email || $scope.filtro.REP_ID
                || $scope.filtro.telefone || $scope.filtro.CLI_ID || $scope.filtro.origemId || $scope.filtro.excluidosDaValidacao) {

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
        }
        formHandlerService.read($scope, config);
    };
    //-------------------------------------------------------Fim Ações -----------------------------------------------------------------------



    $scope.cadastrarClienteComBaseNosFiltros = function () {

        var urlParams = "?";

        if ($scope.filtro) {

            var possuiAlgum = false;

            if ($scope.filtro.cpf_cnpj) {

                urlParams += "cpf_cnpj=" + $scope.filtro.cpf_cnpj;
                possuiAlgum = true;
            }

            if ($scope.filtro.nome) {

                if (possuiAlgum) {

                    urlParams += "&";
                }

                urlParams += "nome=" + $scope.filtro.nome;
                possuiAlgum = true;
            }

            if ($scope.filtro.email) {

                if (possuiAlgum) {

                    urlParams += "&";
                }

                urlParams += "email=" + $scope.filtro.email;
                possuiAlgum = true;
            }

            if ($scope.filtro.telefone) {

                if (possuiAlgum) {

                    urlParams += "&";
                }

                urlParams += "telefone=" + $scope.filtro.telefone;
                possuiAlgum = true;
            }


            if ($scope.filtro.dddTelefone) {

                if (possuiAlgum) {

                    urlParams += "&";
                }

                urlParams += "dddTelefone=" + $scope.filtro.dddTelefone;
                possuiAlgum = true;
            }
            if ($scope.filtro.AREA_ID) {

                if (possuiAlgum) {

                    urlParams += "&";
                }

                urlParams += "AREA_ID=" + $scope.filtro.AREA_ID;
                possuiAlgum = true;
            }

            if ($scope.filtro.CMP_ID) {

                if (possuiAlgum) {

                    urlParams += "&";
                }

                urlParams += "CMP_ID=" + $scope.filtro.CMP_ID;
                possuiAlgum = true;
            }

            var url = Util.getUrl("/franquia/suspect/novo");
            url += urlParams;
            post(url);
        }


    }
    /*
     *--------------------------------------------------------Chamadas internas ----------------------------------------------------------------------
     */
    $scope.ProcessarInclusaoTelefone = function () {

        if ($scope.TipoAcaoTel) {

            if ($scope.TipoAcaoTel.valor === 1) {
                var index = $scope.TipoAcaoTel.index;
                $scope.prospect.PROSPECTS_TELEFONE[index] = $scope.telefone;
                $scope.ResetarTelefone();

            }
            else
                if ($scope.TipoAcaoTel.valor === 0) {

                    $scope.prospect.PROSPECTS_TELEFONE.push($scope.telefone);
                    $scope.ResetarTelefone();
                }
        }

    }

    $scope._desabilitarDemaisTelefones = function (tel) { // desabilitar outras edições abertas excluindo o telefone passado

        if ($scope.prospect && $scope.prospect.PROSPECTS_TELEFONE) {

            angular.forEach($scope.prospect.PROSPECTS_TELEFONE, function (value) {

                if (value != tel) {

                    value.editarTipo = false;
                    value.editarTel = false;
                }
            });


        }
    }

    $scope.adicionarEmail = function () {

        var lista = $scope.cliente.ASSINATURA_EMAIL;

        if (lista.length > 0) {

            var value = lista[lista.length - 1];
            if (value && value.AEM_EMAIL) {

                $scope.cliente.ASSINATURA_EMAIL.push({ CLI_ID: $scope.cliente.CLI_ID });
            }
            else {

                $scope.message = Util.createMessage("fail", "Não é possível adicionar mais uma linha até que a linha anterior esteja correta.");
            }
        }
        else {

            $scope.cliente.ASSINATURA_EMAIL.push({ CLI_ID: $scope.cliente.CLI_ID });

        }
    };

    $scope.removerEmail = function ($index) {

        $scope.cliente.ASSINATURA_EMAIL.splice($index, 1);

    };

    $scope.$watch("cliente", function (cli) {

        if (cli) {

            if (!cli.ASSINATURA) {

                cli.ASSINATURA = [{

                    ASSINATURA_EMAIL: [],
                    ASSINATURA_TELEFONE: [],
                    UEN_ID: 1
                }];
            }
        }
    });


    $scope.carregaRepresentanteAutoCompleteDTO = function () {

        if (!$scope.lstRepAutocomplete) {
            var url = Util.getUrl('/franquia/representante/ListarRepresentantesAutoComplete');
            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'lstRepAutocomplete',
                responseModelName: 'lstRepresentanteAutocompleteDTO',
                success: function () {

                }
            });
        }
    }

    $scope.validarAutoComplete = function () {

        $scope.repInvalido = (!$scope.filtro.REP_ID && $scope.filtro.representante);
    }

    $scope.selecItem = function (_assinatura) {

        $scope.filtro.assinatura = _assinatura;

        $scope.listar();
    }

    $scope.buscarUsuarioLogado = function (item) {

        item.ALTERADO = true;

    }


    $scope.pesquisarEmail = function () {

        if ($scope.filtro.Email.length != null && $scope.filtro.Email.length > 3) {

            showAjaxLoader();

            $http({
                method: "Post",
                dataType: "json",
                url: "/Cliente/BuscarEmail",
                data: { _email: $scope.filtro.Email }
            }).success(function (response) {

                hideAjaxLoader();

                $scope.dbEmail = response.result.lstemail;

            }).error(function (response) {

                $scope.message = Util.createMessage("fail", response);

                hideAjaxLoader();

            });
        }

    }

    $scope.pesquisarTelefone = function () {

        if ($scope.filtro.Telefone.length != null && $scope.filtro.Telefone.length > 3) {

            showAjaxLoader();

            $http({
                method: "Post",
                dataType: "json",
                url: "/Cliente/BuscarTelefone",
                data: { _telefone: $scope.filtro.Telefone }
            }).success(function (response) {

                hideAjaxLoader();

                $scope.dbTelefone = response.result.lsttelefone;

            }).error(function (response) {

                $scope.message = Util.createMessage("fail", response);

                hideAjaxLoader();

            });
        }


    }
    //------------

    $scope.ExcluirEndereco = function (end, index) {

        if (!confirm("Deseja exluir este endereço ?"))
            return;

        $scope.endexcluido = end;

        showAjaxLoader();

        $http({
            method: "Post",
            dataType: "json",
            url: "/Cliente/ExcluirEndereco",
            data: { _endereco: end }
        }).success(function (response) {

            if (response.success == true) {
                $scope.message = Util.createMessage("success", "Registro excluído com sucesso !!!");
             
                if (response.success == true) {
                    url = Util.getUrl("/Cliente/Editar?clienteId=" + $scope.endexcluido.CLI_ID + "&assinatura=" + $scope.assinturaSelect);
                    post(url);
                }
            }
            else {
                $scope.message = Util.createMessage("fail", "Seu usuário não possui permissão para esta funcionalidade.");
            }

            hideAjaxLoader();

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);

            hideAjaxLoader();

        });

    }
    //------------
    $scope.AlterarEndereco = function (end, index) {

        showAjaxLoader();

        $http({
            method: "Post",
            dataType: "json",
            url: "/Cliente/AlterarEndereco"
        }).success(function (response) {

            if (response.success == true) {
                $scope.AbrirModalEndereco(end, index);
            }
            else {
                $scope.message = Util.createMessage("fail", "Seu usuário não possui permissão para esta funcionalidade.");
            }

            hideAjaxLoader();

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);

            hideAjaxLoader();

        });

    }

    //------------

    $scope.pesquisarLogradouro = function () {

        if ($scope.filtro.Logradouro.length != null && $scope.filtro.Logradouro.length > 3) {

            showAjaxLoader();

            $http({
                method: "Post",
                dataType: "json",
                url: "/Cliente/BuscarLogradouro",
                data: { _logradouro: $scope.filtro.Logradouro }
            }).success(function (response) {

                hideAjaxLoader();

                $scope.dbLogradouro = response.result.lstlogradouro;

            }).error(function (response) {

                $scope.message = Util.createMessage("fail", response);

                hideAjaxLoader();

            });

        }

        angular.element("#modal-Logradouro").modal();
    }

    $scope.abrirPopUpLogradouro = function () {

        $scope.limparParametros();
        angular.element("#modal-Logradouro").modal();

    }
    $scope.abrirPopUpEmail = function () {

        $scope.limparParametros();
        angular.element("#modal-Email").modal();
    }
    $scope.abrirPopUpTelefone = function () {

        $scope.limparParametros();
        angular.element("#modal-Telefone").modal();

    }
    $scope.abrirPopUpSituacao = function (assinatura) {

        $scope.assinturaSelect = assinatura;
        $scope.buscarSituacaoCliente()
        angular.element("#modal-Situacao").modal();

    }


    $scope.limparParametros = function () {

        $scope.filtro.assinatura = null;
        $scope.filtro.contrato = null;
        $scope.filtro.pedido = null;
        $scope.filtro.cnpj = null;
        $scope.filtro.cliente = null;
        $scope.filtro.nome = null;
        $scope.filtro.Logradouro = " ";
        $scope.filtro.Email = " ";
        $scope.filtro.Telefone = " ";

        $scope.dbLogradouro = null;
        $scope.dbTelefone = null;
        $scope.dbEmail = null;
        $scope.clientes = null;


    }















    //-------------------------------------------------------Fim Chamadas internas -----------------------------------------------------------------------


    function dataAtualFormatadatxt(dataHora) {

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
    $scope.dataAtualFormatada = function (dataHora) {

        if (dataHora == null)
            return null;

        var parseDate = new Date(parseInt(dataHora.substr(6)));
        var jsDate = new Date(parseDate);

        return jsDate;
    }

    $scope.carregaRegioesParaCadastroSuspect = function (RG_ID) {

        if (!$scope.lstRegioesParaCadastroSuspect) {
            var url = Util.getUrl('/regiao/regioes');
            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'lstRegioesParaCadastroSuspect',
                responseModelName: 'lstRegiao',
                success: function () {

                    if ($scope.cliente) {
                        $scope.cliente.RegiaoIdParaRodizio = Number(RG_ID);
                    }

                }
            });
        }
    }

    if (window.InfoMarketingController !== undefined) {

        InfoMarketingController($scope, formHandlerService, $http, conversionService);
    }

    if (window.TelefoneController !== undefined) {

        TelefoneController($scope, formHandlerService, $http, conversionService);
    }

    if (window.InfoClienteController !== undefined) {

        InfoClienteController($scope, formHandlerService, $http, conversionService, $timeout);
    }

    if (window.HistoricoClienteController !== undefined) {

        HistoricoClienteController($scope, formHandlerService, $http, conversionService, $timeout);
    }

    if (window.CarteiramentoPluginController !== undefined) {

        CarteiramentoPluginController($scope, formHandlerService, $http, conversionService, $timeout);
    }

    if (window.EnderecoController !== undefined) {

        EnderecoController($scope, formHandlerService, $http, $timeout);
    }


    if (window.BoletoAvulsoController !== undefined) {

        BoletoAvulsoController($scope, formHandlerService, $http, conversionService, $timeout);
    }


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

    $scope.listarAcessoTabelas = function () {

        showAjaxLoader();

        var url = "/AcessoTabelas/PesquisarPorAssinatura";
        $http({
            url: url,
            method: "post",
            data: { _mes: $scope.filtro.mes, _ano: $scope.filtro.ano, _tipo_acesso: "L", _assinatura: $scope.assinturaSelect }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.listaAcessoTabelas = response.result.listaAcessoTabelas;

            }
            else {

                $scope.listaAcessoTabelas = null;

                alert(response.message.message);
            }

        }).error(function (response) {

            alert(response);

            hideAjaxLoader();
        })

    }

    $scope.excluirClienteDaValicacao = function (cliId) {

        if (confirm("Deseja remover esse cliente da validação de email/telefone/cnpj-cpf?")) {
            var url = Util.getUrl("/franquia/clientes/ExcluirClienteDaValidacao");

            $scope.objEnvio = { cliId: cliId };

            formHandlerService.submit($scope, {
                url: url,
                objectName: 'objEnvio',
                success: function (resp, status, config, message, validationMessage) {
                    $scope.message = message;
                    $scope.erros = validationMessage;

                    if (resp.success) {
                        $scope.message = Util.createMessage("success", "Cliente Excluído da validação com sucesso!");

                        $timeout(function () {
                            $scope.message = null;
                        }, 1000);
                    }
                }
            });
        }
    }

    $scope.readProspect = function (clienteId, tipo) {

        tipo = (tipo) ? tipo : 'cliente';

        if (!tipo || tipo == 'cliente') {

            $scope.readProspectCorp(clienteId);
        }
        else if (tipo == 'prospect') {
            $scope.readProspectOriginal(clienteId);
        }
    }

    $scope.readProspectCorp = function (clienteId) {

        if (clienteId) {
            var url = Util.getUrl("/prospect/recuperarDadosDoProspect");

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'prospect',
                responseModelName: 'prospect',
                showAjaxLoader: true,
                data: { clienteId: clienteId },
                success: function () {

                    $timeout(function () {

                        if ($scope.prospect != null) {

                            $scope.prospect.carIdStr = $scope.prospect.CAR_ID;

                        }
                    });
                }
            });
        }
    }


    $scope.readProspectOriginal = function (codigo) {

        if (codigo) {
            var url = Util.getUrl("/prospect/recuperarDadosDoProspectOriginal");

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'prospect',
                responseModelName: 'prospect',
                showAjaxLoader: true,
                data: { codigo: codigo },
                success: function () {

                    $timeout(function () {

                        if ($scope.prospect != null) {

                            $scope.prospect.carIdStr = $scope.prospect.CAR_ID;

                        }
                    });
                }
            });
        }
    }

    $scope.buscarProspectsOriginal = function (pageRequest) {

        $scope.message = null;
        $scope.listado = false;

        var url = Util.getUrl("/prospect/buscarProspect");

        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstProspect',
            responseModelName: 'lstProspect',
            showAjaxLoader: true,
            pageConfig: { pageName: 'page', pageTargetName: 'paginaProspect' },
            data: $scope.filtro,
            success: function (response) {
                $scope.listado = true;

            }
        });
    }

    $scope.buscarProspectsCorp = function (pageRequest) {

        $scope.message = null;

        var url = Util.getUrl("/prospect/buscarProspectCorp");

        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstProspectCorp',
            responseModelName: 'lstProspectCorp',
            showAjaxLoader: true,
            pageConfig: { pageName: 'page' /*, pageTargetName: 'paginaProspect'  */ },
            data: $scope.filtro,
            success: function (response) {
                $scope.listado = true;

            }
        });
    }

    $scope.buscarProspects = function (pageRequest) {

        if ($scope.filtro) {
            if ($scope.filtro.cpf_cnpj || $scope.filtro.nome ||
                    $scope.filtro.classificacaoClienteId || $scope.filtro.email || $scope.filtro.REP_ID
                || $scope.filtro.telefone || $scope.filtro.CLI_ID || $scope.filtro.origemId || $scope.filtro.excluidosDaValidacao) {

                if ($scope.filtro.dddTelefone && !$scope.filtro.telefone) {

                    $scope.message = Util.createMessage("fail", "Preencha o telefone ao preencher o ddd");
                    return;
                }
            }
            else {

                $scope.message = Util.createMessage("fail", "Preenche pelo menos um filtro");
                return;
            }
        }

        $scope.buscarProspectsOriginal(1);
        $scope.buscarProspectsCorp(1);

    };


    $scope.mudarTab = function (tab) {
        $scope.tab = tab;
    }

    $scope.buscarCodigosCarteiramento = function (carDescricao) {

        if (carDescricao) {
            var url = Util.getUrl("/prospect/buscarCodigosCarteiramento");

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'lstCarteiramentos',
                responseModelName: 'lstCarteiramentos',
                showAjaxLoader: true,
                data: { carDescricao: carDescricao },
                success: function () {
                }
            });
        }
    }

    $scope.salvarProspect = function () {

        var url = Util.getUrl("/prospect/salvarClienteEProspect");

        formHandlerService.submit($scope, {
            url: url,
            objectName: 'prospect',
            success: function (resp, status, config, message, validationMessage) {
                $scope.message = message;
                $scope.erros = validationMessage;

                $scope.button = 'reset';

                if (resp.success) {

                    $scope.message = Util.createMessage("success", "Salvo com sucesso!");
                    $timeout(function () {

                        $scope.message = null;
                        window.location = Util.getUrl("/prospect/buscar");

                    }, 1000);
                }

            }
        });
    }

});

