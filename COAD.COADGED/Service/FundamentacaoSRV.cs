using Coad.GenericCrud.Service.Base;
using COAD.COADGED.DAO;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Service
{
    [ServiceConfig("FUN_ID")]
    public class FundamentacaoSRV : GenericService<FUNDAMENTACAO, FundamentacaoDTO, int>
    {
        private FundamentacaoDAO _dao = new FundamentacaoDAO();

        public FundamentacaoSRV()
        {
            Dao = _dao;
        }
        public IList<FundamentacaoDTO> Listar(int? _mai_id)
        {
            return _dao.Listar(_mai_id);
        }
    
    }
}
