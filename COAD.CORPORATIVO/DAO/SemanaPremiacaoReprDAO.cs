using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.DAO
{

    public class SemanaPremiacaoReprDAO : DAOAdapter<SEMANA_PREMIACAO_REPR, SemanaPremiacaoReprDTO, object>
    {
 
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public SemanaPremiacaoReprDAO()
        {
            this.db = GetDb<COADCORPEntities>();
        }
        public List<SemanaPremiacaoReprDTO> ListarMetaSemanaRep(int _semana, DateTime _dtini, DateTime _dtfim, int _repid)
        {

            var query = (from a in db.SEMANA_PREMIACAO_REPR
                         join r in db.REPRESENTANTE on a.REP_ID equals r.REP_ID
                         where (a.SPR_SEMANA == _semana)
                            && (a.SPR_DATA_INI == _dtini)
                            && (a.SPR_DATA_FIM == _dtfim)
                            && ((_repid == 0) || (_repid > 0 && _repid == a.REP_ID))
                         select new SemanaPremiacaoReprDTO()
                         {
                             SPR_SEMANA = a.SPR_SEMANA,
                             SPR_DATA_INI = a.SPR_DATA_INI,
                             SPR_DATA_FIM = a.SPR_DATA_FIM,
                             REP_ID = a.REP_ID,
                             REP_NOME = r.REP_NOME,
                             SER_VLR_META = a.SER_VLR_META,
                             SER_VLR_PREMIO = a.SER_VLR_PREMIO,

                         }).ToList();

            return query;
        }
    }
}
