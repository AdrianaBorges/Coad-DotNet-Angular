

function TelefoneController($scope, formHandlerService, $http, conversionService, cepService, includeService) {


    $scope.initClienteTelefone = function () {

        $scope.getLstClienteTelefone();
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

        if (tel && (tel.TIPO_TEL_ID == null || tel.CLI_TEL_TELEFONE == null)) {

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

};
