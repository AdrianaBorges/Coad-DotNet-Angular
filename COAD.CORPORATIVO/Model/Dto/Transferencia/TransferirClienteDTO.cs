using Coad.GenericCrud.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace COAD.CORPORATIVO.Model.Dto.Transferencia
{
    public class TransferirSuspectDTO
    {
        [Required(ErrorMessage = "O tipo de transferencia é indefinido. Favor reinicie a operação")]
        public string Tipo { get; set; }

        [Required(ErrorMessage = "Nenhuma operadora de origem foi selecionada")]
        public string CAR_ID_ORIGEM { get; set; }

        [RequiredIf("Tipo", "operadora", ErrorMessage = "O operador que receberá os suspects não foi selecionado")]
        public string CAR_ID_DESTINO { get; set; }
        public int? RG_ID { get; set; }


    }
}