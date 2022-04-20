

function EnderecoController($scope, formHandlerService, $http, $timeout) {


    $scope.filtro = {};
    $scope.filtro.Logradouro = " ";
    $scope.filtro.Email = " ";
    $scope.filtro.Telefone = " ";

    var now = new Date();
    var dataatual = new Date(now.getFullYear(), now.getMonth(), now.getDate());

    $scope.initEndSimple = function () {

        $scope.listarUfs();

    }

    $scope.initEnd = function (validacaoTotalEndereco) {

        $scope.validacaoTotalEndereco = validacaoTotalEndereco;
        $scope.listarUfs();
        $scope.iniciarCombosEndereco();

        if ($scope.validacaoTotalEndereco == false) {

            $scope.end = { validacaoTotal: false };
        }
        else {
            $scope.end = {};
        }

    }
    $scope.buscarCep = function () {

        if ($scope.end) {
            var _cep_id = $scope.end.END_CEP;

            if (_cep_id && _cep_id.length >= 8) {

                showAjaxLoader();

                var url = "/CEP/BuscarCep";
                $http({
                    url: url,
                    method: "post",
                    data: { _cep: _cep_id }
                }).success(function (retorno) {

                    hideAjaxLoader();
                    $scope.referenciarEndereco();
                    if (retorno.success) {
                        if (retorno.result.cep == null) {
                            $scope.end.END_BAIRRO = null;
                            $scope.end.END_UF = null;
                            $scope.end.MUN_ID = null;
                            $scope.end.END_MUNICIPIO = null;
                            $scope.end.TIPO_LOGRADOURO = null;
                            $scope.end.END_LOGRADOURO = null;
                            $scope.end.END_NUMERO = null;
                            $scope.end.END_COMPLEMENTO = null;
                        }
                        else {
                            if (retorno.result.cep.CEP_BAIRRO != null) {
                                $scope.end.END_BAIRRO = retorno.result.cep.CEP_BAIRRO.BAR_DESCRICAO;
                            }
                            if (retorno.result.cep.MUNICIPIO != null) {
                                $scope.end.END_MUNICIPIO = retorno.result.cep.MUNICIPIO.MUN_DESCRICAO;
                            }
                            $scope.end.END_UF = retorno.result.cep.CEP_UF;
                            $scope.end.MUN_ID = retorno.result.cep.MUN_ID;
                            $scope.end.TIPO_LOGRADOURO = retorno.result.cep.CEP_TIPO_LOGRADOURO;
                            $scope.end.END_LOGRADOURO = retorno.result.cep.CEP_LOG;
                            $scope.end.END_NUMERO = null;
                            $scope.end.END_COMPLEMENTO = null;
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
        }
    }


    $scope.buscarMunicipio = function (munDesc) {

        if (!$scope.filtro) {

            $scope.filtro = {};
        }

        if ($scope.filtro.mun_descricao != null && $scope.filtro.mun_descricao != "" && $scope.filtro.mun_descricao.length < 3) {
            return;
        }
        

        showAjaxLoader();

        var url = "/Municipio/BuscarMunicipio";

        $http({
            url: url,
            method: "Post",
            data: { _nomemunicipio: $scope.filtro.mun_descricao }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {
                $scope.dbMunicipio = response.result.dbMunicipio;
                $scope.page = response.page;
            }
            else {
                $scope.dbMunicipio = null;
                $scope.message = Util.createMessage("fail", response.message.message);
            }

        }).error(function (response) {

            $scope.dbMunicipio = null;
            $scope.message = Util.createMessage("fail", response.message.message);

            hideAjaxLoader();
        })

    };


    $scope.buscarMunicipioEUF = function (munDesc, uf) {

        if (!$scope.filtro) {

            $scope.filtro = {};
        }

     //   $scope.filtro.mun_descricao = munDesc;

        if ($scope.filtro.mun_descricao != null && $scope.filtro.mun_descricao != "" && $scope.filtro.mun_descricao.length < 3) {
            return;
        }


        showAjaxLoader();

        var url = "/Municipio/BuscarMunicipio";

        $http({
            url: url,
            method: "Post",
            data: { _nomemunicipio: $scope.filtro.mun_descricao, uf: uf }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {
                $scope.dbMunicipio = response.result.dbMunicipio;
                $scope.page = response.page;
            }
            else {
                $scope.dbMunicipio = null;
                $scope.message = Util.createMessage("fail", response.message.message);
            }

        }).error(function (response) {

            $scope.dbMunicipio = null;
            $scope.message = Util.createMessage("fail", response.message.message);

            hideAjaxLoader();
        })

    };


    $scope.AbrirModalEndereco = function (end, index) {

        var validacaoTotal = ($scope.validacaoTotalEndereco != false);
        if ($scope.cliente) {

            $scope.end = { CLI_ID: $scope.cliente.CLI_ID, validacaoTotal: validacaoTotal };
        }
        else {
            $scope.end = { validacaoTotal: validacaoTotal };
        }

        if (end) {

            $scope.acaoEnd = { acao: 1, label: 'Alterar', index: index };

            $scope.getMunicipioPorUf(end);
            $scope.end = angular.copy(end);
        }
        else {
            $scope.acaoEnd = { acao: 0, label: 'Incluir', index: index };
        }

        angular.element("#modal-endereco").modal();
    }


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
    
        
    $scope.referenciarEndereco = function () {

        if ($scope.end && $scope.cliente && $scope.cliente.CLIENTES_ENDERECO) {

            if ($scope.end && $scope.end.TIPO_ENDERECO) {

                var tpEndId = $scope.end.TIPO_ENDERECO.TP_END_ID;  

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

    $scope.getMunicipioPorUf = function (end) {

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
    $scope.abrirModalMunicipio = function (end) {

        $scope.end = end;
        angular.element("#Modal-Municipio").modal();
    }

    $scope.selecMunicipio = function (_municipio) {

        $scope.end.END_BAIRRO = null;
        $scope.end.END_UF = null;
        $scope.end.MUN_ID = null;
        $scope.end.END_MUNICIPIO = null;
        $scope.end.TIPO_LOGRADOURO = null;
        $scope.end.END_LOGRADOURO = null;
        $scope.end.END_NUMERO = null;
        $scope.end.END_COMPLEMENTO = null;

        if (_municipio.MUN_CEP != null) {
            $scope.end.END_CEP = _municipio.MUN_CEP;
            $scope.end.MUN_ID = _municipio.MUN_ID;
            $scope.end.END_MUNICIPIO = _municipio.MUN_DESCRICAO;
            $scope.end.END_UF = _municipio.UF;
        }

    }

    $scope.selecILogradouro = function (_cli_id) {

        $scope.filtro.cliente = _cli_id;

        $scope.listar();
    }


    $scope.listarUfs = function () {

        var url = Util.getUrl("/regiao/ListarUfs");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'listUfs',
            responseModelName: 'listUfs',
            showAjaxLoader: true,
            success: function () {

                
            }
        });
    };

    $scope.iniciarCombosEndereco = function () {

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


};
