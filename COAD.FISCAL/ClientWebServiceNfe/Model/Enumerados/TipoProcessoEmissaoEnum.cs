using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.Enumerados
{
    /// <summary>
    /// Processo de emissão da NF-e
    /// </summary>
    public enum TipoProcessoEmissao
    {
        /**
         * 
         * Identificador do processo de emissão da NF-e:
            0 - emissão de NF-e com aplicativo do contribuinte;
            1 - emissão de NF-e avulsa pelo Fisco;
            2 - emissão de NF-e avulsa, pelo contribuinte com seu certificado digital, através do site do Fisco;
            3- emissão NF-e pelo contribuinte com aplicativo fornecido pelo Fisco.
         */
        [XmlEnum("0")]
        AplicativoDoContribuinte = 0,

        [XmlEnum("1")]
        AvulsaPeloFisco = 1,

        [XmlEnum("2")]
        AvulsaComCertificadoDigitalPeloSiteDoFisco = 2

    }
}
