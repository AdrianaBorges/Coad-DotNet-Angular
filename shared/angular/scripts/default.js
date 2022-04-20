//@Autor Diego Andrade da Silva

$(document).ready(function () {

    // funções de desacoplamento de chamada de script
    $("button[data-post], a[data-post]").click(function (e) {
        
        e.preventDefault();
        post($(this).data("post"));
    })

    // funções de desacoplamento de chamada de script
    $("button[data-confirm], a[data-confirm]").click(function (e) {
        e.preventDefault();
        var msg  = ($(this).data("msg")) ? $(this).data("msg") : "Confirma?";
        if (confirm(msg)) {
            post($(this).data("confirm"));
        }
        
    })
});

//função que serve para enviar requisições post
//usando um formulário fantasma. Indicado para links
function post(url) {

    //crio um elemento de formulário para o envio
    var form = document.createElement("form");

    // pego a url e separo a url dos parâmetros
    var splitUrl = url.split("?");
    // pego o primeiro elemento da divisão ou seja o endereço e atribuo
    // ao action do formulário
    form.action = splitUrl[0];
    // defino o método de envio como post
    form.method = "post";
    // atribuo um id para o formulário
    form.id = "formPost";

    if (splitUrl[1]) {
        // divido os parâmetros cada vez que for achado o '&'
        var parans = splitUrl[1].split("&");

        // percorro os parâmetros para atribuir a requisição
        for (var i = 0; i < parans.length; i++) {
            // divido os parâmetros, os que estão antes do '=' é o nome do parâmetro
            // o que está depois do '=' é o valor do parâmetro
            var valores = parans[i].split("=");

            // crio o input que irá carregar o valor do parâmetro
            var input = document.createElement("input");
            // como o campo não precisará ser visto, o torno invisível
            input.type = "hidden";


            // se o nome do parâmetro estiver disponível atribuo ao name do input
            if (valores[0])
                input.name = valores[0];

            // se o valor do parâmetro estiver disponível atribuo ao value do input
            if (valores[1])
                input.value = valores[1];

            // coloco a input no formulário
            form.appendChild(input);
        }
    }
    // pego o elemento body
    var body = document.getElementsByTagName("body");
    // atribuo o formulário a ele
    body[0].appendChild(form);

    // executo a submissão do formulário (envio)
    form.submit();
};
// adiciona a função de indexOf para browsers antigos como o ie8 para baixo
if (!Array.prototype.indexOf)
{
  Array.prototype.indexOf = function(elt /*, from*/)
  {
    var len = this.length >>> 0;

    var from = Number(arguments[1]) || 0;
    from = (from < 0)
         ? Math.ceil(from)
         : Math.floor(from);
    if (from < 0)
      from += len;

    for (; from < len; from++)
    {
      if (from in this &&
          this[from] === elt)
        return from;
    }
    return -1;
  };
}

function showAjaxLoader()
{
    var img = document.createElement("img");
    img.src = "/Content/themes/base/images/ajax-loader.gif";
    img.className = "ajax-loader";
    img.id = "ajaxLoader";

    var value = $(".teste").data("tipoTelefone");
    $("body").append(img);
}

function hideAjaxLoader(){

    $("#ajaxLoader").remove();
}

function ajaxLoader(toExecute){

    if (toExecute) {

        showAjaxLoader();
        toExecute();
        hideAjaxLoader();
    }
}
//--------------------------------------------------------------------------------------------------
// JQUERY CUSTOMIZATIONS SECTION
//--------------------------------------------------------------------------------------------------
var dados = [];
function split(val) {

    if(Util.isValid(val))
        return val.split(/,\s*/);
    return new Array();
};

function extractLast(term) {

    if(Util.isValid(term))
        return split(term).pop();
    return "";
};

function clearStringList(string) {

    //	console.info(string.replace(/([\[]|[\]]|[(|)]|[{]|[}])/g,''));
    return string.replace(/([\[]|[\]]|[(|)]|[{]|[}])/g, '');
}

