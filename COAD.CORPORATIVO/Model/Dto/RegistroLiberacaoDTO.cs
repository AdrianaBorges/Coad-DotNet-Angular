using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(REGISTRO_LIBERACAO))]
    public class RegistroLiberacaoDTO
    {
        public RegistroLiberacaoDTO()
        {
            this.PROPOSTA_ITEM = new HashSet<PropostaItemDTO>();
            this.PROPOSTA = new HashSet<PropostaDTO>();
        }
        public int? RLI_ID { get; set; }
        public string RLI_DESCRICAO { get; set; }
        public string RLI_MOTIVO_REJEICAO { get; set; }
        public Nullable<DateTime> RLT_DATA_ACAO { get; set; }
        public Nullable<DateTime> DATA_EXCLUSAO { get; set; }
        public Nullable<int> REP_ID { get; set; }
        public string USU_LOGIN { get; set; }
        public Nullable<int> RLT_ID { get; set; }
        public Nullable<DateTime> RLI_DATA_CADASTRO { get; set; }
        public Nullable<int> CLI_ID { get; set; }

        public virtual ClienteDto CLIENTES { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual RegistroLiberacaoTipoDTO REGISTRO_LIBERACAO_TIPO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual RepresentanteDTO REPRESENTANTE { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<PropostaItemDTO> PROPOSTA_ITEM { get; set; }


        /// <summary>
        /// Não existe essa propriedade ou relacionamento no banco. É apenas uma propriedade de atalho.
        /// </summary>
        public PropostaItemDTO PropostaItem { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<RegistroLiberacaoItemDTO> REGISTRO_LIBERACAO_ITEM { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<PropostaDTO> PROPOSTA { get; set; }

    }
}
