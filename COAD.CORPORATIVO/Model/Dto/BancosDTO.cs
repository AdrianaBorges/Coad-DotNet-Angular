using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    
    [Mapping(typeof(BANCOS))]
    public partial class BancosDTO
    {
        public BancosDTO()
        {
            this.CHEQUE_DEVOLVIDO = new HashSet<ChequeDevolvidoDTO>();
            this.CNAB_ARQUIVOS = new HashSet<CnabArquivosDTO>();
            this.CNAB_ARQUIVOS_ITEM = new HashSet<CnabArquivosItemDTO>();
            this.CNAB = new HashSet<CnabDTO>();
            this.OCORRENCIA_ERRO = new HashSet<OcorrenciaErroDTO>();
            this.OCORRENCIA_REMESSA = new HashSet<OcorrenciaRemessaDTO>();
            this.OCORRENCIA_RETORNO = new HashSet<OcorrenciaRetornoDTO>();
            this.PARCELA_LIQUIDACAO = new HashSet<ParcelaLiquidacaoDTO>();
            this.PARCELAS = new HashSet<ParcelasDTO>();
            this.PARCELAS_REMESSA = new HashSet<ParcelasRemessaDTO>();
            this.PROPOSTA_ITEM_COMPROVANTE = new HashSet<PropostaItemComprovanteDTO>();
            this.CNAB_CONFIG = new HashSet<CnabConfigDTO>();
            this.SEQUENCIAL_NOSSO_NUMERO = new HashSet<SequencialNossoNumeroDTO>();
        }
    
        public string BAN_NOME { get; set; }
        public string BAN_ID { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<ChequeDevolvidoDTO> CHEQUE_DEVOLVIDO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<CnabArquivosDTO> CNAB_ARQUIVOS { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<CnabArquivosItemDTO> CNAB_ARQUIVOS_ITEM { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<CnabDTO> CNAB { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<OcorrenciaErroDTO> OCORRENCIA_ERRO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<OcorrenciaRemessaDTO> OCORRENCIA_REMESSA { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<OcorrenciaRetornoDTO> OCORRENCIA_RETORNO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<ParcelaLiquidacaoDTO> PARCELA_LIQUIDACAO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<ParcelasDTO> PARCELAS { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<ParcelasRemessaDTO> PARCELAS_REMESSA { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<PropostaItemComprovanteDTO> PROPOSTA_ITEM_COMPROVANTE { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<CnabConfigDTO> CNAB_CONFIG { get; set; }


        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<SequencialNossoNumeroDTO> SEQUENCIAL_NOSSO_NUMERO { get; set; }

    }
}
