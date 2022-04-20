appModule.controller('EmailController', function ($scope, formHandlerService, $http, conversionService, cepService, $timeout) {

    $scope.init = function () {

        $scope.email = {

            Host: "smtp.gmail.com",
            Port: 587,
            User: "coadcorp@coad.com.br",
            Senha: "C04dc0rp@",
            From: "coadcorp@coad.com.br",
            EnableSsl: true,
        };
    };
    
    /*
    *-------------------------------------------------------- Inicializações ----------------------------------------------------------------------
    */


    $scope.enviarEmail = function () {

        var url = "/home/enviaremail"
        
        formHandlerService.submit($scope, {
            url: Util.getUrl(url),
            objectName: 'email',
            success: function (resp, status, config, message, validationMessage) {
                $scope.message = message;
                $scope.erros = validationMessage;
                $scope.erro = resp.result.erro;
                $scope.button = 'send';
            }
        });
    }    
  

});




