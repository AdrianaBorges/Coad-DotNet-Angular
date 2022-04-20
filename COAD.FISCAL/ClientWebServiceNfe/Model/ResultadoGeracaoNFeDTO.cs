using COAD.FISCAL.Model.Enumerados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Model
{
    public class ResultadoGeracaoNFeDTO
    {
        public int? NumeroDaNotaFiscal { get; set; }
        public string ChaveNotaFiscal { get; set; }
        public TipoQualificacaoNFeEnum TipoQualificacao { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
    }
}
