using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Coad.GenericCrud.Validations;
using COAD.CORPORATIVO.Validations;
using GenericCrud.Validations;

namespace COAD.CORPORATIVO.Model.Dto.Custons.ClienteProspect
{
    public class ClienteProspectDTO
    {
        public ClienteProspectDTO()
        {
            Emails = new HashSet<ClienteProspectEmailDTO>();
            Telefones = new HashSet<ClienteProspectTelefoneDTO>();
            CarteirasCliente = new HashSet<CarteiraClienteProspectDTO>();
        }

        public bool OrigemCliente { get; set; }

        public int? ClienteId { get; set; }
        public string Codigo { get; set; }

        [Required(ErrorMessage = "Informe o nome do prospect")]
        [TextValidator(ErrorMessage = "O nome não pode possuir caracteres especiais e espaço no início e fim do texto.")]
        [StringLength(60, MinimumLength = 2, ErrorMessage = "O Nome deve possuir no máximo 60 caracteres")]
        public string Nome {get; set;}

        [CpfOrCnpjValidator("EhPessoaJuridica", ErrorMessage = "CPF/CNPJ inválido.")]
        [StringLengthIf(11, "TipoClienteId", 2, ErrorMessage = "O CPF deve conter 11 caracteres")]
        [StringLengthIf(14, "TipoClienteId", 1, 3, 4, ErrorMessage = "O CNPJ deve conter 14 caracteres")]
        public string CNPJ_CPF { get; set; }

        [TextValidator(ErrorMessage = "O campo Aos Cuidados não pode possuir caracteres especiais e espaço no início e fim do texto.")]
        public string A_C { get; set; }
        public int? TipoClienteId { get; set; }


        [StringLength(13, MinimumLength = 2, ErrorMessage = "O campo [IE] Inscrição estadual deve possuir no mínimo 2 e no máximo 13 caracteres")]
        [TextValidator(ErrorMessage = "O Campo [IE] Inscrição estadual possui caracteres especiais.")]
        [ClienteIsentoDeInscricaoAttribute]
        public string InscricaoEstadual { get; set; }

        #region Isenta cliente de inscrição estadual
            public bool EhIsentoDeInscricaoEstadual { get; set; }

        #endregion

        [RequiredIfNotNullOrEmpty("carIdStr", ErrorMessage = "Selecione um código de carteira válido")]
        public string CAR_ID { get; set; }
        public string carIdStr { get; set; }
        public bool ForcarSalvamento { get; set; }
        public ICollection<ClienteProspectEmailDTO> Emails { get; set; }
        public ICollection<ClienteProspectTelefoneDTO> Telefones { get; set; }

        [EnderecoMunicipioValidator(ErrorMessage = "O Município selecionado não pertence a UF deste endereço")]
        public ClienteProspectEnderecoDTO EnderecoFaturamento { get; set; }

        [EnderecoMunicipioValidator(ErrorMessage = "O Município selecionado não pertence a UF deste endereço")]
        public ClienteProspectEnderecoDTO EnderecoEntrega { get; set; }

        public ICollection<CarteiraClienteProspectDTO> CarteirasCliente { get; set; }

        public bool? EhPessoaJuridica()
        {
            if(TipoClienteId != null)
            {
                return (TipoClienteId != 2);
            }
            else
            {
                if (!string.IsNullOrEmpty(CNPJ_CPF))
                {
                    if (CNPJ_CPF.Length == 14)
                        return true;
                    else return false;    
                }
                return null;
            }
        }

        public bool RequerInscricaoEstadual
        {
            get
            {
                if (!Convert.ToBoolean(EhPessoaJuridica()))
                    return false;

                if (!EhIsentoDeInscricaoEstadual)
                    return true;
                else return false;
            }
        }

    }
}
