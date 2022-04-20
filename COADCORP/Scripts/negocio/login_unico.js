appModule.controller('LoginUnicoController', function ($scope, formHandlerService, $http,
    conversionService, messageService, comparatorService, $timeout, $parse) {

    $scope.query = { show: true };
    
    $scope.init = function (codAssinatura, returnURL, semanticaURL) {

        $scope.passoWizard = 0; // colocar 0 quando acabar testes.
        //$scope.termoAceito = true; // remover quando acabar testes.
        $scope.modalAssinatura = {modalAberta : false};
        $scope.edicaoCliente = true;
        $scope.returnURL = (returnURL && returnURL != '') ? returnURL : 'http://www.coad.com.br/';
        $scope.semanticaURL = (semanticaURL && semanticaURL != '') ? semanticaURL : 'do portal';

        $scope.loginUnicoRequest = {
             
            Assinaturas: [],
            AssinaturaPrincipal: {}
        };
               
        $scope.validations = {};

        if (codAssinatura) {

            $scope.carregarDadosDaAssinatura(codAssinatura);
        }
    }

    $scope.adicionarErroDeValidacao = function (chave, mensagem, mensagemAlert) {

        $scope.message = Util.createMessage('fail', 'Não é possível prosseguir.', mensagemAlert);
        var validation = {};
        var valor = null;

        if(typeof mensagem != 'array')
            valor = [mensagem];

        validation[chave] = valor;

        $scope.validations = validation;
    }

    $scope.carregarDadosDaAssinatura = function (codAssinatura) {

        var url = Util.getUrl('/loginUnico/ObterDadosDaAssinaturaECliente');
        formHandlerService.read($scope, {
            url: url,
            data: { codAssinatura: codAssinatura },
            showAjaxLoader: true,
            targetObjectName: 'assinatura',
            responseModelName: 'assinatura',
            success: function () {

                var cliId = $scope.assinatura.CLI_ID;
                var codAssinatura = $scope.assinatura.ASN_NUM_ASSINATURA;
                $scope.loginUnicoRequest.AssinaturaPrincipal = $scope.assinatura;
                $scope.BuscarResumosDeAssinaturasDoCliente(cliId, codAssinatura, true);

            }
        });

    }

    $scope.validarSenha = function (assinatura, senha, callback, callBackNaoProvacao) {

        var url = Util.getUrl('/loginUnico/testarSenhaDaAssinatura');
        formHandlerService.read($scope, {
            url: url,
            data: {
                codAssinatura: assinatura,
                senha : senha
            },
            showAjaxLoader: true,
            targetObjectName: 'senhaValida',
            responseModelName: 'senhaValida',
            success: function () {

                if ($scope.senhaValida == true && typeof callback == 'function') {
                    callback();
                }
                else {

                    if (typeof callBackNaoProvacao == 'function')
                        callBackNaoProvacao();
                }
            }
        });
    }

    $scope.sugerirUsuario = function (assinatura) {

        var url = Util.getUrl('/loginUnico/sugerirUsuario');
        formHandlerService.read($scope, {
            url: url,
            data: {codAssinatura: assinatura},
            showAjaxLoader: true,
            targetObjectName: 'loginUnicoRequest.Login',
            responseModelName: 'login',
            success: function () {

            }
        });
    }

    $scope.validarUsuarioExistente = function (callback) {

        var url = Util.getUrl('/loginUnico/checarUsuarioJaExiste');

        if(Util.isPathValid($scope,'loginUnicoRequest.Login'))
        {   
            formHandlerService.read($scope, {
                url: url,
                data: { usuario: $scope.loginUnicoRequest.Login },
                showAjaxLoader: true,
                targetObjectName: 'usuarioExiste',
                responseModelName: 'usuarioExiste',
                success: function () {

                    if ($scope.usuarioExiste) {
                        $scope.adicionarErroDeValidacao(
                            "loginUnicoRequest.Login",
                            "O Usuário informado já existe",
                            "O Usuário informado já existe");

                        if (typeof callback == 'function')
                        {
                            callback();
                        }
                    }
                    else {
                        $scope.validations = {};
                    }
                }
            });
        }
    }
    $scope.BuscarResumoAssinatura = function (codAssinatura, senha) {

        var url = Util.getUrl('/loginUnico/buscarResumoAssinatura');

        formHandlerService.read($scope, {
            url: url,
            data: { codAssinatura: codAssinatura },
            showAjaxLoader: true,
            targetObjectName: 'modalAssinatura.infoAssinatura',
            responseModelName: 'logUnicoAssinatura',
            success: function () {

                $scope.modal
            }
        });
        
    }

    $scope.BuscarResumosDeAssinaturasDoCliente = function (cliId, excetoAssinatura, marcarAssinaturasComoNativa) {

        var url = Util.getUrl('/loginUnico/buscarResumosDeAssinaturasDoCliente');

        formHandlerService.read($scope, {
            url: url,
            data: {
                cliId: cliId,
                excetoAssinatura: excetoAssinatura,
                marcarAssinaturasComoNativa: marcarAssinaturasComoNativa
            },
            showAjaxLoader: true,
            targetObjectName: 'lstAssinaturasNativas',
            responseModelName: 'lstLogUnicoAssinatura',
            success: function () {

                if ($scope.lstAssinaturasNativas) {
                    $scope.loginUnicoRequest.Assinaturas = $scope.lstAssinaturasNativas;
                }
            }
        });

    }

    $scope.processarPasso0 = function () {

        if ($scope.passoWizard == 0) {

            var assinatura = $scope.assinatura.ASN_NUM_ASSINATURA;
            var senha = $scope.loginUnicoRequest.SenhaAssinatura;

            $scope.validarSenha(assinatura, senha,
                function () {

                    $scope.irParaPasso(1);
                    $scope.sugerirUsuario($scope.assinatura.ASN_NUM_ASSINATURA);
            },
                function () {

                    $scope.adicionarErroDeValidacao("loginUnicoRequest.SenhaAssinatura", "Assinatura ou senha não estão corretos", "Assinatura ou senha não estão corretos");
                }
            );
        }
    };


    $scope.processarPasso1 = function () {

        if ($scope.loginUnicoRequest) {

            var senha1 = $scope.loginUnicoRequest.Senha;
            var senha2 = $scope.loginUnicoRequest.ConfirmacaoSenha;
            var possuiErro = false;    
            
            if (!$scope.usuarioExiste) {

                if (!senha1) {
                    possuiErro = true;
                    $scope.adicionarErroDeValidacao("loginUnicoRequest.Senha", "Digite sua nova senha", "Erro no cadastro de senha");
                } else if (senha1.length < 6) {

                    possuiErro = true;
                    $scope.adicionarErroDeValidacao("loginUnicoRequest.Senha", "Digite uma senha de no mínimo 6 dígitos", "Erro no cadastro de senha");
                }
                else
                    if (!senha2) {

                        possuiErro = true;
                        $scope.adicionarErroDeValidacao("loginUnicoRequest.ConfirmacaoSenha", "Confirme sua nova senha", "Erro no cadastro de senha");
                    }
                    else
                        if (senha1 != senha2) {

                            possuiErro = true;
                            $scope.adicionarErroDeValidacao("loginUnicoRequest.ConfirmacaoSenha", "A senha e a confirmação de senha devem ser iguais", "Erro no cadastro de senha");
                        }

                if (possuiErro == false) {
                    $scope.irParaPasso(2);
                    var cliId = $scope.assinatura.CLI_ID;
                    var codAssinatura = $scope.assinatura.ASN_NUM_ASSINATURA;

                    $scope.BuscarResumosDeAssinaturasDoCliente(cliId, codAssinatura, true);
                }
            }
            else {
                $scope.adicionarErroDeValidacao("loginUnicoRequest.Login",
                    "O Usuário informado já existe",
                    "O Usuário informado já existe");
            }
        }
        
    }

    $scope.processarPasso2 = function () {

        if(typeof $scope.initInfoCliente == 'function')
            $scope.initInfoCliente('false', $scope.assinatura.CLI_ID);
        
        if(typeof $scope.initClienteTelefone == 'function')
            $scope.initClienteTelefone();
        
        if(typeof $scope.initEnd == 'function')
            $scope.initEnd();
        
        if (Util.isPathValid($scope, 'assinatura.CLIENTES'))
        {
            $scope.cliente = $scope.assinatura.CLIENTES;
        }
        else {
            $scope.cliente = {

                ASSINATURA_EMAIL: [],
                ASSINATURA_TELEFONE: [],
                CLIENTE_ENDERECO: []
            };
        }
        $scope.irParaPasso(3);
    }

    $scope.processarPasso3 = function () {
        
        $scope.validarCliente(function () {

            $scope.passoWizard = 4;
        });
    }

    

    $scope.confirmar = function () {

        if ($scope.passoWizard == 0)
            $scope.processarPasso0();
        else
        if ($scope.passoWizard == 1)
            $scope.processarPasso1();
        else
        if ($scope.passoWizard == 2)
            $scope.processarPasso2();
        else
        if ($scope.passoWizard == 3)
            $scope.processarPasso3();

    };

    $scope.irParaPasso = function (nPasso) {

        $scope.message = null;
        if (!nPasso)
            nPasso = 0;
        $scope.passoWizard = nPasso;
    }

    $scope.avancarProcesso = function () {


       if (!$scope.passoWizard)
            $scope.passoWizard = 0;

        if ($scope.passoWizard < 5)
            $scope.passoWizard++;

       
    }

    $scope.voltarProcesso = function () {

        if (!$scope.passoWizard)
            $scope.passoWizard = 0;

        if($scope.passoWizard > 0)
            $scope.passoWizard--;
    }

    $scope.adicionarAssinatura = function () {

        if ($scope.modalAssinatura.infoAssinatura) {
            $scope.loginUnicoRequest.Assinaturas.push($scope.modalAssinatura.infoAssinatura);

            $scope.modalAssinatura.infoAssinatura = {};
            $scope.modalAssinatura.concluido = true;
        }
    }


    $scope.abrirModalAdicionarAssinatura = function () {

        $scope.modalAssinatura = {modalAberta : true};
        angular.element("#modal-adicionar-assinatura").modal();
    }

    $scope.checarAssinaturaValida = function () {

        $scope.message = null;

        var assinatura = $scope.modalAssinatura.CodAssinatura;
        var senha = $scope.modalAssinatura.SenhaAssinatura;
        $scope.validations = [];

        if (assinatura && assinatura.toUpperCase() == $scope.assinatura.ASN_NUM_ASSINATURA.toUpperCase()) {

            $scope.message = Util.createMessage('fail', 'A assinatura adicionada é a mesma utilizada para iniciar essa configuração. Por favor, escolha outra assinatura.');
            return;
        }

        if ($scope.checarAssinaturaJaAdicionada(assinatura)) {

            $scope.message = Util.createMessage('warning', 'Essa assinatura já foi adicionada.');
            return;
        }

        if (assinatura &&
            senha &&
            $scope.modalAssinatura.modalAberta == true) {

            $scope.modalAssinatura.infoAssinatura = null;
            $scope.validarSenha(assinatura, senha,
                function () {
                    $scope.BuscarResumoAssinatura(assinatura, senha);
                },
                function () {

                    $scope.adicionarErroDeValidacao("modalAssinatura.SenhaAssinatura", "Assinatura ou senha não estão corretos", "Assinatura ou senha não estão corretos");
                }
            );
        }
    };

    $scope.fecharModalAssinatura = function () {

        angular.element("#modal-adicionar-assinatura").modal('hide');
    }

    $scope.removerAssinaturaDaLista = function (index) {

        if (index != null && Util.isPathValid($scope, "loginUnicoRequest.Assinaturas")) {

            $scope.loginUnicoRequest.Assinaturas.splice(index, 1);
        }
    }
    
    $scope.checarAssinaturaJaAdicionada = function (codAssinatura) {

        var achou = false;

        angular.forEach($scope.loginUnicoRequest.Assinaturas, function (value) {

            if (achou === false) {
                if (value.CodAssinatura.toUpperCase() == codAssinatura.toUpperCase())
                    achou = true;
            }
        });

        return achou;
    }

    $scope.salvarConfiguracaoLoginUnico = function () {

        if (confirm("Confirmar configurações.")) {
            formHandlerService.submit($scope, {
                url: Util.getUrl("/loginUnico/salvarConfiguracaoLoginUnico"),
                objectName: 'loginUnicoRequest',
                deepConvertDate: true,
                showAjaxLoader: true,
                success: function (resp, status, config, message, validationMessage) {
                    $scope.message = message;
                    $scope.erros = validationMessage;
                    $scope.button = 'reset';

                    if (resp.success) {

                        $scope.message = Util.createMessage("success", "Configuração de Login Único realizado com sucesso!!");
                        $timeout(function () {
                            $scope.irParaPasso(5);
                            //window.location = Util.getUrl("/franquia/regiao/index");

                        }, 1000);
                    }
                },
                fail: function () {
                    $scope.buttonSave = 'reset';
                }
            });
        }
        else {

            return false;
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

    $scope.validarCliente = function (callback) {

        formHandlerService.submit($scope, {
            url: Util.getUrl("/loginUnico/validarCliente"),
            objectName: 'cliente',
            showAjaxLoader: true,
            success: function (resp, status, config, message, validationMessage) {
                $scope.message = message;
                $scope.erros = validationMessage;

                if (resp.success && typeof callback == 'function') {

                    callback(resp);
                }
            },
            fail: function () {
                
            }
        });
    }


    $scope.$watch('loginUnicoRequest.Assinaturas', function (value, old) {

        $scope.lstAssiIndex = $scope.RetornarQtdPaginasAssinatura();
    },
    true);

    $scope.RetornarQtdPaginasAssinatura = function () {

        if(Util.isPathValid($scope, 'loginUnicoRequest.Assinaturas')){
            var tamanho = Math.ceil($scope.loginUnicoRequest.Assinaturas.length / 4);

            var lstAssiIndex = [];

            for (var index = 0; index < tamanho; index++) {

                lstAssiIndex.push(index * 4);
            }

            return lstAssiIndex;
        }
        return [0];
    }

    $scope.removerEmail = function ($index) {

        $scope.cliente.ASSINATURA_EMAIL.splice($index, 1);

    };

    if (window.TelefoneController !== undefined) {

        TelefoneController($scope, formHandlerService, $http, conversionService);
    }

    if (window.InfoClienteController !== undefined) {

        InfoClienteController($scope, formHandlerService, $http, conversionService, $timeout);
    }

    if (window.EnderecoController !== undefined) {

        EnderecoController($scope, formHandlerService, $http, $timeout);
    }


}); 