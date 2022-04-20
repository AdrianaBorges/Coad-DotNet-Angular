using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class SysPropertiesDTO
    {
        public bool Homologacao { get; set; }
        public string PathXML { get; set; }
        public string EmailTeste { get; set; }
        public string HostName { get; set; }
        public bool EmailTesteAtivo { get; set; }
        public int CodAdquirente { get; set; }
        public bool UseMinResource { get; set; }
    }
}
