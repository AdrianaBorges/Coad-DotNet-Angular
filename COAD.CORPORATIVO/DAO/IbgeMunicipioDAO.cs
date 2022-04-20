using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.DAO
{
    public class IbgeMunicipioDAO : DAOAdapter<IBGE_MUNICIPIO, IbgeMunicipioDTO, string>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public IbgeMunicipioDAO() {

            db = GetDb<COADCORPEntities>();
        }

        public IBGE_MUNICIPIO Buscar(string _ibgecodigo)
        {
            IBGE_MUNICIPIO _ibge = (from e in db.IBGE_MUNICIPIO
                                    where e.IBGE_COD_COMPLETO == _ibgecodigo
                                   select e).First();

            return _ibge;
        }

    }
}
