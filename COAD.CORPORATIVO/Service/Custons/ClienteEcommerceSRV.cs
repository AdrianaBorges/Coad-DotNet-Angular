using COAD.CORPORATIVO.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Service.Custons
{

    public class ClienteEcommerceSRV
    {
        public ClienteSRV clienteSRV { get; set; }


        public ClienteDto ObterClientePorCLIID(int cLI_ID)
        {

            return clienteSRV._dao.ObterClientePorCLIID(cLI_ID);

        }

        public int? ObterTipoPorCLIID(int cLI_ID)
        {

            return clienteSRV._dao.ObterTipoPorCLIID(cLI_ID);

        }

        public ClienteDto CadastrarCliente(ClienteDto cliente)
        {

            return clienteSRV.Save(cliente);

        }

    }

}

