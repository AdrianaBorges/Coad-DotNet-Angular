using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Custons.ClienteProspect;
using COAD.CORPORATIVO.Service;
using COAD.CORPORATIVO.Validations.Enumerados;
using GenericCrud.Service;
using GenericCrud.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GenericCrud.Validations
{
    public class EmailPedidoPropostaValidatorAttribute : ValidationAttribute
    { 
        public EmailValidacaoEnum Campo { get; set; }
        public EmailPedidoPropostaValidatorAttribute()
        {
            
        }

        public EmailPedidoPropostaValidatorAttribute(EmailValidacaoEnum Campo)
        {
            this.Campo = Campo;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Object instance = validationContext.ObjectInstance;

            if (instance != null && instance is PropostaDTO)
            {
                var pro = instance as PropostaDTO;
                if (pro != null)
                {
                    if (pro.CLI_ID != null)
                    {
                        string email = null;
                        if (Campo == EmailValidacaoEnum.EMAIL_CONTATO)
                            email = pro.PRT_EMAIL_CONTATO;
                        if (Campo == EmailValidacaoEnum.EMAIL_NOTA_FISCAL)
                            email = pro.PRT_EMAIL_NOTA_FISCAL;

                        if (!string.IsNullOrWhiteSpace(email))
                        {
                            var jaPossui = ServiceFactory.RetornarServico<AssinaturaEmailSRV>().ChecarClientePossuiEmailAssinatura(email, pro.CLI_ID);
                            if (!jaPossui)
                            {
                                    var resp = ServiceFactory.RetornarServico<ClienteSRV>().ClienteJaExiste(pro.CLI_ID, null, null, null, new List<AssinaturaEmailDTO>() {

                                    new AssinaturaEmailDTO()
                                    {
                                        AEM_EMAIL = email
                                    }
                                });
                                if (resp.HasDuplication)
                                {
                                    return new ValidationResult(ErrorMessage);
                                }
                            }
                        }
                    }
                }
            }
            else if (instance != null && instance is ClienteProspectEnderecoDTO)
            {
                var ped = instance as PedidoCRMDTO;
                if (ped != null)
                {
                    if (ped.CLI_ID != null)
                    {
                        string email = null;
                        if (Campo == EmailValidacaoEnum.EMAIL_CONTATO)
                            email = ped.PED_CRM_EMAIL_CONTATO;
                        if (Campo == EmailValidacaoEnum.EMAIL_NOTA_FISCAL)
                            email = ped.PED_CRM_EMAIL_NOTA_FISCAL;

                        if (!string.IsNullOrWhiteSpace(email))
                        {
                            var resp = ServiceFactory.RetornarServico<ClienteSRV>().ClienteJaExiste(ped.CLI_ID, null, null, null, new List<AssinaturaEmailDTO>() {

                                new AssinaturaEmailDTO()
                                {
                                    AEM_EMAIL = email
                                }
                            });
                            if (resp.HasDuplication)
                            {
                                return new ValidationResult(ErrorMessage);
                            }
                        }
                    }
                }
            }

            return ValidationResult.Success;
        }
    }
}
