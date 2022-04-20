using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.Relatorios
{
    public class RelFaturamentoProdutoDTO
    {
        public int? MES_FAT { get; set; }
        public int? ANO_FAT { get; set; }
        public int? PRO_ID { get; set; }
        public string PRO_NOME { get; set; }
        public int QTDE_CONTRATOS { get; set; }
        public int QTDE_VENDANOVA { get; set; }
        public int QTDE_RENOVACAO { get; set; }
        public Nullable<decimal> VENDA_NOVA { get; set; }
        public Nullable<decimal> RENOVACAO { get; set; }
        public Nullable<decimal> VALOR_TOTAL { get; set; }
        public Nullable<decimal> PR_MEDIO_VENDA { get; set; }
        public Nullable<decimal> PR_MEDIO_RENOVACAO { get; set; }
        public Nullable<decimal> PERC_REALIZADO { get; set; }
    }

    public class RelPrevisaoReceitaProdutoDTO
    {
        public int? MES_FAT { get; set; }
        public int? ANO_FAT { get; set; }
        public int? TPG_ID { get; set; }
        public String TPG_DESCRICAO { get; set; }
        public Nullable<decimal> TOT_PREVISTO { get; set; }
        public Nullable<decimal> TOT_REC_MES { get; set; }
        public Nullable<decimal> TOT_REC_OUTROS { get; set; }
        public Nullable<decimal> TOT_REC { get; set; }
        public Nullable<decimal> PERC_REALIZADO { get; set; }
    }
}
