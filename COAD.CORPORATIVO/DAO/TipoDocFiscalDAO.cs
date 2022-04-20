using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.DAO
{

    public class TipoDocFiscalDAO : RepositorioCorp<TIPO_DOC_FISCAL>
    {
        public List<TIPO_DOC_FISCAL> BuscarTodos()
        {
            List<TIPO_DOC_FISCAL> _tipodocfical = (from t in db.TIPO_DOC_FISCAL
                                                 select t).ToList();

            return _tipodocfical;
        }

    }

}
