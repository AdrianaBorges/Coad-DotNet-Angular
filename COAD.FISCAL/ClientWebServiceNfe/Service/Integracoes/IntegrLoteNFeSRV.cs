using Coad.GenericCrud.Exceptions;
using COAD.FISCAL.Model.DTO;
using COAD.FISCAL.Model.Integracoes;
using COAD.FISCAL.Model.Integracoes.Enumerados;
using COAD.FISCAL.Model.Integracoes.Interfaces;
using COAD.FISCAL.Model.Servicos.Retornos;
using COAD.FISCAL.Service.Integracoes.Interfaces;
using GenericCrud.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using COAD.FISCAL.Model.Servicos;
using COAD.FISCAL.Exceptions;
using GenericCrud.Util;
using System.IO;
using COAD.FISCAL.XmlUtils;
using System.Xml;
using COAD.FISCAL.Model;
using COAD.FISCAL.Model.Batch;
using COAD.FISCAL.Model.Enumerados;
using COAD.FISCAL.Model.Servicos.Enumerados;
using GenericCrud.Service;
using COAD.SEGURANCA.Service.Custons;
using COAD.SEGURANCA.Model.Dto.Custons.Batch;
using COAD.SEGURANCA.Service.Custons.Context;
using COAD.SEGURANCA.Model.Dto.Custons;
using COAD.SEGURANCA.Service;
using COAD.SEGURANCA.Model.Custons;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.FISCAL.Model.NFSe.Retornos;
using COAD.FISCAL.Model.NFSe.Enumerados;
using COAD.FISCAL.Model.NFSe;

