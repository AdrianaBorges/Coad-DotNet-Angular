

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Dao.Base;
using COAD.CORPORATIVO.Repository.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;

namespace COAD.CORPORATIVO.DAO
{
    public class CampanhaVendasProdutoComposicaoDAO : AbstractGenericDao<CAMPANHA_VENDAS_PRODUTO_COMPOSICAO, CampanhaVendasProdutoComposicaoDTO, Int32>
    {
        public CORPORATIVOContext db { get { return GetDb<CORPORATIVOContext>(); } set { } }

        public CampanhaVendasProdutoComposicaoDAO()
        {
            db = GetDb<CORPORATIVOContext>(false);
        }

        /// <summary>
        /// Lista os CampanhaVendasProdutoComposicao por Código de Campanha de Vendas
        /// </summary>
        /// <returns></returns>
        public ICollection<CampanhaVendasProdutoComposicaoDTO> ListarCampVendasProdutoCmpPorCampanha(int? cveId)
        {
            var query = (from cvproCmp
                            in db.CAMPANHA_VENDAS_PRODUTO_COMPOSICAO
                         where cvproCmp.CVE_ID == cveId
                         select cvproCmp);
            return ToDTO(query);
        }
        
    }
}
