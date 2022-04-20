using GenericCrud.Models.Enumerados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Models{
    public class NotifyHandleResult
    {
        public NotifyHandleResult()
        {
            MsgItens = new HashSet<NotifyHandlerResultItem>();
        }

        public JobStatusEnum JobStatus { get; set; }
        public ICollection<NotifyHandlerResultItem> MsgItens { get; set; }
        public string ErroMsg { get; set; }
    }
}

