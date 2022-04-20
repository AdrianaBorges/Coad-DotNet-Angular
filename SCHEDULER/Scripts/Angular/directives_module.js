var directiveModule = angular.module('directiveModule', []);

directiveModule.directive('appModal', function () {
    
    return {
        restrict: 'AE',
        transclude: true,
        scope: {
            appModal: '@',
            header : '@'
        },
        controller: ['$scope', function (scope) {

            scope.clear = function () {

                if (scope.$parent && scope.$parent.message) {

                    scope.$parent.message = null;
                }
            }
        }],

        template : '<div class="modal fade bs-example-modal-sm" tabindex="-1" role="dialog" aria-labelledby="mySmallModalLabel" aria-hidden="true" id="{{appModal}}">'+
                    '<div class="modal-dialog modal-lg">' +
                      '<div class="modal-content">' +
                          '<div class="modal-header">' +
                           '<button type="button" class="close" data-dismiss="modal" aria-label="Close" ng-click="clear()"><span aria-hidden="true">&times;</span></button>' +
                          '<h4 ng-if="header" class="modal-title">{{header}}</h4>' +                           
                          '</div>' +
                        '<div ng-transclude class="modal-body"></div>' +
                        '</div>' +
                        '</div>' +
                    '</div>'
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
        scope: { appMessage: '@' },
        link: function (scope, element, attr) {

            attr.$observe("appMessage", function (name) {

                if (!name) {
                    name = "message";
                }
                scope.$parent.$watch(name, function (value) {
                    
                    scope.message = value;
                });
            });
        },
        template: '<div ng-show="message" class="alert alert-dismissible"' +
            ' ng-class="{\'alert-warning\' : message.type == \'warning\', \'alert-success\' : message.type == \'success\', \'alert-info\' : message.type == \'info\', \'alert-danger\' : message.type == \'danger\', \'alert-danger\' : message.type == \'fail\'}"' +
                  'role="alert">' +
                    '<div>{{message.message}}</div>'+
                    '</div>'
    };
});

directiveModule.directive('appSwitchButton', function () {

    return {

        scope: {
            appSwitchButton: "@",
            switchConf : '='
        },
        controller : function($scope, $parse){

            $scope.index = 0;

            $scope.processaFila = function () {

                var length = $scope.switchConf.length;
                if ($scope.switchConf && length) {

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

                $scope.index = 1;
                var item = $scope.switchConf[0];
                $scope.button.msg = item.label;
                $scope.button.enabled = true;
                $scope.button.state = item.state;
                return item.state;
            }

            $scope.getItemAtual = function () {

                return $scope.switchConf[$scope.index];
            }       

            $scope.setInParent = function(name, value){

                if (name) {
                    var model = $parse(name);
                    model.assign($scope.$parent, value);
                }
               
            }
        },
        link: function (scope, element, attr) {

            scope.button = {

                msg: attr.defaultValue,
                enabled : true
            };

            if (attr.switchVar) {
                attr.$observe("switchVar", function (name) { 

                    scope.$parent.$watch(name, function (state) {

                        if (state != scope.button.state && state != "reset") {

                            var lst = scope.switchConf;
                            for (var index in lst) {
                                var obj = lst[index];

                                if (obj.state === state) {

                                    scope.index = index;
                                }
                            }
                        }
                        else if (state == "reset") {

                            scope.reset();
                        }
                        
                    });

                    scope.$watch("button.state", function (value) {
                        scope.setInParent(name, value);
                    });

                });
            }

           scope.processaFila();
           
          


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

                        scope.setInParent(attr.switchVar, state);
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
        template : "{{button.msg}}"
    };


});

directiveModule.directive('appPost', function () {
    
    return {
        link: function (scope, element, attr) {
            angular.element(element).click(function (e) {
                e.preventDefault();
                post(attr.appPost);
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
        controller: function ($scope, $parse) {

            $scope.adicionar = function (chave, valor) {

                var model = $parse(chave);
                model.assign($scope, valor);
            }
        },
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
        controller : function($scope, $parse){

            var controller = {

                montaLista: function (numeroPaginas, paginaAtual) {

                    if (paginaAtual) {

                        this.set("paginaAtual", paginaAtual);
                    }
                    var lstPagina = [];
                    for (var index = 1; index <= numeroPaginas; index++) {

                        lstPagina.push(index);
                    }

                    this.set("lstPagina" ,lstPagina);
                },
                
                set: function (name, value) {

                    var model = $parse("objAppPagination." + name);
                    model.assign($scope, value);
                },

                setInScope : function(name, value){

                    var model = $parse(name);
                    model.assign($scope, value);
                }
            };

            return controller;
        },
        link: function (scope, element, attr, controller) {

            scope.appPageLocal = {};
            var pageName = (attr.appPagination) ? attr.appPagination : 'page';

            var executa = true;
            scope.$watch(pageName, function (value) {
                                
                if (value && executa) {
                    executa = false;
                    var numeroPaginas = value.numeroPaginas;
                    controller.set("numeroPaginas", numeroPaginas);
                    var paginaAtual = value.pagina;
                    if (numeroPaginas) {
                        controller.montaLista(numeroPaginas, paginaAtual);

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

                    if (name) {

                        if (scope[name] != page)
                            controller.setInScope(name, page);

                        if (attr.loadFunction) {

                            scope.$eval(attr.loadFunction);
                        }
                    }
                    
                });
                
            };

           
            
        },
        template: '<nav>' +
                        '<ul class="pagination">' + 
                        '<li>' +
                            '<a href="javascript:void(0)" aria-label="Previous" ng-click="anterior()" ng-class="{disabled : !testAnterior}">' +
                            '<span aria-hidden="true">&laquo;</span>' + 
                            '</a>' +
                        '</li>' +
                        '<li ng-repeat="pag in objAppPagination.lstPagina" ng-class="{disabled : pag == objAppPagination.paginaAtual }" ng-click="gotoPage(pag)"><a href="javascript:void(0)">{{pag}}</a></li>' +
                         '<li>' +
                            '<a href="javascript:void(0)" aria-label="Next" ng-click="proximo()" ng-class="{disabled : testProximo}">' +
                            '<span aria-hidden="true">&raquo;</span>' +
                            '</a>' + 
                        '</li>' +
                        '</ul>' +
                   '</nav>'
    };
});

directiveModule.directive('appMask', function () {
    return {
        link: function (scope, element, attr) {
            element.bind("keyUp", function () {
                scope.$apply(function () {
                    alert("");
                    if (attr.appMask) {
                        angular.element(element).mask(attr.appMask);
                    }                    
                });
            });
        }
    };
});
