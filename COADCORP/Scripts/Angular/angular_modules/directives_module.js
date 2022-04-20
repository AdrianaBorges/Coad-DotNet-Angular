var directiveModule = angular.module('directiveModule', []);


directiveModule.directive('appDynamicHtml', ['$compile', function ($compile) {
    return {
        restrict: 'A',
        replace: true,
        link: function (scope, ele, attrs) {
            scope.$watch(attrs.dynamic, function (html) {
                ele.html(html);
                $compile(ele.contents())(scope);
            });
        }
    };
}]);

directiveModule.directive('appModal', function () {
    
    return {
        restrict: 'AE',
        transclude: true,
        scope: {
            appModal: '@',
            header: '@',
            submodal: '@',
            size: '@',
            headerBg: '@',
            showTitle: '@',
            modalCinza: '@',
        },
        transclude: true,
        controller: ['$scope', function (scope) {

            if (scope.showTitle == "false") {

                scope.showHeader = false;
            }
            else {
                scope.showHeader = true;
            }
            

            scope.clear = function ($event) {

                if (scope.$parent && scope.$parent.message) {

                    scope.$parent.message = null;
                }

                angular.element($event.currentTarget).closest("#" + scope.appModal).modal('hide');
            }

            scope.getSize = function(){

                var result = (!scope.size || scope.size == 'lg')
                return result;
            }
            

        }],

        
        template :  '<div class="modal fade bs-example-modal-sm" tabindex="-1" role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true" id="{{appModal}}" ng-class="{\'submodal\' : submodal}">'+
                    '<div class="modal-dialog" ng-class="{\'modal-lg\' : !size || size == \'lg\'}">' +
                        '<div class="modal-content">' +
                            '<div class="modal-header" ng-class="{\'bg-info\' : !headerBg || headerBg == true}" ng-if="showHeader">' +
                            '<button type="button" class="close" ng-click="clear($event)"><span aria-hidden="true">&times;</span></button>' +
                            '<h4 ng-if="header" class="modal-title"><span class="glyphicon glyphicon-search"></span>&nbsp{{header}}</h4>' +
                            '</div>' +
                            '<div ng-transclude class="modal-body" ng-class="{\'background-cinza\' : modalCinza == \'true\'}"></div>' +
                            '</div>' +
                        '</div>' +
                    '</div>'
    };
});

directiveModule.directive('appModalInfo', function () {

    return {
        restrict: 'AE',
        transclude: true,
        scope: {
            appModalInfo: '@',
            header: '@',
            submodal: '@',
            size: '@'
        },
        transclude: true,
        controller: ['$scope', function (scope) {

            scope.clear = function ($event) {

                if (scope.$parent && scope.$parent.message) {

                    scope.$parent.message = null;
                }

                angular.element($event.currentTarget).closest("#" + scope.appModalInfo).modal('hide');
            }

            scope.getSize = function () {

                var result = (!scope.size || scope.size == 'lg')
                return result;
            }
        }],


        template: '<div class="modal fade" tabindex="-1" role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true" id="{{appModalInfo}}" ng-class="{\'submodal\' : submodal}">' +
                    '<div class="modal-dialog" ng-class="{\'modal-lg\' : !size || size == \'lg\'}">' +
                        '<div class="modal-content">' +
                            '<div class="modal-header alert-info">' +
                              '<button type="button" class="close" ng-click="clear($event)"><span aria-hidden="true">&times;</span></button>' +
                              '<h4 ng-if="header" class="modal-title"><span class="glyphicon glyphicon glyphicon-info-sign alert-info" ></span>&nbsp{{header}}</h4>' +
                            '</div>' +
                            '<div ng-transclude class="modal-body"></div>' +
                            '<div class="modal-footer"> ' +
						      ' <button type="button" class="btn btn-primary" ng-click="clear($event)"><i class="fa fa-check"></i> OK</button> ' +
                            '</div>' +
                        '</div>' +
                    '</div>' +
                  '</div>'
    };
});

directiveModule.directive('appModalErro', function () {

    return {
        restrict: 'AE',
        transclude: true,
        scope: {
            appModalErro: '@',
            header: '@',
            submodal: '@',
            size: '@'
        },
        transclude: true,
        controller: ['$scope', function (scope) {

            scope.clear = function ($event) {

                if (scope.$parent && scope.$parent.message) {

                    scope.$parent.message = null;
                }

                angular.element($event.currentTarget).closest("#" + scope.appModalErro).modal('hide');
            }

            scope.getSize = function () {

                var result = (!scope.size || scope.size == 'lg')
                return result;
            }
        }],


        template: '<div class="modal fade" tabindex="-1" role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true" id="{{appModalErro}}" ng-class="{\'submodal\' : submodal}">' +
                    '<div class="modal-dialog" ng-class="{\'modal-lg\' : !size || size == \'lg\'}">' +
                        '<div class="modal-content">' +
                            '<div class="modal-header alert-danger">' +
                              '<button type="button" class="close" ng-click="clear($event)"><span aria-hidden="true">&times;</span></button>' +
                              '<h4 ng-if="header" class="modal-title"><span class="glyphicon glyphicon-exclamation-sign" style="color:red;" ></span>&nbsp{{header}}</h4>' +
                            '</div>' +
                            '<div ng-transclude class="modal-body"></div>' +
                            '<div class="modal-footer"> ' +
						      ' <button type="button" class="btn btn-primary" ng-click="clear($event)"><i class="fa fa-check"></i> OK</button> ' +
                            '</div>' +
                        '</div>' +
                    '</div>' +
                  '</div>'
    };
});

directiveModule.directive('appNoScopeModal', function ($compile) {

    return {
        restrict: 'AE',
        transclude: true,
        scope : false,
        controller: ['$scope', 'scopeBindService', function (scope, scopeBindService) {

            scopeBindService.bindInsertFunction(scope);

            scope.clearModal = function () {

                if (scope && scope.message) {

                    scope.message = null;
                }
            }
        }],        
        
        compile: function (element, attr) {
                        
            
            this.id = attr.appNoScopeModal;
            this.header = attr.header;       

            // template --------------------------------------------
            var template = '<div class="modal fade bs-example-modal-sm" tabindex="-1" role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true" id="' + this.id + '">' +
                    '<div class="modal-dialog modal-lg">' +
                      '<div class="modal-content">' +
                          '<div class="modal-header">' +
                           '<button type="button" class="close" data-dismiss="modal" aria-label="Close" ng-click="clearModal()"><span aria-hidden="true">&times;</span></button>' +
                          '<h4 class="modal-title">' + this.header + '</h4>' +
                          '</div>' +
                        '<div ng-transclude class="modal-body"></div>' +
                        '</div>' +
                        '</div>' +
                    '</div>';
            // ------------------------------------------------------------

            var e = angular.element(template);
            element.append(e);

            return function(scope, element, attr, controller, transclude){
                
            }
            var teste = attr.appNoScopeModal;
            
        },
    };
});


directiveModule.directive('appCustomAutocomplete', function () {

    return {

        restrict: 'A',
        require: '^ngModel',
        scope: {

            url: '@',
            lista: '=',
            minLength: '@',
            listResult: '@',
            appCustomAutocomplete: '@',
            onSelect: '@'
        },
        controller: ['$scope', '$parse', function ($scope, $parse) {
            $scope.set = function (path, value) {

                var model = $parse(path);
                model.assign($scope.$parent, value);
            };

            $scope.autocomplete = function (elemento, attrs, controller, lista) {




                var opts = (Util.isValid(lista)) ? { elementos: lista } : { url: attrs.url };

                if (Util.isValid(attrs.minLength)) {

                    opts.minLength = attrs.minLength;
                }

                var attrListResult = (Util.isValid(attrs.listResult)) ? attrs.listResult : attrs.appCustomAutocomplete;


                if (Util.isValid(attrListResult)) {

                    opts.success = function (resp, indiceValores) {

                        $scope.$parent.$apply(function () {
                            $scope.set(attrListResult, resp);

                            var labels = "";


                            for (var key in indiceValores.indice) {

                                if (indiceValores.indice.hasOwnProperty(key)) {
                                    labels += key + ", ";
                                }
                            }

                            $scope.set(attrs.ngModel, labels);

                        });
                    };
                }

                if (Util.isValid(attrs.onSelect)) {

                    opts.onSelect = function () {

                        console.log("executando on-select");
                        $scope.$parent.$apply(attrs.onSelect);
                    };

                }

                $(elemento).keyup(function () {
                    $(elemento).customAutocomplete(opts);
                });

            };

        }]
		,
        link: function (scope, element, attrs, ngModel) {

            if (Util.isValid(attrs.lista)) {

                attrs.$observe('lista', function (value) {

                    scope.$parent.$watch(value, function (newValue, oldValue) {

                        scope.autocomplete(element, attrs, ngModel, newValue);
                    });
                });
            }
            else {

                scope.autocomplete(element, attrs, ngModel);
            }

            var attrListResult = (Util.isValid(attrs.listResult)) ? attrs.listResult : attrs.appCustomAutocomplete;

            attrs.$observe('ngModel', function (value) {

                scope.set(value, null);
                scope.$parent.$watch(value, function (newValue, oldValue) {

                    if (newValue == oldValue || (newValue == "" && (oldValue == undefined || oldValue == null))) { // garante que o watch não chamará ele mesmo

                        return;
                    }
                    element.val(newValue);
                    if (!Util.isValid(newValue)) {

                        scope.set(attrListResult, null);
                    }
                });
            });

        }
    };
});


directiveModule.directive('appSimpleAutocomplete', function () {

    return {

        restrict: 'A',
        require: '^ngModel',

        scope: {

            url: '@',
            minLength: '@',
            listResult: '@',
            appSimpleAutocomplete: '@',
            onSelect: '@',
            lista: '='
        },
        controller: ['$scope', '$parse', function ($scope, $parse) {



            $scope.set = function (path, value) {

                var model = $parse(path);
                model.assign($scope.$parent, value);
            };
            $scope.autocomplete = function (elemento, attrs, controller, lista) {

                if (Util.isNotValid(lista) && Util.isNotValid(attrs.url)) {
                    return;
                }
                var opts = (Util.isValid(lista)) ? { lista: lista } : { url: attrs.url };

                if (Util.isValid(attrs.minLength)) {

                    opts.minLength = attrs.minLength;
                }

                var attrListResult = (Util.isValid(attrs.listResult)) ? attrs.listResult : attrs.appSimpleAutocomplete;


                if (Util.isValid(attrListResult)) {

                    opts.select = function (resp, label) {

                        $scope.$parent.$apply(function () {

                            $scope.set(attrListResult, resp);
                            $scope.set(attrs.ngModel, label);

                            if (Util.isValid(attrs.onSelect)) {

                                $scope.$parent.$eval(attrs.onSelect);
                            }
                        });
                    };
                }


                //				$(elemento).keyup(function(){
                $(elemento).simpleAutocomplete(opts);
                //				});

            };

        }]
		,
        link: function (scope, element, attrs, ngModel) {

            if (Util.isValid(attrs.lista)) {

                attrs.$observe('lista', function (value) {

                    scope.$parent.$watch(value, function (newValue, oldValue) {

                        if (typeof newValue != 'undefined' && newValue != null) {
                            scope.autocomplete(element, attrs, ngModel, newValue);
                        }
                    });
                });
            }
            else {

                scope.autocomplete(element, attrs, ngModel);
            }

            scope.autocomplete(element, attrs, ngModel);

            var attrListResult = (Util.isValid(attrs.listResult)) ? attrs.listResult : attrs.appSimpleAutocomplete;

            attrs.$observe('ngModel', function (value) {

                scope.set(value, null);
                scope.$parent.$watch(value, function (newValue, oldValue) {

                    if (newValue == oldValue || (newValue == "" && (oldValue == undefined || oldValue == null))) { // garante que o watch não chamará ele mesmo

                        return;
                    }
                    element.val(newValue);
                    if (!Util.isValid(newValue)) {

                        scope.set(attrListResult, null);
                    }
                });
            });

        }
    };
});

