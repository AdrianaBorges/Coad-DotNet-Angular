using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(MUNDIPAGG_CLIENTE))]
    public class MundipaggClienteDTO
    {
       
        public int MPG_CLI_ID { get; set; }
        public string MPG_CLI_CUS_ID { get; set; }
        public string MPG_CLI_NAME { get; set; }
        public string MPG_CLI_CODE { get; set; }
        public string MPG_CLI_DOCUMENT { get; set; }
        public string MPG_CLI_TYPE { get; set; }
        public int CLI_ID { get; set; }

        public virtual CLIENTES CLIENTES { get; set; }

    }
}
