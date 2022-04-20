var fadnValor;
var fdsrADN;
var fHEValor;
var fDsrHE;
var fHENoturnaValor;
var fDsrHENoturna;
var fSubtotal;
var fValFGTS;
var fValINSS;
var fValTransTotal;
var fAuxCreche;
var fTotal;

function formatReal(int) {
    var tmp = int + '';
    tmp = tmp.replace(/([0-9]{2})$/g, ",$1");
    if (tmp.length > 6)
        tmp = tmp.replace(/([0-9]{3}),([0-9]{2}$)/g, ".$1,$2");

    return tmp;
}

function funcao(e) {
    var tecla = (window.event) ? e.keyCode : e.which;
    if (tecla == 8 || tecla == 0)
        return true;
    if (tecla != 44 && tecla < 48 || tecla > 57)
        return false;
}

function limpaNumero(numero) {
    numero = numero.replace(".", "");
    numero = numero.replace(",", ".");
    return parseFloat(numero);
}

function atualizaValor() {
    var vSalario = $("#valorSalario").val() != "" ? limpaNumero($("#valorSalario").val()) : 100;
    if (vSalario == 100 || vSalario == 0) {
        $("#valorSalario").val("100,00");
    }
}

function mudaCampohide() {
    if ($("#escolhaFi").val() == 0) {
        $("#qteFilhos").val("0");
        $("#qteFilhosOption").hide();
    }
    else {
        var qf = $("#qteFilhos").val() != "" ? $("#qteFilhos").val() : 1;
        if ($("#qteFilhos").val() == "") $("#qteFilhos").val("1");
        if (qf == 0 || qf > 15) {
            $("#qteFilhos").val("1");
        }
        $("#qteFilhosOption").show();
    }
}

function atualizarHorasSalario(e) {
    var hSalario = $("#horasSalario").val() != "" ? limpaNumero($("#horasSalario").val()) : 10;
    if (hSalario <= 10 || hSalario == 0) {
        $("#horasSalario").val("10");
    }
}

function atualizaADN() {
    var valorSalario = limpaNumero($("#valorSalario").val());
    var horasSalario = limpaNumero($("#horasSalario").val());
    var horasAdicional = $("#horasAdicional").val() != "" ? limpaNumero($("#horasAdicional").val()) : 0;
    var valorAdn = (valorSalario / horasSalario) * 0.2 * horasAdicional;
    if (horasAdicional == 0) {
        $("#horasAdicional").val("0");
    }
    fadnValor = Number(valorAdn);
    $("#contentADN").html(Number((fadnValor).toFixed(2)));
}

function atualizaDADN() {
    var valorADN = $("#contentADN").html();
    fdsrADN = Number((valorADN / 6));
    $("#contentDADN").html(Number((fadnValor / 6).toFixed(2)));
}

function atualizaHE50() {
    var qtdHoras = $("#horasCinquenta").val() != "" ? limpaNumero($("#horasCinquenta").val()) : 0;
    var valorSalario = limpaNumero($("#valorSalario").val());
    var horasSalario = limpaNumero($("#horasSalario").val());
    var valorADN = parseFloat($("#contentADN").html());
    var valor = (valorSalario + valorADN) / horasSalario;
    valor = valor * 1.5 * qtdHoras;
    if (qtdHoras == 0) {
        $("#horasCinquenta").val("0");
    }
    fHEValor = valor;
    $("#contentHora50").html(Number((fHEValor).toFixed(2)));
}

function atualizaDSRSHE() {
    var valorHE50 = $("#contentHora50").html();
    fDsrHE = Number((valorHE50 / 6));
    $("#contentDSRSHE").html(Number((fHEValor / 6).toFixed(2)));
}

function atualizaHEN() {
    var horasNoturnas = $("#horasNoturnas").val() != "" ? limpaNumero($("#horasNoturnas").val()) : 0;
    var valorSalario = limpaNumero($("#valorSalario").val());
    var horasSalario = limpaNumero($("#horasSalario").val());
    var valorGeral = valorSalario / horasSalario;
    valorGeral = valorGeral * 1.2 * 1.5 * horasNoturnas;
    if (horasNoturnas == 0) {
        $("#horasNoturnas").val("0");
    }
    fHENoturnaValor = Number(valorGeral);
    $("#contentHEN").html(Number((fHENoturnaValor).toFixed(2)));
}

function atualizaDSHEN() {
    var valorHEN = $("#contentHEN").html();
    fDsrHENoturna = Number((valorHEN / 6));
    $("#contentHENSN").html(Number((fHENoturnaValor / 6).toFixed(2)));
}

function atualizaSubTotal() {
    var valorSalario = limpaNumero($("#valorSalario").val());
    var valorADN = parseFloat($("#contentADN").html());
    var valorDADN = parseFloat($("#contentDADN").html());
    var valorHora50 = parseFloat($("#contentHora50").html());
    var valorDSRHE = parseFloat($("#contentDSRSHE").html());
    var valorHEN = parseFloat($("#contentHEN").html());
    var valorHENSN = parseFloat($("#contentHENSN").html());
    var valorSubTotal = (valorSalario + fadnValor + fdsrADN + fHEValor + fDsrHE + fHENoturnaValor + fDsrHENoturna);
    fSubtotal = valorSubTotal;
    $("#contentSubTotal").html(Number((valorSubTotal).toFixed(2)));
}

function atualizaFGTS() {
    var valorSubTotal = parseFloat($("#contentSubTotal").html());
    var valorFGTS = limpaNumero($("#baseFGTS").html());
    fValFGTS = Number((valorSubTotal * (valorFGTS / 100)));
    $("#contentFGTS").html(Number((valorSubTotal * (valorFGTS / 100)).toFixed(2)));
}

