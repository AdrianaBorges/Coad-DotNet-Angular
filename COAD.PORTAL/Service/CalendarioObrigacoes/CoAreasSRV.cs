using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Service.Base;
using COAD.PORTAL.DAO.CalendarioObrigacoes;
using COAD.PORTAL.Model.DTO.CalendarioObrigacoes;
using COAD.PORTAL.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace COAD.PORTAL.Service.CalendarioObrigacoes
{
    public class CoAreasSRV : GenericService<CO_AREAS, CoAreasDTO, string>
    {
        private CoAreasDAO _dao = new CoAreasDAO();

        public CoAreasSRV()
        {
            Dao = _dao;
           
        }

        public Pagina<CoAreasDTO> Areas(string codigoArea, string nomeArea, int pagina = 1, int nLinha = 7)
        { 
            return _dao.Areas(codigoArea, nomeArea, pagina , nLinha);
        }

        public IList<CoAreasDTO> AreasCalendario(IList<CoCalendarioDTO> calendarios)
        {
            var areasDisponiveis = (from data in calendarios select new { data.CO_OBRIGACOES.CO_AREAS }).Distinct().ToList();
            CoAreasDTO cadto = null;
            List<CoAreasDTO> areas = new List<CoAreasDTO>();
            if (areasDisponiveis != null && areasDisponiveis.Count() > 0)
            {
                foreach (var area in areasDisponiveis)
                {
                    cadto = new CoAreasDTO();
                    cadto.COD_AREA = area.CO_AREAS.COD_AREA;
                    cadto.NOME_AREA = area.CO_AREAS.NOME_AREA;
                    areas.Add(cadto);
                }
            }

            return areas;
        }

    }
}
