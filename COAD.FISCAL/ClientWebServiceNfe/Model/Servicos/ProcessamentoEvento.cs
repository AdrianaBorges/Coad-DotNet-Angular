using COAD.FISCAL.Model.Servicos.Retornos;
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
    [XmlRoot(ElementName = "procEventoNFe", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class ProcessamentoEvento
    {
        public ProcessamentoEvento()
        {
            Eventos = new List<Evento>();
            EventoRetornos = new List<EventoRetorno>();
        }
        /// <summary>
        /// Atributo
        /// </summary>
        [StringLength(4, MinimumLength = 1, ErrorMessage = "A versão devem conter 4 dígitos")]
        [RegularExpression(@"\d{1,2}.\d{2}", ErrorMessage = "O formato da versão deve ser [N.00]")]
        [Required(ErrorMessage = "O campo versão é obrigatório")]
        [XmlAttribute("versao")]
        public string Versao { get; set; }

        [XmlElement("evento")]
        public List<Evento> Eventos { get; set; }

        [XmlElement("retEvento")]
        public List<EventoRetorno> EventoRetornos { get; set; }
    }
}