function atualizaINSS() {
    var valorSubTotal = parseFloat($("#contentSubTotal").html());
    var valorINSS = limpaNumero($("#baseINSS").html());
    var valorTotal = 499.08;
    if (valorSubTotal <= 4159) {
        valorTotal = (valorSubTotal * (valorINSS / 100));
    }
    fValINSS = Number(valorTotal);
    $("#contentINSS").html(Number((valorTotal).toFixed(2)));
}

function atualizaVT() {
    var qteValeMes = $("#QteValesMensal").val() != "" ? limpaNumero($("#QteValesMensal").val()) : 0;
    var valorSalario = limpaNumero($("#valorSalario").val());
    var valorPassagemAtual = 2.75;
    var porcentagemDesconto = 0.06;
    var descontoSalario = valorSalario * porcentagemDesconto;
    var totalDePassagem = qteValeMes * valorPassagemAtual;
    var valorDesconto = descontoSalario < totalDePassagem ? totalDePassagem - descontoSalario : 0;
    if (qteValeMes == 0) {
        $("#QteValesMensal").val("0");
    }
    $("#contentVT").html(Number((valorDesconto).toFixed(2)));
}

function atualizaTotal() {
    var valorAC = $("#valorAC").val() != "" ? limpaNumero($("#valorAC").val()) : 0;
    var valorSubTotal = parseFloat($("#contentSubTotal").html());
    var valorFGTS = parseFloat($("#contentFGTS").html());
    var valorINSS = parseFloat($("#contentINSS").html());
    var VT = parseFloat($("#contentVT").html());
    var valorTotal = (Number(valorAC) + fSubtotal + fValFGTS + fValINSS + Number(VT));

    if (valorAC == 0) {
        $("#valorAC").val("0");
    }

    $("#contentTotal").html(Number((valorTotal).toFixed(2)));
}

function atualizarHidden() {
    $("#contentADNHidden").val(fadnValor);
    $("#contentDADNHidden").val(fdsrADN);
    $("#contentHENSNHidden").val(fDsrHENoturna);
    $("#contentHENHidden").val(fHENoturnaValor);
    $("#contentDSRSHEHidden").val(fDsrHE);
    $("#contentHora50Hidden").val(fHEValor);
    $("#contentVTHidden").val($("#contentVT").text());
    $("#contentINSSHidden").val(fValINSS);
    $("#contentFGTSHidden").val(fValFGTS);
}

$(function () {
    atualizarHorasSalario();
    mudaCampohide();
    atualizaVT();
    atualizaADN();
    atualizaDADN();
    atualizaHE50();
    atualizaDSRSHE();
    atualizaHEN();
    atualizaDSHEN();
    atualizaSubTotal();
    atualizaFGTS();
    atualizaINSS();
    atualizaTotal();

    $('.valor').mask('00000,00', { reverse: true });
    $('.hora').mask('000', { reverse: true });

    $("#qteFilhos").bind('focusout', function (e) {
        mudaCampohide();
    });

    $("#horasSalario").bind('focusout', function (e) {
        atualizarHorasSalario();
    });

    $("#valorSalario").bind('focusout', function (e) {
        atualizaValor();
        atualizaVT();
        atualizaADN();
        atualizaDADN();
        atualizaHE50();
        atualizaDSRSHE();
        atualizaHEN();
        atualizaDSHEN();
        atualizaSubTotal();
        atualizaFGTS();
        atualizaINSS();
        atualizaTotal();
    });

    $("#horasAdicional").bind('focusout', function (e) {
        atualizaVT();
        atualizaADN();
        atualizaDADN();
        atualizaHE50();
        atualizaDSRSHE();
        atualizaHEN();
        atualizaDSHEN();
        atualizaSubTotal();
        atualizaFGTS();
        atualizaINSS();
        atualizaTotal();
    });
    $("#horasSalario").bind('focusout', function (e) {
        atualizaVT();
        atualizaADN();
        atualizaDADN();
        atualizaHE50();
        atualizaDSRSHE();
        atualizaHEN();
        atualizaDSHEN();
        atualizaSubTotal();
        atualizaFGTS();
        atualizaINSS();
        atualizaTotal();
    });
    $("#horasCinquenta").bind('focusout', function (e) {
        atualizaVT();
        atualizaADN();
        atualizaDADN();
        atualizaHE50();
        atualizaDSRSHE();
        atualizaHEN();
        atualizaDSHEN();
        atualizaSubTotal();
        atualizaFGTS();
        atualizaINSS();
        atualizaTotal();
    });
    $("#horasNoturnas").bind('focusout', function (e) {
        atualizaVT();
        atualizaADN();
        atualizaDADN();
        atualizaHE50();
        atualizaDSRSHE();
        atualizaHEN();
        atualizaDSHEN();
        atualizaSubTotal();
        atualizaFGTS();
        atualizaINSS();
        atualizaTotal();
    });

    $("#valorAC").bind('focusout', function (e) {
        atualizaVT();
        atualizaADN();
        atualizaDADN();
        atualizaHE50();
        atualizaDSRSHE();
        atualizaHEN();
        atualizaDSHEN();
        atualizaSubTotal();
        atualizaFGTS();
        atualizaINSS();
        atualizaTotal();
    });
    $("#QteValesMensal").bind('focusout', function (e) {
        atualizaVT();
        atualizaADN();
        atualizaDADN();
        atualizaHE50();
        atualizaDSRSHE();
        atualizaHEN();
        atualizaDSHEN();
        atualizaSubTotal();
        atualizaFGTS();
        atualizaINSS();
        atualizaTotal();
    });
});