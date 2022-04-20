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
    public class FuncionalidadeDAO : AbstractGenericDao<FUNCIONALIDADE, FuncionalidadeDTO, int>
    {
        public COADGEDEntities db { get { return GetDb<COADGEDEntities>(); } set { } }    
        public FuncionalidadeDAO()
            : base()
        {
            SetProfileName( "GED" );
            db = GetDb<COADGEDEntities>(false);

        }

        public IList<FuncionalidadeDTO> ListarPorReferencia(string _tdc_id)
        {

            var query = (from f in db.FUNCIONALIDADE
                         where f.TDC_ID == _tdc_id
                         select f);


            return ToDTO(query);
        }

        public Pagina<FuncionalidadeDTO> ListarFuncionalidades(string _descricao = null, int _pagina = 1, int _itensPorPagina = 10)
        {
           
            var query = (from f in db.FUNCIONALIDADE
                         select f);

            if (!string.IsNullOrWhiteSpace(_descricao))
                query = query.Where(x => x.FCI_DESCRICAO.Contains(_descricao));

            
            return ToDTOPage(query, _pagina, _itensPorPagina);
        }

        public IList<FuncionalidadeDTO> ListarFuncionalidadesNaoSelect(int? _origem)
        {

            IQueryable<FUNCIONALIDADE> query = null;

            if (_origem != null)
            {
                query = (from f in db.FUNCIONALIDADE
                         where (db.ORIGEM_FUNCIONALIDADE.Count(x => x.FCI_ID == f.FCI_ID && x.OAC_ID == _origem) == 0)
                         select f).OrderBy(x => x.FCI_DESCRICAO);
            }
            else
            {
                query = (from f in db.FUNCIONALIDADE
                         select f).OrderBy(x => x.FCI_DESCRICAO);
            }


            return ToDTO(query);
        }

    }
}
