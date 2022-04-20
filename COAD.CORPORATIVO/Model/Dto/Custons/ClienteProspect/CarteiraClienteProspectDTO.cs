using Coad.GenericCrud.Validations;
using GenericCrud.Validations;
using GenericCrud.Validations.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.ClienteProspect
{
    public class CarteiraClienteProspectDTO
    {
        public CarteiraClienteProspectDTO()
        {
        }

        [RequiredIf("Deletar", false, ErrorMessage = "Informe a Carteira")]
        public string CarId { get; set; }
        public int? CliId { get; set; }
        public bool Deletar { get; set; }
    }
}
