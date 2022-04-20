
appModule.controller('CnabController', function ($scope, formHandlerService, $http, conversionService, $interval, cepService, $timeout, Upload, FilterService) {
        
    $scope.step = 1;
    $scope.init = function () {

        $scope.carregarBancos();
        $scope.carregarEmpresas();
    }

    $scope.initEdicao = function (cncID) {
        if (cncID) {
            $scope.read(cncID);
        }
        else {
            $scope.cnabConfig = {

                CNC_CODIGO_CNAB: "400",
                CNC_ARQUIVO: "1REMESSA",
                CNAB_CONFIG_ARQUIVO: []
            };
        }
        
        $scope.carregarBancos();
        $scope.carregarEmpresas();
        $scope.carregarTipoRegistro();
        $scope.carregarTipoDados();
    };

    $scope.read = function (cncID, onSuccess) {

        if (cncID    != null) {
            var url = Util.getUrl("/cnab/RecuperarDadosDoCnabConfig");

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'cnabConfig',
                responseModelName: 'cnabConfig',
                showAjaxLoader: true,
                data: { cncID: cncID }
            });
        }
    }
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


    $scope.pesquisarCnabConfig = function (pagina) {

        $scope.paginaAtual = pagina;
        $scope.listado = true;
        var url = Util.getUrl("/cnab/pesquisarCnabConfig");

        if (pagina) {

            url += "?pagina=" + pagina;
        }

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstCnabConfig',
            responseModelName: 'lstCnabConfig',
            showAjaxLoader: true,
            data: $scope.filtro,
            pageConfig: { pageName: 'page' /*, pageTargetName: 'pageAgendamentoDoDia' */ },
            success: function (resp) {

            }
        });
    };

    $scope.abrirModalUploadPlanilha = function (index) {

        $scope.modalUpload = { cnabCfgArqIndex: index };
        angular.element("#modal-upload-planilha").modal();
    }

    $scope.uploadPlanilha = function (file, errFiles) {

        $scope.f = file;
        $scope.errFiles = errFiles && errFiles[0];

        if (file) {

            file.upload = Upload.upload({
                url: Util.getUrl("/cnab/receberUploadPlanilhaCarga"),
                data: { file: file }
            });

        }

        file.upload.then(function (response) {

            $timeout(function () {

                var data = response.data;
                file.result = data;
                $scope.uploaded = true;

            });
        }, function (response) {

            if (response.status > 0) {
                $scope.errorMsg = response.status + ': ' + response.data;
            }
        }, function (evt) {
            file.progress = Math.min(100, parseInt(100.0 * evt.loaded / evt.total));

        });


    };
     
    $scope.inserirAtualizarPlanilhaCarga = function (file) {

        if (confirm("Deseja atualizar os dados?")) {

            showAjaxLoader();
            //var file = $scope.file;
            $scope.file = file;
            if (file) {

                file.upload = Upload.upload({
                    url: Util.getUrl("/cnab/inserirAtualizarPlanilhaCarga"),
                    data: { file: $scope.file }
                });

                file.upload.then(function (response) {

                    $timeout(function () {

                        var data = response.data;
                        file.result = data;
                        $scope.message = data.message;
                        $scope.file.progress = null;
                        $scope.step = 2;

                        $scope.buttonUpload = 'reset';

                        if ($scope.modalUpload &&
                                $scope.cnabConfig.CNAB_CONFIG_ARQUIVO &&
                                ($scope.modalUpload.cnabCfgArqIndex || $scope.modalUpload.cnabCfgArqIndex == 0)) {

                            var index = $scope.modalUpload.cnabCfgArqIndex;
                            if ($scope.cnabConfig.CNAB_CONFIG_ARQUIVO.length > index) {

                                $scope.cnabConfig.CNAB_CONFIG_ARQUIVO[index].CNAB = response.data.result.lstCnabs;
                            }
                        }
                    });

                    angular.element("#modal-upload-planilha").modal('hide');
                    hideAjaxLoader();

                }, function (response) {

                    hideAjaxLoader();
                    if (response.status > 0) {
                        $scope.errorMsg = response.status + ': ' + response.data;
                    }
                }, function (evt) {
                    $scope.filename = evt.config.data.file.name;

                    file.progress = Math.min(100, parseInt(100.0 * evt.loaded / evt.total));
                });
            }
        }
        else {
            return false;
        }
    };

    $scope.carregarEmpresas = function () {

        var url = Util.getUrl("/empresa/listarEmpresas");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstEmpresas',
            responseModelName: 'lstEmpresas',
            showAjaxLoader: true,

        });
    };

    $scope.carregarBancos = function () {

        var url = Util.getUrl("/cnab/listarBancos");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstBancos',
            responseModelName: 'lstBancos',
            showAjaxLoader: true,

        });
    };


    $scope.salvarCnabConfig = function () {

        $scope.sincronizarInformacoesNosDetalhes();

        formHandlerService.submit($scope, {
            url: Util.getUrl("/cnab/SalvarCnabConfig"),
            objectName: 'cnabConfig',
            showAjaxLoader: true,
            success: function (resp, status, config, message, validationMessage) {

                $scope.message = message;
                $scope.erros = validationMessage;

                $scope.button = 'reset';
                $scope.button1 = 'reset';
                if (resp.success) {

                    $timeout(function () {

                        $scope.message = null;

                        if (confirm("Voltar para a listagem?")) {
                            window.open(Util.getUrl('/cnab/index'), '_self');
                        }
                        
                    }, 1000);

                }
            }
        });
    }

    $scope.adicionarLinhaArquivo = function () {

        if ($scope.cnabConfig) {

            if (!$scope.cnabConfig.CNAB_CONFIG_ARQUIVO)
                $scope.cnabConfig.CNAB_CONFIG_ARQUIVO = [];
            $scope.cnabConfig.CNAB_CONFIG_ARQUIVO.push({ DATA_CADASTRO: new Date() });
        }
    }

    $scope.deletarLinhaArquivo = function ($index) {

        if(($index || $index == 0) && 
            $scope.cnabConfig && $scope.cnabConfig.CNAB_CONFIG_ARQUIVO) {

            $scope.cnabConfig && $scope.cnabConfig.CNAB_CONFIG_ARQUIVO.splice($index, 1);
        }
    }

    $scope.adicionarLinhaDetalhamento = function (cnabCfgArq) {

        if (cnabCfgArq) {

            if (!cnabCfgArq.CNAB)
                cnabCfgArq.CNAB = [];

            cnabCfgArq.CNAB.push({ DATA_CADASTRO: new Date() });
        }
    }

    $scope.deletarLinhaDetalhamento = function ($index, cnabCfgArq) {

        if (($index || $index == 0) &&
            cnabCfgArq && cnabCfgArq.CNAB) {

            cnabCfgArq.CNAB.splice($index, 1);
        }
    }

    $scope.deletarTodosCnabs = function (cnabCfgArq) {

        if (confirm("Essa ação irá apagar todas as linhas dessa definição. Prosseguir?")) {

            if (cnabCfgArq) {

                cnabCfgArq.CNAB = [];
            }
        }
    }


    $scope.sincronizarInformacoesNosDetalhes = function () {

        if ($scope.cnabConfig && $scope.cnabConfig.CNAB_CONFIG_ARQUIVO) {

            angular.forEach($scope.cnabConfig.CNAB_CONFIG_ARQUIVO, function (cnabConfigArq) {

                if (cnabConfigArq && cnabConfigArq.CNAB) {

                    angular.forEach(cnabConfigArq.CNAB, function (cnab) {

                        cnab.EMP_ID = $scope.cnabConfig.EMP_ID;
                        cnab.BAN_ID = $scope.cnabConfig.BAN_ID;
                        cnab.CNB_CNAB = $scope.cnabConfig.CNC_CODIGO_CNAB;
                        cnab.CNB_ARQUIVO = $scope.cnabConfig.CNC_ARQUIVO;
                        cnab.CNB_REGISTRO = cnabConfigArq.CCA_TIPO;
                        cnab.DATA_CADASTRO = new Date();
                        cnab.EMP_GRP_COAD = 1
                    });
                }
            });
        }
    }

    $scope.carregarTipoRegistro = function () {

        var url = Util.getUrl("/cnab/listarTipoRegistro");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstTipoRegistro',
            responseModelName: 'lstTipoRegistro',
            showAjaxLoader: true,

        });
    }

    $scope.carregarTipoDados = function () {

        var url = Util.getUrl("/cnab/listarTipoDados");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstTipoDados',
            responseModelName: 'lstTipoDados',
            showAjaxLoader: true,

        });
    }

    $scope.downloadPlanilhaComSuspectsComErro = function (ccaId) {

        if (ccaId) {
            var url = Util.getUrl("/cnab/DownloadPlanilhaCnab");
            post(url + "?ccaId=" + ccaId, true);
        }
    }


});