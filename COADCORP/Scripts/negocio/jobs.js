appModule.controller("JobsController", ['$scope', '$http', 'formHandlerService', 'MathService', '$timeout',
    'UploadAjax', 'Upload', 'FilterService', '$sce', '$interval',
function ($scope, $http, formHandlerService, MathService, $timeout,
    UploadAjax, Upload, FilterService, $sce, $interval) {
    
    $scope.initList = function () {

        $scope.intervalPromisse = $interval(function () {
            
            $scope.pesquisarJobAgendamento($scope.paginaAtual);
            
        }, 10000);

        $scope.filtro = { pesquisaCpfCnpjPorIqualdade: true };
    }

    $timeout(function () {

        window.location.reload();
    }, 300000);

    //$scope.init = function (tplId) {
    //    if (tplId) {
    //        $scope.read(tplId, function () {
                 
    //        });
    //    }
    //    else {
    //        $scope.templateHTML = {};

    //    }
    //    $scope.listarTemplateGrupo();
        
    //};

    $scope.pesquisarJobAgendamento = function (pagina) {

        $scope.paginaAtual = pagina;
        $scope.listado = true;
        var url = Util.getUrl("/jobs/PesquisarJobs");

        if (pagina) {

            url += "?pagina=" + pagina;
        }

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstJobs',
            responseModelName: 'lstJobs',
            showAjaxLoader: false,
            data: $scope.filtro,
            pageConfig: { pageName: 'page' /*, pageTargetName: 'pageAgendamentoDoDia' */ },
            success: function (resp) {

            }
        });
    };
    $scope.ligarDesligarJob = function (jobId) {

        if ($scope.intervalPromisse) {
            $interval.cancel($scope.intervalPromisse);
            $scope.intervalPromisse = undefined;
        }

        $timeout(function () {

        if (jobId) {            
                $scope.ligarDesligarRequest = {
                    jobId: jobId
                };

                formHandlerService.submit($scope, {
                    url: Util.getUrl("/jobs/LigarDesligarJob"),
                    objectName: 'ligarDesligarRequest',
                    showAjaxLoader: true,
                    success: function (resp, status, config, message, validationMessage) {
                        $scope.initList();           
                    },
                    fail: function () {

                    }
                });
            }
        });
    }

    $scope.executarManualmenteJob = function (jobId) {

        if (confirm("Agendar agora?")) {
            if (jobId) {
                $scope.executarManualmente = {
                    jobId: jobId
                };

                formHandlerService.submit($scope, {
                    url: Util.getUrl("/jobs/executarManualmenteJob"),
                    objectName: 'executarManualmente',
                    showAjaxLoader: true,
                    success: function (resp, status, config, message, validationMessage) {
                    
                        $scope.message = message;
                        $timeout(function () {
                            $scope.message = null;

                        }, 1000);
                    },
                    fail: function () {

                    }
                });
            }
        }
    }
    
}]);