using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using Coad.GenericCrud.Service.Base;
using Coad.GenericCrud.Repositorios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Service
{
    public class UnidadeMedidaSRV : ServiceAdapter<UNIDADE_MEDIDA, UnidadeMedidaDTO>
    {
        private UnidadeMedidaDAO _dao = new UnidadeMedidaDAO();

        public UnidadeMedidaSRV()
        {
            this.SetDao(_dao);
        }
        public List<UNIDADE_MEDIDA> Buscar()
        {
            return _dao.Buscar();
        }
        public List<FATOR_CONVERSAO> BuscarFatorConversao(string _und_id)
        {
            return _dao.BuscarFatorConversao(_und_id);
        }

        public List<UNIDADE_MEDIDA> BuscarPorPeriodo(int _emp_id, DateTime? _dtini, DateTime? _dtfim)
        {
            return _dao.BuscarPorPeriodo(_emp_id, _dtini, _dtfim);
        }

        public List<UNIDADE_MEDIDA> BuscarEntradaPorPeriodo(int _emp_id, DateTime? _dtini, DateTime? _dtfim)
        {
            return _dao.BuscarEntradaPorPeriodo(_emp_id, _dtini, _dtfim);
        }

    }
}
