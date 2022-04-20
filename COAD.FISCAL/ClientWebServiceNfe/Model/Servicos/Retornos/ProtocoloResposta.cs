using COAD.FISCAL.Model.Enumerados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.Servicos.Retornos
{
    public class ProtocoloResposta
    {
        
        public string Id { get; set; }
        public TipoAmbienteEnum tpAmb { get; set; }
        public string verAplic { get; set; }
        public string chNFe { get; set; }
        public DateTime? dhRecbto { get; set; }
        public string nProt { get; set; }
        public string digVal { get; set; }
        public int? cStat { get; set; }
        public string xMotivo { get; set; }

    }
}
