using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Custons.Pesquisas;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.DAO
{
    public class RegistroLiberacaoDAO : DAOAdapter<REGISTRO_LIBERACAO, RegistroLiberacaoDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public RegistroLiberacaoDAO()
        {
            db = GetDb<COADCORPEntities>();
        }

        public Pagina<RegistroLiberacaoDTO> PesquisarRegistrosLiberacao(PesquisaRegistroLiberacaoDTO pesquisaDTO)
        {
            int? rliId = pesquisaDTO.rliId;
            string descricao = pesquisaDTO.descricao;
            int? ppiId = pesquisaDTO.ppiId;
            int? prtId = pesquisaDTO.prtId;
            string produto = pesquisaDTO.produto;
            int pagina = pesquisaDTO.pagina;
            int registrosPorPagina = pesquisaDTO.registrosPorPagina;

            if (string.IsNullOrWhiteSpace(descricao))
            {
                descricao = null;
            }

            if (string.IsNullOrWhiteSpace(produto))
            {
                produto = null;
            }

            IQueryable<REGISTRO_LIBERACAO> query = (from reLib in db.REGISTRO_LIBERACAO
                         where 
                            reLib.DATA_EXCLUSAO == null &&
                            (descricao == null || reLib.RLI_DESCRICAO.Contains(descricao)) &&
                            (rliId == null || reLib.RLI_ID == rliId)
                            orderby reLib.RLT_DATA_ACAO descending
                         select reLib);

            if(ppiId != null || prtId != null || !string.IsNullOrWhiteSpace(produto))
            {
                query = (from
                            itmPro in db.PROPOSTA_ITEM join
                            reLib in query on itmPro.RLI_ID equals reLib.RLI_ID
                         where
                            (ppiId == null || itmPro.PPI_ID == ppiId) &&
                            (prtId == null || itmPro.PRT_ID == prtId) &&
                            (produto == null || itmPro.PRODUTO_COMPOSICAO.CMP_DESCRICAO.Contains(produto))
                         select reLib);
            }

            return ToDTOPage(query, pagina, registrosPorPagina);
        } 
    }
}
