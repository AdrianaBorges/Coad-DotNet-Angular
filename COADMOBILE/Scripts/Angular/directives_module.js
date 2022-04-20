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
                });                
                
            });
        },
        template: '<div ng-show="message" class="alert alert-dismissible"' +
            ' ng-class="{\'alert-warning\' : message.type == \'warning\', \'alert-success\' : message.type == \'success\', \'alert-info\' : message.type == \'info\', \'alert-danger\' : message.type == \'danger\', \'alert-danger\' : message.type == \'fail\'}"' +
                  'role="alert"><button type="button" class="close" ng-click="fechar()"><span aria-hidden="true">&times;</span></button>' +
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
        scope : true,
        controller : function($scope, $parse){

            var atualizar = true;
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
        },
        link: function (scope, element, attr, controller) {

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

                            scope.$parent.$eval(attr.loadFunction);
                        }
                    }
                    
                });
                
            };

           
            
        },
        template: '<nav id="pagination">' +
                        '<ul class="pagination">' + 
                        '<li>' +
                            '<a href="javascript:void(0)" aria-label="Previous" ng-click="anterior()" ng-class="{disabled : !testAnterior}">' +
                            '<span aria-hidden="true">&laquo;</span>' + 
                            '</a>' +
                        '</li>' +
                        '<li ng-repeat="pag in objAppPagination.lstPagina" ng-class="{disabled : pag == objAppPagination.paginaAtual || pag < 0 }">' 
                        + '<a ng-if="pag > 0" href="javascript:void(0)" ng-click="gotoPage(pag)">{{pag}}</a>'
                        + '<a ng-if="pag < 0" href="javascript:void(0)">...</a></li>' +
                         '<li>' +
                            '<a href="javascript:void(0)" aria-label="Next" ng-click="proximo()" ng-class="{disabled : testProximo}">' +
                            '<span aria-hidden="true">&raquo;</span>' +
                            '</a>' + 
                        '</li>' +
                        '</ul>' +
                   '</nav>'
    };
});


//directiveModule.directive('appMaskDinheiro', function () {
//    return {

//        require: 'ngModel',
//        controller: function ($scope, mask, clearMask, $parse) {

//            $scope._insert = function (chave, valor) {

//                var model = $parse(chave);
//                model.assign($scope, valor);
//            };

//            $scope.mascararValor = function (chave, valor) {

//                valor = mask('mask_dinheiro')(valor);
//                $scope._insert(chave, valor);
//            };

//            $scope.limparMascara = function(value){

//                value = clearMask('mask_dinheiro')(value);
//                return value;
//            };
            

//        },

//        link: function (scope, element, attr, controller) {

//            if (attr.ngModel) {

//                scope.$watch(attr.ngModel, function (value, old) {

//                    if (value && value != old || (value && String(value).search(/(.*,.*)/g) == -1)) {               
                        
//                        scope.mascararValor(attr.ngModel, value);
//                    } 
//                    if (attr.appMaskDinheiro) {

//                        var value = scope.limparMascara(value);                        
//                        var floatValue = (value) ? Number(value) : null;
//                        scope._insert(attr.appMaskDinheiro, floatValue);
//                    }

//                });
                    
//                scope.$watch(attr.appMaskDinheiro, function (value , old) {

//                    if (value != old) {

//                        value = Number(value).toFixed(2);
//                        scope._insert(attr.ngModel, value);
//                    }
//                });
//            }

//        }
//    }
//});


