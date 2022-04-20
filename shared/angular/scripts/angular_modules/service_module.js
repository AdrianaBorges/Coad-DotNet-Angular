
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
                        var rightDate = oldDateFormat.replace(this._datePatternSearch, this._replacementForDateFormat);
                        obj[dateFieldsName[fieldIndex]] = rightDate;
                    }
                }
            }
            return obj;
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
				        if (result && result.message) {

				            var message = result.message;
				            onsuccess(result, status, config, message);
				        }
				        else {
				            onsuccess(result, status, config);
				        }
						
					}
				}).error(function(data ,status, erro, config){
				    if (showLoader === true)
				        hideAjaxLoader();

				    if (onfail) {
				            if (data && data.message) {

				                var message = data.message;
				                onfail(data, status, message);
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

					            model.assign($scope, modelObj);

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


