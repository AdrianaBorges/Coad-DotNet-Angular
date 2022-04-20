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

    public class Tab31DAO : AbstractGenericDao<tab_31, Tab31DTO, int>
    {
        public Tab31DAO() : base()
        {
            SetProfileName( "PortalCoadHom" );
        }

        public Pagina<Tab31DTO> LerTab31(int pagina = 1, int itensPorPagina = 10)
        {
            IQueryable<tab_31> query = GetDbSet();
            
            return ToDTOPage(query, pagina, itensPorPagina);
        }
    }
}
