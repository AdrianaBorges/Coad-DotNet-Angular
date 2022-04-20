/// <reference path="controllers_plugins/endereco.js" />
appModule.controller('ClienteProspectController', ['$scope', 'formHandlerService', '$http', 'conversionService', '$timeout', 'AuthService', '$interval',
    function ($scope, formHandlerService, $http, conversionService, $timeout, AuthService, $interval) {

    $scope.tab = 1;
    $scope.filtroProsp = { pesquisaCpfCnpjPorIqualdade : true};
    $scope.queryCarteira = {Deletar : false};

    $scope.init = function (podeEditar) {
        
        $scope.podeEditar = (podeEditar === 'True') ? true : false;
        AuthService.checarPermissao("PossuiPermissaoParaEditarProspect", "podeEditar", $scope);
        $scope.carregarTipoProposta();
        $scope.getLstTipoTelefone();

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


    $scope.readProspect = function (clienteId, tipo) {

        tipo = (tipo)  ? tipo : 'cliente';

        if (clienteId) {
            if (!tipo || tipo === 'cliente') {

                $scope.readProspectCorp(clienteId);
            }
            else if (tipo == 'prospect') {
                $scope.readProspectOriginal(clienteId);
            }
        }
        else {

            $scope.prospect = {
                Novo: true,
                EnderecoFaturamento: {},
                EnderecoEntrega: {},
                Telefones: [],
                Emails: []
            };
        }
    }

    $scope.getLstTipoTelefone = function () {

        formHandlerService.read($scope, {
            url: Util.getUrl("/franquia/clientetelefone/listartipotelefone"),
            targetObjectName: 'lstTipoTelefone',
            responseModelName: 'lstTipoTelefone',
            success: function () {
            }
        });

    }

    $scope.readProspectCorp = function (clienteId) {

        $scope.prospect = null;
        if (clienteId) {
            var url = Util.getUrl("/prospect/recuperarDadosDoProspect");

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'prospect',
                responseModelName: 'prospect',
                showAjaxLoader: true,
                data : {clienteId : clienteId},
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
            data: $scope.filtroProsp,
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
            pageConfig: { pageName: 'page' /*, pageTargetName: 'paginaProspect'  */},
            data: $scope.filtroProsp,
            success: function (response) {
                $scope.listado = true;

            }
        });
    }

    $scope.buscarProspects = function (pageRequest) {
              
        if ($scope.filtroProsp) {
            if ($scope.filtroProsp.cpf_cnpj || $scope.filtroProsp.nome ||
                    $scope.filtroProsp.classificacaoClienteId || $scope.filtroProsp.email || $scope.filtroProsp.REP_ID
                || $scope.filtroProsp.telefone || $scope.filtroProsp.codigoCliente || $scope.filtroProsp.origemId || $scope.filtroProsp.excluidosDaValidacao) {

                if ($scope.filtroProsp.dddTelefone && !$scope.filtroProsp.telefone) {

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

    $scope.salvarProspect = function (forcar) {

        var url = Util.getUrl("/prospect/salvarClienteEProspect");

        if (forcar == true) {

            if (confirm("Deseja salvar assim mesmo?")) {

                $scope.message = null;
                $scope.prospect.ForcarSalvamento = true;
            }
            else {
                return;
            }
        }

        formHandlerService.submit($scope, {
            url: url,
            objectName: 'prospect',
            showAjaxLoader: true,
            success: function (resp, status, config, message, validationMessage) {
                
                $scope.erros = validationMessage;

                $scope.button = 'reset';

                if (resp.success) {
                    $scope.salvamentoResult = resp.result.salvamentoResult;
                    if ($scope.salvamentoResult && (!$scope.salvamentoResult.ResultadoValidacaoDuplicidade ||
                        !$scope.salvamentoResult.ResultadoValidacaoDuplicidade.HasDuplication))
                    {
                        $scope.codigoProspect = resp.result.cliId;
                        angular.element("#modal-prospect-criado").modal();

                    
                        if ($scope.codigoProspect) {
                            $scope.readProspect($scope.codigoProspect, 'cliente');
                        }
                        else {
                        }
                    }
                    else if (Util.isPathValid($scope, 'salvamentoResult.ResultadoValidacaoDuplicidade') &&
                        $scope.salvamentoResult.ResultadoValidacaoDuplicidade.HasDuplication == true) {

                        if ($scope.salvamentoResult.ResultadoValidacaoDuplicidade.HasDuplicationWarnings &&
                            !$scope.salvamentoResult.ResultadoValidacaoDuplicidade.HasDuplicationErrors) {

                            $scope.message = Util.createMessage('warning', $scope.salvamentoResult.ResultadoValidacaoDuplicidade.ErrorMessage);
                            $timeout(function () {

                                $scope.salvarProspect(true);
                            },
                            3000);
                        }
                        else {
                           $scope.message = Util.createMessage('fail', $scope.salvamentoResult.ResultadoValidacaoDuplicidade.ErrorMessage);
                        }
                        
                        if (Util.isPathValid(resp, 'result.salvamentoResult.ItensReferencia')) {

                            $scope.itensConflito = resp.result.salvamentoResult.ItensReferencia;
                        }

                    }

                }
                else {
                    $scope.message = message;
                }                
            }
        });
    }

    $scope.carregarTipoProposta = function () {

        var url = Util.getUrl("/prospect/listarOpcoesAtendimentos");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstOpcoesAtendimento',
            responseModelName: 'lstOpcoesAtendimento',
            showAjaxLoader: true,

        });
    }



    $scope.adicionarLinhaCarteira = function () {

        if ($scope.prospect.CarteirasCliente && $scope.prospect.CarteirasCliente.length > 0) {

            var lista = $scope.listaDataFiltrada;
            var value = lista[lista.length - 1];
            if (lista.length <= 0 || (value && value.CarId)) {

                $scope.prospect.CarteirasCliente.push({ CLI_ID: $scope.prospect.ClienteId, Deletar: false });
            }
            else {

                $scope.message = Util.createMessage("fail", "Não é possível adicionar mais uma linha até que a linha anterior esteja correta.");
            }
        }
        else {

            $scope.prospect.CarteirasCliente = [{ CLI_ID: $scope.prospect.ClienteId, Deletar : false }];

        }
    };

    $scope.removerCarteira = function ($index) {
        var obj = $scope.listaDataFiltrada[$index];

        if (obj) {
            obj.Deletar = true;
        }

    };

    $scope.adicionarEmail = function () {


        if ($scope.prospect.Emails && $scope.prospect.Emails.length > 0) {

            var lista = $scope.prospect.Emails;
            var value = lista[lista.length - 1];
            if (value && value.Email) {

                $scope.prospect.Emails.push({ CLI_ID: $scope.prospect.ClienteId });
            }
            else {

                $scope.message = Util.createMessage("fail", "Não é possível adicionar mais uma linha até que a linha anterior esteja correta.");
            }
        }
        else {

            $scope.prospect.Emails = [{ CLI_ID: $scope.prospect.ClienteId }];

        }
    };

    $scope.removerEmail = function ($index) {
        $scope.prospect.Emails.splice($index, 1);

    };


    $scope.AdicionarTelefone = function (tel) {

        if (tel && (tel.TipoTelefone == null || tel.Telefone == null)) {

            $scope.message = Util.createMessage("fail", "Preencha corretamente esta linha de telefone antes de adicionar mais uma");
            return;
        }
        if ($scope.PROSPECT_TELEFONE) {

            $scope.PROSPECT_TELEFONE.push({ DATA_INSERSAO: new Date() });
        }
        else {
            $scope.PROSPECT_TELEFONE = [{ DATA_INSERSAO: new Date() }];
        }
    }



    $scope.RemoverTelefone = function (index) {

        if ($scope.PROSPECT_TELEFONE && (index | index == 0)) {

            $scope.PROSPECT_TELEFONE.splice(index, 1);

        }
        else {

            $scope.message = Util.createMessage('fail', 'Ocorreu algum erro ao tentar retirar o telefone da lista');
        }
    }

        $scope.abrirModalPesquisaCNPJ_CPF = function (tipoCliente) {

            if (tipoCliente) {
                if (tipoCliente == 2) {
                    angular.element("#Modal-CPF").modal();
                }
                else {
                    angular.element("#Modal-CNPJ").modal();
                }
            }
        };

    $scope.limpaInscricaoEstadual = function(isento, insEstadual) {

        if (isento) {
            if (insEstadual.length > 0) {
                $scope.prospect.EhIsentoDeInscricaoEstadual = false;

            }
             
        }
    };

    $scope.abrirModalRepresentante = function(){
        if($scope.proposta.CLI_ID){

            $scope.filtroProsp = {CLI_ID : $scope.proposta.CLI_ID};
            $scope.buscarTodosOsRepresentanteDoCliente();
            angular.modal('#modal-busca-representante').modal();
        }
    }

    $scope.removerRepresentante = function(){

        $scope.REP_ID = null;
        $scope.REPRESENTANTE = null;
    }

    $scope.adicionarRepresentante = function(rep){

        $scope.REPRESENTANTE = rep;
        $scope.REP_ID = rep.REP_ID;
    }

    $scope.buscarTodosOsRepresentanteDoCliente = function (pageRequest) {

        $scope.message = null;
        $scope.listado = false;

        if ($scope.filtroProsp.CLI_ID) {

            var url = Util.getUrl("/franquia/representante/listarTodosOsRepresentantesDoCliente");

            if (pageRequest) {
                url += "?pagina=" + pageRequest;
            }

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'lstProspect',
                responseModelName: 'lstProspect',
                showAjaxLoader: true,
                pageConfig: { pageName: 'page', pageTargetName: 'paginaProspect' },
                data: $scope.filtroProsp,
                success: function (response) {
                    $scope.listado = true;

                }
            });
        }
        else {
            $scope.message = Util.createMessage('fail', 'Selecione o Cliente/Prospect antes de pesquisar');
        }
    }

    $scope.buscarCarteiras = function (pageRequest) {

        $scope.message = null;
        $scope.carteiraListada = false;

        var url = Util.getUrl("/franquia/carteiramento/BuscarCarteiras");

        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstCarteiras',
            responseModelName: 'lstCarteiras',
            showAjaxLoader: true,
            pageConfig: { pageName: 'page', pageTargetName: 'paginaCarteira' },
            data: $scope.filtroCart,
            success: function (response) {
                $scope.carteiraListada = true;

            }
        });
    }
    $scope.checarCarteiraValida = function (carId, carCli) {

        formHandlerService.read(carCli, {
            url: Util.getUrl("/prospect/ChecarCarteiraValida"),
            targetObjectName: 'valida',
            responseModelName: 'valida',
            data: {carId : carId},
            success: function () {
            }
        });

    }
    $scope.abrirModalCarteira = function (index) {

        $scope.contextoModalCarteira = { index: index };
        $scope.filtroCart = {};
        $scope.carregarUENs();
        $scope.carregaComboRegioes();
        angular.element('#modal-carteira').modal();
    }


    $scope.listarCarteiras = function (pagina) {

        $scope.carteiraListada = false;
        var url = Util.getUrl("/franquia/carteiramento/buscarCarteiras");

        if (pagina) {

            url += "?pagina=" + pagina;
        }

        $scope.filtroCart.associadoARepresentante = true;
        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstCarteiras',
            responseModelName: 'lstCarteiras',
            data: $scope.filtroCart,
            showAjaxLoader: true,
            pageConfig: { pageName: 'page', pageTargetName: 'paginaCarteira' },
            success: function (resp) {
                $scope.carteiraListada = true;
            }
        });
    }

    $scope.adicionarCarteira = function (car) {

        if ($scope.contextoModalCarteira) {

            var index = $scope.contextoModalCarteira.index;
            if (car && (index == 0 || index)) {

                if (Util.isPathValid($scope, 'prospect.CarteirasCliente')) {
                    $scope.listaDataFiltrada[index].CarId = car.CAR_ID;
                    $scope.checarCarteiraValida($scope.listaDataFiltrada[index].CarId, $scope.listaDataFiltrada[index])
                    angular.element("#modal-carteira").modal('hide');
                }
            }
            else {
                if (!car)
                    Util.createMessage('fail', 'Erro. O objeto de carteira não é valido');
                if (index != 0 && !index)
                    Util.createMessage('fail', 'Erro. O index não é válido');
            }
        }
        else {
            Util.createMessage('fail', 'Erro. Não é possível encontrar o index não é válido');
        }
    }

    $scope.carregaComboRegioes = function (uenId) {

        if ($scope.filtroCart)
            $scope.filtroCart.rgId = null;
        var parans = { uenId: uenId };

        var url = Util.getUrl('/regiao/ListarComboRegiao');
        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'regioesCombo',
            responseModelName: 'lstRegioes',
            data: parans,
            success: function () {

            }
        });

    }

    $scope.buscarCepProspect = function (end) {

        var _cep_id = end.CEP;
        if (_cep_id && _cep_id.length >= 8) {

            var url = Util.getUrl('/CEP/BuscarCep');

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'cep',
                responseModelName: 'cep',
                data: { _cep: _cep_id },
                success: function (result) {
                    
                    if (result.success) {

                        if ($scope.cep == null) {
                            end.Bairro = null;
                            end.UF = null;
                            end.MunId = null;
                            end.Municipio = null;
                            end.TipoLogradouro = null;
                            end.Logradouro = null;
                            end.Numero = null;
                            end.Complemento = null;
                        }
                        else {
                            if ($scope.cep.CEP_BAIRRO != null) {
                                end.Bairro = $scope.cep.CEP_BAIRRO.BAR_DESCRICAO;
                            }
                            if ($scope.cep.MUNICIPIO != null) {
                                end.Municipio = $scope.cep.MUNICIPIO.MUN_DESCRICAO;
                            }
                            end.UF = $scope.cep.CEP_UF;
                            end.MunId = $scope.cep.MUN_ID;
                            end.Munic = $scope.cep.MUNICIPIO;
                            end.TipoLogradouro = $scope.cep.CEP_TIPO_LOGRADOURO;
                            end.Logradouro = $scope.cep.CEP_LOG;
                            end.Numero = null;
                            end.Complemento = null;
							
                        }
                    }
                }
            });
        }
    }

    $scope.removerEnderecoProspectFaturamento = function () {

        if (confirm("Deseja relamente remover esse endereço")) {
            if ($scope.prospect.EnderecoFaturamento) {

                $scope.prospect.EnderecoFaturamento.Municipio = null;
                $scope.prospect.EnderecoFaturamento.MunId = null;
                delete $scope.prospect.EnderecoFaturamento;
            }
            else {
                $scope.message = Util.createMessage('fail', 'O objeto de endereço não está presente');
            }
        }
    }

    $scope.removerEnderecoProspectEntrega = function () {

        if (confirm("Deseja relamente remover esse endereço")) {
            if ($scope.prospect.EnderecoEntrega) {

                $scope.prospect.EnderecoEntrega.Municipio = null;
                $scope.prospect.EnderecoEntrega.MunId = null;
                delete $scope.prospect.EnderecoEntrega;
            }
            else {
                $scope.message = Util.createMessage('fail', 'O objeto de endereço não está presente');
            }
        }
    }
    $scope.adicionarEnderecoEntrega = function () {

        $scope.prospect.EnderecoEntrega = {

            TipoEnd: 1
        };
    }

    $scope.adicionarEnderecoFaturamento = function () {

        $scope.prospect.EnderecoFaturamento = {

            TipoEnd: 2
        };
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

    if (window.GerenciarAssinaturaController !== undefined) {

        GerenciarAssinaturaController($scope, formHandlerService, $timeout);
        $scope.initAssinatura();
    }

        // sobrescrevendo a função declarado na endereco.js
    $scope.selecMunicipio = function (_municipio) {

        $scope.end.Bairro = null;
        $scope.end.UF = null;
        $scope.end.MunId = null;
        $scope.end.Municipio = null;
        $scope.end.TipoLogradouro = null;
        $scope.end.Logradouro = null;
        $scope.end.Numero = null;
        $scope.end.Complemento = null;


        if (_municipio.MUN_CEP != null) {
            $scope.end.CEP = _municipio.MUN_CEP;
            $scope.end.MunId = _municipio.MUN_ID;
            $scope.end.Municipio = _municipio.MUN_DESCRICAO;
            $scope.end.UF = _municipio.UF;
        }

    }
}]);

