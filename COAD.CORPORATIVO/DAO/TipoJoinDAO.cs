
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using Coad.GenericCrud.Dao.Base;


namespace COAD.CORPORATIVO.DAO
{
    public class TipoJoinDAO : AbstractGenericDao<TIPO_JOIN, TipoJoinDTO,int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public TipoJoinDAO()
        {
            db = GetDb<COADCORPEntities>(false);
        }

    }
}
