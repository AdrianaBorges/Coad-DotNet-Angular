using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace COAD.CORPORATIVO.Model.Dto.Custons.Pesquisas
{
    public class PesquisaPropostaDTO
    {
        public string nomeCliente { get; set; }
        public int? CLI_ID { get; set; }
        public int? PST_ID { get; set; }
        public string assinatura { get; set; }
        public string cpfCnpj { get; set; }
        public DateTime? dataInicial { get; set; }
        public DateTime? dataFinal { get; set; }
        public DateTime? dataPagamento { get; set; }
        public DateTime? dataVencimentoInicial { get; set; }
        public DateTime? dataVencimentoFinal { get; set; }
        public int? REP_ID { get; set; }
        public int? RG_ID { get; set; }
        public int? UEN_ID { get; set; }
        public int pagina { get; set; }
        public int registrosPorPagina { get; set; }
        public int? TPP_ID { get; set; }
        public int? TPG_ID { get; set; }
        public int? PRO_ID { get; set; }
        public int? PRT_ID { get; set; }
        public int? PPI_ID { get; set; }
        public int? TNE_ID { get; set; }

        public PesquisaPropostaDTO()
        {
            registrosPorPagina = 15;
            pagina = 1;
        }
    }
}