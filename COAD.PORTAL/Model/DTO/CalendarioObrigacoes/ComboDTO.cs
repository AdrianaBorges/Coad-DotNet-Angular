using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PORTAL.Model.DTO.CalendarioObrigacoes
{
    //A ideia aqui é que toda combo que é montada no banco usará esse DTO como retorno
    public class ComboDTO
    {
        public string valor { get; set; }
        public string texto { get; set; }
    }
}
