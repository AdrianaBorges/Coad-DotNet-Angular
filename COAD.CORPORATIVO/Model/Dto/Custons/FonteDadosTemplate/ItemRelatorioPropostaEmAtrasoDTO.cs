using GenericCrud.Excel.Metadatas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.FonteDadosTemplate
{
    public class ItemRelatorioPropostaEmAtrasoDTO
    {
        [ExcelColumn(Name = "Código da Proposta")]
        public int? CodProposta { get; set; }

        [ExcelColumn(Name = "Código do Item de Proposta")]
        public int? CodItemProposta { get; set; }

        [ExcelColumn(Name = "Produto")]
        public string Produto { get; set; }

        [ExcelColumn(Name = "Representante")]
        public string Representante { get; set; }

        [ExcelIgnore]
        public int CodRepresentante { get; set; }

        [ExcelColumn(Name = "Data de Vencimento")]
        public DateTime? DataVencimento { get; set; }

        [ExcelColumn(Name = "Status")]
        public string Status { get; set; }

        [ExcelColumn(Name = "Código da Proposta")]
        public string TipoDeProposta { get; set; }

        [ExcelColumn(Name = "Assinatura")]
        public string Assinatura { get; set; }

        [ExcelColumn(Name = "Código do Cliente")]
        public int? CodCliente { get; set; }

        [ExcelColumn(Name = "Nome do Cliente")]
        public string NomeCliente { get; set; }

        [ExcelColumn(Name = "CPF/CNPJ")]
        public string CpfCnpj { get; set; }

        [ExcelColumn(Name = "Tipo de Pagamento")]
        public string TipoPagamento { get; set; }

        [ExcelColumn(Name = "Valor de Entrada")]
        public decimal? Entrada { get; set; }

        [ExcelColumn(Name = "Quantidade de Parcelas")]
        public int? QtdParcela { get; set; }

        [ExcelColumn(Name = "Valor de Parcelas")]
        public decimal? ValorParcela { get; set; }

    }
}
