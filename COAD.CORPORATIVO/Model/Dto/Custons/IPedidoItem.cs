using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public interface IPedidoItem
    {
        bool? AcessosConcedidos { get; set; }
        int? PstId { get; set; }
        int? CmpId { get; set; }
        string NomeTipo { get; set; }
        decimal? ValorUnitario { get;}
        int? Qtd { get; }
        decimal? ValorEntradaBruto { get; }
        decimal? ValorEntrada { get; }
        decimal? ValorParcelaBruto { get; }
        decimal? ValorDaParcela { get; }
        int? QtdParcelas { get; }
        ICollection<PropostaItemComprovanteDTO> Comprovantes { get; set; }
        ICollection<PedidoParticipanteDTO> Participantes { get; set; }
        ProdutoComposicaoDTO ProdutoComposicao { get; set; }
    }
}
