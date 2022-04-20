using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.SEGURANCA.DAO;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Contexto;
using Coad.GenericCrud.Service.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using GenericCrud.Service.Base;
using System.Transactions;
using GenericCrud.Config.DataAttributes;

namespace COAD.SEGURANCA.Service
{
    [ServiceConfig("FSI_ID")]
    public class FuncionalidadeSistemaSRV : ServiceAdapter<FUNCIONALIDADE_SISTEMA, FuncionalidadeSistemaDTO, int>
    {
        private FuncionalidadeSistemaDAO _dao;

        public FuncionalidadeSistemaSRV()
        {
            _dao = new FuncionalidadeSistemaDAO();
            SetDao(_dao);
        }
    }
}
