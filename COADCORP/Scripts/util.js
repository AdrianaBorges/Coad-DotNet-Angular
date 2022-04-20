String.prototype.replaceAll2 = function (s1, s2) {
	return this.replace(new RegExp(s1, "g"), s2);
}

function replaceAll(string, token, newtoken) {
	while (string.indexOf(token) != -1) {
		string = string.replace(token, newtoken);
	}
	return string;
}

/**
 * Objeto com várias funções úteis
 * @Author Diego Andrade da Silva (dx_diego@hotmail.com)
 */
var Util = new Object();

Util.isValid = function(value){
	
    return (value) ? true : false;
};

Util.isNotValid = function(value){
	
	return !this.isValid(value);
}


Util.isPathValid = function(obj,property, showTrack){
	
	if(this.isValid(obj) && this.isValid(property)){
		
		var currentObj = obj;
		var path = property.split(".");
		var track = new String();
		for(var key00 in path){
			
				if(track.length > 0){track += ".";}
				
				track += path[key00];
				currentObj = currentObj[path[key00]];
				if(!this.isValid(currentObj)){
					
					if(this.isValid(showTrack) && showTrack == true)
						console.info("O path " + track + " não é valido");
					return false;
				}
			
		}
		return true;
	}
	console.info("O objeto passado é inválido");
	return false;
};

Util.isInvalidChainFields = function(obj,property, showTrack){
	return !this.isValidChainFields(obj,property, showTrack);
};

function newMessage( type,  mensagem){
	var message = {type : type, message : mensagem};
	return message;
}

Util.createMessage = function(type,  mensagem){
	
	
	var message = newMessage(type, mensagem);
	
	message.childs = new Array();
	message.addSubMessage = function(message){
		
		subMessage = newMessage(null, message);
		this.childs.push(subMessage);
	};
	
	return message;
};

Util.getUrl = function (url, values) {

    var path = (contextPath && contextPath != "/") ? contextPath : "";
    var fullPath = path + url;

    if (values && values instanceof  Array) {

        for (var index in values) {

            var value = values[index];
            var regexStr = "\\{" + index + "\\}";
            var regex = new RegExp(regexStr, "g");

            fullPath = String(fullPath).replace(regex, value);
        }
    }
    return fullPath;
}

Util.getLoginUrl = function () {

    return this.getUrl("/Account/LogOn");
}

Util.alertMessage = function(message){
	
	if(Util.isValid(message) && message instanceof Object && Util.isValid(message.message)){
		
		var string = message.message;
		if(Util.isValid(message.childs)){
			for(var key in message.childs){
				if(message.childs.hasOwnProperty(key)){
					if(Util.isValidChainFields(message.childs[key],"message", true)){
						console.info(message.childs[key].message);
						string+= "\n\t\t * " + message.childs[key].message;
					}
				}
			}
		}
		alert(string);
	}
	else{
		throw "Objeto de mensagem nulo ou inválido";
	}
	
};

String.prototype.trocarTudo = function (buscar, trocar) {
    return this.replace(new RegExp(buscar, 'g'), trocar);
};

String.prototype.removerHTML = function () {
    return this.replace(/<.*?>/g, '');
}
