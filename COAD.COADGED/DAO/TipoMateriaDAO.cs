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

    public class TipoMateriaDAO : AbstractGenericDao<TIPO_MATERIA, TipoMateriaDTO, int>
    {

        public TipoMateriaDAO() : base()
        {
            SetProfileName( "GED" );
           
        }

        public Pagina<TipoMateriaDTO> TiposMaterias(int? tipoMateriaId, string descricao = null, int ativoId = 1, int pagina = 1, int itensPorPagina = 10)
        {
            IQueryable<TIPO_MATERIA> query = GetDbSet();

            if (descricao != null)
            {
                descricao = descricao.ToString();
                query = query.Where(x => x.TIP_MAT_DESCRICAO.Contains(descricao));
            }

            query = query.Where(x => x.TIP_MAT_ATIVO == ativoId);
            
            //query = query.Where(p => p.DATA_EXCLUSAO == null); ALT: 23/06/2015 - o campo de exclusão lógica não foi criado...

            return ToDTOPage(query, pagina, itensPorPagina);
        }
    }
}
