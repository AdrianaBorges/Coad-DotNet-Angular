using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.SEGURANCA.Model.Custons.Pesquisas
{
    public class ListaFilaEmailDTO
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Informe o E-Mail")]
        [EmailAddress(ErrorMessage = "Informe o E-Mail")]
        public string Email { get; set; }
        public string Assunto { get; set; }
        public DateTime? Data { get; set; }
        public DateTime? DataEnvio { get; set; }
        public DateTime? DataCancelamento { get; set; }
        public string Usuario { get; set; }
    }
}
