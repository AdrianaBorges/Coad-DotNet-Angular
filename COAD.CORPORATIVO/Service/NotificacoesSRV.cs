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
//using Coad.GenericCrud.Service.Base;
using Coad.GenericCrud.Repositorios.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using GenericCrud.Metadatas;
using GenericCrud.Config.DataAttributes;
using GenericCrud.Service;
using COAD.CORPORATIVO.Model.Dto.Custons.Notificacoes;

namespace COAD.CORPORATIVO.Service
{
    
    [ServiceConfig("NTF_ID")]
    public class NotificacoesSRV : GenericService<NOTIFICACOES, NotificacoesDTO, int>
    {
        public NotificacoesDAO _dao = new NotificacoesDAO();

        public NotificacoesSRV()
        {
            this._dao = new NotificacoesDAO();
            this.Dao = _dao;
        }

        public NotificacoesSRV(NotificacoesDAO _dao)
        {
            this._dao = _dao;
            this.Dao = _dao;
        }

        /// <summary>
        /// Retorna a quantidade de notificacoes não lidas de um representante
        /// </summary>
        /// <param name="REP_ID"></param>
        /// <returns></returns>
        public int ChecaQuantidadeNotificacoesNaoLidas(int REP_ID)
        {
            return _dao.ChecaQuantidadeNotificacoesNaoLidas(REP_ID);
        }

        /// <summary>
        /// Lista todas as notificações não lidas do representantes
        /// </summary>
        /// <param name="REP_ID"></param>
        /// <param name="pagina"></param>
        /// <param name="linhasPorPagina"></param>
        /// <returns></returns>
        public Pagina<NotificacoesDTO> ListarNotificacoesNaoLidas(int REP_ID, int pagina = 1, int linhasPorPagina = 15)
        {
            return _dao.Notificacoes(REP_ID, false, pagina: pagina, linhasPorPagina: linhasPorPagina);
        }

        /// <summary>
        /// Traz as 5 notificações mais recentes não lidas de um representante
        /// </summary>
        /// <param name="REP_ID"></param>
        /// <returns></returns>
        public Pagina<NotificacoesDTO> ResumoDeNotificacoes(int REP_ID)
        {
            return _dao.Notificacoes(REP_ID, pagina: 1, linhasPorPagina : 5);
        }

        public NotificacoesDTO LerEMarcarNotificacaoComoLida(int ntf_id)
        {
            var notificacao = FindByIdFullLoaded(ntf_id);
            notificacao.NTF_VISUALIZADO = true;

            Merge(notificacao);
            return notificacao;

        }


        /// <summary>
        /// Lista todas as notificações do representantes
        /// </summary>
        /// <param name="REP_ID"></param>
        /// <param name="pagina"></param>
        /// <param name="linhasPorPagina"></param>
        /// <returns></returns>
        public Pagina<NotificacoesDTO> Notificacoes(int REP_ID, bool? lidas = null, 
            int? tipoNotificacaoId = null, string urgenciaNotificacaoId = null, int pagina = 1, int linhasPorPagina = 20)
        {
            return _dao.Notificacoes(REP_ID, lidas, tipoNotificacaoId, urgenciaNotificacaoId, pagina, linhasPorPagina);
        }

        public void InserirNotificacao(int tipoNotificacaoId, string urgenciaNotificacaoId, string mensagem, int? CLI_ID = null, int? repId = null, int? repQueEncaminhou = null, int? codRef= null, string codRefStr = null)
        {
            NotificacoesDTO notificacaoDTO = new NotificacoesDTO()
            {
                NTF_DATA = DateTime.Now,
                NTF_DESCRICAO = mensagem,
                NTF_VISUALIZADO = false,
                NTF_EXIBIDO = false,
                TP_NTF_ID = tipoNotificacaoId,
                URG_NTF_ID = urgenciaNotificacaoId,
                REP_ID = repId,
                CLI_ID = CLI_ID,
                REP_QUE_ENCAMINHOU = repQueEncaminhou,
                NTF_COD_REF_INT = codRef,
                NTF_COD_REF_STR = codRefStr
            };
            Save(notificacaoDTO);
        }

