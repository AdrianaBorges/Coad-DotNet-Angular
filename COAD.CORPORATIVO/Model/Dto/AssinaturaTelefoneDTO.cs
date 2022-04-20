using GenericCrud.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public class AssinaturaTelefoneDTO : IPrototype<AssinaturaTelefoneDTO>
    {
        public int? ATE_ID { get; set; }
        public string ASN_NUM_ASSINATURA { get; set; }

        [Required(ErrorMessage = "Digite o telefone")]
        [StringLength(11, MinimumLength = 6, ErrorMessage = "O Telefone deve possuir no mínimo 6 e no máximo 11 caracteres")]
        public string ATE_TELEFONE { get; set; }

        [Required(ErrorMessage = "Selecione o tipo de telefone")]
        public int? TIPO_TEL_ID { get; set; }
        public Nullable<int> OPC_ID { get; set; }


        [StringLength(3, MinimumLength = 1, ErrorMessage = "O DDD deve possuir no máximo 3 caracteres")]
        public string ATE_DDD { get; set; }
        public Nullable<int> CLI_ID { get; set; }
        public string ATE_CONTATO { get; set; }
        public string ATE_RAMAL { get; set; }
        public Nullable<int> TEL_ID_LEGADO { get; set; }
        public Nullable<DateTime> DATA_ALTERA { get; set; }
        public string USU_LOGIN { get; set; }
        public Boolean ALTERADO { get; set; }

        public virtual AssinaturaDTO ASSINATURA { get; set; }
        public virtual OpcoesAtendimentoDTO OPCAO_ATENDIMENTO { get; set; }
        public virtual TipoTelefoneDTO TIPO_TELEFONE { get; set; }
        public virtual ClienteDto CLIENTES { get; set; }

        public AssinaturaTelefoneDTO Clone()
        {
            AssinaturaTelefoneDTO tel = new AssinaturaTelefoneDTO()
            {
                ALTERADO = this.ALTERADO,
                ASN_NUM_ASSINATURA = this.ASN_NUM_ASSINATURA,
                ASSINATURA = this.ASSINATURA,
                ATE_CONTATO = this.ATE_CONTATO,
                ATE_DDD = this.ATE_DDD,
                ATE_ID = this.ATE_ID,
                ATE_RAMAL = this.ATE_RAMAL,
                ATE_TELEFONE = this.ATE_TELEFONE,
                CLI_ID = this.CLI_ID,
                CLIENTES = this.CLIENTES,
                DATA_ALTERA = this.DATA_ALTERA,
                OPC_ID = this.OPC_ID,
                OPCAO_ATENDIMENTO = this.OPCAO_ATENDIMENTO,
                TEL_ID_LEGADO = this.TEL_ID_LEGADO,
                TIPO_TEL_ID = this.TIPO_TEL_ID,
                TIPO_TELEFONE = this.TIPO_TELEFONE,
                USU_LOGIN = this.USU_LOGIN
            };

            return tel;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}
