using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Models
{
    public class BatchStatus
    {
        public bool HasParcent { get; set; }
        public bool IsRunning { get; set; }
        public string BatchStepName { get; set; }
        public int TotalItens { get; set; }
        public int ProcessedItens { get; set; }

        public int? Progress { 
            get {
                if (TotalItens > 0 && HasParcent && IsRunning)
                {
                    int total = ((ProcessedItens * 100) / TotalItens);
                    return total;
                }
                return null;
            } 
        
        }
    }
}
