///---------------------------------TransferirSuspectsController---------------------------------------------------
appModule.controller("TransferirClientesController", function ($scope, $http, formHandlerService, messageService) {
    
    $scope.transferenciaDto = {tipo : 'representante'};

    $scope.listarRepresentantes = function (pagina) {

        var url = Util.getUrl("/franquiado/representantes");

        if ($scope.filtro == null) {

            //$scope.filtro = { registroPorPagina: 7 };
        }

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

            }
        });
    }

    $scope.carregaRepresentantees = function (regiaoId) {

        var url = Util.getUrl('/representantes/list');

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'representantes',
            responseModelName: 'representantes',
            data : {regiaoId : regiaoId}
        });
    }
    $scope.carregaRegioes = function () {

        if (!$scope.regioes) {
            var url = Util.getUrl('/regiao/regioes');
            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'regioes',
                responseModelName: 'lstRegiao',
                success: function () {

                    if ($scope.regioes.length == 1) {

                        var regiaoId = $scope.regioes[0].ID;
                        $scope.regiaoSelecionada = regiaoId;
                        $scope.listarRepresentantes(regiaoId);
                        $scope.carregaRepresentantees(regiaoId);
                       
                        $scope.transferenciaDto.RG_ID = regiaoId;

                    }
                }
            });
        }
    }

    $scope.regiaoOrigemSelecionado = function () {

        var regiaoId = $scope.RegiaoOrigemId;
        $scope.carregaRepresentantesComCarteiramento(regiaoId)
    }

    $scope.regiaoDestinoSelecionado = function () {

        var regiaoId = $scope.RegiaoDestinoId;
        $scope.carregaRepresentantes(regiaoId);
    }

    $scope.aoSelecionaRepresentanteOrigem = function () {

        var REP_ID_ORIGEM = $scope.transferenciaDto.REP_ID_ORIGEM;

        angular.forEach($scope.representantes, function (value) {

            if (value.REP_ID == REP_ID_ORIGEM) {

                value.MOSTRAR = false;
            }
            else {

                value.MOSTRAR = true;
            }
        });

        $scope.qtdCarteiramentos = null;

        var url = Util.getUrl('/representantes/qtdcarteiramentos');

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'qtdCarteiramentos',
            responseModelName: 'qtdCarteiramentos',
            data: {SlpCode : slpCodeOrigem}
        });
    }

    $scope.init = function () {

        $scope.carregaRegioes();
        $scope.query = {MOSTRAR : true};
    }

    $scope.selectAba = function (aba) {

       if (aba) {

            $scope.transferenciaDto.tipo = aba;
        }
        else {
            $scope.transferenciaDto.tipo = "representante"
       }

       if ($scope.regioes.length == 1) {

           $scope.pegaRepresentantes();
       }

    }

    $scope.selectAbaRepresentante = function () {

        $scope.selectAba("representante");
    }
    
    $scope.selectAbaRodizioLogado = function () {

        $scope.transferenciaDto.RegiaoId = {};
        $scope.carregaRegioes();       
        $scope.semantica = "Logadas";
        $scope.selectAba("rodizioLogado");
    }

    $scope.selectAbaRodizioGeral = function () {

        $scope.carregaRegioes();        
        $scope.transferenciaDto.RegiaoId = {};
        $scope.selectAba("rodizioGeral");
        $scope.semantica = "da Região";
    }

    $scope.transferir = function () {

        var obj = $scope.transferenciaDto;
        if (obj.tipo) {

            if (!obj.SlpCodeOrigem) {

                $scope.message = messageService.fail("Selecione a representante de origem.");
                $scope.transState = "reset";
                return false;
            }

            if (obj.tipo == "representante") {

                if (!obj.SlpCodeDestino) {

                    $scope.message = messageService.fail("Selecione a representante de destino.");
                    return false;
                }               
            }

            if (obj.tipo == "rodizioGeral") {

                if (!obj.RegiaoId) {

                    $scope.message = messageService.fail("Selecione a região de destino.");
                    return false;
                }
            }

        }
        else {

            $scope.message = messageService.fail("Não é possível transferir por inconsistência de dados.");
            $scope.transState = "reset";
            return false;
        }


        showAjaxLoader();
        formHandlerService.submit($scope, {
            url: Util.getUrl("/suspect/transferirsuspects"),
            objectName: 'transferenciaDto',
            showAjaxLoader: true,
            success: function (resp, status, config, message) {

                angular.element("#modal-resumo").modal("hide");
                $scope.reset();
                $scope.message = message;
                var regiaoId = $scope.RegiaoOrigemId;

                if(regiaoId)
                    $scope.carregaRepresentanteesComCarteiramento(regiaoId);

                if(resp.result.resumo)
                    $scope.resumo = resp.result.resumo;
                var slpCodeOrigem = $scope.transferenciaDto.SlpCodeDestino = {};
                
            },
            fail: function (resp, data, message) {

                angular.element("#modal-resumo").modal("hide");
                $scope.reset();
                $scope.message = message;                
            }
        });
    }

    $scope.showResume = function () {       

        $scope.transState = "reset";
            var obj = $scope.transferenciaDto;
            if (obj.tipo) {

                if (!obj.SlpCodeOrigem) {

                    $scope.message = messageService.fail("Selecione a representante de origem.");
                    $scope.reset();
                    return false;
                }

                if (obj.tipo == "representante") {

                    if (!obj.SlpCodeDestino) {

                        $scope.message = messageService.fail("Selecione a representante de destino.");
                        return false;
                    }               
                }

            }
            else {

                $scope.message = messageService.fail("Não é possível transferir por inconsistência de dados.");
                $scope.reset();
                return false;
            }

        $scope.resumeModel = {};
        $scope.resumeModel.tipo = $scope.transferenciaDto.tipo;
        $scope.resumeModel.DestTipo = ($scope.transferenciaDto.tipo == "representante") ? "Representante para Representante." : ($scope.transferenciaDto.tipo == "rodizioLogado") ? "Rodizio de representantes logados." : "Rodizio de representantes da região";
        $scope.resumeModel.RepresentanteOrigem = $scope.achaRepresentante($scope.transferenciaDto.SlpCodeOrigem);
        $scope.resumeModel.RepresentanteDestino = $scope.achaRepresentante($scope.transferenciaDto.SlpCodeDestino);

        if ($scope.resumeModel.tipo && $scope.resumeModel.tipo != "representante") {

            $scope.resumeModel.RegiaoDestino = $scope.achaRegiao($scope.transferenciaDto.RegiaoId);
        }
        angular.element("#modal-resumo").modal();

    }


    $scope.reset = function () {

        $scope.transState = "reset";
        $scope.qtdCarteiramentos = null;
        $scope.transferenciaDto = { tipo: $scope.transferenciaDto.tipo};
        $scope.checado = false;
        $scope.representantes = {};
        $scope.representanteesComCarteiramento = {};

    };

    $scope.achaRepresentante = function (REP_ID) {

        var result = null;
        var lista = ($scope.representantes) ? $scope.representantes : $scope.representantesComCarteiramento;
        angular.forEach(lista, function (value) {


            if (value.REP_ID == REP_ID) {

                result = value;
                return;
            }
        });
        return result;
    }

    $scope.achaRegiao = function (regiaoId) {

        var result = null;
        angular.forEach($scope.regioes, function (value) {


            if (value.RG_ID == regiaoId) {

                result = value;
                return;
            }
        });
        return result;
    }
    $scope.pegaRepresentanteLogada = function (regiaoId) {

        var data = {
            regiaoId : regiaoId
        };

        var url = Util.getUrl('/representantes/representanteslogadas');

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'representantes',
            responseModelName: 'representantes',
            data : data
        });
    }


    $scope.pegaRepresentanteDaRegiao = function (regiaoId) {

        var data = {
            regiaoId: regiaoId
        };

        if (regiaoId) {
            var url = Util.getUrl('/representantes/RepresentantesDaRegiao');

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'representantes',
                responseModelName: 'representantes',
                data: data
            });
        }
        else {
            $scope.representantes = null;
        }
        
    }

    $scope.pegaRepresentantes = function () {

        $scope.resumo = null;

        var regiaoId = ($scope.regioes.length == 1) ? $scope.regiaoSelecionada : $scope.transferenciaDto.RegiaoId;

        if ($scope.regioes.length == 1)
            $scope.transferenciaDto.RegiaoId = regiaoId;

        if ($scope.transferenciaDto.tipo == "rodizioLogado") {

            $scope.pegaRepresentanteLogada(regiaoId);
        }

        if ($scope.transferenciaDto.tipo == "rodizioGeral") {

            $scope.pegaRepresentanteDaRegiao(regiaoId);
        }
    }
    
    $scope.$watch("qtdCarteiramentos", function (value) {

        if (value >= 1500) {

            $scope.message = Util.createMessage("warning", "Esse representante possui mais 1500 ou mais encarteiramentos ativos. Esse processo pode demorar mais que o normal.");
        }
        else {
            $scope.message = {};
        }
    });

});
