using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.SEGURANCA.Repositorios.Base;
using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using System.Data.Objects.SqlClient;
using System.Data.Objects;
using COAD.SEGURANCA.Repositorios.Contexto;
using COAD.SEGURANCA.Model;


namespace COAD.CORPORATIVO.DAO
{
    public class HistoricoExecucaoDAO : AbstractGenericDao<HISTORICO_EXECUCAO, HistoricoExecucaoDTO, int>
    {
        public COADSYSEntities db { get { return GetDb<COADSYSEntities>(); } set { } }

        public HistoricoExecucaoDAO()
        {
            SetProfileName("coadsys");
            db = GetDb<COADSYSEntities>();
        }
    }
}
