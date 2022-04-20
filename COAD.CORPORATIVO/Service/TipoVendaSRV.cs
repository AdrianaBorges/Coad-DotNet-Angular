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
    public class TipoVendaSRV : ServiceAdapter<TIPO_VENDA, TipoVendaDTO, int>
    {
        private TipoVendaDAO _dao;

        public TipoVendaSRV()
        {
            this._dao = new TipoVendaDAO();
            SetDao(_dao);
        }

        public TipoVendaSRV(TipoVendaDAO _dao)
        {
            this._dao = _dao;
            SetDao(_dao);
        }

    }
}
