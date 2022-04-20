using COAD.SEGURANCA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class PreviewGeracaoNotaFiscalDTO
    {
        public bool? Valido { get; set; }
        public int? IpeId { get; set; }
        public int? NumeroNota { get; set; }
        public int? EmpId { get; set; }
        public EmpresaModel Empresa { get; set; }
        public DateTime? DataFaturamento { get; set; }
        public int? QtdContratosRetroativosPendentes { get; set; }
        public DateTime? DataUltimoFaturamento { get; set; }

        public string Mensagem { get; set; }
    }
}
