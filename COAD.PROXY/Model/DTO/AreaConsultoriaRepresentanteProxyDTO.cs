using COAD.COADGED.Model.DTO;
using COAD.CORPORATIVO.Model.Dto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PROXY.Model.DTO
{
    [Mapping(Source = typeof(AreaConsultoriaRepresentanteDTO))]
    public class AreaConsultoriaRepresentanteProxyDTO : AreaConsultoriaRepresentanteDTO
    {
        public AreasDTO COLECIONADOR { get; set; }
        public TitulacaoDTO GRANDE_GRUPO { get; set; }
    }
}
