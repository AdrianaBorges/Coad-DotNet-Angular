using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(Source = typeof(CONTA_REF))]
    public partial class ContaRefDTO
    {
        public ContaRefDTO()
        {
            this.PARCELAS = new HashSet<ParcelasDTO>();
            this.CNAB_ARQUIVOS = new HashSet<CnabArquivosDTO>();
            this.CONFIG_ALOCACAO_CONTA = new HashSet<ConfigAlocacaoContaDTO>();
            this.PARCELA_ALOCADA = new HashSet<ParcelaAlocadaDTO>();
            this.PARCELAS_REMESSA = new HashSet<ParcelasRemessaDTO>();
        }

        public Nullable<int> CTA_ID { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<ParcelasDTO> PARCELAS { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<CnabArquivosDTO> CNAB_ARQUIVOS { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<ConfigAlocacaoContaDTO> CONFIG_ALOCACAO_CONTA { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<ParcelaAlocadaDTO> PARCELA_ALOCADA { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<ParcelasRemessaDTO> PARCELAS_REMESSA { get; set; }
    }
}
