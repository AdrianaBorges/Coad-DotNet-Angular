using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.Servicos.Enumerados
{
    /// <summary>
    /// O processamento síncrono do Lote corresponde a entrega da resposta do processamento das NF-e do Lote, 
    /// sem a geração de um Recibo de Lote para consulta futura. A resposta de forma síncrona pela SEFAZ Autorizadora só ocorrerá se:
    /// -a empresa solicitar e constar unicamente uma NF-e no Lote;
    /// -a SEFAZ Autorizadora implementar o processamento
    /// </summary>
    public enum IndicacaoDeSincroniaEnum
    {
        /// <summary>
        /// Assincrono (padrão)
        /// </summary>
        /// 
        [XmlEnum("0")]
        ASSINCRONO = 0,

        /// <summary>
        /// Empresa solicita processamento síncrono do Lote de NF-e (sem a geração de Recibo para consulta futura)
        /// </summary>
        [XmlEnum("1")]
        SINCRONO = 1
        
    }
}
