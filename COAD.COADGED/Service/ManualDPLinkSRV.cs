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
    [ServiceConfig("LNK_TAG")]
    public class ManualDPLinkSRV : GenericService<MANUAL_DP_LINK, ManualDPLinkDTO, String>
    {
        private ManualDPLinkDAO _dao = new ManualDPLinkDAO();

        public ManualDPLinkSRV()
        {
            Dao = _dao;
        }
        public IList<ManualDPLinkDTO> Listar(int? _mai_id)
        {
            return _dao.Listar(_mai_id);
        }
        public IList<ManualDPLinkDTO> ListarPorModulo(int? _man_id)
        {
            return _dao.ListarPorModulo(_man_id);
        }
    }
}
