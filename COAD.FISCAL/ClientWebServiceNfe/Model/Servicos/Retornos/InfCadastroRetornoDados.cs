using COAD.FISCAL.Model.Servicos.Enumerados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.Servicos.Retornos
{
    public class InfCadastroRetornoDados
    {
        public InfCadastroRetornoDados()
        {
            LstEnderecos = new List<EnderecoContribuinte>();
        }

        public string IE { get; set; }
        public string CNPJ { get; set; }
        public string CPF { get; set; }
        public string UF { get; set; }

        [XmlElement("cSit")]
        public SituacaoContribuinteEnum SituacaoConstribuinte { get; set; }

        [XmlElement("indCredNFe")]
        public IndicadorContribuinteNFeEnum IndicadorContribuinteNFe { get; set; }

        [XmlElement("indCredCTe")]
        public IndicadorContribuinteNFeEnum IndicadorContribuinteCTe { get; set; }

        [XmlElement("xNome")]
        public string RazaoSocial { get; set; }

        [XmlElement("xFant")]
        public string NomeFantasia { get; set; }

        [XmlElement("xRegApur")]
        public string RegimeApuracao { get; set; }

        [XmlElement("CNAE")]
        public string CNAE { get; set; }

        [XmlElement("dIniAtiv")]
        public DateTime? DataInicioAtividade { get; set; }

        [XmlElement("dUltSit")]
        public DateTime? DataUltimaModificacao { get; set; }

        [XmlElement("dBaixa")]
        public DateTime? DataBaixa { get; set; }

        [XmlElement("IEUnica")]
        public string IEUnica { get; set; }

        [XmlElement("IEAtual")]
        public string IEAtual { get; set; }

        [XmlElement("ender")]
        List<EnderecoContribuinte> LstEnderecos { get; set; }


    }
}
