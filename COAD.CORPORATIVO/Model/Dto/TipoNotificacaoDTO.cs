using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public class TipoNotificacaoDTO
    {
        public TipoNotificacaoDTO()
        {
            this.NOTIFICACOES = new HashSet<NotificacoesDTO>();
        }
    
        public int TP_NTF_ID { get; set; }
        public string TP_NTF_DESCRICAO { get; set; }
    
        public virtual ICollection<NotificacoesDTO> NOTIFICACOES { get; set; }
    }
}
