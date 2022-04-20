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
    public class ConfigImpostoImpostoDAO : DAOAdapter<CONFIG_IMPOSTO_IMPOSTO, ConfigImpostoImpostoDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public ConfigImpostoImpostoDAO()
        {
            db = GetDb<COADCORPEntities>();
        }

        public ICollection<ConfigImpostoImpostoDTO> ListarImpostosDaConfiguracao(int? cfiId, bool? sobreTotal = null)
        {
            var query = (from cfin
                            in db.CONFIG_IMPOSTO_IMPOSTO
                         where cfin.CFI_ID == cfiId &&
                         (sobreTotal == null || cfin.IMPOSTO.IMP_SOBRE_TOTAL == sobreTotal)
                         select cfin);
            return ToDTO(query);
        }
    }
}
