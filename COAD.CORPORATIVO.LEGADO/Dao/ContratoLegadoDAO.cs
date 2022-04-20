using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.CORPORATIVO.LEGADO.Model.Dto;
using COAD.CORPORATIVO.LEGADO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.LEGADO.Dao
{
    public class ContratoLegadoDAO : AbstractGenericDao<CONTRATOS, ContratoLegadoDTO, string>
    {
        public corporativo2Entities db { get { return GetDb<corporativo2Entities>(); } set { } }

        public ContratoLegadoDAO()
        {
            SetProfileName("corp_old");
            db = GetDb<corporativo2Entities>();
        }   
    }
}
