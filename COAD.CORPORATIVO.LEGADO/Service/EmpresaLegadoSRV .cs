using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.LEGADO.Dao;
using COAD.CORPORATIVO.LEGADO.Model.Dto;
using COAD.CORPORATIVO.LEGADO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.LEGADO.Service
{
    [ServiceConfig("id")]
    public class EmpresaLegadoSRV : GenericService<empresas, EmpresaLegadoDTO, int>
    {
        public EmpresaLegadoDAO _dao;

        public EmpresaLegadoSRV()
        {
            this._dao = new EmpresaLegadoDAO();
            Dao = this._dao;
        }

        public EmpresaLegadoSRV(EmpresaLegadoDAO _dao)
        {
            this._dao = _dao;
            Dao = this._dao;
        }

        public EmpresaLegadoDTO FindById(int? id)
        {
            return _dao.FindById(id);
        }
    }
}
