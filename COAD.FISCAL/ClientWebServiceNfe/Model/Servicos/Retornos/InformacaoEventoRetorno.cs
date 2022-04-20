using COAD.FISCAL.Model.Enumerados;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.Servicos
{
    [Serializable]
    public class InformacaoEventoRetorno
    {
        [Required(ErrorMessage = "O campo Id é obrigatório")]
        [RegularExpression(@"ID[0-9]{54}", ErrorMessage = "Id da está fora do padrão especificado pela Sefaz. Formato 'ID[0-9]{43}'")]
        [XmlAttribute]
        public string Id { get; set; }

        public TipoAmbienteEnum tpAmb { get; set; }
        public string verAplic { get; set; }
        public string cOrgao { get; set; }
        public int? cStat { get; set; }
        public string xMotivo { get; set; }
        public string chNFe { get; set; }
        public string tpEvento { get; set; }
        public string xEvento { get; set; }
        public int? nSeqEvento { get; set; }
        public string CNPJDest { get; set; }
        public string CPFDest { get; set; }
        public string emailDest { get; set; }
        public DateTime? dhRegEvento { get; set; }

    }
}
