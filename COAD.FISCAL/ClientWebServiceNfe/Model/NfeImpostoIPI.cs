using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace COAD.FISCAL.Model
{
    public class NfeImpostoIPI 
    {
        /// <summary>
        /// Tabela a ser criada pela RFB, informar 999 enquanto a tabela não for criada
        /// </summary>
        [Required(ErrorMessage = "O campo cEnq (Código de enquadramento) é obrigatório")]
        [XmlElement("cEnq")]
        public string CodigoEnquadramento { get; set; }
        
        [XmlElement("IPINT")]
        public NfeImpostoIPIGrupoNaoTributado IPINaoTributado { get; set; }
    }
}
