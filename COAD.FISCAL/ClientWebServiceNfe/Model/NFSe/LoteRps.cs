using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.NFSe
{
    [Serializable]
    public class LoteRps
    {
        public LoteRps()
        {
            ListaRps = new List<Rps>();
        }

        [XmlAttribute]
        public string Id { get; set; }
        public int? NumeroLote { get; set; }
        public string Cnpj { get; set; }
        public string InscricaoMunicipal { get; set; }
        public int? QuantidadeRps { get; set; }
        public List<Rps> ListaRps { get; set; }

    }
}
