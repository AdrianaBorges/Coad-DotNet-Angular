using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using Coad.GenericCrud.Dao.Base;


namespace COAD.CORPORATIVO.DAO
{
    public class NivelRepresentanteDAO : AbstractGenericDao<NIVEL_REPRESENTANTE, NivelRepresentanteDTO, int>
    {
        public IList<NivelRepresentanteDTO> ListarNivelRepresentante(int? nivelMinimo = null, bool? franquia = null)
        {
            var query = GetDbSet().Where(x => x.NRP_NIVEL >= nivelMinimo && (franquia == null || x.FRANQUIA == franquia));

            return ToDTO(query);
        }

        public NivelRepresentanteDTO FindByPerId(string perId)
        {
            var query = GetDbSet().Where(x => x.PER_ID == perId);
            var obj = query.FirstOrDefault();

            return ToDTO(obj);
        }
    }
}
