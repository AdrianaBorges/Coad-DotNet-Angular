using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Service.Base;
using COAD.PORTAL.DAO.CalendarioObrigacoes;
using COAD.PORTAL.Model.DTO.CalendarioObrigacoes;
using COAD.PORTAL.Repositorios.Contexto;

namespace COAD.PORTAL.Service.CalendarioObrigacoes
{
    public class CoCalendarioSRV : GenericService<CO_CALENDARIO, CoCalendarioDTO, string>
    {
        private CoCalendarioDAO _dao = new CoCalendarioDAO();

        public CoCalendarioSRV()
        {
            Dao = _dao;
           
        }

        public Pagina<CoCalendarioDTO> CalendarioFiltro(string dataIni, string abrangencia = "", string area = "", string estado = "", string municipio = "", int pagina = 1, int nLinha = 7)
        {
            DateTime parseDataIni = DateTime.Now;
            DateTime parseDataFim = DateTime.Now;

            if (string.IsNullOrEmpty(area))
                area = "";

            if (!string.IsNullOrEmpty(dataIni) && DateTime.TryParse(dataIni, out parseDataIni))
                parseDataIni = DateTime.Parse(dataIni);

            //if (!string.IsNullOrEmpty(dataFim) && DateTime.TryParse(dataFim, out parseDataFim))
            //    parseDataFim = DateTime.Parse(dataFim);
            //else
            //    parseDataFim = parseDataIni.AddDays(1);
            
            return _dao.Calendario(parseDataIni, abrangencia, municipio, estado, area, pagina, nLinha);
        }

        public Pagina<CoCalendarioDTO> CalendarioAtual(int pagina = 1, int nLinha = 7)
        {
            return _dao.CalendarioAtual(pagina,nLinha);
        }

        public IList<CoCalendarioDTO> CalendarioTop()
        {
            return _dao.CaledarioTop();
        }

        public IList<CoCalendarioDTO> CalendarioAtual()
        {
            return _dao.CalendarioAtual();
        }

        public CoCalendarioDTO CalendarioPorIDObrigacao(int id)
        {
            return _dao.CalendarioPorIDObrigacao(id);
        }

        public IList<CoCalendarioDTO> CalendarioPorData(string data)
        {
            DateTime parseDataIni = DateTime.Now;

            if (!string.IsNullOrEmpty(data) && DateTime.TryParse(data, out parseDataIni))
                parseDataIni = DateTime.Parse(data);

            return _dao.CalendarioPorData(parseDataIni);
        }
    }
}
