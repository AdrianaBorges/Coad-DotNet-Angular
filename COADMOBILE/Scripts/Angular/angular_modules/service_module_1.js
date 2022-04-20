
//-----------------------------------------------------------------------------------------------
//-------------------------------------Módulo para criação de serviços---------------------------
//-----------------------------------------------------------------------------------------------
var serviceModule = angular.module('serviceModule', []);

// Serviço para as conversões dos objetos json do sistema
serviceModule.factory("conversionService", function () { 

    var conversionService = {

        _datePatternSearch: /(\/Date\((.*)\)\/)/g, // regex para substituição do padrão '/Date(_time_stamp)/' que vem dos objetos com campo datetime da aplicação para transformar em um timestamp puro
        _replacementForDateFormat: "$2", // string de substituição onde a o padrão acima será substituido apenas pelo timestamp

        /**
        * Função que converte os campos datetime que vem como string de padrão citado acima para um campo de date válido
        * param 1 obj => objeto que contem os campos
        * param 2 dataFieldsName => array com o nome dos campos que devem ser convertidos
        */
        convertDateField: function (obj, dateFieldsName) { 

            if (obj && dateFieldsName) {

                for (var fieldIndex in dateFieldsName) {

                    var oldDateFormat = obj[dateFieldsName[fieldIndex]];

                    if (oldDateFormat) {
                        var rightDate = this.toDate(oldDateFormat);
                        obj[dateFieldsName[fieldIndex]] = rightDate;
                    }
                }
            }
            return obj;
        },

        convert : function(date){

            if (date) {
                var rightDate = date.replace(this._datePatternSearch, this._replacementForDateFormat);
                return rightDate;
            }
        },

        toDate: function (date) {

            var value = this.convert(date);

            if(value)
                return new Date(parseInt(value));
            else
                return null;
        }


    };

    return conversionService;
});

// Serviço para facilitar requisições de form via ajax
serviceModule.factory('formHandlerService', function ($http, $parse, conversionService) {
	
    var formHandlerService = {
			
            handlerPost : function(url,data, onsuccess, onfail, showLoader){
				
                if (showAjaxLoader === true)
                    showAjaxLoader();
				$http({
					url : url,
					method : 'post',
					data: JSON.stringify(data),
					headers: { 'Content-Type': 'application/json' }
				}).success(function(result, status, config){
					
				    if (showLoader === true)
				        hideAjaxLoader();

				    if (onsuccess) {
				        if (result) {
                            			           
				            var message = result.message;
				            var validationMessage = result.validationMessage;
				            onsuccess(result, status, config, message, validationMessage);   
				        }
				        else {
				            onsuccess(result, status, config);
				        }
						
					}
				}).error(function(data ,status, erro, config){
				    if (showLoader === true)
				        hideAjaxLoader();

				    if (onfail) {
				            if (data) {

				                var message = data.message;
				                var validationMessage = result.validationMessage;
				                onfail(data, status, message, validationMessage);
				            }
							onfail(data, status);							
						}
						else if(status = '404'){
							
							alert("Não foi possível realizar a requisição. O página solicitada não foi encontrada");
						}
				});
			},
			
			submit : function($scope, params){				
									
					if(params && params.objectName){
						
						var objectData = $scope[params.objectName];
						
						if (objectData) {
						    console.info(objectData);
							
						    var postData = angular.copy(objectData);
						    console.info(postData);
							formHandlerService.handlerPost(params.url, postData, params.success, params.fail, params.showAjaxLoader);
						}
					}
				
			},
			
			read : function($scope, params){
				
				if(params &&  params.url && params.targetObjectName){
					
				    var objectName = params.targetObjectName; // nome do objeto json que será montado no $scope

				    // nome do objeto model que está sendo retornado no resultado
                    // se o responseModelName não for definido será usado o mesmo nome do objectName
				    var modelName = (params.responseModelName) ? params.responseModelName : null;
                    var data = params.data 
                    var dateFieldsNames = params.dateFieldsNames;
                    var page = params.pageConfig;

                   var config = {
                        url : params.url,
                        method : 'post',
                   }
                   if (data) {
                       config.data = data;
                   }
					$http(config).success(function(result, status, config){						
					        
					        if (typeof result['success'] == 'undefined' ||  (result['success'] && result['success'] === true)) {

					            var model = $parse(objectName);
					            var modelObj = (modelName) ? result.result[modelName] : result;

					            if (dateFieldsNames) {
					                modelObj = conversionService.convertDateField(modelObj, dateFieldsNames);
					            }

					            console.info("Objeto carregado");
					            model.assign($scope, modelObj);
					            console.info(modelObj);

                                // --------------------- paginação ----------------
					            if (page && page instanceof Object) {

					                var pageName = (page.pageName) ? page.pageName : "page"; // determina o nome onde está a página no resultado
					                var pageObj = result[pageName]; // pega o objeto de página no resultado

					                if (pageObj) {
					                    
					                    var objName = (page.pageTargetName) ? page.pageTargetName : pageName; // determina qual será o nome do objeto no scopo
					                    var modelPage = $parse(objName);
					                    modelPage.assign($scope, pageObj);
					                }
					            }
                                //---------------------------------------------------
					            if (params.success && typeof params.success == "function") {

                                    params.success(result, status, config)
					            }

					            if (result['message']) {
					                $scope.message = result['message'];
					            }
					        }
					        else {
					            if (result['message']) {
					                $scope.message = result['message'];
					            }
					        }
					   

					}).error(function(data ,status, erro, config){
							
					    $scope.message = Util.createMessage("fail", "Ocorreu um erro. status = [" + status + "] ");
					});
				}
			},
            
			
	};
	
	return formHandlerService;
});


