using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("URA_ID", "PRO_ID", "UF_SIGLA_ACESSO")]
    public class UraConfigSRV : ServiceAdapter<URA_CONFIG, UraConfigDTO>
    {
        public UraConfigDAO _dao = new UraConfigDAO();

        public UraConfigSRV()
        {
            SetDao(_dao);
        }
        public IList<UraConfigDTO> BuscarConfiguracao(string _ura_id, int _pro_id)
        {
           return  _dao.BuscarConfiguracao(_ura_id, _pro_id);
        }
        public Pagina<UraConfigDTO> BuscarConfiguracaoPaginas(string _ura_id, int _pro_id, int pagina = 1, int itensPorPagina = 10)
        {
            return _dao.BuscarConfiguracaoPaginas(_ura_id, _pro_id, pagina, itensPorPagina);
        }

    }
}
