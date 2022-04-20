appModule.controller('PublicacaoRevisaoController', function ($scope, formHandlerService, $http, conversionService) {

    //
    function objEditor(nomeDoEditor) {
        return document.getElementById(nomeDoEditor);
    }

    //
    $scope.salvarPublicacao = function () {
        formHandlerService.read($scope, {
            url: Util.getUrl("/publicacaoRevisao/salvarPublicacao"),
            targetObjectName: 'salvar',
            responseModelName: 'salvar',
            data: { _pub: $scope._pub, publicacaoAreaConsultoria: $scope._pubArea },
            success: function (retorno) {

            }
        })
    }

    // modal - editor do Digitador
    $scope.modalEditorDigitador = function (_pub, _pubArea) {

        $scope._pub = _pub;
        $scope._pubArea = _pubArea;
        $scope._pubArea.revisao = "D";

        formHandlerService.read($scope, {
            url: Util.getUrl("/publicacaoRevisao/buscarTextoDigitador"),
            targetObjectName: 'revisao',
            responseModelName: 'revisao',
            data: { pub_id: _pub.PUB_ID },
            success: function (retorno) {
                var edt = objEditor("EditorRapido");
                if (edt) {
                    edt = edt.editor;
                    if (!(typeof edt === "undefined")) {
                        edt.SetText($scope.revisao);
                        edt.SetWidth(835);
                        edt.SetHeight(450);
                        edt.Focus();

                        $scope._pub.PUB_CONTEUDO_RESENHA_DGT = $scope.revisao;

                        $scope.tit = "Matéria para ajustes do digitador";

                        $scope.editorAtivo = 'Digitador';

                        angular.element("#modalEditorDigitador").modal();
                    }
                }
            }
        })
    }

    // modal - editor do Diagramador
    $scope.modalEditorDiagramador = function (_pub) {

        $scope._pub = _pub;

        formHandlerService.read($scope, {
            url: Util.getUrl("/publicacaoRevisao/buscarTextoDiagramador"),
            targetObjectName: 'revisao',
            responseModelName: 'revisao',
            data: { pub_id: _pub.PUB_ID },
            success: function (retorno) {
                var edt = objEditor("EditorRapido");
                if (edt) {
                    edt = edt.editor;
                    if (!(typeof edt === "undefined")) {
                        edt.SetText($scope.revisao);
                        edt.SetWidth(835);
                        edt.SetHeight(450);
                        edt.Focus();

                        $scope._pub.PUB_CONTEUDO_RESENHA_RVO = $scope.revisao;

                        $scope.tit = "Matéria do Diagramador";

                        $scope.editorAtivo = 'Diagramador';

                        angular.element("#modalEditorDiagramador").modal();
                    }
                }
            }
        })
    }

    // Modal - Histórico da Matéria...
    $scope.historicoMateria = function (msg) {
        $scope.msg = msg.replace(/;/g, '<br/><br/>');
        $scope.tit = "H I S T Ó R I C O   *   D A   *   M A T É R I A";
        angular.element("#historicoMateria").modal();
    }

    // abre modal historico...
    $scope.modalHistorico = function () {
        angular.element("#modalHistorico").modal();
    };

    // diagramacao...
    $scope.diagramacao = function (colecionadorId, pageRequest) {
        if (!$scope.filtro)
            $scope.filtro = { colecionadorId: colecionadorId };
        if (!pageRequest)
            pageRequest = 1;

        var url = Util.getUrl("/publicacaoRevisao/Diagramacao");
        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }
        var config = {
            url: url,
            targetObjectName: 'revisao',
            responseModelName: 'revisao',
            pageConfig: { pageName: 'page' },
            success: function (retorno) {
            }
        };
        if ($scope.filtro) {
            config.data = angular.copy($scope.filtro);
        }
        formHandlerService.read($scope, config);
    }

    // revisaoTecnica...
    $scope.revisaoTecnica = function (colecionadorId, pageRequest) {
        if (!$scope.filtro)
            $scope.filtro = { colecionadorId: colecionadorId };
        if (!pageRequest)
            pageRequest = 1;

        var url = Util.getUrl("/publicacaoRevisao/RevisaoTecnica");
        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }

        var config = {
            url: url,
            targetObjectName: 'revisao',
            responseModelName: 'revisao',
            pageConfig: { pageName: 'page' },
            success: function (retorno) {
            }
        };

        if ($scope.filtro) {
            config.data = angular.copy($scope.filtro);
        }

        formHandlerService.read($scope, config);
    };

    // aprovar revisão técnica...
    $scope.aprovarRevisaoTecnica = function (revId, publicacaoId) {
        if (confirm("Confirma a Aprovação Técnica da Matéria Nº [" + publicacaoId.toString() + "] ?")) {

            // aprovando...
            formHandlerService.read($scope, {
                url: Util.getUrl("/publicacaoRevisao/aprovarRevisaoTecnica"),
                targetObjectName: 'revisao',
                responseModelName: 'revisao',
                data: { revId: revId },
                success: function (retorno) {
                    alert("Aprovação Técnica realizada com sucesso!");
                    window.location = Util.getUrl("/publicacaoRevisao/aprovacaoRevisaoTecnica");
                    //window.history.back();
                }
            })
        }
    }

    // reprovar revisão técnica...
    $scope.reprovarRevisaoTecnica = function (revId, publicacaoId) {
        if (confirm("Confirma a Reprovação Técnica da Matéria Nº [" + publicacaoId.toString() + "] ?")) {
            // motivo da reprovação da matéria...
            var motivo = prompt("Informe o motivo para a reprovação técnica da matéria", "");

            // reprovando...
            formHandlerService.read($scope, {
                url: Util.getUrl("/publicacaoRevisao/reprovarRevisaoTecnica"),
                targetObjectName: 'revisao',
                responseModelName: 'revisao',
                data: { revId: revId, motivo: motivo },
                success: function (retorno) {
                    alert("Reprovação Técnica realizada com sucesso!");
                    window.location = Util.getUrl("/publicacaoRevisao/aprovacaoRevisaoTecnica");
                    //window.history.back();
                }
            })
        }
    }
    ///////////////////////////////////////////////////////////////////////////////////////////////////////

    // digitacao...
    $scope.digitacao = function (colecionadorId, pageRequest) {
        if (!$scope.filtro)
            $scope.filtro = { colecionadorId: colecionadorId };
        if (!pageRequest)
            pageRequest = 1;

        var url = Util.getUrl("/publicacaoRevisao/digitacao");
        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }

        var config = {
            url: url,
            targetObjectName: 'revisao',
            responseModelName: 'revisao',
            pageConfig: { pageName: 'page' },
            success: function (retorno) {
            }
        };

        if ($scope.filtro) {
            config.data = angular.copy($scope.filtro);
        }
        formHandlerService.read($scope, config);
    };

    // aprovar digitação...
    $scope.aprovarDigitacao = function (revId, publicacaoId) {
        if (confirm("Confirma a Aprovação da Digitação da Matéria Nº [" + publicacaoId.toString() + "] ?")) {

            // aprovando...
            formHandlerService.read($scope, {
                url: Util.getUrl("/publicacaoRevisao/aprovarDigitacao"),
                targetObjectName: 'revisao',
                responseModelName: 'revisao',
                data: { revId: revId },
                success: function (retorno) {
                    alert("Aprovação da Digitação realizada com sucesso!");
                    window.location = Util.getUrl("/publicacaoRevisao/aprovacaoDigitacao");
                    //window.history.back();
                }
            })
        }
    }

    // reprovar digitação...
    $scope.reprovarDigitacao = function (revId, publicacaoId) {
        if (confirm("Confirma a Reprovação da Digitação da Matéria Nº [" + publicacaoId.toString() + "] ?")) {
            // motivo da reprovação da matéria...
            var motivo = prompt("Informe o motivo para a reprovação da matéria", "");

            // reprovando...
            formHandlerService.read($scope, {
                url: Util.getUrl("/publicacaoRevisao/reprovarDigitacao"),
                targetObjectName: 'revisao',
                responseModelName: 'revisao',
                data: { revId: revId, motivo: motivo },
                success: function (retorno) {
                    alert("Reprovação da Digitação realizada com sucesso!");
                    window.location = Util.getUrl("/publicacaoRevisao/aprovacaoDigitacao");
                    //window.history.back();
                }
            })
        }
    }
    ///////////////////////////////////////////////////////////////////////////////////////////////////////

    // revisão ortográfica...
    $scope.revisaoOrtografica = function (colecionadorId, pageRequest) {
        if (!$scope.filtro)
            $scope.filtro = { colecionadorId: colecionadorId };
        if (!pageRequest)
            pageRequest = 1;

        var url = Util.getUrl("/publicacaoRevisao/revisaoOrtografica");
        if (pageRequest) {
            url += "?pagina=" + pageRequest;
        }

        var config = {
            url: url,
            targetObjectName: 'revisao',
            responseModelName: 'revisao',
            pageConfig: { pageName: 'page' },
            success: function (retorno) {
            }
        };

        if ($scope.filtro) {
            config.data = angular.copy($scope.filtro);
        }
        formHandlerService.read($scope, config);
    };

    // aprovar revisao Ortografica...
    $scope.aprovarRevisaoOrtografica = function (revId, publicacaoId) {
        if (confirm("Confirma a Aprovação Ortográfica da Matéria Nº [" + publicacaoId.toString() + "] ?")) {

            // aprovando...
            formHandlerService.read($scope, {
                url: Util.getUrl("/publicacaoRevisao/aprovarRevisaoOrtografica"),
                targetObjectName: 'revisao',
                responseModelName: 'revisao',
                data: { revId: revId },
                success: function (retorno) {
                    alert("Aprovação Ortográfica realizada com sucesso!");
                    window.location = Util.getUrl("/publicacaoRevisao/aprovacaoRevisaoOrtografica");
                    //window.history.back();
                }
            })
        }
    }

    // reprovar revisao Ortografica...
    $scope.reprovarRevisaoOrtografica = function (revId, publicacaoId) {
        if (confirm("Confirma a Reprovação Ortográfica da Matéria Nº [" + publicacaoId.toString() + "] ?")) {
            // motivo da reprovação da matéria...
            var motivo = prompt("Informe o motivo para a reprovação da matéria", "");

            // reprovando...
            formHandlerService.read($scope, {
                url: Util.getUrl("/publicacaoRevisao/reprovarRevisaoOrtografica"),
                targetObjectName: 'revisao',
                responseModelName: 'revisao',
                data: { revId: revId, motivo: motivo },
                success: function (retorno) {
                    alert("Reprovação Ortográfica realizada com sucesso!");
                    window.location = Util.getUrl("/publicacaoRevisao/aprovacaoRevisaoOrtografica");
                    //window.history.back();
                }
            })
        }
    }
    ///////////////////////////////////////////////////////////////////////////////////////////////////////
}).directive('toggle', function () {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            if (attrs.toggle == "tooltip") {
                $(element).tooltip();
            }
            if (attrs.toggle == "popover") {
                $(element).popover();
            }
        }
    };
});

//
function RichTextEditor_OnBlur(editor) {
    if (editor._config.containerid == 'EditorRapido') {
        if (angular.element(document.getElementById('idMateria')).scope().editorAtivo == 'Digitador') {
            angular.element(document.getElementById('idMateria')).scope()._pub.PUB_CONTEUDO_RESENHA_DGT = editor._geh_htmlcode.trim();
        }
    }
}