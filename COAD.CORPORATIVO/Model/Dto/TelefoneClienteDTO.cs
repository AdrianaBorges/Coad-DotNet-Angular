using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace COAD.CORPORATIVO.Model.DTO
{
    public class PrePedidoTelefoneDTO
    {

        public int PPT_ID { get; set; }

        [Required(ErrorMessage = "Digite o telefone")]
        [MaxLength(13, ErrorMessage = "O telefone deve ter no máximo 13 caracteres")]
        public string PPT_TELEFONE { get; set; }

        [Required(ErrorMessage = "Escolhe o tipo de telefone")]
        public string TIPO_TEL_ID { get; set; }

        [Required(ErrorMessage = "Escolhe o setor")]
        public Nullable<int> OPC_ID { get; set; }

        public int? EMP_ID { get; set; }
        public int? PRE_PEDIDO_ID { get; set; }
        public int PED_NUM_PEDIDO { get; set; }

        //public virtual OPCAO_ATENDIMENTO OPCAO_ATENDIMENTO { get; set; }

        //public string clientes { get; set; }
        //public string idtelefone { get; set; }

        
        //public string telefone { get; set; }

        //[Required(ErrorMessage = "Escolhe o tipo de telefone")]
        //public string tipo { get; set; }

        //[Required(ErrorMessage = "Escolhe o setor")]
        //public int? idsetor { get; set; } 
        //public string setor { get; set; }
    }
}