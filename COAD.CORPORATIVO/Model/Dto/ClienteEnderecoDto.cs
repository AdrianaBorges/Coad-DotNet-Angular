using Coad.GenericCrud.Validations;
using GenericCrud.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{

    [EnderecoMunicipioValidator(ErrorMessage = "O Município selecionado não pertence a UF deste endereço")]
    public class ClienteEnderecoDto
    {
        public ClienteEnderecoDto()
        {
            //this.CLIENTES_ENDERECO_HISTORICO = new HashSet<CLIENTES_ENDERECO_HISTORICO>();
            validacaoTotal = true;
        }
    
        public int? CLI_ID { get; set; }

        /// <summary>
        /// Esse atributo não existe no banco. Serve como flag para indicar se haverá uma validação total dos campos
        /// </summary>
        public bool? validacaoTotal {get; set;}

        [Required(ErrorMessage = "Selecione o tipo de endereço")]
        public int? END_TIPO { get; set; }

        [TextValidator(ErrorMessage = "O Complemento não é válido. Verifique se existe algum caracter especial e remova-o.")]
        [RequiredIf("Validar", true,ErrorMessage = "Digite o endereço")]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "O Logradouro deve possuir no mínimo 2 e no máximo 60 caracteres")]
        public string END_LOGRADOURO { get; set; }

        [TextValidator(ErrorMessage = "O Complemento não é válido. Verifique se existe algum caracter especial e remova-o.")]
        [StringLength(60, MinimumLength = 1, ErrorMessage = "O Complemento deve possuir no máximo 60 caracteres")]
        public string END_COMPLEMENTO { get; set; }

        [RequiredIf("Validar", true, ErrorMessage = "Digite o número do endereço")]
        [MaxLength(6, ErrorMessage = "O número deve ter no máximo 6 dígitos")]
        public string END_NUMERO { get; set; }

        [RequiredIf("Validar", true, ErrorMessage = "Digite o bairro")]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "O Bairro deve possuir no máximo 60 caracteres")]
        public string END_BAIRRO { get; set; }

        [RequiredIf("Validar", true, ErrorMessage = "Digite o cep")]
        [MaxLength(8, ErrorMessage = "O Cep deve conter no máximo 8 caracteres")]
        [MinLength(6, ErrorMessage = "O Cep deve possuir no mínimo 6 caracteres")]
        public string END_CEP { get; set; }
        public string IBGE_COD_COMPLETO { get; set; }

        public string END_MUNICIPIO { get; set; }

        [RequiredIf("Validar", true,ErrorMessage = "Digite o municipio")]
        public Nullable<int> MUN_ID { get; set; }
        public Nullable<int> TIPO_LOG_ID { get; set; }
        public string ASN_NUM_ASSINATURA { get; set; }
        public Nullable<System.DateTime> DATA_CADASTRO { get; set; }
        public Nullable<System.DateTime> DATA_ALTERA { get; set; }
        public string USU_LOGIN { get; set; }
        [Required(ErrorMessage = "Selecione UF")]
        public string END_UF { get; set; }
        public string TIPO_COMP_ID { get; set; }
        public string TIPO_LOGRADOURO { get; set; }

        public virtual AssinaturaDTO ASSINATURA { get; set; }
        public virtual ClienteDto CLIENTES { get; set; }
        //public virtual ICollection<CLIENTES_ENDERECO_HISTORICO> CLIENTES_ENDERECO_HISTORICO { get; set; }
        public virtual IbgeMunicipioDTO IBGE_MUNICIPIO { get; set; }
        public virtual MunicipioDTO MUNICIPIO { get; set; }
        public virtual TipoEnderecoDTO TIPO_ENDERECO { get; set; }
        public bool Excluir { get; set; }
        
        public bool Validar
        {
            get
            {
                return (validacaoTotal == true);
            }
        }

    }
}
