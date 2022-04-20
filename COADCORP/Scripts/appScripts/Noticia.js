appModule.controller('NoticiaController', function ($scope, formHandlerService, $http, $interval, conversionService, $sce) {
                                                   
    $scope.param = {};
    $scope.editor = {};
    $scope.init = function (id) {

        showAjaxLoader();

        var url = "/Noticia/CarregarTela";
        $http({
            url: url,
            method: "post",
            data: { _id: id }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.noticia = response.result.noticia;
                //---------
                $scope.noticia.DATA_ALTERA = $scope.ConvertDateJSON($scope.noticia.DATA_ALTERA);
                $scope.noticia.DATA_CADASTRO = $scope.ConvertDateJSON($scope.noticia.DATA_CADASTRO);
                $scope.noticia.DATA_PUBLICACAO = $scope.ConvertDateJSON($scope.noticia.DATA_PUBLICACAO);

                //for (var ind in $scope.tabela.TAB_DINAMICA_ITEM) {
                //    $scope.tabela.TAB_DINAMICA_ITEM[ind].TAB_DATA_ALTERA = $scope.ConvertDateJSON($scope.tabela.TAB_DINAMICA_ITEM[ind].TAB_DATA_ALTERA);
                //    $scope.tabela.TAB_DINAMICA_ITEM[ind].TAB_DATA_INCLUSAO = $scope.ConvertDateJSON($scope.tabela.TAB_DINAMICA_ITEM[ind].TAB_DATA_INCLUSAO);
                //}


            }
            else {

                if (response.message != null)
                    $scope.message = Util.createMessage("fail", response.message.message);
                else
                    $scope.message = Util.createMessage("fail", response);

                $scope.noticia = null;

                hideAjaxLoader();
            }

        }).error(function (response) {

            alert(message);

            $scope.message = Util.createMessage("fail", response);
            $scope.listatabela = null;

            hideAjaxLoader();
        });

    }
    $scope.ConvertDateJSON = function (jsondata) {

        var data = null;

        if (jsondata != null)
            data = new Date(parseInt(jsondata.substr(6)));

        return data;
    }
    $scope.listar = function (pageRequest) {

        //if (($scope.param.TGR_ID < 1) && ($scope.param.TIT_ID < 1) && ($scope.param.manchete == null || $scope.param.manchete.length < 3)) {
        //    $scope.message = Util.createMessage("fail", "Informe a manchete ou parte dela para pesquisa.");
        //    $scope.listanoticias = null;
        //}
        //else {

            showAjaxLoader();

            var url = "/Noticia/Pesquisar";
            $http({
                url: url,
                method: "post",
                data: { _manchete:$scope.param.manchete, _grandegrupo:$scope.param.TIT_ID, _class:$scope.param.TGR_ID, _pagina: pageRequest }
            }).success(function (response) {

                hideAjaxLoader();

                if (response.success == true) {

                    $scope.listanoticias = response.result.listanoticias;

                    $scope.page = response.page;
                }
                else {

                    if (response.message!=null)
                        $scope.message = Util.createMessage("fail", response.message.message);
                    else
                        $scope.message = Util.createMessage("fail", response);

                    $scope.listanoticias = null;

                    hideAjaxLoader();
                }

            }).error(function (response) {

                $scope.message = Util.createMessage("fail", response);
                $scope.listanoticias = null;

                hideAjaxLoader();
            });
        //}


    }
    $scope.publicar = function () {

        $http({
            method: 'Post',
            url: '/Noticia/Publicar',
            data: { _noticia: $scope.noticia }

        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.message = Util.createMessage("success", "Notícia publicado com sucesso !!");

            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);

            hideAjaxLoader();
        });

    }
    $scope.removerPublicacao = function () {

        $http({
            method: 'Post',
            url: '/Noticia/RemoverPublicacao',
            data: { _noticia: $scope.noticia }

        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.message = Util.createMessage("success", "Publicação removida com sucesso !!");

            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);

            hideAjaxLoader();
        });

    }

    $scope.salvar = function () {

        showAjaxLoader();

        var url = "/Noticia/Salvar";

        $http({
            url: url,
            method: "Post",
            data: { _noticia: $scope.noticia }
        }).success(function (response) {

            hideAjaxLoader();

            $scope.message = response.message;
            $scope.erros = response.validationMessage;

            if (response.success == true) {
                url = Util.getUrl("/Noticia/Index");
                post(url);
            }


        }).error(function (response) {

            alert(response);

            hideAjaxLoader();
        })

    }
    $scope.excluir = function () {

        showAjaxLoader();

        if (confirm("Deseja excluir este registro?")) {

            $http({
                url: "/Noticia/Excluir",
                method: "Post",
                dataType: 'json',
                data: $scope.noticia
            }).success(function (response) {

                if (response.success) {
                    location.href = "/Noticia/Index";
                }
                else
                    $scope.message = Util.createMessage("fail", response.message.message);

                hideAjaxLoader();

            }).error(function (response) {

                hideAjaxLoader();

                $scope.message = Util.createMessage("fail", response.message.message);

            });
        };

        hideAjaxLoader();

    }
    $scope.dataAtualFormatada = function (dataHora) {

        if (dataHora == null)
            return null;

        var parseDate = new Date(parseInt(dataHora.substr(6)));
        var jsDate = new Date(parseDate);

        return jsDate;
    }
    $scope.trustAsHtml = function (string) {
        return $sce.trustAsHtml(string);
    }

});
