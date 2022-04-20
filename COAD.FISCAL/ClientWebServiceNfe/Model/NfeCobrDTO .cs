using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model
{
    public class NfeCobrDTO
    {

        /// <summary>
        /// Faturamentos
        /// </summary>
        [XmlElement("fat")]
        public NfeFatDTO[] NFat { get; set; }

        /// <summary>
        /// Faturamentos
        /// </summary>
        [XmlElement("dup")]
        public NfeDupDTO[] NDup { get; set; }


    }
}