directiveModule.directive('appMessage', function () {

    return {
        restrict: 'A',
        scope: { appMessage: '=' },
        link: function (scope, element, attr) {

            attr.$observe("appMessage", function (name) {

                if (!name) {
                    name = "message";
                }

                scope.fechar = function () {
                    scope.$parent[name] = null;
                };

                scope.$parent.$watch(name, function (value) {
                    
                    scope.message = value;

                    if (value && value.subMessages && value.subMessages.length > 0) {
                        value.message = "Ocorreu ao processar essa ação.";
                    }
                });                
                
            });
        },
        template:
            '<div class="div-alert">' +
                '<div ng-show="message" class="alert alert-dismissible"' +
                ' ng-class="{\'alert-warning\' : message.type == \'warning\', \'alert-success\' : message.type == \'success\', \'alert-info\' : message.type == \'info\', \'alert-danger\' : message.type == \'danger\', \'alert-danger\' : message.type == \'fail\'}"' +
                      'role="alert"><button type="button" class="close" ng-click="fechar()"><span aria-hidden="true">&times;</span></button>' +
                        '<div>{{message.message}}</div>'+
                        '</div>' +
                        '<div ng-show="message && message.subMessages">' +
                            '<div class="panel panel-danger semi-opacity" style="border-width: 3px;">' +
                                '<!-- <div class="panel-heading bold head-panel"><span class="glyphicon glyphicon-edit icon-panel"></span> Erros de Validação</div> -->' +
                                    '<div class="panel-body limit-div">' +
                                        '<ul class="lista-sem-estilo lista-limpa">' +
                                            '<li ng-repeat-start="sub in message.subMessages">' +
                                                '<strong class="text-danger">{{$index + 1}} {{sub.message}}</strong>' +
                                            '</li>' +
                                            '<li ng-repeat-end>' +
                                                '<ul class="lista-sem-estilo padding-left" ng-show="sub.ValidationErrors">' +
                                                    '<li ng-repeat-start="(key, value) in sub.ValidationErrors" ng-show="$index == 0">' +
                                                        '<em class="text-danger">Mensagens de Validação</em>' +
                                                    '</li>' +
                                                    '<li ng-repeat-end>' +
                                                        '<ul class="lista-sem-estilo">' +
                                                            '<li ng-repeat="(key, subValue) in value">' +
                                                                '<span class="text-danger">{{$parent.$parent.$index + 1}}.{{$parent.$index + 1}} {{subValue}}</span>' +
                                                            '</li>' +
                                                        '</ul>' +
                                                    '</li>' +
                    			                '</ul>' +
                                               '</li>' +
                                        '</ul>' +
                    		        '</div>' +
                            '</div>' +
                    '</div>' +
                '</div>' +
            '</div>'
    };
});

directiveModule.directive('appSwitchButton', function () {

    return {

        scope: {
            appSwitchButton: "@",
            switchConf: '=',
            switchVar: '=',
            onReset: "@"
        },
        transclude : true,
        controller: ['$scope', '$parse', function ($scope, $parse) {

            $scope.index = 0;

            $scope.processaFila = function () {

                
                if ($scope.switchConf && $scope.switchConf.length) {

                    var length = $scope.switchConf.length;
                    var obj = $scope.switchConf[$scope.index];

                    $scope.button.msg = obj.label;
                    $scope.button.enabled = (obj.disabled === true) ? false : true;
                    $scope.button.state = obj.state;

                    if ($scope.index >= (length - 1))
                        $scope.index = 0;
                    else
                        $scope.index++;

                    return obj.state;
                }
            }

            $scope.reset = function () {

                if ($scope.switchConf && $scope.switchConf.length) {

                    $scope.index = 1;
                    var item = $scope.switchConf[0];
                    $scope.button.msg = item.label;
                    $scope.button.enabled = true;
                    $scope.button.state = item.state;

                    if ($scope.onReset) {

                        $scope.$parent.$eval($scope.onReset);
                    }
                    return item.state;
                }
                return null;
            }

            $scope.getItemAtual = function () {

                return $scope.switchConf[$scope.index];
            }       

            //$scope.setInParent = function(name, value){

            //    if (name) {
            //        var model = $parse(name);
            //        model.assign($scope.$parent, value);
            //    }
               
            //}
        }],
        link: function (scope, element, attr) {

            scope.button = {

                msg: attr.defaultValue,
                enabled : true
            };

              scope.$watch("switchVar", function (state) {

                        if (state != scope.button.state && state != "reset") {

                            var lst = scope.switchConf;
                            for (var index in lst) {
                                var obj = lst[index];

                                if (obj.state === state) {

                                    scope.index = Number(index);
                                    if (scope.index == 0) {
                                        scope.reset();
                                    }
                                }
                            }
                        }
                        else if (state == "reset" || state == null) {

                            scope.reset();
                        }
                        
                    });

                    scope.$watch("button.state", function (value) {
                        scope.switchVar = value;
                    });

           scope.processaFila();       
          

           scope.$watch("switchConf", function (value, old) {

               if (value) {
                   scope.reset();
               }               
           });

           scope.$watch("button.enabled", function (value) {

               if (value == true) {
                   element.removeAttr("disabled");
               }
               else if (value === false) {
                   element.prop("disabled", "disabled");
               }

           });
          
          element.bind('click', function () {
                         
                scope.$apply(function(){
                
                    var state = scope.processaFila();

                    var resp = null;
                    if (attr.appSwitchButton) {

                        scope.switchVar = state;
                        if (scope.$parent){
                            resp = scope.$parent.$eval(attr.appSwitchButton);
                        }
                        else
                            resp = scope.$apply(attr.appSwitchButton);

                        if (resp === false) {
                            scope.reset();
                        }
                    }
                });
                
            });
        },
        template : "<span ng-transclude></span> {{button.msg}}"
    };


});

directiveModule.directive('appPost', function () {
    
    return {
        link: function (scope, element, attr) {
            angular.element(element).click(function (e) {
                e.preventDefault();

                if (attr.newTab) {

                    post(attr.appPost, true);
                }
                else {
                    post(attr.appPost);
                }
                
            });
            
        }
    };
});

directiveModule.directive('appConfirm', function () {

    return {
        link: function (scope, element, attr) {
            angular.element(element).click(function (e) {
                e.preventDefault();
                var msg = (attr.appMsg) ? attr.appMsg : "Confirma?";
                if (confirm(msg)) {
                    post(attr.appConfirm);
                }
            });

        }
    };
});

directiveModule.directive('appSelectable', function () {

    return {
        controller: ['$scope', '$parse', function ($scope, $parse) {

            $scope.adicionar = function (chave, valor) {

                var model = $parse(chave);
                model.assign($scope, valor);
            }
        }],
        link: function (scope, element, attr) {
            angular.element(element).click(function (e) {

                scope.$apply(function () {

                    e.preventDefault();
                    if (attr.value && attr.appSelectable) {

                        var value = scope.$eval(attr.value);                        
                        scope.adicionar(attr.appSelectable, value);
                    }
                });
                
            });

        }
    };
});

directiveModule.directive('appPagination', function () {

    return {
        restrict: 'AE',
        scope: {
            fixPadding: '@',
            pageRequest: '=?'
        },
        controller: ['$scope', '$parse', function ($scope, $parse) {

            var atualizar = true;
            $scope.pageRequest = { pagina: 1 };
            var controller = {
                
                montaLista: function (paginaAtual, numeroPaginas) { // monta a lista de paginação

                    if (paginaAtual) { // joga a pagina a atual no scopo

                        this.set("paginaAtual", paginaAtual);
                    }
                    var lstPagina = []; 

                    var index = 1; // valores padrões da paginação
                    var ate = (numeroPaginas > 8) ? 9 : numeroPaginas;
                    
                    if (paginaAtual >= 8 && paginaAtual <= numeroPaginas) { // se a pagina atual passar o limite de 8 fragmenta a lista de paginação

                        // inclui na lista as primeiras duas paginas
                        lstPagina.push(1);
                        lstPagina.push(2);
                        // pagina negativa indica que será inserido um ... no link da paginação ou seja não será clicavel
                        lstPagina.push(-1);

                        // pega pagina atual e mostra 3 páginas antes e 3 páginas depois da pagina atual
                        index = paginaAtual - 3;
                        ate = paginaAtual + 3;
                        
                        if (numeroPaginas <= ate) {

                            ate = numeroPaginas;
                        }                        
                        //atualizar = true;
                    }/*
                    else if(paginaAtual < 8 && paginaAtual < numeroPaginas) {
                        atualizar = true;
                    }*/

                    /*if (atualizar) {
                        atualizar = false;*/
                        for (index; index <= ate; index++) {

                            lstPagina.push(index);
                        }
                        if (ate + 1 < numeroPaginas) {

                            lstPagina.push(-2);
                            lstPagina.push(numeroPaginas - 1);
                            lstPagina.push(numeroPaginas );
                        }
                        this.set("lstPagina" ,lstPagina);
                   // }
                    
                    
                },
                
                set: function (name, value) {

                    var model = $parse("objAppPagination." + name);
                    model.assign($scope, value);
                },

                setInScope : function(name, value){

                    var model = $parse(name);
                    model.assign($scope.$parent, value);
                }
            };

            return controller;
        }],
        link: function (scope, element, attr, controller) {

            scope.id = (!scope.fixPadding || scope.fixPadding == true) ?
                'pagination' : 'not-padding-fix-pagination';            
            
            scope.appPageLocal = {};
            var pageName = (attr.appPagination) ? attr.appPagination : 'page';

            //var executa = true;
            scope.$parent.$watch(pageName, function (value, old) {
                         
                
                if (value) {
                    //executa = false;
                    var numeroPaginas = value.numeroPaginas;
                    controller.set("numeroPaginas", numeroPaginas);
                    var paginaAtual = value.pagina; 
                    if (numeroPaginas) {
                        controller.montaLista(paginaAtual, numeroPaginas);

                        if(attr.requestPage)
                            controller.setInScope(attr.requestPage, paginaAtual);
                    }
                    
                }
                
            });

            scope.testAnterior = false;
            scope.testProximo = true;
            scope.podeIrParaAnterior = function () {

                if(scope.objAppPagination)
                    scope.testAnterior = (scope.objAppPagination.paginaAtual >= 1);
                return false;
            }

            scope.podeIrProProximo = function () {

                if (scope.objAppPagination) {

                    var paginaAtual = scope.objAppPagination.paginaAtual;
                    scope.testProximo = (scope.objAppPagination.numeroPaginas >= paginaAtual);
                }
                return false;
               
            }

            scope.anterior = function () {

                //if (scope.podeIrParaAnterior()) {
                var paginaAtual = scope.objAppPagination.paginaAtual;
                    
                if (paginaAtual > 1) {
                    scope.gotoPage(paginaAtual - 1);
                }
                   
                //}               
            }

            scope.proximo = function () {

                var paginaAtual = scope.objAppPagination.paginaAtual;
                var numeroPaginas = scope.objAppPagination.numeroPaginas;

                if(paginaAtual < numeroPaginas)
                    scope.gotoPage(paginaAtual + 1);
                              
            }

            scope.gotoPage = function (page) {

                attr.$observe("requestPage", function (name) {

                    controller.set("paginaAtual", page);
                    var paginaAtual = scope.objAppPagination.paginaAtual;
                    var numeroPaginas = scope.objAppPagination.numeroPaginas;

                    controller.montaLista(paginaAtual, numeroPaginas);

                    if (name) {

                        if (scope[name] != page)
                            controller.setInScope(name, page);

                        if (attr.loadFunction) {
                            if (!scope.pageRequest) {

                                scope.pageRequest = { pagina: page };
                            }
                            else {
                                scope.pageRequest.pagina = page;
                            }
                            scope.$parent.$eval(attr.loadFunction);
                        }
                    }
                    
                });
                
            };

           
            
        },
        templateUrl : Util.getUrl('/directives/appPagination')
    };
});



directiveModule.directive('appMaskDinheiro', ['$parse', 'MoneyService', function ($parse, MoneyService) {
    return {

        scope: {
            'appMaskDinheiro': '=',
            'ngModel': '='
        },

        link: function (scope, element, attr, ctrl) {

            //Essas duas variáveis controlam o sentido de excecução dos watchs
            scope.switchModel = false;
            scope.switchValue = false;
            scope.checkAndRunChange = function (change) {

                if (change && scope.switchModel === true) {
                    scope.$parent.$eval(attr.change);
                }
                scope.switchModel = false;
                scope.switchValue = false;
            }

            if (attr.ngModel) {

                scope.$parent.$watch(attr.ngModel, function (value, old) {

                    if (value || value == 0 || old) { // se não existir valor novo e nem anterior não faz nada

                        if (scope.switchValue !== true) {

                            scope.switchModel = true;
                        }
                       
                        var resp = MoneyService.ApplyMask(value, true);
                        //attr.ngModel = resp.valorMascarado;

                        if ((attr.appMaskDinheiro) && (resp.valorFloat != scope.appMaskDinheiro)) {
                            scope.appMaskDinheiro = resp.valorFloat;
                        }
                        else {
                            scope.checkAndRunChange(attr.change);
                        }
                    }
                    else {

                        scope.checkAndRunChange(attr.change);
                    }
                });

                scope.$parent.$watch(attr.appMaskDinheiro, function (value, old) {

                    if ((value || value == 0 || old)) {

                        if (scope.switchModel !== true) {

                            scope.switchValue = true;
                        }
                        if ((value != old) || (!scope.ngModel && value)) {

                            var resp = MoneyService.ApplyMask(value, false);
                            if (resp.valorMascarado != scope.ngModel) {
                                scope.ngModel = resp.valorMascarado;
                            }
                        }
                        else {
                            scope.checkAndRunChange(attr.change);
                        }
                    }
                    else {
                        scope.checkAndRunChange(attr.change);
                    }

                });
            }

        }
    }
}]);

