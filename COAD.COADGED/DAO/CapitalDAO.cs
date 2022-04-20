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
    // ALT: 22/06/2015 15h45m
    // Redação e Editoração...

    public class CapitalDAO : AbstractGenericDao<CAPITAL, CapitalDTO, int>
    {

        public CapitalDAO() : base()
        {
            SetProfileName( "GED" );
           
        }

        public Pagina<CapitalDTO> Capital(int? capId, string nome = null, int pagina = 1, int itensPorPagina = 10)
        {
            IQueryable<CAPITAL> query = GetDbSet();

            if (!String.IsNullOrWhiteSpace(nome))
            {
                query = query.Where(x => x.CAP_NOME.Contains(nome.ToString()));
            }

            if (capId != null)
            {
                query = query.Where(x => x.CAP_ID == capId);
            }

            return ToDTOPage(query, pagina, itensPorPagina);
        }
    }
}
