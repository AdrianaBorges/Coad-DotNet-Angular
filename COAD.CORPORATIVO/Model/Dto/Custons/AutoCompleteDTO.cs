using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class AutoCompleteDTO
    {
        public string value { get; set; }
        public string label { get; set; }
    }

    public class AutoCompleteDTO<T>
    {
        public T value { get; set; }
        public string label { get; set; }
    }
}
