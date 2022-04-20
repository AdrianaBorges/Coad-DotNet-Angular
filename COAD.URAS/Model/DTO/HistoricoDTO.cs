using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.URAS.Model.DTO
{
    public class HistoricoDTO
    {
        public string codigo { get; set; }
        public DateTime data { get; set; }
        public int qte_cons { get; set; }
        public int qte_realiz { get; set; }

    }
}
