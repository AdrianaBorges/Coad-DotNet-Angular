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
    public class ClassificacaoDAO : DAOAdapter<CLASSIFICACAO, ClassificacaoDTO>
    {
public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public ClassificacaoDAO()
        {
            this.db = GetDb<COADCORPEntities>();
        }
   
    }
}
