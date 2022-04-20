using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.CORPORATIVO.LEGADO.Model.Dto;
using COAD.CORPORATIVO.LEGADO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.LEGADO.Dao
{
    public class AgendaDAO : AbstractGenericDao<AGENDA, AgendaDTO, object>
    {
        public corporativo2Entities db { get { return GetDb<corporativo2Entities>(); } set { } }
        
        public AgendaDAO()
        {
            SetProfileName("corp_old");
            db = GetDb<corporativo2Entities>();
        }   

        public Pagina<AgendaDTO> BuscarPorAssinatura(string _assinatura = null, int pagina = 1, int registroPorPagina = 10)
        {
            IQueryable<AGENDA> query = GetDbSet().Where(x => x.CODIGO == _assinatura).OrderByDescending(x => x.DATA_HIST);

            return ToDTOPage(query, pagina, registroPorPagina);
        }

        public IList<AgendaDTO> ListarPorAssinatura(string _assinatura)
        {
            IQueryable<AGENDA> query = GetDbSet().Where(x => x.CODIGO == _assinatura && x.DATA_HIST.Length==10).OrderByDescending(x => x.DATA_HIST.Substring(6,4)).ThenByDescending(x => x.DATA_HIST.Substring(3, 2)).ThenByDescending(x => x.DATA_HIST.Substring(0, 2));

            return ToDTO(query);
        }
        public IList<AgendaDTO> BuscarPorCliente(string _cli_id)
        {
            char valor = System.Convert.ToChar("0");

            _cli_id = _cli_id.PadLeft(2, valor);

            IQueryable<AGENDA> query =  (from a in db.AGENDA 
                                         join i in db.ASSINATURA on a.CODIGO equals i.CODIGO_UNIX
                                        where i.CODIGO == _cli_id
                                       select a).OrderByDescending(x => x.DATA_HIST);
                
                //GetDbSet().Where(x => x. == _assinatura).OrderByDescending(x => x.DATA_HIST);

            return ToDTO(query);
        }
        public Pagina<AgendaDTO> BuscarPorCliente(string _cli_id, int pagina = 1, int registroPorPagina = 10)
        {
            char valor = System.Convert.ToChar("0");

            _cli_id = _cli_id.PadLeft(2, valor);

            IQueryable<AGENDA> query = (from a in db.AGENDA
                                        join i in db.ASSINATURA on a.CODIGO equals i.CODIGO_UNIX
                                        where i.CODIGO == _cli_id
                                        select a).OrderByDescending(x => x.DATA_HIST);


            return ToDTOPage(query, pagina, registroPorPagina);
        }


    }
}
