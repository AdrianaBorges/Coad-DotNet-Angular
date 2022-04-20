using COAD.FISCAL.Model.Batch;
using COAD.FISCAL.Model.DTO;
using COAD.FISCAL.Model.Integracoes;
using COAD.FISCAL.Model.Integracoes.Interfaces;
using COAD.FISCAL.Model.NFSe;
using COAD.FISCAL.Model.NFSe.Retornos;
using COAD.FISCAL.Model.Servicos;
using COAD.FISCAL.Model.Servicos.Retornos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Service.Integracoes.Interfaces
{
    public interface ILoteNFeSRV
    {
        string DefaultPath { get; set; }
        INFeLote CriarNovoLote(RequisicaoNovoLote requisicaoCriacao);
        PedidoDTO RetornarPedido(int? CodPedido, string CodContrato = null, int? NfConfigID = null);
        PedidoDTO RetornarProposta(int? CodPedido);
        INFeLote SalvarOuAtualizarLote(INFeLote Lote);
        INFeLote AdicionarPedidoLoteVigente(int? codPedido, int? EmpresaID, string CodContrato = null);
        void ProcessarRetornoLote(ConsultaLoteRetorno consultaRetorno, INFeLote lote);
        void ProcessarRetornoLoteMG(ConsultaLoteRetorno consultaRetorno, INFeLote lote);
        void ProcessarRetornoLoteNFse(ConsultarSituacaoLoteRpsResposta consultaRetorno, INFeLote lote);

        void PreecherLoteItem(INFeLote lote);
        INFeLote RetornarLote(int? LoteID);
        int? RetornarSequencialEmpresa(int? EmpresaID);
        ICollection<INotaFiscalReferenciada> ListarNotasReferenciadas(int? loteItm);

        /// <summary>
        /// Retorna o próximo lote para ser enviodo para o Sefaz
        /// </summary>
        /// <returns></returns>
        INFeLote RetornarProximoLoteParaEnvioCompleto();
        ICollection<INFeLote> ListarLotesPendentesDeEnvio();
        ICollection<INFeLote> ListarLotesNFsePendentesDeEnvio();

        /// <summary>
        /// Retorna o próximo lotep para ser processado ao retornar a resposta do Sefaz
        /// </summary>
        /// <returns></returns>
        INFeLote RetornarProximoLoteParaProcessarCompleto();
        ICollection<INFeLote> ListarLotesPendentesDeProcessamento();
        ICollection<INFeLote> ListarLotesNFsePendentesDeProcessamento();


        ILoteItemNFeSRV LoteItemNFeSRV { get; set; }
        void NotaFiscalAutorizadaCallBack(NFeAutorizadaContext context);
        void NotaFiscalDevolvidaCallBack(NFeDevolucaoContext context);
        void NotaFiscalRejeitadaCallBack(NFeRejeitadaContext context);
        void NFseAutorizadaCallBack(NFseAutorizadaContext context);
        void NFseRejeitadaCallBack(NFeRejeitadaContext context);

        IList<LoteBatchStatuDTO> RetornarStatusDoLote(ICollection<int> LoteID);
        IEmpresa RetornarEmpresa(int? EmpresaID);
        ProcessamentoEventoRetorno EnviarEvento(INFeLote lote);
        INotaFiscal RetornarNotaFiscal(int? notaFiscalID);
        ICollection<INotaFiscal> SalvarNotas(ICollection<INotaFiscal> lstNotaFiscal);
        ClienteDTO RetornarClientePorNota(int? codNota);
        IdentificacaoPrestadorRps RetornarDadosPrestador(int? EmpresaID);
        IdentificacaoRps RetornarIndentificacaoRps(INFeLoteItem loteItem);
        ICollection<INotaFiscalItemMSG> ConverterNotaFiscalItemMSG(ListaMensagemRetornoNfse listaMensagemRetornoNfse);
        void EnviarEmailCancelamentoDeServico(ICollection<INotaFiscal> lstNotas);
        IList<ParcelaDTO> RetornarParcelasPorPedido(int? IdPedido, int? IdItemPedido);
        IList<ParcelaDTO> RetornarParcelasPorProposta(int? CodProposta);

    }

}
