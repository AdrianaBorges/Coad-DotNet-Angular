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
    public class InfoMarketingDAO : AbstractGenericDao<INFO_MARKETING, InfoMarketingDTO,int>
    {
        /// <summary>
        /// Traz a informação de marketing com as listas populadas baseadas no id do cliente
        /// </summary>
        /// <param name="CLI_ID"></param>
        /// <returns></returns>
        public InfoMarketingDTO FindByCliIdFullLoaded(int CLI_ID)
        {
            IQueryable<INFO_MARKETING> query = GetDbSet();
            query = query.Where(x => x.MKT_CLI_ID == CLI_ID);

            var obj = query.FirstOrDefault();

            if (obj != null)
            {
                var dto = ToDTO(obj);

                dto.AREA_INFO_MARKETING = Convert<IEnumerable<AREA_INFO_MARKETING>, List<AreaInfoMarketingDTO>>(obj.AREA_INFO_MARKETING);
                dto.PRODUTO_COMPOSICAO_INFO_MARKETING = Convert<IEnumerable<PRODUTO_COMPOSICAO_INFO_MARKETING>, List<ProdutoComposicaoInfoMarketingDTO>>(obj.PRODUTO_COMPOSICAO_INFO_MARKETING);


                // removendo objetos que causam referência circular
                return dto;
            }

            return null;
        }

        public override INFO_MARKETING Save(INFO_MARKETING t)
        {
            var lstProdutoComposicaoInfoMarketing = t.PRODUTO_COMPOSICAO_INFO_MARKETING;
            var lstAreaInfoMarketing = t.AREA_INFO_MARKETING;

            t.PRODUTO_COMPOSICAO_INFO_MARKETING = null;
            t.AREA_INFO_MARKETING = null;

            var salvo = base.Save(t);
            t.PRODUTO_COMPOSICAO_INFO_MARKETING = lstProdutoComposicaoInfoMarketing;
            t.AREA_INFO_MARKETING = lstAreaInfoMarketing;

            return t;
        }

    }
}
