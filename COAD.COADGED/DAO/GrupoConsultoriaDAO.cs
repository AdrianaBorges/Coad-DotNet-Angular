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

    //public class GrupoConsultoriaDAO : AbstractGenericDao<GRUPO_CONSULTORIA, GrupoConsultoriaDTO, int>
    //{

    //    public GrupoConsultoriaDAO() : base()
    //    {
    //        SetProfileName( "GED" );
           
    //    }

    //    public Pagina<GrupoConsultoriaDTO> GruposConsultoria(int? grupoConsultoriaId, string descricao = null, int ativoId = 1, int pagina = 1, int itensPorPagina = 10)
    //    {
    //        IQueryable<GRUPO_CONSULTORIA> query = GetDbSet();

    //        if (descricao != null)
    //        {
    //            descricao = descricao.ToString();
    //            query = query.Where(x => x.GRD_CONS_DESCRICAO.Contains(descricao));
    //        }

    //        query = query.Where(x => x.GRD_CONS_ATIVO == ativoId);
            
    //        //query = query.Where(p => p.DATA_EXCLUSAO == null); ALT: 23/06/2015 - o campo de exclusão lógica não foi criado...

    //        return ToDTOPage(query, pagina, itensPorPagina);
    //    }
    //}
}