//directiveModule.directive('appMaskDinheiro', function ($parse) {
//    return {

//        scope: {
//            'appMaskDinheiro': '=',
//            'ngModel' : '='
//        },
//        controller: function ($scope, mask, clearMask, $parse) {
                        
//            return {
             
//                mascararValor : function (valor) {

//                    valor = mask('mask_dinheiro')(valor);
//                    $scope.ngModel = valor;
//                },
//                mask : function(valor){
//                    valor = mask('mask_dinheiro')(valor);
//                    return valor;
//                },
//                limparMascara : function (value) {

//                    value = clearMask('mask_dinheiro')(value);
//                    return value;
//                }
          
//          };
//        },

//        link: function (scope, element, attr, ctrl) {

//            //Essas duas variáveis controlam o sentido de excecução dos watchs
//            scope.switchModel = false;
//            scope.switchValue = false;

//            if (attr.ngModel) {

//                scope.$parent.$watch(attr.ngModel, function (value, old) {

//                    if (value || old) { // se não existir valor novo e nem anterior não faz nada

//                        if (scope.switchValue !== true) {

//                            scope.switchModel = true;
//                        }

//                        if (value && value != old || (value && String(value).search(/(.*,.*)/g) == -1)) {

//                            ctrl.mascararValor(value);
//                        }
//                        if (attr.appMaskDinheiro) {

//                            var value = ctrl.limparMascara(value);
//                            var floatValue = (value) ? Number(value) : null;
//                            scope.appMaskDinheiro = floatValue;
//                        }
//                    }
//                    else {

//                        if (attr.change && scope.switchModel === true) {
//                            scope.$parent.$eval(attr.change);
//                        }
//                        scope.switchModel = false;
//                        scope.switchValue = false;
//                    }
//                });

//                scope.$parent.$watch(attr.appMaskDinheiro, function (value, old) {
                 
//                    if ((value || old)) {

//                        if (scope.switchModel !== true) {

//                            scope.switchValue = true;
//                        }
//                        if ((value != old && scope.ngModel != value) || (!scope.ngModel && value)) {

//                            value = Number(value).toFixed(2);
//                            scope.ngModel = value;
//                        }
//                    }
//                    else {
//                        if (attr.change && scope.switchModel === true) {
//                            scope.$parent.$eval(attr.change);
//                        }

//                        scope.switchModel = false;
//                        scope.switchValue = false;
//                    }
                    
//                });
//            }

//        }
//    }
//});



directiveModule.directive('appDate', function () {
    return {

        scope: {
            appDate: '=',
            ngModel: '=',
            appDateChange: '=',
            dateName : '@'
        },
        controller: ['$scope', 'mask', 'clearMask', '$parse', 'conversionService',
            function ($scope, mask, clearMask, $parse, conversionService) {

            var controller = {};
            controller._insert = function (chave, valor) {

                var model = $parse(chave);
                model.assign($scope, valor);
            };

            controller._convertToDate = function (string) {

                return conversionService.toDate(string);
            }

            controller._needConvert = function (value) {

                if (!value || value instanceof Date)
                    return false;
                return (String(value).match(conversionService._datePatternSearch) != -1) ? true : false;

            }

            return controller;
        }],
        link: function (scope, element, attr, controller) {

            if (!attr.showDatePicker || attr.showDatePicker == true) {

                if (scope.dateName) {

                    angular.element(element).attr('id',scope.dateName);
                }

                angular.element(element).datepicker({
                    showOtherMonths: true,
                    selectOtherMonths: true
                });
            }

            if (attr.ngModel) {

                scope.$parent.$watch(attr.ngModel, function (value) {

                    if (value) {
                        console.info(value);

                        var dateArray = value.split("/");
                        var dia = dateArray[0];
                        var mes = dateArray[1] - 1;
                        var ano = dateArray[2];

                        if (scope.appDate) {

                            var date = scope.appDate;
                            date.setDate(1);
                            date.setFullYear(ano);
                            date.setMonth(mes);
                            date.setDate(dia);

                            scope.appDate = date;
                        }
                        else {

                            var date = new Date(ano, mes, dia, 0, 0, 0, 0);

                        }

                        console.info(date);

                        scope.appDate = date;
                    }

                });

                scope.$parent.$watch(attr.appDate, function (value, old) {

                    if (controller._needConvert(value)) {

                        value = controller._convertToDate(value);
                        if ($scope.appDateChange) {

                            $scope.appDateChange();
                        }
                        
                    }
                    else {

                        var ngModel = scope.ngModel;
                        if (!ngModel) {

                            if (value instanceof Date) {

                                var ano = value.getFullYear();
                                var mes = value.getMonth() + 1;

                                if (mes <= 9)
                                    mes = "0" + mes;
                                var dia = value.getDate();

                                if (dia <= 9)
                                    dia = "0" + dia;

                                var date = dia + "/" + mes + "/" + ano;

                                scope.ngModel = date;
                            }
                        }
                    }
                });
            } //           
        }
    }
});



directiveModule.directive('appMonth', function () {
    return {

        scope: {
            appMonth: '=',
            ngModel: '=',
            appDateChange: '=',
            dateName: '@'
        },
        controller: ['$scope', 'mask', 'clearMask', '$parse', 'conversionService',
            function ($scope, mask, clearMask, $parse, conversionService) {

                var controller = {};
                controller._insert = function (chave, valor) {

                    var model = $parse(chave);
                    model.assign($scope, valor);
                };

                controller._convertToDate = function (string) {

                    return conversionService.toDate(string);
                }

                controller._needConvert = function (value) {

                    if (!value || value instanceof Date)
                        return false;
                    return (String(value).match(conversionService._datePatternSearch) != -1) ? true : false;

                }

                controller._bindDate = function(scope, value){

                    if (value) {
                        console.info(value);

                        var dateArray = value.split("/");

                        var mes = dateArray[0] - 1;
                        var ano = dateArray[1];

                        if (scope.appMonth) {

                            var date = scope.appMonth;
                            date.setFullYear(ano);
                            date.setMonth(mes);
                            date.setDate(1);

                            scope.appMonth = date;
                        }
                        else {

                            var date = new Date(ano, mes, 1, 0, 0, 0, 0);

                        }

                        scope.appMonth = date;
                    }
                };

                return controller;
            }],
        link: function (scope, element, attr, controller) {

            if (!attr.showDatePicker || attr.showDatePicker == true) {

                if (scope.dateName) {

                    angular.element(element).attr('id', scope.dateName);
                }

                angular.element(element).MonthPicker({
                    Button: null,
                    OnAfterChooseMonth: function (selectedDate) {
                        
                        scope.appMonth = selectedDate;
                    },
                    i18n: {
                        year: 'Ano',
                        buttonText: 'Selecionar',
                        prevYear: "Ano Anterior", 
                        nextYear: "Próximo Ano",
                        next12Years: 'Próximos 12 anos',
                        prev12Years: '12 anos antes',
                        nextLabel: "Próximo",
                        prevLabel: "Anterior",
                        jumpYears: "Pular Anos",
                        backTo: "Pular Para",
                        months: ["Jan", "Fev", "Mar", "Abr", "Mai", "Jun", "Jul", "Ago", "Set", "Out", "Nov", "Dez"]
                    }
                });
            }

            if (attr.ngModel) {

                scope.$parent.$watch(attr.ngModel, function (value) {

                    controller._bindDate(scope, value);

                });

                scope.$parent.$watch(attr.appMonth, function (value, old) {

                    if (controller._needConvert(value)) {

                        value = controller._convertToDate(value);
                        if ($scope.appDateChange) {

                            $scope.appDateChange();
                        }

                    }
                    else {

                        var ngModel = scope.ngModel;
                        if (!ngModel) {

                            if (value instanceof Date) {

                                var ano = value.getFullYear();
                                var mes = value.getMonth() + 1;

                                if (mes <= 9)
                                    mes = "0" + mes;
                                
                                var date = mes + "/" + ano;

                                scope.ngModel = date;
                            }
                        }
                    }
                });
            }   
        }
    }
});

directiveModule.directive('appTime', function () {
    return {

        scope: { appTime: '=' },       
        controller: ['$scope', 'mask', 'clearMask', '$parse', 'conversionService',
            function ($scope, mask, clearMask, $parse, conversionService) {

            var controller = {};

            controller._convertToDate = function (string) {

                return conversionService.toDate(string);
            }

            controller._needConvert = function (value) {

                if (!value || value instanceof Date)
                    return false;
                return (String(value).match(conversionService._datePatternSearch) != -1) ? true : false;
            }

            return controller;
        }],
        link: function (scope, element, attr, controller) {
            
            if (attr.ngModel) {

                scope.$parent.$watch(attr.ngModel, function (value) {

                    if (value) {
                        console.info(value);                        

                        var dateArray = value.split(":");
                        var hora = Number(dateArray[0]);
                        var min = Number(dateArray[1]);
                        var segundo = dateArray[2];

                        if (scope.appTime) {

                            var date = scope.appTime;

                            date.setHours(hora);
                            date.setMinutes(min);

                            scope.appTime = date;

                        }
                        else {

                            scope.appTime = new Date(0, 0, 0, hora, min, 0, 0);
                        }
                             
                    }

                });

                scope.$parent.$watch(attr.appTime, function (value, old) {

                    if ((value != old)) {

                        if (controller._needConvert(value)) {

                            value = controller._convertToDate(value);
                        }

                        else {

                            if (!scope.appTime) {

                                if (value instanceof Date) {


                                    var hora = value.getHours();
                                    var min = value.getMinutes();
                                    var segundo = value.getSeconds();

                                    var time = hora + ":" + min;
                                    scope.ngModel = time;
                                }
                            }
                        }
                    }
                });
            }        
        }
    }
});

directiveModule.directive('appValidationMsg', function () {
    return {
        scope: {
            'appValidationMsg': '=',
            'for' : '@',
        },
        link: function (scope, element, attr, controller) {

            var validationResource = (attr.appValidationMsg) ? attr.appValidationMsg : 'validationMessage';

            scope.$parent.$watch(validationResource, function (list) {

                if (attr.for && list) {

                    scope.listValidationsMsg = list[attr.for];

                    if (scope.listValidationsMsg) {
                        element.removeClass("field-validation-valid");
                    }
                    else {
                        element.addClass("field-validation-valid");
                    }
                    
                }
                else {

                    element.addClass("field-validation-valid");
                }
            });
            
        },

        template: '<span ng-repeat="validationMsg in listValidationsMsg" class="message-error">{{validationMsg}}</span>'

    }
});

directiveModule.directive('appShowDate', function () {
    return {
        scope: {
            'appShowDate': '=',
            'showTime' : '@'
        },

        controller: ['$scope', 'conversionService', '$parse', 
            function ($scope, conversionService, $parse) {

            $scope.convert = function (date) {
                if (date instanceof Date)
                    return date;
                return conversionService.toDate(date);
            };
        }],
        link: function (scope, element, attr, controller) {
            
            scope.$parent.$watch(attr.appShowDate, function (value) {

                var date = null
                if (value) {
                    var resp = scope.convert(value);         
                                        

                    var ano = resp.getFullYear();
                    var mes = resp.getMonth() + 1;

                    if (mes <= 9)
                        mes = "0" + mes;
                    var dia = resp.getDate();

                    if (dia <= 9)
                        dia = "0" + dia;

                    date = dia + "/" + mes + "/" + ano;

                    if (attr.showTime == "") {

                        var horas = resp.getHours();

                        if (horas <= 9) {
                            horas = "0" + horas;
                        }

                        var minutos = resp.getMinutes();

                        if (minutos <= 9) {

                            minutos = "0" + minutos;
                        }

                        var seconds = resp.getSeconds();

                        if (seconds <= 9) {

                            seconds = "0" + seconds;
                        }

                        date += " " + horas + ":" +  minutos  + ":" + seconds;
                    }

                }                
                scope.date = date;
            });

        },

        template: '{{date}}'

    }
});

directiveModule.directive('appShowTime', function () {
    return {
        scope: {
            'appShowTime': '='
        },

        controller: ['$scope', 'conversionService', '$parse',
            function ($scope, conversionService, $parse) {

            $scope.convert = function (date) {
                if (date instanceof Date)
                    return date;
                return conversionService.toDate(date);
            };
        }],
        link: function (scope, element, attr, controller) {

            scope.$parent.$watch(attr.appShowTime, function (value) {

                var date = null
                if (value) {
                    var resp = scope.convert(value);
     
                    var horas = resp.getHours();

                    if (horas < 9) {
                        horas = "0" + horas;
                    }

                    var minutos = resp.getMinutes();

                    if (minutos < 9) {

                        minutos = "0" + minutos;
                    }

                    var seconds = resp.getSeconds();

                    if (seconds < 9) {

                        seconds = "0" + seconds;
                    }

                    date = " " + horas + ":" + minutos + ":" + seconds;
                    
               }
                scope.date = date;
            });

        },

        template: '{{date}}'

    }
});

