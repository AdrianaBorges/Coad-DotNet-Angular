using COAD.FISCAL.Model.Integracoes.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Model.Servicos.Retornos
{
    public class ProcessamentoEventoItem
    {
        public INotaFiscal NotaFiscal { get; set; }
        public string NomeArquivo { get; set; }
        public byte[] Arquivo { get; set; }
        public INFeLoteItem LoteItem { get; set; }
    }
}
