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

    public class VeiculoDAO : AbstractGenericDao<VEICULO, VeiculoDTO, int>
    {

        public VeiculoDAO() : base()
        {
            SetProfileName( "GED" );
           
        }

        public Pagina<VeiculoDTO> Veiculos(int? veiculoId, string descricao = null, int? periodoId = null, int? ativoId = null, int pagina = 1, int itensPorPagina = 10)
        {
            IQueryable<VEICULO> query = GetDbSet();

            if (descricao != null)
            {
                descricao = descricao.ToString();
                query = query.Where(x => x.TVI_DESCRICAO.Contains(descricao));
            }

            if (periodoId != null)
            {
                query = query.Where(x => x.PRD_ID == periodoId);
            }

            if (ativoId != null)
            {
                query = query.Where(x => x.TVI_ATIVO == ativoId);
            }

            //query = query.Where(p => p.DATA_EXCLUSAO == null); ALT: 23/06/2015 - o campo de exclusão lógica não foi criado...

            return ToDTOPage(query, pagina, itensPorPagina);
        }
    }
}
