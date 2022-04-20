using Coad.GenericCrud.ActionResultTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    [DataContract]
    public class WebServiceResult
    {
        [DataMember]
        public Message Message { get; set; }

        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public long TempoDeExecucao { get; set; }

    }
}
