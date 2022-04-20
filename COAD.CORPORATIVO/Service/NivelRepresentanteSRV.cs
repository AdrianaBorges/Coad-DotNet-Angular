
using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Repositorios.Base;
using GenericCrud.Config.DataAttributes;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("NRP_ID")]
    public class NivelRepresentanteSRV : GenericService<NIVEL_REPRESENTANTE, NivelRepresentanteDTO, int>
    {   
        public NivelRepresentanteDAO _dao = new NivelRepresentanteDAO();

        public NivelRepresentanteSRV()
        {
            this.Dao = _dao;
        }

        public IList<NivelRepresentanteDTO> ListarNivelRepresentante(int? nivelMinimo = null, bool? franquia = null)
        {
            return _dao.ListarNivelRepresentante(nivelMinimo, franquia);
        }

        public NivelRepresentanteDTO FindByPerId(string perId)
        {
            return _dao.FindByPerId(perId);
        }
    }
}
