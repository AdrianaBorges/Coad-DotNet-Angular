using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(MUNDIPAGG_ASSINATURA))]
    public class MundipaggAssinaturaDTO
    {
        public int MPG_ASN_ID { get; set; }
        public string MPG_ASN_SUB_ID { get; set; }
        public int MPG_CLI_ID { get; set; }

        public virtual MUNDIPAGG_CLIENTE MUNDIPAGG_CLIENTE { get; set; }
    }
}
