using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class ContextoContratoFaturamentoDTO : ContextoFaturamentoDTO
    {
        public ContextoContratoFaturamentoDTO(ContextoFaturamentoDTO contexto)
        {
            if (contexto != null)
            {
                this.assinatura = contexto.assinatura;
                this.entrada = contexto.entrada;
                this.inicioVigencia = contexto.inicioVigencia;
                this.itemPedido = contexto.itemPedido;
                this.pagamentoRestante = contexto.pagamentoRestante;
                this.pagamentoSemEntrada = contexto.pagamentoSemEntrada;
                this.PEDIDO = contexto.PEDIDO;
                this.produto = contexto.produto;
                this.REGIAO = contexto.REGIAO;
                this.tabelaPreco = contexto.tabelaPreco;
                this.uenId = contexto.uenId;
            }
        }

        public ContratoDTO contrato { get; set; }
        public DadosDeContratoDTO dadosDoContrato { get; set; }
        public IList<ParcelasDTO> parcelas { get; set; }

    }
}
