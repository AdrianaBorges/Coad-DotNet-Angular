using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Extensions;

namespace COAD.CORPORATIVO.DAO
{
    public class URADAO : DAOAdapter<URA, UraDTO>
    {
public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public URADAO()
        {
            this.db = GetDb<COADCORPEntities>();
        }

        public Pagina<CONFIG_URA_vw> ListarConfigUra(string _ura_id, int numpagina = 1, int linhas = 10)
        {
            IList<CONFIG_URA_vw> query = db.CONFIG_URA_vw.Where(x => x.URA_ID == _ura_id).ToList();

            return query.Paginar(numpagina, linhas);
        }

    }
}
