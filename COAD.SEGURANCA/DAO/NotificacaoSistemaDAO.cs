using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.SEGURANCA.Repositorios.Base;
using Coad.GenericCrud.Dao.Base;
using COAD.SEGURANCA.Repositorios.Contexto;
using COAD.SEGURANCA.Model;
using Coad.GenericCrud.Dao.Base.Pagination;


namespace COAD.CORPORATIVO.DAO
{
    public class NotificacaoSistemaDAO : AbstractGenericDao<NOTIFICACAO_SISTEMA, NotificacaoSistemaDTO,int>
    {
        public COADSYSEntities db { get { return GetDb<COADSYSEntities>(); } set { } }

        public NotificacaoSistemaDAO()
        {
            SetProfileName("coadsys");
        }

        public NotificacaoSistemaDTO RetornarNotificacaoSistema(int? tns_id , int? codRefInt = null, string codRefStr = null, string descricaoEx = null)
        {
            if (string.IsNullOrWhiteSpace(codRefStr))
                codRefStr = null;
            var query = (from ntSis in db.NOTIFICACAO_SISTEMA
                         where 
                            ntSis.TNS_ID == tns_id &&
                            (
                                (
                                    (codRefInt == null || ntSis.NTS_COD_REF_INT == codRefInt) &&
                                    (codRefStr == null || ntSis.NTS_COD_REF_STR == codRefStr)
                                ) || ntSis.NTS_ERRO_DESCRICAO.Contains(descricaoEx)
                            ) &&
                            ntSis.NTS_DATA_RESOLUCAO == null &&
                            ntSis.NTF_DATA_CANCELAMENTO == null

                         select ntSis).FirstOrDefault();

            return ToDTO(query);
        }

        public IList<NotificacaoSistemaDTO> RetornarNotificacoesSistema(int? tns_id, int? codRefInt = null, string codRefStr = null)
        {
            if (string.IsNullOrWhiteSpace(codRefStr))
                codRefStr = null;
            var query = (from ntSis in db.NOTIFICACAO_SISTEMA
                         where
                            ntSis.TNS_ID == tns_id &&
                            (codRefInt == null || ntSis.NTS_COD_REF_INT == codRefInt) &&
                            (codRefStr == null || ntSis.NTS_COD_REF_STR == codRefStr) &&
                            ntSis.NTS_DATA_RESOLUCAO == null &&
                            ntSis.NTF_DATA_CANCELAMENTO == null

                         select ntSis);

            return ToDTO(query);
        }
        /// <summary>
        /// Ao passar o tipo de notificação e informar uma lista de códigos de referencia, pesquisa e retorna os códigos que não possua nenhuma notificação sistema.
        /// </summary>
        /// <param name="tnsId"></param>
        /// <param name="lstCodRefId"></param>
        /// <returns></returns>
        public IList<int> RetornarCodRefenciasIntSemNotificacoesEmAberto(IList<int> lstCodRefId, int? tnsId)
        {
            IList<int> lstCodRefIdRetorno = new List<int>();

            if (lstCodRefId != null)
            {

                lstCodRefIdRetorno = (from codRefId in lstCodRefId
                         where 
                             !(from ntSis in db.NOTIFICACAO_SISTEMA
                                 where
                                    ntSis.TNS_ID == tnsId &&
                                    ntSis.NTS_DATA_RESOLUCAO == null &&
                                    ntSis.NTF_DATA_CANCELAMENTO == null
                             select ntSis.NTS_COD_REF_INT).Contains(codRefId)
                         select codRefId).ToList();            
            }
            return lstCodRefIdRetorno;
        }

        /// <summary>
        /// Ao passar o tipo de notificação e informar uma lista de códigos de referencia, pesquisa e retorna os códigos que não possua nenhuma notificação sistema.
        /// </summary>
        /// <param name="tnsId"></param>
        /// <param name="lstCodRefStr"></param>
        /// <returns></returns>
        public IList<string> RetornarCodRefenciasStrSemNotificacoesEmAberto(IList<string> lstCodRefStr, int? tnsId)
        {
            IList<string> lstCodRefIdRetorno = new List<string>();

            if (lstCodRefStr != null)
            {

                lstCodRefIdRetorno = (from codRefId in lstCodRefStr
                                      where
                                          !(from ntSis in db.NOTIFICACAO_SISTEMA
                                           where
                                                ntSis.TNS_ID == tnsId &&
                                                ntSis.NTS_DATA_RESOLUCAO == null &&
                                                ntSis.NTF_DATA_CANCELAMENTO == null
                                            select ntSis.NTS_COD_REF_STR).Contains(codRefId)
                                      select codRefId).ToList();
            }
            return lstCodRefIdRetorno;
        }

        //public IList<NotificacaoSistemaDTO> ListarNotificacoesDeEnvioEmAberto()
        //{

        //}
    }
}
