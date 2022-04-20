appModule.controller('DepartamentoController', function ($scope, formHandlerService, $http, conversionService, cepService, $timeout) {

    $scope.filtro = {};
 

    
    /*
    *-------------------------------------------------------- Inicializações ----------------------------------------------------------------------
    */

    $scope.init = function () {

        $scope.query = { show: true };
    }
    
    $scope.read = function (DP_ID) {
              
        if (DP_ID) {
            var url = Util.getUrl("/departamento/RecuperarDadosDoDepartamento");
            

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'departamento',
                responseModelName: 'departamento',
                data: { DP_ID: DP_ID },
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
    
   

    $scope.salvar = function () {

      formHandlerService.submit($scope, {
            url: Util.getUrl("/departamento/salvar"),
            objectName: 'departamento',
            success: function (resp, status, config, message, validationMessage) {
                $scope.message = message;
                $scope.erros = validationMessage;

                if (resp.success) {

                    $scope.message = Util.createMessage("success", "Salvo com sucesso!");
                    $timeout(function () {

                        $scope.message = null;
                        window.location = Util.getUrl("/departamento/index");

                    }, 1000);
                }
                else {
                    $scope.button = 'save';

                    if ($scope.message.type == 'warning') {

                        $timeout(function () {

                            $scope.message = null;
                            window.location = Util.getUrl("/departamento/index");

                        }, 6000);
                    }
                }
               
            }
        });
    }
  
    $scope.listar = function (pageRequest) {

        $scope.message = null;
         
       var url = Util.getUrl("/departamento/departamentos");

        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }

        var config = {
            url: url,
            targetObjectName: 'lstDepartamentos',
            responseModelName: 'lstDepartamentos',
            showAjaxLoader: true,
            pageConfig: { pageName: 'page' }
        };

        if ($scope.filtro) {

            if ($scope.filtro.nome) {

                config.data = angular.copy($scope.filtro);
            }
            
        }
        formHandlerService.read($scope, config);
    };

    $scope.deletarDepartamento = function () {

        if (confirm("Deseja realmente deletar este departamento?")) {

            $scope.dpIdDeletar = { DP_ID: $scope.departamento.DP_ID } ;

            formHandlerService.submit($scope, {
                url: Util.getUrl("/departamento/excluir"),
                objectName: 'dpIdDeletar',
                success: function (resp, status, config, message, validationMessage) {
                    $scope.message = message;
                    $scope.erros = validationMessage;
                    if (resp.success) {

                        $scope.message = Util.createMessage("success", "Excluido com sucesso!");
                        $timeout(function () {

                            $scope.message = null;
                            window.location = Util.getUrl("/departamento/index");

                        }, 1000);
                    }
                    else {
                        $scope.buttonDel = 'reset';

                        if ($scope.message.type == 'warning') {

                            $timeout(function () {

                                $scope.message = null;
                                window.location = Util.getUrl("/departamento/index");

                            }, 6000);
                        }
                    }

                }
            });

        }
        else {
            $scope.buttonDel = 'reset';
        }
       
    }
});




