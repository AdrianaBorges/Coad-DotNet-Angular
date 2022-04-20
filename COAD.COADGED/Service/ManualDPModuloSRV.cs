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
    [ServiceConfig("MOD_ID")]
    public class ManualDPModuloSRV : GenericService<MANUAL_DP_MODULO, ManualDPModuloDTO, int>
    {
        private ManualDPModuloDAO _dao = new ManualDPModuloDAO();

        public ManualDPModuloSRV()
        {
            Dao = _dao;
        }
        public IList<ManualDPModuloDTO> BuscarModulos()
        {
            return _dao.BuscarModulos();
        }
    }
}
