appModule.controller('ProdutoController', function ($scope, formHandlerService, $http, conversionService) {

   $scope.salvarProduto = function(){

            
        formHandlerService.submit($scope, {
            url: Util.getUrl("/produto/salvar"),
            objectName: 'produto',
            showAjaxLoader: true,
            success: function (resp, status, config, message, validationMessage) {
                $scope.message = message;
                $scope.erros = validationMessage;

                if (resp.success) {
                    alert("Salvo com sucesso.");
                    window.location = Util.getUrl("/produto/index");
                }
                else {
                    alert($scope.message);
                }
            }
        });
   }

   $scope.listar = function (pageRequest) {

       var url = Util.getUrl("/produto/produtos");

       if (pageRequest) {
           url += "?pagina=" + pageRequest;
       }

       var config = {
           url: url,
           targetObjectName: 'produtos',
           responseModelName: 'produtos',
           pageConfig: { pageName: 'page' }
       };

       if ($scope.filtro) {

           config.data = angular.copy($scope.filtro);
       }
       formHandlerService.read($scope, config);
   };

   $scope.read = function (produtoId) {

       if (produtoId) {

           formHandlerService.read($scope, {
               url: Util.getUrl("/produto/readproduto"),
               targetObjectName: 'produto',
               responseModelName: 'produto',
               dateFieldsNames: ['DATA_CADASTRO', 'DATA_ALTERA', 'DATA_EXCLUSAO'],
               data: { produtoId: produtoId },
               success: function () {

                   $scope.produto.PRO_RECEBE_MALA = Boolean($scope.produto.PRO_RECEBE_MALA);
               }
           });
       };
         

   }
});