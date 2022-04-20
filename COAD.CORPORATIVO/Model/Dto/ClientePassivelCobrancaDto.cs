using System;
using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using GenericCrud.Models.Interfaces;

namespace COAD.CORPORATIVO.Model.Dto
{
    [Mapping(typeof(CLIENTE_PASSIVEL_COBRANCA))]
    public partial class ClientePassivelCobrancaDTO : IPrototype<ClientePassivelCobrancaDTO>
    {
        #region Atributos

        public int ID { get; set; }
        public string ASN_NUM_ASSINATURA { get; set; }
        public string CTR_NUM_CONTRATO { get; set; }
        public string PAR_NUM_PARCELA { get; set; }
        public string CLI_NOME { get; set; }
        public Nullable<System.DateTime> PAR_DATA_VENCTO { get; set; }
        public Nullable<decimal> PAR_VLR_PARCELA { get; set; }
        public string ASN_A_C { get; set; }
        public string BAN_ID { get; set; }
        public Nullable<int> CLI_ID { get; set; }
        public Nullable<int> DIAS_ATRASO { get; set; }
        public string USU_LOGIN { get; set; }
        public string PAR_SITUACAO { get; set; }
        public Nullable<System.DateTime> DATA_ALTERA { get; set; }
        public string USU_LIBERACAO { get; set; }
        public Nullable<System.DateTime> CTR_DATA_FIM_VIGENCIA { get; set; }
        public Nullable<bool> PAR_PRIMEIRA_PARCELA { get; set; }
        
        #endregion

        public virtual ParcelasDTO PARCELAS { get; set; }
        
        public ClientePassivelCobrancaDTO Clone()
        {
            var novo = new ClientePassivelCobrancaDTO()
            {
                ID = this.ID,
                ASN_NUM_ASSINATURA = this.ASN_NUM_ASSINATURA,
                CTR_NUM_CONTRATO = this.CTR_NUM_CONTRATO,
                PAR_NUM_PARCELA = this.PAR_NUM_PARCELA,
                CLI_NOME = this.CLI_NOME,
                PAR_DATA_VENCTO = this.PAR_DATA_VENCTO,
                PAR_VLR_PARCELA = this.PAR_VLR_PARCELA,
                ASN_A_C = this.ASN_A_C,
                BAN_ID = this.BAN_ID,
                CLI_ID = this.CLI_ID,
                DIAS_ATRASO = this.DIAS_ATRASO,
                USU_LOGIN = this.USU_LOGIN,
                PAR_SITUACAO = this.PAR_SITUACAO,
                DATA_ALTERA = this.DATA_ALTERA,
                USU_LIBERACAO = this.USU_LIBERACAO,
                CTR_DATA_FIM_VIGENCIA = this.CTR_DATA_FIM_VIGENCIA,
                PAR_PRIMEIRA_PARCELA = this.PAR_PRIMEIRA_PARCELA,
                PARCELAS = this.PARCELAS

            };

            return novo;
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}
