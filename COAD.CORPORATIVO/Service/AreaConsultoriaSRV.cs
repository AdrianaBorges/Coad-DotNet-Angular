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
    public class AreaConsultoriaSRV : ServiceAdapter<AREA_CONSULTORIA, AreaConsultoriaDTO>
    {
        private AreaConsultoriaDAO _dao = new AreaConsultoriaDAO();

        public AreaConsultoriaSRV()
        {
            SetDao(_dao);
        }
        public IList<AreaConsultoriaDTO> BuscarNaoCadastrada(int _prod_id, string _ura_id,string _uf_sigla)
        {
            return _dao.BuscarNaoCadastrada(_prod_id, _ura_id,_uf_sigla);
        }
    }
    
}
