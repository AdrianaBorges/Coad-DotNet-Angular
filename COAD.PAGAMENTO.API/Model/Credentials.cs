using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PAGAMENTO.API.Model
{
    public class Credentials
    {
        public string TOKEN { get; set; }
        public string KEY { get; set; }
        
        public Credentials(string TOKEN, string KEY)
        {
            this.TOKEN = TOKEN;
            this.KEY = KEY;
        }
    }
}
