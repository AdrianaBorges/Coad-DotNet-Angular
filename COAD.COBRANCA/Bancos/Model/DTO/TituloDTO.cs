using System;

namespace COAD.COBRANCA.Bancos.Model.DTO
{
    public class TituloDTO
    {
        public DateTime? DataVencimento { get; set; }
        public string NossoNumero { get; set; }
        public decimal? Valor { get; set; }
    }
}
