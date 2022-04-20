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

    public class Publicacao_vwDAO : AbstractGenericDao<PUBLICACAO_vw, Publicacao_vwDTO, int>
    {
        public Publicacao_vwDAO() : base()
        {
            SetProfileName( "GED" );
        }

        public Pagina<Publicacao_vwDTO> Busca(string coadgedBI = null, int pagina = 1, int itensPorPagina = 10)
        {
            IQueryable<PUBLICACAO_vw> query = GetDbSet();

            Int32 nrMateria = 0;
            bool buscarNumeroMateria = Int32.TryParse(coadgedBI, out nrMateria);

            if (buscarNumeroMateria)
            {
                query = query.Where(x => x.PUB_ID == nrMateria);
            } else if (!String.IsNullOrWhiteSpace(coadgedBI))
            {
                query = query.Where(x => x.PUB_CONTEUDO.Contains(coadgedBI) || x.PUB_CONTEUDO_RESENHA.Contains(coadgedBI));
            }

            return ToDTOPage(query, pagina, itensPorPagina);
        }
    }
}
