using COAD.FISCAL.Model.Enumerados;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model
{
    [Serializable]
    public class NfeInfoTransporteDTO
    {
        /// <summary>
        /// Modalidade do frete
        /// </summary>
        [Required(ErrorMessage = "O objeto modFrete é obrigatório")]    
        [XmlElement("modFrete")]
        public TipoModalidadeTransporteEnum ModalidadeFrete { get; set; }
    }
}
