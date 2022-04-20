using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class ContextoFaturamentoDTO
    {
        public ContextoFaturamentoDTO()
        {
            LstRegistroDeFaturamento = new HashSet<RegistroFaturamentoDTO>();
        }

        public ICollection<RegistroFaturamentoDTO> LstRegistroDeFaturamento { get; set; }
        public ClienteDto cliente { get; set; }
        public virtual PedidoCRMDTO PEDIDO { get; set; }
        public ItemPedidoDTO itemPedido { get; set; }
        public TabelaPrecoDTO tabelaPreco { get; set; }
        public ProdutoComposicaoDTO produto { get; set; }
        public AssinaturaDTO assinatura { get; set; }
        public AssinaturaDTO assinaturaAntiga { get; set; }
        public bool pagamentoSemEntrada { get; set; }
        public PedidoPagamentoDTO entrada { get; set; }
        public PedidoPagamentoDTO pagamentoRestante { get; set; }
        public string USU_LOGIN { get; set; }
        public int? REP_ID_QUE_EXECUTOU_ACAO { get; set; }
        public DateTime? ultimaDataVencimentoGerada { get; set; }
        public bool Cortesia { get; set; }

        public int? EMP_ID { get; set; }
        public int? uenId { get; set; }
        public RegiaoDTO REGIAO { get; set; }
        public DateTime? inicioVigencia { get; set; }
        public DateTime? dataFaturamento { get; set; }
        public string PathNotaFiscal { get; set; }

        public RequisicaoFaturamentoDTO RequisicaoFaturamento { get; set; }   
        
        public RegistroFaturamentoDTO RetornarRegistroFaturamento(int? ipeId)
        {
            var regFat = LstRegistroDeFaturamento.Where(x => x.IPE_ID == ipeId)
                .FirstOrDefault();

            if(regFat == null)
            {
                regFat = new RegistroFaturamentoDTO()
                {
                    IPE_ID = ipeId
                };
                LstRegistroDeFaturamento.Add(regFat);
            }

            return regFat;
        }
    }
}
