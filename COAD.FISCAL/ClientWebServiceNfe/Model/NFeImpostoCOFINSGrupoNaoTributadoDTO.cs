using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Model
{
    public class NFeImpostoCOFINSGrupoNaoTributadoDTO 
    {
        [Required(ErrorMessage = "O campo CST é obrigatório")]
        public string CST { get { return "06"; } set { } }
    }
}
