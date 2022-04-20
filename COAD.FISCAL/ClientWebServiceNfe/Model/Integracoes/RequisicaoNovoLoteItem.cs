using COAD.FISCAL.Model.Integracoes.Enumerados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Model.Integracoes
{
    public class RequisicaoNovoLoteItem
    {
        public RequisicaoNovoLoteItem()
        {
            this.CodNotaReferencia = new List<int>();
        }

        public int? CodPedido { get; set; }
        public int? CodProposta { get; set; }
        public int? CodCliente { get; set; }
        public int? CodEmpresa { get; set; }
        public int? NfConfigID { get; set; }
        public string CodContrato { get; set; }
        public string ChaveNotaFiscal { get; set; }
        public bool NotaAntecipada { get; set; }
        public TipoLoteItemEnum? Tipo { get; set; }

        public ICollection<int> CodNotaReferencia { get; set; }
        public string NumeroProtocolo { get; set; }
        public int? CodNotaFiscal { get; set; }
        public string CartaCorrecao { get; set; }
    }
}
