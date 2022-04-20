using Coad.GenericCrud.Dao.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Prospect;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.DAO
{
    public class ClienteTelefoneDAO : AbstractGenericDao<CLIENTES_TELEFONE, ClienteTelefoneDTO, int>
    {
        public ClienteTelefoneDAO()
        {
            
        }

        public IList<ClienteTelefoneDTO> FindAllByCliId(int CLI_ID)
        {
            var query = GetDbSet().Where(x => x.CLI_ID == CLI_ID);
            return ToDTO(query);
        }

        
    }
}
