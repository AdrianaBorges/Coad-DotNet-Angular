using COAD.URAS.DAO;
using COAD.URAS.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.URAS.Service
{
    public class HistoricoSRV
    {
        public HistoricoDAO _dao = null ;
        public HistoricoSRV()
        {
        }
        public HistoricoSRV(string _ura_id)
        {
            this._dao = new HistoricoDAO(_ura_id);
        }
        public List<HistoricoDTO> BuscarTodos()
        {
            return _dao.BuscarTodos();
        }
        public List<HistoricoDTO> BuscarPorAssinatura(string codigo)
        {
            return _dao.BuscarPorAssinatura(codigo);
        }
    }
}
