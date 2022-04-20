using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.Enumerados
{
    public enum TipoPagamentoEnum
    {
        [XmlEnum("01")]
        DINHEIRO = 1,

        [XmlEnum("02")]
        CHEQUE = 2,

        [XmlEnum("03")]
        CARTAO_CREDITO = 3,

        [XmlEnum("04")]
        CARTAO_DEBITO = 4,

        [XmlEnum("05")]
        CREDITO_LOJA = 5,

        [XmlEnum("10")]
        VALE_ALIMENTACAO = 10,

        [XmlEnum("11")]
        VALE_REFEICAO = 11,

        [XmlEnum("12")]
        VALE_PRESENTE = 12,

        [XmlEnum("13")]
        VALE_COMBUSTIVEL = 13,

        [XmlEnum("14")]
        DUPLICATA_MERCANTIL = 14,


        [XmlEnum("15")]
        BOLETO_BANCARIO = 15,


        [XmlEnum("90")]
        SEM_PAGAMENTO = 90,


        [XmlEnum("99")]
        OUTROS = 99,


    }
}
