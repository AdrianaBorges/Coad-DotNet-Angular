using Coad.GenericCrud.Dao.Base;
using COAD.CORPORATIVO.LEGADO.Model.Dto;
using COAD.CORPORATIVO.LEGADO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PROSPECTADOS.Dao
{
    public class UltimoCodigoLegadoDAO : AbstractGenericDao<ULTIMO_CODIGO, UltimoCodigoLegadoDTO, string>
    {
        public corporativo2Entities db { get { return GetDb<corporativo2Entities>(); } set { } }

        public UltimoCodigoLegadoDAO()
        {
            SetProfileName("corp_old");
            db = GetDb<corporativo2Entities>(false);
        }

    }
}
