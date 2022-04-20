function InfoMarketingController($scope, formHandlerService, $http, conversionService, cepService) {
          
   // $scope.infoMarketing = { AREA_INFO_MARKETING: [], PRODUTO_COMPOSICAO_INFO_MARKETING: [] };
   
       $scope.$watch("cliente", function (value, old) {

           if (value != old || value != undefined) {

               if (!value.INFO_MARKETING) {

                   $scope.infoMarketing = { AREA_INFO_MARKETING: [], PRODUTO_COMPOSICAO_INFO_MARKETING: [], MTK_CLI_ID: value.CLI_ID };
               }
           }
            
        });
    

    $scope.inicializarInfos = function (CLI_ID) {


        $scope.query = { show: true };
        $scope.getLstOrigemCadastro();
        $scope.getLstAreas();
        $scope.getLstProdutosInteresse();

        
        if (CLI_ID && $scope.infoMarketing && !$scope.infoMarketing.MKT_CLI_ID) {

            $scope.infoMarketing.MKT_CLI_ID = CLI_ID;
        }
        
    }

    $scope.inicializarAreas = function () {

        angular.forEach($scope.lstAreas, function (value, old) {

            value.show = true;
        });

    }

    $scope.inicializarProdutoInteresse = function () {

        angular.forEach($scope.lstProdutosInteresse, function (value, old) {

            value.show = true;
        });

    }

    $scope.getLstOrigemCadastro = function () {

        formHandlerService.read($scope, {
            url: Util.getUrl("/infomarketing/listarorigenscadastro"),
            targetObjectName: 'lstOrigemCadastro',
            responseModelName: 'lstOrigemCadastro',
            success: function () {

            }
        });
        
    }

    $scope.getLstAreas = function () {

        formHandlerService.read($scope, {
            url: Util.getUrl("/infomarketing/listarareas"),
            targetObjectName: 'lstAreas',
            responseModelName: 'lstAreas',
            success: function () {

                $scope.inicializarAreas();

                if ($scope.infoMarketing) {

                    $scope.acharAreaAdicionada($scope.infoMarketing.AREA_INFO_MARKETING);
                }
            }
        });

    }

    $scope.getLstProdutosInteresse = function () {

        formHandlerService.read($scope, {
            url: Util.getUrl("/infomarketing/listarprodutosdeinteresse"),
            targetObjectName: 'lstProdutosInteresse',
            responseModelName: 'lstProdutosInteresse',
            success: function () {

                $scope.inicializarProdutoInteresse();

                if ($scope.infoMarketing) {

                    $scope.acharProdutoInteresseAdicionada($scope.infoMarketing.PRODUTO_COMPOSICAO_INFO_MARKETING);
                }
            }
        });

    }
    
    // -------------------------- Areas
    $scope.adicionarArea = function (area) {

        if (area && $scope.infoMarketing && $scope.infoMarketing.AREA_INFO_MARKETING) {

            if ($scope.verificaDuplicacaoArea(area)) {

                $scope.infoMarketing.AREA_INFO_MARKETING.push({ AREAS: angular.copy(area), AREA_ID : area.AREA_ID});
            }
            else {

                $scope.message = Util.createMessage("fail", "Area já adicionada!");
            }

        }
    }

    $scope.verificaDuplicacaoArea = function (area) {

        var achou = true;
        if (area && $scope.infoMarketing && $scope.infoMarketing.AREA_INFO_MARKETING) {

            angular.forEach($scope.infoMarketing.AREA_INFO_MARKETING, function (value, index) {

                if (value.AREAS.AREA_ID === area.AREA_ID) {

                    achou = false;
                    return;
                }
            });
        }

        return achou;
    }

    $scope.$watch("infoMarketing.AREA_INFO_MARKETING", function (value, old) {

        if (value) {
            $scope.acharAreaAdicionada(value);
        }

    }, true);

    $scope.acharAreaAdicionada = function (areaInfoMarketing) {

        $scope.inicializarAreas();

        if (areaInfoMarketing) {

            angular.forEach($scope.lstAreas, function (value, old) {

                angular.forEach(areaInfoMarketing, function (subValue, subOld) {

                    if (subValue.AREAS && value.AREA_ID == subValue.AREAS.AREA_ID) {

                        value.show = false;
                    }
                });
            });
        }

    }

    $scope.excluirArea = function (index, descricao) {

        if ($scope.infoMarketing && $scope.infoMarketing.AREA_INFO_MARKETING) {

            $scope.infoMarketing.AREA_INFO_MARKETING.splice(index, 1);

        }
    }

    //----------------------------------------------------Produtos de interesse -----------------------------
   
    $scope.adicionarProdutoInteresse = function (prod) {

        if (prod && $scope.infoMarketing && $scope.infoMarketing.PRODUTO_COMPOSICAO_INFO_MARKETING) {

            if ($scope.verificaDuplicacaoProdutoComposicao(prod)) {

                $scope.infoMarketing.PRODUTO_COMPOSICAO_INFO_MARKETING.push({ PRODUTO_COMPOSICAO: angular.copy(prod), CMP_ID : prod.CMP_ID, PRO_DESCRICAO : prod.CMP_DESCRICAO });
            }
            else {

                $scope.message = Util.createMessage("fail", "Produto de interesse já adicionada!");
            }

        }
    }

    $scope.verificaDuplicacaoProdutoComposicao = function (prod) {

        var achou = true;
        if (prod && $scope.infoMarketing && $scope.infoMarketing.PRODUTO_COMPOSICAO_INFO_MARKETING) {

            angular.forEach($scope.infoMarketing.PRODUTO_COMPOSICAO_INFO_MARKETING, function (value, index) {

                if (value.PRODUTO_COMPOSICAO.CMP_ID === prod.CMP_ID) {

                    achou = false;
                    return;
                }
            });
        }

        return achou;
    }

    $scope.$watch("infoMarketing.PRODUTO_COMPOSICAO_INFO_MARKETING", function (value, old) {

        if (value) {
            $scope.acharProdutoInteresseAdicionada(value);
        }

    }, true);

    $scope.acharProdutoInteresseAdicionada = function (produto_composicao_info_marketing) {

        $scope.inicializarProdutoInteresse();

        if (produto_composicao_info_marketing) {

            angular.forEach($scope.lstProdutosInteresse, function (value, old) {

                angular.forEach(produto_composicao_info_marketing, function (subValue, subOld) {

                    if (subValue.PRODUTO_COMPOSICAO && value.CMP_ID == subValue.PRODUTO_COMPOSICAO.CMP_ID) {

                        value.show = false;
                    }
                });
            });
        }

    }

    $scope.excluirProdutoInteresse = function (index, descricao) {

        if ($scope.infoMarketing && $scope.infoMarketing.PRODUTO_COMPOSICAO_INFO_MARKETING) {

            $scope.infoMarketing.PRODUTO_COMPOSICAO_INFO_MARKETING.splice(index, 1);

        }
    }

}