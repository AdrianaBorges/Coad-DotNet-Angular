using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Service
{
    public class BancosSRV : ServiceAdapter<BANCOS, BancosDTO, int>
    {
        private BancosDAO _dao;

        public BancosSRV()
        {
            _dao = new BancosDAO();
            SetDao(_dao);
        }

        public BancosSRV(BancosDAO _dao)
        {
            this._dao = _dao;
            SetDao(_dao);
        }

    }
}
