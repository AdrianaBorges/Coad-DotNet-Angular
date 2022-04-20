using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PAGAMENTO.API.Model
{
    public class RespostaEnvio
    {
        public string ID { get; set; }
        public string Status { get; set; }
        public string Token { get; set; }

        public RespostaEnvio()
        {

        }
       
        public RespostaEnvio(string ID, string Status, string Token)
        {
            this.ID = ID;
            this.Status = Status;
            this.Token = Token;
        }

        public bool IsSuccess()
        {
            if (String.IsNullOrWhiteSpace(Status))
            {
                return (Status.Equals("Sucesso"));
            }
            return false;
        }

    }
}
