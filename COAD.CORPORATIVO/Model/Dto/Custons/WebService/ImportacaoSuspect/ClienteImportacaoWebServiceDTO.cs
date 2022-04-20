using GenericCrud.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.WebService.ImportacaoSuspect
{
    [DataContract]
    public class ClienteImportacaoWebServiceDTO
    {

        [TextValidator(ErrorMessage = "O Nome não pode possuir caracteres especiais.")]
        [MaxLength(100, ErrorMessage = "O Nome deve ter no máximo 100 caracteres.")]
        [Required(ErrorMessage = "Informe o Nome")]
        [DataMember]
        public string Nome { get; set; }

        [NumberValidator(ErrorMessage = "O CPF/CNPJ deve possuir apenas caracteres numéricos.")]
        [MaxLength(14, ErrorMessage = "O CPF/CNPJ deve ter no máximo 14 caracteres.")]
        [DataMember]
        public string CPF_CNPJ { get; set; }

        [TextValidator(ErrorMessage = "O Contato não pode possuir caracteres especiais.")]
        [MaxLength(90, ErrorMessage = "O Contato deve ter no máximo 90 caracteres.")]
        [DataMember]
        public string Contato { get; set; }

        [NumberValidator(ErrorMessage = "O Telefone deve possuir apenas caracteres numéricos.")]
        [MaxLength(22, ErrorMessage = "O DDD + Telefone deve ter no máximo 22 caracteres.")]
        [Required(ErrorMessage = "Informe o Telefone com DDD")]
        [DataMember]
        public string Telefone { get; set; }

        [NumberValidator(ErrorMessage = "O Fax deve possuir apenas caracteres numéricos.")]
        [MaxLength(22, ErrorMessage = "O Fax deve ter no máximo 22 caracteres.")]
        [DataMember]
        public string Fax { get; set; }

        [NumberValidator(ErrorMessage = "O Celular deve possuir apenas caracteres numéricos.")]
        [MaxLength(22, ErrorMessage = "O Celular deve ter no máximo 22 caracteres.")]
        [DataMember]
        public string Celular { get; set; }

        [EmailAddress(ErrorMessage = "Formato de E-Mail Inválido")]
        [MaxLength(100, ErrorMessage = "O E-Mail deve ter no máximo 100 caracteres.")]
        [Required(ErrorMessage = "Informe o E-Mail")]
        [DataMember]
        public string Email { get; set; }

        [MaxLength(2, ErrorMessage = "O UF deve ter no máximo 2 caracteres.")]
        [Required(ErrorMessage = "Informe a UF")]
        [DataMember]
        public string UF { get; set; }

        [TextValidator(ErrorMessage = "A cidade não pode possuir caracteres especiais.")]
        [MaxLength(60, ErrorMessage = "A cidade deve ter no máximo 60 caracteres.")]
        [Required(ErrorMessage = "Informe a Cidade")]
        [DataMember]
        public string Cidade { get; set; }

        [TextValidator(ErrorMessage = "O Bairro não pode possuir caracteres especiais.")]
        [MaxLength(60, ErrorMessage = "O Bairro deve ter no máximo 60 caracteres.")]
        [DataMember]
        public string Bairro { get; set; }

        [DataMember]
        public string ComentarioCliente { get; set; }
        
        public ImportacaoSuspectDTO ConverterParaImportacaoSuspect()
        {
            var importacaoSuspect = new ImportacaoSuspectDTO();
            importacaoSuspect.IPS_NOME = this.Nome;
            importacaoSuspect.IPS_TELEFONE = this.Telefone;
            importacaoSuspect.IPS_FAX = this.Fax;
            importacaoSuspect.IPS_BAIRRO = this.Bairro;
            importacaoSuspect.IPS_CELULAR = this.Celular;
            importacaoSuspect.IPS_CIDADE = this.Cidade;
            importacaoSuspect.IPS_COMENTARIO_CLIENTE = this.ComentarioCliente;
            importacaoSuspect.IPS_CONTATO = this.Contato;
            importacaoSuspect.IPS_CPF_CNPJ = this.CPF_CNPJ;
            importacaoSuspect.IPS_EMAIL = this.Email;
            importacaoSuspect.IPS_UF = this.UF;

            return importacaoSuspect;
        }

    }
}
