appModule.controller('ClienteController', function ($scope, formHandlerService, $http, conversionService, cepService) {

    $scope.filtro = {};
    $scope.filtro.URA_ID = null;
    $scope.assinturaSelect = null;
    $scope.totassSelect = null;
    $scope.consassSelect = null;
 

    /// Pega data do sistema.

    var now = new Date();
    var dataatual = new Date(now.getFullYear(), now.getMonth(), now.getDate());

    /*
    *-------------------------------------------------------- Inicializações ----------------------------------------------------------------------
    */

    $scope.init = function () {

        $scope.TipoAcaoTel = { label: 'Incluir', valor: 0 };
        $scope.prospect = { PROSPECTS_TELEFONE: [] };
        $scope.end = {};
        $scope.iniciarCombos();
    }


    $scope.iniciarCombos = function () {

        var url = "/endereco/combosendereco";
        $http({
            url: url,
            method: "post"
        }).success(function (retorno) { 

            $scope.tipoLogradouro  = retorno.result.tipoLogradouro;
            $scope.tipoEnderecos = retorno.result.tipoEnderecos;
            $scope.ListaClassificacao = retorno.result.ListaClassificacao;
   
        });

    }
    
    $scope.LiberarDocVencido = function (parcela) {

        if (confirm("Confirmar o desbloqueio ?")) {

            if (parcela.PAR_SITUACAO == "NOR")
               parcela.PAR_SITUACAO = "LIB";
            else
               parcela.PAR_SITUACAO = "NOR";

            var url = "/Parcelas/Salvar";
            $http({
                url: url,
                method: "post",
                data: {_parcela : parcela}
            }).success(function (response) {

                hideAjaxLoader();

                if (response.success == true) {

                    alert(response.message.message);
                }
                else {

                    alert(response.message.message);
                }

            }).error(function (response) {

                alert(response);

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
    $scope.realizarAtendimento = function (ass) {

        $scope.assinturaSelect = ass.ASN_NUM_ASSINATURA;
        $scope.totassSelect = ass.ASN_QTDE_CONS_CONTRATO;
        $scope.consassSelect = ass.ASN_QTDE_CONS_UTILIZADA;
        $scope.BuscarInformativo(ass.PRO_ID);

        $scope.preencherGrids(ass);

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
    $scope.selAssinatura = function (ass) {

        $scope.assinturaSelect = ass.ASN_NUM_ASSINATURA;
        $scope.totassSelect = ass.ASN_QTDE_CONS_CONTRATO;
        $scope.consassSelect = ass.ASN_QTDE_CONS_UTILIZADA;

        $scope.preencherGrids();

        angular.element("#selecionarHist").trigger("click");

        angular.element("#selecionartab").trigger("click");

    }
    $scope.preencherGrids = function (pageRequest) {

        showAjaxLoader();

        var url = "/Cliente/PreencherGrids";
        $http({
            url: url,
            method: "post",
            data: { _assinatura: $scope.assinturaSelect, pagina: pageRequest}
        }).success(function (response) {
            
            hideAjaxLoader();

            if (response.success == true) {

                $scope.listacontrato = response.result.listacontrato;
                $scope.listaemail    = response.result.listaemail;
                $scope.listatelefone = response.result.listatelefone;

                if (response.result.listauras != null)
                    $scope.listauras = response.result.listauras.lista;

                if (response.result.listaagenda != null)
                    $scope.listaagenda = response.result.listaagenda;

                $scope.listaQtdeConsEmail = response.result.listaQtdeConsEmail;

                angular.forEach($scope.listaQtdeConsEmail, function (obj, index) {
                    obj.contratadas = $scope.totassSelect;
                    obj.realizadoura =  $scope.consassSelec;
                });

                $scope.listaatend = response.result.listaatend;

                angular.forEach($scope.listacontrato, function (obj, index) {
                    obj.CTR_DATA_INI_VIGENCIA = dataAtualFormatada(obj.CTR_DATA_INI_VIGENCIA);
                    obj.CTR_DATA_FIM_VIGENCIA = dataAtualFormatada(obj.CTR_DATA_FIM_VIGENCIA);

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
    $scope.buscarHistorico = function (assinatura) {

        showAjaxLoader();

        var url = "/Cliente/BuscarHistorico";
        $http({
            url: url,
            method: "post",
            data: { _assinatura: assinatura }
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
    
    $scope.read = function (clienteId) {

        if (clienteId) {

            formHandlerService.read($scope, {
                url: Util.getUrl("/cliente/recuperardadosdocliente"),
                targetObjectName: 'cliente',
                responseModelName: 'cliente',
                data: { clienteId: clienteId },
                success: function () {
                    $scope.cliente.ENDERECO_ENTREGA = null;
                    $scope.cliente.ENDERECO_FATURAMENTO = null;
                }
            });
        }


    }
    $scope.acaoEnd = { acao: 0, label: 'Incluir' };

    //-------------------------------------------------------Fim Inicializações ---------------------------------------------

    /*
    *-------------------------------------------------------- Ações --------------------------------------------------------
    */

    $scope.GravarAtendimento = function (_atendimento) {

        _atendimento.ASN_NUM_ASSINATURA = $scope.assinturaSelect;
        
        var lbletiqueta = "";

        switch(_atendimento.TIP_ATEND_ID) {
            case 1:
                lbletiqueta = _atendimento.Remessa01 + ' A ' + _atendimento.Remessa02 + '-' + _atendimento.colecionador + '-' + _atendimento.Estado;
                break;
            case 2:
                lbletiqueta = "PST="+_atendimento.Pasta+" ANO="+_atendimento.AnoPasta+"-"+_atendimento.Estado;
                break;
            case 3:
                lbletiqueta = "PST="+_atendimento.Pasta+"-"+_atendimento.colecionador+" ANO="+_atendimento.AnoPasta+"-"+_atendimento.Estado;
                break;
            case 4:
                lbletiqueta = "ALTERACAO DE CARACTERISTICAS - "+_atendimento.CampoAlteracao;
                break;
            case 5:
                lbletiqueta = atendimento.Motivo;
                break;
            case 6:
                lbletiqueta = "";
                break;
        }

        _atendimento.HAT_TEXTO_ETIQUETA = lbletiqueta;
        _atendimento.HAT_DESCRICAO = _atendimento.HAT_DESCRICAO + lbletiqueta;

        showAjaxLoader();

        var url = "/cliente/GravarAtendimento";
        $http({
            url: url,
            method: "post",
            data: JSON.stringify(_atendimento)
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.buscarHistorico($scope.assinturaSelect);

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

    $scope.salvarClienteFranquia = function () {

        formHandlerService.submit($scope, {
            url: Util.getUrl("/franquia/clientes/salvar"),
            objectName: 'cliente',
            success: function (resp, status, config, message, validationMessage) {
                $scope.message = message;
                $scope.erros = validationMessage;

                if (resp.success) {
                    alert("Salvo com sucesso.");
                    window.location = Util.getUrl("/franquia/clientes");
                }
            }
        });
    }
    $scope.SalvarCliente = function () {

        showAjaxLoader();

        var url = "/Cliente/Salvar";

        $http({
            url: url,
            method: "Post",
            data: { _cliente: $scope.cliente, _assinatura: $scope.assinturaSelect,  _telefones : $scope.listatelefone, _emails: $scope.listaemail }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {
                alert(response.message.message);
                window.location = Util.getUrl("/Cliente/index");
            }
            else {

                alert(response.message.message);
            }

        }).error(function (response) {

            alert(response);

            hideAjaxLoader();
        })

    }
    $scope.listar = function (pageRequest) {

        $scope.message = null;
        var url = Util.getUrl("/cliente/clientes");

        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }

        var config = {
            url: url,
            targetObjectName: 'clientes',
            responseModelName: 'clientes',
            showAjaxLoader: true,
            pageConfig: { pageName: 'page' }
        };

        if ($scope.filtro) {
            if (!$scope.filtro.cnpj && !$scope.filtro.nome && !$scope.filtro.assinatura && !$scope.filtro.logradouro) {
                $scope.message = Util.createMessage("fail", "Escolha pelo menos um filtro antes de pesquisar.");
                return;
            }
            config.data = angular.copy($scope.filtro);
        }
        else {

            $scope.message = Util.createMessage("fail", "Escolha pelo menos um filtro antes de pesquisar.");
            return;
        }
        formHandlerService.read($scope, config);
    };
    $scope.abrirEdicaoItem = function (index, item) {

        $scope.TipoAcaoTel = { label: 'Alterar', valor: 1, index: index };
        var telefoneObj = angular.copy(item);
        $scope.telefone = telefoneObj;
    }

    // ---- Manutenção Lista Telefones
    $scope.IncluirTelefone = function (tel) {

        if (tel && tel.TIPO_TEL_ID == null) {

            $scope.message = Util.createMessage("fail", "Preencha corretamente o email antes de adicionar mais uma linha.");
            return;
        }
        if ($scope.listatelefone) {
            novo = {};
            novo.ASN_NUM_ASSINATURA =  $scope.assinturaSelect; 
            $scope.listatelefone.push(novo);
        }
    }
    $scope.ExcluirTelefone = function (index) {

        $scope.listatelefone.splice(index, 1);

        //if ($scope.listatelefone && (index | index == 0)) {
        //    if (confirm("Confirmar exclusão")) {
             
        //    }
        //}
        //else {
        //    $scope.message = Util.createMessage('fail', 'Ocorreu algum erro ao tentar retirar o telefone da lista');
        //}
    }
    // ---- Manutenção Lista Emails
    $scope.IncluirEmail = function (email) {

        if (email && email.AEM_EMAIL == null) {

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
    $scope.ExcluirEmail = function (index) {

        $scope.listaemail.splice(index, 1);

        //if ($scope.listaemail && (index | index == 0)) {
        //    if (confirm("Confirmar exclusão")) {
                
        //    }
        //}
        //else {
        //    $scope.message = Util.createMessage('fail', 'Ocorreu algum erro ao tentar retirar o telefone da lista');
        //}
    }
    //---------------------------------

    //$scope.editarTel = function (tel) {

    //    $scope._desabilitarDemaisTelefones(tel);
    //    tel.editarTel = true;

    //}

    //$scope.confirmarEdicaoTel = function ($event, tel) {

    //    $event.stopPropagation();
    //    if (tel) {
    //        tel.editarTel = false;
    //    }

    //};

    //$scope.editarTipo = function (tel) {

    //    $scope._desabilitarDemaisTelefones(tel);
    //    tel.editarTipo = true;

    //}

    //$scope.confirmarEdicaoTipo = function ($event, tel) {

    //    $scope.erros = [];
    //    $event.stopPropagation();
    //    if (tel) {
    //        tel.editarTipo = false;
    //    }

    //};

    $scope.AbrirModalEndereco = function (end, index) {

        if ($scope.cliente) {

            $scope.end = { CLI_ID: $scope.cliente.CLI_ID };
        }
        else {
            $scope.end = {};
        }

        if (end) {

            $scope.acaoEnd = { acao: 1, label: 'Alterar', index: index };

            $scope.getBairrosPorUf(end);
            $scope.end = angular.copy(end);
        }
        else {
            $scope.acaoEnd = { acao: 0, label: 'Incluir', index: index };
        }

        angular.element("#modal-endereco").modal();
    }

    $scope.AbrirModalParcelas = function (contrato) {

        showAjaxLoader();

        var url = "/Cliente/BuscarParcelas";

        $http({
            url: url,
            method: "post",
            data: { _numcontrato: contrato.CTR_NUM_CONTRATO }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {
                
                $scope.listaparcelas = response.result.listaparcelas;

                angular.forEach($scope.listaparcelas, function (obj, index) {
                    obj.PAR_DATA_VENCTO = dataAtualFormatada(obj.PAR_DATA_VENCTO);
                    if (obj.PAR_DATA_VENCTO > dataatual)
                        obj.SITUACAO = "Aberta";
                    else if (obj.PAR_DATA_VENCTO <= dataatual)
                        obj.SITUACAO = "Vencida";
                });

                angular.element("#modal-parcelas").modal();
            }
            else {

                alert(response.message.message);
            }

        }).error(function (response) {

            alert(response);
  
            hideAjaxLoader();
        })

    }
    //---- Funções para buscar clientes que ja estão  na URA ---// 
    $scope.AbrirModalURA = function (_assinatura) {

        showAjaxLoader();
        $http({
            url: "/URA/ConsultarURASCliente",
            method: "Post",
            dataType: 'json',
            data: {_asn_id: _assinatura.ASN_NUM_ASSINATURA}
        }).success(function (response) {

            if (response.success == true) {
                $scope.listauras = response.result.listauras;

                angular.element("#modal-ura").modal();
            }
            else {
                alert(response.message.message);
            }

            hideAjaxLoader();
        })


    };



    $scope.salvarEndereco = function () {

        if ($scope.cliente != null && !$scope.cliente.CLIENTES_ENDERECO)
            $scope.cliente.CLIENTES_ENDERECO = [];


        if ($scope.end && $scope.cliente && $scope.cliente.CLIENTES_ENDERECO) {

            formHandlerService.submit($scope, {
                url: Util.getUrl("/endereco/validarendereco"),
                objectName: 'end',
                success: function (resp, status, config, message, validationMessage) {
                    $scope.message = message;
                    $scope.erros = validationMessage;

                    if (resp.success) {
                        var end = angular.copy($scope.end);

                        if ($scope.acaoEnd && $scope.acaoEnd.acao === 1) {

                            var index = $scope.acaoEnd.index;
                            $scope.cliente.CLIENTES_ENDERECO[index] = end;
                        }
                        else {

                            $scope.cliente.CLIENTES_ENDERECO.push(end);
                        }

                        $scope.end = {};
                        angular.element("#modal-endereco").modal("hide");
                    }


                },
                error: function (resp, status, config, message, validationMessage) {

                    $scope.message = message;
                    angular.element("#modal-endereco").modal("hide");
                }
            });

        }
    }

    $scope.listarClientesRepresentante = function (pageRequest) {

        $scope.message = null;
         
       var url = Util.getUrl("/franquia/clientes/clientesporrepresentante");

        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }

        var config = {
            url: url,
            targetObjectName: 'clientes',
            responseModelName: 'clientes',
            showAjaxLoader: true,
            pageConfig: { pageName: 'page' }
        };

        if ($scope.filtro) {

            if ($scope.filtro.cpf_cnpj || $scope.filtro.nome) {

                config.data = angular.copy($scope.filtro);
            }
            
        }
        formHandlerService.read($scope, config);
    };
    //-------------------------------------------------------Fim Ações -----------------------------------------------------------------------


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

    $scope.tipoEnderecoSelecionado = function () {

        if ($scope.end && $scope.cliente && $scope.cliente.CLIENTES_ENDERECO) {

            if ($scope.end && $scope.end.TIPO_ENDERECO) {

                var tpEndId = $scope.end.TIPO_ENDERECO.TP_END_ID;

                var qtd = 0;

                angular.forEach($scope.cliente.CLIENTES_ENDERECO, function (value, index) {

                    if (value.END_TIPO == tpEndId)
                        qtd++;
                });

                if (qtd == 0) {

                    $scope.end.END_TIPO = tpEndId;
                }
                else {

                    $scope.message = Util.createMessage("fail", "Já existe um endereço com o tipo selecionado.");
                    $scope.end.END_TIPO = null;
                    $scope.end.TIPO_ENDERECO = null;
                }


            }
        }



    }

    $scope.municipioSelecionado = function () {

        if ($scope.end && $scope.end.MUNICIPIO) {
            var munId = $scope.end.MUNICIPIO.MUN_ID;
            $scope.end.MUN_ID = munId;

        }
        else {
            $scope.end.MUN_ID = null;
        }

    }

    $scope.getBairrosPorUf = function (end) {

        $scope.end.MUN_ID = null;
        $scope.municipios = null;

        if (end && end.END_UF) {

            formHandlerService.read($scope, {
                url: Util.getUrl("/endereco/municipiosporuf"),
                targetObjectName: 'municipios',
                responseModelName: 'municipios',
                data: { uf: end.END_UF },
                success: function () {

                }
            });
        }

    }

    $scope.podeAdicionar = function () {

        if ($scope.end && $scope.cliente && $scope.cliente.CLIENTES_ENDERECO) {

            var qtdEntrega = 0;
            var qtdFaturamento = 0;

            angular.forEach($scope.cliente.CLIENTES_ENDERECO, function (value, old) {

                if (value.END_TIPO == 1)
                    qtdEntrega++;

                if (value.END_TIPO == 2)
                    qtdFaturamento++;
            });

            if (qtdEntrega >= 1 && qtdFaturamento >= 1) {

                return false;
            }
            return true;

        }
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

    function dataAtualFormatada(dataHora) {

        var parseDate = new Date(parseInt(dataHora.substr(6)));
        var jsDate = new Date(parseDate);

        return jsDate;
    }


});
