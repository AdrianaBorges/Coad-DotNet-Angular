appModule.controller('RepresentanteController', ['$scope', 'formHandlerService', '$http', 'conversionService', '$interval', 'cepService', '$timeout',

    function ($scope, formHandlerService, $http, conversionService, $interval, cepService, $timeout) {
    
    //$scope.$watch("filtro.data", function (value, old) {

    //    if (value) {

    //        $scope.loadRelatoriosDePassivos();
    //    }
    //}, true);

    $scope.filtro = {};
    $scope.queryCarteira = {

        Deletar: false
    };
    var now = new Date();
    var dataatual = new Date(now.getFullYear(), now.getMonth(), now.getDate());

    $scope.iniRelRepresentanteCarteira = function () {

        $scope.filtro.dtini = new Date(now.getFullYear(), now.getMonth(), 1);
        $scope.filtro.dtfim = new Date(now.getFullYear(), now.getMonth(), now.getDate());
        $scope.filtro.emp_id = 2;
        $scope.filtro.ordalfabetica = false;
        
        $scope.dataHoraFormatada();

    }
    $scope.initRelatorioPassivos = function () {

        $scope.filtro = { data: new Date() };
        angular.element("#wrapper").addClass('toggled');
    }

    $scope.$watch("representante.USUARIO", function (value, old) {

        if ((value && !value.CADASTRADO_POR)) {

            if ($scope.USU_LOGIN) {

                value.CADASTRADO_POR = angular.copy($scope.USU_LOGIN);
            }
        }
    });

    $scope.init = function () {

        $scope.filtro = {

            cpfExato: true
        };

        $scope.carregarUENs();
        $scope.carregarEmpresas();
        $scope.carregaComboRegioes();
        $scope.criarFiltros();
    }


    $scope.criarFiltros = function () {

        var url = Util.getUrl("/representante/retonarDadosDeFiltro");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'filtros',
            responseModelName: 'filters',
            showAjaxLoader: true,
            success: function (resp) {

            }
        });
    }

    $scope.initEdicao = function (adm) {

        $scope.representante = {};
        $scope.listarUsuarioDTO();
        $scope.carregarNivelRepresentante();
        $scope.carregarEmpresas();

        if (adm == 'True') {

            $scope.carregarUENs();
            $scope.carregaRegioes();
        }
        else {
            $scope.carregaRegioes();
        }
        
    }

    $scope.carregarEmpresas = function () {

        var url = Util.getUrl("/proposta/listarEmpresas");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstEmpresas',
            responseModelName: 'lstEmpresas',
            showAjaxLoader: true,

        });
    }

    $scope.nivelRepresentanteSelecionado = function () {

        if($scope.representante && $scope.representante.NIVEL_REPRESENTANTE){

            $scope.representante.NRP_ID = $scope.representante.NIVEL_REPRESENTANTE.NRP_ID;
        }        
    }

    $scope.carregaRegioes = function () {

        var parans = {};

        if ($scope.representante && $scope.representante.UEN_ID) {

            parans.UEN_ID = angular.copy($scope.representante.UEN_ID);
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

    $scope.carregaComboRegioes = function (uenId) {

        if ($scope.filtroCart)
            $scope.filtroCart.rgId = null;
        var parans = {uenId : uenId};

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

        if ($scope.representante) {
            $scope.representante.RG_ID = null;
        }
        $scope.carregaRegioes();
        
    }

    $scope.$watch("representante", function (value, old) {

        if (value && value.UEN_ID) {
            $scope.carregaRegioes();
        }
    });

    $scope.pesquisarNotaFiscal = function (pagina) {

        $scope.listado = false;
        var url = Util.getUrl("/representante/PesquisarNotaFiscal");

        if (pagina) {

            url += "?pagina=" + pagina;
        }

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'listanf',
            responseModelName: 'listanf',
            data: $scope.filtro,
            showAjaxLoader: true,
            pageConfig: { pageName: 'page' /*, pageTargetName: 'paginaPrioridades' */},
            success: function (resp) {
                $scope.listado = true;
            }
        });
    }
    
    $scope.listarRepresentantesComUsuario = function (pagina) {

        $scope.listado = false;
        var url = Util.getUrl("/representante/representantesComUsuario");

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

    $scope.read = function (REP_ID, USU_LOGIN) {

        if (USU_LOGIN) {
            
            $scope.USU_LOGIN = USU_LOGIN;
        }

        if (REP_ID) {
            var url = Util.getUrl("/representante/RecuperarDadosDoRepresentante");


            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'representante',
                responseModelName: 'representante',
                data: { REP_ID: REP_ID },
                success: function () {

                    var representante = $scope.representante;
                    if (representante != null && representante.USUARIO != null) {

                        $scope.origemSalvar = "editar";
                    }
                    else {
                        $scope.origemSalvar = "incluir";
                    }
                }
            });
        }
        else {
            $scope.representante = {
                USUARIO: {},
                CARTEIRA_REPRESENTANTE : [],
            };
            $scope.origem = "incluir";
        }
    }
    $scope.listarRepresentantesCarteira = function () {

        showAjaxLoader();

        $scope.iniRelRepresentanteCarteira();

        var url = "/RelRepresentanteCarteira/ListarRepresentantesCarteira";
        $http({
            url: url,
            method: "post"
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.listaRepresentantesCarteira = response.result.listaRepresentantesCarteira;

                conversionService.deepConversion($scope.listaRepresentantesCarteira);
            }
            else {

                $scope.message = Util.createMessage("fail", response.message);
                $scope.listaRepresentantesCarteira = null;

                hideAjaxLoader();
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response.message);
            $scope.listaRepresentantesCarteira = null;

            hideAjaxLoader();
        });
    }
    $scope.BuscarRepresentante = function (pagina) {

        showAjaxLoader();

        var url = "/representante/BuscarRepresentante";
        if (pagina) {

            url += "?pagina=" + pagina;
        }

        $http({
            url: url,
            method: "post",
            data: $scope.filtro,
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.representantes = response.result.representantes;

                conversionService.deepConversion($scope.representantes);

                $scope.page = response.page;
            }
            else {

                $scope.message = Util.createMessage("fail", response.message);
                $scope.representantes = null;

                hideAjaxLoader();
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response.message);
            $scope.representantes = null;

            hideAjaxLoader();
        });
    }


    $scope.salvar = function () {

        var url = ($scope.origemSalvar == 'editar') ? "/franquia/representante/salvar" : "/franquia/representante/incluir";

        formHandlerService.submit($scope, {
            url: url,
            objectName: 'representante',
            showAjaxLoader: true,
            success: function (resp, status, config, message, validationMessage) {
                $scope.message = message;
                $scope.erros = validationMessage;

                if (resp.success) {

                    $scope.message = Util.createMessage("success", "Salvo com sucesso!");
                    $timeout(function () {

                        $scope.message = null;
                        window.location = Util.getUrl("/franquia/representante/listar");

                    }, 1000);
                }
                else {
                    $scope.button = 'save';

                    if ($scope.message.type == 'warning') {

                        $timeout(function () {

                            $scope.message = null;
                            window.location = Util.getUrl("/franquia/representante/listar");

                        }, 6000);
                    }
                }

            }
        });
    }

    $scope.deletarRepresentante = function () {

        if (confirm("Deseja realmente deletar este representante?")) {

            $scope.repId = { REP_ID: $scope.representante.REP_ID };

            formHandlerService.submit($scope, {
                url: Util.getUrl("/franquia/representante/excluir"),
                objectName: 'repId',
                success: function (resp, status, config, message, validationMessage) {
                    $scope.message = message;
                    $scope.erros = validationMessage;
                    if (resp.success) {

                        $scope.message = Util.createMessage("success", "Excluido com sucesso!");
                        $timeout(function () {

                            $scope.message = null;
                            window.location = Util.getUrl("/franquia/representante/listar");

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
       
    $scope.loadRelatoriosDePassivos = function () {

        $scope.listarRelatorioPassivos();
        $scope.listarRelatorioDePassivosRepresentanteLogados();
        $scope.listarSuspectsCadastradosNoDia();
        $scope.listarClientesComPrioridade()
    }

    $scope.listarRelatorioPassivos = function (pagina) {

        $scope.listado = false;
        var url = Util.getUrl("/representante/RelatorioDePassivos");

        if (pagina) {

            url += "?pagina=" + pagina;
        }

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'relatorioPassivos',
            responseModelName: 'relatorioPassivos',
            data: $scope.filtro,
            showAjaxLoader: true,
            pageConfig: { pageName: 'page' /*, pageTargetName: 'paginaPrioridades' */ },
            success: function (resp) {
                $scope.listado = true;
                $scope.relatorioPassivosTotal = angular.copy($scope.relatorioPassivos[$scope.relatorioPassivos.length - 1]);
            }
        });
    }

    $scope.listarRelatorioDePassivosRepresentanteLogados = function (pagina) {

        $scope.listado = false;
        var url = Util.getUrl("/representante/RelatorioDePassivosRepresentanteLogados");

        if (pagina) {

            url += "?pagina=" + pagina;
        }

        formHandlerService.read($scope, {
            url: url,
            targetObjectName:  'relatorioPassivosRepresentanteLogados',
            responseModelName: 'relatorioPassivosRepresentanteLogados',
            data: $scope.filtro,
            showAjaxLoader: true,
            pageConfig: { pageName: 'page' /*, pageTargetName: 'paginaPrioridades' */ },
            success: function (resp) {
                $scope.listado = true;
            }
        });
    }

    $scope.listarSuspectsCadastradosNoDia = function (pagina) {

        $scope.listado = false;
        var url = Util.getUrl("/clientes/SuspectsCadastradosNoDia");

        if (pagina) {

            url += "?pagina=" + pagina;
        }

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstSuspectsCadastradosNoDia',
            responseModelName: 'lstSuspectsCadastradosNoDia',
            data: $scope.filtro,
            showAjaxLoader: true,
            pageConfig: { pageName: 'page' /*, pageTargetName: 'paginaPrioridades' */ },
            success: function (resp) {
                $scope.listado = true;
            }
        });
    }


    $scope.listarClientesComPrioridade = function (pagina) {

        $scope.listado = false;
        var url = Util.getUrl("/clientes/ClientesComPrioridade");

        if (pagina) {

            url += "?pagina=" + pagina;
        }

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstClientesComPrioridade',
            responseModelName: 'lstClientesComPrioridade',
            data: $scope.filtro,
            showAjaxLoader: true,
            pageConfig: { pageName: 'page' /*, pageTargetName: 'paginaPrioridades' */ },
            success: function (resp) {
                $scope.listado = true;
            }
        });
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

    $scope.usuarioEscolhido = function () {
        
        //if ($scope.representante.USU_LOGIN) {

        //    $scope.escolhidoPorAutoComp
        //}
    }


    $scope.recuperarDadosDoUsuario = function () {


        if (Util.isPathValid($scope, "representante.USU_LOGIN")) {

            var url = Util.getUrl("/usuario/RecuperarDadosDoUsuario");


            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'usuario',
                responseModelName: 'usuario',
                showAjaxLoader: true,
                data : {usu_login : $scope.representante.USU_LOGIN},
                success: function (response) {
                    
                    $scope.message = response.message;

                    if (response.success) {

                        if ($scope.usuario) {

                            $scope.origemSalvar = "editar";
                            $scope.representante.USUARIO = $scope.usuario;
                            $scope.representante.USUARIO.REP_ID = $scope.representante.REP_ID;
                        }
                    }
                    
                }
            });
        }
    }

    $scope.removerUsuario = function () {

        if (Util.isPathValid($scope, "representante.USUARIO")) {

            $scope.origemSalvar = "incluir";
            $scope.representante.USUARIO = null;
        }
    }

    $scope.dataHoraFormatada = function (dataHora) {

        if (dataHora == null)
            dataHora = new Date();


        var data = dataHora;
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

        $scope.filtro.dataatual = dia + "/" + mes + "/" + ano + " " + hora;

        return dia + "/" + mes + "/" + ano + " " + hora;
    }


    $scope.abrirModalUsuario = function () {
        
        $scope.filtroUsuario = { cpfExato: true };
        angular.element('#modal-usuario').modal();
    }

    $scope.abrirModalCarteira = function (index) {

        $scope.contextoModalCarteira = { index: index };
        $scope.filtroCart = {};
     //   $scope.carregaComboRegioes();
        angular.element('#modal-carteira').modal();
    }
    $scope.adicionarUsuario = function (usuario) {

        $scope.representante.USUARIO = usuario;
        angular.element("#modal-usuario").modal('hide');
    }

    $scope.listarUsuarios = function (pagina) {

        $scope.usuarioListado = false;
        var url = Util.getUrl("/usuario/ListarUsuariosSemRepresentante");

        if (pagina) {

            url += "?pagina=" + pagina;
        }

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstUsuarios',
            responseModelName: 'lstUsuarios',
            data: $scope.filtroUsuario,
            showAjaxLoader: true,
            pageConfig: { pageName: 'page' , pageTargetName: 'paginaUsuario'},
            success: function (resp) {
                $scope.usuarioListado = true;
            }
        });
    }


    $scope.adicionarLinhaCarteira = function () {

        if (Util.isPathValid($scope, 'representante.CARTEIRA_REPRESENTANTE') && $scope.representante.CARTEIRA_REPRESENTANTE.length > 0) {

            var lista = $scope.listaDataFiltrada;
            var value = lista[lista.length - 1];
            if (lista.length <= 0 || (value && value.CAR_ID)) {

                $scope.representante.CARTEIRA_REPRESENTANTE.push({ REP_ID: $scope.representante.REP_ID, Deletar: false });
            }
            else {

                $scope.message = Util.createMessage("fail", "Não é possível adicionar mais uma linha até que a linha anterior esteja correta.");
            }
        }
        else {

            $scope.representante.CARTEIRA_REPRESENTANTE = [{ REP_ID: $scope.representante.REP_ID, Deletar: false }];
        }
    };

    $scope.removerCarteira = function ($index) {
        var obj = $scope.listaDataFiltrada[$index];

        if (obj) {
            if (!obj.CAR_ID || !obj.EMP_ID) {

                obj.Remover = true;
                var lstCarteiraCopy = angular.copy($scope.representante.CARTEIRA_REPRESENTANTE);

                angular.forEach(lstCarteiraCopy, function (value, index) {

                    if (value.Remover == true) {
                        $scope.representante.CARTEIRA_REPRESENTANTE.splice(index, 1);
                    }
                });
            }
            else {
                obj.Deletar = true;
            }
        }


    };

    $scope.listarCarteiras = function (pagina) {

        $scope.carteiraListada = false;
        var url = Util.getUrl("/franquia/carteiramento/buscarCarteiras");

        if (pagina) {

            url += "?pagina=" + pagina;
        }

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

                if (Util.isPathValid($scope, 'representante.CARTEIRA_REPRESENTANTE')) {
                    $scope.listaDataFiltrada[index].CARTEIRA = car;
                    $scope.listaDataFiltrada[index].CAR_ID = car.CAR_ID;
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

    $scope.checarCarteiraValida = function (carId, carRep) {

        formHandlerService.read(carRep, {
            url: Util.getUrl("/carteiramento/ChecarCarteiraValida"),
            targetObjectName: 'valida',
            responseModelName: 'valida',
            data: { carId: carId },
            success: function () {
            }
        });

    }

}]);