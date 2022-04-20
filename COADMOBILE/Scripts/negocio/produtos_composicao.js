appModule.controller('ProdutoComposicaoController', function ($scope, formHandlerService, $http, conversionService, messageService) {

    $scope.lstButton = [{ label: 'Incluir', show: true }, { label: 'Inclindo...', show: false }];
    $scope.lstButtonAlterar = [{ label: 'Alterar', show: true }, { label: 'Alterando...', show: false }];
    $scope.lstButtonSave = [{ label: 'Salvar', show: true }, { label: 'Salvando...', show: false }];
    $scope.button = $scope.lstButton[0];
    $scope.buttonSave = $scope.lstButtonSave[0];

    $scope.salvarComposicao = function(){        

        $scope.buttonSave = $scope.lstButtonSave[1];
        formHandlerService.submit($scope, {
            url: Util.getUrl("/produtocomposicao/salvar"),
            objectName: 'produtocomposicao',
            showAjaxLoader: true,
            success: function (resp, status, config, message, validationMessage) {
                $scope.message = message;
                $scope.erros = validationMessage;
                    
                $scope.buttonSave = $scope.lstButtonSave[0];
                if (resp.success) {
                    alert("Salvo com sucesso.");
                    window.location = Util.getUrl("/produtocomposicao/index");
                }
            },
            fail: function () {
                    $scope.buttonSave = $scope.lstButtonSave[0];
                }
        });
   }

   $scope.listar = function (pageRequest) {

       var url = Util.getUrl("/produtocomposicao/produtoscomposicao");

       if (pageRequest) {
           url += "?pagina=" + pageRequest;
       }

       var config = {
           url: url,
           targetObjectName: 'produtosComposicao',
           responseModelName: 'produtosComposicao',
           pageConfig: { pageName: 'page' }
       };

       if ($scope.filtro) {

           config.data = angular.copy($scope.filtro);
       }
       formHandlerService.read($scope, config);
   };

   $scope.read = function (composicaoId) {

       $scope.getLstProdutos();
       $scope.getLstTipoPeriodo();
       if (composicaoId) {

           formHandlerService.read($scope, {
               url: Util.getUrl("/produtocomposicao/readcomposicao"),
               targetObjectName: 'produtocomposicao',
               responseModelName: 'produtocomposicao',
               //dateFieldsNames: ['DATA_CADASTRO', 'DATA_ALTERA', 'DATA_EXCLUSAO'],
               data: { composicaoId: composicaoId},
               success: function () {

                   $scope.produtocomposicao.PRODUTOS = null;
                   // tratamentos para se adequar a tela                   
                   angular.forEach($scope.produtocomposicao.PRODUTO_COMPOSICAO_ITEM, function (value) {

                       value.PRODUTO_COMPOSICAO = null;
                       if (value.PRODUTOS) {
                           value.NomeDoProduto = value.PRODUTOS.PRO_NOME;
                           value.PRODUTO = value.PRODUTOS;
                           value.PRODUTOS = null;
                           
                       }
                   });
               }
           });
       }
       else
       {
           $scope.produtocomposicao = {};
           $scope.produtocomposicao.PRODUTO_COMPOSICAO_ITEM = [];
       }

         

   }

   $scope.getLstProdutos = function () {

     formHandlerService.read($scope, {
           url: Util.getUrl("/produto/lstprodutos"),
           targetObjectName: 'lstprodutos',
           responseModelName: 'produtos',
           success: function () {

               angular.forEach($scope.lstprodutos, function (value) {

                   value.DATA_CADASTRO = conversionService.toDate(value.DATA_CADASTRO);
                   value.DATA_ALTERA = conversionService.toDate(value.DATA_ALTERA);
                   value.DATA_EXCLUSAO = conversionService.toDate(value.DATA_EXCLUSAO);
               });
           }
       });
   }

   $scope.getLstTipoPeriodo = function () {

       formHandlerService.read($scope, {
           url: Util.getUrl("/tipoperiodo/lsttipoperiodo"),
           targetObjectName: 'lsttipoperiodo',
           responseModelName: 'tiposPeriodo',          
       });
   }
  
   $scope.addComposicaoItem = function (composicao, subscope) {

       $scope.button = ($scope.EdicaoIndex == null) ? $scope.lstButton[1] : $scope.lstButtonAlterar[1];

       formHandlerService.submit($scope, {
           url: Util.getUrl("/produtocomposicaoitem/validar"),
           objectName: 'composicaoitem',
           showAjaxLoader: true,
           success: function (resp, status, config, message, validationMessage) {
               $scope.message = message;
               $scope.errosModal = validationMessage;

               if (resp.success) {// se não existe outra composição item com o produto passado

                   var sair = false;
                   angular.forEach($scope.produtocomposicao.PRODUTO_COMPOSICAO_ITEM, function (value, index) { // verifica se não existem dois ou mais itens de composição com o mesmo produto

                       if (($scope.EdicaoIndex == null || ($scope.EdicaoIndex != null && $scope.EdicaoIndex != index)) && composicao.PRO_ID == value.PRO_ID) { // valida se existe o mesmo produto em outro item

                           //value.PRO_ID = null;
                           $scope.message = messageService.fail("Já existe um item de composição com o produto selecionado");
                           sair = true;
                           $scope.button = ($scope.EdicaoIndex == null) ? $scope.lstButton[0] : $scope.lstButtonAlterar[0];
                           //$scope.button = $scope.lstButton[0];
                       }
                   });

                   if (sair)
                       return;

                   composicaoCopiado = angular.copy(composicao); // evita que modificações externas da composição modifique a lista consolidada
                   
                   composicaoCopiado.PRODUTO = composicaoCopiado.PRODUTOSCombo;
                   composicaoCopiado.TIPO_PERIODO = composicaoCopiado.TIPO_PERIODOCombo;
                   if (composicaoCopiado.PRODUTO) {
                       composicaoCopiado.NomeDoProduto = composicaoCopiado.PRODUTO.PRO_NOME;
                   }

                   
                   
                   var index = $scope.EdicaoIndex;
                   if (index != null) {

                       $scope.produtocomposicao.PRODUTO_COMPOSICAO_ITEM[index] = composicaoCopiado;
                       $scope.EdicaoIndex = null;
                   }
                   else {
                       $scope.produtocomposicao.PRODUTO_COMPOSICAO_ITEM.push(composicaoCopiado);
                   }
                   
                   angular.element("#modalComposicaoItem").modal('hide');
                   $scope.composicaoitem = {};
               }
               $scope.button = ($scope.EdicaoIndex == null) ? $scope.lstButton[0] : $scope.lstButtonAlterar[0];
               //$scope.button = $scope.lstButton[0];
               
           },
           fail: function () {
               //$scope.button = $scope.lstButton[0];
               $scope.button = ($scope.EdicaoIndex == null) ? $scope.lstButton[0] : $scope.lstButtonAlterar[0];
           }
       });
      
   }

 
   $scope.produtoSelecionado = function (composicao, item) {

       composicao.PRO_ID = composicao.PRODUTOSCombo.PRO_ID;
   }

   $scope.tipoPeriodoSelecionado = function (composicao, item) {

       composicao.TTP_ID = composicao.TIPO_PERIODOCombo.TTP_ID;
   }

   $scope.removerComposicaoItem = function ($index) {

       if (confirm("Remover essa composição?")) {
           if ($scope.produtocomposicao && $scope.produtocomposicao.PRODUTO_COMPOSICAO_ITEM) {

               $scope.produtocomposicao.PRODUTO_COMPOSICAO_ITEM.splice($index, 1);
           }
       }     
       
   }

   $scope.abrirEdicaoItem = function ($index, item) {

       // recarrega o id dos PRODUTO como se a combobox de ambos tivessem sido modificadas
       item = angular.copy(item);
       item.PRODUTOSCombo = item.PRODUTO;
       item.TIPO_PERIODOCombo = item.TIPO_PERIODO;

       $scope.erros = null;
       $scope.EdicaoIndex = $index;
       $scope.composicaoitem = item;
       angular.element("#modalComposicaoItem").modal();
       $scope.button = $scope.lstButtonAlterar[0];
   }

   $scope.renovarForm = function () {

       $scope.erros = null;
       $scope.composicaoitem = {};
       $scope.EdicaoIndex = null;
   }
});