using COAD.CORPORATIVO.LEGADO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace COAD.CORPORATIVO.LEGADO.Model.Dto
{
    public class ParcelasLegadoUpdateDTO
    {
        [Required]
        public string CONTRATO { get; set; }
        [Required]
        public string LETRA { get; set; }
        [Required]
        public string CD { get; set; }
        [Required]
        public string BCO_ALOC { get; set; }
        [Required]
        public string DT_ALOC { get; set; }
        [Required]
        public string DT_EMISSAO_BLQ { get; set; }
        [Required]
        public string ALOC_BANCO { get; set; }
        [Required]
        public string CART_ALOC { get; set; }
        [Required]
        public string CART_ALOC_2 { get; set; }
        [Required]
        public string NOSSO_NUMERO { get; set; }
        [Required]
        public string CEDENTE { get; set; }
        [Required]
        public string DT_PAGTO { get; set; }
        [Required]
        public string VLR_PAGO { get; set; }
    }
}
