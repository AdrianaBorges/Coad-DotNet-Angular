using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.DAO
{
    public class ParcelaLiquidacaoDAO : DAOAdapter<PARCELA_LIQUIDACAO, ParcelaLiquidacaoDTO, string>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public ParcelaLiquidacaoDAO()
        {
            db = GetDb<COADCORPEntities>();
        }

        /// <summary>
        /// ALT: 24/11/2016 - (alterou) buscar liquidação NÃO EXCLUÍDA por parcela
        /// </summary>
        /// <param name="_par_num_parcela"></param>
        /// <returns></returns>
        private IList<PARCELA_LIQUIDACAO> ListarPorParcela(string _par_num_parcela)
        {
            IList<PARCELA_LIQUIDACAO> query = db.PARCELA_LIQUIDACAO.Where(x => x.PAR_NUM_PARCELA == _par_num_parcela && x.PLI_DATA_EXCLUSAO == null).OrderByDescending(x => x.PLI_DATA).ToList();

            return query.ToList();
        }
        public IList<ParcelaLiquidacaoDTO> BuscarPorParcela(string _par_num_parcela)
        {
            IList<PARCELA_LIQUIDACAO> query = this.ListarPorParcela(_par_num_parcela);

            return ToDTO(query);
        }
        public Pagina<ParcelaLiquidacaoDTO> BuscarPorParcela(string _par_num_parcela, int numpagina = 1, int linhas = 10)
        {
            IList<PARCELA_LIQUIDACAO> query = this.ListarPorParcela(_par_num_parcela);

            return ToDTOPage(query, numpagina, linhas);
        }

    }
}
