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
    [ServiceConfig("TPJ_ID")]
    public class TipoJoinSRV : GenericService<TIPO_JOIN, TipoJoinDTO, int>
    {

        private TipoJoinDAO _dao = new TipoJoinDAO();

        public TipoJoinSRV()
        {
            this.Dao = _dao;
        }


    }
}
