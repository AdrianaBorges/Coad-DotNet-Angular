
appModule.controller('FranquiaController', function ($scope, formHandlerService, $http, conversionService, $interval, cepService, $timeout, Upload, FilterService) {
        
    $scope.step = 1;    
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

    $scope.initList = function () {

        $scope.tab = 0;
        $scope.filtroImportacoesSuspect = {};
        $scope.criarFiltros();
        //$scope.carregarImportacaoStatus();
    }

    $scope.init = function (impID) {
        $scope.modalImportacao = { impID: impID };
        $scope.retonarDadosDaImportacao();
        $scope.retonarProgressoDoJob();

    }

    $scope.gerarPreviaDeImportacao = function () {

        var url = Util.getUrl("/importacaosuspect/GerarPreviaDeImportacao");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'previaImportacao',
            responseModelName: 'previaImportacao',
            showAjaxLoader: true,
            //pageConfig: { pageName: 'page' /*, pageTargetName: 'pageAgendamentoDoDia' */ },
            success: function (resp) {

            }
        });
    };
    
    $scope.uploadPlanilhaSuspect = function (file, errFiles) {
            
            showAjaxLoader();
            $scope.f = file;
            $scope.errFiles = errFiles && errFiles[0];

            if (file) {

                file.upload = Upload.upload({
                    url: Util.getUrl("/importacaosuspect/ReceberUploadPlanilhaSuspect"),
                    data: { file: file }
                });

            }

            file.upload.then(function (response) {

                $timeout(function () {

                    var data = response.data;
                    file.result = data;
                    $scope.message = data.message;
                    $scope.f.progress = null;
                    $scope.step = 2;

                    hideAjaxLoader();
                });

                //$timeout(function () {

                //    file.progress = 0;
                //}, 1000);

            }, function (response) {

                hideAjaxLoader();
                if (response.status > 0) {
                    $scope.errorMsg = response.status + ': ' + response.data;
                }
            }, function (evt) {
                $scope.filename = evt.config.data.file.name;
                 
                file.progress = Math.min(100, parseInt(100.0 * evt.loaded / evt.total));

            });
            //var data = { IPE_ID: IPE_ID };
            //var url = Util.getUrl("/pedido/ReceberUpload");

            //UploadAjax.upload(url, $scope.modalNFe.chaveNFe, data);


    }

    $scope.acompanharStatusBatch = function () {

        var intervalPromise =
            $interval(function () {
            var url = Util.getUrl("/batch/RetornarStatusDeBatchImportacaoSuspect");
            
                $http({
                    url: url,
                    method: 'POST'
                })
                .then(function (response) {

                    if (response.data.result != null) {

                        $scope.batchStatus = response.data.result.batchStatus;
                    }
                }, function (response){

            });
            }, 300,
            0,
            false);

        return intervalPromise;
    }

    $scope.agendarImportacaoDeSuspect = function () {

        if (confirm("Deseja realmente agendar a importar?")) {

       
            $scope.mockObj = { foo: true };

            formHandlerService.submit($scope, {
                url: Util.getUrl("/importacaoSuspect/agendarImportacaoDeSuspect"),
                objectName: 'mockObj',
                showAjaxLoader: true,
                success: function (resp, status, config, message, validationMessage) {


                    $scope.buttonImport = 'reset';

                    $scope.message = message;
                    $scope.erros = validationMessage;
                    $scope.step = 4;

                    if (resp.result.importacao) {

                        $scope.importacao = resp.result.importacao;
                        $scope.modalImportacao = { impID: resp.result.importacao.IMP_ID, aguardandoExecucao : true };
                    }

                    if (resp.success) {

                        $scope.message = Util.createMessage("success", "Importação agendada com sucesso!!");

                        $timeout(function () {
                            $scope.message = null;
                            //window.open(Util.getUrl('/pedido/index'), '_self');                        

                        }, 5000);

                    }

                }
            });
        }
        else {
            return false;
        }
    }

    $scope.pesquisar = function (pagina) {

        $scope.paginaAtual = pagina;
        $scope.listado = true;
        var url = Util.getUrl("/importacaoSuspect/pesquisarImportacoes");

        if (pagina) {

            url += "?pagina=" + pagina;
        }

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstImportacoes',
            responseModelName: 'lstImportacoes',
            showAjaxLoader: true,
            data: $scope.filtro,
            pageConfig: { pageName: 'page' /*, pageTargetName: 'pageAgendamentoDoDia' */ },
            success: function (resp) {

            }
        });
    };

    $scope.criarFiltros = function () {

        var url = Util.getUrl("/gerenciaFranquia/retonarDadosDeFiltro");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'filters',
            responseModelName: 'filters',
            showAjaxLoader: true,
            success: function (resp) {

            }
        });
    }


    $scope.abrirModalUploadPlanilhaErro = function (impID) {

        $scope.modalUpload = { impID: impID };
        $scope.batchResp = null;
        
        angular.element("#modal-upload-planilha").modal();
    }


});