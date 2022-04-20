using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(UEN))]
    public class UENDTO
    {
        public UENDTO()
        {
            this.CARTEIRA = new HashSet<CarteiraDTO>();
            this.REPRESENTANTE = new HashSet<RepresentanteDTO>();
            this.HISTORICO_ATENDIMENTO = new HashSet<HistoricoAtendimentoDTO>();
            this.PROPOSTA = new HashSet<PropostaDTO>();
        }
    
        public int UEN_ID { get; set; }
        public string UEN_DESCRICAO { get; set; }
    
        public virtual ICollection<CarteiraDTO> CARTEIRA { get; set; }
        public virtual ICollection<RepresentanteDTO> REPRESENTANTE { get; set; }
        public virtual ICollection<AssinaturaDTO> ASSINATURA { get; set; }
        public virtual ICollection<HistoricoAtendimentoDTO> HISTORICO_ATENDIMENTO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<PropostaDTO> PROPOSTA { get; set; }
    }
}