namespace COAD.FISCAL.Service.Integracoes
{
    public abstract class IntegrLoteNFeSRV<TNFeLote, TNFeLoteItem> : ILoteNFeSRV
        where TNFeLote : INFeLote 
        where TNFeLoteItem : INFeLoteItem
    {
        public IntegrLoteNFeSRV()
        {
        }

        public abstract PedidoDTO RetornarPedido(int? CodPedido, string CodContrato = null, int? NfConfigID = null);
        public IntegrLoteItemNFeSRV<TNFeLoteItem> IntegrLoteItemNFeSRV { get; set; }
        public ILoteItemNFeSRV LoteItemNFeSRV
        {
            get => IntegrLoteItemNFeSRV;
            set => IntegrLoteItemNFeSRV = (IntegrLoteItemNFeSRV<TNFeLoteItem>)value;
        }
        public string DefaultPath { get ; set ; }

        public abstract INFeLote SalvarOuAtualizarLote(INFeLote Lote);
        public abstract INFeLote RetornarLote(int? LoteID);
        public abstract INFeLote RetornarLoteVigente(int? EmpresaID);
        public abstract INFeLote RetornarProximoLoteParaEnviar();
        public abstract INFeLote RetornarProximoLoteParaProcessar();
        public abstract IEmpresa RetornarEmpresa(int? EmpresaID);
        
        public abstract INotaFiscal RetornarNotaFiscal(int? notaFiscalID);
        public abstract ICollection<INotaFiscal> SalvarNotas(ICollection<INotaFiscal> lstNotaFiscal);
        public abstract void EnviarEmailEventoNota(ProcessamentoEventoItem procEvent);
        public abstract void EnviarEmailNotaServicoCancelada(ProcessamentoEventoItem procEvent);
        public abstract INFeLote RetornarLotePorCodRecibo(string CodRecibo);
        public abstract int? RetornarSequencialEmpresa(int? EmpresaID);
        public abstract ClienteDTO RetornarClientePorNota(int? codNota);
        public abstract ICollection<INotaFiscalReferenciada> ListarNotasReferenciadas(int? loteItm);
        public abstract PedidoDTO RetornarProposta(int? CodProposta);
        public abstract void EnviarEmailCancelamentoDeServico(ICollection<INotaFiscal> lstNotas);

        public abstract INotaFiscalEventoDTO CriarInstanciaNotaFiscalEvento();
        public abstract INotaFiscalEventoDTO SalvarEvento(INotaFiscalEventoDTO lstNotaFiscalEvento);
        public abstract ICollection<INotaFiscalItemMSG> ConverterNotaFiscalItemMSG(ListaMensagemRetornoNfse listaMensagemRetornoNfse);
        public abstract void NFseRejeitadaCallBack(NFeRejeitadaContext context);

        /// <summary>
        /// Método que será chamado quando um NFe for processada como autorizada
        /// </summary>
        public abstract void NotaFiscalAutorizadaCallBack(NFeAutorizadaContext context);
        public abstract void NFseAutorizadaCallBack(NFseAutorizadaContext context);
        /// <summary>
        /// Método que será chamado quando um NFe for processada como rejeitada
        /// </summary>
        /// <param name="context"></param>
        public abstract void NotaFiscalRejeitadaCallBack(NFeRejeitadaContext context);

        public abstract ICollection<INFeLote> ListarLotesPendentesDeEnvio();
        public abstract ICollection<INFeLote> ListarLotesNFsePendentesDeEnvio();
        public abstract ICollection<INFeLote> ListarLotesPendentesDeProcessamento();
        public abstract ICollection<INFeLote> ListarLotesNFsePendentesDeProcessamento();
        public abstract void NotaFiscalDevolvidaCallBack(NFeDevolucaoContext context);

        public abstract IList<ParcelaDTO> RetornarParcelasPorPedido(int? IdPedido, int? IdItemPedido);
        public abstract IList<ParcelaDTO> RetornarParcelasPorProposta(int? CodProposta);

        public void PreecherLoteItem(INFeLote lote)
        {
            if (lote != null)
            {
                ICollection<INFeLoteItem> result = IntegrLoteItemNFeSRV.RetornarItensDoLote(lote.LoteID);
                lote.Itens = result;

                if(lote.Itens != null)
                {
                    foreach(var lo in lote.Itens)
                    {
                        lo.NotaFiscalReferenciados = ListarNotasReferenciadas(lo.ItemLoteID);
                    }
                }
            }
        }
        public INFeLote RetornarLoteCompleto(int? LoteID)
        {
            var lote = RetornarLote(LoteID);
            PreecherLoteItem(lote);
            return lote;

        }

        public INFeLote RetornarLotePorCodReciboCompleto(string CodRecibo)
        {
            var lote = RetornarLotePorCodRecibo(CodRecibo);
            if (lote != null)
                lote.Itens = IntegrLoteItemNFeSRV.RetornarItensDoLote(lote.LoteID);
            return lote;

        }
        public INFeLote CriarNovoLote(RequisicaoNovoLote requisicaoCriacao)
        {
            try
            {
                if (requisicaoCriacao == null)
                {
                    throw new ArgumentNullException("O objeto de requisição não pode ser nulo.");
                }

                var validacao = ValidatorProxy.RecursiveValidate(requisicaoCriacao);
                if (!validacao.IsValid)
                {
                    throw new ValidacaoException("Ocorreu algum erro de validação na requisição de criação", validacao);
                }

                using (var scope = new  TransactionScope())
                {
                    var lote = Activator.CreateInstance<TNFeLote>();
                    if(lote != null)
                    {
                        lote.EmpresaID = requisicaoCriacao.EmpresaID;
                        lote.DataCadastro = DateTime.Now;
                        lote.Status = StatusLoteEnum.ENVIO_PENDENTE;
                        lote.EnvioImediato = true;
                        lote.TipoLote = requisicaoCriacao.TipoLote;
                        var loteSalvo = SalvarOuAtualizarLote(lote);
                        lote.LoteID = loteSalvo.LoteID;
                        lote.Itens = IntegrLoteItemNFeSRV.CriarLoteItens(requisicaoCriacao, lote.LoteID);

                        if(lote.TipoLote == TipoLoteEnum.ENVIO_LOTE_NFE || 
                            lote.TipoLote == TipoLoteEnum.ENVIO_LOTE_RPS_NFSE ||
                            lote.TipoLote == TipoLoteEnum.ENVIO_NFE_AVULSA || 
                            lote.TipoLote == TipoLoteEnum.ENVIO_NFSE_AVULSA)
                        {
                            RegistrarNotificacaoDeJob(lote);
                        }

                        scope.Complete();
                        return lote;
                    }
                }
                return null;

            }
            catch(Exception e)
            {
                throw new Exception("Não é possível criar um novo Lote.", e);
            }
        }

        private INFeLote CriarOuRetornarLoteVigente(int? EmpresaID, TipoLoteEnum tipoLote = TipoLoteEnum.ENVIO_LOTE_NFE)
        {
            var lote = RetornarLoteVigente(EmpresaID);
            PreecherLoteItem(lote); 
            if(lote == null || (lote != null && lote.Itens.Count >= 50))
            {
                lote = Activator.CreateInstance<TNFeLote>();
                lote.DataCadastro = DateTime.Now;
                lote.EmpresaID = EmpresaID;
                lote.Status = StatusLoteEnum.ENVIO_PENDENTE;
                lote.TipoLote = tipoLote;

                var novoLote = SalvarOuAtualizarLote(lote);
                lote.LoteID = novoLote.LoteID;
                RegistrarNotificacaoDeJob(lote);
            }
            return lote;
        }

        public INFeLote AdicionarPedidoLoteVigente(int? codPedido, int? EmpresaID, string CodContrato = null)
        {
            try
            {
                if (codPedido == null)
                {
                    throw new ArgumentNullException("O codPedido de requisição não pode ser nulo.");
                }

                using (var scope = new TransactionScope())
                {
                    var lote = CriarOuRetornarLoteVigente(EmpresaID);
                    if (lote != null)
                    {
                        
                        var loteItem = IntegrLoteItemNFeSRV.CriarLoteItem(codPedido, lote.LoteID, CodContrato);
                        lote.Itens.Add(loteItem);
                        scope.Complete();

                        return lote;
                    }
                }
                return null;

            }
            catch (Exception e)
            {
                throw new Exception("Não é possível criar um novo Lote.", e);
            }
        }

        public void GravarRecibo(string CodRecibo, int? LoteID)
        {
            if (string.IsNullOrWhiteSpace(CodRecibo))
                throw new Exception("Informe o Código do Recibo");
            if(LoteID == null)
            {
                throw new Exception("Informe o Código do Lote");
            }

            var lote = RetornarLote(LoteID);
            lote.CodRecibo = CodRecibo;

            SalvarOuAtualizarLote(lote);
        }

        public void ProcessarRetornoLote(ConsultaLoteRetorno consultaRetorno, INFeLote lote)
        {
            try
            {
                if(consultaRetorno != null)
                {
                    if(consultaRetorno.cStat == 104)
                    {
                        if (lote != null)
                        {
                            lote.CodRetornoProcessamento = consultaRetorno.cStat;
                            lote.MensagemRetornoProcessamento = consultaRetorno.xMotivo;

                            foreach(var retorno in consultaRetorno.protNFe)
                            {
                                var item = lote.Itens.Where(x => x.ChaveNota == retorno.infProt.chNFe).FirstOrDefault();

                                if(item != null)
                                {
                                    ProcessarRetornoNota(retorno, item, lote);
                                }
                            }
                            IntegrLoteItemNFeSRV.SalvarOuAtualizarLoteItens(lote.Itens);

                            lote.Status = StatusLoteEnum.PROCESSADA_COM_EXITO;
                        }
                    }
                    else
                    if (consultaRetorno.cStat == 106)
                    {
                        lote.Status = StatusLoteEnum.LOTE_EM_PROCESSAMENTO;
                    }
                    else
                    {
                        lote.Status = StatusLoteEnum.ERRO_AO_PROCESSAR;
                    }
                    SalvarOuAtualizarLote(lote);
                }

            }
            catch (Exception e)
                {
                lote.Status = StatusLoteEnum.ERRO_AO_PROCESSAR;
                SalvarOuAtualizarLote(lote);
                throw new Exception("Não é possível processar o retorno do lote", e);
            }
            
        }

        public void ProcessarRetornoLoteNFse(ConsultarSituacaoLoteRpsResposta situacaoLote, INFeLote lote)
        {
            try
            {
                if (situacaoLote != null)
                {
                    if (situacaoLote.Situacao == SituacaoLoteRpsEnum.PROCESSADO_COM_SUCESSO)
                    {
                        if (lote != null && lote.Itens != null)
                        {
                            foreach (var ltItm in lote.Itens)
                            {
                                ProcessarRetornoNotaNFse(ltItm, lote);
                            }
                             
                            IntegrLoteItemNFeSRV.SalvarOuAtualizarLoteItens(lote.Itens);
                            lote.Status = StatusLoteEnum.PROCESSADA_COM_EXITO;
                        }
                    }
                    //else
                    //if (situacaoLote.Situacao == SituacaoLoteRpsEnum.NAO_PROCESSADO)
                    //{
                    //    lote.Status = StatusLoteEnum.LOTE_EM_PROCESSAMENTO;
                    //}
                    else
                    {
                        lote.Status = StatusLoteEnum.ERRO_AO_PROCESSAR;
                        var consultaLote = ConsultarLoteRps(lote);

                        if (consultaLote != null)
                            lote.MensagemRetorno = FormatarMensagemRetorno(consultaLote.ListaMensagemRetorno);

                    }
                    SalvarOuAtualizarLote(lote);
                }

            }
            catch (Exception e)
            {
                lote.Status = StatusLoteEnum.ERRO_AO_PROCESSAR;
                SalvarOuAtualizarLote(lote);
                throw new Exception("Não é possível processar o retorno do lote", e);
            }

        }
                

        private void ProcessarRetornoNota(ProtocoloRecebimento protocolo, INFeLoteItem item, INFeLote lote)
        {
            item.CodRetorno = protocolo.infProt.cStat;
            item.MensagemRetorno = protocolo.infProt.xMotivo;
            item.NumeroProtocolo = protocolo.infProt.nProt;
            item.DataAutorizacaoRejeicao = protocolo.infProt.dhRecbto;

            if(item.CodRetorno == 100) // Autorizado
            {
                if (string.IsNullOrWhiteSpace(DefaultPath))
                {
                    DefaultPath = @"C:\";
                }

                item.Status = StatusLoteItemEnum.AUTORIZADA;

                var binarioArquivo = item.BinarioNFeXml;
                if(binarioArquivo != null)
                {
                    var pathNfe = SysUtils.RetornarPathNFeXML();
                    var pathArquivo = DefaultPath + pathNfe + @"\nfe-processadas";
                    if (!Directory.Exists(pathArquivo))
                    {
                        Directory.CreateDirectory(pathArquivo);
                    }

                    var fileSavePath = pathArquivo + item.PathArquivoNFeXml;

                    File.WriteAllBytes(fileSavePath, binarioArquivo);
                    var xmlContent = File.ReadAllText(fileSavePath);

                    var proNFe = RetornarNFeDistribuicao(xmlContent);

                    NFeProcessada nfeProcessada = new NFeProcessada();
                    nfeProcessada.NFe = new List<NFeDistribuicaoDTO>() { proNFe };
                    nfeProcessada.protNFe = protocolo;
                    nfeProcessada.versao = ConstantsNFe.VERSAO;

                    var newBytes = XmlUtil.SerializeAsXmlBinary(nfeProcessada, fileSavePath);
                    item.BinarioNFeXml = newBytes;
                    item.PathArquivoNFeXml = fileSavePath;
                }
            }
            else 
            {
                item.Status = StatusLoteItemEnum.REJEITADA;
            }
        }

        private void ProcessarRetornoNotaNFse(INFeLoteItem item, INFeLote lote)
        {
            if (item != null &&
                lote != null)
            {
                var consulta = ConsultarNFsePorRps(item);
                var consultaLote = ConsultarLoteRps(lote);

                if (consulta != null)
                {
                    if (consulta.CompNfse != null &&
                        (consulta.ListaMensagemRetorno == null || consulta.ListaMensagemRetorno.MensagemRetorno.Count() <= 0))
                    {
                        item.NumeroNota = consulta.CompNfse.Nfse.InfNfse.Numero;
                        var compNfse = consulta.CompNfse;
                        var cnpjPrestador = compNfse.Nfse.InfNfse.PrestadorServico.IdentificacaoPrestador.Cnpj;
                        var numero = compNfse.Nfse.InfNfse.Numero;

                        if (string.IsNullOrWhiteSpace(DefaultPath))
                        {
                            DefaultPath = @"C:\";
                        }

                        item.Status = StatusLoteItemEnum.AUTORIZADA;

                        var pathNfe = SysUtils.RetornarPathNFeXML();
                        var pathArquivo = DefaultPath + pathNfe + @"\nfe-processadas";
                        if (!Directory.Exists(pathArquivo))
                        {
                            Directory.CreateDirectory(pathArquivo);
                        }

                        var fileName = $"NFse_{cnpjPrestador}_{numero}.xml";
                        var fileSavePath = pathArquivo + fileName;

                        var newBytes = XmlUtil.SerializeAsXmlBinary(compNfse, fileSavePath);
                        item.BinarioNFeXml = newBytes;
                        item.PathArquivoNFeXml = fileSavePath;
                    }
                    else
                    {
                        item.Status = StatusLoteItemEnum.REJEITADA;
                        if(consulta.ListaMensagemRetorno != null &&
                            consulta.ListaMensagemRetorno.MensagemRetorno != null)
                        {
                            var msgs = item.NotaFiscalLoteItemMSG;
                            if (msgs == null)
                                msgs = new List<INotaFiscalItemMSG>();

                            if(consultaLote != null)
                            {
                                var mensagens = ConverterNotaFiscalItemMSG(consultaLote.ListaMensagemRetorno);

                                if (mensagens != null)
                                    msgs = msgs.Concat(mensagens).ToList();

                                item.NotaFiscalLoteItemMSG = msgs;
                            }
                        }
                    }

                }
            }
        }


        public INFeLote RetornarProximoLoteParaEnvioCompleto()
        {
            var lote = RetornarProximoLoteParaEnviar();
            PreecherLoteItem(lote);
            return lote;
        }

        public INFeLote RetornarProximoLoteParaProcessarCompleto()
        {
            var lote = RetornarProximoLoteParaProcessar();
            if(lote != null)
            {
                if(lote.Status != StatusLoteEnum.LOTE_ENVIADO_NAO_PROCESSADO)
                {
                    throw new Exception("Esse lote não pode ser processado O status dele não é 'LOTE_ENVIADO_NAO_PROCESSADO'.");
                }

                if (string.IsNullOrWhiteSpace(lote.CodRecibo))
                {
                    throw new Exception("O Lote marcado para ser processado não possui código de recibo. Essa informação é necessário para o processo.");
                }
                PreecherLoteItem(lote);
                return lote;
            }
            return null;
        }

        public NFeDistribuicaoDTO RetornarNFeDistribuicao(string xmlContent)
        {
            var nfe = XmlUtil.LoadFromXMLString<NotaFiscal>(xmlContent);
            var proNFe = new NFeDistribuicaoDTO(nfe);
            return proNFe;
        }

        public void MarcarLoteParaEnvioImediato(INFeLote item)
        {
            if(item != null)
            {
                item.EnvioImediato = true;
                SalvarOuAtualizarLote(item);
            }
        }

      
        
        public IList<LoteBatchStatuDTO> RetornarStatusDoLote(ICollection<int> LstLoteID)
        {
            IList<LoteBatchStatuDTO> retorno = new List<LoteBatchStatuDTO>();

            foreach (var id in LstLoteID)
            {
                var lote = RetornarLote(id);
                string mensagem = "Lote não encontrado";
                int? codStatus = null;

                if (lote != null)
                {
                    switch (lote.Status)
                    {

                        case StatusLoteEnum.ENVIO_PENDENTE:
                            {
                                mensagem = "Aguardando o Serviço em Segunda Plano Enviar as NFe.";
                                codStatus = (int)lote.Status;
                                break;
                            }
                        case StatusLoteEnum.ERRO_AO_PROCESSAR:
                            {
                                mensagem = "Ocorreu um erro ao enviar/processar as NFes. Favor verificar as mensagens de retorno.";
                                codStatus = (int)lote.Status;
                                break;
                            }
                        case StatusLoteEnum.LOTE_ENVIADO_NAO_PROCESSADO:
                            {
                                mensagem = "Lote Enviado com Sucesso... Aguardando o Serviço em Segunda Plano Processar o  Retorno da NFe.";
                                codStatus = (int)lote.Status;
                                break;
                            }
                        case StatusLoteEnum.LOTE_EM_PROCESSAMENTO:
                            {
                                mensagem = "O Sefaz retornou que o lote ainda não foi processado. Aguardando uma nova consulta...";
                                codStatus = (int)lote.Status;
                                break;
                            }
                        case StatusLoteEnum.PROCESSADA_COM_EXITO:
                            {
                                mensagem = "As NFes foram processadas com êxito.";
                                codStatus = (int)lote.Status;
                                break;
                            }

                    }
                }
                retorno.Add(new LoteBatchStatuDTO()
                {
                    CodEmpresa = lote.EmpresaID,
                    BatchStatus = mensagem,
                    CodStatus = codStatus
                });
            }
            return retorno;
        }
        
        public ProcessamentoEventoRetorno EnviarEvento(INFeLote lote)
        {

            //Por algum motivo, embora o webservice de recepção de evento esteja na versão 4.0, só está aceitando a versão 1.00 de evento e dados.
            ProcessamentoEventoRetorno processamentoRetornoEvento = new ProcessamentoEventoRetorno();

            if (lote != null && lote.EmpresaID != null)
            {
                var empresa = RetornarEmpresa(lote.EmpresaID);

                var requisicao = new RequisicaoRecepcaoEvento()
                {
                    idLote = lote.LoteID,
                    versao = "1.00"
                };
                
                int seq = 1;
                foreach (var loteItem in lote.Itens)
                {
                    TipoEventoEnum tipoEvento = TipoEventoEnum.CANCELAMENTO;
                    DetalhamentoEvento detEvento = null;

                    if (loteItem.Tipo == TipoLoteItemEnum.CANCELAMENTO)
                    {
                        tipoEvento = TipoEventoEnum.CANCELAMENTO;

                        detEvento = new DetalhamentoEvento()
                        {
                            versao = "1.00",
                            descEvento = "Cancelamento",
                            Justificativa = "O pedido foi cancelado.",
                            NumeroProtocolo = loteItem.NumeroProtocolo
                        };
                    }

                    if (loteItem.Tipo == TipoLoteItemEnum.CARTA_CORRECAO)
                    {
                        tipoEvento = TipoEventoEnum.CARTA_CORRECAO;

                        detEvento = new DetalhamentoEvento()
                        {
                            versao = "1.00",
                            descEvento = "Carta de Correção",
                            DescricaoCorrecao = loteItem.CartaCorrecao,
                            CondicaoDeUso = ConstantsNFe.CONDICAO_USO_CARTA_CORRECAO
                        };
                    }

                    var id = string.Format("ID{0}{1}{2}", (int)tipoEvento, loteItem.ChaveNota, StringUtil.PreencherZeroEsquerda(seq, 2));
                    requisicao.evento.Add(new Evento()
                    {
                        versao = "1.00",
                        infEvento = new InformacaoEvento()
                        {
                            Id = id,
                            chNFe = loteItem.ChaveNota,
                            detEvento = detEvento,
                            tpAmb = (SysUtils.InHomologation()) ? TipoAmbienteEnum.Homologacao : TipoAmbienteEnum.Producao,
                            nSeqEvento = seq,
                            DataEvento = DateTime.Now,
                            TipoEvento = tipoEvento,
                            cOrgao = "33",
                            verEvento = "1.00",
                            CNPJ = empresa.CNPJ,
                        }
                    });
                    seq++;
                }

                var clienteEnviEvento = ServiceFactory.RetornarServico<ClientWsNfeRecepcaoSRV>();
                var certificado = CertificateUtil.RetornarCertificado(lote.EmpresaID);
                var resposta = clienteEnviEvento.EnviarEvento(requisicao, certificado);

                if (resposta != null)
                {
                    if (resposta.cStat == 128)
                    {
                        if (resposta.retEvento != null)
                        {
                            foreach (var evRet in resposta.retEvento)
                            {
                                var loteItem = lote.Itens.Where(x => x.ChaveNota == evRet.infEvento.chNFe).FirstOrDefault();
                                var evento = requisicao.evento.Where(x => x.infEvento.chNFe == evRet.infEvento.chNFe).FirstOrDefault();

                                if (loteItem != null)
                                {
                                    loteItem.CodRetorno = evRet.infEvento.cStat;
                                    loteItem.MensagemRetorno = evRet.infEvento.xMotivo;

                                    var notaFiscal = RetornarNotaFiscal(loteItem.CodNotaFiscal);
                                    if (notaFiscal != null)
                                    {
                                        if (loteItem.CodRetorno == 135)
                                        {
                                            notaFiscal.EventoAnexado = true;

                                            if(loteItem.Tipo == TipoLoteItemEnum.CANCELAMENTO)
                                                notaFiscal.Status = StatusNotaFiscalEnum.CANCELADA;

                                            processamentoRetornoEvento.BatchContext.AdicionarContagemSucesso();
                                            var eventoSalvo = SalvarNotaFiscalEvento(notaFiscal, evRet, evento);

                                            processamentoRetornoEvento.ProcessamentoEventoItens.Add(new ProcessamentoEventoItem()
                                            {
                                                NotaFiscal = notaFiscal,
                                                LoteItem = loteItem,
                                                NomeArquivo = (eventoSalvo != null) ? eventoSalvo.ArquivoNome: null,
                                                Arquivo = (eventoSalvo != null) ? eventoSalvo.Arquivo: null
                                            });
                                        }
                                        else
                                        {
                                            processamentoRetornoEvento.BatchContext.AdicionarContagemFalha();
                                            processamentoRetornoEvento.BatchContext.ListErros.Add(new ErroReportItemDTO()
                                            {

                                                Mensagem = string.Format("O evento não pode ser anexado a nota de Código {0} e chave {1}. Mensagem de retorno: Código: {2}. Motivo: {3}", notaFiscal.CodNotaFiscal, notaFiscal.ChaveNota, loteItem.CodRetorno, loteItem.MensagemRetorno)
                                            });
                                        }
                                    }
                                }


                            }
                        }
                        LoteItemNFeSRV.SalvarOuAtualizarLoteItens(lote.Itens);
                    }
                    else
                    {
                        throw new RetornoProcessamentoException(resposta.cStat, resposta.xMotivo);
                    }
                }

                if (processamentoRetornoEvento.ProcessamentoEventoItens != null && processamentoRetornoEvento.ProcessamentoEventoItens.Count > 0)
                {
                    var lstNotas = processamentoRetornoEvento.ProcessamentoEventoItens.Select(x => x.NotaFiscal).ToList();
                    SalvarNotas(lstNotas);

                    foreach (var procEvent in processamentoRetornoEvento.ProcessamentoEventoItens)
                    {
                        EnviarEmailEventoNota(procEvent);
                    }
                }               

            }
            return processamentoRetornoEvento;
        }

        public void SalvarArquivoDeEvento(Evento evento, EventoRetorno eventoRetorno, INotaFiscal nota, INotaFiscalEventoDTO notaFiscalEvento, string fileNameProc)
        { 
            if(evento != null && 
                eventoRetorno != null &&
                nota != null &&
                notaFiscalEvento != null)
            {
                var procEvento = new ProcessamentoEvento()
                {
                    Versao = "1.00"
                };

                procEvento.Eventos.Add(evento);
                procEvento.EventoRetornos.Add(eventoRetorno);

                var fileName = string.Format("{0}-{1}.xml", nota.ChaveNota, fileNameProc);                

                var bytes = XmlUtil.SerializeAsXmlBinary(procEvento, true, fileName, false);
                nota.NomeArquivoEvento = fileName;
                nota.ArquivoEvento = bytes;

                notaFiscalEvento.ArquivoNome = fileName;
                notaFiscalEvento.Arquivo = bytes;

            }
        }

        public INotaFiscalEventoDTO SalvarNotaFiscalEvento(INotaFiscal notaFiscal, EventoRetorno retornoEvento, Evento envReq)
        {
            if(retornoEvento != null)
            {

                if(retornoEvento != null &&
                    envReq != null)
                {
                    if(envReq != null)
                    {
                        var nomeArquivoEvento = "procEventoNFe";
                        var evento = CriarInstanciaNotaFiscalEvento();
                        evento.CNPJ = envReq.infEvento.CNPJ;
                        evento.CodRetorno = retornoEvento.infEvento.cStat;
                        evento.DescRetorno = retornoEvento.infEvento.xMotivo;
                        evento.Data = DateTime.Now;
                        evento.EventoXMLID = envReq.infEvento.Id;
                        evento.NotaFiscalID = notaFiscal.CodNotaFiscal;

                        if (retornoEvento.infEvento.tpEvento == ((int)TipoEventoEnum.CARTA_CORRECAO).ToString())
                        {
                            nomeArquivoEvento = "procEventoCCeNFe";
                            var det = envReq.infEvento.detEvento;
                            evento.DescCartaCorrecao = det.DescricaoCorrecao;
                            evento.CondicaoUso = det.CondicaoDeUso;
                            evento.Tipo = TipoLoteItemEnum.CARTA_CORRECAO;
                        }

                        if (retornoEvento.infEvento.tpEvento == ((int)TipoEventoEnum.CANCELAMENTO).ToString())
                        {
                            nomeArquivoEvento = "procEventoCancNFe";
                            var det = envReq.infEvento.detEvento;
                            evento.DescJustificativa = det.Justificativa;
                            evento.NumeroProtocolo = det.NumeroProtocolo;
                            evento.Tipo = TipoLoteItemEnum.CANCELAMENTO;
                        }
                            
                        SalvarArquivoDeEvento(envReq, retornoEvento, notaFiscal, evento, nomeArquivoEvento);
                        SalvarEvento(evento);
                        return evento;
                    }
                }
                
            }
            return null;
        }

        private void RegistrarNotificacaoDeJob(INFeLote lote)
        {
            string usuario = "COADSYS";
            int? repID = 1;

            if (SessionContext.PossuiSessao())
            {
                usuario = SessionContext.login;
                repID = SessionContext.rep_id;
            }

            string mensagemEnvio = null;
            string mensagemRetorno = null;
            int? jobIdEnvio = null;
            int? jobIdRetorno = null;

            if(lote.TipoLote == TipoLoteEnum.ENVIO_LOTE_NFE)
            {
                mensagemEnvio = string.Format("Envio de Lote de NFe com {0} Notas", lote.Itens.Count);
                mensagemRetorno = "Processamento de retorno de Lote de NFe.";
                jobIdEnvio = 10;
                jobIdRetorno = 11;
            }
            else if(lote.TipoLote == TipoLoteEnum.ENVIO_LOTE_RPS_NFSE)
            {
                mensagemEnvio = string.Format("Envio de Lote de NFse com {0} Notas", lote.Itens.Count);
                mensagemRetorno = "Processamento de retorno de Lote de NFse.";
                jobIdEnvio = 10;
                jobIdRetorno = 13;
            }

            var jobNotify = ServiceFactory.RetornarServico<JobNotificacaoSRV>();
            jobNotify.RegistrarJobNotificacao(new JobNotificacaoRequestDTO()
            {
                codRef = lote.LoteID,
                usuario = usuario,
                repId = repID,
                jobID = jobIdEnvio,
                descricao = mensagemEnvio
            });

            jobNotify.RegistrarJobNotificacao(new JobNotificacaoRequestDTO()
            {

                codRef = lote.LoteID,
                usuario = usuario,
                repId = repID,
                jobID = jobIdRetorno,
                descricao = mensagemRetorno
            });
        }
        
        public ConsultarLoteRpsResposta ConsultarLoteRps(INFeLote lote)
        {
            var prestador = RetornarDadosPrestador(lote.EmpresaID);

            ConsultarLoteRpsEnvio consultaLote = new ConsultarLoteRpsEnvio()
            {
                Prestador = prestador,
                Protocolo = lote.CodRecibo
            };

            var validacao = ValidatorProxy.RecursiveValidate(consultaLote);

            if (!validacao.IsValid)
            {
                throw new ValidacaoException("Ocorre consultar o resultado do processamento do lote.", validacao);
            }

            var certificado = CertificateUtil.RetornarCertificado(lote.EmpresaID);
            var srvNFse = ServiceFactory.RetornarServico<ClienteNfseSRV>();

            var situacaoRps = srvNFse.ConsultarLoteRps(consultaLote, certificado);
            return situacaoRps;
        }

        public ConsultarNfseRpsResposta ConsultarNFsePorRps(INFeLoteItem loteItem)
        {
            var dadosPrestador = RetornarDadosPrestador(loteItem.Lote.EmpresaID);
            var identificacaoRps = RetornarIndentificacaoRps(loteItem);

            ConsultarNfseRpsEnvio consultaNfsePorRps = new ConsultarNfseRpsEnvio()
            {
                IdentificacaoRps = identificacaoRps,
                Prestador = dadosPrestador
            };

            var validacao = ValidatorProxy.RecursiveValidate(consultaNfsePorRps);

            if (!validacao.IsValid)
            {
                throw new ValidacaoException("Ocorre consultar o resultado do processamento do lote.", validacao);
            }

            var certificado = CertificateUtil.RetornarCertificado(loteItem.Lote.EmpresaID);
            var srvNFse = ServiceFactory.RetornarServico<ClienteNfseSRV>();

            var situacaoRps = srvNFse.ConsultarNfsePorRps(consultaNfsePorRps, certificado);
            return situacaoRps;
        }

        public IdentificacaoPrestadorRps RetornarDadosPrestador(int? EmpresaID)
        {
            var empresa = RetornarEmpresa(EmpresaID);

            if(empresa != null)
            {
                var indPrestador = new IdentificacaoPrestadorRps()
                {
                    Cnpj = empresa.CNPJ,
                    InscricaoMunicipal = empresa.IM
                };
                return indPrestador;
            }
            return null;
        }

        public IdentificacaoRps RetornarIndentificacaoRps(INFeLoteItem loteItem)
        {
            if(loteItem != null &&
                loteItem.NumeroRps != null &&
                !string.IsNullOrWhiteSpace(loteItem.Serie))
            {
                var identRps = new IdentificacaoRps()
                {
                    Numero = loteItem.NumeroRps,
                    Serie = loteItem.Serie,
                    Tipo = TipoRPSEnum.RPS
                };
                return identRps;
            }

            return null;
        }

        public string FormatarMensagemRetorno(ListaMensagemRetornoNfse ListaMensagem)
        {
            string mensagem = null;
            if (ListaMensagem != null && ListaMensagem.MensagemRetorno != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var msg in ListaMensagem.MensagemRetorno)
                {
                    sb.Append($"Código de Retorno: {msg.Codigo} Mensagem Retorno: {msg.Mensagem} Correção: {msg.Correcao} <br />");
                }
                mensagem += sb.ToString();
            }
            return mensagem;
        }

