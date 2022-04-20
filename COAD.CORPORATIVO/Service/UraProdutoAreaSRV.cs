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

    [ServiceConfig("URA_ID","PRO_ID","UF_SIGLA_ACESSO","ACO_ID")]
    public class UraProdutoAreaSRV : ServiceAdapter<URA_PRODUTO_AREA, UraProdutoAreaDTO>
    {
        public UraProdutoAreaDAO _dao = new UraProdutoAreaDAO();

        public UraProdutoAreaSRV()
        {
            SetDao(_dao);
        }
        public IList<UraProdutoAreaDTO> BuscarAreas(string _ura_id, int _pro_id)
        {
            return _dao.BuscarAreas(_ura_id, _pro_id);
        }
        public IList<UraProdutoAreaDTO> BuscarAreas(string _ura_id, int _pro_id, string _ufacesso)
        {
            return _dao.BuscarAreas(_ura_id, _pro_id, _ufacesso);
        }
    }
}
