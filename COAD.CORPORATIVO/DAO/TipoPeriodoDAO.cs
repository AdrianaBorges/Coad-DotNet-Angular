using Coad.GenericCrud.Dao.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.DAO
{
    //
    public class TipoPeriodoDAO : AbstractGenericDao<TIPO_PERIODO, TipoPeriodoDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public TipoPeriodoDAO()
        {
            db = GetDb<COADCORPEntities>();
        }

        public IList<TipoPeriodoDTO> ListarTipoPeriodoDoProduto(int? cmpId)
        {
            var query = (from tp in db.PRODUTO_COMPOSICAO_TIPO_PERIODO
                         where tp.CMP_ID == cmpId
                         select tp.TIPO_PERIODO);

            return ToDTO(query);
        }
    }
}
