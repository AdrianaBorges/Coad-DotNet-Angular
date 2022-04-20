using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Model.NFSe
{
    public class DadosServicoRps
    {
        [Required(ErrorMessage = "Informe os valores de Serviço")]
        public ValoresRps Valores { get; set; }

        [Required(ErrorMessage = "Informe o ItemListaServico Código do Serviço")]
        public string ItemListaServico { get; set; }
        public string CodigoCnae { get; set; }

        [Required(ErrorMessage = "Informe o Código de Tributacao do Municipio ")]
        public string CodigoTributacaoMunicipio { get; set; }

        [Required(ErrorMessage = "Informe a Discriminacao")]
        public string Discriminacao { get; set; }

        [Required(ErrorMessage = "Informe o Código de Município")]

        public int? CodigoMunicipio { get; set; }

    }
}
