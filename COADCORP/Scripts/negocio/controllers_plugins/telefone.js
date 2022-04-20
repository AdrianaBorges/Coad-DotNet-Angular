

function TelefoneController($scope, formHandlerService, $http, conversionService) {


    //$scope.$watch("cliente", function (value, old) {

    //    if (value != old || value != undefined) {

    //        if (!value.ASSINATURA[0].ASSINATURA_TELEFONE)

    //            $scope.infoMarketing = { AREA_INFO_MARKETING: [], PRODUTO_COMPOSICAO_INFO_MARKETING: [], MTK_CLI_ID: value.CLI_ID };

    //        addAreaOnInit($scope);
    //        addProdutoInteresseOnInit($scope);
    //        value.INFO_MARKETING = $scope.infoMarketing;
    //    }
    //}
    //}

    //});

    $scope.telefoneInicializer = function(){

            return CLIENTES_TELEFONE = [];
    }

    $scope.initClienteTelefone = function (telefone, dddTelefone) {

        $scope.getLstClienteTelefone();

        if (telefone) {

            if (!$scope.CLIENTES_TELEFONE) {

                $scope.CLIENTES_TELEFONE = [];
            }

            $scope.CLIENTES_TELEFONE.push({

                ATE_TELEFONE: telefone,
                ATE_DDD: dddTelefone,
                TIPO_TEL_ID: 4
            });
        }
    }

    $scope.getLstClienteTelefone = function () {

        formHandlerService.read($scope, {
            url: Util.getUrl("/franquia/clientetelefone/listartipotelefone"),
            targetObjectName: 'lstTipoTelefone',
            responseModelName: 'lstTipoTelefone',
            success: function () {
            }
        });

    }

    $scope.TipoAcaoTel = { label: 'Incluir', valor: 0 };



    $scope.IncluirTelefone = function (tel) {

        if (tel && (tel.TIPO_TEL_ID == null || tel.ATE_TELEFONE == null)) {

            $scope.message = Util.createMessage("fail", "Preencha corretamente esta linha de telefone antes de adicionar mais uma");
            return;
        }
        if ($scope.CLIENTES_TELEFONE) {

            $scope.CLIENTES_TELEFONE.push({});
        }
        else {
            $scope.CLIENTES_TELEFONE = [{}];
        }
    }



   $scope.ExcluirTelefone = function (index) {

        if ($scope.CLIENTES_TELEFONE && (index | index == 0)) {

            $scope.CLIENTES_TELEFONE.splice(index, 1);

        }
        else {

            $scope.message = Util.createMessage('fail', 'Ocorreu algum erro ao tentar retirar o telefone da lista');
        }
   }

   $scope.tipoTelefoneSelecionado = function (tel) {
        
       if (tel && tel.tipoTelefone != null) {

           tel.TIPO_TEL_ID = tel.tipoTelefone.TIPO_TEL_ID;
       }
   }
        
};

appModule.controller("ForEachTelefoneController", function($scope){
    
    $scope.$watch("tel.TIPO_TEL_ID", function (value, old) {
           
            if (value && $scope.tel != null) {

                if ($scope.tel.tipoTelefone) {

                    $scope.tel.tipoTelefone.TIPO_TEL_ID = value;
                }
                else {

                    var achou = false;

                    if (!$scope.$parent.lstTipoTelefone) {
                        var unbindWatch = $scope.$parent.$watch("lstTipoTelefone", function () {
                            // É necessário adicionar esse watch temporário
                            // porque esse método é executado na inicialização do página. Esse método depende de dois recursos (cliente, lista de tipos de telefone).
                            // Esse método será chamado quando o telefone do cliente é carregado. Mas nada garante que o a lista de tipos de telefone estará 
                            // disponível no momento. Se não tiver disponível postergo a execução até a lista ficar disponível com o watch e logo em seguida
                            // removo o watch


                            angular.forEach($scope.$parent.lstTipoTelefone, function (item, index) {

                                if (!achou && value == item.TIPO_TEL_ID) {

                                    $scope.tel.tipoTelefone = item;
                                    achou = true;
                                }
                            });

                            if (achou === true) {

                                unbindWatch();
                            }
                            
                        });

                    } else {
                        
                        angular.forEach($scope.$parent.lstTipoTelefone, function (item, index) {

                            if (!achou && value == item.TIPO_TEL_ID) {

                                $scope.tel.tipoTelefone = item;
                                achou = true;
                            }
                        });
                    }
                }
            }
        });

});