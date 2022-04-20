using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class ItensNfeXmlPedidoResponseDTO
    {
        public ItensNfeXmlPedidoResponseDTO()
        {
            PreviewGeracao = new HashSet<PreviewGeracaoNotaFiscalDTO>();
            LstNfeXml = new HashSet<NfeXmlDTO>();
        }

        public ICollection<NfeXmlDTO> LstNfeXml { get; set; }
        public ICollection<PreviewGeracaoNotaFiscalDTO> PreviewGeracao { get; set; }
        public bool ExistemNotas
        {
            get
            {
                if (LstNfeXml == null)
                    return false;
                if (LstNfeXml.Count() <= 0)
                    return false;
                return true;
            }
            
        }
    }
}
