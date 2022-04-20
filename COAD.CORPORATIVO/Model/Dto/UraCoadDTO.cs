using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto
{
    public partial class UraCoadDTO
    {
        public int ID { get; set; }
        public string URAID { get; set; }
        public Nullable<int> VIP { get; set; }
        public Nullable<int> DDD { get; set; }
        public string TELEFONE { get; set; }
        public Nullable<int> SENHA { get; set; }
        public string CODIGO { get; set; }
        public string NOME { get; set; }
        public Nullable<int> PODE { get; set; }
        public Nullable<int> QTE_CONS { get; set; }
        public Nullable<int> ACESSO { get; set; }
        public Nullable<int> QTE_REALIZ { get; set; }
        public string GRUPO { get; set; }

        public virtual UraDTO URA { get; set; }
    }
}
