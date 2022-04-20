using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public partial class UraProdutoAreaDTO
    {
        public string URA_ID { get; set; }
        public int PRO_ID { get; set; }
        public string UF_SIGLA_ACESSO { get; set; }
        public int ACO_ID { get; set; }
        public Nullable<bool> UPA_ATIVO { get; set; }

        public virtual AreaConsultoriaDTO AREA_CONSULTORIA { get; set; }
        public virtual UraConfigDTO URA_CONFIG { get; set; }
        

    }
}
