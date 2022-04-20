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
    public class AcaoAtendimentoSRV : ServiceAdapter<ACAO_ATENDIMENTO, AcaoAtendimentoDTO, int>
    {
        private AcaoAtendimentoDAO _dao = new AcaoAtendimentoDAO();

        public AcaoAtendimentoSRV()
        {
            SetDao(_dao);
        }
        public bool ImpEtiqueta(int _acao_id)
        {
            return _dao.ImpEtiqueta(_acao_id);
        }

    }
}
