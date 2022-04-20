using GenericCrud.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class CancelamentoDTO : AlteracaoStatusDTO, ICloneable, IPrototype<CancelamentoDTO>
    {
        public CancelamentoDTO()
        {
            Itens = new HashSet<CancelamentoItemDTO>(); 
        }

        public bool EnviaEmail { get; set; }
        public string MengagemEmailAssCanc { get; set; }
        public ICollection<CancelamentoItemDTO> Itens { get; set; }

        object ICloneable.Clone()
        {
            return Clone();
        }

        public new CancelamentoDTO Clone()
        {
            var cancelamento = new CancelamentoDTO()
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
                ValorPago = this.ValorPago,
                EnviaEmail = this.EnviaEmail,
                Itens = this.Itens,
                MengagemEmailAssCanc = this.MengagemEmailAssCanc
            };

            return cancelamento;
        }
    }
}