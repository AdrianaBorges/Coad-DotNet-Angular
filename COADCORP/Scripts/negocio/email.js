appModule.controller('EmailController',
    ['$scope', 'formHandlerService', 'FilterService', '$sce', function ($scope, formHandlerService, FilterService, $sce) {

    $scope.CarregarEmails = function () {

        var url = Util.getUrl("/email/carregarEmails");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstEmails',
            responseModelName: 'lstEmails',
            showAjaxLoader: true,
            orderProp: 'Id',
            success: function (resp)
            {

                $scope.SelecionarEmail("0");

            }

        });

    };


    $scope.SelecionarEmail = function (value) {

        var url = Util.getUrl("/email/selecionarEmail");

        if (value) {

            url += "?id=" + value;
        }


        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'emailSelecionado',
            responseModelName: 'emailSelecionado',
            showAjaxLoader: true,
            data: $scope.filtro,
            pageConfig: { pageName: 'page' /*, pageTargetName: 'pageAgendamentoDoDia' */ },
            success: function (resp)
            {

                if ( value == "0" ) document.getElementsByName("assuntos")[0].checked = true;

            }

        });

    };

    $scope.trustHTML = function (value) {

        var trustedContent = $sce.trustAsHtml(value);
        return trustedContent;

    }
    
    $scope.enviarEmail = function () {

        formHandlerService.submit($scope, {
            url: Util.getUrl("/email/enviarEmail"),
            objectName: 'email',
            showAjaxLoader: true,
            success: function (resp, status, config, message, validationMessage) {

                $scope.message = message;
                $scope.erros = validationMessage;

                $scope.button = 'reset';

                if (resp.success) {

                    $scope.message = null;

                    window.open(Util.getUrl('/Email/index'), '_self');

                }
            }
        });
    }

    $scope.init = function () {

        $scope.CarregarEmails();
        $scope.filtro = { selecionarEmail: true };

    }

    $scope.initEnviar = function () {

        var url = Util.getUrl("/email/iniciarEmail");


        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'email',
            responseModelName: 'email',
            showAjaxLoader: true,
            data: $scope.filtro,
            pageConfig: { pageName: 'page' /*, pageTargetName: 'pageAgendamentoDoDia' */ },
            success: function (resp) {

            }

        });

    }
  
}]);