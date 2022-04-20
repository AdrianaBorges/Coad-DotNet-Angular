using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.CORPORATIVO.LEGADO.Model.Dto;
using COAD.CORPORATIVO.LEGADO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.LEGADO.Dao
{
    public class AssinaturaLegadoDAO : AbstractGenericDao<ASSINATURA, AssinaturaLegadoDTO, object>
    {
        public corporativo2Entities db { get { return GetDb<corporativo2Entities>(); } set { } }
        
        public AssinaturaLegadoDAO()
        {
            SetProfileName("corp_old");
            db = GetDb<corporativo2Entities>();
        }

        public void TransferirVigencia(string vASSIN_ANT, string vASSIN_ATU, string vSOLIC, string vDATA_TRANSF, string vVIGENCIA, string vCONTRATO, Nullable<int> vMES_REFERENCIA, Nullable<System.DateTime> vDATA_INI_VIGENCIA, Nullable<System.DateTime> vDATA_FIM_VIGENCIA, string uSU_LOGIN, string aSN_TRANSF_MOTIVO)
        {
            var vRETORNO = new ObjectParameter("vRETORNO", typeof(string));

            db.TRANSF_ASSIN(vASSIN_ANT,
                        vASSIN_ATU,
                        vSOLIC,
                        vDATA_TRANSF,
                        vVIGENCIA,
                        vCONTRATO,
                        vMES_REFERENCIA,
                        vDATA_INI_VIGENCIA,
                        vDATA_FIM_VIGENCIA,
                        uSU_LOGIN,
                        aSN_TRANSF_MOTIVO,
                        vRETORNO);
        }

        
    }
}
