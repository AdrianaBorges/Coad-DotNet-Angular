using Coad.GenericCrud.Dao.Base;
using COAD.CORPORATIVO.LEGADO.Model.Dto;
using COAD.CORPORATIVO.LEGADO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.LEGADO.Dao
{
    public class ParamDAO : AbstractGenericDao<PARAM, ParamDTO, int>
    {        
        public corporativo2Entities db { get { return GetDb<corporativo2Entities>(); } set { } }
        
        public ParamDAO()
        {
            SetProfileName("corp_old");
            db = GetDb<corporativo2Entities>();
        }

        public ParamDTO GetParam()
        {
            var param = GetDbSet().FirstOrDefault();
            return ToDTO(param);
        }        
    }
}
