using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Model.DTO
{
    public class FuncionalidadeDTO
    {
        public FuncionalidadeDTO()
        {
            this.ORIGEM_FUNCIONALIDADE = new HashSet<OrigemFuncionalidadeDTO>();
        }
    
        public Nullable<int> FCI_ID { get; set; }
        public string FCI_DESCRICAO { get; set; }
        public string FCI_CONTEUDO { get; set; }
        public string FCI_URL { get; set; }
        public Nullable<System.DateTime> FCI_DATA_ALTERA { get; set; }
        public string USU_LOGIN { get; set; }
        public Nullable<int> NIV_ID { get; set; }
        public Nullable<int> PRO_ID { get; set; }
        public string FCI_TIPO { get; set; }
        public string FCI_IMG01 { get; set; }
        public string FCI_IMG02 { get; set; }
        public string FCI_IMG03 { get; set; }
        public string FCI_IMG04 { get; set; }
        public string TDC_ID { get; set; }
        public string FCI_URL_LINK { get; set; }
    
        public virtual FuncionalidadeNivelAcessoDTO FUNCIONALIDADE_NIVEL_ACESSO { get; set; }
        public virtual ProdutoRefDTO PRODUTO_REF { get; set; }
        public virtual ICollection<OrigemFuncionalidadeDTO> ORIGEM_FUNCIONALIDADE { get; set; }

    }
}
