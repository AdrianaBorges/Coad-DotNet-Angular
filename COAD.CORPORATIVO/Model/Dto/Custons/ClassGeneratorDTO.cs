using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public class ClassGeneratorDTO
    {
        public ClassGeneratorDTO()
        {
            keys = new List<string>();

            
        }

        public string ClassName { get; set; }
        public string EntityName { get; set; }
        public string DTOName { get; set; }
        public string KeyType { get; set; }
        public IList<string> keys { get; set; }
    }
}
