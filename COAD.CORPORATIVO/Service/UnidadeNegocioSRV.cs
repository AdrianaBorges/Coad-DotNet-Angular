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
    public class UnidadeNegocioSRV : GenericService<UNIDADE_NEGOCIO, UnidadeNegocioDTO, int>
    {
        private UnidadeNegocioDAO _dao;

        public UnidadeNegocioSRV()
        {
            this._dao = new UnidadeNegocioDAO();
            Dao = _dao;
        }

        public UnidadeNegocioSRV(UnidadeNegocioDAO _dao)
        {
            this._dao = _dao;
            Dao = _dao;
        }
    }
}
