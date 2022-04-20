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
//using Coad.GenericCrud.Service.Base;
using Coad.GenericCrud.Repositorios.Base;

namespace COAD.CORPORATIVO.Service
{
    public class TipoEnvioSRV : GenericService<TIPO_ENVIO, TipoEnvioDTO, int>
    {
        private TipoEnvioDAO _dao;

        public TipoEnvioSRV()
        {
            this._dao = new TipoEnvioDAO();
            Dao = _dao;
        }

        public TipoEnvioSRV(TipoEnvioDAO _dao)
        {
            this._dao = _dao;
            Dao = _dao;
        }
    }
}
