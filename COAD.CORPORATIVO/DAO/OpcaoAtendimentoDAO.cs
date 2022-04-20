using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Model.Dto;
using Coad.GenericCrud.Repositorios.Base;

namespace COAD.CORPORATIVO.DAO
{
    public class OpcaoAtendimentoDAO : DAOAdapter<OPCAO_ATENDIMENTO, OpcoesAtendimentoDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public OpcaoAtendimentoDAO()
        {
            db = GetDb<COADCORPEntities>();
        }
        public IList<OpcoesAtendimentoDTO> BuscarSetorDeTelefones()
        {
            var listaSetorTelefone = (from x in db.OPCAO_ATENDIMENTO where x.OPC_ATIVO_TELEFONE == 1 select x);
            return ToDTO(listaSetorTelefone);
        }

        public IList<OpcoesAtendimentoDTO> BuscarSetorDeEmails()
        {
            var listaSetorEmail = (from x in db.OPCAO_ATENDIMENTO select x);
            return ToDTO(listaSetorEmail);
        }
    }
}
