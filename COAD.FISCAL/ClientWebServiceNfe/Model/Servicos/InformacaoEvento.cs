using COAD.FISCAL.Model.Enumerados;
using COAD.FISCAL.Model.Servicos.Enumerados;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.Servicos
{
    [Serializable]
    public class InformacaoEvento
    {
        [Required(ErrorMessage = "O campo Id é obrigatório")]
        [RegularExpression(@"ID[0-9]{54}", ErrorMessage = "Id da está fora do padrão especificado pela Sefaz. Formato 'ID[0-9]{43}'")]
        [XmlAttribute]
        public string Id { get; set; }

        public string cOrgao { get; set; }
        public TipoAmbienteEnum tpAmb { get; set; }
        public string CNPJ { get; set; }
        public string CPF { get; set; }
        public string chNFe { get; set; }
        
        [XmlIgnore]
        public DateTime DataEvento { get; set; }
        public string dhEvento
        {

            get
            {

                if (DataEvento != null)
                {
                    return DataEvento.ToString("yyyy-MM-ddTHH:mm:sszzz");
                }
                return null;
            }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    DataEvento = DateTime.ParseExact(value, "yyyy-MM-ddTHH:mm:sszzz", provider);
                }
            }
        }

        [XmlElement("tpEvento")]
        public TipoEventoEnum TipoEvento { get; set; }
        public int? nSeqEvento { get; set; }
        public string verEvento { get; set; }

        public DetalhamentoEvento detEvento { get; set; }

    }
}
