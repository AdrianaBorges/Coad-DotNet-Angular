using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.Enumerados
{
    /// <summary>
    /// Finalidade de emissão da NF-e
    /// </summary>
    public enum FinalidadeNotaFiscalEnum
    {
        [XmlEnum("1")]
        NfeNormal = 1,
        
        [XmlEnum("2")]
        NfeComplementar = 2,
        
        [XmlEnum("3")]
        NfeAjuste = 3,
            
        [XmlEnum("4")]
        DevolucaoMercadoria = 4
    }
}
