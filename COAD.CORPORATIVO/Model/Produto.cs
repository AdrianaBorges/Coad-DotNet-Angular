using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model
{
    public class Produto
    {
        public Produto()
        {
            this.ASSINATURA = new HashSet<ASSINATURA>();
            this.NOTA_FISCAL_ITEM = new HashSet<NOTA_FISCAL_ITEM>();
            this.PRODUTO_COMPOSICAO = new HashSet<PRODUTO_COMPOSICAO>();
            this.PRODUTO_COMPOSICAO1 = new HashSet<PRODUTO_COMPOSICAO>();
            this.PRODUTO_FORNECEDOR = new HashSet<PRODUTO_FORNECEDOR>();
        }

        public int PRO_ID { get; set; }
        public string PRO_SIGLA { get; set; }
        public string PRO_NOME { get; set; }
        public Nullable<int> PRO_ID_DERVADO { get; set; }
        public string PRO_MOD_CARTA_URA { get; set; }
        public Nullable<int> PRO_TIPO_REMESSA { get; set; }
        public Nullable<int> PRO_RECEBE_MALA { get; set; }
        public Nullable<int> PRO_RECEBE_PASTA_SN { get; set; }
        public Nullable<int> PRO_PRODUTO_ACABADO { get; set; }
        public Nullable<int> PRO_STATUS { get; set; }
        public int TIPO_PRO { get; set; }
        public string PRO_EMITE_NF { get; set; }
        public string NCM_ID { get; set; }
        public int GRUPO_ID { get; set; }
        public Nullable<System.DateTime> DATA_CADASTRO { get; set; }
        public Nullable<System.DateTime> DATA_ALTERA { get; set; }
        public string PRO_UN_COMPRA { get; set; }
        public string PRO_UN_VEND { get; set; }
        public Nullable<decimal> PRO_PRECO_COMPRA { get; set; }
        public Nullable<decimal> PRO_PRECO_CUSTO { get; set; }
        public Nullable<int> AREA_ID { get; set; }

        public AREAS AREAS { get; set; }
        public ICollection<ASSINATURA> ASSINATURA { get; set; }
        public GRUPO GRUPO { get; set; }
        public ICollection<NOTA_FISCAL_ITEM> NOTA_FISCAL_ITEM { get; set; }
        public ICollection<PRODUTO_COMPOSICAO> PRODUTO_COMPOSICAO { get; set; }
        public ICollection<PRODUTO_COMPOSICAO> PRODUTO_COMPOSICAO1 { get; set; }
        public ICollection<PRODUTO_FORNECEDOR> PRODUTO_FORNECEDOR { get; set; }
    }
}