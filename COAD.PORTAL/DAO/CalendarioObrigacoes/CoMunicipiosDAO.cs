using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Dao.Base;
using COAD.PORTAL.Model.DTO.CalendarioObrigacoes;
using COAD.PORTAL.Repositorios.Contexto;

namespace COAD.PORTAL.DAO.CalendarioObrigacoes
{
    public class CoMunicipiosDAO : AbstractGenericDao<CO_MUNICIPIOS, CoMunicipiosDTO, string>
    {
        public CoMunicipiosDAO()
        {
            SetProfileName("portal");
        }
    }
}
