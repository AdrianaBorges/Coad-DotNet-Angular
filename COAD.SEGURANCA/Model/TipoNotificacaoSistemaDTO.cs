using COAD.SEGURANCA.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.SEGURANCA.Model
{
    [Mapping(typeof(TIPO_NOTIFICACAO_SISTEMA))]
    public class TipoNotificacaoSistemaDTO
    {
        public TipoNotificacaoSistemaDTO()
        {
            this.NOTIFICACAO_SISTEMA = new HashSet<NotificacaoSistemaDTO>();
        }

        public int? TNS_ID { get; set; }
        public string TNS_DESCRICAO { get; set; }
        public string TNS_COD_REF_DESC { get; set; }
        public string TNS_COD_REF_NOME_CAMPO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<NotificacaoSistemaDTO> NOTIFICACAO_SISTEMA { get; set; }
    }
}
