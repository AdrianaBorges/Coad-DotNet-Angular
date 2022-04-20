using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(Source = typeof(PAIS))]
    public partial class PaisDTO
    {
        public PaisDTO()
        {
        }
        public string PAI_ID { get; set; }
        public string PAI_NOME { get; set; }

    }
}
