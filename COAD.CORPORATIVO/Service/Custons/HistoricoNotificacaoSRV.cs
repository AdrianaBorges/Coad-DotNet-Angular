using Coad.Reflection;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.CORPORATIVO.Model.Dto.Custons.Historicos;
using COAD.CORPORATIVO.Model.Dto.Formatters;
using GenericCrud.Metadatas;
using GenericCrud.Models.HistoryRegister;
using GenericCrud.Models.MessageFormatter;
using GenericCrud.Service;
using GenericCrud.Service.Formatting;
using GenericCrud.Service.HistoryRegister;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Service.Custons
{
    public class HistoricoNotificacaoSRV : HistoryRegisterService
    {
        public RepresentanteSRV _representanteService { get; set; }
        public RegiaoSRV _regiaoSRV { get; set; }
        public UENSRV _uenSRV { get; set; }

        public HistAtendSRV histAtend { get; set; }
        public HistoricoPedidoSRV histPedido { get; set; }
        public NotificacoesSRV notificacaoSRV { get; set; }

        public HistoricoNotificacaoSRV()
        {
            this.histAtend = new HistAtendSRV();
            this.histPedido = new HistoricoPedidoSRV();
            this._representanteService = new RepresentanteSRV();
            this._regiaoSRV = new RegiaoSRV();
            this._uenSRV = new UENSRV();
            this.notificacaoSRV = new NotificacoesSRV();
            Init();
        }

        public HistoricoNotificacaoSRV(HistAtendSRV histAtendSRV, HistoricoPedidoSRV histPedidoSRV, MessageFormatterService messageService) 
            : base(messageService)
        {
            this.histAtend = histAtendSRV;
            this.histPedido = histPedidoSRV;
            Init();
        }
        
        public void Init()
        {
            var repIdMapper = this.formatterService.AddFormater("repId", new RepIdFormatter());

            repIdMapper.AddAllToken("representante", "representanteQueExecutouAcao");
            repIdMapper.DefinirCampoDeValor("RepId");

            var cliIdMapper = formatterService.AddFormater("cliId", new RepIdFormatter());

            cliIdMapper.AddToken("cliente");
            cliIdMapper.DefinirCampoDeValor("CliId");
            
        }

        /// <summary>
        /// Método especializado para registrar o histórico na emissão da venda.
        /// </summary>
        /// <param name="usuario">Nome do usuário logado</param>
        /// <param name="REP_ID_EXECUTOU_A_ACAO">Id do representante que está executando a adição de carteira</param>
        /// <param name="CLI_ID">Id do cliente, que será adicionado a uma nova região, onde este histórico será anexado.</param>
        public void RegistrarHistoricoPedidoEmitido(string usuario, int? REP_ID_EXECUTOU_A_ACAO, int? CLI_ID, int? IPE_ID, int? cmpId = null, string observacao = null)
        {
            DateTime? dataDeHj = DateTime.Now;

            var produtoService = new ProdutoComposicaoSRV();
            var produtoComposto = produtoService.FindById(cmpId);
            produtoService.ChecaEMarcaProdutoCurso(produtoComposto);

            var classificacao = (produtoComposto.EhCurso) ? "curso" : "produto";

            // Recuperando as informações dos representantes para registrar seus nomes no histórico
            var representanteQueExecutouAAcao = _representanteService.FindById(REP_ID_EXECUTOU_A_ACAO);

            string nomeRepresentanteQueExecutouAAcao =
                                (representanteQueExecutouAAcao != null && !string.IsNullOrWhiteSpace(representanteQueExecutouAAcao.REP_NOME))
                                    ? representanteQueExecutouAAcao.REP_NOME : "(Nome indisponível)";

            string nomeDoProdutoComposto =
                    (produtoComposto != null && !string.IsNullOrWhiteSpace(produtoComposto.CMP_DESCRICAO))
                        ? produtoComposto.CMP_DESCRICAO : "(Nome indisponível)";

            string descricao = @"O representante {0} emitiu um pedido do {1} ('{2}') com as seguintes observações. {3}'";

            string mensagemFinal = string.Format(descricao, nomeRepresentanteQueExecutouAAcao, classificacao, nomeDoProdutoComposto, observacao);

            //Registrando o histórico no atendimento / pedido
            this.histAtend.RegistrarHistorico(dataDeHj, mensagemFinal, usuario, REP_ID_EXECUTOU_A_ACAO, CLI_ID, 15, null, IPE_ID);
            this.histPedido.RegistrarHistorico(dataDeHj, mensagemFinal, usuario, REP_ID_EXECUTOU_A_ACAO, 1, IPE_ID);
        }

        /// <summary>
        /// Método especializado para registrar o histórico na emissão da venda.
        /// </summary>
        /// <param name="usuario">Nome do usuário logado</param>
        /// <param name="REP_ID_EXECUTOU_A_ACAO">Id do representante que está executando a adição de carteira</param>
        /// <param name="CLI_ID">Id do cliente, que será adicionado a uma nova região, onde este histórico será anexado.</param>
        public void RegistrarHistoricoPedidoEmitidoOnline(string usuario, int? CLI_ID, int? IPE_ID, int? cmpId = null)
        {
            DateTime? dataDeHj = DateTime.Now;

            var produtoService = new ProdutoComposicaoSRV();
            var produtoComposto = produtoService.FindById(cmpId);
            produtoService.ChecaEMarcaProdutoCurso(produtoComposto);

            var classificacao = (produtoComposto.EhCurso) ? "curso" : "produto";

            string nomeDoProdutoComposto =
                    (produtoComposto != null && !string.IsNullOrWhiteSpace(produtoComposto.CMP_DESCRICAO))
                        ? produtoComposto.CMP_DESCRICAO : "(Nome indisponível)";

            string descricao = @"O cliente acaba de realizar uma compra do {0} ('{1}')'";

            string mensagemFinal = string.Format(descricao, classificacao, nomeDoProdutoComposto);

            //Registrando o histórico
            this.histAtend.RegistrarHistorico(dataDeHj, mensagemFinal, usuario, null, CLI_ID, 15, null, IPE_ID);
            this.histPedido.RegistrarHistorico(dataDeHj, mensagemFinal, usuario, null, 1, IPE_ID);
        }


        /// <summary>
        /// Método especializado para registrar o histórico no cancelamento de um pedido.
        /// </summary>
        /// <param name="usuario">Nome do usuário logado</param>
        /// <param name="REP_ID_EXECUTOU_A_ACAO">Id do representante que está executando a adição de carteira</param>
        /// <param name="CLI_ID">Id do cliente, que será adicionado a uma nova região, onde este histórico será anexado.</param>
        public void RegistrarHistoricoPedidoCancelado(string usuario, int? CLI_ID, int? REP_ID_EXECUTOU_A_ACAO, int? PED_CRM_ID, int? IPE_ID, string motivosDoCancelamento = "", int? PPI_ID = null)
        {
            DateTime? dataDeHj = DateTime.Now;

            // Recuperando as informações dos representantes para registrar seus nomes no histórico
            var representanteQueExecutouAAcao = _representanteService.FindById(REP_ID_EXECUTOU_A_ACAO);

            string nomeRepresentanteQueExecutouAAcao =
                                (representanteQueExecutouAAcao != null && !string.IsNullOrWhiteSpace(representanteQueExecutouAAcao.REP_NOME))
                                    ? representanteQueExecutouAAcao.REP_NOME : "(Nome indisponível)";

            string descricao = @"O representante de nome {0} cancelou o item de pedido de código: {1} que pertence ao pedido de número: {2} pelos seguintes motivos: {3}";
            string descricaoDoPedido = @"O representante de nome {0} cancelou o item de pedido: {1}";

            string mensagemFinal = string.Format(descricao, nomeRepresentanteQueExecutouAAcao, IPE_ID, PED_CRM_ID, motivosDoCancelamento);
            string mensagemFinalPedido = string.Format(descricaoDoPedido, nomeRepresentanteQueExecutouAAcao, motivosDoCancelamento);

            //Registrando o histórico
            this.histAtend.RegistrarHistorico(dataDeHj, mensagemFinal, usuario, REP_ID_EXECUTOU_A_ACAO, CLI_ID, 16, null, IPE_ID, null, PPI_ID);
            this.histPedido.RegistrarHistorico(dataDeHj, mensagemFinalPedido, usuario, REP_ID_EXECUTOU_A_ACAO, 5, IPE_ID, PPI_ID);
            
        }

        /// <summary>
        /// Método especializado para registrar o histórico no cancelamento de uma proposta.
        /// </summary>
        /// <param name="usuario">Nome do usuário logado</param>
        /// <param name="REP_ID_EXECUTOU_A_ACAO">Id do representante que está executando a adição de carteira</param>
        /// <param name="CLI_ID">Id do cliente, que será adicionado a uma nova região, onde este histórico será anexado.</param>
        public void RegistrarHistoricoPropostaCancelada(string usuario, int? CLI_ID, int? REP_ID_EXECUTOU_A_ACAO, int? PPI_ID, string motivosDoCancelamento = "")
        {
            DateTime? dataDeHj = DateTime.Now;

            // Recuperando as informações dos representantes para registrar seus nomes no histórico
            var representanteQueExecutouAAcao = _representanteService.FindById(REP_ID_EXECUTOU_A_ACAO);

            string nomeRepresentanteQueExecutouAAcao =
                                (representanteQueExecutouAAcao != null && !string.IsNullOrWhiteSpace(representanteQueExecutouAAcao.REP_NOME))
                                    ? representanteQueExecutouAAcao.REP_NOME : "(Nome indisponível)";

            string descricaoDoPedido = @"O representante de nome {0} cancelou a proposta de código: '{1}' pelos seguintes motivos: '{2}'";

            string mensagemFinalPedido = string.Format(descricaoDoPedido, nomeRepresentanteQueExecutouAAcao, PPI_ID, motivosDoCancelamento);

            //Registrando o histórico
            this.histPedido.RegistrarHistorico(dataDeHj, mensagemFinalPedido, usuario, REP_ID_EXECUTOU_A_ACAO, 5, null, PPI_ID);

        }

        /// <summary>
        /// Método especializado para registrar o histórico na alteração de status de um pedido.
        /// </summary>
        /// <param name="usuario">Nome do usuário logado</param>
        /// <param name="REP_ID_EXECUTOU_A_ACAO">Id do representante que está executando a adição de carteira</param>
        /// <param name="CLI_ID">Id do cliente, que será adicionado a uma nova região, onde este histórico será anexado.</param>
        public void RegistrarHistoricoPedidoAlteracaoDeStatus(string usuario, int? CLI_ID, int? REP_ID_EXECUTOU_A_ACAO, int? IPE_ID, string acao, int? acaId = null, int? PST_ID = null)
        {
            DateTime? dataDeHj = DateTime.Now;

            // Recuperando as informações dos representantes para registrar seus nomes no histórico
            var representanteQueExecutouAAcao = _representanteService.FindById(REP_ID_EXECUTOU_A_ACAO);

            string nomeRepresentanteQueExecutouAAcao =
                                (representanteQueExecutouAAcao != null && !string.IsNullOrWhiteSpace(representanteQueExecutouAAcao.REP_NOME))
                                    ? representanteQueExecutouAAcao.REP_NOME : "(Nome indisponível)";

            string descricao = @"O representante {0} alterou o status do item de pedido número: {1} para {2}. ";
            string descricaoPedido = @"O representante {0} alterou o status do item de pedido para: {1}. ";


            string mensagemFinal = string.Format(descricao, nomeRepresentanteQueExecutouAAcao, IPE_ID, acao);
            string mensagemFinalPedido = string.Format(descricaoPedido, nomeRepresentanteQueExecutouAAcao, acao);

            //Registrando o histórico
            this.histAtend.RegistrarHistorico(dataDeHj, mensagemFinal, usuario, REP_ID_EXECUTOU_A_ACAO, CLI_ID, acaId, null, IPE_ID);
            this.histPedido.RegistrarHistorico(dataDeHj, mensagemFinalPedido, usuario, REP_ID_EXECUTOU_A_ACAO, PST_ID, IPE_ID);
        }


        /// <summary>
        /// Método especializado para registrar o histórico na alteração de status de um pedido.
        /// </summary>
        /// <param name="usuario">Nome do usuário logado</param>
        /// <param name="REP_ID_EXECUTOU_A_ACAO">Id do representante que está executando a adição de carteira</param>
        /// <param name="CLI_ID">Id do cliente, que será adicionado a uma nova região, onde este histórico será anexado.</param>
        public void RegistrarHistoricoPedidoAlteracaoParaPagoComPendenciaDeConferencia(string usuario, int? CLI_ID, int? REP_ID_EXECUTOU_A_ACAO, int? IPE_ID, int? PED_CRM_ID, string motivoAlteracaoPago = "")
        {
            DateTime? dataDeHj = DateTime.Now;

            // Recuperando as informações dos representantes para registrar seus nomes no histórico
            var representanteQueExecutouAAcao = _representanteService.FindById(REP_ID_EXECUTOU_A_ACAO);

            string nomeRepresentanteQueExecutouAAcao =
                                (representanteQueExecutouAAcao != null && !string.IsNullOrWhiteSpace(representanteQueExecutouAAcao.REP_NOME))
                                    ? representanteQueExecutouAAcao.REP_NOME : "(Nome indisponível)";

            string descricao = @"O representante de nome {0} marcou manualmente que o item de pedido de código: {1} que pertence ao pedido de número: {2} foi pago. Observações do Representante: {3}";
            string descricaoPedido = @"O representante de nome {0} marcou manualmente que o item de pedido foi pago. Observações do Representante: {1}";

            string mensagemFinal = string.Format(descricao, nomeRepresentanteQueExecutouAAcao, IPE_ID, PED_CRM_ID, motivoAlteracaoPago);
            string mensagemFinalPedido = string.Format(descricaoPedido, nomeRepresentanteQueExecutouAAcao, motivoAlteracaoPago);

            //Registrando o histórico
            this.histAtend.RegistrarHistorico(dataDeHj, mensagemFinal, usuario, REP_ID_EXECUTOU_A_ACAO, CLI_ID, 20, null, IPE_ID);
            this.histPedido.RegistrarHistorico(dataDeHj, mensagemFinalPedido, usuario, REP_ID_EXECUTOU_A_ACAO, 2, IPE_ID);

        }

        /// <summary>
        /// Método especializado para registrar o histórico na alteração de status de um pedido.
        /// </summary>
        /// <param name="usuario">Nome do usuário logado</param>
        /// <param name="REP_ID_EXECUTOU_A_ACAO">Id do representante que está executando a adição de carteira</param>
        /// <param name="CLI_ID">Id do cliente, que será adicionado a uma nova região, onde este histórico será anexado.</param>
        public void RegistrarHistoricoPedidoAlteracaoParaPago(string usuario, int? CLI_ID, int? REP_ID_EXECUTOU_A_ACAO, int? IPE_ID)
        {
            RegistrarHistoricoPedidoAlteracaoDeStatus(usuario, CLI_ID, REP_ID_EXECUTOU_A_ACAO, IPE_ID, "Pago", 17, 7);
        }

        /// <summary>
        /// Método especializado para registrar o histórico na alteração de status de um pedido para aprovado.
        /// </summary>
        /// <param name="usuario">Nome do usuário logado</param>
        /// <param name="REP_ID_EXECUTOU_A_ACAO">Id do representante que está executando a adição de carteira</param>
        /// <param name="CLI_ID">Id do cliente, que será adicionado a uma nova região, onde este histórico será anexado.</param>
        public void RegistrarHistoricoPedidoAlteracaoParaDescontoAprovado(string usuario, int? CLI_ID, int? REP_ID_EXECUTOU_A_ACAO, int? IPE_ID, int? PED_CRM_ID)
        {
            DateTime? dataDeHj = DateTime.Now;

            // Recuperando as informações dos representantes para registrar seus nomes no histórico
            var representanteQueExecutouAAcao = _representanteService.FindById(REP_ID_EXECUTOU_A_ACAO);

            string nomeRepresentanteQueExecutouAAcao =
                                (representanteQueExecutouAAcao != null && !string.IsNullOrWhiteSpace(representanteQueExecutouAAcao.REP_NOME))
                                    ? representanteQueExecutouAAcao.REP_NOME : "(Nome indisponível)";

            string mensagem = @"O representante de nome {0} aprovou o desconto no item de pedido de código: {1} que pertence pedido de número: {2}";
            string mensagemPedido = @"O representante de nome {0} aprovou o desconto no item de pedido.";

            string mensagemFinal = string.Format(mensagem, nomeRepresentanteQueExecutouAAcao, IPE_ID, PED_CRM_ID);
            string mensagemFinalPedido = string.Format(mensagemPedido, nomeRepresentanteQueExecutouAAcao);


            //Registrando o histórico
            this.histAtend.RegistrarHistorico(dataDeHj, mensagemFinal, usuario, REP_ID_EXECUTOU_A_ACAO, CLI_ID, 18, null, IPE_ID, null); 
            this.histPedido.RegistrarHistorico(dataDeHj, mensagemFinalPedido, usuario, REP_ID_EXECUTOU_A_ACAO, 6, IPE_ID);


        }


        /// <summary>
        /// Método especializado para registrar o histórico na alteração de status de um pedido para faturado.
        /// </summary>
        /// <param name="usuario">Nome do usuário logado</param>
        /// <param name="REP_ID_EXECUTOU_A_ACAO">Id do representante que está executando a adição de carteira</param>
        /// <param name="CLI_ID">Id do cliente, que será adicionado a uma nova região, onde este histórico será anexado.</param>
        public void RegistrarHistoricoPedidoAlteracaoParaFaturado(string usuario, int? CLI_ID, int? REP_ID_EXECUTOU_A_ACAO, int? IPE_ID)
        {
            RegistrarHistoricoPedidoAlteracaoDeStatus(usuario, CLI_ID, REP_ID_EXECUTOU_A_ACAO, IPE_ID, "Faturado", 19, 3);

        }

        ///// <summary>
        ///// Método especializado para registrar o histórico do pedido quando ocorre a recusa de pagamento.
        ///// </summary>
        ///// <param name="usuario">Nome do usuário logado</param>
        ///// <param name="REP_ID_EXECUTOU_A_ACAO">Id do representante que está executando a adição de carteira</param>
        ///// <param name="CLI_ID">Id do cliente, que será adicionado a uma nova região, onde este histórico será anexado.</param>
        //public void RegistrarHistoricoPedidoRecusaIndicacaoManualDePagamento(string usuario, int? CLI_ID, int? REP_ID_EXECUTOU_A_ACAO, int? IPE_ID, string observacoes = "")
        //{
        //    DateTime? dataDeHj = DateTime.Now;

        //    // Recuperando as informações dos representantes para registrar seus nomes no histórico
        //    var representanteQueExecutouAAcao = _representanteService.FindById(REP_ID_EXECUTOU_A_ACAO);

        //    string nomeRepresentanteQueExecutouAAcao =
        //                        (representanteQueExecutouAAcao != null && !string.IsNullOrWhiteSpace(representanteQueExecutouAAcao.REP_NOME))
        //                            ? representanteQueExecutouAAcao.REP_NOME : "(Nome indisponível)";

        //    string descricao = @"O representante de nome {0} recusou a indicação manual de pagamento. Motivos: {1}";
        //    string mensagemFinalPedido = string.Format(descricao, nomeRepresentanteQueExecutouAAcao, observacoes);

        //    //Registrando o histórico
        //    this.histPedido.RegistrarHistorico(dataDeHj, mensagemFinalPedido, usuario, REP_ID_EXECUTOU_A_ACAO, 1, IPE_ID);

        //}

        [MetodoAuxiliar]
        private void RegistroIndidual(RegistroHistDTO parametros, string mensagem, bool historicoCliente = false, bool historicoPedido = false, bool notificacao = false)
        {
            var data = DateTime.Now;
            if (historicoPedido) { this.histPedido.RegistrarHistorico(data, mensagem, parametros.UsuLogin, parametros.RepId, parametros.PstId, parametros.IpeId, parametros.PpiId); }
            if (historicoCliente) { this.histAtend.RegistrarHistorico(data, mensagem, parametros.UsuLogin, parametros.RepId, parametros.CliId, parametros.AcaId, parametros.PedCrmId, parametros.IpeId); }
            if (notificacao) { this.notificacaoSRV.InserirNotificacao(parametros.TipoNoticacao, parametros.UrgenciaNotificacao, mensagem, parametros.CliId, parametros.RepId); }

        }


        public void Registrar(RegistroHistDTO parametros)
        {
            DateTime? data = DateTime.Now;

            if (parametros != null)
            {
                if (parametros.Individual)
                {
                    Validar(parametros, parametros.TiposDeRegistro);

                    string mensagemNaoFormatada = parametros.Mensagem;
                    var lstTiposRegistro = parametros.TiposDeRegistro;

                    string mensagem = FormatarMensagem(parametros, mensagemNaoFormatada);

                    foreach (var tRegistro in lstTiposRegistro)
                    {
                        if (tRegistro.Equals(TipoRegistro.TODOS))
                        {
                            RegistroIndidual(parametros, mensagem, true, true, true);
                            break;
                        }
                        else
                        {
                            if (tRegistro.Equals(TipoRegistro.HISTORICO_PEDIDO))
                            {
                                RegistroIndidual(parametros, mensagem, false, true, false);
                            }

                            if (tRegistro.Equals(TipoRegistro.HISTORICO_CLIENTE))
                            {
                                RegistroIndidual(parametros, mensagem, true, false, false);
                            }

                            if (tRegistro.Equals(TipoRegistro.NOTIFICACAO))
                            {
                                RegistroIndidual(parametros, mensagem, false, false, true);
                            }
                        }


                    }
                }
                else
                {
                    var lstMensagemIndividual = parametros.MensagensIndividuais;


                    if (lstMensagemIndividual != null)
                    {
                        Validar(parametros, lstMensagemIndividual.Select(sel => sel.TipoRegistro).ToList());

                        foreach (var msgIndividual in lstMensagemIndividual)
                        {

                            if (msgIndividual.TipoRegistro.Equals(TipoRegistro.HISTORICO_PEDIDO))
                            {
                                var mensagem = msgIndividual.Mensagem;
                                mensagem = base.FormatarMensagem(parametros, mensagem);

                                RegistroIndidual(parametros, mensagem, false, true, false);
                            }

                            if (msgIndividual.TipoRegistro.Equals(TipoRegistro.HISTORICO_CLIENTE))
                            {
                                var mensagem = msgIndividual.Mensagem;
                                mensagem = base.FormatarMensagem(parametros, mensagem);

                                RegistroIndidual(parametros, mensagem, true, false, false);
                            }

                            if (msgIndividual.TipoRegistro.Equals(TipoRegistro.NOTIFICACAO))
                            {
                                var mensagem = msgIndividual.Mensagem;
                                mensagem = base.FormatarMensagem(parametros, mensagem);

                                RegistroIndidual(parametros, mensagem, false, false, true);
                            }
                        }
                    }
                }

            }

        }

        // <summary>
        /// Método especializado para registrar o histórico do pedido quando ocorre a recusa de pagamento.
        /// </summary>
        /// <param name="usuario">Nome do usuário logado</param>
        /// <param name="REP_ID_EXECUTOU_A_ACAO">Id do representante que está executando a adição de carteira</param>
        /// <param name="CLI_ID">Id do cliente, que será adicionado a uma nova região, onde este histórico será anexado.</param>
        public void RegistrarHistoricoPedidoRecusaIndicacaoManualDePagamento(string usuario, int? CLI_ID, int? REP_ID_EXECUTOU_A_ACAO, int? REP_ID_DESTINO, int? IPE_ID = null, string observacoes = "", int? PPI_ID = null)
        {

            string descricaoHistorico = @"O representante de nome {representanteQueExecutouAcao} recusou as informações de pagamento do pedido. Motivos: {obs}";
            string descricaoNotificacao = @"O representante de nome {representanteQueExecutouAcao} recusou as informações de pagamento do pedido número {IpeId} . Motivos: {obs}";


            RegistroHistDTO parametros = new RegistroHistDTO()
            {
                RepIdQueExecutouAcao = REP_ID_EXECUTOU_A_ACAO,
                RepId = REP_ID_DESTINO,
                UsuLogin = usuario,
                CliId = CLI_ID,
                IpeId = IPE_ID,
                PstId = 1,
                Observacoes = observacoes,
                TipoNoticacao = 3,
                UrgenciaNotificacao = "ERROR",

            };

            parametros.AddMensagensIndividuais(TipoRegistro.HISTORICO_PEDIDO, descricaoHistorico);
            parametros.AddMensagensIndividuais(TipoRegistro.NOTIFICACAO, descricaoNotificacao);

            Registrar(parametros);
            //Registrando o histórico

        }

        // <summary>
        /// Método especializado para registrar o histórico da proposta quando ocorre a recusa de pagamento.
        /// </summary>
        /// <param name="usuario">Nome do usuário logado</param>
        /// <param name="REP_ID_EXECUTOU_A_ACAO">Id do representante que está executando a adição de carteira</param>
        /// <param name="CLI_ID">Id do cliente, que será adicionado a uma nova região, onde este histórico será anexado.</param>
        public void RegistrarHistoricoPropostaRecusaIndicacaoManualDePagamento(string usuario, int? CLI_ID, int? REP_ID_EXECUTOU_A_ACAO, int? REP_ID_DESTINO, int? PPI_ID, string observacoes = "")
        {

            string descricaoHistorico = @"O representante de nome {representanteQueExecutouAcao} recusou as informações de pagamento do pedido. Motivos: {obs}";
            string descricaoNotificacao = @"O representante de nome {representanteQueExecutouAcao} recusou as informações de pagamento do Item de proposta número {PpiId} . Motivos: {obs}";


            RegistroHistDTO parametros = new RegistroHistDTO()
            {
                RepIdQueExecutouAcao = REP_ID_EXECUTOU_A_ACAO,
                RepId = REP_ID_DESTINO,
                UsuLogin = usuario,
                CliId = CLI_ID,
                PpiId = PPI_ID,
                PstId = 1,
                Observacoes = observacoes,
                TipoNoticacao = 3,
                UrgenciaNotificacao = "ERROR",

            };

            parametros.AddMensagensIndividuais(TipoRegistro.HISTORICO_PEDIDO, descricaoHistorico);
            parametros.AddMensagensIndividuais(TipoRegistro.NOTIFICACAO, descricaoNotificacao);

            Registrar(parametros);
            //Registrando o histórico

        }


        // <summary>
        /// Método especializado para registrar o histórico do pedido quando ocorre a alteração da observação.
        /// </summary>
        /// <param name="usuario">Nome do usuário logado</param>
        /// <param name="REP_ID_EXECUTOU_A_ACAO">Id do representante que está executando a adição de carteira</param>
        /// <param name="CLI_ID">Id do cliente, que será adicionado a uma nova região, onde este histórico será anexado.</param>
        public void RegistrarHistoricoPedidoAlteracaoObservacoes(string usuario, int? REP_ID_EXECUTOU_A_ACAO, int? IPE_ID, string observacoesOriginal = "", string observacoesFinal = "")
        {

            string descricaoHistorico = @"O representante de nome {representanteQueExecutouAcao} alterou a observação do pedido de '{observacaoOriginal}' para '{observacaoFinal}'";

            RegistroHistDTO parametros = new RegistroHistDTO()
            {
                RepIdQueExecutouAcao = REP_ID_EXECUTOU_A_ACAO,
                RepId = REP_ID_EXECUTOU_A_ACAO,
                UsuLogin = usuario,
                IpeId = IPE_ID,
                PstId = 1,
                Mensagem = descricaoHistorico

            };

            parametros.DefinirTipos(TipoRegistro.HISTORICO_PEDIDO);

            parametros.ParametrosAdicionais.Add("observacaoOriginal", observacoesOriginal);
            parametros.ParametrosAdicionais.Add("observacaoFinal", observacoesFinal);

            //Registrando o histórico
            Registrar(parametros);

        }

        /// <summary>
        /// Método especializado para registrar o histórico na emissão da venda.
        /// </summary>
        /// <param name="usuario">Nome do usuário logado</param>
        /// <param name="REP_ID_EXECUTOU_A_ACAO">Id do representante que está executando a adição de carteira</param>
        /// <param name="CLI_ID">Id do cliente, que será adicionado a uma nova região, onde este histórico será anexado.</param>
        public void RegistrarHistoricoPropostaEmitida(string usuario, int? REP_ID_EXECUTOU_A_ACAO, int? CLI_ID, int? PPI_ID, int? cmpId = null, string observacao = null, bool nova = false)
        {
            DateTime? dataDeHj = DateTime.Now;

            var produtoService = ServiceFactory.RetornarServico<ProdutoComposicaoSRV>();
            var produtoComposto = produtoService.FindById(cmpId);
            produtoService.ChecaEMarcaProdutoCurso(produtoComposto);

            var classificacao = (produtoComposto.EhCurso) ? "curso" : "produto";

            // Recuperando as informações dos representantes para registrar seus nomes no histórico
            var representanteQueExecutouAAcao = _representanteService.FindById(REP_ID_EXECUTOU_A_ACAO);

            string nomeRepresentanteQueExecutouAAcao =
                                (representanteQueExecutouAAcao != null && !string.IsNullOrWhiteSpace(representanteQueExecutouAAcao.REP_NOME))
                                    ? representanteQueExecutouAAcao.REP_NOME : "(Nome indisponível)";

            string nomeDoProdutoComposto =
                    (produtoComposto != null && !string.IsNullOrWhiteSpace(produtoComposto.CMP_DESCRICAO))
                        ? produtoComposto.CMP_DESCRICAO : "(Nome indisponível)";

            string semantica = (nova) ? "emitiu" : "alterou";
            string descricao = @"O representante {0} {1} uma proposta {2} ('{3}') com as seguintes observações. {4}'";

            string mensagemFinal = string.Format(descricao, nomeRepresentanteQueExecutouAAcao, semantica, classificacao, nomeDoProdutoComposto, observacao);

            //Registrando o histórico no atendimento / pedido
            this.histAtend.RegistrarHistorico(dataDeHj, mensagemFinal, usuario, REP_ID_EXECUTOU_A_ACAO, CLI_ID, 15, null, null, 6, PPI_ID);
            this.histPedido.RegistrarHistorico(dataDeHj, mensagemFinal, usuario, REP_ID_EXECUTOU_A_ACAO, 1, null, PPI_ID);
        }

        /// <summary>
        /// Método especializado para registrar o histórico na emissão de pedido a partir de uma proposta já paga.
        /// </summary>
        /// <param name="usuario">Nome do usuário logado</param>
        /// <param name="REP_ID_EXECUTOU_A_ACAO">Id do representante que está executando a adição de carteira</param>
        /// <param name="CLI_ID">Id do cliente, que será adicionado a uma nova região, onde este histórico será anexado.</param>
        public void RegistrarHistoricoPedidoEmitidoAPartirDaProposta(string usuario, int? REP_ID_EXECUTOU_A_ACAO, int? CLI_ID, int? IPE_ID, int? PPI_ID)
        {
            DateTime? dataDeHj = DateTime.Now;
            
            // Recuperando as informações dos representantes para registrar seus nomes no histórico
            var representanteQueExecutouAAcao = _representanteService.FindById(REP_ID_EXECUTOU_A_ACAO);

            string nomeRepresentanteQueExecutouAAcao =
                                (representanteQueExecutouAAcao != null && !string.IsNullOrWhiteSpace(representanteQueExecutouAAcao.REP_NOME))
                                    ? representanteQueExecutouAAcao.REP_NOME : "(Nome indisponível)";


            string descricao = @"O representante '{0}' emitiu um item de pedido de código ({1}) a partir do item de proposta proposta de código ({2})";

            string mensagemFinal = string.Format(descricao, nomeRepresentanteQueExecutouAAcao, IPE_ID, PPI_ID);

            //Registrando o histórico no atendimento / pedido
            this.histAtend.RegistrarHistorico(dataDeHj, mensagemFinal, usuario, REP_ID_EXECUTOU_A_ACAO, CLI_ID, 15, null, IPE_ID, 6, PPI_ID);
            this.histPedido.RegistrarHistorico(dataDeHj, mensagemFinal, usuario, REP_ID_EXECUTOU_A_ACAO, 8, IPE_ID, PPI_ID);
        }

        /// <summary>
        /// Método especializado para registrar o histórico na ação de informar uma venda.
        /// </summary>
        /// <param name="usuario">Nome do usuário logado</param>
        /// <param name="REP_ID_EXECUTOU_A_ACAO">Id do representante que está executando a adição de carteira</param>
        /// <param name="CLI_ID">Id do cliente, que será adicionado a uma nova região, onde este histórico será anexado.</param>
        public void RegistrarHistoricoAcessosDoClienteConcedidos(HistoricoConcessaoAcessoDTO histDTO)
        {
            DateTime? dataDeHj = DateTime.Now;
            string consultas = "";

            if (histDTO.QtdConsulta > 0)
            {
                consultas = "Quantidade de consultas: " + histDTO.QtdConsulta;
            }
            
            string descricao = @"O {0} concedeu acessos ao cliente de acordo com o produto {1}. {2}'";
            string mensagemFinal = string.Format(descricao, histDTO.SemanticaAcao, histDTO.CodProduto, consultas);
            
            //Registrando o histórico no atendimento / pedido
            this.histAtend.RegistrarHistorico(dataDeHj, mensagemFinal, histDTO.Usuario, null, histDTO.CliId, histDTO.AcaId, null, histDTO.IpeId, 6, histDTO.PpiId);
            this.histPedido.RegistrarHistorico(dataDeHj, mensagemFinal, histDTO.Usuario, null, histDTO.PstId, histDTO.IpeId, histDTO.PpiId);
        
        }

        /// <summary>
        /// Método especializado para registrar o histórico no cancelamento da assinatura
        /// </summary>
        /// <param name="usuario">Nome do usuário logado</param>
        /// <param name="REP_ID_EXECUTOU_A_ACAO">Id do representante que está executando a adição de carteira</param>
        /// <param name="CLI_ID">Id do cliente, que será adicionado a uma nova região, onde este histórico será anexado.</param>
        public void RegistrarHistoricoCancelamentoAssinatura(string usuario, int? REP_ID_EXECUTOU_A_ACAO, int? CLI_ID, string assinatura, string observacoes = null)
        {
            DateTime? dataDeHj = DateTime.Now;

            // Recuperando as informações dos representantes para registrar seus nomes no histórico
            var representanteQueExecutouAAcao = _representanteService.FindById(REP_ID_EXECUTOU_A_ACAO);

            string nomeRepresentanteQueExecutouAAcao =
                                (representanteQueExecutouAAcao != null && !string.IsNullOrWhiteSpace(representanteQueExecutouAAcao.REP_NOME))
                                    ? representanteQueExecutouAAcao.REP_NOME : "(Nome indisponível)";


            string descricao = @"O representante '{0}' cancelou a assinatura '{1}' junto com todos os seus contratos. Observações: {2}";
            string mensagemFinal = string.Format(descricao, nomeRepresentanteQueExecutouAAcao, assinatura, observacoes);

            //Registrando o histórico no atendimento / pedido
            this.histAtend.RegistrarHistorico(dataDeHj, mensagemFinal, usuario, REP_ID_EXECUTOU_A_ACAO, CLI_ID, 15, null, 6);
        }

        /// <summary>
        /// Método especializado para registrar o histórico no cancelamento do contrato
        /// </summary>
        /// <param name="usuario">Nome do usuário logado</param>
        /// <param name="REP_ID_EXECUTOU_A_ACAO">Id do representante que está executando a adição de carteira</param>
        /// <param name="CLI_ID">Id do cliente, que será adicionado a uma nova região, onde este histórico será anexado.</param>
        public void RegistrarHistoricoCancelamentoContrato(string usuario, int? REP_ID_EXECUTOU_A_ACAO, int? CLI_ID, string codContrato, string observacoes = null)
        {
            DateTime? dataDeHj = DateTime.Now;

            // Recuperando as informações dos representantes para registrar seus nomes no histórico
            var representanteQueExecutouAAcao = _representanteService.FindById(REP_ID_EXECUTOU_A_ACAO);

            string nomeRepresentanteQueExecutouAAcao =
                                (representanteQueExecutouAAcao != null && !string.IsNullOrWhiteSpace(representanteQueExecutouAAcao.REP_NOME))
                                    ? representanteQueExecutouAAcao.REP_NOME : "(Nome indisponível)";


            string descricao = @"O representante '{0}' cancelou o contrato '{1}' junto com todas as suas parcelas. Observações: {2}";
            string mensagemFinal = string.Format(descricao, nomeRepresentanteQueExecutouAAcao, codContrato, observacoes);

            //Registrando o histórico no atendimento / pedido
            this.histAtend.RegistrarHistorico(dataDeHj, mensagemFinal, usuario, REP_ID_EXECUTOU_A_ACAO, CLI_ID, 15, null, 6);
        }

        /// <summary>
        /// Método especializado para registrar o histórico no cancelamento da assinatura
        /// </summary>
        /// <param name="usuario">Nome do usuário logado</param>
        /// <param name="REP_ID_EXECUTOU_A_ACAO">Id do representante que está executando a adição de carteira</param>
        /// <param name="CLI_ID">Id do cliente, que será adicionado a uma nova região, onde este histórico será anexado.</param>
        public void RegistrarHistoricoCancelamentoDoPedidoEAssinatura(string usuario, int? REP_ID_EXECUTOU_A_ACAO, int? CLI_ID, int? IPE_ID, string assinatura, string observacoes = null)
        {
            DateTime? dataDeHj = DateTime.Now;

            // Recuperando as informações dos representantes para registrar seus nomes no histórico
            var representanteQueExecutouAAcao = _representanteService.FindById(REP_ID_EXECUTOU_A_ACAO);

            string nomeRepresentanteQueExecutouAAcao =
                                (representanteQueExecutouAAcao != null && !string.IsNullOrWhiteSpace(representanteQueExecutouAAcao.REP_NOME))
                                    ? representanteQueExecutouAAcao.REP_NOME : "(Nome indisponível)";


            string descricao = @"O representante '{0}' cancelou o item de pedido {1} e a assinatura '{2}' junto com todos os seus contratos. Observações: {3}";

            string mensagemFinal = string.Format(descricao, nomeRepresentanteQueExecutouAAcao, IPE_ID, assinatura, observacoes);

            //Registrando o histórico no atendimento / pedido
            this.histAtend.RegistrarHistorico(dataDeHj, mensagemFinal, usuario, REP_ID_EXECUTOU_A_ACAO, CLI_ID, 15, null, IPE_ID);
            this.histPedido.RegistrarHistorico(dataDeHj, mensagemFinal, usuario, REP_ID_EXECUTOU_A_ACAO, 5, IPE_ID);
        }

        /// <summary>
        /// Método especializado para registrar o histórico na transferencia da assinatura
        /// </summary>
        /// <param name="usuario">Nome do usuário logado</param>
        /// <param name="REP_ID_EXECUTOU_A_ACAO">Id do representante que está executando a adição de carteira</param>
        /// <param name="CLI_ID">Id do cliente, que será adicionado a uma nova região, onde este histórico será anexado.</param>
        public void RegistrarHistoricoTransferenciaAssinaturaDaProposta(HistoricoTransAssinaturaDTO histDTO)
        {
            if (histDTO == null)
                throw new Exception("Não é possível registrar histórico. O DTO está nullo.");

            if (histDTO.CLI_ID == null)
                throw new Exception("Não é possível registrar histórico. O código do cliente não foi informado.");

            DateTime? dataDeHj = DateTime.Now;

            // Recuperando as informações dos representantes para registrar seus nomes no histórico
            var representanteQueExecutouAAcao = _representanteService.FindById(histDTO.REP_ID_EXECUTOU_A_ACAO);

            string nomeRepresentanteQueExecutouAAcao =
                                (representanteQueExecutouAAcao != null && !string.IsNullOrWhiteSpace(representanteQueExecutouAAcao.REP_NOME))
                                    ? representanteQueExecutouAAcao.REP_NOME : "(Nome indisponível)";


            string descricao = 
                @"O Item de proposta {0} do tipo Migração (Transferência) foi marcada como paga. " +
                "Assim, essa proposta migrou a assinatura de '{1}' para a assinatura {2}." + 
                "Para maiores detalhes visualize o histórico de tranferencia da assinatura. Motivos: {3}";
            string mensagemFinal = string.Format(descricao, histDTO.ppiId, histDTO.assinaturaAnterior, histDTO.assinaturaNova, histDTO.observacoes);

            //Registrando o histórico no atendimento / pedido
            this.histAtend.RegistrarHistorico(dataDeHj, mensagemFinal, histDTO.usuario, histDTO.REP_ID_EXECUTOU_A_ACAO, histDTO.CLI_ID, 15, histDTO.pedCrmId, histDTO.ipeId, 6, histDTO.ppiId);
            this.histPedido.RegistrarHistorico(dataDeHj, mensagemFinal, histDTO.usuario, histDTO.REP_ID_EXECUTOU_A_ACAO, histDTO.pstId, histDTO.ipeId, histDTO.ppiId);

            ServiceFactory.RetornarServico<AssinaturaTransferenciaSRV>()
                .RegistrarHistoricoTransferencia((int) histDTO.CLI_ID, histDTO.assinaturaAnterior, histDTO.assinaturaNova, histDTO.usuario, histDTO.ContratoOrigem, histDTO.ContratoDestino, histDTO.observacoes);
        }

        /// <summary>
        /// Método especializado para registrar o histórico na transferencia da assinatura
        /// </summary>
        /// <param name="usuario">Nome do usuário logado</param>
        /// <param name="REP_ID_EXECUTOU_A_ACAO">Id do representante que está executando a adição de carteira</param>
        /// <param name="CLI_ID">Id do cliente, que será adicionado a uma nova região, onde este histórico será anexado.</param>
        public void RegistrarHistoricoTransferenciaAssinatura(HistoricoTransAssinaturaDTO histDTO)
        {
            if (histDTO == null)
                throw new Exception("Não é possível registrar histórico. O DTO está nullo.");

            if (histDTO.CLI_ID == null)
                throw new Exception("Não é possível registrar histórico. O código do cliente não foi informado.");
            var obs = histDTO.observacoes;
            DateTime? dataDeHj = DateTime.Now;

            // Recuperando as informações dos representantes para registrar seus nomes no histórico
            var representanteQueExecutouAAcao = _representanteService.FindById(histDTO.REP_ID_EXECUTOU_A_ACAO);

            string nomeRepresentanteQueExecutouAAcao =
                                (representanteQueExecutouAAcao != null && !string.IsNullOrWhiteSpace(representanteQueExecutouAAcao.REP_NOME))
                                    ? representanteQueExecutouAAcao.REP_NOME : "(Nome indisponível)";


            string descricao = "O representante {0} Transferiu todas as informações de contrato e parcelas da assinatura {1} para a assinatura {2} . Periodo Bonus: {3}. Observações {4} .";
            string mensagemFinal = string.Format(descricao, nomeRepresentanteQueExecutouAAcao, histDTO.assinaturaAnterior, histDTO.assinaturaNova, histDTO.PeriodoBonus, histDTO.observacoes);

            //Registrando o histórico no atendimento / pedido
            this.histAtend.RegistrarHistorico(dataDeHj, mensagemFinal, histDTO.usuario, histDTO.REP_ID_EXECUTOU_A_ACAO, histDTO.CLI_ID, 15, histDTO.pedCrmId, histDTO.ipeId, 6, histDTO.ppiId);

            if(histDTO.ipeId != null || histDTO.ppiId != null)
            {
                this.histPedido.RegistrarHistorico(dataDeHj, mensagemFinal, histDTO.usuario, histDTO.REP_ID_EXECUTOU_A_ACAO, histDTO.pstId, histDTO.ipeId, histDTO.ppiId);
            }

            ServiceFactory.RetornarServico<AssinaturaTransferenciaSRV>()
                .RegistrarHistoricoTransferencia((int)histDTO.CLI_ID, histDTO.assinaturaAnterior, histDTO.assinaturaNova, histDTO.usuario, histDTO.ContratoOrigem, histDTO.ContratoDestino, histDTO.observacoes);
        }
        /// <summary>
        /// Método especializado para registrar o histórico quando um Nota Fiscal é extornada na ação de cancelamento.
        /// </summary>
        public void RegistrarHistoricoNfeXmlExtornada(string usuario, int? REP_ID_EXECUTOU_A_ACAO, int? pstId, int? ipeId, int? numeroNota)
        {
            DateTime? dataDeHj = DateTime.Now;

            // Recuperando as informações dos representantes para registrar seus nomes no histórico
            var representanteQueExecutouAAcao = _representanteService.FindById(REP_ID_EXECUTOU_A_ACAO);

            string nomeRepresentanteQueExecutouAAcao =
                                (representanteQueExecutouAAcao != null && !string.IsNullOrWhiteSpace(representanteQueExecutouAAcao.REP_NOME))
                                    ? representanteQueExecutouAAcao.REP_NOME : "(Nome indisponível)";


            string descricao = @"O representante '{0}' cancelou o pedido. Como concequência. A nota de número {0} foi extornada. 
            Pois essa nota era a última a ser gerada.";
            string mensagemFinal = string.Format(descricao, nomeRepresentanteQueExecutouAAcao, numeroNota);

            //Registrando o histórico no atendimento / pedido
            this.histPedido.RegistrarHistorico(dataDeHj, mensagemFinal, usuario, REP_ID_EXECUTOU_A_ACAO, pstId, ipeId);
        }

        /// <summary>
        /// Método especializado para registrar o histórico na transferencia da assinatura
        /// </summary>
        /// <param name="usuario">Nome do usuário logado</param>
        /// <param name="REP_ID_EXECUTOU_A_ACAO">Id do representante que está executando a adição de carteira</param>
        /// <param name="CLI_ID">Id do cliente, que será adicionado a uma nova região, onde este histórico será anexado.</param>
        public void RegistrarHistoricoParcelaExtornada(ExtornoParcelaHistoricoDTO histDTO)
        {
            if (histDTO == null)
                throw new Exception("Não é possível registrar histórico. O DTO está nullo.");

            if (histDTO.CliId == null)
                throw new Exception("Não é possível registrar histórico. O código do cliente não foi informado.");

            DateTime? dataDeHj = DateTime.Now;

            // Recuperando as informações dos representantes para registrar seus nomes no histórico
            var representanteQueExecutouAAcao = _representanteService.FindById(histDTO.RepId);

            string nomeRepresentanteQueExecutouAAcao =
                                (representanteQueExecutouAAcao != null && !string.IsNullOrWhiteSpace(representanteQueExecutouAAcao.REP_NOME))
                                    ? representanteQueExecutouAAcao.REP_NOME : "(Nome indisponível)";


            string descricao = "O representante {0} extornou o pagamento das seguintes parcelas: {1}.";
            string parcelasExtornadas = "";
            
            if(histDTO != null && histDTO.Items != null)
            {
                StringBuilder sb = new StringBuilder("( ");
                int index = 0;
                foreach(var item in histDTO.Items)
                {
                    sb.Append(string.Format(CultureInfo.GetCultureInfo("pt-BR"), "[Número da Parcela: {0}, Data de Pagamento : {1: dd/MM/yyyy - hh:mm:ss}, Valor Pago: R$ {2:N}] \n", item.CodParcela, item.DataPagamento, item.ValorPago));
                    if(index < (histDTO.Items.Count() - 1))
                    {
                        sb.Append(",");
                    }
                }
                sb.Append(")");
                parcelasExtornadas = sb.ToString();
            }

            string mensagemFinal = string.Format(descricao, nomeRepresentanteQueExecutouAAcao, parcelasExtornadas);

            //Registrando o histórico no atendimento / pedido
            this.histAtend.RegistrarHistorico(dataDeHj, mensagemFinal, histDTO.Usuario, histDTO.RepId, histDTO.CliId, 15, null, histDTO.IpeId, 6, histDTO.PpiId);

            if (histDTO.IpeId != null || histDTO.PpiId != null)
            {
                this.histPedido.RegistrarHistorico(dataDeHj, mensagemFinal, histDTO.Usuario, histDTO.RepId, histDTO.PstId, histDTO.IpeId, histDTO.PpiId);
            }

        }

        /// <summary>
        /// Método especializado para registrar o histórico na alteração de status de um pedido.
        /// </summary>
        /// <param name="usuario">Nome do usuário logado</param>
        /// <param name="REP_ID_EXECUTOU_A_ACAO">Id do representante que está executando a adição de carteira</param>
        /// <param name="CLI_ID">Id do cliente, que será adicionado a uma nova região, onde este histórico será anexado.</param>
        public void RegistrarHistoricoPendenciaDeLiberação(HistoricoFormatterSRV descriptor)
        {
            string usuario = descriptor.usuario;
            int? CLI_ID = descriptor.CLI_ID;
            int? REP_ID = descriptor.REP_ID;
            int? IPE_ID = descriptor.IPE_ID;
            string acao = descriptor.acao;
            int? acaId = descriptor.acaId;
            int? PST_ID = descriptor.PST_ID;
            int? PPI_ID = descriptor.PPI_ID;

            DateTime? dataDeHj = DateTime.Now;

            var descricao = descriptor.FormatMessage();
            
            //Registrando o histórico
            this.histAtend.RegistrarHistorico(dataDeHj, descricao, usuario, REP_ID, CLI_ID, acaId, null, IPE_ID, 6, PPI_ID);
            this.histPedido.RegistrarHistorico(dataDeHj, descricao, usuario, REP_ID, PST_ID, IPE_ID, PPI_ID);
        }

        /// <summary>
        /// Método especializado para registrar o histórico quando um é registrado um histórico de liberação.
        /// </summary>
        public void RegistrarHistoricoRegistroLiberacaoItem(HistoricoFormatterSRV descriptor)
        {
            DateTime? dataDeHj = DateTime.Now;
            string usuario = descriptor.usuario;
            int? CLI_ID = descriptor.CLI_ID;
            int? REP_ID = descriptor.REP_ID;
            int? IPE_ID = descriptor.IPE_ID;
            string acao = descriptor.acao;
            int? acaId = descriptor.acaId;
            int? PST_ID = descriptor.PST_ID;
            int? PPI_ID = descriptor.PPI_ID;
            int? RLI_ID = descriptor.RLI_ID;

            var representanteQueExecutouAAcao = _representanteService.FindById(descriptor.REP_ID);

            string nomeRepresentanteQueExecutouAAcao =
                                (representanteQueExecutouAAcao != null && !string.IsNullOrWhiteSpace(representanteQueExecutouAAcao.REP_NOME))
                                    ? representanteQueExecutouAAcao.REP_NOME : "(Nome indisponível)";


            var descricao = @"O representante {0} emitiu um item de proposta '{1}'. Porém ouve pendência para liberação da mesma. 
                              Para mais detalhes visualize as informações no registro de liberação de Código '{2}'";
            descricao = string.Format(descricao, nomeRepresentanteQueExecutouAAcao, PPI_ID, RLI_ID);

            //Registrando o histórico
            this.histAtend.RegistrarHistorico(dataDeHj, descricao, usuario, REP_ID, CLI_ID, acaId, null, IPE_ID, 6, PPI_ID);
            this.histPedido.RegistrarHistorico(dataDeHj, descricao, usuario, REP_ID, PST_ID, IPE_ID, PPI_ID);
        }


        /// <summary>
        /// Método especializado para registrar o histórico de Nota Fiscal Rejeitada.
        /// </summary>
        /// <param name="usuario">Nome do usuário logado</param>
        /// <param name="REP_ID_EXECUTOU_A_ACAO">Id do representante que está executando a adição de carteira</param>
        /// <param name="CLI_ID">Id do cliente, que será adicionado a uma nova região, onde este histórico será anexado.</param>
        public void RegistrarHistoricoNotaFiscalRejeitada(string usuario, int? REP_ID_EXECUTOU_A_ACAO, int? IPE_ID, int? numeroNota, int? codRetorno, string motivo)
        {
            DateTime? dataDeHj = DateTime.Now;
            

            string descricao = @"A nota Fiscal de Número {0} foi marcada como rejeitada pela SEFAZ. Código de Retorno: {1} Mensagem de Motivo {2}";

            string mensagemFinal = string.Format(descricao, numeroNota, codRetorno, motivo);
            
            this.histPedido.RegistrarHistorico(dataDeHj, mensagemFinal, usuario, REP_ID_EXECUTOU_A_ACAO, 11, IPE_ID);
        }

        /// <summary>
        /// Método especializado para registrar o histórico de Nota Fiscal Autorizada para uso.
        /// </summary>
        /// <param name="usuario">Nome do usuário logado</param>
        /// <param name="REP_ID_EXECUTOU_A_ACAO">Id do representante que está executando a adição de carteira</param>
        /// <param name="CLI_ID">Id do cliente, que será adicionado a uma nova região, onde este histórico será anexado.</param>
        public void RegistrarHistoricoNotaFiscalAutorizada(string usuario, int? REP_ID_EXECUTOU_A_ACAO, int? IPE_ID, int? numeroNota, int? codRetorno, string motivo, int? ppiID = null)
        {
            DateTime? dataDeHj = DateTime.Now;

            string descricao = @"A nota Fiscal de Número {0} foi Autorizada para uso pela SEFAZ. Código de Retorno: {1} Mensagem de Motivo {2}";

            string mensagemFinal = string.Format(descricao, numeroNota, codRetorno, motivo);

            this.histPedido.RegistrarHistorico(dataDeHj, mensagemFinal, usuario, REP_ID_EXECUTOU_A_ACAO, 12, IPE_ID, ppiID);
        }

        /// <summary>
        /// Método especializado para registrar o histórico de Nota Fiscal Autorizada para uso.
        /// </summary>
        /// <param name="usuario">Nome do usuário logado</param>
        /// <param name="REP_ID_EXECUTOU_A_ACAO">Id do representante que está executando a adição de carteira</param>
        /// <param name="CLI_ID">Id do cliente, que será adicionado a uma nova região, onde este histórico será anexado.</param>
        public void RegistrarHistoricoNotaFiscalAntecipada(string usuario, int? REP_ID_EXECUTOU_A_ACAO, int? IPE_ID, int? numeroNota)
        {
            DateTime? dataDeHj = DateTime.Now;

            string descricao = @"A nota Fiscal de Número {0} foi Emitida antecipadamente na proposta. Portanto ela foi anexada e esse pedido quando o pedido foi emitido.";

            string mensagemFinal = string.Format(descricao, numeroNota);

            this.histPedido.RegistrarHistorico(dataDeHj, mensagemFinal, usuario, REP_ID_EXECUTOU_A_ACAO, 12, IPE_ID);
        }

        /// <summary>
        /// Método especializado para registrar o histórico de Nota Fiscal Autorizada para uso.
        /// </summary>
        /// <param name="usuario">Nome do usuário logado</param>
        /// <param name="REP_ID_EXECUTOU_A_ACAO">Id do representante que está executando a adição de carteira</param>
        /// <param name="CLI_ID">Id do cliente, que será adicionado a uma nova região, onde este histórico será anexado.</param>
        public void RegistrarHistoricoNotaFiscalServicoAutorizada(string usuario, int? REP_ID_EXECUTOU_A_ACAO, int? IPE_ID, int? numeroNota, int? ppiID = null)
        {
            DateTime? dataDeHj = DateTime.Now;
            string descricao = @"A nota Fiscal de Serviço de Número {0} foi Autorizada.";
            string mensagemFinal = string.Format(descricao, numeroNota);
            this.histPedido.RegistrarHistorico(dataDeHj, mensagemFinal, usuario, REP_ID_EXECUTOU_A_ACAO, 12, IPE_ID, ppiID);
        }

    }
}