// CUSTOM AUTOCOMPLETE SUBSECTION 
//-------------------------------------------------------------------------------------------------
(function ($) {

    $.fn.customAutocomplete = function (config) {


        // inicializando e setando as configurações --------------------------------------
        var minLength = 3;
        var callback = null;

        if (config != undefined && config != null) {

            $.extend(config);

            if (config.returnType != null) {
                returnType = config.returnType;
            }
            minLength = (config.minLength != undefined) ? config.minLength : 3;
            callback = (config.success != undefined) ? config.success : null;
            onSelectCallback = (Util.isValid(config.onSelect)) ? config.onSelect : null;

            // resolvendo o indexador do elemento---------------------------------------------
            var indexador = "semIdEClasse";
            if ($(this).attr("id") != undefined && $(this).attr("id") != null) {

                indexador = $(this).attr("id");
            } else if ($(this).attr('class') != undefined && $(this).attr('class') != null) {
                indexador = $(this).attr('class');
            }
            //--------------------------------------------------------------------------------

            // Obtendo o valor referente ao elemento passado, possibilitando a função 
            //ser usada por multiplos elementos na mesma página //----------------------

            if (dados[indexador] == null || dados[indexador] == undefined) {

                dados[indexador] = [];
            }

            if (dados[indexador].elementos == undefined || dados[indexador].elementos == null) {
                dados[indexador].elementos = [];
            }

            // se os elementos não forem diretamente atribuidos ao passar o atributo url é possível pegar essa
            // lista por essa função
            if (config.elementos != undefined)
                dados[indexador].elementos = config.elementos;
            else if (config.url != undefined && dados[indexador].elementos != undefined && dados[indexador].elementos.length < 1) {

                dados[indexador].elementos = get(config.url);
            }
            //-------------------------------------------------------------------------------

            var indiceValores = dados[indexador];
            if (indiceValores.indice == undefined || indiceValores.indice == null) {

                indiceValores.indice = new Array();
            }

            if (indiceValores.labels == undefined) {
                indiceValores.labels = null;
            }

            //-----------------------------------------------------------------------------------
            $(this).autocomplete({
                minLength: minLength,
                source: function (request, response) {

                    response($.ui.autocomplete.filter(indiceValores.elementos, extractLast(request.term)));
                },
                focus: function (event, ui) {
                    return false;
                },
                select: function (event, ui) {
                    if (this.value != null && this.value != undefined) {
                        var termos = split(this.value);
                        termos.pop();
                        termos.push(ui.item.label);
                        termos.push("");
                        this.value = termos.join(", ");
                    } else {
                        this.value = ui.item.label + ", ";
                    }

                    if (ui.item.label != null && ui.item.label != 0) {
                        // atribuição ao indice
                        indiceValores.indice[ui.item.label] = ui.item.value;
                    }

                    // executa a função de callback
                    if (callback != null) {

                        var indices = new Array();

                        for (var key6 in indiceValores.indice) {
                            //debug(indiceValores.indice);
                            if (indiceValores.indice.hasOwnProperty(key6)) {
                                indices.push(indiceValores.indice[key6]);
                            }

                        }
                        if (indices.length > 0)
                            callback(indices, indiceValores);
                        else
                            callback(null, indiceValores);

                    }

                    if (onSelectCallback != null) {

                        onSelectCallback();
                    }

                    return false;
                },
                change: function (event, ui) {

                    //						if ( ui.item === null || ( valor == null )
                    //								|| ( valor.trim().length == 0 ) )
                    //						{
                    //							$scope.filtro.escola.descricao = null;
                    //							$scope.filtro.escola.id = null;
                    //						}
                    //					
                    return false;
                }
            });

            // procura os valores das labels que não existem na array para remover as 
            //ids sem referencia
            var indices = indiceValores.indice;
            indiceValores.indice = new Array();

            var labels = split($(this).val());
            for (var i = 0; i < labels.length; i++) {

                for (var key in indices) {

                    if (indices.hasOwnProperty(key) && key === labels[i])
                        indiceValores.indice[key] = indices[key];
                }

            }

            // executa a função de callback

            var indices = new Array();

            for (var key7 in indiceValores.indice) {
                //debug(indiceValores.indice);
                if (indiceValores.indice.hasOwnProperty(key7)) {
                    indices.push(indiceValores.indice[key7]);
                }

            }
            if (indices.length > 0)
                callback(indices, indiceValores);
            else
                callback(null, indiceValores);

        }
    };  

    // GET SUBSECTION
    //---------------------------------------------------------------------------------------------------

    // Executa um ajax syncrono
    function get(url) {

        var array = [];
        $.ajax({

            url: url,
            async: false,

            success: function (result) {

                array = result.result;
            },
            error: function (xrs, erroNum, erroMsg) {

                alert("Não foi possível obter a lista para o filtro selecionado erro número: " + erroNum + " erro : " + erroMsg);
            }
        });
        return array;

    }

    // SIMPLEAUTOCOMPLETE SUBSECTION
    //--------------------------------------------------------------------------------------------------------


    $.fn.simpleAutocomplete = function (config) {

        if (config) { $.extend(config); }

        var lista = new Array();
        if (!Util.isValid(config.lista) && Util.isValid(config.url)) {

            var url = config.url;

            lista = get(url);
        }
        else if (Util.isValid(config.lista)) {

            lista = config.lista;
        }

        $(this).autocomplete({

            source: lista,
            focus: function (event, ui) {

                $(this).val(ui.item.label);
                return false;
            },
            select: function (event, ui) {
                $(this).val(ui.item.label);

                if (config.select) {

                    var value = ui.item.value;
                    var label = ui.item.label;
                    config.select(value, label);
                }

                return false;
            },
            change: function (event, ui) {

                return false;
            }
        });
    };
})(jQuery);
var appModule = angular.module("appModule", ['serviceModule','directiveModule']);