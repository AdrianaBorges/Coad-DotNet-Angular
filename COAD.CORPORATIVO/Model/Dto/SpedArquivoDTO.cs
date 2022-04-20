using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public class SpedArquivoDTO
    {
        public int SPED_ID { get; set; }
        public Nullable<System.DateTime> SPED_DATA { get; set; }
        public Nullable<System.DateTime> SPED_DATA_INICIAL { get; set; }
        public Nullable<System.DateTime> SPED_DATA_FINAL { get; set; }
        public byte[] SPED_ARQUIVO1 { get; set; }
        public Nullable<int> EMP_ID { get; set; }

        public virtual EmpresaRefDTO EMPRESA_REF { get; set; }
    }
}
