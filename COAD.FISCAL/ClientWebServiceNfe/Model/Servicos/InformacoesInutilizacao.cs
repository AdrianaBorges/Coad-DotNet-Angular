using COAD.FISCAL.Model.DTOCriptografia;
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
    public class InformacoesInutilizacao
    {

        /// <summary>
        /// Atributo
        /// </summary>
        [Required(ErrorMessage = "O campo Id é obrigatório")]
        [RegularExpression(@"ID[0-9]{43}", ErrorMessage = "Id da está fora do padrão especificado pela Sefaz. Formato 'ID[0-9]{43}'")]
        [XmlAttribute]
        public string Id { get; set; }

        public TipoAmbienteEnum tpAmb { get; set; }
        public string xServ { get; set; }
        public string cUF { get; set; }
        public string ano { get; set; }
        public string CNPJ { get; set; }

        /// <summary>
        /// Modelo do documento (55 ou 65)
        /// </summary>
        public string mod { get; set; }
        public int? serie { get; set; }
        public int? nNFIni { get; set; }
        public int? nNFFin { get; set; }

        /// <summary>
        /// Informar a justificativa do pedido de inutilização
        /// </summary>
        public string xJust { get; set; }
        public SignatureDTO Signature { get; set; }

    }
}
