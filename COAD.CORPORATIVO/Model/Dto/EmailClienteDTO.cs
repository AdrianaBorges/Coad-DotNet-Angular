using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace COAD.CORPORATIVO.Model.DTO
{
    public class EmailClienteDTO
    {
        public int PPE_ID { get; set; }
        
        [Required(ErrorMessage = "Digite o email")]
        [EmailAddress(ErrorMessage = "O email informado é inválido")]
        public string PPE_EMAIL { get; set; }

         [Required(ErrorMessage = "Defina o tipo de pedido")]
        public Nullable<int> OPC_ID { get; set; }

        public int? EMP_ID { get; set; }
        public int? PRE_PEDIDO_ID { get; set; }
        public int PED_NUM_PEDIDO { get; set; }

        //public virtual OPCAO_ATENDIMENTO OPCAO_ATENDIMENTO { get; set; }

        //public string emailCliente.email { get; set; }
        //public string emailCliente.idtipo { get; set; }
        
        //public string clientes { get; set; }
        //public string idemail { get; set; } 

        ////[Required(ErrorMessage = "Digite o email")]
        ////[EmailAddress(ErrorMessage =  "O email informado é inválido")]
        ////public string email { get; set; }

        ////[Required(ErrorMessage = "Defina o tipo de pedido")]
        ////public int? idtipo { get; set; }
        
        //public string tipo { get; set; }
    }
}