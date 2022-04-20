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

    public class LabelsDAO : AbstractGenericDao<LABELS, LabelsDTO, int>
    {

        public LabelsDAO() : base()
        {
            SetProfileName( "GED" );
           
        }

        public Pagina<LabelsDTO> Labels(int? labelId, string label = null, string descricao = null, int ativoId = 1, int pagina = 1, int itensPorPagina = 10)
        {
            IQueryable<LABELS> query = GetDbSet();

            if (descricao != null)
            {
                descricao = descricao.ToString();
                query = query.Where(x => x.LBL_DESCRICAO.Contains(descricao));
            }

            if (label != null)
            {
                label = label.ToString();
                query = query.Where(x => x.LBL_NOME.Contains(label));
            }

            query = query.Where(x => x.LBL_ATIVO == ativoId);
            
            //query = query.Where(p => p.DATA_EXCLUSAO == null); ALT: 23/06/2015 - o campo de exclusão lógica não foi criado...

            return ToDTOPage(query, pagina, itensPorPagina);
        }
    }
}
