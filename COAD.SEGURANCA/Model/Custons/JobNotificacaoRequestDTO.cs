using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.SEGURANCA.Model.Custons
{
    public class JobNotificacaoRequestDTO
    {
        public int? jobID { get; set; }
        public int? codRef { get; set; }
        public string codRefStr { get; set; }
        public string descricao { get; set; }
        public string usuario { get; set; }
        public int? repId { get; set; }
    }
}
