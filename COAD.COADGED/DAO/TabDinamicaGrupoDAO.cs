using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.DAO
{

    public class TabDinamicaGrupoDAO : AbstractGenericDao<TAB_DINAMICA_GRUPO, TabDinamicaGrupoDTO, int>
    {
        public COADGEDEntities db { get { return GetDb<COADGEDEntities>(); } set { } } 
        public TabDinamicaGrupoDAO()
            : base()
        {
            SetProfileName( "GED" );

            db = GetDb<COADGEDEntities>(false);
        }
        public IList<TabDinamicaGrupoDTO> BuscarGrupoTabela(int _tipo, int? _tgr_id, int? _tgr_tipo)
        {
            var query = new List<TAB_DINAMICA_GRUPO>();

            if (_tipo == 1 )
            {
                var query01 = (from g in db.TAB_DINAMICA_GRUPO
                               where g.TAB_DINAMICA_CONFIG.Any(x => x.TDC_TIPO == _tipo && x.TDC_DATA_PUBLICACAO != null)
                                  && (_tgr_id == null || g.TGR_ID == _tgr_id)
                                  && (_tgr_tipo == null || g.TGR_TIPO == _tgr_tipo)
                               select g).ToList();
                   query = query01;
            }
            else
            {
                var query02 = (from g in db.TAB_DINAMICA_GRUPO
                               where g.TAB_DINAMICA_CONFIG.Any(x => (x.TDC_TIPO == 2 || x.TDC_TIPO == 3) && x.TDC_DATA_PUBLICACAO != null)
                                  && (_tgr_id == null || g.TGR_ID == _tgr_id)
                                  && (_tgr_tipo == null || g.TGR_TIPO == _tgr_tipo)
                               select g).ToList();

                   query = query02;
            }

                      
            return ToDTO(query);

        }
    
    }
}
