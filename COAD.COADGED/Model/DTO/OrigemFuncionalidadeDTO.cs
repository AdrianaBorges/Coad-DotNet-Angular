using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Model.DTO
{
    public class OrigemFuncionalidadeDTO
    {
        public int OAC_ID { get; set; }
        public int FCI_ID { get; set; }
        public string OFU_DESCRICAO { get; set; }
        public Nullable<bool> OFU_ATIVO { get; set; }
        public Nullable<int> OFU_ORDEM { get; set; }
        public Nullable<System.DateTime> OFU_DATA_ALTERA { get; set; }
        public string USU_LOGIN { get; set; }
        public string OFU_LOCAL_EXIBE { get; set; }
        public Nullable<bool> OFU_DESCRICAO_EXIBE { get; set; }

        public virtual FuncionalidadeDTO FUNCIONALIDADE { get; set; }
        public virtual OrigemAcessoRefDTO ORIGEM_ACESSO_REF { get; set; }

    }
}
