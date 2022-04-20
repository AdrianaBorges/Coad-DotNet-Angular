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
    public class TabelaPrecoDAO : AbstractGenericDao<TABELA_PRECO, TabelaPrecoDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public TabelaPrecoDAO()
        {
            db = GetDb<COADCORPEntities>(false);
        }

        public IList<TabelaPrecoDTO> GetTabelaPrecoByComposicao(int CMP_ID)
        {
            IQueryable<TABELA_PRECO> query = GetDbSet();
            query = query.Where(op => op.CMP_ID == CMP_ID && op.TP_DATA_EXCLUSAO == null);

            return ToDTO(query);
        }

        public IList<TabelaPrecoDTO> ListarTabelaPrecoByProdutoERegiao(int? CMP_ID, int? RG_ID)
        {
            IQueryable<TABELA_PRECO> query =
                (from tb in db.TABELA_PRECO
                 join rgTb in db.REGIAO_TABELA_PRECO on tb.TP_ID equals rgTb.TP_ID
                 where 
                    tb.CMP_ID == CMP_ID && rgTb.RG_ID == RG_ID &&
                    tb.TP_DATA_EXCLUSAO == null
                 orderby tb.TP_DESCRICAO
                 select tb);

            return ToDTO(query);
        }

    }
}
        



