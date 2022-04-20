
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
    [ServiceConfig("TCC_ID")]
    public class TipoConfigContaSRV : GenericService<TIPO_CONFIG_CONTA, TipoConfigContaDTO, int>
    {
        public TipoConfigContaDAO _dao = new TipoConfigContaDAO();

        public TipoConfigContaSRV()
        {
            this.Dao = _dao;
        }        
    }
}
