using COAD.FISCAL.Model.Enumerados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Model.DTO
{
    public class ParcelaDTO
    {
        public ParcelaDTO()
        {
            Impostos = new HashSet<ImpostosDTO>();
        }

        public decimal? ValorParcela { get; set; }
        public decimal? BaseCalculo { get; set; }
        public decimal? ValorLiquido { get; set; }
        public decimal? ValorLiquidoServico { get; set; }
        public int? NumeroParcela { get; set; }
        public TipoPagamentoEnum TipoPagamentoParcela { get; set; }
        public ICollection<ImpostosDTO> Impostos { get; set; }
        public bool? NaoRetevePorRegra { get; set; }
        public string Parcela { get; set; }
        public string Contrato { get; set; }
        public DateTime Vencimento { get; set; }

        public virtual ICollection<ImpostosDTO> ImpostosFederais {
            get
            {
                if(Impostos != null)
                {
                    var impostosFederais = Impostos
                        .Where(x => x.TipoImposto == ImpostosEnum.IR)
                        .ToList();
                    return impostosFederais;
                }
                return null;
            }
        }

        public virtual ICollection<ImpostosDTO> ImpostosMunicipais
        {
            get
            {
                if (Impostos != null)
                {
                    var impostosMunicipais = Impostos
                        .Where(x => x.TipoImposto != ImpostosEnum.IR)
                        .ToList();
                    return impostosMunicipais;
                }
                return null;
            }
        }
    }
}
