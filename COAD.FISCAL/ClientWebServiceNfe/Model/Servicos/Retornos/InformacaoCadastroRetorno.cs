using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.Servicos.Retornos
{
    public class InformacaoCadastroRetorno
    {
        public InformacaoCadastroRetorno()
        {
            this.lstInfCadRetDados = new List<InfCadastroRetornoDados>();
        }

        [XmlElement("verAplic")]
        public string VersaoDoAplicativo { get; set; }

        [XmlElement("cStat")]
        public int? CodigoRetorno { get; set; }

        [XmlElement("xMotivo")]
        public string MensagemRetorno { get; set; }
        public string UF { get; set; }
        public string IE { get; set; }
        public string CNPJ { get; set; }
        public string CPF { get; set; }

        [XmlElement("dhCons")]
        public DateTime? DataConsulta { get; set; }

        [XmlElement("cUF")]
        public int? CodigoUF { get; set; }

        [XmlElement("infCad")]
        public List<InfCadastroRetornoDados> lstInfCadRetDados { get; set; }

    }
}
