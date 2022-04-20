using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(Source = typeof(UF))]
    public class UFDTO
    {
        public UFDTO()
        {
            this.CADASTRO_GRATUITO = new HashSet<CadastroGratuitoDTO>();
            this.CARTEIRA = new HashSet<CarteiraDTO>();
            this.CFOP_ICMS = new HashSet<CFOPIcmsDTO>();
            this.CFOP_ICMS1 = new HashSet<CFOPIcmsDTO>();
         //   this.CONTRATOS = new HashSet<CONTRATOS>();
            this.MUNICIPIO = new HashSet<MunicipioDTO>();
            this.NOTA_FISCAL = new HashSet<NotaFiscalDTO>();
            this.REPRESENTANTE = new HashSet<RepresentanteDTO>();
            this.URA = new HashSet<UraDTO>();
        }
    
        public string UF_SIGLA { get; set; }
        public string UF_DESCRICAO { get; set; }
        public Nullable<bool> UF_VALIDA { get; set; }
        public Nullable<int> RG_ID { get; set; }
        public string UF_COD { get; set; }

        /// <summary>
        /// Esse campo é um espelho do campo uf_sigla
        /// </summary>
        public virtual string UF 
        { 
            get 
            {
                return UF_SIGLA;                
            } 
            set
            {
                UF_SIGLA = value;
            } 
        }

        [IgnoreMemberMapping(MappingDirection.Both)]        
        public virtual ICollection<CadastroGratuitoDTO> CADASTRO_GRATUITO { get; set; }

        [IgnoreMemberMapping(MappingDirection.SourceToDestiny)]
        public virtual ICollection<CarteiraDTO> CARTEIRA { get; set; }

        [IgnoreMemberMapping(MappingDirection.SourceToDestiny)]
        public virtual ICollection<CFOPIcmsDTO> CFOP_ICMS { get; set; }

        [IgnoreMemberMapping(MappingDirection.SourceToDestiny)]
        public virtual ICollection<CFOPIcmsDTO> CFOP_ICMS1 { get; set; }
     //   public virtual ICollection<ContratosDTO> CONTRATOS { get; set; }

        [IgnoreMemberMapping(MappingDirection.SourceToDestiny)]
        public virtual ICollection<MunicipioDTO> MUNICIPIO { get; set; }

        [IgnoreMemberMapping(MappingDirection.SourceToDestiny)]
        public virtual ICollection<NotaFiscalDTO> NOTA_FISCAL { get; set; }

        [IgnoreMemberMapping(MappingDirection.SourceToDestiny)]
        public virtual ICollection<RepresentanteDTO> REPRESENTANTE { get; set; }

        [IgnoreMemberMapping(MappingDirection.SourceToDestiny)]
        public virtual ICollection<UraDTO> URA { get; set; }

        [IgnoreMemberMapping(MappingDirection.DestinyToSource)]
        public virtual RegiaoDTO REGIAO { get; set; }

        [IgnoreMemberMapping(MappingDirection.Both)]
        public virtual ICollection<PropostaItemComprovanteDTO> PROPOSTA_ITEM_COMPROVANTE { get; set; }
        
    }
}
