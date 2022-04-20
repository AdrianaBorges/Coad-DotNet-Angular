using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons.EnvioEmail
{
    public class EnvioEmailDTO
    {
        public int? PPI_ID { get; set; }
        public int? REP_ID { get; set; }
        public string USU_LOGIN { get; set; }

        public List<EmailDTO> LstEmail { get; set; }

        public string EmailsConcat()
        {
            var count = LstEmail.Count;
            if(LstEmail != null && count > 0)
            {
                StringBuilder sb = new StringBuilder();

                int index = 0;
                foreach(var email in LstEmail)
                {
                    sb.Append(email.Email);

                    if (index < (count - 1))
                        sb.Append(", ");
                    index++;
                }
                return sb.ToString();
            }
            return null;
        }
    }
}
