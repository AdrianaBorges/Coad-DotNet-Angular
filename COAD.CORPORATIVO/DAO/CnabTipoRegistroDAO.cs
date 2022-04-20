

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Dao.Base;
using COAD.CORPORATIVO.Repository.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;

namespace COAD.CORPORATIVO.DAO
{
    public class CnabTipoRegistroDAO : AbstractGenericDao<CNAB_TIPO_REGISTRO, CnabTipoRegistroDTO, String>
    {
        public CORPORATIVOContext db { get { return GetDb<CORPORATIVOContext>(); } set { } }

        public CnabTipoRegistroDAO()
        {
            db = GetDb<CORPORATIVOContext>(false);
        }

        
    }
}
