using COAD.FISCAL.Model.DTOCriptografia;
using COAD.FISCAL.Model.Enumerados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.Servicos.Retornos
{
    [Serializable]
    public class InformacoesInutilizacaoRetorno
    {
        [XmlAttribute]
        public string Id { get; set; }
        public TipoAmbienteEnum tpAmb { get; set; }
        public string verAplic { get; set; }
        public int? cStat { get; set; }
        public string xMotivo { get; set; }
        public int? cUF { get; set; }
        public int? ano { get; set; }
        public string CNPJ { get; set; }

        /// <summary>
        /// Modelo do documento (55 ou 65)
        /// </summary>
        public int? mod { get; set; }
        public int? serie { get; set; }
        public int? nNFIni { get; set; }
        public int? nNFFin { get; set; }

        public DateTime? dhRecbto { get; set; }
        public string nProt { get; set; }
        public SignatureDTO Signature { get; set; }


    }
}
