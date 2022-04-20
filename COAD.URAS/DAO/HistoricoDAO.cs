using COAD.URAS.Model.Base;
using COAD.URAS.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.URAS.DAO
{
    public class HistoricoDAO : BaseConnection<HistoricoDAO>
    {
        public HistoricoDAO()
        {

        }
        public HistoricoDAO(string _ura_id)
        {
           

        }
        public List<HistoricoDTO> BuscarTodos()
        {
            return new List<HistoricoDTO>();
        }
        public List<HistoricoDTO> BuscarPorAssinatura(string codigo)
        {
            return new List<HistoricoDTO>();
        }
    }
}