        public void InserirNotificacaoRecebimentoRodizio(string nomeRepresentante, string nomeCliente, int REP_ID, int CLI_ID)
        {

            string mensagem = @"Sua carteria acaba de receber por rodizio um novo suspect: [{0}] ";
            mensagem = string.Format(mensagem, nomeCliente);

            InserirNotificacao(2, @"INFO", mensagem, CLI_ID, REP_ID);
        }

        
        public void InserirNotificacaoParaGerenteRecebimentoRodizio(string nomeRepresentante, string nomeCliente, int GERENTE_REP_ID, int CLI_ID)
        {

            string mensagem = @"O prospect [{0}] foi adicionado com sucesso a operadora {1} ";
            mensagem = string.Format(mensagem, nomeCliente, nomeRepresentante);

            InserirNotificacao(2, @"SUCCESS", mensagem, CLI_ID, GERENTE_REP_ID);
        }

        public void InserirNotificacaoNovoCliente(string nomeRepresentante, string nomeCliente, int REP_ID, int CLI_ID)
        {

            string mensagem = @"Sua carteria acaba de receber um novo suspect: [{0}] ";
            mensagem = string.Format(mensagem, nomeCliente);

            InserirNotificacao(5, @"INFO", mensagem, CLI_ID, REP_ID);
        }

        public void InserirNotificacaoPedidoCancelado(int? REP_ID_EXECUTOU_ACAO, int? REP_ID_RECETOR, int? CLI_ID, int? PED_CRM_ID, int? IPE_ID, string motivos)
        {
            // Recuperando as informações dos representantes para registrar seus nomes no histórico
            var representanteQueExecutouAAcao = ServiceFactory.RetornarServico<RepresentanteSRV>().FindById(REP_ID_EXECUTOU_ACAO);

            string nomeRepresentanteQueExecutouAAcao =
                                (representanteQueExecutouAAcao != null && !string.IsNullOrWhiteSpace(representanteQueExecutouAAcao.REP_NOME))
                                    ? representanteQueExecutouAAcao.REP_NOME : "(Nome indisponível)";


            var cliente = ServiceFactory.RetornarServico<ClienteSRV>().FindById(CLI_ID);

            string nomeCliente =
                                (cliente != null && !string.IsNullOrWhiteSpace(cliente.CLI_NOME))
                                    ? representanteQueExecutouAAcao.REP_NOME : "(Nome indisponível)";
            

            string mensagem = @"O representante de nome {0} cancelou o item de pedido de código: {1} que pertence pedido de número: {2} pelos seguintes motivos: {3}";
            mensagem = string.Format(mensagem, nomeRepresentanteQueExecutouAAcao, IPE_ID, PED_CRM_ID, motivos);

            InserirNotificacao(3, @"WARN", mensagem, CLI_ID, REP_ID_RECETOR);
        }

        public void InserirNotificacaoPropostaCancelada(int? REP_ID_EXECUTOU_ACAO, int? REP_ID_RECETOR, int? CLI_ID, int? PPI_ID, int? PRT_ID, string motivos)
        {
            // Recuperando as informações dos representantes para registrar seus nomes no histórico
            var representanteQueExecutouAAcao = ServiceFactory.RetornarServico<RepresentanteSRV>().FindById(REP_ID_EXECUTOU_ACAO);

            string nomeRepresentanteQueExecutouAAcao =
                                (representanteQueExecutouAAcao != null && !string.IsNullOrWhiteSpace(representanteQueExecutouAAcao.REP_NOME))
                                    ? representanteQueExecutouAAcao.REP_NOME : "(Nome indisponível)";


            var cliente = ServiceFactory.RetornarServico<ClienteSRV>().FindById(CLI_ID);

            string nomeCliente =
                                (cliente != null && !string.IsNullOrWhiteSpace(cliente.CLI_NOME))
                                    ? representanteQueExecutouAAcao.REP_NOME : "(Nome indisponível)";


            string mensagem = @"O representante de nome {0} cancelou o item de proposta de código: {1} que pertence ao pedido de número: {2} pelos seguintes motivos: {3}";
            mensagem = string.Format(mensagem, nomeRepresentanteQueExecutouAAcao, PPI_ID, PRT_ID, motivos);

            InserirNotificacao(3, @"WARN", mensagem, CLI_ID, REP_ID_RECETOR);
        }



