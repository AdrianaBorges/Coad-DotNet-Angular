using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Service.Base;
using COAD.COADGED.DAO;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Repositorios.Contexto;

namespace COAD.COADGED.Service
{
    public class ClientesRefSRV : GenericService<CLIENTES_REF, ClientesRefDTO, int>
    {
        private ClientesRefDAO _dao = new ClientesRefDAO();

        public ClientesRefSRV()
        {
            Dao = _dao;
        }
    }
}
