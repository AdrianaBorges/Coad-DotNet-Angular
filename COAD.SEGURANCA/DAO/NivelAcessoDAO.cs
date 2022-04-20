using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.SEGURANCA.Repositorios.Base;
using Coad.GenericCrud.Dao.Base;
using COAD.SEGURANCA.Repositorios.Contexto;
using COAD.SEGURANCA.Model;
using Coad.GenericCrud.Dao.Base.Pagination;
using GenericCrud.Config.DataAttributes;

namespace COAD.CORPORATIVO.DAO
{
    [DAOConfig("coadsys")]
    public class NivelAcessoDAO : AbstractGenericDao<NIVEL_ACESSO, NivelAcessoDTO,int>
    {
        public NivelAcessoDAO()
        {
        }
    }
}
