using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class ImportarClienteAgendaDTO
    {
        public ImportarClienteAgendaDTO()
        {
            this.ASSINATURA_EMAIL = new HashSet<AssinaturaEmailDTO>();
            this.ASSINATURA_TELEFONE = new HashSet<AssinaturaTelefoneDTO>();            
        }

        public int? RG_ID { get; set; }

        [Required(ErrorMessage = "Não é possível localizar o Id do cliente.")]
        public int? CLI_ID { get; set; }
        public int? REP_ID { get; set; }
        public int? REP_ID_DEMANDANTE { get; set; }

        public virtual ICollection<AssinaturaTelefoneDTO> ASSINATURA_TELEFONE { get; set; }
        public virtual ICollection<AssinaturaEmailDTO> ASSINATURA_EMAIL { get; set; }
        
    }
}
