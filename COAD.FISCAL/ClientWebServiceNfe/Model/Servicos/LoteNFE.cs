using COAD.FISCAL.Model.Servicos.Enumerados;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model.Servicos
{
    [Serializable]
    [XmlRoot(ElementName = "enviNFe", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class LoteNFE
    {
        public LoteNFE()
        {
            this.NFe = new List<NotaFiscal>();
        }

        [StringLength(4, MinimumLength = 1, ErrorMessage = "A versão devem conter 4 dígitos")]
        [RegularExpression(@"\d{1,2}.\d{2}", ErrorMessage = "O formato da versão deve ser [N.00]")]
        [Required(ErrorMessage = "O campo versão é obrigatório")]
        [XmlAttribute]
        public string versao { get; set; }

        [Range(1, 999999999999999, ErrorMessage = "O código do Lote deve ser um número entre 1 e 999999999999999")]
        [Required(ErrorMessage = "Infome o Código do Lote")]
        public int? idLote { get; set; }

        public IndicacaoDeSincroniaEnum indSinc { get; set; } 

        [MaxLength(50, ErrorMessage = "Não é possível enviar mais de 50 notas por lote.")]
        [MinLength(1, ErrorMessage = "Informe pelo menos 1 nota fiscal")]
        [XmlElement]
        public List<NotaFiscal> NFe { get; set; }
    }
}
