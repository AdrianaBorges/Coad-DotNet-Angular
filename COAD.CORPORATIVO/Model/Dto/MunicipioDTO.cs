using Coad.GenericCrud.Validations;
using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(Source = typeof(MUNICIPIO))]
    public class MunicipioDTO
    {
        public MunicipioDTO()
        {
            this.CEP_LOGRADOURO = new HashSet<CepLogradouroDTO>();
            this.CLIENTES_ENDERECO = new HashSet<ClienteEnderecoDto>();
            this.FORNECEDOR = new HashSet<FornecedorDTO>();
            this.TRANSPORTADOR = new HashSet<TransportadorDTO>();
        }
    
        public int MUN_ID { get; set; }
        public string MUN_DESCRICAO { get; set; }
        public string MUN_TIPO { get; set; }

        //[Required(ErrorMessage = "Digite a UF")]
        public string UF { get; set; }
        public string IBGE_COD_COMPLETO { get; set; }
        public string MUN_CEP { get; set; }
        public Nullable<int> RG_ID { get; set; }
    

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<ClienteEnderecoDto> CLIENTES_ENDERECO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<FornecedorDTO> FORNECEDOR { get; set; }
        
        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual UFDTO UF1 { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<TransportadorDTO> TRANSPORTADOR { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<CepLogradouroDTO> CEP_LOGRADOURO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual RegiaoDTO REGIAO { get; set; }
    }
}
