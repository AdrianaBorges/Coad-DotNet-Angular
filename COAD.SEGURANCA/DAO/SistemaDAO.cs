using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Contexto;
using COAD.SEGURANCA.Model;
using Coad.GenericCrud.Repositorios.Base;
using GenericCrud.Config.DataAttributes;

namespace COAD.SEGURANCA.DAO
{
    [DAOConfig("coadsys")]
    public class SistemaDAO : DAOAdapter<SISTEMA, SistemaModel, string>
    {

        public COADSYSEntities db { get { return GetDb<COADSYSEntities>(); } set { } }
        
        public SistemaDAO()
        {
            db = GetDb<COADSYSEntities>(false);
        }

        public SISTEMA Buscar(string _sis_id)
        {
            var _perfil = (from s in db.SISTEMA
                           where s.SIS_ID == _sis_id
                           select s).First();

            return _perfil;
        }

        public List<SISTEMA> Listar()
        {
            List<SISTEMA> _sistema = (from s in db.SISTEMA
                                    select s).ToList();

            return _sistema;
        }
    }
}
