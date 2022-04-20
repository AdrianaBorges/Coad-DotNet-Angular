using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Service.Base;

namespace COAD.CORPORATIVO.DAO
{
    public class UraLogDAO : DAOAdapter<URA_LOG, UraLogDTO>
    {
public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public UraLogDAO()
        {
            this.db = GetDb<COADCORPEntities>();
        }

        //public Pagina<URA_COAD> BuscarClientesUra(string _ura_id, string _asn_id, int numpagina = 1, int linhas = 10)
        //{
        //    IList<URA_COAD> query = db.URA_COAD.Where(x => x.URAID == _ura_id).ToList();

        //    if (_asn_id != null)
        //        query = query.Where(x => x.CODIGO == _asn_id).ToList();

        //    return query.Paginar(numpagina, linhas);
        //}
        //public Pagina<CONFIG_URA_vw> ListarConfigUra(string _ura_id, int numpagina = 1, int linhas = 10)
        //{
        //    IList<CONFIG_URA_vw> query = db.CONFIG_URA_vw.Where(x => x.URA_ID == _ura_id).ToList();

        //    return query.Paginar(numpagina, linhas);
        //}
    }
}

