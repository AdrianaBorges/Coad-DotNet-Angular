using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Model.NFSe
{
    public class MensagemRetornoLoteNfse
    {
        public IdentificacaoRps IdentificacaoRps { get; set; }
        public int? Codigo { get; set; }
        public string Mensagem { get; set; }
    }
}
