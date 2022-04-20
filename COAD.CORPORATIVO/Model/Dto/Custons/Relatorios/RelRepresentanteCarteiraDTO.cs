using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.Relatorios
{
    public class RelRepresentanteCarteiraDTO
    {
        public RelRepresentanteCarteiraDTO()
        {
            this.LISTA = new HashSet<RelRepresentanteCarteiraDTO>();
        }
        
        public int? REP_ID_SUPERVISOR { get; set; }
        public string REP_NOME_SUPERVISOR { get; set; }
        public int? REP_ID { get; set; }
        public string REP_NOME { get; set; }
        public string CAR_ID { get; set; }
        public string REP_EMAIL { get; set; }
        public string REP_RAMAL { get; set; }
        public string REP_DDD_TELEFONE { get; set; }
        public string REP_TELEFONE { get; set; }
        public virtual ICollection<RelRepresentanteCarteiraDTO> LISTA { get; set; }
    }
}
