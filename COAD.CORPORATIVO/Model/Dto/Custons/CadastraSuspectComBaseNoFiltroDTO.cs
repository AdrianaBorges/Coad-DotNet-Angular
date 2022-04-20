using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class CadastraSuspectComBaseNoFiltroDTO
    {
        public string cpf_cnpj {get; set;}
        public string nome {get; set;}
        public string email {get; set;}
        public string dddTelefone {get; set;}
        public string telefone {get; set;}
        public int? AREA_ID {get; set;}
        public int? CMP_ID {get; set;}            
    }
}
