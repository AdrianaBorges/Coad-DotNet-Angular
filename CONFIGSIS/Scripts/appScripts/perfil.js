appModule.controller("PerfilControler", function ($scope, $http) {

    $scope.Confirmar = function () {

        $http({
            url: "/Perfil/Configurar",
            method: "Post",
            dataType: 'json',
            data: JSON.stringify($scope.listaitemmenu)
        }).success(function (response) {

            window.location.href = "/Perfil/Index";

            alert("Registro Alterado com sucesso.");
        })

    };

    $scope.CarregarItemMenu = function () {

        showAjaxLoader();

        $http({
            url: "/Perfil/CarregarItens",
            method: "Post",
            dataType: 'json',
            data: { _per_id: $scope.model.PER_ID, _sis_id: $scope.model.SIS_ID }
        }).success(function (response) {
                    
            hideAjaxLoader();

            if (response.success == true) {

                $scope.listaitemmenu = response.result.listaitemmenu;
                
            }
            else {

                alert(response.message.message);
            }
            
        }).error(function (response) {
            
            alert(response);

            hideAjaxLoader();
        })


    };
    

    $scope.CarregarPerfis = function (_sistema) {

        var _data = { _sis_id: _sistema };

        $http({
            url: "/Perfil/CarregarPerfis",
            method: "Post",
            dataType: 'json',
            data: _data
        }).success(function (response) {

            hideAjaxLoader();

            if (response.success == true) {

                $scope.listaperfis = response.result.listaperfis;
                
            }
            else {

                $scope.message = response.message.message;
            }
            
        }).error(function (response) {
            
            $scope.message = response;

            hideAjaxLoader();
        })

    };

   
    $scope.CarregaPerfis = function (empresa_id) {
        $http({
            url: "/Usuario/Carregar",
            method: "Post",
            dataType: 'json',
            data: { emp_id: empresa_id }
        }).success(function (response) {
            $scope.listaperfil = response;
        })
    };

    $scope.CarregarPerfilUsuario = function (usuario) {
        $http({
            url: "/Usuario/CarregarPerfilUsuario",
            method: "Post",
            dataType: 'json',
            data: { usu_login: usuario }
        }).success(function (response) {
            $scope.lista = response;
        }) 
    };

    $scope.apagaitem = function (item) {

        var ind = $scope.lista.indexOf(item);

        if (ind !== -1) {
            $scope.lista.splice(ind, 1);
        }

    };

    $scope.additem = function (item) {

        existe = false;
        existepadrao = false;

        for (i = 0; i < $scope.lista.length; i++)
        {
            if (item.PER_ID == $scope.lista[i].PER_ID)
            {
                existe = true;
            }

            if ($scope.lista[i].PUS_DEFAULT == 1)
            {
                existepadrao = true;
            }

        }

        if (item && existe == false) {
           if ($scope.lista.indexOf(item) === -1)
           {
               if (existepadrao == true) {
                   item.PUS_DEFAULT = 0;
               }
               else {
                   item.PUS_DEFAULT = 1;
               }

               $scope.lista.push(item);
            }
        }

    };


    $scope.marcartodos = function (item) {

        if (item && item.MenuItens && item.MenuItens.length > 0) {

            angular.forEach(item.MenuItens, function (value) {

                value.MenuNivAcesso = (item.MenuNivAcesso) ? false : true;
                value.MenuNivInsert = (item.MenuNivAcesso) ? false : true;
                value.MenuNivEdit = (item.MenuNivAcesso) ? false : true;
                value.MenuNivDelete = (item.MenuNivAcesso) ? false : true;

            });
        }

    };

    $scope.marcarlinha = function (item, subitem) {

        if (item) {

            item.MenuNivAcesso = true;
            subitem.MenuNivInsert = (subitem.MenuNivAcesso) ? false : true;
            subitem.MenuNivEdit = (subitem.MenuNivAcesso) ? false : true;
            subitem.MenuNivDelete = (subitem.MenuNivAcesso) ? false : true;
        }

    };

    $scope.marcarreverso = function (item, subitem) {


        if (item) {

            if ((subitem.MenuNivInsert == false &&
                 subitem.MenuNivDelete == false &&
                 subitem.MenuNivEdit == false)) {
                item.MenuNivAcesso = true;
                subitem.MenuNivAcesso = true;
            }

        }

    };
});
