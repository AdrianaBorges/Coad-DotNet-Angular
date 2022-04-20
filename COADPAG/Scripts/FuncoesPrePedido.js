function Mascara(element) {
    var value = element.value;
    value = value.replace(/\D/g, "");
    element.value = value;
}

function MascaraDataVigencia(element) {
    var value = element.value;
    value = value.replace(/\D/g, "")                    
    value = value.replace(/(\d{2})(\d)/, "$1/$2")       
    value = value.replace(/(\d{4})(\d)/)                                               
    
    element.value = value;
}

function MascaraValor(element) {
    var value = element.value;
    value = value.replace(/\D/g, "")
    value = value.replace(/(\d)(\d{8})$/, "$1.$2");//coloca o ponto dos milhões
    value = value.replace(/(\d)(\d{5})$/, "$1.$2");//coloca o ponto dos milhares
 
    value = value.replace(/(\d)(\d{2})$/, "$1,$2");//coloca a virgula antes dos 2 últimos dígitos

    element.value = value;
}