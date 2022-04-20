using Coad.GenericCrud.Dao.Base;
using COAD.CORPORATIVO.LEGADO.Model;
using COAD.CORPORATIVO.LEGADO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.LEGADO.Dao
{
    public class clienteLegDAO : AbstractGenericDao<CLIENTES, clienteLegDTO, string>
    {
        public clienteLegDAO()
        {
            SetProfileName("corp_old");
        }
    }
}
