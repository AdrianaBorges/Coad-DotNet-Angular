using COAD.FISCAL.Model.Integracoes.Interfaces;
using COAD.FISCAL.Model.Servicos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Model.Integracoes
{
    public class NFeDevolucaoContext
    {
        public INFeLote Lote { get; set; }
        public INFeLoteItem LoteItem { get; set; }
        public string ChaveNota { get; set; }
    }
}
