using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Service
{
    public class TipoDocFiscalSRV: ServiceCorp<TIPO_DOC_FISCAL>
    {
        public List<TIPO_DOC_FISCAL> BuscarTodos()
        {
            return new TipoDocFiscalDAO().BuscarTodos();
        }
    }
}
