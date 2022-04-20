using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    
    public partial class TipoTelefoneDTO
    {
        public TipoTelefoneDTO()
        {
            this.ASSINATURA_TELEFONE = new HashSet<AssinaturaTelefoneDTO>();
            this.CLIENTES_TELEFONE = new HashSet<ClienteTelefoneDTO>();
        }
    
        public int TIPO_TEL_ID { get; set; }
        public string TIPO_TEL_DESCRICAO { get; set; }
        public int? CLI_ID { get; set; }
    
        public virtual ICollection<AssinaturaTelefoneDTO> ASSINATURA_TELEFONE { get; set; }
        public virtual ICollection<ClienteTelefoneDTO> CLIENTES_TELEFONE { get; set; }
    }
}
