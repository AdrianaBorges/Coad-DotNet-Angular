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

    public class ChequeDevolvidoSRV : ServiceAdapter<CHEQUE_DEVOLVIDO, ChequeDevolvidoDTO, int>
    {
        private ChequeDevolvidoDAO _dao = new ChequeDevolvidoDAO();

        public ChequeDevolvidoSRV()
        {
            SetDao(_dao);
        }
        public IList<ChequeDevolvidoDTO> BuscarPorAssinatura(string _asn_num_assinatura)
        {
           return _dao.BuscarPorAssinatura(_asn_num_assinatura);
        }
        public Pagina<ChequeDevolvidoDTO> BuscarPorAssinatura(string _asn_num_assinatura, int numpagina = 1, int linhas = 10)
        {
            return _dao.BuscarPorAssinatura(_asn_num_assinatura, numpagina, linhas);
        }
    }
}
