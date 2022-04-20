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
    public class AreasDAO : AbstractGenericDao<AREAS, AreasCorpDTO,int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public AreasDAO()
        {
            db = GetDb<COADCORPEntities>(false);
        }

        public AreasCorpDTO ObterAreasPorNome(string areaStr)
        {
            var query = (from are in db.AREAS 
                         where are.AREA_NOME.Trim().ToLower() == areaStr.Trim().ToLower() 
                         select are).FirstOrDefault();

            return ToDTO(query);
        }
    }
}
