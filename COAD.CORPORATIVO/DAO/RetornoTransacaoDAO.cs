using Coad.GenericCrud.Dao.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.DAO
{
    public class RetornoTransacaoDAO : AbstractGenericDao<RETORNO_TRANSACAO, RetornoTransacaoDTO, int>
    {
public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public RetornoTransacaoDAO()
        {
            this.db = GetDb<COADCORPEntities>();
        }

    }
}
