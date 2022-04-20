using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.DAO
{
    public class ChequeDevolvidoDAO : DAOAdapter<CHEQUE_DEVOLVIDO, ChequeDevolvidoDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public ChequeDevolvidoDAO()
        {
            db = GetDb<COADCORPEntities>();
        }
        private IList<CHEQUE_DEVOLVIDO> ListarPorAssinatura(string _asn_num_assinatura)
        {
            IList<CHEQUE_DEVOLVIDO> query = db.CHEQUE_DEVOLVIDO.Where(x => x.ASN_NUM_ASSINATURA == _asn_num_assinatura).OrderByDescending(x => x.CHD_DATA_CHEQUE).ToList();

            return query.ToList();
        }
        public IList<ChequeDevolvidoDTO> BuscarPorAssinatura(string _asn_num_assinatura)
        {
            IList<CHEQUE_DEVOLVIDO> query = this.ListarPorAssinatura(_asn_num_assinatura);
            
            return ToDTO(query);
        }
        public Pagina<ChequeDevolvidoDTO> BuscarPorAssinatura(string _asn_num_assinatura, int numpagina = 1, int linhas = 10)
        {
            IList<CHEQUE_DEVOLVIDO> query = this.ListarPorAssinatura(_asn_num_assinatura);

            return ToDTOPage(query, numpagina, linhas);
        }

    }

}