directiveModule.directive('appShowMonth', function () {
    return {
        scope: {
            'appShowMonth': '=',
            'showTime': '@'
        },

        controller: ['$scope', 'conversionService', '$parse',
            function ($scope, conversionService, $parse) {

                $scope.convert = function (date) {
                    if (date instanceof Date)
                        return date;
                    return conversionService.toDate(date);
                };
            }],
        link: function (scope, element, attr, controller) {

            scope.$parent.$watch(attr.appShowMonth, function (value) {

                var date = null
                if (value) {
                    var resp = scope.convert(value);


                    var ano = resp.getFullYear();
                    var mes = resp.getMonth() + 1;

                    if (mes <= 9)
                        mes = "0" + mes;

                    date =  mes + "/" + ano;

                    if (attr.showTime == "") {

                        var horas = resp.getHours();

                        if (horas <= 9) {
                            horas = "0" + horas;
                        }

                        var minutos = resp.getMinutes();

                        if (minutos <= 9) {

                            minutos = "0" + minutos;
                        }

                        var seconds = resp.getSeconds();

                        if (seconds <= 9) {

                            seconds = "0" + seconds;
                        }

                        date += " " + horas + ":" + minutos + ":" + seconds;
                    }

                }
                scope.date = date;
            });

        },

        template: '{{date}}'

    }
});
directiveModule.directive('appShowDinheiro', ['$parse', function ($parse) {
    return {
        scope: {
            'appShowDinheiro': '=',
            'hideWhenZero': '@',
            'outputModel': '@',
            'showSimbolo': '@'
        },

        controller: ['$scope', 'mask', 'clearMask', '$parse',
            function ($scope, mask, clearMask, $parse) {

            $scope.mask = function (valor) {

                valor = mask('mask_dinheiro')(valor);
                return valor;
            };
        }],
        link: function (scope, element, attr, controller) {

            scope.$parent.$watch(attr.appShowDinheiro, function (value) {

                var dinheiro = null;
                if (value) {

                    value = Number(value).toFixed(2);
                    var maskedValue = scope.mask(value);

                    if (scope.outputModel) {

                        var model = $parse(scope.outputModel);
                        model.assign(scope.$parent, maskedValue);
                    }

                    if (scope.showSimbolo != false && scope.showSimbolo != 'false') {
                        dinheiro = "R$ " + maskedValue;
                    }
                    else {
                        dinheiro = maskedValue;
                    }
                }
                else if (!scope.hideWhenZero) {

                    if (scope.showSimbolo != false && scope.showSimbolo != 'false') {

                        dinheiro = "R$ 0,00";
                    }
                    else {
                        dinheiro = "0,00";
                    }
                }
                scope.dinheiro = dinheiro;
            });

        },

        template: '{{dinheiro}}'

    }
}]);
directiveModule.directive('appShowInteiro', ['$parse', function ($parse) {
    return {
        scope: {
            'appShowInteiro': '=',
            'hideWhenZero': '@',
            'outputModel': '@',
            'showSimbolo': '@'
        },

        controller: ['$scope', 'mask', 'clearMask', '$parse',
            function ($scope, mask, clearMask, $parse) {

                $scope.mask = function (valor) {

                    valor = mask('mask_dinheiro')(valor);
                    return valor;
                };
            }],
        link: function (scope, element, attr, controller) {

            scope.$parent.$watch(attr.appShowInteiro, function (value) {

                var inteiro = null;
                if (value) {

                    value = Number(value).toFixed(0);
                 //   var maskedValue = scope.mask(value);

                    if (scope.outputModel) {

                        var model = $parse(scope.outputModel);
                        model.assign(scope.$parent, value);
                    }

                    inteiro = value;
                }
                else if (!scope.hideWhenZero) {

                    inteiro = '0';
                }

                scope.inteiro = inteiro;
            });

        },

        template: '{{inteiro}}'

    }
}]);


directiveModule.directive('appCep', function () {
    return {

        scope: {
            'ngModel': '='
        },
        require : 'ngModel',
        controller: ['$scope', 'scopeBindService', 'cepService',
            function ($scope, scopeBindService, cepService) {

            scopeBindService.bindInsertFunction($scope);

            $scope.loadCep = function (cep, callback, erroCallback) {

                cepService.cep(cep, callback, erroCallback);
            }
        }],
        link: function (scope, element, attr, controller) {

          

                var modelValue = null;

                scope.$parent.$watch(attr.ngModel, function (value) {

                    modelValue = value;
                });

                angular.element(element).blur(function () {

                    scope.loadCep(modelValue, function (enderecoObj) {

                        scope.InsertInParent(attr.appCep, enderecoObj);
                    }
                    );
                });
            }    
    }
});

directiveModule.directive('appCepCampo', function () {

    return {

        scope: {
            'ngModel': '='
        },
        require: 'ngModel',
        controller: ['$scope', 'scopeBindService', function ($scope, scopeBindService) {

            scopeBindService.bindInsertFunction($scope);

        }],
        link: function (scope, element, attr, controller) {

            if (attr.appCepCampo && attr.campo) {

                attr.$observe('appCepCampo', function (name) {

                    scope.$parent.$watch(name, function (value, old) {

                        if (value && value != old) {

                            var mapaCampos = {
                                'uf' : 'estado',
                                'bairro' : 'bairro',
                                'logradouro' : 'logradouro',
                                'cidade' : 'cidade',
                                'municipio' : 'estado_info'
                            };

                            var nomeCampo = mapaCampos[attr.campo];
                            var valorCampo = (nomeCampo == 'estado_info') ? value[nomeCampo]['nome'] : value[nomeCampo];
                            scope.InsertInParent(attr.ngModel, valorCampo);

                            if (attr.ngChange) {

                                scope.$parent.$eval(attr.ngChange);
                            }

                        }
                    });

                });

            }
        }
    }
});


directiveModule.directive('appBindRichTextEvent', ['$parse', function ($parse) {

    return {

        scope: false,
        controller: ['$scope', 'scopeBindService',
            function ($scope, scopeBindService) {

            return {

                _getId: function (editor) {

                    if (editor.Oxc
                        && editor.Oxc.editorelement
                        && editor.Oxc.editorelement.id) {

                        return editor.Oxc.editorelement.id;
                    }
                    return null;
                },
                _getScope : function(editor){

                    var id = this._getId(editor);

                    if ($scope.editores && $scope.editores[id]) {

                        return $scope.editores[id];
                    }
                    return null;
                },
                _set : function(editor){
                    var id = this._getId(editor)
                    if (id) {

                        var model = $parse("editores." + id);
                        var text = editor.GetText();
                        var obj = { text: text, editor: editor, contador: 0 };

                        model.assign($scope, obj);
                    }
                },
                SetValue: function (editor) {

                    var value = this._getScope(editor);

                    if (value) {

                        if (value.text != editor.GetText()) {

                            this._set(editor);
                        }                    
                            
                    }
                    else {

                        this._set(editor);
                    }

                    
                },
                Load : function(editor){

                    var id = this._getId(editor);
                    this.SetValue(editor, false);
                    if ($scope["editores"] && $scope["editores"][id] && $scope.editores[id].preValue) {

                        editor.SetValue($scope.editores[id].preValue);                        
                    }
                }
            };

        }],
        link: function (scope, element, attr, controller) {
            
            window.RichTextEditor_OnLoad = function (editor) {

                scope.$apply(function () {
                    controller.Load(editor)
                });

                editor.AttachEvent("TextChanged", function () {

                    scope.$apply(function () {
                        controller.SetValue(editor, false)
                    });                   
                    
                });

            }
        }
    }
}]);

directiveModule.directive('appRichEditor', ['$parse', function ($parse) {
    return {

        scope: false,
        required : 'ngModel',
        controller: ['$scope', 'scopeBindService', 'cepService',
            function ($scope, scopeBindService, cepService) {

            return {

                SetValue: function (editor, name, scopeName) {

                    if (name) {

                        if (editor.Oxc
                            && editor.Oxc.editorelement
                            && editor.Oxc.editorelement.id) {

                            if (editor.Oxc.editorelement.id == name) {

                                this.SetScope(scopeName, value);
                            }
                        }                    
                       
                    }
                 
                },

                SetScope: function (name, value) {

                    var model = $parse(name);                    
                    model.assign($scope, value);
                    console.info($scope);
                }
            };
        }],
        link: function (scope, element, attr, controller) {
                        
            var editorName = "";
            attr.$observe('appRichEditor', function (name) {

                editorName = name;
                scope.$watch('editores.' + name + '.text', function (value, old) {

                    if (value != old) {

                        var model = $parse(attr.ngModel);
                        model.assign(scope, value);
                    }
                });
            });

            attr.$observe('ngModel', function (modelName) {        

                scope.$watch(modelName, function (value, old) {

                    if (value != old) {
                        
                        if (scope.editores &&
                            scope.editores[editorName]) {

                            if (scope.editores[editorName].text != value) {
                                scope['editores'][editorName].editor.SetText(value);
                            }                          
                            
                        }
                        else if((value && (value.length || value.length > 0))){                                                
                            var model = $parse("editores." + editorName);
                            model.assign(scope, {preValue : value, name : editorName, contador : 0});
                        
                        }  
                    }                   
                    
                })
            });

        }
    }
}]);

directiveModule.directive('appLinkModels', ['$parse', function ($parse) {
    return {

        scope: {
            appLinkModels: '=',
            sourceProperty: '=',
            destinyProperty: '=',
            inicializer: '@'

        },
        restrict: 'EA',
        controller: ['$scope', 'scopeBindService', 'cepService',
            function ($scope, scopeBindService, cepService) {

            var controller = {

                linkModels: function () {

                
                    }               
            };

            return controller;
        }],
        link: function (scope, element, attr, controller) {
            
            //controller.linkModels();

            scope.$watch("appLinkModels", function (value, old) {

                if (value) {

                    if (scope.inicializer) {

                        var val = scope.$parent.$eval(scope.inicializer);
                    }
                    else {
                        var val = {};
                    }

                    if (scope.sourceProperty && !scope.destinyProperty) {
                        scope.destinyProperty = scope.sourceProperty;
                    }

                    if (scope.destinyProperty && !scope.sourceProperty) {
                        scope.sourceProperty = scope.destinyProperty;
                    }

                    if (!scope.sourceProperty && !scope.sourceProperty) {

                        scope.sourceProperty = val;
                        scope.destinyProperty = scope.sourceProperty;
                    }
                }
            });


            scope.$watch("destinyProperty", function (value, old) { // propriedade do objeto

                if (value && value != null) {

                    if (scope.sourceProperty) {

                        scope.sourceProperty = value;
                    }
                }
                //}
                //else if(scope.sourceProperty) {


                //}
            });
        }
    }
}]);



directiveModule.directive('appModelBind', ['$parse', function ($parse) {
    return {

        scope: false,
        restrict : 'EA',
        controller: ['$scope', 'scopeBindService', 'cepService',
            function ($scope, scopeBindService, cepService) {

            return {

                bind: function (sourceName, destinyName, sourceChange, destinyChange, sourceInicializer, destinyInicializer) {

                    $scope.$watch(sourceName, function (value, old) {

                        if (value != old) {
                            
                            if (!value && sourceInicializer) {

                                value = $scope.$eval(sourceInitializer);

                            }
                            var model = $parse(destinyName);
                            model.assign($scope, value);
                            
                            if (sourceChange) {
                                $scope.$eval(sourceChange);
                            }

                        }
                    });

                    if (!sourceInicializer) {
                        
                        $scope.$watch(destinyName, function (value, old) {

                            if (value != old) {

                                if (!value && sourceInicializer) {

                                    value = $scope.$eval(destinyInitializer);

                                }
                                var model = $parse(sourceName);
                                model.assign($scope, value);

                                if (destinyChange) {
                                    $scope.$eval(destinyChange);
                                }
                            }
                        });
                    }
                }
            };
        }],
        link: function (scope, element, attr, controller) {

            if (attr.sourceModel && attr.targetModel) {

                controller.bind(attr.sourceModel, attr.targetModel, attr.sourceChange, attr.destinyChange, attr.sourceInicializer, attr.destinyInicializer)
            }
        }    
    }
}]);

