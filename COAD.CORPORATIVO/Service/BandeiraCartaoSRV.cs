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
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.SEGURANCA.Service;
using GenericCrud.Config.DataAttributes.Maps;
using System.Transactions;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("BAC_ID")]
    public class BandeiraCartaoSRV : GenericService<BANDEIRA_CARTAO, BandeiraCartaoDTO, int>
    {
        private BandeiraCartaoDAO _dao;


        public BandeiraCartaoSRV(BandeiraCartaoDAO _dao)
        {
            this._dao = _dao;
            Dao = _dao;
        }

        public BandeiraCartaoSRV()
        {
            _dao = new BandeiraCartaoDAO();
            Dao = _dao;
        }
        
    }
}
