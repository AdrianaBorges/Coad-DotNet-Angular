using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Service
{
    public class IbgeMunicipioSRV : ServiceAdapter<IBGE_MUNICIPIO, IbgeMunicipioDTO, string>
    {
        private IbgeMunicipioDAO _dao = new IbgeMunicipioDAO();

        public IbgeMunicipioSRV()
        {
            SetDao(_dao);
        }

        public IBGE_MUNICIPIO Buscar(string _ibgecodigo)
        {
            return _dao.Buscar(_ibgecodigo);
        }
    }
}
