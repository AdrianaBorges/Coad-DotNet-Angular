using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Comparators
{
    public class ClienteTelefoneComparator : IEqualityComparer<ClienteTelefoneDTO>
    {

        public bool Equals(ClienteTelefoneDTO x, ClienteTelefoneDTO y)
        {
            if (x != null && y != null)
            {

                return (x.CLI_TEL_ID.Equals(y.CLI_TEL_ID));
            }

            return false;
        }

        public int GetHashCode(ClienteTelefoneDTO obj)
        {
            return (obj.CLI_TEL_ID != null) ? obj.CLI_TEL_ID.GetHashCode() : 0;
        }
    }
}
