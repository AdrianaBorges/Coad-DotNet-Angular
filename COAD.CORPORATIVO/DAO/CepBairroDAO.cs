using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.DAO
{
    public class CepBairroDAO : DAOAdapter<CEP_BAIRRO, CepBairroDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public CepBairroDAO()
        {
            db = GetDb<COADCORPEntities>();
        }

        public IList<CepBairroDTO> Buscar(string _bairrodescricao)
        {

            var _query = db.CEP_BAIRRO.Where(x => x.BAR_DESCRICAO.Contains(_bairrodescricao));

            return ToDTO(_query);
        }
    }

}
