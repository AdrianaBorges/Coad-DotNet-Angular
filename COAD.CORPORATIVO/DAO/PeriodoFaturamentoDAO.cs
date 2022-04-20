

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Dao.Base;
using COAD.CORPORATIVO.Repository.Base;
using COAD.CORPORATIVO.Model.DTO;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Model.Dto;

namespace COAD.CORPORATIVO.DAO
{
    public class PeriodoFaturamentoDAO : AbstractGenericDao<PERIODO_FATURAMENTO, PeriodoFaturamentoDTO, object>
    {
        public CORPORATIVOContext db { get { return GetDb<CORPORATIVOContext>(); } set { } }

        public PeriodoFaturamentoDAO()
        {
            this.db = GetDb<CORPORATIVOContext>(false);
        }
        public IList<PeriodoFaturamentoDTO> ListarSemanas(int _PEF_MES, int _PEF_ANO)
        {
            var query = (from f in db.PERIODO_FATURAMENTO
                         where f.PEF_MES == _PEF_MES
                            && f.PEF_ANO == _PEF_ANO
                         select f);

            return ToDTO(query);
        }


    }
}
