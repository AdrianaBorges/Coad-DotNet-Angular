using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class CancelamentoItemDTO
    {
        public CancelamentoDTO DadosCancelamentoPai { get; set; }
        public int? ipeId { get; set; }
        public string NomeProduto { get; set; }
        public string NomeCliente { get; set; }
        public decimal? ValorPedido { get; set; }
        public bool EnviarEmailCancAssi { get; set; }
        public bool ExtornarNumeroNotaFiscal { get; set; }
        public bool ValidadoPraExtornarNumNota { get; set; }
        public string MotivoNaoPodeExtornar { get; set; }
        public string Assinatura { get; set; }
        public int? EmpId { get; set; }
        public int? PstId { get; set; }
        public string Email { get; set; }
    }
}