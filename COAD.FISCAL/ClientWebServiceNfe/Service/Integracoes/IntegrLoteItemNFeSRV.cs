using COAD.FISCAL.Model;
using COAD.FISCAL.Model.Integracoes;
using COAD.FISCAL.Model.Integracoes.Enumerados;
using COAD.FISCAL.Model.Integracoes.Interfaces;
using COAD.FISCAL.Model.NFSe;
using COAD.FISCAL.Service.Integracoes.Interfaces;
using GenericCrud.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.FISCAL.Service.Integracoes
{
    public abstract class IntegrLoteItemNFeSRV<TNFeLoteItem> : ILoteItemNFeSRV
        where TNFeLoteItem : INFeLoteItem
    {
        public abstract ICollection<INFeLoteItem> SalvarOuAtualizarLoteItens(ICollection<INFeLoteItem> Itens);
        public abstract INFeLoteItem SalvarOuAtualizarLoteItem(INFeLoteItem Item);
        public abstract INFeLoteItem RetornarLoteItem(int? ItemLoteID);
        public abstract INFeLoteItem RetornarLoteItemPorChave(string ChaveNota);
        public abstract IList<INFeLoteItem> RetornarItensDoLote(int? LoteID);
        public abstract INotaFiscalReferenciada CriarNotaNotaReferenciada();
        public abstract INFeLoteItem RetornarLoteItemDaNotaAutorizada(int? CodNotaFiscal);

        /// <summary>
        /// Retorna todos os subitens de um lote item
        /// </summary>
        /// <param name="LoteItemPaiID"></param>
        /// <returns></returns>
        public abstract ICollection<INotaFiscalReferenciada> SalvarNotaFiscalReferenciadas(ICollection<INotaFiscalReferenciada> notasFiscaisReferenciadas);

        public ICollection<INFeLoteItem> CriarLoteItens(RequisicaoNovoLote requisicaoCriacao, int? LoteID)
        {
            if(requisicaoCriacao != null && 
                requisicaoCriacao.LstRequisicoes != null && 
                LoteID != null)
            {
                var lstNfeItem = new List<INFeLoteItem>();
                
                foreach(var req in requisicaoCriacao.LstRequisicoes)
                {
                    var loteItm = Activator.CreateInstance<TNFeLoteItem>();
                    if(loteItm != null)
                    {
                        loteItm.LoteID = LoteID;
                        loteItm.Status = StatusLoteItemEnum.ENVIO_PENDENTE;

                        loteItm.CodProposta = req.CodProposta;
                        loteItm.CodPedido = req.CodPedido;
                        loteItm.CodContrato = req.CodContrato;
                        loteItm.ChaveNota = req.ChaveNotaFiscal;
                        loteItm.NumeroProtocolo = req.NumeroProtocolo;
                        loteItm.CodNotaFiscal = req.CodNotaFiscal;
                        loteItm.ClienteID = req.CodCliente;
                        loteItm.EmpresaID = req.CodEmpresa;
                        loteItm.CartaCorrecao = req.CartaCorrecao;
                        loteItm.NotaAntecipada = req.NotaAntecipada;
                        loteItm.NfConfigID = req.NfConfigID;

                        if (req.Tipo == null)
                            loteItm.Tipo = TipoLoteItemEnum.ENVIO;
                        else
                        {
                            loteItm.Tipo = req.Tipo.Value;
                        }
                        
                        if(req.CodNotaReferencia != null && req.CodNotaReferencia.Count > 0)
                        {
                            var loteSRV = ServiceFactory.RetornarServico<ILoteNFeSRV>();
                            var lstReferencial = new HashSet<INotaFiscalReferenciada>();
                            foreach(var notaID in req.CodNotaReferencia)
                            {
                                var nota = loteSRV.RetornarNotaFiscal(notaID);
                                if(nota != null)
                                {
                                    var notaRef = CriarNotaNotaReferenciada();
                                    //notaRef.CodNotaFiscal = notaID;
                                    notaRef.ChaveNota = nota.ChaveNota;
                                    lstReferencial.Add(notaRef);
                                }
                            }
                            loteItm.NotaFiscalReferenciados = lstReferencial;
                        }
                        lstNfeItem.Add(loteItm);

                    }
                }
                
                var itensSalvos = SalvarOuAtualizarLoteItens(lstNfeItem);
                ICollection<INFeLoteItem> lstItensReferencia = new List<INFeLoteItem>();

                var index = 0;
                foreach (var itens in itensSalvos)
                {
                    var currentItem = lstNfeItem[index];
                    currentItem.ItemLoteID = itens.ItemLoteID;
                    SalvarNfeReferenciaDoLoteItem(currentItem);
                }

                SalvarOuAtualizarLoteItens(lstItensReferencia);

                return itensSalvos;
            }
            return new List<INFeLoteItem>();
        }
        private void SalvarNfeReferenciaDoLoteItem(INFeLoteItem item)
        {

            if (item.NotaFiscalReferenciados != null && item.NotaFiscalReferenciados.Count > 0)
            {
                foreach (var ntRef in item.NotaFiscalReferenciados)
                {
                    ntRef.LoteItemID = item.ItemLoteID;
                }
                SalvarNotaFiscalReferenciadas(item.NotaFiscalReferenciados);
            }
                    
        }
        public INFeLoteItem CriarLoteItem(int? codPedido, int? LoteID, string CodContrato = null, int? nfConfigID = null)
        {
            if (codPedido != null &&
                LoteID != null)
            {
                var loteItm = Activator.CreateInstance<TNFeLoteItem>();
                if (loteItm != null)
                {
                    loteItm.LoteID = LoteID;
                    loteItm.DataFaturamento = DateTime.Now;
                    loteItm.DataEmissao = DateTime.Now;
                    loteItm.Status = StatusLoteItemEnum.ENVIO_PENDENTE;
                    loteItm.CodPedido = codPedido;
                    loteItm.CodContrato = CodContrato;
                }
                
                var itensSalvos = SalvarOuAtualizarLoteItem(loteItm);

                if (loteItm.ItemLoteID == null)
                    loteItm.ItemLoteID = itensSalvos.ItemLoteID;
                return itensSalvos;
            }
            return null;
        }

        /// <summary>
        /// Extrai do DTO de nota Fiscal a Chave e o número da nota fiscal.
        /// Esse método não Salva no Banco.
        /// </summary>
        /// <param name="loteItem"></param>
        /// <param name="NFe"></param>
        public void InserirChaveENumeroDaNota(INFeLoteItem loteItem, NotaFiscal NFe)
        {
            if(loteItem != null &&
                NFe != null &&
                NFe.lstInfNFe != null &&
                NFe.lstInfNFe.Count > 0)
            {

                var infNFe = NFe.lstInfNFe[0];
                loteItem.NumeroNota = (infNFe.Identificacao != null) ? infNFe.Identificacao.NumeroNotaFiscal : null;
                loteItem.ChaveNota = infNFe.Id;

                if (!string.IsNullOrWhiteSpace(loteItem.ChaveNota))
                {
                    loteItem.ChaveNota = loteItem.ChaveNota.Replace("NFe", "");
                }
            }
        }

        /// <summary>
        /// Extrai do DTO de nota Fiscal a Chave e o número da nota fiscal.
        /// Esse método não Salva no Banco.
        /// </summary>
        /// <param name="loteItem"></param>
        /// <param name="NFe"></param>
        public void InserirChaveENumeroDaNotaFiscalServico(INFeLoteItem loteItem, Rps rps)
        {
            if (loteItem != null &&
                rps != null &&
                rps.InfRps != null &&
                rps.InfRps.IdentificacaoRps != null)
            {
                var infRps = rps.InfRps.IdentificacaoRps;
                loteItem.NumeroRps = infRps.Numero;
                loteItem.Serie = infRps.Serie;
            }
        }

    }
}
