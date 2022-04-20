using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.FonteDadosTemplate
{
    public class DetalhesPropostasEmAtrasoDTO
    {
        public DetalhesPropostasEmAtrasoDTO()
        {
            this.RelatorioPropostaEmAtraso = new List<ItemRelatorioPropostaEmAtrasoDTO>();
        }

        public int? QtdPropostasAtrasadas {
            get {
                int count = 0;
                if (RelatorioPropostaEmAtraso != null)
                    count = RelatorioPropostaEmAtraso.Count;
                return count;
            }
        }

        public DateTime? DataDeChecagem { get; set; }
        public string HoraDeChecagem
        {
            get
            {
                if (DataDeChecagem != null)
                    return string.Format("{0:hh:mm:ss}", DataDeChecagem);
                return null;
            }
        }

        public string HostName { get; set; }
        public IList<ItemRelatorioPropostaEmAtrasoDTO> RelatorioPropostaEmAtraso { get; set; }
        public string Ambiente { get; set; }
    }
}
