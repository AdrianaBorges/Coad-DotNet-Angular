appModule.controller("TemplatesController", ['$scope', '$http', 'formHandlerService', 'MathService', '$timeout',
    'UploadAjax', 'Upload', 'FilterService', '$sce',
function ($scope, $http, formHandlerService, MathService, $timeout,
    UploadAjax, Upload, FilterService, $sce) {
    
    $scope.initList = function (ehGerente) {
        
        $scope.listarTemplateGrupo();
        $scope.filtro = {pesquisaCpfCnpjPorIqualdade : true};
    }

    $scope.init = function (tplId) {
        if (tplId) {
            $scope.read(tplId, function () {
                 
            });
        }
        else {
            $scope.templateHTML = {};

        }
        $scope.listarTemplateGrupo();
        
    };

    $scope.pesquisarTemplatesHTML = function (pagina) {

        $scope.paginaAtual = pagina;
        $scope.listado = true;
        var url = Util.getUrl("/templates/pesquisarTemplatesHTML");

        if (pagina) {

            url += "?pagina=" + pagina;
        }

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstTemplateHTML',
            responseModelName: 'lstTemplateHTML',
            showAjaxLoader: true,
            data: $scope.filtro,
            pageConfig: { pageName: 'page' /*, pageTargetName: 'pageAgendamentoDoDia' */ },
            success: function (resp) {

            }
        });
    };
    
    $scope.pesquisarFonteDadosTemplate = function (pagina) {

        $scope.paginaAtual = pagina;
        $scope.listado = true;
        var url = Util.getUrl("/templates/pesquisarFonteDadosTemplate");

        if (pagina) {

            url += "?pagina=" + pagina;
        }

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstFonteDadosHTML',
            responseModelName: 'lstFonteDadosHTML',
            showAjaxLoader: true,
            data: $scope.filtro,
            pageConfig: { pageName: 'page' /*, pageTargetName: 'pageAgendamentoDoDia' */ },
            success: function (resp) {

            }
        });
    };
    $scope.listarTemplateGrupo = function () {

        var url = Util.getUrl("/templates/listarTemplateGrupo");

        formHandlerService.read($scope, {
            url: url,
            targetObjectName: 'lstTemplateGrupo',
            responseModelName: 'lstTemplateGrupo',
            showAjaxLoader: true,

        });
    }

    $scope.read = function (tplId, onSuccess) {

        if (tplId != null) {
            var url = Util.getUrl("/templates/RecuperarDadosDoTemplate");

            formHandlerService.read($scope, {
                url: url,
                targetObjectName: 'templateHTML',
                responseModelName: 'templateHTML',
                showAjaxLoader: true,
                data: { tplId: tplId },
                success: function () {
                    
                    if (onSuccess && typeof (onSuccess) == 'function') {

                        onSuccess();
                    }
                }
            });
        }
    }

    $scope.abreModalEnviarEmailParaCliente = function (ppiId, cliId) {

        $scope.modalEnvioEmail = {
            PPI_ID: ppiId,
            CLI_ID: cliId,
        };

        $scope.listarEmailsDoCliente(cliId);
        $scope.listarAssinaturas(cliId);
        angular.element("#modal-enviar-email").modal();
    }

       
    $scope.salvarTemplates = function (sair) {

        formHandlerService.submit($scope, {
            url: Util.getUrl("/templates/SalvarTemplate"),
            objectName: 'templateHTML',
            showAjaxLoader: true,
            success: function (resp, status, config, message, validationMessage) {

                $scope.message = message;
                $scope.erros = validationMessage;

                $scope.button = 'reset';
                $scope.button1 = 'reset';
                if (resp.success) {

                    $timeout(function () {

                        $scope.message = null;
                        if (sair) {
                            window.open(Util.getUrl('/templates/index'), '_self');
                        }
                        else {
                            $scope.abrirPreview($scope.templateHTML.TPL_ID);
                        }

                    }, 1000);

                }
            }
        });
    }


    $scope.abrirLayoutsPartial = function () {

        angular.element("#template-layout-modal").modal();
    }

    $scope.pesquisarLayoutsHTML = function (pagina) {

        if (!$scope.filtro) {

            $scope.filtro = {};
        }
        $scope.filtro.layout = true;
        $scope.pesquisarTemplatesHTML(pagina);
    }

    $scope.adicionarLayout = function (layout) {

        $scope.templateHTML.TEMPLATE_HTML2 = layout;
        $scope.templateHTML.TPL_ID_LAYOUT = layout.TPL_ID;
        angular.element("#template-layout-modal").modal('hide');
    }

    $scope.removerLayout = function () {

        if (Util.isPathValid($scope, "templateHTML.TEMPLATE_HTML2")) {
            $scope.templateHTML.TEMPLATE_HTML2 = null;
            $scope.templateHTML.TPL_ID_LAYOUT = null;
        }
    }

    $scope.checarTipoLayout = function () {

        if ($scope.templateHTML.TPL_ID_LAYOUT) {

            if (!confirm("Esse template possui um layout. Ele não pode ter um layout ao mesmo tempo que é um layout. Se prosseguir o seu layout será removido. Prosseguir?")) {
                $scope.templateHTML.LAYOUT = false;
                return false;
            }
        }

        if ($scope.templateHTML.LAYOUT == true) {
            $scope.removerLayout();
        }
    }

    $scope.trustHTML = function (value) {

        var trustedContent = $sce.trustAsHtml(value);
        return trustedContent;
    }

    $scope.abrirPreview = function (tplId) {

        var url = Util.getUrl("/templates/Prevew?tplId=") + tplId;
        post(url, true);
    }

    $scope.abrirFonteDadosModal = function () {

        angular.element("#fonte-dados-modal").modal();
    }
    
    $scope.adicionarFonteDados = function (fonteDados) {

        $scope.templateHTML.FONTE_DADOS_TEMPLATE = fonteDados;
        $scope.templateHTML.FDA_ID = fonteDados.FDA_ID;
        angular.element("#fonte-dados-modal").modal('hide');
    }

    $scope.removerFonteDados = function () {

        if (Util.isPathValid($scope, "templateHTML.FONTE_DADOS_TEMPLATE")) {
            $scope.templateHTML.FONTE_DADOS_TEMPLATE = null;
            $scope.templateHTML.FDA_ID = null;
        }
    }
}]);