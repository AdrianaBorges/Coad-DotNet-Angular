using Coad.GenericCrud.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Prospect
{
    public class ProspectDTO
    {
        public ProspectDTO()
        {
            this.PROSPECTS_TELEFONE = new HashSet<ProspectsTelefoneDTO>();
        }

        [DisplayName("Código")]
        public int? ID { get; set; }

        [DisplayName("Nome")]
        [MaxLength(35, ErrorMessage = "O nome do prospect deve ter no máximo 35 caracteres")]
        [Required(ErrorMessage = "Digite o nome do prospect")]
        public string NOME { get; set; }

        [MaxLength(35, ErrorMessage = "Aos cuidados deve ter no máximo 35 caracteres")]
        [DisplayName("Aos cuidados")]
        public string A_C { get; set; }


        public string TIPO { get; set; }

        [MaxLength(50, ErrorMessage = "O logradouro deve ter no máximo 50 caracteres")]
        [DisplayName("Logradouro")]
        public string LOGRAD { get; set; }

        [MaxLength(6, ErrorMessage = "O número deve ter no máximo 6 caracteres")]
        [DisplayName("Número")]
        public string NUMERO { get; set; }

        public string TIPO_COMPL { get; set; }

        [MaxLength(15, ErrorMessage = "O complemento deve ter no máximo 15 caracteres")]
        [DisplayName("Complemento")]
        public string COMPL { get; set; }
        public string TIPO_COMPL2 { get; set; }
        public string COMPL2 { get; set; }
        public string TIPO_COMPL3 { get; set; }
        public string COMPL3 { get; set; }

        [DisplayName("Bairro")]
        public string BAIRRO { get; set; }

        [DisplayName("Município")]
        public string MUNIC { get; set; }

        [DisplayName("UF")]
        public string UF { get; set; }

        [MaxLength(9, ErrorMessage = "O CEP deve ter 9 caracteres")]
        [MinLength(8, ErrorMessage = "O CEP deve ter 9 caracteres")]
        [DisplayName("CEP")]
        public string CEP { get; set; }
        public string DDD_TEL { get; set; }
        public string TELEFONE { get; set; }
        public string DDD_FAX { get; set; }
        public string FAX { get; set; }

        [MaxLength(50, ErrorMessage = "O E-Mail deve ter no máximo 50 caracteres")]
        [DisplayName("E-Mail")]
        public string E_MAIL { get; set; }

        [MaxLength(30, ErrorMessage = "O cargo deve ter no máximo 30 caracteres")]
        [DisplayName("Cargo")]
        public string CARGO { get; set; }

        [MaxLength(30, ErrorMessage = "A profissão deve ter no máximo 30 caracteres")]
        [DisplayName("Profissão")]
        public string PROF { get; set; }

        public string IDENTIFICACAO { get; set; }
        public Nullable<System.DateTime> DATA_CADASTRO { get; set; }
        public string cep_status { get; set; }

        [DisplayName("CNPJ/CPF")]
        [MinLength(11, ErrorMessage = "O CNPJ/CPF deve ter no mínimo 11 caracteres")]
        [MaxLength(14, ErrorMessage = "O CNPJ/CPF deve ter no máximo 14 caracteres")]
        public string CNPJ_CPF { get; set; }

        public Nullable<System.DateTime> DATA_ATUALIZACAO { get; set; }
        public Nullable<System.DateTime> DATA_EXCLUSAO { get; set; }
        
        [RequiredList(1, ErrorMessage = "Adicione pelo menos um telefone")]
        public virtual ICollection<ProspectsTelefoneDTO> PROSPECTS_TELEFONE { get; set; }
    }
}
