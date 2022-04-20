

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
    public class SequencialNossoNumeroDAO : AbstractGenericDao<SEQUENCIAL_NOSSO_NUMERO, SequencialNossoNumeroDTO, object>
    {
        public CORPORATIVOContext db { get { return GetDb<CORPORATIVOContext>(); } set { } }

        public SequencialNossoNumeroDAO()
        {
            db = GetDb<CORPORATIVOContext>(false);
        }
        
    }
}
