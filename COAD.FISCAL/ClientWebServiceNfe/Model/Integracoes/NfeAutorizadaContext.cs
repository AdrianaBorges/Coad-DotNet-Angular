using COAD.FISCAL.Model.Integracoes.Interfaces;
using COAD.FISCAL.Model.Servicos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Model.Integracoes
{
    public class NFeAutorizadaContext
    {
        public INFeLote Lote { get; set; }
        public INFeLoteItem LoteItem { get; set; }
        public string fileName { get; set; }
        public string filePath { get; set; }
        public byte[] bytesNFe { get; set; }
        public NFeProcessada proNFe { get; set; }
    }
}
