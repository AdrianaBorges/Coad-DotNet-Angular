//@Autor Diego Andrade da Silva

$(document).ready(function () {

    // funções de desacoplamento de chamada de script
    $("button[data-post], a[data-post]").click(function (e) {
        
        e.preventDefault();
        post($(this).data("post"));
    })

    $.datepicker.regional['pt-BR'] = {
        closeText: 'Fechar',
        prevText: '&#x3c;Anterior',
        nextText: 'Pr&oacute;ximo&#x3e;',
        currentText: 'Hoje',
        monthNames: ['Janeiro', 'Fevereiro', 'Mar&ccedil;o', 'Abril', 'Maio', 'Junho',
        'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
        monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun',
        'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
        dayNames: ['Domingo', 'Segunda-feira', 'Ter&ccedil;a-feira', 'Quarta-feira', 'Quinta-feira', 'Sexta-feira', 'Sabado'],
        dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sab'],
        dayNamesMin: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sab'],
        weekHeader: 'Sm',
        dateFormat: 'dd/mm/yy'
    };

    $.datepicker.setDefaults($.datepicker.regional["pt-BR"]);

    $(document).ready(function () {
        $(".datepicker").datepicker({
            showOtherMonths: true,
            selectOtherMonths: true
        });

        $(".table-responsiv").customScrollbar();
    });
});

//função que serve para enviar requisições post
//usando um formulário fantasma. Indicado para links
function post(url, newTab) {

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

    if (newTab) { // verifico se é para abrir o resultado em outra aba

        form.target = '_blank'; // se for indico para o formulário que é necessário abrir em uma nova aba
    }

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

function OnlyNumber(element) {
    //console.info(element.value);
    var value = element.value;
    value = value.replace(/\D/g, "");
    element.value = value;
}

var BigDecimal = {
       
    toFixed : function toFixed(num, fixed) {
        fixed = fixed || 2;
        fixed = Math.pow(10, fixed);

        var resp = Math.floor(num * fixed) / fixed;
        resp = resp.toFixed(2);
        return resp;
    }
}

function toFixed(num, fixed) {
    fixed = fixed || 0;
    fixed = Math.pow(10, fixed);
    return Math.floor(num * fixed) / fixed;
}

function showAjaxLoader() {
    var img = document.createElement("img");
    img.src = "/Content/themes/base/images/ajax-loader.gif";
    img.className = "ajax-loader";
    img.id = "ajaxLoader";

    var value = $(".teste").data("tipoTelefone");
    $("body").append(img);
}

function hideAjaxLoader() {

    $("#ajaxLoader").remove();
}


var appModule = angular.module("appModule", ["serviceModule","directiveModule"]);