directiveModule.directive('appMaskDinheiro', function ($parse) {
    return {

        scope: {
            'appMaskDinheiro': '=',
            'ngModel' : '='
        },
        controller: function ($scope, mask, clearMask, $parse) {
                        
            return {
             
                mascararValor : function (valor) {

                    valor = mask('mask_dinheiro')(valor);
                    $scope.ngModel = valor;
                },
                mask : function(valor){
                    valor = mask('mask_dinheiro')(valor);
                    return valor;
                },
                limparMascara : function (value) {

                    value = clearMask('mask_dinheiro')(value);
                    return value;
                }
          
          };
        },

        link: function (scope, element, attr, ctrl) {

            if (attr.ngModel) {

                scope.$parent.$watch(attr.ngModel, function (value, old) {

                   if (value || old) { // se não existir valor novo e nem anterior não faz nada
                                              
                            if (value && value != old || (value && String(value).search(/(.*,.*)/g) == -1)) {

                                ctrl.mascararValor(value);
                            }
                            if (attr.appMaskDinheiro) {

                                var value = ctrl.limparMascara(value);
                                var floatValue = (value) ? Number(value) : null;
                                scope.appMaskDinheiro = floatValue;
                            }
                        
                    } 
                });

                scope.$parent.$watch(attr.appMaskDinheiro, function (value, old) {
                                      
                    if ((value || old )) {

                        if ((value != old && scope.ngModel != value) || (!scope.ngModel && value)) {

                           value = Number(value).toFixed(2);
                           scope.ngModel = value;
                        }
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

        controller: function ($scope, mask, clearMask, $parse, conversionService) {

            $scope._insert = function (chave, valor) {

                var model = $parse(chave);
                model.assign($scope, valor);
            };

            $scope._convertToDate = function(string){

                return conversionService.toDate(string);
            }

            $scope._needConvert = function(value){

                if (!value || value instanceof Date)
                    return;
                return (String(value).match(conversionService._datePatternSearch) != -1) ? true : false;
                
            }   
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
                    
                    if ((scope._needConvert(value)) || (value != old)) {
                       
                        if (scope._needConvert(value)) {

                            value = scope._convertToDate(value);
                        }

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
            'appShowDate': '='
        },

        controller: function ($scope, conversionService, $parse) {

            $scope.convert = function (date) {
                if (date instanceof Date)
                    return date;
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

directiveModule.directive('appShowDinheiro', function ($parse) {
    return {
        scope: {
            'appShowDinheiro': '=',
            'hideWhenZero': '@',
            'outputModel' : '@'
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
                    var maskedValue = scope.mask(value);

                    if (scope.outputModel) {

                        var model = $parse(scope.outputModel);
                        model.assign(scope.$parent, maskedValue);
                    }

                    dinheiro = "R$ " + maskedValue;
                }
                else if (!scope.hideWhenZero) {
                    dinheiro = "R$ 0,00";
                }
                scope.dinheiro = dinheiro;
            });

        },

        template: '{{dinheiro}}'

    }
});


directiveModule.directive('appCep', function () {
    return {

        scope: {
            'ngModel': '='
        },
        require : 'ngModel',
        controller: function ($scope, scopeBindService, cepService) {

            scopeBindService.bindInsertFunction($scope);

            $scope.loadCep = function (cep, callback, erroCallback) {

                cepService.cep(cep, callback, erroCallback);
            }
        },
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
        controller: function ($scope, scopeBindService) {

            scopeBindService.bindInsertFunction($scope);

        },
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


directiveModule.directive('appBindRichTextEvent', function ($parse) {

    return {

        scope: false,
        controller: function ($scope, scopeBindService) {

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

        },
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
});

directiveModule.directive('appRichEditor', function ($parse) {
    return {

        scope: false,
        required : 'ngModel',
        controller: function ($scope, scopeBindService, cepService) {

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
        },
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
});


directiveModule.directive('appModelBind', function ($parse) {
    return {

        scope: false,
        restrict : 'EA',
        controller: function ($scope, scopeBindService, cepService) {

            return {

                bind: function (sourceName, destinyName) {

                    $scope.$watch(sourceName, function (value, old) {

                        if (value != old) {

                            var model = $parse(destinyName);
                            model.assign($scope, value);
                        }
                    });

                    $scope.$watch(destinyName, function (value, old) {

                        if (value != old) {

                            var model = $parse(sourceName);
                            model.assign($scope, value);
                        }
                    });
                }
            };
        },
        link: function (scope, element, attr, controller) {

            if (attr.sourceModel && attr.targetModel) {

                controller.bind(attr.sourceModel, attr.targetModel)
            }
        }    
    }
});

directiveModule.directive('appEditableTd', function ($parse) {

    return {
        restrict: 'AE',
        transclude: true,
        scope: {
            appEditableTd: '=',
            label: '=',
            blurEvent: '@',
            isMoney : '@'

        },
        controller: function ($scope) {

                        
                $scope.ExcluirTelefone = function (index) {

                    if ($scope.PEDIDO_TELEFONE && (index | index == 0)) {

                        if (confirm("Confirmar exclusão")) {
                            $scope.PEDIDO_TELEFONE.splice(index, 1);
                        }

                    }
                    else {

                        $scope.message = Util.createMessage('fail', 'Ocorreu algum erro ao tentar retirar o telefone da lista');
                    }
                }


                $scope._desabilitarDemaisTelefones = function (element) { // desabilitar outras edições abertas excluindo o telefone passado

                    if ($scope.$parent.editableTdControlList) {

                        angular.forEach($scope.$parent.editableTdControlList, function (value, key) {

                            if (element != key) {

                                value = false;
                            }
                        });


                    }
                }

               $scope.confirmarEdicao = function ($event) {

                    $event.stopPropagation();
                    $scope.editarItem = false;


                };
                $scope.triggerBlur = function ($event) {

                    $scope.$parent.erros = [];
                    $event.stopPropagation();
                    if ( $scope.editarItem) {
                        $scope.editarItem = false;
                    }
                };

                if ($scope.blurEvent) {

                    var model = $parse($scope.blurEvent);
                    model.assign($scope.$parent, function ($event) {

                        $scope.triggerBlur($event);
                    });
            
                }

                if (!$scope.$parent.editableTdControlList) {

                    $scope.$parent.editableTdControlList = []; // lista criada no scopo principal para controlar todos dos eventos de edição de modo global
                }
           
            },
        link: function (scope, element, attr, controller) {

           
            var selectElement = element.find(":input")[0];

            
            if (selectElement) {

                angular.element(selectElement).bind('blur', function (e) {

                    scope.triggerBlur(e);
                });
            }

            scope.$watch("editarItem", function (value) {

                if (value === true) {

                    if (selectElement)
                        angular.element(selectElement).focus();
                }
            });

            scope.ativarEditarItem = function () {
               scope.editarItem = true;
                
            }

            //element.bind('click', function () {

            //    scope.ativarEditarItem(element);
            //});

            
            

        },
        template: '<div ng-click="ativarEditarItem()">' +
                            '<span ng-show="editarItem || !appEditableTd">' +
                                '<span ng-transclude ng-blur="triggerBlur()" class="span-transclude"></span>' +
                            '</span>' +
                        '<span ng-if="isMoney" ng-hide="editarItem && appEditableTd" app-show-dinheiro="label" hide-when-zero="true"></span>' +
                        '<span ng-if="!isMoney" ng-hide="editarItem && appEditableTd">{{label}}</span>' +
                        '<div app-validation-msg="$parent.erros" for="PEDIDO_TELEFONE[{{$index}}].OPC_ID"></div>' +
                    '</div>'
    };
});


directiveModule.directive("appRadio", function ($parse) {

    return {

        restrict: 'EA',
        scope: {

            ngModel: '@',
            list: '=',
            appRadio: '@',
            listModel : '@',
        },
        link: function (scope, element, attr, controller) {
               
             //
            scope.$watch(scope.ngModel, function (value, old) {

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

                    if (value === true) {

                        element.attr('checked', true); 
                    }
                });


            });
           
        }
    };
});

directiveModule.directive("appNumber", function ($parse) {

    return {

        require : '^ngModel',
        restrict: 'EA',        
        scope: false
        ,
        link: function (scope, element, attr, controller) {

            scope.$watch(attr.ngModel, function (value, old) {

                if (value && value != old) {
                                     
                    var valueString = String(value);
                    valueString = valueString.replace(/\D/g, "");
                    value = Number(valueString);

                    var model = $parse(attr.ngModel);
                    model.assign(scope, value);
                }
            });
        }
    };
});

