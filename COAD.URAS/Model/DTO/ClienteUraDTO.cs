using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.URAS.Model.DTO
{
    public class ClienteUraDTO
    {
        public ClienteUraDTO()
        {
            this.historico = new HashSet<HistoricoDTO>();
        }

        public string uraid { get; set; }
        public int id {get;set;}
        public string vip { get; set; }
        public string ddd { get; set; }
        public string telefone { get; set; }
        public string senha { get; set; }
        public string codigo {get;set;}
        public string nome {get;set;}
        public string pode { get; set; }
        public string qte_cons { get; set; }
        public string acesso { get; set; }
        public string qte_realiz { get; set; }
        public string grupo {get;set;}
        public virtual ICollection<HistoricoDTO> historico { get; set; }

    }
}
