using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("TRE_ID")]
    public class TipoRemessaSRV : GenericService<TIPO_REMESSA, TipoRemessaDTO, int>
    {
        private TipoRemessaDAO _dao;


        public TipoRemessaSRV()
        {
            _dao = new TipoRemessaDAO();
            this.Dao = _dao;
        }

        public TipoRemessaSRV(TipoRemessaDAO _dao)
        {
            this._dao = _dao;
            this.Dao = _dao;
        }

    }
}
