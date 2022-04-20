
using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Repositorios.Base;
using GenericCrud.Config.DataAttributes;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("CFI_ID", "RG_ID")]
    public class ConfigImpostoRegiaoSRV : GenericService<CONFIG_IMPOSTO_REGIAO, ConfigImpostoRegiaoDTO, int>
    {        
        public ConfigImpostoRegiaoDAO _dao = new ConfigImpostoRegiaoDAO();

        public ConfigImpostoRegiaoSRV()
        {
            this.Dao = _dao;
        }        
    }
}
