using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GatewayApiClient.DataContracts;
using GatewayApiClient.DataContracts.EnumTypes;
using GenericCrud.Models.Interfaces;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class AlteracaoStatusDTO : ICloneable, IPrototype<AlteracaoStatusDTO>
    {
        public int? RLI_ID { get; set; }
        public int? IPE_ID { get; set; }
        public int? PED_CRM_ID { get; set; }
        public int? PPI_ID { get; set; }
        public string MOTIVO_ALTERACAO { get; set; }
        public string OBSERVACOES { get; set; }
        public string USU_LOGIN { get; set; }
        public int? REP_ID { get; set; }
        public int? CLI_ID { get; set; }
        public Guid? ChaveTransacaoBoleto { get; set; }
        public Guid? ChaveTransacaoCartao { get; set; } 
        public string UrlBoleto { get; set; }
        public string CodigoBarras { get; set; }
        public CreditCardTransactionStatusEnum? StatusTransacaoCC { get; set; }
        public bool Gateway { get; set; }

        public BoletoTransactionStatusEnum? StatusTransacaoBoleto { get; set; }
        public decimal? ValorPago { get; set; }
        public string StatusTransacao { get; set; }

        object ICloneable.Clone()
        {
            return Clone();
        }

        public AlteracaoStatusDTO Clone()
        {

            AlteracaoStatusDTO alteracao = new AlteracaoStatusDTO()
            {
                ChaveTransacaoBoleto = this.ChaveTransacaoBoleto,
                ChaveTransacaoCartao = this.ChaveTransacaoCartao,
                CLI_ID = this.CLI_ID,
                CodigoBarras = this.CodigoBarras,
                Gateway = this.Gateway,
                IPE_ID = this.IPE_ID,
                MOTIVO_ALTERACAO = this.MOTIVO_ALTERACAO,
                OBSERVACOES = this.OBSERVACOES,
                PED_CRM_ID = this.PED_CRM_ID,
                PPI_ID = this.PPI_ID,
                REP_ID = this.REP_ID,
                StatusTransacao = this.StatusTransacao,
                StatusTransacaoBoleto = this.StatusTransacaoBoleto,
                StatusTransacaoCC = this.StatusTransacaoCC,
                UrlBoleto = this.UrlBoleto,
                USU_LOGIN = this.USU_LOGIN,
                ValorPago = this.ValorPago
            };

            return alteracao;
        }
    }
}
