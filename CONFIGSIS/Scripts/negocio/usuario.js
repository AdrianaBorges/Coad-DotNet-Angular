appModule.controller('UsuarioController', function ($scope, formHandlerService, $http, conversionService, cepService, $timeout) {

    $scope.filtro = {};
 

    
    /*
    *-------------------------------------------------------- Inicializações ----------------------------------------------------------------------
    */

    $scope.init = function () {

        $scope.query = { show: true };
    }
    
    $scope.read = function (usuLogin, origem, adminDeLogin) {

        $scope.origem = origem;

        if (adminDeLogin) {

            $scope.adminDeLogin = adminDeLogin;
        }

        $scope.init();
        if (usuLogin) {
            var url = Util.getUrl("/usuario/recuperardadosdousuario");
            

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'usuario',
                responseModelName: 'usuario',
                data: { USU_LOGIN: usuLogin },
                success: function () {

                    if ($scope.usuario) {

                        $scope.usuario.editando = ($scope.origem == "editar");
                    }                    
                }
            });
        }
        else {
            $scope.usuario = { PERFIL_USUARIO: [] };

            if ($scope.adminDeLogin) {

                $scope.usuario.CADASTRADO_POR = $scope.adminDeLogin;
            }

        }
    }

    $scope.salvarUsuario = function () {

        var url = ($scope.origem == "editar") ? "/usuario/salvar" : "/usuario/incluir";
        var usuario = angular.copy($scope.usuario);
        if (usuario && usuario.PERFIL_USUARIO) {

            if (usuario.PERFIL_USUARIO) {

                angular.forEach(usuario.PERFIL_USUARIO, function (value, index) {

                    value.PERFIL = null;
                });
            }
        }

        $scope.usuarioParaSalvar = usuario;

        formHandlerService.submit($scope, {
            url: Util.getUrl(url),
            objectName: 'usuarioParaSalvar',
            success: function (resp, status, config, message, validationMessage) {
                $scope.message = message;
                $scope.erros = validationMessage;


                if (resp.success) {

                    $scope.message = Util.createMessage("success", "Salvo com sucesso!");
                    $timeout(function () {

                        $scope.message = null;
                        window.location = Util.getUrl("/usuario/index");

                    }, 1000);
                }
                else {
                    $scope.button = 'save';

                    if ($scope.message.type == 'warning') {

                        $timeout(function () {

                            $scope.message = null;
                            window.location = Util.getUrl("/usuario/index");

                        }, 6000);
                    }
                }
               
               
                    
                
               
            }
        });
    }
  
     
    $scope.checaLogin = function () {

        $scope.bloqueiaSalvamento = false;
        if (!$scope.erros) {
            $scope.erros = [];
        }


        if ($scope.usuario && !$scope.usuario.editando) {

            $scope.bloqueiaSalvamento = true;
            var login = $scope.usuario.USU_LOGIN;

            if (login) {
                var url = Util.getUrl("/usuario/ChecaLogin");

                formHandlerService.read($scope, {
                    url: url,
                    targetObjectName: 'existe',
                    responseModelName: 'existe',
                    data: { login: login },
                    success: function (result) {

                        if ($scope.existe === true) {
                            $scope.loginMsg = "O login digitado já pertence a outro usuário.";
                            $scope.erros["USU_LOGIN"] = ["O login digitado já pertence a outro usuário."];
                        }
                        else {
                            $scope.loginMsg = null;
                            $scope.bloqueiaSalvamento = false;
                        }
                    }
                });
            }
            else {
                $scope.bloqueiaSalvamento = false;
            }
        }
    }
    
    $scope.listar = function (pageRequest) {

        $scope.listado = true;
        $scope.message = null;

        var url = Util.getUrl("/usuario/usuarios");

        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }

        var config = {
            url: url,
            targetObjectName: 'usuarios',
            responseModelName: 'lstUsuarios',
            showAjaxLoader: true,
            pageConfig: { pageName: 'page' }
        };

        if ($scope.filtro) {
            
            config.data = angular.copy($scope.filtro);
        }

        formHandlerService.read($scope, config);
    };
   

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
   

    $scope.ListarPerfis = function (_sistema) {

        $scope.EMP_ID = _sistema;
        var data = { emp_id: _sistema };


        formHandlerService.read($scope, {
            url: Util.getUrl("/Perfil/CarregarPerfis"),
            targetObjectName: 'lstPerfis',
            responseModelName: 'listaperfis',
            data: data,
            success: function () {

                $scope.inicializarPerfis();

                if ($scope.usuario) {

                    $scope.acharPerfilAdicionado($scope.usuario.PERFIL_USUARIO);
                }
            }
        });

    };

    $scope.inicializarPerfis = function () {

        angular.forEach($scope.lstPerfis, function (value, old) {

            value.show = true;
        });

    };


    $scope.adicionarPerfil = function (perfil) {

        if (perfil && $scope.usuario && $scope.usuario.PERFIL_USUARIO) {

            if ($scope.verificaDuplicacaoPerfil(perfil)) {

                var empId = $scope.EMP_ID;
                $scope.usuario.PERFIL_USUARIO.push({ PERFIL: angular.copy(perfil), PER_ID: perfil.PER_ID, EMP_ID: empId });
            }
            else {

                $scope.message = Util.createMessage("fail", "Perfil já adicionado!");
            }

        }
    };

    $scope.verificaDuplicacaoPerfil = function (item) {

        var achou = true;
        if (item && $scope.usuario && $scope.usuario.PERFIL_USUARIO) {

            angular.forEach($scope.usuario.PERFIL_USUARIO, function (value, index) {

                if (value.PERFIL.PER_ID === item.PER_ID) {

                    achou = false;
                    return;
                }
            });
        }

        return achou;
    };

    $scope.$watch("usuario.PERFIL_USUARIO", function (value, old) {

        if (value) {
            $scope.acharPerfilAdicionado(value);
        }

    }, true);

    $scope.acharPerfilAdicionado = function (perfilUsuario) {

        $scope.inicializarPerfis();

        if (perfilUsuario) {

            angular.forEach($scope.lstPerfis, function (value, old) {

                angular.forEach(perfilUsuario, function (subValue, subOld) {

                    if (subValue.PERFIL && value.PER_ID == subValue.PERFIL.PER_ID) {

                        value.show = false;
                    }
                });
            });
        }

    };

    $scope.excluirPerfil = function (index) {

        if ($scope.usuario && $scope.usuario.PERFIL_USUARIO) {

            $scope.usuario.PERFIL_USUARIO.splice(index, 1);

        }
    };

    $scope.readPerfis = function (usuLogin) {


        if (usuLogin) {
            var url = Util.getUrl("/perfil/PerfilsDoUsuario");


            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'perfilUsuario',
                responseModelName: 'perfilUsuario',
                data: { usu_login: usuLogin },
                success: function () {

                }
            });
        }
    }

    $scope.abreModalUsuario = function (cli) {

        if (cli && cli.PERFIL_USUARIO) {

            $scope.perfis = cli.PERFIL_USUARIO;
        }

        angular.element("#perfis-modal").modal();

    }

});




