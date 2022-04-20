using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public class ConfigSpedFiscalDTO
    {
        public ConfigSpedFiscalDTO()
        {
        }
        public int EMP_ID { get; set; }
        public string REG1010_IND_EXP { get; set; }
        public string REG1010_IND_CCRF { get; set; }
        public string REG1010_IND_COMB { get; set; }
        public string REG1010_IND_USINA { get; set; }
        public string REG1010_IND_VA { get; set; }
        public string REG1010_IND_EE { get; set; }
        public string REG1010_IND_CART { get; set; }
        public string REG1010_IND_FORM { get; set; }
        public string REG1010_IND_AER { get; set; }
        public Nullable<int> CRE_DEB_ICMS { get; set; }

      //  public virtual EMPRESA_REF EMPRESA_REF { get; set; }
    }
}
