appModule.controller('PerfilController', function ($scope, formHandlerService, $timeout) {
    

    $scope.listarPerfis = function (pagina) {

        $scope.listado = false;
        var url = Util.getUrl("/perfil/perfis");

        if (pagina) {

            url += "?pagina=" + pagina;
        }

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'perfis',
            responseModelName: 'perfis',
            data: $scope.filtro,
            showAjaxLoader: true,
            pageConfig: { pageName: 'page' /*, pageTargetName: 'paginaPrioridades' */},
            success: function (resp) {
                $scope.listado = true;
            }
        });
    }

    $scope.read = function (PER_ID, editar) {

        if (editar == 1) {

            $scope.editar = true;
        }
        else {
            $scope.editar = false;
        }
        
        if (PER_ID) {
            var url = Util.getUrl("/perfil/RecuperarDadosDoPerfil");

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'perfil',
                responseModelName: 'perfil',
                showAjaxLoader: true,
                data: { PER_ID: PER_ID },
                success: function () {

                }
            });
        }
        else {

            $scope.perfil = {};
        }
    }


    $scope.salvar = function () {

        var url = Util.getUrl("/perfil/salvar");

        formHandlerService.submit($scope, {
            url: url,
            objectName: 'perfil',
            success: function (resp, status, config, message, validationMessage) {
                $scope.message = message;
                $scope.erros = validationMessage;

                if (resp.success) {

                    $scope.message = Util.createMessage("success", "Salvo com sucesso!");
                    $timeout(function () {

                        $scope.message = null;
                        window.location = Util.getUrl("/perfil/index");

                    }, 1000);
                }
                else {
                    $scope.button = 'reset';                   
                }

            }
        });
    }


   
});