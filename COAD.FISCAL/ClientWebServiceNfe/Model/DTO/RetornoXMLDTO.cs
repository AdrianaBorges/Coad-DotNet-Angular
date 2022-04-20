using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Model.DTO
{
    public class RetornoXMLDTO
    {
        public int CodXML { get; set; }
        public string ChaveNota { get; set; }
        public string PathNota { get; set; }
        public string CodPedido { get; set; }
        public Nullable<int> NFX_TIPO { get; set; }
        public Nullable<int> NumeroNota { get; set; }
        public Nullable<System.DateTime> DataEmiNota { get; set; }
    }
}
