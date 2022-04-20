appModule.controller('ProfessorController', function ($scope, formHandlerService, $http, conversionService, $interval, cepService, $timeout, $parse) {
    
    $scope.$watch("filtro.data", function (value, old) {

        if (value) {

            $scope.loadRelatoriosDePassivos();
        }
    }, true);

    $scope.initRelatorioPassivos = function () {

        $scope.filtro = { data: new Date() };
    }

    $scope.$watch("professor.USUARIO", function (value, old) {

        if ((value && !value.CADASTRADO_POR)) {

            if ($scope.USU_LOGIN) {

                value.CADASTRADO_POR = angular.copy($scope.USU_LOGIN);
            }
        }
    });

    $scope.$watch("professor", function (value, old) {

        if (value && value.UEN_ID) {
            $scope.carregaRegioes();
        }
    });


    $scope.init = function (adm) {

        if (adm == 'True') {

            $scope.carregarUENs();
        }
        else {
            $scope.carregaRegioes();
        }
    }

    $scope.initEdicao = function (adm) {

        $scope.professores = {};
        $scope.listarUsuarioDTO();
        $scope.matrizGrandeGrupo = [[]];
        
        $scope.listarColecionador();
        //$scope.listarGrandeGrupo();
        $scope.init(adm);
        
    }

    $scope.carregaRegioes = function () {

        var parans = {};

        if (($scope.professor && $scope.professor.UEN_ID)) {

            parans.UEN_ID = angular.copy($scope.professor.UEN_ID);
        }

        if ($scope.filtro && $scope.filtro.UEN_ID) {

            parans.UEN_ID = angular.copy($scope.filtro.UEN_ID)
        }
        
        var url = Util.getUrl('/regiao/regioes');
        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'regioes',
            responseModelName: 'lstRegiao',
            data : parans,
            success: function () {

            }
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

    $scope.carregarNivelRepresentante = function () {

        var url = Util.getUrl('/NivelRepresentante/ListarNiveisRepresentante');
        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstNivelRepresentante',
            responseModelName: 'lstNivelRepresentante',
            success: function () {

            }
        });

    }

    $scope.uenSelecionado = function(){

        if ($scope.professor) {
            $scope.professor.RG_ID = null;
        }
        $scope.carregaRegioes();
        
    }

    $scope.listarProfessoresComUsuario = function (pagina) {

        $scope.listado = false;
        var url = Util.getUrl("/professor/listarProfessoresComUsuario");

        if (pagina) {

            url += "?pagina=" + pagina;
        }
               
        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'professores',
            responseModelName: 'professores',
            data: $scope.filtro,
            showAjaxLoader: true,
            pageConfig: { pageName: 'page' /*, pageTargetName: 'paginaPrioridades' */ },
            success: function (resp) {
                $scope.listado = true;
            }
        });
    }

    $scope.read = function (REP_ID, USU_LOGIN) {

        if (USU_LOGIN) {
            
            $scope.USU_LOGIN = USU_LOGIN;
        }

        if (REP_ID) {
            var url = Util.getUrl("/professor/recuperarDadosDoProfessor");


            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'professor',
                responseModelName: 'professor',
                data: { REP_ID: REP_ID },
                success: function () {

                    var professor = $scope.professor;
                    if (professor != null && professor.USUARIO != null) {

                        $scope.origemSalvar = "editar";
                    }
                    else {
                        $scope.origemSalvar = "incluir";
                    }

                    if (!$scope.professor.USUARIO) {

                        $scope.professor.USUARIO = {};
                    }
                }
            });
        }
        else {
            $scope.professor = { USUARIO: {}, AREA_CONSULTORIA_REPRESENTANTE_PROXY : []};
            $scope.origem = "incluir";
        }
    }

    $scope.salvar = function () {

        var url = ($scope.origemSalvar == 'editar') ? "/franquia/professor/salvar" : "/franquia/professor/incluir";

        formHandlerService.submit($scope, {
            url: url,
            objectName: 'professor',
            success: function (resp, status, config, message, validationMessage) {
                $scope.message = message;
                $scope.erros = validationMessage;

                if (resp.success) {

                    $scope.message = Util.createMessage("success", "Salvo com sucesso!");
                    $timeout(function () {

                        $scope.message = null;
                        window.location = Util.getUrl("/franquia/professor/index");

                    }, 1000);
                }
                else {
                    $scope.button = 'save';

                    if ($scope.message.type == 'warning') {

                        $timeout(function () {

                            $scope.message = null;
                            window.location = Util.getUrl("/franquia/professor/index");

                        }, 6000);
                    }
                }

            }
        });
    }

    $scope.deletarProfessor = function () {

        if (confirm("Deseja realmente deletar este professor?")) {

            $scope.repId = { REP_ID: $scope.professor.REP_ID };

            formHandlerService.submit($scope, {
                url: Util.getUrl("/franquia/professor/excluir"),
                objectName: 'repId',
                success: function (resp, status, config, message, validationMessage) {
                    $scope.message = message;
                    $scope.erros = validationMessage;
                    if (resp.success) {

                        $scope.message = Util.createMessage("success", "Excluido com sucesso!");
                        $timeout(function () {

                            $scope.message = null;
                            window.location = Util.getUrl("/franquia/professor/index");

                        }, 1000);
                    }
                    else {
                        $scope.buttonDel = 'reset';                        
                    }

                }
            });

        }
        else {
            $scope.buttonDel = 'reset';
        }

    }
          

    $scope.listarUsuarioDTO = function (pagina) {

        var url = Util.getUrl("/usuario/ListaDeUsuarioSemRepresentante");
        
        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'listaUsuario',
            responseModelName: 'listaUsuario',
            showAjaxLoader: true,
            success: function (resp) {
            }
        });
    }

    $scope.recuperarDadosDoUsuario = function () {


        if (Util.isPathValid($scope, "professor.USU_LOGIN")) {

            var url = Util.getUrl("/usuario/RecuperarDadosDoUsuario");


            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'usuario',
                responseModelName: 'usuario',
                showAjaxLoader: true,
                data: { usu_login: $scope.professor.USU_LOGIN },
                success: function (response) {
                    
                    $scope.message = response.message;

                    if (response.success) {

                        if ($scope.usuario) {

                            $scope.origemSalvar = "editar";
                            $scope.professor.USUARIO = $scope.usuario;
                            $scope.professor.USUARIO.REP_ID = $scope.professor.REP_ID;
                        }
                    }
                    
                }
            });
        }
    }

    $scope.removerUsuario = function () {

        if (Util.isPathValid($scope, "professor.USUARIO")) {

            $scope.origemSalvar = "incluir";
            $scope.professor.USUARIO = null;
        }
    }

    $scope.listarColecionador = function () {

        var url = Util.getUrl("/franquia/professor/LstColecionadores");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstColecionadores',
            responseModelName: 'lstColecionadores',
            showAjaxLoader: true,
            success: function (resp) {
            }
        });
    }

    $scope.listarGrandeGrupo = function (value, index) {

        var filter = { areConsId: value };
        var targetName = 'matrizGrandeGrupo[' + index + '].lstGrandeGrupo';

        if (value) {
            
            var url = Util.getUrl("/franquia/professor/LstGrandeGrupo");

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: targetName,
                responseModelName: 'lstGrandeGrupo',
                showAjaxLoader: true,
                data: filter,
                success: function (resp) {
                }
            });
        }
        else {
            var setter = $parse(targetName);
            setter.assign($scope, []);
        }
    }

    $scope.colecionadorSelecionado = function (value, index) {

        $scope.listarGrandeGrupo(value, index);
    }

    $scope.IncluirAreas = function () {

        if (Util.isPathValid($scope, 'professor.AREA_CONSULTORIA_REPRESENTANTE_PROXY')) {

            var lstLength = $scope.professor.AREA_CONSULTORIA_REPRESENTANTE_PROXY.length;

            if (lstLength > 0) {
                var lastObj = $scope.professor.AREA_CONSULTORIA_REPRESENTANTE_PROXY[lstLength - 1];

                if (!lastObj.ARE_CONS_ID) {

                    $scope.message = Util.createMessage("fail", "Selecine pelo menos o colecionador antes de adicionar uma nova linha.");
                    return;
                }
            }
            var repId = $scope.professor.REP_ID;
            $scope.professor.AREA_CONSULTORIA_REPRESENTANTE_PROXY.push({ REP_ID: repId });
        }
    }

    $scope.ExcluirAreas = function (index) {

        if(Util.isPathValid($scope, 'professor.AREA_CONSULTORIA_REPRESENTANTE_PROXY')){

            $scope.professor.AREA_CONSULTORIA_REPRESENTANTE_PROXY.splice(index, 1);
        }

        angular.forEach($scope.professor.AREA_CONSULTORIA_REPRESENTANTE_PROXY, function (value, index) {

            if (value && value.ARE_CONS_ID) {
                $scope.colecionadorSelecionado(value.ARE_CONS_ID, index)
            }
        });
    }

    
});

