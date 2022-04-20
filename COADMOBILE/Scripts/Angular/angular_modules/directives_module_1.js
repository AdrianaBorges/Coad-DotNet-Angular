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

            var atualizar = true;
            var controller = {
                
                montaLista: function (numeroPaginas, paginaAtual) {

                    if (paginaAtual) {

                        this.set("paginaAtual", paginaAtual);
                    }
                    var lstPagina = [];

                    var index = 1;
                    var ate = 5;
                    
                    if (paginaAtual % 6 == 0 && paginaAtual < numeroPaginas) {
                        
                        atualizar = true;
                        index = paginaAtual;
                        ate = paginaAtual + 5;

                    }

                    if (atualizar) {
                        atualizar = false;
                        for (index; index <= ate; index++) {

                            lstPagina.push(index);
                        }
                        this.set("lstPagina" ,lstPagina);
                    }
                    
                    
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
                    //executa = false;
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
                    var paginaAtual = scope.objAppPagination.paginaAtual;
                    var numeroPaginas = scope.objAppPagination.numeroPaginas;

                    controller.montaLista(paginaAtual, numeroPaginas);

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


directiveModule.directive('appMaskDinheiro', function () {
    return {

        require: 'ngModel',
        controller: function ($scope, mask, clearMask, $parse) {

            $scope._insert = function (chave, valor) {

                var model = $parse(chave);
                model.assign($scope, valor);
            };

            $scope.mascararValor = function (chave, valor) {

                valor = mask('mask_dinheiro')(valor);
                $scope._insert(chave, valor);
            };

            $scope.limparMascara = function(value){

                value = clearMask('mask_dinheiro')(value);
                return value;
            };
            

        },

        link: function (scope, element, attr, controller) {

            if (attr.ngModel) {

                scope.$watch(attr.ngModel, function (value, old) {

                    if (value && value != old || (value && String(value).search(/(.*,.*)/g) == -1)) {               
                        
                        scope.mascararValor(attr.ngModel, value);
                    } 
                    if (attr.appMaskDinheiro) {

                        var value = scope.limparMascara(value);                        
                        var floatValue = (value) ? Number(value) : null;
                        scope._insert(attr.appMaskDinheiro, floatValue);
                    }

                });

                scope.$watch(attr.appMaskDinheiro, function (value , old) {

                    if (value != old) {

                        value = Number(value).toFixed(2);
                        scope._insert(attr.ngModel, value);
                    }
                });
            }

        }
    }
});

/*
directiveModule.directive('appMaskDinheiro', function () {
    return {

        link: function (scope, element, attr, controller) {

            angular.element(element).maskMoney({ allowNegative: false, thousands: '.', decimal: ',', affixesStay: false });
            if (attr.ngModel) {

                scope.$watch(attr.ngModel, function (value) {

                    if (value) {
                        console.info(value);
                        //scope.mascararValor(attr.ngModel, value);
                    }

                });
            }            
        }
    }
});
*/


directiveModule.directive('appDate', function () {
    return {

        controller: function ($scope, mask, clearMask, $parse) {

            $scope._insert = function (chave, valor) {

                var model = $parse(chave);
                model.assign($scope, valor);
            };
        },
        link: function (scope, element, attr, controller) {
            
            if (!attr.showDatePicker || attr.showDatePicker == true) {

                angular.element(element).datepicker({
                    showOtherMonths: true,
                    selectOtherMonths: true
                });
            }

            if (attr.ngModel) {

                scope.$watch(attr.ngModel, function (value) {

                    if (value) {
                        console.info(value);

                        var dateArray = value.split("/");
                        var dia = dateArray[0];
                        var mes = dateArray[1] -1;
                        var ano = dateArray[2];
                        var date = new Date(ano, mes, dia, 0, 0, 0, 0);
                       
                        console.info(date);
                        if (attr.appDate) {

                            scope._insert(attr.appDate, date);
                        }
                    }

                });

                scope.$watch(attr.appDate, function (value, old) {
                    
                    if (value != old) {
                       
                        if (value instanceof Date) {

                            var ano = value.getFullYear();
                            var mes = value.getMonth() + 1;

                            if (mes < 9)
                                mes = "0" + mes;
                            var dia = value.getDate();

                            if (dia < 9)
                                dia = "0" + dia;

                            var date = dia + "/" + mes + "/" + ano;

                            scope._insert(attr.ngModel, date);
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
            });
            
        },

        template: '<span ng-repeat="validationMsg in listValidationsMsg">{{validationMsg}}</span>'

    }
});

directiveModule.directive('appShowDate', function () {
    return {
        scope: {
            'appShowDate': '='
        },

        controller: function ($scope, conversionService, $parse) {

            $scope.convert = function (date) {

                return conversionService.toDate(date);
            };
        },
        link: function (scope, element, attr, controller) {
            
            scope.$parent.$watch(attr.appShowDate, function (value) {

                var date = null
                if (value) {

                    var resp = scope.convert(value);                    

                    var ano = resp.getFullYear();
                    var mes = resp.getMonth() + 1;

                    if (mes < 9)
                        mes = "0" + mes;
                    var dia = resp.getDate();

                    if (dia < 9)
                        dia = "0" + dia;

                    date = dia + "/" + mes + "/" + ano;

                }                
                scope.date = date;
            });

        },

        template: '{{date}}'

    }
});

directiveModule.directive('appShowDinheiro', function () {
    return {
        scope: {
            'appShowDinheiro': '='
        },

        controller: function ($scope, mask, clearMask, $parse) {

            $scope.mask = function (valor) {

                valor = mask('mask_dinheiro')(valor);
                return valor;
            };
        },
        link: function (scope, element, attr, controller) {

            scope.$parent.$watch(attr.appShowDinheiro, function (value) {

                var dinheiro = null;
                if (value) {

                    value = Number(value).toFixed(2);                    
                    dinheiro = "R$ " + scope.mask(value);
                }
                else{
                    dinheiro = "R$ 0,00";
                }
                scope.dinheiro = dinheiro;
            });

        },

        template: '{{dinheiro}}'

    }
});