directiveModule.directive('appEditableTd',['$parse', function ($parse) {

    return {
        restrict: 'AE',
        transclude: true,
        scope: {
            appEditableTd: '=',
            label: '=',
            blurEvent: '@',
            isMoney: '@',
            validationPropertyName: '@',
            validationObj: '=',
            disableLabel: '='

        },
        controller: ['$scope', function ($scope) {

                $scope.confirmarEdicao = function ($event) {

                    $event.stopPropagation();
                    $scope.editarItem = false;
                };

                $scope.triggerBlur = function ($event) {

                    if ($scope.disableLabel == true) {

                        $scope.editarItem = true;
                    }
                    else {
                        if ($scope.validationPropertyName && $scope.validationObj) {
                            $scope.validationObj[$scope.validationPropertyName] = [];
                        }

                        $event.stopPropagation();
                        if ($scope.editarItem) {
                            $scope.editarItem = false;
                        }

                        $scope.focused = false;
                    }
                };

                if ($scope.blurEvent) {

                    var model = $parse($scope.blurEvent);
                    model.assign($scope.$parent, function ($event) {

                        console.debug("blur");
                        $scope.triggerBlur($event);
                    });
            
                }
            }],
        link: function (scope, element, attr, controller) {
                        
            var selectElement = element.find(":input")[0];          

            angular.element(selectElement).focus(function () {

                scope.editarItem = true;
            });

            if (selectElement && !attr.blurEvent) {

                selectElement.bind('blur', function (e) {

                    console.debug("blur");
                    scope.triggerBlur(e);
                });
            }
            
            scope.$watch("editarItem", function (value) {

               if (value === true) {
                   var isFocus = angular.element(selectElement).is(":focus");              
                    if (selectElement && !isFocus)
                        selectElement.focus();
                }


            });
                       
            
            scope.ativarEditarItem = function ($event) {
                scope.editarItem = true;
                $event.stopPropagation();
            }

            if (scope.disableLabel == true) {

                scope.editarItem = true;
            }
        },
        template: '<div ng-click="ativarEditarItem($event)" class="pointer">' +
                            '<span ng-show="editarItem || (!appEditableTd && !focused)">' +
                                '<span ng-transclude class="span-transclude"></span>' +
                            '</span>' +
                        '<span ng-if="isMoney" ng-hide="editarItem && appEditableTd" app-show-dinheiro="label" hide-when-zero="true"></span>' +
                        '<span ng-if="!isMoney" ng-hide="editarItem && appEditableTd">{{label}}</span>' +
                        '<div ng-if="validationObj[validationPropertyName]" class="padding"></div>' + 
                        '<div ng-if="validationObj && validationPropertyName" app-validation-msg="validationObj" for="{{validationPropertyName}}"></div>' +
                    '</div>'
    };
}]);


directiveModule.directive("appRadio", ['$parse', function ($parse) {

    return {

        restrict: 'EA',
        scope: {

            ngModel: '=', // ngModel
            list: '=', // nome da lista de valores onde o radio deve trabalhar
            appRadio: '@',
            listModel : '@',
        },
        link: function (scope, element, attr, controller) {
               
             //
            scope.$watch(attr.ngModel, function (value, old) {

                if ((value != old) && (value == 0 || value)) {

                    angular.forEach(scope.list, function (obj, index) {

                        var select = null;

                        if (value == index) {

                            selected = true;
                        }
                        else {
                            selected = false;
                        }

                        var model = $parse(scope.listModel);
                        model.assign(scope.list[index], selected);

                    });                   

                }

                scope.$parent.$watch(scope.appRadio, function (value, old) {

                    value = Boolean(value);
                    if (value === true ) {

                        element.attr('checked', true); 
                    }
                });


            });
           
        }
    };
}]);

directiveModule.directive("appNumber", ['$parse', function ($parse) {

    return {

        require : '^ngModel',
        restrict: 'EA',        
        scope: false
        ,
        link: function (scope, element, attr, controller) {

            scope.$watch(attr.ngModel, function (value, old) {

                if (value && value != old) {
                                     
                    var valueString = String(value);
                    value = valueString.replace(/\D/g, "");

                    if (attr.convert) {
                        value = Number(value);
                    }
                   
                    var model = $parse(attr.ngModel);
                    model.assign(scope, value);
                }
            });
        }
    };
}]);

directiveModule.directive("appDropdownLink", function () {

    return {

        restrict: 'EA',
        scope: {appDropdownLink : '@'},
        link: function (scope, element, attr, controller) {

            element.on("click", function (e) {

                var elementSelector = (scope.appDropdownLink) ? scope.appDropdownLink : "dropdown-menu";
                e.stopPropagation();
                angular.element(elementSelector).dropdown('toogle');                
            });
        }
    };
});


directiveModule.directive("appPopover", ['$parse', '$compile', function ($parse, $compile) {

    return {

        restrict: 'EA',        
        compile: function (element, attrs, transcludeFn) {
            
            return {
                pre: function (scope, element, attrs, ctrl, transcludeFn) {

                    var trigger = (attrs.trigger) ? attrs.trigger : 'hover';
                    var content = null;

                    if (attrs.htmlVar) {

                        var htmlVar = $parse(attrs.htmlVar)(scope);
                        content = $compile(htmlVar)(scope);
                    }
                    else {

                        content = attrs.content;
                    }

                    var title = "Informações";
                    var placement = attrs.direction;
                    var container = attrs.container;
                    var popoverClass = "";
                    var style = "";

                    if (attrs.popoverTitle) {

                        title = attrs.popoverTitle;
                    }

                    if (attrs.fullWidth) {

                        popoverClass = "popover-max";
                    }

                    if (attrs.style) {

                        popoverClass = attrs.style;
                    }

                    if (attrs.style) {

                        style = 'style="' + attrs.style + '"';
                    }

                    var template =
                       '<div class="popover ' + popoverClass + '" ' + style + ' role="tooltip">' +
                           '<div class="arrow"></div>' +
                           '<h3 class="popover-title"></h3>' +
                           '<div class="popover-content"></div>' +
                       '</div>';

                    var opts = {

                        content: content,
                        template: template,
                        trigger: trigger,
                        html: true,
                        title: '<h5 class="custom-title"><span class="glyphicon glyphicon-info-sign"></span>  ' + title + '</h5>',
                        delay: {"hide": 100 }
                    };

                    if (placement) {

                        opts.placement = placement;
                    }

                    if (container) {

                        opts.container = container;
                    }
                    angular.element(element).popover(opts);
                }
            };
        }
    };
}]);



directiveModule.directive("appTooltip", ['$parse', '$compile', function ($parse, $compile) {

    return {

        restrict: 'EA',
        compile: function (element, attrs, transcludeFn) {

            return {
                pre: function (scope, element, attrs, ctrl, transcludeFn) {

                    var trigger = (attrs.trigger) ? attrs.trigger : 'hover';

                    var title = attrs.tooltipTitle;
                    var placement = attrs.direction;
                    var container = attrs.container;


                    var opts = {

                        title: title,
                        trigger: trigger,
                        html: true,
                        delay: { "hide": 100 }
                    };

                    if (placement) {

                        opts.placement = placement;
                    }

                    if (container) {

                        opts.container = container;
                    }

                    angular.element(element).tooltip(opts);
                }
            };
        }
    };
}]);


directiveModule.directive("appHtmlVar", ['$parse', '$compile', function ($parse, $compile) {

    return {

        transclude: true,
        restrict: 'EA',
        compile: function (element, attrs, transcludeFn) {
            
            return {
                pre: function (scope, element, attrs, ctrl, transcludeFn) {
                    transcludeFn(function (clone) {
                        
                        if (attrs.appHtmlVar) {

                            var htmlDom = clone;
                            $parse(attrs.appHtmlVar).assign(scope, htmlDom);
                        }
                    });
                }
            };
        }
    };
}]);


directiveModule.directive("appListPopover", ['$compile', '$parse', function ($compile, $parse) {

    return {

        restrict: 'EA',
        scope: {
            appListPopover: '=',
            label: '@',
        },
        controller: ['$scope', function ($scope) {

            var controller = {

                listar: function () {

                    if ($scope.label && $scope.appListPopover) {

                        var lista = $scope.appListPopover;
                       
                        return html;
                    }
                    return "";
                }

            };

            return controller;
        }],
        link: function (scope, element, attr, controller) {
            
            scope.$watch("appListPopover", function (value, old) {
                
                if (value) {

                    if (scope.label && scope.appListPopover) {

                        var html =
                           "<div>" +
                               "<ul class='format-ul'>";

                        angular.forEach(value, function (value, index) {

                            var actuallyValue = $parse(scope.label)(value);

                            html += "<li>" + actuallyValue + "</li>";
                        });
                        html += "</ul>" +
                        "</div>";
                        element.attr("data-content", html);
                    }
                }
            });

            var trigger = (attr.trigger) ? attr.trigger : 'hover';
            angular.element(element).popover({

                trigger: trigger,
                html: true,
                title: '<h5 class="custom-title"><span class="glyphicon glyphicon-info-sign"></span> Informação</h5>' 
            });
        }
    };
}]);

directiveModule.directive("appInitEditor", ['$parse', '$sce', function ($parse, $sce) {

    return {

        restrict: 'EA',
        scope: {
            appInitEditor: '=',
            saveMethodName: '@',
            isOpen: '=?',
            fullscreen : '@'
        },
        link: function (scope, element, attr, ctrl) {
         
            if (attr.appInitEditor) {
                scope.modalEditorId = attr.appInitEditor;
            }

            scope.cancel = function () {    
                angular.element("#" + scope.modalEditorId).modal('hide');
            }

            tinymce.init({
                selector: 'textarea#app-init-editor',
                height: (scope.fullscreen) ? 800 : 300,
                theme: 'modern',
                force_br_newlines: true,
                force_p_newlines: false,
                forced_root_block: '',
                plugins: [
                  'advlist autolink lists link image charmap print preview hr anchor pagebreak',
                  'searchreplace wordcount visualblocks visualchars code fullscreen',
                  'insertdatetime media nonbreaking save table contextmenu directionality',
                  'emoticons template paste textcolor colorpicker textpattern imagetools'
                ],
                toolbar1: 'insertfile undo redo | styleselect | bold italic | alignleft aligncenter alignright alignjustify | bullist numlist outdent indent | link image',
                toolbar2: 'print preview media | forecolor backcolor emoticons',
                image_advtab: true,
                templates: [
                  { title: 'Test template 1', content: 'Test 1' },
                  { title: 'Test template 2', content: 'Test 2' }
                ],
                content_css: [
                  '//fast.fonts.net/cssapi/e6dc9b99-64fe-4292-ad98-6974f93cd2a2.css',
                  '//www.tinymce.com/css/codepen.min.css'
                ],

                setup: function (ed) {                    

                    scope.saveValue = function () {

                        var content = ed.getContent();
                        var callerNameGetterSetter = $parse("appEditorGlobals.callerName");

                        var callerName = callerNameGetterSetter(scope.$parent);
                        var editorGetterSetter = $parse("appEditorGlobals.editors." + callerName + ".getterSetter");
                        var func = editorGetterSetter(scope.$parent);
                        func(null);
                        func(content);
                        
                    };

                    if (scope.modalEditorId) {
                        
                        var model = $parse("appEditorGlobals." + scope.modalEditorId + ".callModal"); // variável global para uso dos editores


                        if (attr.isOpen) {
                            angular.element("#" + scope.modalEditorId).on('show.bs.modal', function (e) {

                                scope.isOpen = true;
                            });

                            angular.element("#" + scope.modalEditorId).on('hide.bs.modal', function (e) {

                               scope.isOpen = false;
                            });
                        }
                        model.assign(scope.$parent, function (value, editorCallerName) {

                            var model = $parse("appEditorGlobals.callerName");
                            model.assign(scope.$parent, editorCallerName);
                            ed.setContent(value || "");
                            angular.element("#" + scope.modalEditorId).modal();
                        });

                    }
                }
            });
        },
        template:
             '<div id="{{modalEditorId}}" class="modal fade" data-backdrop="static">' +
                        '<div class="modal-dialog" ng-class="{\'modal-lg\' : !fullscreen, \'modal-fullscreen\' : fullscreen}">' +
                            '<div class="modal-content">' +
                                '<div class="modal-body">' +
                                '<textarea id="app-init-editor"></textarea>' +                      
                                '</div>' +
                                '<div class="modal-footer">' +
                                    '<button type="button" class="btn btn-primary" data-dismiss="modal" ng-click="saveValue()"><span class="glyphicon glyphicon-ok"></span> Confirmar</button>' +
                                    '<button type="button" class="btn btn-default" data-dismiss="modal" ng-click="cancel()"><span class="glyphicon glyphicon-remove"></span> Cancelar</button>' +
                                '</div>' +
                            '</div>'+
                        '</div>' +
                    '</div>' 
    };
}]);


