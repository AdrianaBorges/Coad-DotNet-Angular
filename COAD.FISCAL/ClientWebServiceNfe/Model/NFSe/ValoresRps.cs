using COAD.FISCAL.Model.NFSe.Enumerados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Model.NFSe
{
    public class ValoresRps
    {
        public decimal? ValorServicos { get; set; }
        public decimal? ValorDeducoes { get; set; }
        public decimal? ValorPis { get; set; }
        public decimal? ValorCofins { get; set; }
        public decimal? ValorInss { get; set; }
        public decimal? ValorIr { get; set; }
        public decimal? ValorCsll { get; set; }
        public IdenSimNaoEnum IssRetido { get; set; }
        public decimal? ValorIss { get; set; }
        public decimal? ValorIssRetido { get; set; }
        public decimal? OutrasRetencoes { get; set; }


        /// <summary>
        /// Valor dos serviços - Valor das
        /// deduções - descontos
        /// incondicionados
        /// </summary>
        public decimal? BaseCalculo { get; set; }
        public decimal? Aliquota { get; set; }


        /// <summary>
        /// ValorServicos - ValorPIS -
        /// ValorCOFINS - ValorINSS -
        /// ValorIR - ValorCSLL -
        /// OutrasRetençoes -
        /// ValorISSRetido -
        /// DescontoIncondicionado -
        // DescontoCondicionado
        /// </summary>
        public decimal? ValorLiquidoNfse { get; set; }
        public decimal? DescontoIncondicionado { get; set; }
        public decimal? DescontoCondicionado { get; set; }


        public bool ShouldSerializeValorDeducoes()
        {
            return (ValorDeducoes.HasValue);
        }
        public bool ShouldSerializeValorPis()
        {
            return (ValorPis.HasValue);
        }
        public bool ShouldSerializeValorCofins()
        {
            return (ValorCofins.HasValue);
        }
        public bool ShouldSerializeValorInss()
        {
            return (ValorInss.HasValue);
        }

        public bool ShouldSerializeValorIr()
        {
            return (ValorIr.HasValue);
        }
        public bool ShouldSerializeValorCsll()
        {
            return (ValorCsll.HasValue);
        }

        public bool ShouldSerializeValorIss()
        {
            return (ValorIss.HasValue);
        }
        public bool ShouldSerializeOutrasRetencoes()
        {
            return (OutrasRetencoes.HasValue);
        }

        public bool ShouldSerializeBaseCalculo()
        {
            return (BaseCalculo.HasValue);
        }

        public bool ShouldSerializeAliquota()
        {
            return (Aliquota.HasValue);
        }

        public bool ShouldSerializeValorIssRetido()
        {
            return (ValorIssRetido.HasValue);
        }

        public bool ShouldSerializeDescontoCondicionado()
        {
            return (DescontoCondicionado.HasValue);
        }

        public bool ShouldSerializeDescontoIncondicionado()
        {
            return (DescontoIncondicionado.HasValue);
        }
    }
}
