using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class UploadNotaFiscalDTO
    {
        [Required(ErrorMessage = "O código do pedido não foi encontrado")] 
        public int? IPE_ID {get;set;}

        [Required(ErrorMessage = "Digite o código da nota")]
        public string chaveDaNota { get; set; }

    }
}
