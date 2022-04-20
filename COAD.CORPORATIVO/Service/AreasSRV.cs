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
    public class AreasSRV : GenericService<AREAS, AreasCorpDTO, int>
    {
        private AreasDAO _dao;

        public AreasSRV()
        {
            this._dao = new AreasDAO();
            this.Dao = _dao;
        }

        public AreasSRV(AreasDAO _dao)
        {
            this._dao = _dao;
            this.Dao = _dao;
        }

        public AreasCorpDTO ObterAreasPorNome(string areaStr)
        {
            return _dao.ObterAreasPorNome(areaStr);
        }
    }
}
