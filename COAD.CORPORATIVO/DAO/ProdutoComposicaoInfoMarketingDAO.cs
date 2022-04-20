using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using Coad.GenericCrud.Dao.Base;


namespace COAD.CORPORATIVO.DAO
{
    public class ProdutoComposicaoInfoMarketingDAO : AbstractGenericDao<PRODUTO_COMPOSICAO_INFO_MARKETING, ProdutoComposicaoInfoMarketingDTO, int>
    {
        public IQueryable<PRODUTO_COMPOSICAO_INFO_MARKETING> TemplateProdutoComposicaoInfoMarketing(int MKT_CLI_ID, int CMP_ID)
        {
            var query = GetDbSet().Where(x => x.MKT_CLI_ID == MKT_CLI_ID && x.CMP_ID == CMP_ID);
            return query;

        }

        public ProdutoComposicaoInfoMarketingDTO FindProdutoComposicaoInfoMarketing(int MKT_CLI_ID, int CMP_ID)
        {
            var query = TemplateProdutoComposicaoInfoMarketing(MKT_CLI_ID, CMP_ID).FirstOrDefault();
            return ToDTO(query);
        }

        public bool HasProdutoComposicaoInfoMarketing(int MKT_CLI_ID, int CMP_ID)
        {
            var query = TemplateProdutoComposicaoInfoMarketing(MKT_CLI_ID, CMP_ID);

            return (query.Count() > 0);
        }
    }
}
