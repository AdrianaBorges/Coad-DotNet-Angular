appModule.controller('CEPController', function ($scope, formHandlerService, $http, $interval, conversionService, $sce) {
  
    $scope._listaBairro = [];
    $scope.param  = {};
    $scope.filtro = {};
    $scope.filtro.mun_descricao = null;
    $scope.param.cep = null;
    $scope.param.logradouro = null;

    $scope.buscarCep = function (_cep_id) {


        if (_cep_id.length < 8)
            return;

        showAjaxLoader();
        
        var url = "/CEP/BuscarCep";
        $http({
            url: url,
            method: "post",
            data: { _cep: _cep_id }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success) {
                $scope.cep = {};
                $scope.message = Util.createMessage("fail", "CEP já cadastrado");
            }
            else {
                $scope.message = Util.createMessage("fail", response.message.message);
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);

            hideAjaxLoader();
        });


    }
    $scope.init = function (_cep_id) {

        showAjaxLoader();

        var url = "/CEP/BuscarCep";
        $http({
            url: url,
            method: "post",
            data: { _cep: _cep_id }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success) {
                $scope.cep = response.result.cep;
                $scope.cep.END_MUNICIPIO = response.result.cep.MUNICIPIO.MUN_DESCRICAO;
                $scope.TEXTO = response.result.cep.CEP_BAIRRO.BAR_DESCRICAO;
            }

        }).error(function (response) {

            $scope.message =  Util.createMessage("fail", response);

            hideAjaxLoader();
        });

    }

    $scope.salvarCEP = function () {

        showAjaxLoader();

        $scope.cep.CEP_LOG_SEM_ACENTO = $scope.cep.CEP_LOG;

        var url = "/Cep/Salvar";

        $http({
            url: url,
            method: "post",
            data: { _cep: $scope.cep}
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {
                alert(response.message.message);
                url = Util.getUrl("/Cep/Index");
                post(url);
            }
            else {
                $scope.message = Util.createMessage("fail", response.message.message);
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);

            hideAjaxLoader();

        });

    }
    $scope.listar = function (paginaReq) {

        showAjaxLoader();

        var url = "/Cep/Pesquisar";

        $http({
            url: url,
            method: "post",
            data: { _cep: $scope.param.cep, _cep_log: $scope.param.logradouro, _pagina: paginaReq }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.listacep = response.result.listacep;

                $scope.page = response.page;
            }
            else {

                $scope.message = Util.createMessage("fail", response.message.message);
                $scope.listacep = null;

                hideAjaxLoader();
            }

        }).error(function (response) {

            $scope.message = Util.createMessage("fail", response);
            $scope.listacep = null;

            hideAjaxLoader();

        });

    }
    $scope.buscarMunicipio = function () {

        if ($scope.filtro) {
            if ($scope.filtro.mun_descricao != null && $scope.filtro.mun_descricao != "" && $scope.filtro.mun_descricao.length < 3) {
                return;
            }
        }

        showAjaxLoader();

        var url = "/Municipio/BuscarMunicipio";

        $http({
            url: url,
            method: "Post",
            data: { _nomemunicipio: $scope.filtro.mun_descricao }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {
                $scope.dbMunicipio = response.result.dbMunicipio;
                $scope.page = response.page;
            }
            else {
                $scope.dbMunicipio = null;
                $scope.message = Util.createMessage("fail", response.message.message);
            }

        }).error(function (response) {

            $scope.dbMunicipio = null;
            $scope.message = Util.createMessage("fail", response.message.message);

            hideAjaxLoader();
        })

    };
    $scope.buscarBairro = function (_descricao) {

        if ($scope.cep) {
            if (_descricao != null && _descricao != "" && _descricao.length < 3) {
                return;
            }
        }

        showAjaxLoader();

        var url = "/Municipio/BuscarBairro";

        $http({
            url: url,
            method: "Post",
            data: { _bairrodescricao: _descricao }
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {
                $scope._listaBairro = response.result.listaBairro;
            }
            else {
                $scope._listaBairro = null;
            }

        }).error(function (response) {

            $scope._listaBairro = null;

            hideAjaxLoader();
        })

  
    };
    $scope.selecMunicipio = function (_municipio) {
        
        if ($scope.cep == null) {
            $scope.cep = {};
        }

        $scope.cep.CEP_ID = 0;
        $scope.cep.CEP_UF = null;
        //$scope.cep.CEP_LOG = null;
        //$scope.cep.BAR_ID = null;
        //$scope.cep.CEP_NUMERO = null;
        $scope.cep.CEP_TIPO_LOGRADOURO = null;
        $scope.cep.CEP_LOG_SEM_ACENTO = null;
        $scope.cep.MUN_ID = null;

        //-----------

        $scope.cep.MUN_ID = _municipio.MUN_ID;
        $scope.cep.END_MUNICIPIO = _municipio.MUN_DESCRICAO;
        $scope.cep.CEP_UF = _municipio.UF;

        if (_municipio.MUN_CEP != null) {
            $scope.cep.CEP_NUMERO = _municipio.MUN_CEP;
        }

    }
  

});