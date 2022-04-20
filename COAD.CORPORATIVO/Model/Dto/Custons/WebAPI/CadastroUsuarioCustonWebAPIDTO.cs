using Coad.GenericCrud.Validations;
using GenericCrud.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.WebAPI
{
    public class CadastroUsuarioCustonWebAPIDTO
    {
        public bool empresa { get; set; }
        public bool EhEmpresa()
        {
            return empresa;
        }

        [Required(ErrorMessage = "Informe o nome/razão social")]
        [TextValidator(ErrorMessage = "O Bairro não pode possuir caracteres especiais e espaço no início e fim do texto.")]
        public string nomerazaosocial { get; set; }
        [Required(ErrorMessage = "Informe CPF/CNPJ")]
        [CpfOrCnpjValidator("EhEmpresa", ErrorMessage = "CPF/CNPJ inválido.")]
        [StringLengthIf(11, "empresa", false, ErrorMessage = "O CPF deve conter 11 caracteres")]
        [StringLengthIf(14, "empresa", true, ErrorMessage = "O CNPJ deve conter 14 caracteres")]
        public string cpfcnpj { get; set; }
        [Required(ErrorMessage = "Informe o E-Mail")]
        [EmailAddress(ErrorMessage = "E-mail em formato inválido.")]
        public string email { get; set; }
        [Required(ErrorMessage = "Informe o telefone")]
        [NumberValidator(ErrorMessage = "O campo telefone deve conter apenas numerais.")]
        [Range(1000000000,99999999999,ErrorMessage = "Número de telefone inválido")]
        public string telefone { get; set; }
        [Required(ErrorMessage = "Informe o estado")]
        public string estado { get; set; }  
        [NumberValidator(ErrorMessage = "O campo cpfcnpj responsável deve conter apenas numerais.")]
        public string cpfcnpjresponsavel { get; set; }
        [Required(ErrorMessage = "Informe o número do produto.")]
        [NumberValidator(ErrorMessage = "O campo prod_composicao responsável deve conter apenas numerais.")]
        public int produto { get; set; }
        [NumberValidator(ErrorMessage = "O campo prod_composicao responsável deve conter apenas numerais.")]
        public int prod_composicao { get; set; }
        public string area_de_interesse { get; set; }
        public string nomeresponsavel { get; set; }
        public string senha { get; set; }
        public string assinatura { get; set; }

    }
}
