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

    public class CargosDAO : AbstractGenericDao<CARGOS, CargosDTO, int>
    {

        public CargosDAO() : base()
        {
            SetProfileName( "GED" );
           
        }

        public Pagina<CargosDTO> Cargos(int? cargoId, string descricao = null, int pagina = 1, int itensPorPagina = 10)
        {
            IQueryable<CARGOS> query = GetDbSet();

            if (descricao != null)
            {
                descricao = descricao.ToString();
                query = query.Where(x => x.CRG_DESCRICAO.Contains(descricao));
            }

            if (cargoId != null)
            {
                query = query.Where(x => x.CRG_ID == cargoId);
            }

            //query = query.Where(p => p.DATA_EXCLUSAO == null); ALT: 23/06/2015 - o campo de exclusão lógica não foi criado...

            return ToDTOPage(query, pagina, itensPorPagina);
        }
    }
}
