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


namespace COAD.CORPORATIVO.Service
{
    public class TipoPrioridadeAtendimentoSRV : GenericService<TIPO_PRIORIDADE_ATENDIMENTO, TipoPrioridadeAtendimentoDTO, int>
    {
        public TipoPrioridadeAtendimentoDAO _dao = new TipoPrioridadeAtendimentoDAO();

        public TipoPrioridadeAtendimentoSRV()
        {
            this.Dao = _dao;
        }       

    }
}

