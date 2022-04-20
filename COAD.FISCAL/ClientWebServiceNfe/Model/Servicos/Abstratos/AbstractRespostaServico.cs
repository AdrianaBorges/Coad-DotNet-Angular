using COAD.FISCAL.Model.Enumerados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.Servicos.Abstratos
{
    public class AbstractRespostaServico
    {
        [XmlAttribute]
        public string versao { get; set; }
        public TipoAmbienteEnum tpAmb { get; set; }
        public string verAplic { get; set; }
        public string nRec { get; set; }
        public int? cStat { get; set; }
        public string xMotivo { get; set; }
        public int? cUF { get; set; }
    }
}
