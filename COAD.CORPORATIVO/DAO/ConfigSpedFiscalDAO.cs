using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Service.Base;
using Coad.GenericCrud.Repositorios.Base;

namespace COAD.CORPORATIVO.DAO
{
    public class ConfigSpedFiscalDAO : DAOAdapter<CONFIG_SPED_FISCAL, ConfigSpedFiscalDTO> 
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public ConfigSpedFiscalDAO()
        {
            this.db = GetDb<COADCORPEntities>();
        }
    }

}