directiveModule.directive("appEditor", ['$parse', '$sce',
    function ($parse, $sce) {

    return {

        scope: {
            appEditor : '=',
            editorModel : '=',
            editorId : '@',
            disabled : '@'
        },
        transclude: true,
        restrict: 'EA',
        controller: ['$scope', function ($scope) {

                $scope.trustHTML = function (value) {

                    var trustedContent = $sce.trustAsHtml(value);
                    return trustedContent;
                }
                $scope.isDisabled = function () {

                    if ($scope.disabled) {

                        return $scope.$parent.$eval($scope.disabled);
                    }
                    return false;
                }
            
        }],
        link: function (scope, element, attr, ctrl) {

            var id = angular.element(element).attr('id');
            if (!id) {
                id = attr.appEditor;
            }
            scope.modalEditorId = attr.editorModel;
            scope.openEditor = function () { // editorCallerName é o id do editor que está chamando a modal
                
                var model = $parse("appEditorGlobals." + scope.modalEditorId + ".callModal"); // faz referencia a variável global do editor modal
                var func = model(scope.$parent);

                var value = scope.appEditor
                func(value, id); // chamando a função para passar o valor de model para o editor.
            }

            var editorScope = angular.element("#app-init-editor").scope();
                var model = $parse("appEditorGlobals.editors." + id + ".getterSetter"); // faz referencia a variável global do editor modal
                model.assign(editorScope.$parent, function (value) { // registrando uma função para atualizar o valor do ng-model

                    scope.appEditor = value;
                });
                
            
        },
        template:
            '<div>' +
            '<div ng-hide="isDisabled()" class="edit-modal pull-right" ng-click="openEditor()"><span class="btn-clipboard">Editar</span></div>' +
             '<figure class="highlight">' +
                    '<pre>' +
                    '<div id="{{editorId}}" ng-bind-html="trustHTML(appEditor)" style="min-height: 70px;  padding-top: 26px" class="app-editor"></div>' +
                    '</pre>' +
                    '<div ng-transclude></div>' +
             '</figure>' +
             '</div>'
    };
}]);


directiveModule.directive("appRecursiveTree", ['$compile', '$parse',
    function ($compile, $parse) {

    return {

        scope: false,
        restrict: 'EA',
        transclude: true,
        compile: function (element, attrs, ctrl) {


            return {
                pre: function (scope, element, attrs, ctrl, transcludeFn) {

                    var template = '';
                    var type = 'ul';

                    if (attrs.type) {

                        type = attrs.type;
                    }

                    if (type == 'ul') {

                        template = '<li ng-repeat="${expression}" ${attributes}>${transclude}</li>'
                    }
                    if (type == 'tr') {

                        template = '<tr ng-repeat="${expression}">${transclude}</tr>'
                    }


                    var attr = ' ';
                    for (var index in attrs) {

                        if (attrs.hasOwnProperty(index) && index != 'appRecursiveTree' && index != 'type' && index != '$$element' && index != '$attr') {

                            attr += attrs.$attr[index] + '=' + '"' + attrs[index] + '" ';
                        }
                    }

                    template = template.replace('${expression}', attrs.appRecursiveTree);
                    template = template.replace('${attributes}', attr);

                    transcludeFn(function (clone) {

                        var tempSpan = document.createElement("span");
                        angular.element(tempSpan).append(clone);
                        var cloneHtml = angular.element(tempSpan).html();

                        template = template.replace('${transclude}', cloneHtml);
                    });

                    element.replaceWith($compile(template)(scope));

                    scope.$watch(attrs.childs, function (value) {


                        if (value) {

                            console.info(value);
                        }
                    });

                },
            }
        }
    };
}]);


directiveModule.directive("appRecursive", ['$compile', '$parse', function ($compile, $parse) {

    return {

        scope: true,
        restrict: 'EA',
        transclude: true,
        controller: ['$scope', function ($scope) {

            var controller = {
                template: null,
                include: function (baseHTML, step) {
                    var clone = baseHTML.clone();

                    var tempSpan = document.createElement("span");
                    angular.element(tempSpan).append(clone);
                    var cloneHtml = angular.element(tempSpan).html();
                    return cloneHtml;

                },
                outerHTML: function (obj) {

                    var clone = obj.clone();

                    var tempSpan = document.createElement("span");
                    angular.element(tempSpan).append(clone);
                    var cloneHtml = angular.element(tempSpan).html();
                    return angular.copy(cloneHtml);
                }
            };

            return controller;
        }],
        compile: function (element, attrs, transcludeFn) {
            var elem = element.clone();

            return {
                pre: function (scope, element, attrs, ctrl, transcludeFn) {

                    ctrl.html = ctrl.outerHTML(element);
                    var body = "";
                    transcludeFn(function (clone) {

                        element.append(clone);
                        var resp = ctrl.outerHTML(element);
                        ctrl.html = resp;
                    });

                    // element.append($compile(resp)(scope));

                }
            };
        }
    };
}]);



directiveModule.directive("appClonePoint", ['$compile', function ($compile) {

    return {

        restrict: 'EA',
        require: '^appRecursive',
        compile: function (element, attrs, transcludeFn) {

            return {
                pre: function (scope, element, attrs, ctrl, transcludeFn) {

                    ctrl.html;
                    element.append($compile(ctrl.html)(scope));
                }
            };

        }
    };
}]);


directiveModule.directive("appSelect2", ['$compile', 'formHandlerService', '$parse',
    function ($compile, formHandlerService, $parse) {

    return {

        restrict: 'EA',
        scope: {
            'appSelect2': '=?',
            'itemVar' : '=?',
            'ngModel': '=?',
            'listLabel': '@',
            'listValue': '@',
            'resultList': '@',
            'loadFunction': '@',
            'searchParam': '@',
            'startWithFullList': '@',
            'ctrlVar': '=?',
            'change': '@',
            'disabled': '=',
            'blur': '@',
            'relative' : '@'
        },
        controller: ['$scope', function ($scope) {

            var controller = {
                search: function () {

                    if (($scope.startWithFullList == true && (!$scope.ngModel || $scope.ngModel.length <= 0)) || (($scope.ngModel && $scope.ngModel.length > 0 && ($scope.ngModel.length % 3 == 0 || (!$scope.filtedList || $scope.filtedList.length <= 0))))) {

                        var model = $parse($scope.searchParam);
                        model.assign($scope.$parent, $scope.ngModel);

                        $scope.$parent.$eval($scope.loadFunction);
                    }
                }
            };

            $scope.open = false;
            $scope.selectIndex = -1;
            $scope.query = {};
            $scope.ngDisabled = false;

            if (!$scope.ctrlVar) {

                $scope.ctrlVar = {};
            }

            if (!$scope.resultList) {
                $scope.resultList = "list";
            }

            if (!$scope.respList) {
                $scope.respList = "response";
            }

            if (!$scope.listLabel) {
                $scope.listLabel = "label";
            }

            if (!$scope.listValue) {
                $scope.listValue = "value";
            }

            $scope.openSelect = function ($event) {

                $event.stopPropagation();
                $scope.selectIndex = -1;

                if ($scope.ngModel == null || $scope.ngModel.length <= 0) {

                    controller.search();
                }

                if ($scope.selecionou == true) {
                    $scope.selecionou = false;
                    $scope.open = false;
                }
                else {
                    $scope.open = true;
                }

            }

            $scope.selecionar = function (valor) {

                $scope.selecionou = true;
                $scope.selectIndex = -1;
                $scope.query = {};
                $scope.open = false;

                if (valor) {
                    var scopeValue =  $parse($scope.listValue)(valor);
                    if (scopeValue) {

                        $scope.itemVar = angular.copy(valor);
                        $scope.appSelect2 = scopeValue;
                    }
                    var labelValue = $parse($scope.listLabel)(valor);
                    if (labelValue) {

                        $scope.ngModel = labelValue;
                    }
                    else {
                        $scope.ngModel = valor;
                    }
                }

            }

            $scope._selecionarPorIndex = function () {

                if (($scope.selectIndex || $scope.selectIndex == 0) && $scope.filtedList) {

                    var value = $scope.filtedList[$scope.selectIndex];
                    $scope.selecionar(value);
                }
            }

            $scope.indexarLinha = function ($index) {

                $scope.selectIndex = $index;
            }

            $scope._subirNavegacao = function () {

                if (($scope.selectIndex || $scope.selectIndex == 0) && $scope.selectIndex > 0) {

                    $scope.selectIndex--;
                }
            }

            $scope._descerNavegacao = function () {

                if (($scope.selectIndex || $scope.selectIndex == 0) && $scope.filtedList && $scope.selectIndex < ($scope.filtedList.length - 1)) {

                    $scope.selectIndex++;
                }
            }

            $scope.navegarSelecao = function ($event) {


                switch ($event.which) {
                    case 37: // left
                        break;

                    case 38: // up
                        $scope._subirNavegacao();
                        break;

                    case 39: // right
                        break;

                    case 40: // down
                        $scope._descerNavegacao();
                        break;

                    case 13: // enter
                        $scope._selecionarPorIndex();
                        break;
                    default: return;
                }
                $event.preventDefault();
            }

            $scope.preventBackspace = function ($event) {

                if ($event.which === 8) {
                    $event.preventDefault();
                }
            }

            $scope.acionarBlur = function () {

                if ($scope.ngModel) {

                    $scope.$parent.$eval($scope.blur);
                }
            }


            return controller;
        }],
        transclude: true,
        compile: function (element, attrs, transcludeFn) {

            return {
                pre: function (scope, element, attrs, ctrl, transcludeFn) {
                    


                    if (scope.change) {

                        scope.$parent.$watch(attrs.appSelect2, function (value, oldValue) {

                            if (value != oldValue) {

                                scope.$parent.$eval(scope.change);
                            }
                        });
                    }
                    var blur = ""
                    if (scope.blur) {

                        blur = "ng-blur=\"acionarBlur()\"";
                    }
                    if (attrs.itemVar) {

                        scope.$watch("itemVar", function (value) {

                            if (value) {

                                var itemValue = value[scope.listValue];
                                var itemLabel = value[scope.listLabel];

                                scope.appSelect2 = itemValue;

                                scope.ctrlVar.stayClose = true;
                                scope.ngModel = itemLabel;
                            }
                        });
                    }

                    scope.$watch("ngModel", function (value, old) {

                        scope.query[scope.listLabel] = value;
                        scope.selectIndex = -1;

                        if (!value || value.length <= 0) {

                            scope.appSelect2 = null;
                            scope.itemVar = null;
                        }

                        if ((value && value.length != 0 && (value.length % 3 == 0 || (!scope.filtedList || scope.filtedList.length <= 0))) || (scope.startWithFullList  && (!value || value.length == 0)) && scope.open === true) {

                            ctrl.search();
                            if (scope.ctrlVar && scope.ctrlVar.stayClose === true) {

                                scope.ctrlVar.stayClose = false;
                            }
                            else {
                                scope.open = true;
                            }
                        }
                    });

                    angular.element(document).on('click', function () {

                        if (!scope.$phase) {
                            scope.$apply(function () {

                                scope.selectIndex = null;
                                scope.open = false;
                            });
                        }

                    });
                    
                    var template =
                         "<div class='select2-box'>"
                            + '<div class="input-group">'                            
                            + '<input type="text" id="{{id}}" ng-model="ngModel" class="form-control" ng-focus="openSelect($event)" ng-click="openSelect($event)" ng-keydown="navegarSelecao($event)" app-focus="selectIndex == -1" placeholder="Pesquisar..." ng-disabled="disabled" ' + blur + '/>'
                            + '<span class="input-group-addon"><i class="glyphicon glyphicon-search"></i></span></div>'
                            + '<img src="/Content/themes/base/images/facebook-loader.gif" class="ajax-loader-select2" ng-show="ctrlVar.loader"/>'
                            + "<ul class='select2-ul animate-show-fast' ng-show='open === true && $parent." + scope.resultList + ".length > 0' ng-class=\"{'select2-ul-relative' : relative}\">"
                                  + "<li ng-repeat='item in $parent." + scope.resultList + " | filter:query | as: \"filtedList\"'  ng-click='selecionar(item)'"
                                  + "ng-mouseover='indexarLinha($index)'"
                                    + " ng-class='{active: selectIndex == $index}' tabindex='-1' app-focus='selectIndex == $index' ng-keydown='navegarSelecao($event); preventBackspace($event)'>"
                                      + "<a href='javascript:void(0)'>{{item." + scope.listLabel + "}}</a>"
                                  + "</li>"
                             + "</ul>"
                        + "</div>";
                    //angular.element(element).after($compile(template)(scope));

                    transcludeFn(function (clone) {

                        angular.element(element).append($compile(template)(scope));
                    });
                }
            };

        }
    };
}]);

directiveModule.directive("appChange", ['$compile', function ($compile) {

    return {

        restrict: 'EA',
        require: 'ngModel',
        scope : false,
        link: function (scope, element, attrs, transcludeFn) {

            scope.$watch(attrs.ngModel, function (value, old) {
                
                if (attrs.appChange) {

                    //recursiveEvalFunctionService(scope, attrs.ngChange);
                    scope.$eval(attrs.appChange);
                }                    
            });
        }
    };
}]);


directiveModule.directive("appPrint", ['$parse', function ($parse) {

    return {

        restrict: 'EA',
        scope: false,
        controller : function(){
            return {

                putHtml: function (printContent) {

                    var html = angular.element(printContent).html();
                    angular.element("#printDiv").html(html);

                }
            };
        },
        link: function (scope, element, attrs, ctrl) {

            var printContent = attrs.appPrint;          

            if (printContent) {

                // Variável de controle para indicar que a página foi carregada e pode ser impressa.
                // Essa variável deve ter seu valor alterado para true quando o controller for inicializado
                if (attrs.printFlag) { 

                    scope.$watch(attrs.printFlag, function (newValue, old) { // assim que o valor for true inicializa a diretiva
                        if (newValue == 'true' || newValue == true) {

                            ctrl.putHtml(printContent);
                        }
                    });
                }

                angular.element(element).bind('click', function () {

                    ctrl.putHtml(printContent);
                    window.print();

                });
            }
        }
    };
}]);