        public void ProcessarRetornoLoteMG(ConsultaLoteRetorno consultaRetorno, INFeLote lote)
        {
            try
            {
                if (consultaRetorno != null)
                {
                    if (consultaRetorno.cStat == 104)
                    {
                        if (lote != null)
                        {
                            lote.CodRetornoProcessamento = consultaRetorno.cStat;
                            lote.MensagemRetornoProcessamento = consultaRetorno.xMotivo;

                            foreach (var retorno in consultaRetorno.protNFe)
                            {
                                var item = lote.Itens.Where(x => x.ChaveNota == retorno.infProt.chNFe).FirstOrDefault();

                                if (item != null)
                                {
                                    ProcessarRetornoNota(retorno, item, lote);
                                }
                            }
                            IntegrLoteItemNFeSRV.SalvarOuAtualizarLoteItens(lote.Itens);

                            lote.Status = StatusLoteEnum.PROCESSADA_COM_EXITO;
                        }
                    }
                    else
                    if (consultaRetorno.cStat == 106)
                    {
                        lote.Status = StatusLoteEnum.LOTE_EM_PROCESSAMENTO;
                    }
                    else
                    {
                        lote.Status = StatusLoteEnum.ERRO_AO_PROCESSAR;
                    }
                    SalvarOuAtualizarLote(lote);
                }

            }
            catch (Exception e)
            {
                lote.Status = StatusLoteEnum.ERRO_AO_PROCESSAR;
                SalvarOuAtualizarLote(lote);
                throw new Exception("Não é possível processar o retorno do lote", e);
            }

        }

    }
}
