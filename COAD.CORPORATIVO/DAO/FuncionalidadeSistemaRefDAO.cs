using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Contexto;
using Coad.GenericCrud.Repositorios.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Repositorios.Base;

namespace COAD.CORPORATIVO.DAO
{
    public class FuncionalidadeSistemaRefDAO : DAOAdapter<FUNCIONALIDADE_SISTEMA_REF, FuncionalidadeSistemaRefDTO, int>
    {
        public COADCORPContext db { get { return GetDb<COADCORPContext>(); } set { } }

        public FuncionalidadeSistemaRefDAO()
        {
            db = GetDb<COADCORPContext>();
        }


    }
}
