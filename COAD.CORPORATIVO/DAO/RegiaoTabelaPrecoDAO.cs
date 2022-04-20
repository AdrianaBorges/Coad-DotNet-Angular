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
    public class RegiaoTabelaPrecoDAO : AbstractGenericDao<REGIAO_TABELA_PRECO, RegiaoTabelaPrecoDTO,int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public RegiaoTabelaPrecoDAO()
        {
            db = GetDb<COADCORPEntities>(false);
        }

        public bool HasRegiaoTabelaPreco(int? TP_ID, int RG_ID)
        {
            IQueryable<REGIAO_TABELA_PRECO> query = GetDbSet().Where(x => 
                x.RG_ID == RG_ID && 
                x.TP_ID == TP_ID);

            int count = query.Count();

            return (count > 0);
        }

        public IList<RegiaoTabelaPrecoDTO> ListarRegiaoTabelaPrecoPorRegiaoEProdutoComposicao(int? RG_ID, int? CMP_ID)
        {
            IQueryable<REGIAO_TABELA_PRECO> query = GetDbSet().Where(x =>
                x.RG_ID == RG_ID &&
                x.TABELA_PRECO.CMP_ID == CMP_ID &&
                x.TABELA_PRECO.TP_DATA_EXCLUSAO == null &&
                x.RTP_DATA_EXCLUSAO == null);

            return ToDTO(query);
        }

        public IList<RegiaoTabelaPrecoDTO> ListarRegiaoTabelaPrecoPorRegiaoProdutoComposicaoETipoPagamento(int? RG_ID, int? CMP_ID, int? TPG_ID, int? TTP_ID = null, bool ehCurso = false)
        {
            IQueryable<REGIAO_TABELA_PRECO> query = 
            (from regTabPre in db.REGIAO_TABELA_PRECO
             join tabPrecoTipoPag in db.TABELA_PRECO_TIPO_PAGAMENTO 
             on regTabPre.TABELA_PRECO.TP_ID equals tabPrecoTipoPag.TP_ID
             where
                regTabPre.RG_ID == RG_ID &&
                regTabPre.TABELA_PRECO.CMP_ID == CMP_ID &&
                 tabPrecoTipoPag.TPG_ID == TPG_ID &&
                 regTabPre.RTP_DATA_EXCLUSAO == null &&
                 regTabPre.TABELA_PRECO.TP_DATA_EXCLUSAO == null &&            
                 (ehCurso || TTP_ID == null || regTabPre.TABELA_PRECO.TTP_ID == TTP_ID)
             orderby regTabPre.TABELA_PRECO.TP_NUM_PARCELAS_MIN
             select regTabPre);

            return ToDTO(query);
        }

        public RegiaoTabelaPrecoDTO BuscarTabelaPreco(int _RG_ID, int _CMP_ID)
        {
            
            var regiaoTabPreco = (from r in db.REGIAO_TABELA_PRECO
                                  join t in db.TABELA_PRECO on r.TABELA_PRECO.TP_ID equals t.TP_ID
                                  where r.RG_ID == _RG_ID &&
                                        t.CMP_ID == _CMP_ID &&
                                        t.TP_DATA_EXCLUSAO == null &&
                                        r.RTP_DATA_EXCLUSAO == null
                                  select r).FirstOrDefault();

            return ToDTO(regiaoTabPreco);

        }

        public IList<RegiaoTabelaPrecoDTO> ListarPorTabelaPreco(int? TP_ID)
        {
            var lstRegiaoTabelaPreco = (from r in db.REGIAO_TABELA_PRECO
                                     where r.TP_ID == TP_ID && 
                                     r.RTP_DATA_EXCLUSAO == null
                                     select r);

            return ToDTO(lstRegiaoTabelaPreco);
        }
    }
}
