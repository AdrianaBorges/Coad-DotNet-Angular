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
    public class OrigemDAO : AbstractGenericDao<ORIGEM_FUNCIONALIDADE, OrigemFuncionalidadeDTO, int>
    {
                public COADGEDEntities db { get { return GetDb<COADGEDEntities>(); } set { } } 
        public OrigemDAO()
            : base()
        {
            SetProfileName( "GED" );
            db = GetDb<COADGEDEntities>(false);

        }
        public Pagina<OrigemFuncionalidadeDTO> ListarOrigem(string _descricao = null, int _pagina = 1, int _itensPorPagina = 10)
        {
            var query = (from f in db.ORIGEM_FUNCIONALIDADE
                         select f);

            if (!string.IsNullOrWhiteSpace(_descricao))
                query = query.Where(x => x.OFU_DESCRICAO.Contains(_descricao));


            return ToDTOPage(query, _pagina, _itensPorPagina);
        }

    }
}
