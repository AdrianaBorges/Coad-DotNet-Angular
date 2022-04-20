using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(Source = typeof(EMPRESA_REF))]
    public partial class EmpresaRefDTO
    {
        public EmpresaRefDTO()
        {
            this.CONTRATOS = new HashSet<ContratoDTO>();
            this.NOTA_FISCAL = new HashSet<NotaFiscalDTO>();
            this.PARCELAS = new HashSet<ParcelasDTO>();
            this.CARTEIRA_REPRESENTANTE = new HashSet<CarteiraRepresentanteDTO>();
            this.TOTAL_VENDAS_CARTAO = new HashSet<TotalVendasCartaoDTO>();
            this.CARTEIRA = new HashSet<CarteiraDTO>();
            this.REGIAO = new HashSet<RegiaoDTO>();
            this.CNAB = new HashSet<CnabDTO>();
            this.SEQUENCIAL_NOSSO_NUMERO = new HashSet<SequencialNossoNumeroDTO>();
        }

        public int EMP_ID { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ConfigSpedFiscalDTO CONFIG_SPED_FISCAL { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<ContratoDTO> CONTRATOS { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<NotaFiscalDTO> NOTA_FISCAL { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<ParcelasDTO> PARCELAS { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<SpedArquivoDTO> SPED_ARQUIVO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<TotalVendasCartaoDTO> TOTAL_VENDAS_CARTAO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<CarteiraDTO> CARTEIRA { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<RegiaoDTO> REGIAO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<CnabDTO> CNAB { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<CarteiraRepresentanteDTO> CARTEIRA_REPRESENTANTE { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<SequencialNossoNumeroDTO> SEQUENCIAL_NOSSO_NUMERO { get; set; }
    }
}
