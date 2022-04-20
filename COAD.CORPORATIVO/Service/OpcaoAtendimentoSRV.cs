using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Model.Dto;
using Coad.GenericCrud.Service.Base;

namespace COAD.CORPORATIVO.Service
{
    public class OpcaoAtendimentoSRV : ServiceAdapter<OPCAO_ATENDIMENTO, OpcoesAtendimentoDTO, int>
    {
        private OpcaoAtendimentoDAO _dao { get; set; }

        public OpcaoAtendimentoSRV()
        {
            _dao = new OpcaoAtendimentoDAO();
            SetDao(_dao);
        }

        public OpcaoAtendimentoSRV(OpcaoAtendimentoDAO _dao)
        {
            this._dao = _dao;
            SetDao(_dao);
        }

        public IList<OpcoesAtendimentoDTO> BuscarSetorDeTelefones()
        {
            return _dao.BuscarSetorDeTelefones();
        }

        public IList<OpcoesAtendimentoDTO> BuscarSetorDeEmails()
        {
            return _dao.BuscarSetorDeEmails();
        }
    }
}
