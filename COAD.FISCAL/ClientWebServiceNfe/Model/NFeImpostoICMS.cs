using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model
{
    [Serializable]
   // [XmlInclude(typeof(NFeImpostoICMSGrupo40))]
    public class NFeImpostoICMS 
    {
        //public NFeImpostoGrupoICMS grupo { get; set; }
        public NFeImpostoICMSGrupo40 ICMS40 { get; set; }
    }
}
