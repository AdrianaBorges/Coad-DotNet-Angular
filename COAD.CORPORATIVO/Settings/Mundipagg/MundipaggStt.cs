using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Settings.Mundipagg
{
    public class MundipaggStt
    {
        public ApiStt Api { get; set; }
        public CoadPagStt CoadPag { get; set; }
        public string SecretKey { get; set; }
        public string PublicKey { get; set; }

        public string GetUriVersionOneCustomers()
        {
            return string.Concat(this.Api.EndPoint,this.Api.Customers);
        }

    }
}
