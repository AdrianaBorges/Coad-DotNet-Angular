//appModule.config(['$httpProvider', function($httpProvider){ // configurando o http

//    $httpProvider.useApplyAsync(true);
//}]);

appModule.controller('ImportacaoSuspectController', function ($scope, formHandlerService, $http, conversionService, $interval, cepService, $timeout, Upload, FilterService) {
        
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
        $scope.carregarImportacaoStatus();
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

    $scope.pesquisarImportacoes = function (pagina) {

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

    $scope.pesquisarHistorico = function (pagina, impID, ipsID) {

        if (!$scope.filtroHistorico) {

            $scope.filtroHistorico = {};
        }

        if(impID != null)
            $scope.filtroHistorico.impID = impID;

        if(ipsID != null)
            $scope.filtroHistorico.ipsID = ipsID;
        var url = Util.getUrl("/importacaoSuspect/pesquisarHistorico");

        if (pagina) {

            url += "?pagina=" + pagina;
        }

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstHistoricos',
            responseModelName: 'lstHistoricos',
            showAjaxLoader: true,
            data: $scope.filtroHistorico,
            pageConfig: { pageName: 'page' , pageTargetName: 'pageHistoricoImportacao' },
            success: function (resp) {

            }
        });
    };

    $scope.carregarImportacaoStatus = function () {

        var url = Util.getUrl("/importacaoSuspect/ListarImportacaoStatus");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstImportacaoStatus',
            responseModelName: 'lstImportacaoStatus',
            showAjaxLoader: true,
            success: function () {

                FilterService.adicionarDadoCombo($scope.filtrosImportacoesSuspect, $scope.lstImportacaoStatus, "ImsID");
            }

        });
    }

    $scope.abrirModalNomeArquivo = function (nomeArquivo) {

        $scope.NomeDoArquivo = nomeArquivo;
        angular.element("#nome-arquivo").modal();
    }

    $scope.abrirModalDescricao = function (descricao) {

        $scope.descricaoHistorico = descricao;
        angular.element("#descricao-importacao-importacao").modal();
    }


    $scope.abrirHistoricoImportacao = function (impID, ipsID) {
        $scope.filtroHistorico = {};
        $scope.pesquisarHistorico(1, impID, ipsID);
        angular.element("#descricao-importacao-importacao").modal('hide');
        angular.element("#modal-historico-importacao").modal();
    }

    $scope.pesquisarImportados = function(item){

        $scope.importacaoModal = item;
        $scope.tab = 1;
        angular.element("#panelImportacao").collapse('hide');
        angular.element("#panelImportacaoSuspect").collapse();

        $scope.pesquisarImportacoesSuspect();
    }

    $scope.pesquisarImportacoesSuspect = function (pagina) {

        if ($scope.importacaoModal) {

            if (!$scope.filtroImportacoesSuspect)
                $scope.filtroImportacoesSuspect = {};
            $scope.filtroImportacoesSuspect.ImpID = $scope.importacaoModal.IMP_ID;
            $scope.paginaAtual = pagina;
            //$scope.listado = true;
            var url = Util.getUrl("/importacaoSuspect/pesquisarImportacoesSuspect");

            if (pagina) {

                url += "?pagina=" + pagina;
            }

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'lstImportacoesSuspect',
                responseModelName: 'lstImportacoesSuspect',
                showAjaxLoader: true,
                data: $scope.filtroImportacoesSuspect,
                pageConfig: { pageName: 'page' , pageTargetName: 'pageImportacaoSuspect'},
                success: function (resp) {

                }
            });
        }
    };


    $scope.limparBR = function (text) {

        if (text) {
            text = text.replace('<br />', '');
            text = text.replace('<BR />', '');
        }
        return text;
    }

    $scope.abrirTabImportacoes = function () {

        $scope.tab = 0;
    }

    $scope.abrirModalResultadoDeRodizio = function (impID) {

        $scope.listarResultadosDeRodizio(impID);
        angular.element("#modal-resultado-rodizio").modal();
    }

    $scope.listarResultadosDeRodizio = function (impID) {

        var url = Util.getUrl("/importacaoSuspect/listarResultadosDeRodizio");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'retorno',
            responseModelName: 'retorno',
            showAjaxLoader: true,
            data: {impID : impID}
        });
    }

    $scope.downloadPlanilhaComSuspectsComErro = function (impID) {

        if (impID) {
            var url = Util.getUrl("/importacaoSuspect/downloadPlanilhaSuspectComErro");
            post(url + "?impID=" + impID, true);
        }
    }
    
    $scope.criarFiltros = function () {

        $scope.filtrosImportacoesSuspect =
            [{
                nomeGrupo: 'Padrão',
                idGrupo: "padrao",
                filtros: [
                            {
                                label: "Nome do Suspect",
                                chave: "Nome",
                                ordem: 0,
                                size: 96,
                                tipo: 'texto'
                            },
                            {
                                label: "CNPJ/CPF do cliente",
                                chave: "CPF_CNPJ",
                                ordem: 1,
                                maxLength: 14,
                                size: 170,
                                tipo: 'texto'
                            },
                            {
                                label: "Telefone",
                                chave: "Telefone",
                                ordem: 2,
                                tipo: 'texto'
                            },
                            {
                                label: "E-Mail",
                                chave: "Telefone",
                                ordem: 3,
                                tipo: 'texto'
                            },

                ]
            },
           {
               nomeGrupo: 'Detalhes',
               idGrupo: "cod",
               filtros: [
                        {
                            label: "UF",
                            chave: "UF",
                            ordem: 0,
                            tipo: 'texto'
                        },
                        {
                            label: "Bairro",
                            chave: "Bairro",
                            ordem: 1,
                            tipo: 'texto'
                        },
                        {
                            label: "Status",
                            chave: "ImsID",
                            ordem: 0,
                            size: 96,
                            tipo: 'select',
                            valueName: 'IMS_ID',
                            labelName: 'IMS_DESCRICAO',
                            renderIf: $scope.ehGerente
                        },
                        //{
                        //    label: "Região",
                        //    chave: "Bairro",
                        //    ordem: 1,
                        //    tipo: 'texto'
                        //},
               ]
           }
       ];
    }


    $scope.abrirModalUploadPlanilhaErro = function (impID) {

        $scope.modalUpload = { impID: impID };
        $scope.batchResp = null;
        
        angular.element("#modal-upload-planilha").modal();
    }

    $scope.uploadPlanilha = function (file, errFiles) {

            $scope.f = file;
            $scope.errFiles = errFiles && errFiles[0];

            if (file) {

                file.upload = Upload.upload({
                    url: Util.getUrl("/importacaoSuspect/receberUploadPlanilhaComErroImportacao"),
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

    $scope.atualizarSuspectsIncorretos = function () {

        if (confirm("Deseja atualizar os dados?")) {
            
            formHandlerService.submit($scope, {
                url: Util.getUrl("/importacaoSuspect/atualizarSuspectsIncorretos"),
                objectName: 'modalUpload',
                showAjaxLoader: true,
                success: function (resp, status, config, message, validationMessage) {

                    $scope.pesquisarImportacoes();
                    $scope.buttonUpload = 'reset';
                    $scope.message = message;
                    $scope.erros = validationMessage;
                    $scope.batchResp = resp.result.context;

                    if (resp.success) {

                        if ($scope.batchResp.TotalFalha <= 0) {

                            $scope.message = Util.createMessage("success", "Atualização realizada com sucesso!!");
                            $timeout(function () {
                                $scope.message = null;
                                angular.element("#modal-upload-planilha").modal('hide');
                            }, 1000);

                        }
                        else {
                            if ($scope.batchResp.TotalExito > 0)
                            {
                                $scope.message = Util.createMessage("warning", "Algumas linhas não foram atualizadas!!");
                            }
                            else {
                                $scope.message = Util.createMessage("fail", "Nenhuma linha foi atualizada!!");
                            }

                            $timeout(function () {
                                $scope.message = null;
                                
                            }, 1500)
                        }

                    }

                }
            });
        }
        else {
            return false;
        }
    }

    $scope.reexecutarImportacao = function () {

        if (confirm("Executar agora?")) {

            formHandlerService.submit($scope, {
                url: Util.getUrl("/importacaoSuspect/reexecutarImportacao"),
                objectName: 'modalImportacao',
                showAjaxLoader: true,
                success: function (resp, status, config, message, validationMessage) {

                    $scope.buttonReexecucao = 'reset';
                    $scope.modalImportacao.aguardandoExecucao = true;
                    $scope.message = message;
                    $timeout(function () {
                        $scope.message = null;

                    }, 10000);
                },
                fail: function () {

                }
            });            
        }
    }

    $scope.retonarProgressoDoJob = function () {

            $scope.progressPromisse = $interval(function () {
            var url = Util.getUrl("/importacaosuspect/RetonarProgressoDoJob");

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'batchProgress',
                responseModelName: 'batchProgress',
                //showAjaxLoader: true,
                data: $scope.modalImportacao,
                success: function (resp) {

                    if ($scope.batchProgress) {

                        if (!$scope.batchProgress.Executando && $scope.executando) {

                            if ($scope.batchProgress.CodStatus == 4) {
                                $scope.message = Util.createMessage('success', 'A importação foi finalizada com sucesso.');
                            }
                            else
                            if ($scope.batchProgress.CodStatus == 3) {
                                $scope.message = Util.createMessage('fail', 'A importação terminou com alguns erros.');
                            }
                            else {
                                $scope.message = Util.createMessage('info', 'A importação terminou porém não retornou nenhum status.');
                            }
                            $interval.cancel($scope.progressPromisse);
                            $timeout(function () {  
                                $scope.message = null;
                                window.location = Util.getUrl("/franquia/importacaosuspect/index");
                            }, 2000);
                        }
                        else 
                        if ($scope.batchProgress.Executando && $scope.modalImportacao.aguardandoExecucao === true) {

                            $scope.modalImportacao.aguardandoExecucao = false;
                            $scope.executando = $scope.batchProgress.Executando;
                        }
                    }
                }
            });


            }, 1000);
    };

    $scope.retonarDadosDaImportacao = function () {

        if ($scope.modalImportacao && $scope.modalImportacao.impID) {

            var url = Util.getUrl("/importacaosuspect/RetonarDadosDaImportacao");

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'importacao',
                responseModelName: 'importacao',
                //showAjaxLoader: true,
                data: $scope.modalImportacao,
                //pageConfig: { pageName: 'page' /*, pageTargetName: 'pageAgendamentoDoDia' */ },
                success: function (resp) {

                    
                }
            });
        };
    }

    $scope.cancelarImportacao = function (impID) {

        if (confirm("Cancelar Importação?")) {

            $scope.cancelamentoModal = { impID: impID };
            formHandlerService.submit($scope, {
                url: Util.getUrl("/importacaoSuspect/cancelarImportacao"),
                objectName: 'cancelamentoModal',
                showAjaxLoader: true,
                success: function (resp, status, config, message, validationMessage) {
                    $scope.pesquisarImportacoes();
                    $scope.message = message;
                    $timeout(function () {
                        $scope.message = null;

                    }, 10000);
                },
                fail: function () {

                }
            });
        }
    }

    $scope.cancelarUploadAgendamento = function () {

        $scope.step = 1;
    }


    $scope.abrirClienteCurso = function (ipsID) {

        if (ipsID) {

            var url = Util.getUrl("/importacaosuspect/RetornarCodigoDoCliente");

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'cliID',
                responseModelName: 'cliID',
                showAjaxLoader: true,
                data: { ipsID: ipsID },
                //pageConfig: { pageName: 'page' /*, pageTargetName: 'pageAgendamentoDoDia' */ },
                success: function (resp) {

                    if ($scope.cliID) {
                        var url = Util.getUrl("/franquia/clientes/Editar?clienteId=" + $scope.cliID);

                        post(url, true);
                    }

                }
            });
        };
    }



});