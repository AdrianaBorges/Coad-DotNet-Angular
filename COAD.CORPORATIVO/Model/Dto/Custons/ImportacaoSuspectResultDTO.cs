using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class PreviaImportacaoSuspectDTO
    {
        public bool Validado { get; set; }

        public ICollection<ImportacaoSuspectDTO> ListaSuspectsNaoConvertidos { get; set; }
        public ICollection<ImportacaoSuspectDTO> ListaResumo { get; set; }

        public int QuantidadeRegistros { get; set; }
        public int QuantidadeReal { get; set; }
        public int QuantidadeDuplicada { get; set; }
        
        public PreviaImportacaoSuspectDTO()
        {
            this.ListaSuspectsNaoConvertidos = new List<ImportacaoSuspectDTO>();
        }
    }
}
