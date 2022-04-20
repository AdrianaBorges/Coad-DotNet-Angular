using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.SEGURANCA.Model;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(Source = typeof(REGIAO))]
    public class RegiaoDTO
    {
        public RegiaoDTO()
        {
            this.REGIAO_TABELA_PRECO = new HashSet<RegiaoTabelaPrecoDTO>();
            this.REPRESENTANTE = new HashSet<RepresentanteDTO>();
            this.PEDIDO_CRM = new HashSet<PedidoCRMDTO>();
            this.CONFIG_IMPOSTO_REGIAO = new HashSet<ConfigImpostoRegiaoDTO>();
            this.MUNICIPIO = new HashSet<MunicipioDTO>();
            this.UF = new HashSet<UFDTO>();
            this.CONTRATOS = new HashSet<ContratoDTO>();
            this.PROPOSTA = new HashSet<PropostaDTO>();
            this.IMPORTACAO_SUSPECT = new HashSet<ImportacaoSuspectDTO>();
            this.IMPORTACAO_RESULTADO_RODIZIO = new HashSet<ImportacaoResultadoRodizioDTO>();
        }
    
        public int? RG_ID { get; set; }

        public string RG_DESCRICAO { get; set; }
        public Nullable<int> EMP_ID { get; set; }
        public Nullable<bool> RG_FRANQUIA { get; set; }
        public Nullable<int> UEN_ID { get; set; }
        
        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<FilaCadastroDTO> FILA_CADASTRO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<RegiaoTabelaPrecoDTO> REGIAO_TABELA_PRECO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<FranquiaDTO> FRANQUIA { get; set; }
        
        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<RepresentanteDTO> REPRESENTANTE { get; set; }
        
        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<CarteiraDTO> CARTEIRA { get; set; }
        
        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual EmpresaRefDTO EMPRESA_REF { get; set; }
        
        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual UENDTO UEN { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<PedidoCRMDTO> PEDIDO_CRM { get; set; }
        
        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<ConfigImpostoRegiaoDTO> CONFIG_IMPOSTO_REGIAO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<MunicipioDTO> MUNICIPIO { get; set; }
        
        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<UFDTO> UF { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<ContratoDTO> CONTRATOS { get; set; }

        public virtual EmpresaModel EMPRESA { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<PropostaDTO> PROPOSTA { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<ImportacaoSuspectDTO> IMPORTACAO_SUSPECT { get; set; }


        [IgnoreMemberMapping(Direction = MappingDirection.Both)]
        public virtual ICollection<ImportacaoResultadoRodizioDTO> IMPORTACAO_RESULTADO_RODIZIO { get; set; }

    }
}
