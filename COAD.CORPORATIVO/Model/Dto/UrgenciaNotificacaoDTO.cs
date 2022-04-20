using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(URGENCIA_NOTIFICACAO))]
    public class UrgenciaNotificacaoDTO
    {
        public UrgenciaNotificacaoDTO()
        {
            this.NOTIFICACOES = new HashSet<NotificacoesDTO>();
        }
    
        public string URG_NTF_ID { get; set; }
        public string URG_NTF_DESCRICAO { get; set; }
        

        public virtual ICollection<NotificacoesDTO> NOTIFICACOES { get; set; }
    }
}
