using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.SEGURANCA.DAO;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Contexto;
using COAD.SEGURANCA.Model;
using Coad.GenericCrud.Service.Base;
using GenericCrud.Config.DataAttributes;

namespace COAD.SEGURANCA.Service
{
    [ServiceConfig("SIS_ID")]
    public class SistemaSRV : ServiceAdapter<SISTEMA, SistemaModel, string>
    {
        private SistemaDAO _dao = new SistemaDAO();

        public SistemaSRV()
        {
            SetDao(_dao);
        }

        public List<SISTEMA> Listar()
        {
            List<SISTEMA> _sistema = new SistemaDAO().Listar();

            return _sistema;
        }
    }
}
