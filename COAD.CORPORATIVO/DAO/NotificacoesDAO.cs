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
using Coad.GenericCrud.Dao.Base.Pagination;
using System.Data.SqlClient;


namespace COAD.CORPORATIVO.DAO
{
    public class NotificacoesDAO : AbstractGenericDao<NOTIFICACOES, NotificacoesDTO, int>
    {
public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public NotificacoesDAO() : base()
        {
            this.db = GetDb<COADCORPEntities>();
        }

        public IQueryable<NOTIFICACOES> TemplateNotificacoes(int REP_ID, bool? visualizado = null, int? tipoNotificacaoId = null, string urgenciaNotificacaoId = null)
        {
            if (!string.IsNullOrWhiteSpace(urgenciaNotificacaoId))
            {
                urgenciaNotificacaoId = null;
            }

            IQueryable<NOTIFICACOES> query = (from nf in db.NOTIFICACOES 
                                              where nf.REP_ID == REP_ID &&
                                              (visualizado == null || nf.NTF_VISUALIZADO == visualizado) &&
                                              (tipoNotificacaoId == null || nf.TP_NTF_ID == tipoNotificacaoId) &&
                                              (urgenciaNotificacaoId == null || nf.URG_NTF_ID == urgenciaNotificacaoId) 
                                              orderby 
                                                nf.NTF_DATA descending 
                                              select nf);
            
            return query;
        }

        public int ChecaQuantidadeNotificacoesNaoLidas(int REP_ID)
        {
            var query = TemplateNotificacoes(REP_ID, false);
            return query.Count();
        }

        public Pagina<NotificacoesDTO> Notificacoes(int REP_ID, bool? lidas = null, 
            int? tipoNotificacaoId = null, string urgenciaNotificacaoId = null, int pagina = 1, int linhasPorPagina = 15)
        {
                var query = TemplateNotificacoes(REP_ID, lidas, tipoNotificacaoId, urgenciaNotificacaoId);
                return ToDTOPage(query, pagina, linhasPorPagina);            
        }

        public void MarcarTodasAsNotificacoesComoLidas(int? REP_ID)
        {
            var sqlComand = @"UPDATE [NOTIFICACOES] 
             SET NTF_VISUALIZADO = 1 
             WHERE REP_ID = {0}";

            db.Database.ExecuteSqlCommand(sqlComand, REP_ID);

            //var query = (from nf in db.NOTIFICACOES where nf.REP_ID == REP_ID select nf);
            //return ToDTO(query);
            
        }

        public IList<NotificacoesDTO> ListarNotificacoesNaoExibidas(int? REP_ID)
        {
            var query = (from nt in db.NOTIFICACOES
                         where
                            nt.REP_ID == REP_ID 
                         orderby nt.NTF_DATA descending
                         select nt).
                         Take(5).
                         Where(nt => (nt.NTF_VISUALIZADO == false) &&
                            (nt.NTF_EXIBIDO == null || nt.NTF_EXIBIDO == false));
            return ToDTO(query);

        }

        public bool ChecaRepreJaNotificado(int? repId, int? tpNtfID, int? codRef)
        {
            var query = (from nt in db.NOTIFICACOES
                         where
                            nt.REP_ID == repId &&
                            nt.TP_NTF_ID == tpNtfID &&
                            nt.NTF_COD_REF_INT == codRef
                         select nt);

            return (query.Count() > 0);
        }

        public bool ChecaRepreJaNotificado(int? repId, int? ntTpID, string codRef)
        {
            var query = (from nt in db.NOTIFICACOES
                         where
                            nt.REP_ID == repId &&
                            nt.TP_NTF_ID == ntTpID &&
                            nt.NTF_COD_REF_STR == codRef
                         select nt);

            return (query.Count() > 0);
        }

    }
}
