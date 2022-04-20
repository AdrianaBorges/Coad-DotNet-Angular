using COAD.FISCAL.Model.Integracoes.Interfaces;
using COAD.FISCAL.Model.Servicos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Model.Integracoes
{
    public class NFeRejeitadaContext
    {
        public INFeLote Lote { get; set; }
        public INFeLoteItem LoteItem { get; set; }
    }
}
