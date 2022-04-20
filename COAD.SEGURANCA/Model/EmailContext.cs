using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.SEGURANCA.Model
{
    public class EmailContext
    {
        public int? CodFila { get; set; }
        public string Email { get; set; }
        public string Assunto { get; set; }
        public int? RepId { get; set; }
        public string Usuario { get; set; }
        public int? ContextId { get; set; }
        public string ContextIdStr { get; set; }
    }
}
