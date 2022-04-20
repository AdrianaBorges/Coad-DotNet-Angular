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
    [XmlRoot(ElementName = "consReciNFe", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class ConsultaLote
    {

        [StringLength(4, MinimumLength = 1, ErrorMessage = "A versão devem conter 4 dígitos")]
        [RegularExpression(@"\d{1,2}.\d{2}", ErrorMessage = "O formato da versão deve ser [N.00]")]
        [Required(ErrorMessage = "O campo versão é obrigatório")]
        [XmlAttribute]
        public string versao { get; set; }

        [Required(ErrorMessage = "Informe o campo tpAmb (Tipo de ambiente)")]
        public TipoAmbienteEnum tpAmb { get; set; }

        [Required(ErrorMessage = "Preencha o campo nRec (Número de recibo).")]
        public string nRec { get; set; }

    }
}
