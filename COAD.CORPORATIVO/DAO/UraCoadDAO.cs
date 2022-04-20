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
    public class UraCoadDAO : DAOAdapter<URA_COAD, UraCoadDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public UraCoadDAO()
        {
            db = GetDb<COADCORPEntities>();
        }
        public Pagina<UraCoadDTO> BuscarClientes(string _ura_id, string _asn_id, string _telefone, int numpagina = 1, int linhas = 10)
        {
            IList<URA_COAD> query = (from u in db.URA_COAD
                                    where (
                                            ((_telefone == null || _telefone == "") || (_telefone != null && u.TELEFONE == _telefone)) &&
                                            ((_asn_id == null || _asn_id == "") || (_asn_id != null && u.CODIGO == _asn_id)) &&
                                            ((_ura_id == null || _ura_id == "") || (_ura_id != null && u.URAID == _ura_id))
                                          )
                                   select u).ToList();

            return ToDTOPage(query, numpagina, linhas);
        }
        public bool ImpEtiqueta(int _acao_id)
        {
            return (bool)db.ACAO_ATENDIMENTO.Where(x => x.ACA_ID == _acao_id).FirstOrDefault().ACA_IMP_ETIQUETA;
        }
    }
}