        public void InserirNotificacaoAprovacaoDesconto(int? REP_ID_EXECUTOU_ACAO, int? REP_ID_RECETOR, int? CLI_ID, int? PED_CRM_ID, int? IPE_ID)
        {
            // Recuperando as informações dos representantes para registrar seus nomes no histórico
            var representanteQueExecutouAAcao = new RepresentanteSRV().FindById(REP_ID_EXECUTOU_ACAO);

            string nomeRepresentanteQueExecutouAAcao =
                                (representanteQueExecutouAAcao != null && !string.IsNullOrWhiteSpace(representanteQueExecutouAAcao.REP_NOME))
                                    ? representanteQueExecutouAAcao.REP_NOME : "(Nome indisponível)";


            var cliente = new ClienteSRV().FindById(CLI_ID);

            string nomeCliente =
                                (cliente != null && !string.IsNullOrWhiteSpace(cliente.CLI_NOME))
                                    ? cliente.CLI_NOME : "(Nome indisponível)";


            string mensagem = @"O representante de nome {0} aprovou o desconto no item de pedido de código: {1} que pertence pedido de número: {2}";
            mensagem = string.Format(mensagem, nomeRepresentanteQueExecutouAAcao, IPE_ID, PED_CRM_ID);

            InserirNotificacao(3, @"INFO", mensagem, CLI_ID, REP_ID_RECETOR);
        }


        public void InserirNotificacaoNotaDeAntecipacao(int? REP_ID, int? PPI_ID)
        {
            string mensagem = @"uma nota antecipada foi gerado no item de proposta {0}";
            mensagem = string.Format(mensagem, PPI_ID);

            InserirNotificacao(3, @"INFO", mensagem, null, REP_ID);
        }

        public void MarcarTodasAsNotificacoesComoLidas(int? REP_ID)
        {
            _dao.MarcarTodasAsNotificacoesComoLidas(REP_ID);

            //if (lst != null)
            //{
            //    foreach (var nf in lst)
            //    {
            //        nf.NTF_VISUALIZADO = true;
            //    }

            //    SaveOrUpdateAll(lst);
            //}
        }

        /// <summary>
        /// Busca as últimas 5 notificações não visualizadas e exibidas na tela por meio de popup.
        /// </summary>
        /// <param name="REP_ID"></param>
        /// <returns></returns>
        [MetodoTopLevelReferenciavel]
        public IList<NotificacoesDTO> ListarNotificacoesNaoExibidas(int? REP_ID)
        {
            return _dao.ListarNotificacoesNaoExibidas(REP_ID);
        }


        /// <summary>
        /// Indica que uma notificação já foi exibida na tela.
        /// </summary>
        [MetodoTopLevelReferenciavel]
        public void MarcarNotificacaoComoExibida(int? ntfId)
        {
            if (ntfId != null)
            {   var notify = FindById(ntfId);
                notify.NTF_EXIBIDO = true;

                SaveOrUpdate(notify);
            }
        }

        public void InserirNotiPagPropostaPraRep(NotificacaoPropostaPagaDTO dados)
        {
            InserirNotificacao(6, @"INFO", dados.Mensagem, dados.CliId, dados.RepId);
        }

        public void InserirNotiPropostaApro(NotificacaoPropostaPagaDTO dados)
        {
            InserirNotificacao(3, @"INFO", dados.Mensagem, dados.CliId, dados.RepId);
        }

        public void InserirNotiRegistroLiberacao(NotificacaoRegistroLiberacaoDTO dados)
        {
            var tipo = (dados.Aceito == true) ? @"INFO" : @"WARN";
            InserirNotificacao(6, tipo, dados.Mensagem, dados.CliId, dados.RepId);
        }

        public bool ChecaRepreJaNotificado(int? repId, int? tpNtfID, int? codRef)
        {
            return _dao.ChecaRepreJaNotificado(repId, tpNtfID, codRef);
        }

        public bool ChecaRepreJaNotificado(int? repId, int? ntTpID, string codRef)
        {
            return _dao.ChecaRepreJaNotificado(repId, ntTpID, codRef);
        }

    }
}
