

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
    public class NotaFiscalLoteDAO : AbstractGenericDao<NOTA_FISCAL_LOTE, NotaFiscalLoteDTO, Int32>
    {
        public CORPORATIVOContext db { get { return GetDb<CORPORATIVOContext>(); } set { } }

        public NotaFiscalLoteDAO()
        {
            db = GetDb<CORPORATIVOContext>(false);
        }

        private IQueryable<NOTA_FISCAL_LOTE> TemplateLoteParaEnvio()
        {
            var query = (from lot in db.NOTA_FISCAL_LOTE
                         where
                            lot.NLS_ID == 1 &&
                            (lot.NLT_ID == 1)
                         orderby lot.NFL_DATA ascending
                         select lot);
            return query;
        }

        private IQueryable<NOTA_FISCAL_LOTE> TemplateRetornarLoteParaProcessar()
        {
            var query = (from lot in db.NOTA_FISCAL_LOTE
                         where
                            lot.NLT_ID == 1 &&
                            (lot.NLS_ID == 8 || lot.NLS_ID == 9)
                         orderby lot.NFL_DATA ascending
                         select lot);
            return query;
        }

        public NotaFiscalLoteDTO RetornarLotePorRecibo(string codRecibo)
        {
            var query = (from lot in db.NOTA_FISCAL_LOTE
                         where lot.NLS_COD_RECIBO == codRecibo
                         select lot);

            return ToDTO(query.FirstOrDefault());
        }

        public NotaFiscalLoteDTO RetornarLoteVigente(int? empID)
        {
            var query = (from lot in db.NOTA_FISCAL_LOTE
                         where
                            lot.NLS_ID == 1 &&
                            lot.EMP_ID == empID &&
                            lot.NLT_ID == 1
                         orderby lot.NFL_DATA ascending
                         select lot);

            return ToDTO(query.FirstOrDefault());
        }

        public NotaFiscalLoteDTO RetornarLoteParaEnviar()
        {
            var query = TemplateLoteParaEnvio();
            return ToDTO(query.FirstOrDefault());
        }

        public NotaFiscalLoteDTO RetornarLoteParaProcessar()
        {
            var query = TemplateRetornarLoteParaProcessar();
            return ToDTO(query.FirstOrDefault());
        }

        public ICollection<NotaFiscalLoteDTO> RetornarLotesParaEnviar()
        {
            var query = TemplateLoteParaEnvio();
            return ToDTO(query);
        }

        public ICollection<NotaFiscalLoteDTO> RetornarLotesParaProcessar()
        {
            var query = TemplateRetornarLoteParaProcessar();
            return ToDTO(query);
        }

        public ICollection<NotaFiscalLoteDTO> RetornarLoteNFseParaEnviar()
        {
            var query = (from lot in db.NOTA_FISCAL_LOTE
                         where
                            lot.NLS_ID == 1 &&
                            (lot.NLT_ID == 3 || lot.NLT_ID == 5)
                         orderby lot.NFL_DATA ascending
                         select lot);
            return ToDTO(query);
        }

        public ICollection<NotaFiscalLoteDTO> RetornarLotesNFseParaProcessar()
        {

            var query = (from lot in db.NOTA_FISCAL_LOTE
                         where
                            (lot.NLT_ID == 3 || lot.NLT_ID == 5) &&
                            (lot.NLS_ID == 8 || lot.NLS_ID == 9)
                         orderby lot.NFL_DATA ascending
                         select lot);
            return ToDTO(query);
        }

        public ICollection<NotaFiscalLoteDTO> RetornarLoteAutorizadoItemNaoEnviado()
        {
            var query = (from lot in db.NOTA_FISCAL_LOTE
                         where
                            lot.NLT_ID == 1 &&
                            (lot.NLS_ID == 3) &&
                            (from itm in db.NOTA_FISCAL_LOTE_ITEM 
                             where itm.NLS_ID == 6
                             select lot.NFL_ID).Contains(lot.NFL_ID)
                         orderby lot.NFL_DATA ascending
                         select lot);
            return ToDTO(query);
        }
    }
}
