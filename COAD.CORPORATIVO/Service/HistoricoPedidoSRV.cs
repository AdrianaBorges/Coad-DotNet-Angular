
using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Repositorios.Base;
using GenericCrud.Config.DataAttributes;
using Coad.GenericCrud.Dao.Base.Pagination;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("HIP_ID")]
    public class HistoricoPedidoSRV : GenericService<HISTORICO_PEDIDO, HistoricoPedidoDTO, int>
    {
        private HistoricoPedidoDAO _dao = new HistoricoPedidoDAO();
        private RepresentanteSRV _representanteService = new RepresentanteSRV();

        public HistoricoPedidoSRV()
        {
            _dao = new HistoricoPedidoDAO();
            _representanteService = new RepresentanteSRV();
            this.Dao = _dao;
        }

        public HistoricoPedidoSRV(HistoricoPedidoDAO _dao)
        {
            this._dao = _dao;
            this.Dao = _dao;
        }

        public void RegistrarHistorico(DateTime? date, string observacao, string usuario, int? REP_ID, int? PST_ID = null, int? IPE_ID = null, int? PPI_ID = null)
        {
            HistoricoPedidoDTO hist = new HistoricoPedidoDTO();
            hist.HIP_DATA = date;
            hist.HIP_DESCRICAO = observacao;
            hist.PST_ID = PST_ID;
            hist.REP_ID = REP_ID;
            hist.IPE_ID = IPE_ID;
            hist.PPI_ID = PPI_ID;

            if (!string.IsNullOrEmpty(usuario))
            {
                hist.USU_LOGIN = usuario;
            }

            InserirHistoricoPedido(new List<HistoricoPedidoDTO>() { hist });

        }

        public void InserirHistoricoPedido(IEnumerable<HistoricoPedidoDTO> lstHistoricoAtendimento)
        {
            if (lstHistoricoAtendimento != null)
            {
                SaveAll(lstHistoricoAtendimento);
            }
        }

        public Pagina<HistoricoPedidoDTO> ListarHistoricoPorItemPedido(int? IPE_ID, DateTime? dataInicial = null, DateTime? dataFinal = null, 
            int? PST_ID = null, 
            int pagina = 1, 
            int registroPorPagina = 10,
            int? PPI_ID = null)
        {
            return _dao.ListarHistoricoPorItemPedido(IPE_ID, dataInicial, dataFinal, PST_ID, pagina, registroPorPagina, PPI_ID);
        }

        public IList<HistoricoPedidoDTO> ListarHistoricoPorItemPedidoSemPaginacao(int CLI_ID, DateTime? 
            dataInicial = null, 
            DateTime? dataFinal = null, 
            int? PST_ID = null,
            int? PPI_ID = null)
        {
            return _dao.ListarHistoricoPorItemPedidoSemPaginacao(CLI_ID, dataInicial, dataFinal, PST_ID, PPI_ID);
        }

        public void RegistrarHistoricoEnvioLinkEmail(string usuario, int? REP_ID_EXECUTOU_A_ACAO, string email, int? IPE_ID, int? PST_ID, int? PPI_ID = null)
        {

            var mensagem = "enviou o link de boleto interno para o email '{0}'";
            mensagem = string.Format(mensagem, email);

            RegistrarHistoricoAcaoPagamento(usuario, REP_ID_EXECUTOU_A_ACAO, IPE_ID, PST_ID, mensagem, PPI_ID);
        
        }

        public void RegistrarHistoricoEnvioLinkGateway(string usuario, int? REP_ID_EXECUTOU_A_ACAO, string email, int? IPE_ID, int? PST_ID, int? PPI_ID = null)
        {
            var mensagem = "enviou o link do gateway para o email '{0}'.";
            mensagem = string.Format(mensagem, email);

            RegistrarHistoricoAcaoPagamento(usuario, REP_ID_EXECUTOU_A_ACAO, IPE_ID, PST_ID, mensagem, PPI_ID);
        }

        public void RegistrarHistoricoPagamentoProposta(string usuario, int? REP_ID_EXECUTOU_A_ACAO, int? PPI_ID, int? PST_ID, string tipoPagamento = null)
        {
            string mensagem = string.Format("O {0} do item da proposta foi pago.", tipoPagamento);
            RegistrarHistorico(DateTime.Now, mensagem, usuario, REP_ID_EXECUTOU_A_ACAO, PST_ID, null, PPI_ID);
        }

        public void RegistrarHistoricoConfirmarPagamento(string usuario, int? REP_ID_EXECUTOU_A_ACAO, int? PPI_ID, int? PST_ID)
        {

            var representanteQueExecutouAAcao = _representanteService.FindById(REP_ID_EXECUTOU_A_ACAO);

            string nomeRepresentanteQueExecutouAAcao =
                                (representanteQueExecutouAAcao != null && !string.IsNullOrWhiteSpace(representanteQueExecutouAAcao.REP_NOME))
                                    ? representanteQueExecutouAAcao.REP_NOME : "(Nome indisponível)";

            string mensagem = string.Format("O representante {0} confirmou que o item da proposta foi pago e seus dados estão corretos.", nomeRepresentanteQueExecutouAAcao);
            RegistrarHistorico(DateTime.Now, mensagem, usuario, REP_ID_EXECUTOU_A_ACAO, PST_ID, null, PPI_ID);
        }

        public void RegistrarHistoricoPagamentoInformadoProposta(string usuario, int? REP_ID_EXECUTOU_A_ACAO, int? PPI_ID, int? PST_ID, string observacoes = null)
        {

            var representanteQueExecutouAAcao = _representanteService.FindById(REP_ID_EXECUTOU_A_ACAO);

            string nomeRepresentanteQueExecutouAAcao =
                                (representanteQueExecutouAAcao != null && !string.IsNullOrWhiteSpace(representanteQueExecutouAAcao.REP_NOME))
                                    ? representanteQueExecutouAAcao.REP_NOME : "(Nome indisponível)";

            string mensagem = string.Format("O representante {0} informou manualmente que o item da proposta foi pago." + 
                " Essa proposta está pendente de conferência. Informações adicionais: {1}", nomeRepresentanteQueExecutouAAcao, observacoes);
            
            RegistrarHistorico(DateTime.Now, mensagem, usuario, REP_ID_EXECUTOU_A_ACAO, PST_ID, null, PPI_ID);
        }

        public void RegistrarHistoricoAcaoPagamento(string usuario, int? REP_ID_EXECUTOU_A_ACAO, int? IPE_ID, int? PST_ID, string acao, int? PPI_ID = null)
        {
            DateTime? dataDeHj = DateTime.Now;

            var representanteQueExecutouAAcao = _representanteService.FindById(REP_ID_EXECUTOU_A_ACAO);

            string nomeRepresentanteQueExecutouAAcao =
                                (representanteQueExecutouAAcao != null && !string.IsNullOrWhiteSpace(representanteQueExecutouAAcao.REP_NOME))
                                    ? representanteQueExecutouAAcao.REP_NOME : "(Nome indisponível)";

            string descricao = @"O representante {0} {1} .";

            string mensagemFinal = string.Format(descricao, acao, nomeRepresentanteQueExecutouAAcao);

            //Registrando o histórico no atendimento / pedido
            RegistrarHistorico(dataDeHj, mensagemFinal, usuario, REP_ID_EXECUTOU_A_ACAO, 1, IPE_ID, PPI_ID);

        }

        public void RegistrarHistoricoComprovanteExcluido(string usuario, int? REP_ID_EXECUTOU_A_ACAO, int? PPI_ID, int? IPE_ID ,int? PST_ID, string toStringComprovante)
        {

            var representanteQueExecutouAAcao = _representanteService.FindById(REP_ID_EXECUTOU_A_ACAO);

            string nomeRepresentanteQueExecutouAAcao =
                                (representanteQueExecutouAAcao != null && !string.IsNullOrWhiteSpace(representanteQueExecutouAAcao.REP_NOME))
                                    ? representanteQueExecutouAAcao.REP_NOME : "(Nome indisponível)";

            string semantica = null;
            int? codReferencia = null;
            if (IPE_ID != null)
            {
                semantica = "item de pedido";
                codReferencia = IPE_ID;
            }
            else
            if (PPI_ID != null)
            {
                semantica = "item de proposta";
                codReferencia = PPI_ID;
            }
                
            string mensagem = string.Format("O representante {0} excluiu um comprovante do {1} de código {2}. Dados do comprovante. : \n {3}", nomeRepresentanteQueExecutouAAcao, semantica, codReferencia, toStringComprovante);
            RegistrarHistorico(DateTime.Now, mensagem, usuario, REP_ID_EXECUTOU_A_ACAO, PST_ID, IPE_ID, PPI_ID);
        }

        public void RegistrarHistoricoAceiteVendaAPrazo(string usuario, int? REP_ID_EXECUTOU_A_ACAO, int? PPI_ID, int? PST_ID)
        {

            string mensagem = "O cliente aprovou essa proposta à prazo.";
            RegistrarHistorico(DateTime.Now, mensagem, usuario, REP_ID_EXECUTOU_A_ACAO, PST_ID, null, PPI_ID);
        }
    }
}
