using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PORTAL.Model.DTO.Uras
{
    public partial class coadDTO
    {
        public int id { get; set; }
        public Nullable<int> vip { get; set; }
        public Nullable<int> ddd { get; set; }
        public Nullable<long> telefone { get; set; }
        public Nullable<long> senha { get; set; }
        public string codigo { get; set; }
        public string nome { get; set; }
        public Nullable<int> pode { get; set; }
        public Nullable<int> qte_cons { get; set; }
        public Nullable<int> acesso { get; set; }
        public Nullable<int> qte_realiz { get; set; }
        public string grupo { get; set; }
    }

}
