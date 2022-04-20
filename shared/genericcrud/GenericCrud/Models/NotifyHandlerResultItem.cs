using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Models
{
    public class NotifyHandlerResultItem
    {
        public int? CodRef { get; set; }
        public string CodRefStr { get; set; }
        public string Mensagem { get; set; }
        public DateTime? Data { get; set; }
    }
}
