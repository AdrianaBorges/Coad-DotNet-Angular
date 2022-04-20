using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model
{
    public class NFeImpostoPIS
    {
        [XmlElement("PISNT")]
        public NFeImpostoPISGrupoNaoTributadoDTO PISGrupoNaoTributado { get; set; }
    }
}
