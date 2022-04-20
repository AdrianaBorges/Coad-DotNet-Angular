appModule.controller('TipoMateriaController', function ($scope, formHandlerService, $http, conversionService) {

    $scope.init = function () {
        $scope.filtro = { ativoId: 1 };
    }

    $scope.salvarTipoMateria = function () {
        formHandlerService.submit($scope, {
            url: Util.getUrl("/tipoMateria/salvar"),
            objectName: 'tipoMateria',
            showAjaxLoader: true,
            success: function (resp, status, config, message, validationMessage) {
                $scope.message = message;
                $scope.erros = validationMessage;
                if (resp.success) {
                    alert("Salvo com sucesso.");
                    window.location = Util.getUrl("/tipoMateria/index");
                }
            }
        });
    }

    $scope.listar = function (pageRequest) {
        var url = Util.getUrl("/tipoMateria/tiposMaterias");
        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }
        var config = {
            url: url,
            targetObjectName: 'tiposMaterias',
            responseModelName: 'tiposMaterias',
            pageConfig: { pageName: 'page' }
        };
        if ($scope.filtro) {
            config.data = angular.copy($scope.filtro);
        }
        formHandlerService.read($scope, config);
    };

    $scope.read = function (tipoMateriaId) {
        if (tipoMateriaId) {
            formHandlerService.read($scope, {
                url: Util.getUrl("/tipoMateria/ReadtipoMateria"),
                targetObjectName: 'tipoMateria',
                responseModelName: 'tipoMateria',
                dateFieldsNames: ['DATA_CADASTRO', 'DATA_ALTERA', 'DATA_EXCLUSAO'],
                data: { tipoMateriaId: tipoMateriaId },
                success: function () {
                }
            });
        };
    }
});

function RichTextEditor_OnTextChanged(editor) {
    angular.element(document.getElementById('idMateria')).scope().tipoMateria.ARE_CABECA_MATERIA = editor._geh_htmlcode;
}

function RichTextEditor_OnBlur(editor) {
    angular.element(document.getElementById('idMateria')).scope().tipoMateria.ARE_CABECA_MATERIA = editor._geh_htmlcode;
}