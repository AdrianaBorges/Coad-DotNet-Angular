using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model
{
    public class NFeImpostoCOFINS 
    {
        [XmlElement("COFINSNT")]
        public NFeImpostoCOFINSGrupoNaoTributadoDTO COFINSGrupoNaoTributado { get; set; }
    }
}
