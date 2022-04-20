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
    public class ProdutoComposicaoItemDAO : AbstractGenericDao<PRODUTO_COMPOSICAO_ITEM, ProdutoComposicaoItemDTO,int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public ProdutoComposicaoItemDAO()
        {
            db = GetDb<COADCORPEntities>();
        }

        public bool ChecaDuplicidade(ProdutoComposicaoItemDTO composicaoItemDTO)
        {
            if (composicaoItemDTO != null)
            {

                int? CMP_ID = composicaoItemDTO.CMP_ID;
                int? PRO_ID = composicaoItemDTO.PRO_ID;
                var value = (from res in GetDbSet().Where(x => x.CMP_ID == CMP_ID && x.PRO_ID == PRO_ID) select true).FirstOrDefault();

                return value;
            }

            return false;


        }

        public PRODUTO_COMPOSICAO_ITEM FindById(int id, int produtoId)
        {
            var obj = GetDbSet().Where(o => o.CMP_ID == id && o.PRO_ID == produtoId).FirstOrDefault();
            return obj;
        }

        public override void DeleteAll(IEnumerable<PRODUTO_COMPOSICAO_ITEM> lstObj, params string[] nameId)
        {
            foreach (var item in lstObj)
            {
                var obj = FindById(item.CMP_ID, item.PRO_ID);
                GetDbSet().Remove(obj);
            }
            GetDb(false).SaveChanges();
        }

        public bool ProdutoComposicaoPossuiComposicaoItemDeCurso(int? CMP_ID)
        {
            var query = GetDbSet().Where(x => x.CMP_ID == CMP_ID && x.PRODUTOS.OAC_ID == 8 && x.PRODUTOS.DATA_EXCLUSAO == null);
            int count = query.Count();

            return (count > 0);
        }

        public ProdutoComposicaoItemDTO ObterProdutoComposicaoItemQueGeraAssinatura(int? cmpId) 
        {   
            var itemPedido = (
                from proItem in db.PRODUTO_COMPOSICAO_ITEM
                where proItem.CMP_ID == cmpId && 
                    (
                        (from subItem in db.PRODUTO_COMPOSICAO_ITEM 
                         where subItem.CMP_ID == cmpId 
                         select subItem).Count() == 1 
                         || 
                        proItem.CMI_GERA_ASSINATURA_LEGADO
                    )
                select proItem)
                .FirstOrDefault();

            return ToDTO(itemPedido);
        }

        public IList<ProdutoComposicaoItemDTO> BuscaProdCmpItemPorComposicaoId(int? cmpId)
        {

            var query = (from proCmpItem in db.PRODUTO_COMPOSICAO_ITEM
                         where proCmpItem.CMP_ID == cmpId
                         select proCmpItem).ToList();

            return ToDTO(query);
        }

    }
}
