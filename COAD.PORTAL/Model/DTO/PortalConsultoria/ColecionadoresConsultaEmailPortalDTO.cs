using COAD.PORTAL.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PORTAL.Model.DTO.PortalConsultoria
{
    [Mapping(Source = typeof(colecionadores))]
    public class ColecionadoresConsultaEmailPortalDTO
    {
        public int id { get; set; }
        public string menu_coadcorp { get; set; }
        public string tipo_portal { get; set; }
        public string observacao { get; set; }
    }
}
