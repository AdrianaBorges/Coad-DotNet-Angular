using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.Relatorios
{
    public partial class RelFaturamentoAreaDTO
    {
        public RelFaturamentoAreaDTO()
        {
            this.LISTAPROD = new HashSet<RelFaturamentoAreaProdutoDTO>();
        }

        public Nullable<int> AREA_ID { get; set; }
        public string AREA_NOME { get; set; }
        public Nullable<int> QTDE { get; set; }
        public Nullable<decimal> VALOR { get; set; }
        public Nullable<decimal> CANCELADOS { get; set; }
        public virtual ICollection<RelFaturamentoAreaProdutoDTO> LISTAPROD { get; set; }

    }
    public partial class RelFaturamentoAreaProdutoDTO
    {
        public RelFaturamentoAreaProdutoDTO()
        {
            this.LISTAUF = new HashSet<RPT_CONTRATOS_PRODUTO_REGIAO_Result>();
        }

        public Nullable<int> PRO_ID { get; set; }
        public string PRO_NOME { get; set; }
        public Nullable<int> QTDE { get; set; }
        public Nullable<decimal> VALOR { get; set; }
        public Nullable<decimal> CANCELADOS { get; set; }
        public virtual ICollection<RPT_CONTRATOS_PRODUTO_REGIAO_Result> LISTAUF { get; set; }
    }
    
}
