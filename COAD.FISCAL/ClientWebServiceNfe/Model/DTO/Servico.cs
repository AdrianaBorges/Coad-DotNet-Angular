using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Model.DTO
{
    public class Servico
    {
        public ICollection<ImpostosDTO> Impostos { get; set; }
        public string DescricaoServico { get; set; }
        public string CodigoTributacaoMunicipio { get; set; }
        public string CodListaServico { get; set; }
        public decimal? ValorServicos { get; set; }
        public decimal? ValorDeducoes { get; set; }
        public decimal? ValorPis { get; set; }
        public decimal? ValorCofins { get; set; }
        public decimal? ValorInss { get; set; }
        public decimal? ValorIr { get; set; }
        public decimal? ValorCsll { get; set; }
        public bool IssRetido { get; set; }
        public decimal? ValorIss { get; set; }
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
        public decimal? ValorIssRetido { get; set; }
        public decimal? DescontoCondicionado { get; set; }
        public decimal? DescontoIncondicionado { get; set; }
    }
}
