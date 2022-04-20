using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.SEGURANCA.Repositorios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace COAD.CORPORATIVO.DAO
{
    public class UraProdutoAreaDAO : DAOAdapter<URA_PRODUTO_AREA, UraProdutoAreaDTO>
    {

        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }
        public UraProdutoAreaDAO()
        {
            this.db = GetDb<COADCORPEntities>();
        }
        public IList<UraProdutoAreaDTO> BuscarAreas(string _ura_id, int _pro_id)
        {
            IQueryable<URA_PRODUTO_AREA> query = GetDbSet();
            query = query.Where(x => x.URA_ID == _ura_id && x.PRO_ID == _pro_id);

            return ToDTO(query);
        }
        public IList<UraProdutoAreaDTO> BuscarAreas(string _ura_id, int _pro_id, string _ufacesso)
        {
            IQueryable<URA_PRODUTO_AREA> query = GetDbSet();
            query = query.Where(x => x.URA_ID == _ura_id && x.PRO_ID == _pro_id && x.UF_SIGLA_ACESSO == _ufacesso);

            return ToDTO(query);
        }

    }
}