serviceModule.factory('messageService', function () {

    var messageService = {

        watchForAlert: function ($scope, attrName) {
            
            if (attrName) {

                attrName = "message";
            }
            $scope.$watch(attrName, function (newValue, oldValue) {
                if (newValue) {
                    Util.alertMessage(newValue);
                }
            });
        },

        fail: function (msg) {

            return Util.createMessage("fail", msg);
        },

        success: function (msg) {

            return Util.createMessage("success", msg);
        },

        warning: function (msg) {

            return Util.createMessage("warning", msg);
        }
    };

    return messageService;
});

serviceModule.factory("mask", function (maskDinheiro) {

    return function (name) {

        switch (name) {

            case 'mask_dinheiro': {

                return maskDinheiro;
            }
            default: { return function () { };}
        }
    }
});

serviceModule.factory("clearMask", function (clearMaskDinheiro) {

    return function (name) {

        switch (name) {

            case 'mask_dinheiro': {

                return clearMaskDinheiro;
            }
            default: { return function () { }; }
        }
    }
});


serviceModule.factory("maskDinheiro", function (clearMask) {
    
    return function (value) {
       
        if (value) {

            value = String(value);
            
            value = clearMask('mask_dinheiro')(value);
            value = value.replace(/\D/g, "");
            value = value.replace(/(\d)(\d{8})$/, "$1.$2");//coloca o ponto dos milhões
            value = value.replace(/(\d)(\d{5})$/, "$1.$2");//coloca o ponto dos milhares
            value = value.replace(/(\d)(\d{2})$/, "$1,$2");//coloca a virgula antes dos 2 últimos dígitos
            element = value;
        }
      
        return value;
   
    }
});


serviceModule.factory("clearMaskDinheiro", function () {

    return function (value) {
             
        if (value) {

            value = String(value);

            var regex = /\D/g;
            value = String(value).replace(regex, ""); //mantém apenas números da string
            value = value.replace(/^([0]+)/g, ""); //remove zeros a esquerda
            value = String((value / 100).toFixed(2)); // divide o valor por 100 para que valores abaixo de 1 real seram preenchidos por 0,00 ex: 9 na verdade é 0,09

        }
       return value;

    }
});

serviceModule.filter("telFilter", function () {

    return function (input) {


    }
});