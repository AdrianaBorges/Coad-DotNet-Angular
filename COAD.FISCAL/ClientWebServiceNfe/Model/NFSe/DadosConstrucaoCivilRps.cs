using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Model.NFSe
{
    public class DadosConstrucaoCivilRps
    {
        [Required(ErrorMessage = "Informe o Código da Obra")]
        public string CodigoObra { get; set; }

        [Required(ErrorMessage = "Informe o Código para Art")]
        public string Art { get; set; }
    }
}
