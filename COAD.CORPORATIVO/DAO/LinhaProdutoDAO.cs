using Coad.GenericCrud.Dao.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.DAO
{
    public class LinhaProdutoDAO : AbstractGenericDao<LINHA_PRODUTO, LinhaProdutoDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public LinhaProdutoDAO()
        {
            db = GetDb<COADCORPEntities>();
        }
    }
}
