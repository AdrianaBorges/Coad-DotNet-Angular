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
    public class AreaInfoMarketingDAO : AbstractGenericDao<AREA_INFO_MARKETING, AreaInfoMarketingDTO,int>
    {

        public IQueryable<AREA_INFO_MARKETING> TemplateAreaInfoMarketing(int MKT_CLI_ID, int AREA_ID)
        {
            var query = GetDbSet().Where(x => x.MKT_CLI_ID == MKT_CLI_ID && x.AREA_ID == AREA_ID);
            return query;

        }

        public AreaInfoMarketingDTO FindAreaInfoMarketing(int MKT_CLI_ID, int AREA_ID)
        {
            var query = TemplateAreaInfoMarketing(MKT_CLI_ID, AREA_ID).FirstOrDefault();
            return ToDTO(query);
        }

        public bool HasAreaInfoMarketing(int MKT_CLI_ID, int AREA_ID)
        {
            var query = TemplateAreaInfoMarketing(MKT_CLI_ID, AREA_ID);

            return (query.Count() > 0);
        }
    }
}
