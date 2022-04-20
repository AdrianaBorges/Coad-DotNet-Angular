using GenericCrud.Models.Interfaces;
using GenericCrud.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public class AssinaturaEmailDTO : IPrototype<AssinaturaEmailDTO>
    {
        public int? AEM_ID { get; set; }
        public string ASN_NUM_ASSINATURA { get; set; }

        [Required(ErrorMessage = "Digite o E-Mail")]
        [EmailAddress(ErrorMessage = "Digite um E-Mail válido")]
        [RegularExpression(@"(.*[^\.\s]$)", ErrorMessage = "Digite um E-Mail válido")]
        [StringLength(60, MinimumLength = 1, ErrorMessage = "O email deve possuir no máximo 60 caracteres")]
        public string AEM_EMAIL { get; set; }

        public Nullable<int> OPC_ID { get; set; }
        public Nullable<int> CLI_ID { get; set; }
        public Nullable<int> EMAIL_ID_LEGADO { get; set; }
        public Nullable<DateTime> DATA_ALTERA { get; set; }
        public string USU_LOGIN { get; set; }
        public Boolean ALTERADO { get; set; }

        public virtual AssinaturaDTO ASSINATURA { get; set; }
        public virtual OpcoesAtendimentoDTO OPCAO_ATENDIMENTO { get; set; }

        public virtual ClienteDto CLIENTES { get; set; }

        public AssinaturaEmailDTO Clone()
        {
            var novo = new AssinaturaEmailDTO()
            {
                AEM_EMAIL = this.AEM_EMAIL,
                AEM_ID = this.AEM_ID,
                ALTERADO = this.ALTERADO,
                ASN_NUM_ASSINATURA = this.ASN_NUM_ASSINATURA,
                ASSINATURA = this.ASSINATURA,
                CLI_ID  = this.CLI_ID,
                CLIENTES = this.CLIENTES,
                DATA_ALTERA = this.DATA_ALTERA,
                EMAIL_ID_LEGADO = this.EMAIL_ID_LEGADO,
                OPC_ID = this.OPC_ID,
                OPCAO_ATENDIMENTO = this.OPCAO_ATENDIMENTO,
                USU_LOGIN = this.USU_LOGIN
            };

            return novo;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}
