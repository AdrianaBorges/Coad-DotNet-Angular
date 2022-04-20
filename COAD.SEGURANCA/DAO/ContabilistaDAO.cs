using Coad.GenericCrud.Repositorios.Base;
using COAD.SEGURANCA.Model.Dto;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.SEGURANCA.DAO
{
    [DAOConfig("coadsys")]
    public class ContabilistaDAO : DAOAdapter<CONTABILISTA, ContabilistaDTO, int>
    {
        public COADSYSEntities db { get { return GetDb<COADSYSEntities>(); } set { } }

        public ContabilistaDAO()
        {
            db = GetDb<COADSYSEntities>();
        }
    }
}
