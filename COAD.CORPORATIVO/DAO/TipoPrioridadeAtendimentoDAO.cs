
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
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using Coad.GenericCrud.Dao.Base.Pagination;
using System.Data.Objects;


namespace COAD.CORPORATIVO.DAO
{

    public class TipoPrioridadeAtendimentoDAO : AbstractGenericDao<TIPO_PRIORIDADE_ATENDIMENTO, TipoPrioridadeAtendimentoDTO, int>
    {

    }
}

