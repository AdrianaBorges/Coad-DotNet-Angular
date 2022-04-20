using COAD.FISCAL.Model.Enumerados;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Model
{
    public class NfeImpostoIPIGrupoNaoTributado
    {
        [Required(ErrorMessage = "Informe a situação tributária")]
        public TipoTributacaoIPIEnum CST { get; set; }
       
    }
}
