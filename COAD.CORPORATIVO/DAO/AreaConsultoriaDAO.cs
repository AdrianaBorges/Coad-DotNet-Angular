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
    public class AreaConsultoriaDAO : DAOAdapter<AREA_CONSULTORIA, AreaConsultoriaDTO>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public AreaConsultoriaDAO()
        {
            this.db = GetDb<COADCORPEntities>();
        }

        public IList<AreaConsultoriaDTO> BuscarNaoCadastrada(int _prod_id, string _ura_id, string _uf_sigla)
        {
            List<int> areas = new List<int>();

            List<URA_PRODUTO_AREA> _uraconfig = this.db.URA_PRODUTO_AREA.Where(x => x.PRO_ID == _prod_id && x.URA_ID == _ura_id && x.UF_SIGLA_ACESSO == _uf_sigla).ToList();

            foreach (var _item in _uraconfig)
            {
                areas.Add(_item.ACO_ID);
            }

            var query = this.db.AREA_CONSULTORIA.Where(x => !areas.Contains(x.ACO_ID));

            return ToDTO(query);
        }

    }

}
