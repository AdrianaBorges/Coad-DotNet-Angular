using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.Servicos.Enumerados
{
    public enum SituacaoContribuinteEnum
    {
        [XmlEnum("0")]
        NAO_HABILITADO = 0,

        [XmlEnum("1")]
        HABILITADO = 1
    }
}
