using COAD.PORTAL.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PORTAL.Model.DTO.PortalConsultoria
{
    [Mapping(Source = typeof(consultores))]
    public class ConsultoresPortalDTO
    {
        public int id { get; set; }
        public string usuario { get; set; }
        public string senha { get; set; }
        public string colec { get; set; }
        public string tipo { get; set; }
        public string filial { get; set; }
        public System.DateTime dataCadastro { get; set; }
    }
}