directiveModule.directive("appDraggable", ['$parse', 'StoreService', function ($parse, StoreService) {

    return {

        restrict: 'EA',
        scope: {
            ref: '@',
            model: '=',
            'appDraggable': '=',
            'idDrag': '@'

        },
        controller: ['$scope', function ($scope) {
            
            return {
                            
                drag : function (ev) {
                    
                    var transferObj = {
                            modelValue: $scope.model,
                            idDrag: $scope.idDrag
                    };
                    var mod = $parse("transferObj");
                    mod.assign(StoreService.value, transferObj);
                }  
            };
        }],
        link: function (scope, element, attrs, ctrl) {

            element.attr("draggable", true);
            element.addClass("draggable");
            element.bind("dragstart", function ($event) {

                $event.originalEvent.dataTransfer.dropEffect = 'copy';
                ctrl.drag($event);
            });
        }
    };
}]);


directiveModule.directive("appDroppable", ['$parse', 'StoreService', function ($parse, StoreService) {

    return {

        restrict: 'EA',
        scope: false,        
        transclude : true,
        controller: ['$scope', function ($scope) {

            return {

                allowDrop: function (ev) {
                    ev.preventDefault();
                },
                drop: function (ev, attrs) {
                    ev.preventDefault();
                    
                    var value = $parse("transferObj")(StoreService.value);

                    //if ($scope.isList == 'true') {
                    //    if (!$scope.obj) {

                    //        $scope.obj = [];
                    //    }

                    //    $scope.obj.push(value.modelValue);
                    //}
                    //else {
                    //    $scope.obj = value.modelValue;
                    //}

                    $scope.idDrop = attrs.idDrop;
                    $scope.idDrag = value.idDrag;

                    var getterESetter = $parse(attrs.transferModel);

                    getterESetter.assign($scope, value.modelValue);
                    $scope.$eval(attrs.appDroppable);
                }
            };
        }],
        link: function (scope, element, attrs, ctrl) {

            element.bind("drop", function ($event) {
                $event.stopPropagation();
                scope.$apply(function () {
                    ctrl.drop($event, attrs);
                });
            });

            element.bind("dragover", function ($event) {

                ctrl.allowDrop($event);
            });
        },
        template : '<div ng-transclude></div>'
    };
}]);


directiveModule.directive("appFileUpload", ['$parse', 'StoreService', function ($parse, StoreService) {

    return {

        restrict: 'EA',
        scope: {
            model: '=',
            'appFileUpload': '='

        },
        controller: function ($scope) {

            var $ctrl = {

                upload: function () {

                    var data = new FormData();

                    data.append("file", file);

                    $http.post("//myDomain.com/api/demo", data, {
                        headers: { 'Content-Type': undefined },
                        transformRequest: angular.identity
                    }).success(function (data, status, headers, config) {

                    }).error(function (data, status, headers, config) {

                    });
                }

            };

            return $ctrl;
        },
        link: function (scope, element, attrs, ctrl) {
            
            if ($scope.appFileUpload) {

                element.on('change', function ($event) {

                    
                });
            }
        }
    };
}]);

directiveModule.directive("appTextarea", ['$parse', '$sce', function ($parse, $sce) {

    return {

        scope: {
            appTextarea: '=',
            rows: '@',
            statusVar: '=?',
            editorId: '@'
        },
        restrict: 'EA',
        controller: ['$scope', function ($scope) {

            $scope.trustHTML = function (value) {

                var trustedContent = $sce.trustAsHtml(value);
                return trustedContent;
            }

        }],
        link: function (scope, element, attr, ctrl) {

            scope.editorOpen = false;
            scope.openEditor = function (open) {

                scope.editorOpen = open;
                scope.statusVar = scope.editorOpen;
            }
        },
        template:
            '<div ng-hide="editorOpen == true">' +
            '<div class="edit-modal pull-right" ng-click="openEditor(true)"><span class="btn-clipboard">Editar</span></div>' +
             '<figure class="highlight">' +
                    '<pre>' +
                    '<div id="{{editorId}}" ng-bind-html="trustHTML(appTextarea)" style="min-height: 70px;  padding-top: 26px" class="app-editor"></div>' +
                    '</pre>' +
             '</figure>' +
             '</div>' +
             '<div ng-show="editorOpen == true">' +
             '<div class="edit-modal" ng-click="openEditor(false)"><span class="btn-clipboard">Confirmar</span></div>' +
                '<textarea class="form-control" ng-model="appTextarea" rows="{{rows}}">' +
                '</textarea>' +
             '</div>'
    };
}]);


directiveModule.directive("appStatusModal", ['$parse', '$compile', function ($parse, $compile) {

    return {

        scope: false,
        restrict: 'EA',
        controller: ['$scope', function ($scope) {

            $scope.trustHTML = function (value) {

                var trustedContent = $sce.trustAsHtml(value);
                return trustedContent;
            }

        }], compile: function (element, attrs, transcludeFn) {

            return {
                pre: function (scope, element, attrs, ctrl, transcludeFn) {

                    var template = 
                        '<div app-modal="${id}" header="{{${alteracaoModal}.header}}" submodal data-size="small">' +
                            '<div class="row form">' +
                                '<div class="form-group col-lg-12 limit-modal-height">' +
                                    '<label for="motivo">{{${alteracaoModal}.label}}</label>' +
                                    '<textarea class="form-control" ng-model="${alteracaoModal}.OBSERVACOES">' +

                                    '</textarea>' +
                                '</div>' +
                                '<div class="form-group col-lg-12 limit-modal-height">' +
                                     '<button ' +
                                        'app-switch-button="${alteracaoModal}.acaoDoBotao()" ' +
                                        'switch-conf="${alteracaoModal}.buttonConfig" ' +
                                          'switch-var="${alteracaoModal}.buttonVar" ' +
                                          'class="btn btn-default"> ' +
                                          '<span class="glyphicon glyphicon-save"></span> ' +
                                      '</button>' +
                                '</div>' +
                            '</div>' +  
                        '</div>';


                    template = template.replace(/\$\{alteracaoModal\}/g, attrs.appStatusModal);
                    template = template.replace("${id}", attrs.id);

                    var compiledHTML = $compile(template)(scope);
                    angular.element(element).replaceWith(compiledHTML);

                }
            };
        }
    };
}]);


directiveModule.directive("appFiltroLateral", ['$parse', '$compile', 'FilterService',
    function ($parse, $compile, FilterService) {

    return {

        scope: {
            'appFiltroLateral': '=',
            'model': '=',
            'list': '=',
            'acaoPesquisar': '@',
            'flagAbrir': '=?'
        },
        restrict: 'EA',
        controller: ['$scope', function ($scope) {
            $scope.flagAbrir = false;

            $scope.toggleMostrar = function () {

                $scope.flagAbrir = !$scope.flagAbrir;
            }

            $scope.pesquisar = function () {

                if ($scope.acaoPesquisar) {

                    $scope.list = angular.copy($scope.auxList);
                    $scope.$parent.$eval($scope.acaoPesquisar);
                    FilterService.buscarFiltrosPreenchidos($scope, "appFiltroLateral");
                }
            }
            
        }],
        compile: function (element, attrs, transcludeFn) {

            return {
                pre: function (scope, element, attrs, ctrl, transcludeFn) {

                  

                    var template =
                        '<div class="panel panel-default float-left div-filtro animate-show" ng-show="flagAbrir != false">' +
                                            '<div class="div-quebra"></div>' +
                                            '<div class="panel-body" style="padding:4px">' +   
                                                    '<a href="javascript:void(0);" class="remove_link_lateral" ng-click="toggleMostrar();">' +
                                                    '<span class="glyphicon glyphicon-remove red"></span></a>' +

                                                 '<div class="panel panel-default">' +
                                                    '<div class="panel-heading">' +
                                                        'Filtros' +
                                                    '</div>' +
                                                    '<div class="panel-body limit-div-5">' +
                                                        '<fieldset ng-repeat="fil in appFiltroLateral">' +
                                                            '<legend><strong>{{fil.nomeGrupo}}</strong></legend>' +
                                                            '<div class="form">' +
                                                                '<div class="form-group" ng-repeat="itm in fil.filtros | orderBy:\'ordem\'"  ng-show="(itm.renderIf == undefined || itm.renderIf == null || itm.renderIf == true) && ($parent.$index > 0 || ($parent.$index == 0 && $index > 3))">' +
                                                                    '<label for="{{itm.chave + \'_\' + $index}}" class="label-filtro">{{itm.label}}</label>' +
                                                                        '<div ng-switch="itm.tipo">' +
                                                                            '<div ng-switch-when="data">' +
                                                                                '<input type="text"' +
                                                                                       'id="{{itm.chave + \'_\' + $index}}" ' +
                                                                                       'name="{{itm.chave + \'_\' + $index}}"' +
                                                                                       'date-name="{{itm.chave + \'_\' + $index}}"' +
                                                                                       'ng-model="$parent.$parent.$parent.model[itm.chave + \'STR\']"' +
                                                                                        'app-date="$parent.$parent.$parent.model[itm.chave]"' +
                                                                                       'class="form-control form-control-filtro"/>' +
                                                                             '</div>' +
                                                                             '<div ng-switch-when="toogle">' +
                                                                                '<label class="switch switch-success switch-round">' +
                                                                                    '<input type="checkbox" ng-model="$parent.$parent.$parent.model[itm.chave]" id="{{itm.chave + \'_\' + $index}}" ng-checked="$parent.$parent.$parent.model[itm.chave] == true">' +
                                                                                    '<span class="switch-label" data-on="Sim" data-off="Não"></span>' +
                                                                                '</label>' +
                                                                             '</div>' +
                                                                             '<div ng-switch-when="select">' +
                                                                                '<select id="{{itm.chave + \'_\' + $index}}" name="{{itm.chave + \'_\' + $index}}" class="form-control" ng-model="$parent.$parent.$parent.model[itm.chave]"' +
                                                                                        'ng-options="obj.value as obj.label for obj in itm.listCombo">' +
                                                                                    '<option value="">Selecione</option>' +
                                                                                '</select>' +
                                                                             '</div>' +
                                                                             '<div ng-switch-when="autocomplete">' +
                                                                                '<div app-select2="$parent.$parent.$parent.model[itm.chave]"' +
                                                                                        'item-var="$parent.itm.dados.objeto"' +
                                                                                        'ng-model="$parent.$parent.$parent.model[itm.chave + \'STR\']"' +
                                                                                        'result-list="$parent.$parent.$parent.$parent[$parent.$parent.itm.lstAutoCompleteName]"' +
                                                                                        'search-param="itm.autoCompleteParam"' +
                                                                                        'load-function="$parent.$parent.$parent.$parent[itm.funcaoAutoComplete](itm.autoCompleteParam)"' +
                                                                                        'list-value="value"' +
                                                                                        'list-label="label"' +
                                                                                        'ctrl-var="itm.select2Ctrl"' +
                                                                                        'start-with-full-list="false"></div>' +
                                                                             '</div>' +
                                                                             '<div ng-switch-when="grupo">' +
                                                                                '<div ng-repeat="grp in itm.grupo">' +
                                                                                    '<div class="row">' +                                                                                    
                                                                                        '<div class="col-md-1">' +
                                                                                            '<input type="radio"' +
                                                                                               'id="{{itm.chave + \'_\' + $index + \'_\' + $parent.$index}}"' +
                                                                                               'name="{{itm.chave + \'_\' + $parent.$index}}"' +
                                                                                               'ng-model="$parent.$parent.$parent.$parent.model[grp.chave]"' +
                                                                                               'ng-value="grp.valor"' +
                                                                                               '/>' +
                                                                                         '</div>' +
                                                                                        '<div class="col-md-10">' +
                                                                                            '<label for="{{itm.chave + \'_\' + $index + \'_\' + $parent.$index}}" class="label-right" app-tooltip data-placement="top" tooltip-title="{{grp.label}}">' +
                                                                                                '{{grp.label | limitTo: 16}} <span ng-if="grp.label.length > 16">...</span> <span class="label label-info">{{grp.Count}}</span>' +
                                                                                            '</label>' +
                                                                                        '</div>' +
                                                                                    '</div>' +
                                                                                    '<br />' +                                                                                           
                                                                                '</div>' +
                                                                             '</div>' +
                                                                             '<div ng-switch-default>' +
                                                                                '<input type="text"' +
                                                                                       'id="{{itm.chave + \'_\' + $index}}" ' +
                                                                                       'name="{{itm.chave + \'_\' + $index}}"' +
                                                                                       'ng-model="$parent.$parent.$parent.model[itm.chave]" ' +
                                                                                       'class="form-control form-control-filtro" ' +
                                                                                       'maxlength="{{itm.maxLength}}"/>' +
                                                                             '</div>' +
                                                                        '</div>' +
                                                                '</div>' +
                                                            '</div>' +

                                                        '<div if="!$last"><br /><br /></div>' +
                                                        '</fieldset>' +
                                                    '</div>' +
                                                '</div>' +                                                
                                            '</div>' +
                                        '</div>';



                    var compiledHTML = $compile(template)(scope);
                    angular.element(element).html(compiledHTML);

                }
            };
        }
    };
}]);



