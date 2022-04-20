using COAD.FISCAL.Model.Enumerados;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.Servicos.Retornos
{
    [Serializable]
    [XmlRoot(ElementName = "retEnvEvento", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class RecepcaoEventoRetorno
    {
        public RecepcaoEventoRetorno()
        {
            this.retEvento = new List<EventoRetorno>();
        }

        [StringLength(4, MinimumLength = 1, ErrorMessage = "A versão devem conter 4 dígitos")]
        [RegularExpression(@"\d{1,2}.\d{2}", ErrorMessage = "O formato da versão deve ser [N.00]")]
        [Required(ErrorMessage = "O campo versão é obrigatório")]
        [XmlAttribute]
        public string versao { get; set; }

        [Range(1, 999999999999999, ErrorMessage = "O código do Lote deve ser um número entre 1 e 999999999999999")]
        [Required(ErrorMessage = "Infome o Código do Lote")]
        public int? idLote { get; set; }

        public TipoAmbienteEnum tpAmb { get; set; }
        public string verAplic { get; set; }
        public string cOrgao { get; set; }
        public int? cStat { get; set; }
        public string xMotivo { get; set; }

        [XmlElement]
        public List<EventoRetorno> retEvento { get; set; }

    }
}
