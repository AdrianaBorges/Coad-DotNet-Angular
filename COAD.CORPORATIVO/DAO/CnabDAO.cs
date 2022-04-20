using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Web.Configuration;
using COAD.SEGURANCA.Repositorios.Base;
using System.Data.Objects.SqlClient;

namespace COAD.CORPORATIVO.DAO
{
    public class CnabDAO : AbstractGenericDao<CNAB, CnabDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public CnabDAO()
        {
            db = GetDb<COADCORPEntities>(false);
        }

        public IList<CnabDTO> LerCNAB(int? empresa = null, string banco=null, string leiaute=null, string arquivo=null, string registro=null, string campo=null)
        {
            var _query = (from c in db.CNAB
                          where c.DATA_EXCLUSAO == null
                             && (banco == null || (banco != null && c.BAN_ID == banco))
                             && (leiaute == null || (leiaute != null && c.CNB_CNAB == leiaute))
                             && (arquivo == null || (arquivo != null && c.CNB_ARQUIVO == arquivo))
                             && (registro == null || (registro != null && c.CNB_REGISTRO == registro))
                             && (campo == null || (campo != null && c.CNB_CAMPO == campo)) &&
                             (c.CCA_ID == null || 
                                 (
                                    from 
                                        cnabCfgArq in db.CNAB_CONFIG_ARQUIVO join
                                        cnbCfg in db.CNAB_CONFIG on cnabCfgArq.CNC_ID equals cnbCfg.CNC_ID
                                    where 
                                        cnbCfg.DATA_EXCLUSAO == null &&
                                        cnabCfgArq.DATA_EXCLUSAO == null
                                    select cnabCfgArq.CCA_ID)
                                    .Contains((int) c.CCA_ID)
                             )
                             orderby c.CNB_INICIO ascending
                          select c);


            return ToDTO(_query);
        }
        public Pagina<CnabDTO> LerCNAB(int? empresa = null, string banco = null, string leiaute = null, string arquivo = null, string registro = null, string campo = null, int pagina = 1, int itensPorPagina = 10)
        {
            var _query = (from c in db.CNAB
                          where c.DATA_EXCLUSAO == null
                             && (banco == null || (banco != null && c.BAN_ID == banco))
                             && (leiaute == null || (leiaute != null && c.CNB_CNAB == leiaute))
                             && (arquivo == null || (arquivo != null && c.CNB_ARQUIVO == arquivo))
                             && (registro == null || (registro != null && c.CNB_REGISTRO == registro))
                             && (campo == null || (campo != null && c.CNB_CAMPO == campo))
                          orderby c.CNB_INICIO ascending
                          select c);


            return ToDTOPage(_query, pagina, itensPorPagina);
        }
        public IList<CnabDTO> BuscarDetalheCNAB(CnabDTO _Cnab)
        {
            var _query = (from c in db.CNAB
                          where c.EMP_ID == _Cnab.EMP_ID &&
                                c.BAN_ID == _Cnab.BAN_ID &&
                                c.CNB_CNAB == _Cnab.CNB_CNAB &&
                                c.CNB_ARQUIVO == _Cnab.CNB_ARQUIVO &&
                                c.CNB_REGISTRO == _Cnab.CNB_REGISTRO
                          select c).OrderBy( x => x.CNB_ID);

            return ToDTO(_query);
        }
        public List<CnabDTO> BuscarCNAB()
        {
            var _query = (from c in db.CNAB
                          group c by new { c.EMP_ID, c.BAN_ID, c.CNB_CNAB, c.CNB_ARQUIVO, c.CNB_REGISTRO } into grp
                          select new CnabDTO
                          {
                              EMP_ID = grp.Key.EMP_ID,
                              BAN_ID = grp.Key.BAN_ID,
                              CNB_CNAB = grp.Key.CNB_CNAB,
                              CNB_ARQUIVO = grp.Key.CNB_ARQUIVO,
                              CNB_REGISTRO = grp.Key.CNB_REGISTRO,
                              CNB_REFERENCIA = SqlFunctions.StringConvert((double)grp.Key.EMP_ID) +"-"+
                                               grp.Key.CNB_REGISTRO + "-" +
                                               grp.Key.BAN_ID + "-" +
                                               grp.Key.CNB_CNAB + "-" +
                                               grp.Key.CNB_ARQUIVO

                          }).OrderBy(x => x.EMP_ID).ThenBy(x => x.BAN_ID).ThenBy(x => x.CNB_ARQUIVO).ThenBy(x => x.CNB_REGISTRO).ToList();

            return _query;
        }

        public IList<CnabDTO> ListarCnabsDoCnabConfigArquivo(int? ccaId)
        {
            var query = (from cnb in db.CNAB
                         where
                            cnb.DATA_EXCLUSAO == null &&
                            cnb.CCA_ID == ccaId
                            orderby cnb.CNB_INICIO ascending
                         select cnb);
            return ToDTO(query);
        }
    }
}
