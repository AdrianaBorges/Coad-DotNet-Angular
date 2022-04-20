appModule.controller('UenController', ['$scope', 'formHandlerService', '$http', 'conversionService', '$interval', '$timeout', 'UenService',
    function ($scope, formHandlerService, $http, conversionService, $interval, $timeout, UenService) {
    
    $scope.init = function (uenId) {

        $scope.open = false;
        $scope.carregarUENs();
        $scope.buscarUEN(uenId);
    }

    
    $scope.abrirPainelUen = function () {

        $scope.open = !$scope.open;
    }


    $scope.buscarUEN = function (uenId) {

        var url = Util.getUrl('/UEN/buscarUEN');
        formHandlerService.read($scope, {
            url: url,
            data: {uenId : uenId},
            targetObjectName: 'uen',
            responseModelName: 'uen',
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
$scope.trocarUen = function () {

        var url = Util.getUrl("/franquia/uen/TrocarUen");

        formHandlerService.submit($scope, {
            url: url,
            objectName: 'uen',
            success: function (resp, status, config, message, validationMessage) {
                $scope.message = message;
                $scope.erros = validationMessage;

                if (resp.success) {

                    $scope.button = 'reset';
                    $scope.message = Util.createMessage("success", "Salvo com sucesso!");
                    $scope.uenAtual = angular.copy(resp.result.uen);

                    UenService.notificarUenAuterada($scope.uenAtual);
                    
                    $timeout(function () {

                        $scope.message = null;
                        $scope.open = false;

                        if (!UenService.scopes)
                            location.reload();
                        
                    }, 1000);
                }
                else {
                    $scope.button = 'reset';
                    $scope.open = false;
                }

            }
        });
    }
    
    
}]);