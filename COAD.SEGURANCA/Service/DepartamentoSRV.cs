using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Contexto;
using COAD.SEGURANCA.Model;
using Coad.GenericCrud.Dao.Base.Pagination;
using GenericCrud.Config.DataAttributes;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("DP_ID")]
    public class DepartamentoSRV : GenericService<DEPARTAMENTO, DepartamentoDTO, int>
    {
        public DepartamentoDAO _dao = new DepartamentoDAO();

        public DepartamentoSRV()
        {
            this.Dao = _dao;
        }

        public Pagina<DepartamentoDTO> Departamentos(string nome, int pagina = 1, int numeroRegistrosPagina = 7)
        {
            return _dao.Departamentos(nome, pagina, numeroRegistrosPagina);
        }

        public void DeletarDepartamento(int DP_ID)
        {
            var dp = FindById(DP_ID);
           
            if(dp != null)
            {
                dp.DATA_DESATIVACAO = DateTime.Now;
                Merge(dp);
            }
            
        }
    }
}
