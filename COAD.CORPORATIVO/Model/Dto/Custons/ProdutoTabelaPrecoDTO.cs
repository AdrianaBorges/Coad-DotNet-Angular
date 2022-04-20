using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class ProdutoTabelaPrecoDTO
    {
        public int? CMP_ID { get; set; }
        public int? CMP_ID_ORIGEM { get; set; }
        public string CMP_DESCRICAO { get; set; }
        public decimal? RTP_PRECO_VENDA { get; set; }
        public int? RG_ID { get; set; }
        public int? TP_ID { get; set; }
        public int? TPG_ID { get; set; }
    }
}
