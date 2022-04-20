using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.LEGADO.Dao;
using COAD.CORPORATIVO.LEGADO.Model;
using COAD.CORPORATIVO.LEGADO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.LEGADO.Service
{
    [ServiceConfig("CODIGO")]
    public class clienteLegSRV : GenericService<CLIENTES, clienteLegDTO, string>
    {
        private clienteLegDAO _dao = new clienteLegDAO();

        public clienteLegSRV()
        {
            Dao = _dao;
        }

    }
}
