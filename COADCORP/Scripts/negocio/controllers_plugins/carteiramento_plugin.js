

function CarteiramentoPluginController($scope, formHandlerService, $http, conversionService, $timeout) {

  $scope.initReencarteirar = function (franquiador) {

      franquiador = (franquiador == "true") ? true : false;

      $scope.podeEncarteirar = true;
      
      $scope.modalCarteiramento = {franquiador : (franquiador)};
      $scope.tab = 1;
  }

  $scope.initCarteiramento = function () {

      $scope.initReencarteirar(franquiador);

  }

  $scope.abreModalCarteiramento = function (CLI_ID) {

      $scope.modalCarteiramento.CLI_ID = CLI_ID;
      $scope.carregaRegioesDoCliente();
      $scope.modalCarteiramento.RG_ID = null;
      if ($scope.modalCarteiramento.franquiador != true) {

          $scope.listarRepresentantesParaReencarteiramentos();
      }
      
      angular.element("#modal-reencarteiramento").modal();
  }

  $scope.listarRepresentantesParaReencarteiramentos = function (pagina) {

      if (!$scope.modalCarteiramento.CLI_ID) {

          $scope.message = Util.createMessage("fail", "Não é possível listar os representantes pois nenhum cliente foi selecionado. Por favor selecione um cliente.");
          $timeout(function () {

              $scope.message = null;
              anguler.element("#modal-reencarteiramento").modal('hide');
          }, 1000);
      }
      else {
          var url = Util.getUrl("/franquia/representante/RepresentantesQueNaoSaoDoCliente");

          if (pagina) {

              url += "?pagina=" + pagina;
          }

          formHandlerService.read($scope, {
              url: url,
              targetObjectName: 'lstRepresentantesReencarteiramento',
              responseModelName: 'representantes',
              data: $scope.modalCarteiramento,
              showAjaxLoader: true,
              pageConfig: { pageName: 'page', pageTargetName: 'paginaRepresentanteReencarteiramento' },
              success: function (resp) {
              }
          });
      }
  }

  $scope.reencarteirar = function (REP_ID) {

      if (confirm("Confirmar reencarteiramento?")) {

          $scope.podeEncarteirar = false;
          if ($scope.modalCarteiramento) {

              var url = Util.getUrl("/franquia/carteiramento/reencarteirarParaRepresentante");
              $scope.modalCarteiramento.REP_ID = REP_ID;

                  formHandlerService.submit($scope, {

                      url: url,
                      objectName: 'modalCarteiramento',
                      showAjaxLoader: true,
                      success: function (resp, status, config, message, validationMessage) {


                          $scope.podeEncarteirar = true;
                          $scope.message = message;
                          $scope.erros = validationMessage;

                          $scope.buttonRodizo = 'reset';
                          if (resp.success) {

                              $scope.podeEncarteirar = true;
                              $timeout(function () {

                                  angular.element("#modal-reencarteiramento").modal('hide');
                                  $scope.message = null;

                              }, 1000);

                          }

                      }
                  });

          }
          else {

              $scope.message = Util.createMessage("fail", "Não é possível reencarteirar por falta de informação. Recarregue a tela e tente novamente.");
              return false;
          }
          
      }
      else {
          return false;
      }
  }

  $scope.selecionarTabReencarteirar = function (index) {

      $scope.tab = index;
  }

  $scope.reencarteirarPorRodizio = function () {

      if (confirm("Confirmar reencarteiramento?")) {

          if ($scope.modalCarteiramento) {

              var url = Util.getUrl("/franquia/carteiramento/reencarteirarPorRodizio");

              formHandlerService.submit($scope, {

                  url: url,
                  objectName: 'modalCarteiramento',
                  showAjaxLoader: true,
                  success: function (resp, status, config, message, validationMessage) {

                      $scope.podeEncarteirar = true;
                      $scope.message = message;
                      $scope.erros = validationMessage;
                      
                      $scope.buttonRodizo = 'reset';
                      if (resp.success) {
                          $scope.podeEncarteirar = true;

                          $timeout(function () {

                              angular.element("#modal-reencarteiramento").modal('hide');
                              $scope.message = null;

                          }, 1000);

                      }

                  }
              });

          }
          else {

              $scope.message = Util.createMessage("fail", "Não é possível reencarteirar por falta de informação. Recarregue a tela e tente novamente.");
              return false;
          }

      }
      else {
          return false;
      }
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

  $scope.carregaRegioesDoCliente = function () {

      var data = { CLI_ID: $scope.modalCarteiramento.CLI_ID };

      var url = Util.getUrl('/regiao/regioesDoCliente');
          formHandlerService.read($scope, {
              url: url,
              targetObjectName: 'lstRegioesDoCliente',
              responseModelName: 'lstRegioesDoCliente',
              data: data,
              success: function () {

              }
          });
      
  }

  $scope.abreModalAdicionarRegiao = function (CLI_ID) {

      $scope.modalCarteiramento.CLI_ID = CLI_ID;
      $scope.carregaRegioes();
      angular.element("#modal-adicionar-regiao").modal();
  }

  $scope.abreModalRemoverRegiao = function (CLI_ID) {

      $scope.modalCarteiramento.CLI_ID = CLI_ID;
      $scope.carregaRegioesDoCliente();
      $scope.modalCarteiramento.RG_ID = null;
      angular.element("#modal-remover-regiao").modal();
  }

  $scope.adicionarRegiao = function () {

      if (confirm("Confirmar?")) {

          if ($scope.modalCarteiramento) {

              var url = Util.getUrl("/franquia/carteiramento/adicionarRegiao");

              formHandlerService.submit($scope, {

                  url: url,
                  objectName: 'modalCarteiramento',
                  showAjaxLoader: true,
                  success: function (resp, status, config, message, validationMessage) {

                      $scope.message = message;
                      $scope.erros = validationMessage;
                      $scope.buttonAddRegiao = 'reset';
                      if (resp.success) {
                          $scope.podeEncarteirar = true;

                          $timeout(function () {

                              angular.element("#modal-adicionar-regiao").modal('hide');
                              $scope.message = null;

                          }, 1000);

                      }

                  },
                  fail: function () {
                      $scope.buttonAddRegiao = 'reset';
                  }
              });

          }
          else {

              $scope.message = Util.createMessage("fail", "Não é possível encarteirar por falta de informação. Recarregue a tela e tente novamente.");
              return false;
          }

      }
      else {
          return false;
      }
  }

  $scope.removerRegiao = function () {

      if (confirm("Deseja realmente remover essa região?")) {

          if ($scope.modalCarteiramento) {

              var url = Util.getUrl("/franquia/carteiramento/removerRegiao");

              formHandlerService.submit($scope, {

                  url: url,
                  objectName: 'modalCarteiramento',
                  showAjaxLoader: true,
                  success: function (resp, status, config, message, validationMessage) {

                      $scope.message = message;
                      $scope.erros = validationMessage;
                      $scope.buttonRemoveRegiao = 'reset';
                      if (resp.success) {
                          //$scope.podeEncarteirar = true;

                          $timeout(function () {

                              angular.element("#modal-remover-regiao").modal('hide');
                              $scope.message = null;

                          }, 1000);

                      }

                  },
                  fail: function () {
                      $scope.buttonRemoveRegiao = 'reset';
                  }
              });

          }
          else {

              $scope.message = Util.createMessage("fail", "Não é possível remover por falta de informação. Recarregue a tela e tente novamente.");
              return false;
          }

      }
      else {
          return false;
      }
  }


};