directiveModule.directive("appFiltroTopo", ['$parse', '$compile', 'FilterService',
    function ($parse, $compile, FilterService) {

        return {

            scope: {
                'appFiltroTopo': '=',
                'model': '=',
                'list': '=',
                'acaoPesquisar': '@',
                'flagAbrir': '=',
                'watchPesquisa': '=?'
            },
            restrict: 'EA',
            controller: ['$scope', '$timeout', function ($scope, $timeout) {

                $scope.pesquisar = function () {

                    if ($scope.acaoPesquisar) {

                        $scope.list = angular.copy($scope.auxList);
                        $scope.$parent.$eval($scope.acaoPesquisar);
                        FilterService.buscarFiltrosPreenchidos($scope, "appFiltroTopo");
                    }
                }

                $scope.removerFiltro = function (chave, $index) {

                    if (chave) {
                        delete $scope.model[chave];
                        delete $scope.model[chave + 'STR'];

                        var token = $scope.listOrdenada[$index];

                        var indexReal = -1;
                        angular.forEach($scope.list, function (value, index) {

                            if (indexReal < 0) {
                                if (value == token)
                                    indexReal = index;
                            }
                        });

                        if (indexReal > -1)
                            $scope.list.splice(indexReal, 1);
                    }
                }

                $scope.toogleFiltrosExtras = function () {

                    $scope.flagAbrir = !$scope.flagAbrir;
                }

                $scope.acaoFiltroBlur = function (fil, grupo) {

                    if (!$scope.auxList)
                        $scope.auxList = [];

                    if (fil && $scope.model && $scope.model[fil.chave]) {

                        fil.value = $scope.model[fil.chave];
                        var achou = false;
                        angular.forEach($scope.auxList, function (value, index) {

                            if (achou === true || fil.chave == value.chave) {

                                achou = true;
                            }

                        });

                        if (achou != true)
                            $scope.auxList.push(fil);
                    }
                }

                $scope.$watch('watchPesquisa', function (value, old) {

                    if (value === true) {
                        $scope.pesquisar();
                        value = false;
                    }
                })               
            }],
            templateUrl: Util.getUrl('/directives/appFiltroTopo')
        };
    }]);


directiveModule.directive("appFiltroTopo2", ['$parse', '$compile', 'FilterService', '$timeout',
function ($parse, $compile, FilterService, $timeout) {

        return {

            scope: {
                'appFiltroTopo2': '=',
                'model': '=',
                'list': '=',
                'acaoPesquisar': '@',
                'flagAbrir': '=',
                'watchPesquisa': '=?'
            },
            restrict: 'EA',
            controller: ['$scope', function ($scope) {

                $scope.pesquisar = function () {

                    if ($scope.acaoPesquisar) {

                        $scope.list = angular.copy($scope.auxList);
                        $scope.$parent.$eval($scope.acaoPesquisar);
                        FilterService.buscarFiltrosPreenchidos($scope, "appFiltroTopo2");
                    }
                }

                $scope.removerFiltro = function (chave, $index) {

                    if (chave) {
                        delete $scope.model[chave];
                        delete $scope.model[chave + 'STR'];

                        var token = $scope.listOrdenada[$index];

                        var indexReal = -1;
                        angular.forEach($scope.list, function (value, index) {

                            if (indexReal < 0) {
                                if (value == token)
                                    indexReal = index;
                            }
                        });

                        if (indexReal > -1)
                            $scope.list.splice(indexReal, 1);
                    }
                }

                $scope.toogleFiltrosExtras = function () {

                    $scope.flagAbrir = !$scope.flagAbrir;
                }

                $scope.acaoFiltroBlur = function (fil, grupo) {

                    if (!$scope.auxList)
                        $scope.auxList = [];

                    if (fil && $scope.model && $scope.model[fil.chave]) {

                        fil.value = $scope.model[fil.chave];
                        var achou = false;
                        angular.forEach($scope.auxList, function (value, index) {

                            if (achou === true || fil.chave == value.chave) {

                                achou = true;
                            }

                        });

                        if (achou != true)
                            $scope.auxList.push(fil);
                    }
                }

                $scope.$watch('watchPesquisa', function (value, old) {

                    if (value === true) {
                        $scope.pesquisar();
                        value = false;
                    }
                });

                $scope.preencherValor = function (objPath, subPath, value) {

                    if (objPath && subPath && (value || value === 0)) {

                        var getter = $parse(objPath + '.' + subPath);
                        getter.assign(value, $scope);
                    }
                };

                $scope.removerTodos = function () {

                    $scope.model = null;
                    $timeout(function () {
                        $scope.pesquisar();
                    });
                };
            }],
            templateUrl : Util.getUrl('/directives/appFiltroTopo2')
        };
    }]);

directiveModule.directive("appFiltroPopover", ['$parse', '$compile', 'FilterService', '$timeout',
function ($parse, $compile, FilterService, $timeout) {

        return {

            scope: {
                'appFiltroPopover': '=',
                'model': '=',
                'list': '=',
                'acaoPesquisar': '@',
                'flagAbrir': '=',
                'watchPesquisa': '=?'
            },
            restrict: 'EA',
            controller: ['$scope', function ($scope) {

                $scope.pesquisar = function () {

                    if ($scope.acaoPesquisar) {

                        $scope.list = angular.copy($scope.auxList);
                        $scope.$parent.$eval($scope.acaoPesquisar);
                        FilterService.buscarFiltrosPreenchidos($scope, "appFiltroPopover");
                    }
                }

                $scope.removerFiltro = function (chave, $index) {

                    if (chave) {
                        delete $scope.model[chave];
                        delete $scope.model[chave + 'STR'];

                        var token = $scope.listOrdenada[$index];

                        var indexReal = -1;
                        angular.forEach($scope.list, function (value, index) {

                            if (indexReal < 0) {
                                if (value == token)
                                    indexReal = index;
                            }
                        });

                        if (indexReal > -1)
                            $scope.list.splice(indexReal, 1);
                    }
                }

                $scope.toogleFiltrosExtras = function () {

                    $scope.flagAbrir = !$scope.flagAbrir;
                }

                $scope.acaoFiltroBlur = function (fil, grupo) {

                    if (!$scope.auxList)
                        $scope.auxList = [];

                    if (fil && $scope.model && $scope.model[fil.chave]) {

                        fil.value = $scope.model[fil.chave];
                        var achou = false;
                        angular.forEach($scope.auxList, function (value, index) {

                            if (achou === true || fil.chave == value.chave) {

                                achou = true;
                            }

                        });

                        if (achou != true)
                            $scope.auxList.push(fil);
                    }
                }

                $scope.$watch('watchPesquisa', function (value, old) {

                    if (value === true) {
                        $scope.pesquisar();
                        value = false;
                    }
                });

                $scope.preencherValor = function (objPath, subPath, value) {

                    if (objPath && subPath && (value || value === 0)) {

                        var getter = $parse(objPath + '.' + subPath);
                        getter.assign(value, $scope);
                    }
                };

                $scope.removerTodos = function () {

                    $scope.model = {};
                    $timeout(function () {
                        $scope.pesquisar();
                    });
                };
            }],
            templateUrl : Util.getUrl('/directives/appFiltroPopover')
        };
    }]);

directiveModule.directive("appManyCheckbox", ['$parse', '$compile', function ($parse, $compile) {

    return {

        scope: {
            appManyCheckbox: '=',
            keyValue: '=',
            controlObj: '=',
            keyName: '@',
            selectWatch : '@'
        },
        restrict: 'EA',
        controller: ['$scope', function ($scope) {

            
        }],
        link: function (scope, element, attrs) {
            scope.$parent.$watch(scope.selectWatch, function (newValue, oldValue) {
                 
                if (newValue === true || newValue === false) {
                    if (!scope.controlObj[scope.keyValue])
                        scope.controlObj[scope.keyValue] = false;
                }

                if (newValue != oldValue && newValue != scope.controlObj[scope.keyValue]) {
                    scope.controlObj[scope.keyValue] = !scope.controlObj[scope.keyValue];
                    scope.adicionarRemoverNaLista();
                }
            });
            scope.adicionarRemoverNaLista = function () {
                
                var checkBox = angular.element(element).find("input[type='checkbox']");
                if (scope.controlObj[scope.keyValue] == true) {

                    if (!scope.appManyCheckbox)
                        scope.appManyCheckbox = [];
                    var obj = { key: scope.keyValue, selected: true };
                    obj[scope.keyName] = scope.keyValue;
                    scope.appManyCheckbox.push(obj);
                }
                else {
                    var checkIndex = -1;
                    var keep = true;
                    angular.forEach(scope.appManyCheckbox, function (value, index) {

                        if (keep && value.key == scope.keyValue) {
                            keep = false;
                            checkIndex = index;
                        }
                    });

                    if (checkIndex > -1) {

                        scope.appManyCheckbox.splice(checkIndex, 1);
                    }

                }
            }
        },
        template : '<input type="checkbox" class="form-control big-checkbox"' + 
                    'ng-model="controlObj[keyValue]"' +
                    'ng-change="adicionarRemoverNaLista()"' +
                    'ng-selected="controlObj[keyValue] == true"' +
                    '>'   
    };
}]);


directiveModule.directive("appHtml", ['$sce', function ($sce) {

    return {

        scope: {
            appHtml: '=',
            appHtmlID: '@',

        },
        restrict: 'EA',
        link: function (scope, element, attrs) {
            scope.trustHTML = function (value) {

                var trustedContent = $sce.trustAsHtml(value);
                return trustedContent;
            }
        },
        template:
            '<figure class="highlight">' +
                '<pre>' +
                '<div id="{{appHtmlID}}" ng-bind-html="trustHTML(appHtml)" style="min-height: 70px; padding-top: 26px"></div>' +
                '</pre>' +
             '</figure>'
    };
}]);



directiveModule.directive("appDropdown", ['$sce', function ($sce) {

    return {

        scope: {
            appDropdown: '=',
            ngModel: '=',
            keyName : '@',
            labelName: '@',
            defaultLabel: '@',
            iconClass: '@',
            ngChange: '@',

        },
        restrict: 'EA',
        link: function (scope, element, attrs) {
            
            scope.$watch('ngModel', function(value, oldValue){
            
                if (!value) {
                    scope.selected = null;
                }
                else
                if(!scope.internalSetting){

                    if (!scope.selected || scope.selected[scope.keyName] != scope.ngModel) {

                        angular.forEach(scope.appDropdown, function(itm, index){
                        
                            if(itm[scope.keyName] == value){
                                scope.selected = itm;
                            }
                        });
                    }
                }
                else{
                    scope.internalSetting = false;
                }

                if (scope.ngChange) {
                    scope.$parent.$eval(scope.ngChange);
                }
            });

            scope.preencherValor = function(value){

                if(scope.keyName && value){
                    scope.selected = value;
                    scope.ngModel = value[scope.keyName];
                    scope.internalSetting = true;
                }
            }

            scope.removerSelecao = function () {

                scope.selected = null;
                scope.ngModel = null;
                scope.internalSetting = true;
            };
        },
        templateUrl: Util.getUrl("/directives/appDropdown")
            
    };
}]);



directiveModule.directive("appOrderingTh", ['$timeout', function ($timeout) {

    return {

        scope: {
            appOrderingTh: '=',
            loadFunction: '@',
            orderProperty: '@'

        },
        transclude: true,
        restrict: 'EA',
        link: function (scope, element, attrs) {

            scope.load = function () {

                if (scope.loadFunction) {

                    scope.$parent.$eval(scope.loadFunction);
                }
            };

            scope.ordernar = function () {

                if (scope.appOrderingTh && scope.appOrderingTh.property != scope.orderProperty) {

                    scope.appOrderingTh = { property: scope.orderProperty, order: 'ASC' };
                    $timeout(function () {
                        scope.load();
                    });
                }
                else {
                    if(!scope.appOrderingTh){
                        scope.appOrderingTh = { property: scope.orderProperty, order: 'ASC' };
                    }
                    else {
                        if (scope.appOrderingTh.order == 'ASC') {
                            scope.appOrderingTh.order = 'DESC';
                        }
                        else {
                            scope.appOrderingTh.order = 'ASC';
                        }
                    }
                    $timeout(function () {
                        scope.load();
                    });
                }
            };
        },
        templateUrl: Util.getUrl("/directives/appOrderingTh")

    };
}]);
