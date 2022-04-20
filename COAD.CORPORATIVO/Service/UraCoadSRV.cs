using Coad.GenericCrud.Dao.Base.Pagination;
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
    public class UraCoadSRV : ServiceAdapter<URA_COAD, UraCoadDTO, int>
    {
        private UraCoadDAO _dao = new UraCoadDAO();

        public UraCoadSRV()
        {
            SetDao(_dao);
        }
        public Pagina<UraCoadDTO> BuscarClientes(string _ura_id, string _asn_id, string _telefone, int numpagina = 1, int linhas = 10)
        {
            return _dao.BuscarClientes(_ura_id, _asn_id, _telefone, numpagina, linhas);
        }

    }
}
