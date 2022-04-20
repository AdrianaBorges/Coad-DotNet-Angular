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
    public class RepresentanteMetaDAO : DAOAdapter<REPRESENTANTE_META, RepresentanteMetaDTO, object>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public RepresentanteMetaDAO()
        {
            db = GetDb<COADCORPEntities>();
        }
        public List<RepresentanteMetaDTO> ListarMetaRep(int _mes, int _ano, int _tcoid, int _repid)
        {
            var query = (from a in db.REPRESENTANTE_META
                         join r in db.REPRESENTANTE on a.REP_ID equals r.REP_ID
                         join t in db.TIPO_COMISSAO on a.TCO_ID equals t.TCO_ID
                         where (a.RME_MES == _mes)
                            && (a.RME_ANO == _ano)
                            && ((_tcoid == 0) || (_tcoid > 0 && _tcoid == t.TCO_ID))
                            && ((_repid == 0) || (_repid > 0 && _repid == a.REP_ID))
                         select new RepresentanteMetaDTO()
                         {
                            REP_ID = r.REP_ID,
                            REP_NOME = r.REP_NOME,
                            TCO_ID = a.TCO_ID,
                            RME_MES = a.RME_MES,
                            RME_ANO = a.RME_ANO,
                            RME_VLR_META = a.RME_VLR_META,
                            SER_PREMIO_MIN = a.SER_PREMIO_MIN,
                            SER_PREMIO_MAX = a.SER_PREMIO_MAX,

                         }).ToList();

            return query;
        }

    }

}
