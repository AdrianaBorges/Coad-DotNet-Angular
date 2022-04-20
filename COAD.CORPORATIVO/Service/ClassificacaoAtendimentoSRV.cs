using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Service
{
    public class ClassificacaoAtendimentoSRV : ServiceAdapter<CLASSIFICACAO_ATENDIMENTO, ClassificacaoAtendimentoDTO, int>
    {
        private ClassificacaoAtendimentoDAO _dao = new ClassificacaoAtendimentoDAO();

        public ClassificacaoAtendimentoSRV()
        {
            SetDao(_dao);
        }
    }
}
