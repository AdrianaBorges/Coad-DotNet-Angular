appModule.controller('CarteiramentoController', function ($scope, formHandlerService, $http, conversionService, $timeout) {

    $scope.mostraFiltro = true;
    $scope.queryRepresentante = {EXCLUIR : false};

    
    $scope.initTransferencia = function (RG_ID, franquiador) {

        $scope.transferenciaDTO = { tipo: 'representante', RG_ID: RG_ID };
        $scope.regiaoId = RG_ID;

        $scope.franquiador = franquiador == "1" ? true : false;
        $scope.init($scope.franquiador);
    }

    $scope.init = function (franquiador) {
        
        $scope.filtro = { RG_ID_REPRESENTANTE: null };

        $scope.franquiador = franquiador == "1" ? true : false;
        $scope.listarRepresentantes();
        $scope.carregaRegioes();
    }
   
   $scope.salvarConfig = function(){
                   
        formHandlerService.submit($scope, {
            url: Util.getUrl("/carteiramento/salvarconfig"),
            objectName: 'carteira',
            showAjaxLoader: true,
            success: function (resp, status, config, message, validationMessage) {
                $scope.message = message;
                $scope.erros = validationMessage;

                if (resp.success) {
                    alert("Salvo com sucesso.");
                    window.location = Util.getUrl("/carteiramento/index");
                }
            }
        });
   }
   
   $scope.listar = function (pageRequest) {

       var url = Util.getUrl("/carteiramento/carteiramentos");

       if (pageRequest) {
           url += "?pagina=" + pageRequest;
       }

       var config = {
           url: url,
           targetObjectName: 'carteiramentos',
           responseModelName: 'carteiramentos',
           showAjaxLoader : true,
           pageConfig: { pageName: 'page' }
       };

       if ($scope.filtro) {

           config.data = angular.copy($scope.filtro);
       }
       formHandlerService.read($scope, config);
   };

   $scope.listarCarteirasFranquia = function (pageRequest) {

       var url = Util.getUrl("/carteiramento/carteiramentosFranquia");

       if (pageRequest) {
           url += "?pagina=" + pageRequest;
       }

       var config = {
           url: url,
           targetObjectName: 'carteiramentos',
           responseModelName: 'carteiramentos',
           showAjaxLoader: true,
           pageConfig: { pageName: 'page' }
       };

       if ($scope.filtro) {

           config.data = angular.copy($scope.filtro);
       }
       formHandlerService.read($scope, config);
   };


   $scope.read = function (carteiraId) {

       if (carteiraId) {

           formHandlerService.read($scope, {
               url: Util.getUrl("/carteiramento/readcarteiramento"),
               targetObjectName: 'carteira',
               responseModelName: 'carteira',
               showAjaxLoader: true,
               data: { carteiraId: carteiraId },
               success: function () {
               }
           });
       };     

   }

   $scope.getLstRepresentante = function (pageRequestRepresentante) {

       var url = Util.getUrl("/representante/representantes");

       if (pageRequestRepresentante) {
           url += "?pagina=" + pageRequestRepresentante;
       }
       $scope.mostraFiltro = false;

       var config = {
           url: url,
           targetObjectName: 'representantes',
           responseModelName: 'representantes',
           pageConfig: { pageName: 'page', pageTargetName: 'paginaRep' },
           success: function () {

               angular.element("#modal-representantes").modal();
           }
       };

       if ($scope.filtroRepresentante) {

           config.data = angular.copy($scope.filtroRepresentante);
       }
       formHandlerService.read($scope, config);
   }


   $scope.IncluirRepresentante = function (rep) {

       if ($scope.carteira && $scope.carteira.CARTEIRA_REPRESENTANTE) {

           var carteira = angular.copy($scope.carteira);
           carteira.CARTEIRA_REPRESENTANTE = null;
           $scope.carteira.CARTEIRA_REPRESENTANTE.push({ REPRESENTANTE: rep, CARTEIRA : carteira, EXCLUIR : false, NOVO : true});          
       }

       angular.element("#modal-representantes").modal('hide');
   }
   $scope.AbrirModalRepresentante = function (cartId) {

       $scope.acaoTroca = {};
       $scope.acaoTroca.carteiraId = cartId;
       angular.element("#modal-representantes").modal();
   }

   $scope.ExcluirRepresentante = function (index) {

       if ($scope.carteira && $scope.carteira.CARTEIRA_REPRESENTANTE) {

           if (!$scope.carteira.CARTEIRA_REPRESENTANTE.NOVO) {
                $scope.carteira.CARTEIRA_REPRESENTANTE[index].EXCLUIR = true;
           }
           else {
               $scope.carteira.CARTEIRA_REPRESENTANTE.splice(index, 1);
           }
           
       }
   }
   $scope.getLstUfsRepresentante = function (produtoId) {

       formHandlerService.read($scope, {
               url: Util.getUrl("/representante/getufrepresentantes"),
               targetObjectName: 'ufs',
               responseModelName: 'ufs'
       });
   };

   $scope.listarRepresentantes = function (pagina) {

       var url = Util.getUrl("/franquia/carteiramento/representantes");

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

   $scope.selecionarRepresentante = function (representante) {

       $scope.representante = representante;

       $scope.filtro.CAR_ID = null;
       if(representante){

           $scope.REP_ID = representante.REP_ID;
           $scope.filtro.REP_ID = representante.REP_ID;
       }

       $scope.listarQuantidadeTipoCliente();
       $scope.listarCarteirasDoRepresentante();
        
   }

   $scope.listarClientesRepresentante = function (pageRequest) {

       $scope.message = null;

       if ($scope.REP_ID) {

           
           var url = Util.getUrl("/franquia/carteiramento/ClientesPorRepresentante");

           if (pageRequest) {
               url += "?pagina=" + pageRequest;
           }

           var config = {
               url: url,
               targetObjectName: 'clientes',
               responseModelName: 'clientes',
               pageConfig: { pageName: 'page' , pageTargetName: 'paginaClientes' },
               showAjaxLoader: true,
           };

           if ($scope.filtro) {

               if ($scope.carteira) {

                   $scope.filtro.CAR_ID = angular.copy($scope.carteira.CAR_ID);
               }

               $scope.filtro.REP_ID = angular.copy($scope.REP_ID);
               config.data = angular.copy($scope.filtro);
           }
           else {

               config.data = {REP_ID : angular.copy($scope.REP_ID)};
           }

           formHandlerService.read($scope, config);
       }
       
   };

   $scope.listarQuantidadeTipoCliente = function () {

       var url = Util.getUrl("/franquia/carteiramento/QuantidadeClientesPorTipo");

       formHandlerService.read($scope, {
           url: url,
           targetObjectName: 'resumoQuantidadeTipoCliente',
           responseModelName: 'resumoQuantidadeTipoCliente',
           showAjaxLoader: true,
           data: $scope.filtro,
           success: function (resp) {

           }
       });
   }


   $scope.listarCarteirasDoRepresentante = function () {

       var url = Util.getUrl("/franquia/carteiramento/CarteiramentosDaRepresentanteParaTransferencia");

       formHandlerService.read($scope, {
           url: url,
           targetObjectName: 'carteiramentos',
           responseModelName: 'carteiramentos',
           showAjaxLoader: true,
           data: $scope.filtro,
           success: function (resp) {

           }
       });
   }

   $scope.filtrarUsuariosPorCarteira = function (carteira){

       $scope.carteira = carteira;       
       $scope.listarClientesRepresentante();       

   }

   $scope.listarCarteirasParaEscolha = function (pageRequest) {

       var url = Util.getUrl("/carteiramento/ListarCarteiramentoParaEscolha");


       if (pageRequest) {
           url += "?pagina=" + pageRequest;
       }

       formHandlerService.read($scope, {
           url: url,
           targetObjectName: 'carteiramentosEscolha',
           responseModelName: 'carteiramentos',
           showAjaxLoader: true,
           pageConfig: { pageName: 'page', pageTargetName: 'paginaCarteirasEscolha' },
           data: $scope.filtro,
           success: function (resp) {

           }
       });
   }


   $scope.AbreModalTrocarCarteiramento = function (CAR_ID) {

       $scope.carteiraRequest = {
           CAR_ID_ANTIGO: CAR_ID,
           REP_ID: $scope.filtro.REP_ID,
           tipoAcao: 'trocar',
       }

       $scope.listarCarteirasParaEscolha();
       angular.element("#modalCarteiras").modal();
   }


   $scope.AbreModalIncluirCarteiramento = function () {

       $scope.carteiraRequest = {
           REP_ID: $scope.filtro.REP_ID,
           tipoAcao : 'incluir',
       };

       $scope.listarCarteirasParaEscolha();
       angular.element("#modalCarteiras").modal();
   }

   $scope.TrocarCarteiramento = function (CAR_ID_NOVO) {

       $scope.carteiraRequest.CAR_ID_NOVO = CAR_ID_NOVO;

       formHandlerService.submit($scope, {
           url: Util.getUrl("/carteiramento/TrocarCarteiramento"),
           objectName: 'carteiraRequest',
           showAjaxLoader: true,
           success: function (resp, status, config, message, validationMessage) {
                             
               $scope.message = message;
               $scope.erros = validationMessage;

               if (resp.success) {
                   $scope.message = Util.createMessage("success", "Trocado com sucesso!");
                   $scope.listarCarteirasDoRepresentante();
                   angular.element("#modalCarteiras").modal('hide');
               }
             
           }
       });
   }


   $scope.AdicionarCarteiramento = function (CAR_ID_NOVO) {

       $scope.carteiraRequest.CAR_ID_NOVO = CAR_ID_NOVO;

       formHandlerService.submit($scope, {
           url: Util.getUrl("/carteiramento/AdicionarCarteiramento"),
           objectName: 'carteiraRequest',
           showAjaxLoader: true,
           success: function (resp, status, config, message, validationMessage) {               
               $scope.message = message;
               $scope.erros = validationMessage;

               if (resp.success) {
                   $scope.message = Util.createMessage("success", "Adicionado com sucesso!");
                   $scope.listarCarteirasDoRepresentante();
                   angular.element("#modalCarteiras").modal('hide');
               }

           }
       });
   }

   $scope.RemoverCarteiramento = function (CAR_ID, $index) {

       if (confirm("Deseja realmente desassociar a carteira ao representante?")) {

           $scope.carteiraExRequest = {
               CAR_ID: CAR_ID,
               REP_ID: $scope.REP_ID
           };

           formHandlerService.submit($scope, {
               url: Util.getUrl("/carteiramento/RemoverCarteiramento"),
               objectName: 'carteiraExRequest',
               showAjaxLoader: true,
               success: function (resp, status, config, message, validationMessage) {

                   $scope.message = message;
                   $scope.erros = validationMessage;

                   if (resp.success) {
                       $scope.message = Util.createMessage("success", "Excluido com sucesso!");                     
                       $scope.listarCarteirasDoRepresentante();
                       angular.element("#modalCarteiras").modal('hide');
                   }                
               }
           });
       }
   }

   $scope.IncluirOuTrocarCarteira = function (CAR_ID_NOVO) {

       switch ($scope.carteiraRequest.tipoAcao) {

           case 'trocar': {

               $scope.TrocarCarteiramento(CAR_ID_NOVO);
               break;
           }
           case 'incluir' :{

               $scope.AdicionarCarteiramento(CAR_ID_NOVO);
               break;
           }

       }
   }

   $scope.selecionarCarteiraTransferencia = function (carteiramento) {

       if(carteiramento != null){

           if (Util.isPathValid($scope, "transferenciaDTO.CAR_ID_ORIGEM") && $scope.transferenciaDTO.tipo == 'representante') {

               if ($scope.transferenciaDTO.CAR_ID_ORIGEM != carteiramento.CAR_ID) {
                   $scope.transferenciaDTO.CAR_ID_DESTINO = angular.copy(carteiramento.CAR_ID);
               }
           }
           else {

               var CAR_ID = angular.copy(carteiramento.CAR_ID);
               $scope.transferenciaDTO.CAR_ID_ORIGEM = CAR_ID; 
               $scope.GetQtdClienteCarteira(CAR_ID);
           }
       }
   }

   $scope.deselecionarCarteiraTransferenciaOrigem = function (carteiramento) {

       $scope.qtdClientes = null;
       $scope.transferenciaDTO.CAR_ID_ORIGEM = null;
       $scope.transferenciaDTO.CAR_ID_DESTINO = null;
   }

   $scope.deselecionarCarteiraTransferenciaDestino = function () {

       $scope.transferenciaDTO.CAR_ID_DESTINO = null;
   }

   $scope.selectAba = function (aba) {

       if (aba) {

           $scope.transferenciaDTO.tipo = aba;
       }
       else {
           $scope.transferenciaDTO.tipo = "representante"
       }
       
   }

   $scope.selectAbaRepresentante = function () {

       $scope.selectAba("representante");
   }

   $scope.selectAbaRodizioLogado = function () {

       $scope.transferenciaDTO.RegiaoId = {};
       $scope.semantica = "Logadas";
       $scope.selectAba("rodizioLogado");

       // se for um franquiador não carrega automático e sim após selecionar
       // a região na tela
       if ($scope.franquiador == false) {
           $scope.listarRepresentantesLogados();
       }       
   }

   $scope.selectAbaRodizioGeral = function () {

       $scope.carregaRegioes();
       $scope.transferenciaDTO.RegiaoId = {};
       $scope.selectAba("rodizioGeral");
       $scope.semantica = "da Região";
   }

   $scope.carregaRegioes = function () {

       if (!$scope.regioes) {
           var url = Util.getUrl('/regiao/regioes');
           formHandlerService.read($scope, {
               url: url,
               targetObjectName: 'regioes',
               responseModelName: 'lstRegiao',
               success: function () {

               }
           });
       }
   }

   $scope.transferirCarteira = function () {

       formHandlerService.submit($scope, {
           url: Util.getUrl("/carteiramento/TransferirSuspects"),
           objectName: 'transferenciaDTO',
           showAjaxLoader: true,
           success: function (resp, status, config, message, validationMessage) {

               $scope.message = message;
               $scope.erros = validationMessage;
               
               if (resp.success) {

                   $scope.message = Util.createMessage("success", "Transferido com sucesso!");


                   $timeout(function () {
                       angular.element("#modal-resumo").modal("hide");
                       $scope.reset();
                       if (resp.result.resumo) {
                           $scope.resumo = resp.result.resumo;
                           angular.element("#resultado").modal();
                       }
                   }, 2500);

               } else {

                   $scope.buttonTransfer = "reset";
               }
           },
           fail: function (resp, data, message) {

               angular.element("#modal-resumo").modal("hide");
               $scope.reset();
               $scope.message = message;
           }
       });
   }

   $scope.listarRepresentantesLogados = function () {

       var url = Util.getUrl("/franquia/carteiramento/representantesLogadosTransferencia");

       formHandlerService.read($scope, {
           url: url,
           targetObjectName: 'representantesLogados',
           responseModelName: 'representantes',
           data: {RG_ID : $scope.transferenciaDTO.RG_ID},
           showAjaxLoader: true,
           //pageConfig: { pageName: 'page' /*, pageTargetName: 'paginaPrioridades' */ },
           success: function (resp) {

           }
       });
   }


   $scope.$watch("qtdCarteiramentos", function (value) {

       if (value >= 1500) {

           $scope.message = Util.createMessage("warning", "Esse representante possui mais 1500 ou mais encarteiramentos ativos. Esse processo pode demorar mais que o normal.");
       }
       else {
           $scope.message = null;
       }
   });


   $scope.showResume = function () {

       $scope.transState = "reset";
       var obj = $scope.transferenciaDTO;
       if (obj.tipo) {

           if (!obj.CAR_ID_ORIGEM) {

               $scope.message = Util.createMessage("fail", "Selecione a representante de origem.");
               $scope.reset();
               return false;
           }

           if (obj.tipo == "representante") {

               if (!obj.CAR_ID_DESTINO) {

                   $scope.message = Util.createMessage("fail", "Selecione a representante de destino.");
                   return false;
               }
           }

           if (obj.tipo == "rodizioLogado") {

               if ($scope.representantesLogados.length < 1) {

                   $scope.message = Util.createMessage("fail", "Não existe operadores logados para transferir.");
                   return false;
               }
           }

           if (obj.tipo == "rodizioLogado" || obj.tipo == "rodizioGeral") {

               if (!$scope.transferenciaDTO.RG_ID && $scope.franquiador) {

                   $scope.message = Util.createMessage("fail", "Antes de transferir selecione a região de destino.");
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
       $scope.resumeModel.tipo = $scope.transferenciaDTO.tipo;
       $scope.resumeModel.DestTipo = ($scope.transferenciaDTO.tipo == "representante") ? "Representante para Representante." : ($scope.transferenciaDTO.tipo == "rodizioLogado") ? "Rodizio de representantes logados." : "Rodizio de representantes da região";
       $scope.resumeModel.carteiraOrigem = $scope.transferenciaDTO.CAR_ID_ORIGEM;
       $scope.resumeModel.carteiraDestino = $scope.transferenciaDTO.CAR_ID_DESTINO;


       if ($scope.resumeModel.tipo && $scope.resumeModel.tipo != "representante") {

           $scope.resumeModel.RegiaoDestino = $scope.achaRegiao($scope.transferenciaDTO.RG_ID);
       }
       angular.element("#modal-resumo").modal();

   }


   $scope.reset = function () {

       $scope.buttonTransfer = "reset";
       $scope.qtdCarteiramentos = null;
       $scope.transferenciaDTO = { tipo: $scope.transferenciaDTO.tipo, RG_ID : $scope.transferenciaDTO.RG_ID };
       $scope.checado = false;

   };

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
  
   $scope.carregaRegioes = function () {

       if (!$scope.regioes) {
           var url = Util.getUrl('/regiao/regioes');
           formHandlerService.read($scope, {
               url: url,
               targetObjectName: 'regioes',
               responseModelName: 'lstRegiao',
               success: function () {

               }
           });
       }
   }

   $scope.abreModalGerarCarteira = function () {

       $scope.gerarCarteiraDTO = {};
       $scope.message = null;
       angular.element("#modal-gerar-regiao").modal();
   }


   $scope.gerarCarteira = function () {

           formHandlerService.submit($scope, {
               url: Util.getUrl("/carteiramento/gerarCarteira"),
               objectName: 'gerarCarteiraDTO',
               showAjaxLoader: true,
               success: function (resp, status, config, message, validationMessage) {

                   $scope.message = message;
                   $scope.erros = validationMessage;
                   
                   if (resp.success) {

                       $scope.message = angular.copy(message);
                       $timeout(function () {
                           angular.element("#modal-gerar-regiao").modal("hide");
                           $scope.buttonGerar = "reset";
                           $scope.message = null;
                           
                       }, 2900);

                    
                   } 
               },
               fail: function (resp, data, message) {

                   angular.element("#modal-resumo").modal("hide");
                   $scope.reset();
                   $scope.message = message;
               }
           });
   }

   $scope.excluirCarteira = function (item) {

       if (confirm("Confirmar a exclusão da carteira")) {
           
           $scope.excluirCarteiraDTO = angular.copy(item);

           formHandlerService.submit($scope, {
               url: Util.getUrl("/carteiramento/ExcluirCarteira"),
               objectName: 'excluirCarteiraDTO',
               showAjaxLoader: true,
               success: function (resp, status, config, message, validationMessage) {

                   $scope.message = message;
                   $scope.erros = validationMessage;
                   $scope.excluirCarteiraDTO = null;
                   $scope.listarCarteirasFranquia();

               },
               fail: function (resp, data, message) {

                   $scope.message = null;
               }
           });
       }
   }

   $scope.GetQtdClienteCarteira = function (CAR_ID) {

       var url = Util.getUrl("/franquia/carteiramento/QtdClienteCarteiramento");
       var data = { CAR_ID: CAR_ID };

       var config = {
           url: url,
           targetObjectName: 'qtdClientes',
           responseModelName: 'qtdClientes',
           showAjaxLoader: true,
           pageConfig: { pageName: 'page' },
           data : data
       };

       formHandlerService.read($scope, config);
   };

   $scope.regiaoRepresentanteSelecionado = function () {

       $scope.listarRepresentantes();
   };
});