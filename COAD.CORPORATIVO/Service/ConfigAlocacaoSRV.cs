
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
    [ServiceConfig("CFA_ID")]
    public class ConfigAlocacaoSRV : GenericService<CONFIG_ALOCACAO, ConfigAlocacaoDTO, int>
    {
        public ConfigAlocacaoDAO _dao = new ConfigAlocacaoDAO();

        public ConfigAlocacaoSRV()
        {
            this.Dao = _dao;
        }        
    }
}
