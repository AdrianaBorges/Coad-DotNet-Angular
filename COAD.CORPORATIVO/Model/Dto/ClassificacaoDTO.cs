using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public class ClassificacaoDTO
    {
        public ClassificacaoDTO()
        {
            this.CARTEIRA_ASSINATURA = new HashSet<CarteiraAssinaturaDTO>();
            this.CLIENTES = new HashSet<ClienteDto>();
        }
    
        public int CLA_ID { get; set; }
        public string CLA_DESCRICAO { get; set; }
    
        public virtual ICollection<CarteiraAssinaturaDTO> CARTEIRA_ASSINATURA { get; set; }
        public virtual ICollection<ClienteDto> CLIENTES { get; set; }
    }
}
