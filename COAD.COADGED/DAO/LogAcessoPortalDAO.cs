using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Dao.Base;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Repositorios.Contexto;

namespace COAD.COADGED.DAO
{
    public class LogAcessoPortalDAO : AbstractGenericDao<LOG_ACESSO_PORTAL, LogAcessoPortalDTO, int>
    {
        public COADGEDEntities db { get { return GetDb<COADGEDEntities>(); } set { } }

        public LogAcessoPortalDAO() : base()
        {
            SetProfileName("GED");
            db = GetDb<COADGEDEntities>(false);
        }

        public List<int?> RetornarNoticiasMaisLidas()
        {
            var query = GetDbSet().GroupBy(n => n.NOT_ID_PORTAL).
                Select(n => new { name = n.Key, count = n.Count() }).
                OrderByDescending(n => n.count).Select(n => n.name).
                Where(x => x.Value != 0).Take(10).ToList();

            return query;
            //return ToDTO(query); 
        }

    }
}
