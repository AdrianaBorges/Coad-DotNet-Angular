using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class ProspectRepresentanteDTO
    {
        public RepresentanteDTO Representante { get; set; }
        public string CarId { get; set; }
        public string Regiao { get; set; }
        public string Uen { get; set; }
    }
}
