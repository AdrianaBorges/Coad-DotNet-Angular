using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using Coad.GenericCrud.Service.Base;
using Coad.GenericCrud.Repositorios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Service
{
    public class ConfigSpedFiscalSRV : ServiceAdapter<CONFIG_SPED_FISCAL,ConfigSpedFiscalDTO>
    {
        private ConfigSpedFiscalDAO _dao = new ConfigSpedFiscalDAO();

        public ConfigSpedFiscalSRV()
        {   
            SetDao(_dao);
        }
    }
}
