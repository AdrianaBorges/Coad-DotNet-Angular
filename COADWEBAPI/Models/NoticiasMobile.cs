using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace COADWEBAPI.Models
{
    [DataContract]
    public class NoticiasMobile
    {
        [DataMember]
        public int id { get; set; }
        [DataMember]
        public int id_prod { get; set; }
        [DataMember]
        public int id_tipo { get; set; }
        [DataMember]
        public string texto { get; set; }
        [DataMember]
        public string verbete { get; set; }
        [DataMember]
        public string grupo { get; set; }
        [DataMember]
        public string subverbete { get; set; }
        [DataMember]
        public string data_cadastro { get; set; }

    }
}