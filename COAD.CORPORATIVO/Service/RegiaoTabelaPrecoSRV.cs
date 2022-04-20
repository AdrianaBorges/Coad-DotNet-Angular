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
using COAD.CORPORATIVO.Model.Comparators;
using GenericCrud.Config.DataAttributes;
using COAD.CORPORATIVO.Model.Dto.Custons.Relatorios;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.CORPORATIVO.Util;
using GenericCrud.Service;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("TP_ID" ,"RG_ID")]
    public class RegiaoTabelaPrecoSRV : GenericService<REGIAO_TABELA_PRECO, RegiaoTabelaPrecoDTO, int>
    {
        public RegiaoTabelaPrecoDAO _dao = new RegiaoTabelaPrecoDAO();

        public RegiaoTabelaPrecoSRV()
        {
            Dao = _dao;
        }

        public RegiaoTabelaPrecoDTO BuscarTabelaPreco(int _RG_ID, int _CMP_ID)
        {
            return _dao.BuscarTabelaPreco(_RG_ID, _CMP_ID);
        }

        public bool HasRegiaoTabelaPreco(int TP_ID, int RG_ID)
        {
            return _dao.HasRegiaoTabelaPreco(TP_ID, RG_ID);
        }

        /// <summary>
        /// Pega a lista de RegiaoTabelaPreco e preenche com os campos chave da tabela preço passada.
        /// </summary>
        /// <param name="tabelaPreco"></param>
        public void PreecherChavesRegiao(IEnumerable<RegiaoTabelaPrecoDTO> lstRegTabPreco, TabelaPrecoDTO tabelaPreco)
        {
            if (lstRegTabPreco != null && tabelaPreco != null)
            {
                foreach (var regTabPreco in lstRegTabPreco)
                {
                    if (regTabPreco.REGIAO != null)
                    {
                        regTabPreco.RG_ID = regTabPreco.REGIAO.RG_ID;
                    }
                }
            }
        }

        /// <summary>
        /// Processa o salvamento da região da tabela de preço
        /// </summary>
        /// <param name="tabelaPreco"></param>
        public void SalvarEExcluirRegiaoTabelaPreco(TabelaPrecoDTO tabelaPreco)
        {
            var regiaoTabelaPreco = tabelaPreco.REGIAO_TABELA_PRECO;
            if (regiaoTabelaPreco != null)
            {
                ExcluirRegiaoTabelaPreco(tabelaPreco);
                SalvarRegiaoTabelaPreco(regiaoTabelaPreco, tabelaPreco);
            }

        }
        /// <summary>
        /// Salva a RegiaoTabelaPreco
        /// </summary>
        /// <param name="regiaoTabelaPreco"></param>
        public void SalvarRegiaoTabelaPreco(IEnumerable<RegiaoTabelaPrecoDTO> regiaoTabelaPreco)
        {
            if (regiaoTabelaPreco != null)
            {
                SaveOrUpdateNonIdentityKeyEntity(regiaoTabelaPreco, "HasRegiaoTabelaPreco");
            }
            
        }

        public void SalvarRegiaoTabelaPreco(IEnumerable<RegiaoTabelaPrecoDTO> regiaoTabelaPreco, TabelaPrecoDTO tabelaPreco)
        {
            PreecherChavesRegiao(regiaoTabelaPreco, tabelaPreco);
            CheckAndAssignKeyFromParentToChildsList(tabelaPreco, regiaoTabelaPreco, "TP_ID");            
            SalvarRegiaoTabelaPreco(regiaoTabelaPreco);
        }


        /// <summary>
        /// Excluir os telefones do prospect, exceto os passados no método
        /// </summary>
        /// <param name="regiaoTabPreco"></param>
        /// <param name="excecoes"></param>
        public void ExcluirRegiaoTabelaPreco(TabelaPrecoDTO tabPreco)
        {
            IEnumerable<RegiaoTabelaPrecoDTO> listRegiaoTabelaPreco = new List<RegiaoTabelaPrecoDTO>();

            if (tabPreco.TP_DATA_EXCLUSAO == null)
            {
                var tpId = tabPreco.TP_ID;
                var tabelaDoBanco = new TabelaPrecoSRV().FindByIdFullLoaded(tpId, true);

                listRegiaoTabelaPreco = GetMissinList<TabelaPrecoDTO>(tabPreco, tabelaDoBanco, "REGIAO_TABELA_PRECO");
            }
            else
            {
                listRegiaoTabelaPreco = tabPreco.REGIAO_TABELA_PRECO;    
            }

            DeletarRegiaoTabelaPreco(listRegiaoTabelaPreco);
        }

        public void DeletarRegiaoTabelaPreco(IEnumerable<RegiaoTabelaPrecoDTO> regiaoTabelaPrecoPraExcluir)
        {
            if (regiaoTabelaPrecoPraExcluir != null)
            {
                foreach (var regTab in regiaoTabelaPrecoPraExcluir)
                {
                    regTab.RTP_DATA_EXCLUSAO = DateTime.Now;
                }

                SaveOrUpdateAll(regiaoTabelaPrecoPraExcluir);
            }
        }

        public IList<RegiaoTabelaPrecoDTO> ListarRegiaoTabelaPrecoPorRegiaoEProdutoComposicao(int? RG_ID, int? CMP_ID)
        {
            var lstRegiaoTabelaPreco = _dao.ListarRegiaoTabelaPrecoPorRegiaoEProdutoComposicao(RG_ID, CMP_ID);
            var lstTabelaPreco = lstRegiaoTabelaPreco.Select(sel => sel.TABELA_PRECO);

            ServiceFactory.RetornarServico<TabelaPrecoSRV>().GetAssociations(lstTabelaPreco, "tabPrecoTpPag");
            return lstRegiaoTabelaPreco;
        }

        public IList<RegiaoTabelaPrecoDTO> ListarRegiaoTabelaPrecoPorRegiaoProdutoComposicaoETipoPagamento(int? RG_ID, int? CMP_ID, int? TPG_ID, int? TTP_ID = null, bool ehCurso = false)
        {
            var lstRegiaoTabelaPreco = _dao.ListarRegiaoTabelaPrecoPorRegiaoProdutoComposicaoETipoPagamento(RG_ID, CMP_ID, TPG_ID, TTP_ID, ehCurso);
            var lstTabelaPreco = lstRegiaoTabelaPreco.Select(sel => sel.TABELA_PRECO);

            ServiceFactory.RetornarServico<TabelaPrecoSRV>().GetAssociations(lstTabelaPreco, "tabPrecoTpPag");
            return lstRegiaoTabelaPreco;
        }
        public ResumoParcelamentoDTO ListarResumoDeParcelamento(int? RG_ID, int? CMP_ID, int? TPG_ID, int? QTD = 1, int? TTP_ID = null, int? QTDPARCELAS = null)
        {
            var lstResumoParcelamento = this.ListarResumoDeParcelamento(RG_ID, CMP_ID, TPG_ID, QTD, TTP_ID);
            
            ResumoParcelamentoDTO _tipopagamento = null;

            if (lstResumoParcelamento.Count() > 0)
                _tipopagamento = lstResumoParcelamento.Where(x => x.Parcela == QTDPARCELAS).FirstOrDefault();

            return _tipopagamento;
        }
        public IList<ResumoParcelamentoDTO> ListarResumoDeParcelamento(int? RG_ID, int? CMP_ID, int? TPG_ID, int? QTD = 1, int? TTP_ID = null)
        {
            var ehCurso = ServiceFactory.RetornarServico<ProdutoComposicaoSRV>().ChecaProdutoComposicaoEhCurso((int)CMP_ID);

            if (ehCurso)
            {                
                TTP_ID = null;
            }
            else
            {
                if (TTP_ID == null)
                {
                    TTP_ID = 6;
                }
            }

            var lstRegiaoTabelaPreco = ListarRegiaoTabelaPrecoPorRegiaoProdutoComposicaoETipoPagamento(RG_ID, CMP_ID, TPG_ID, TTP_ID, ehCurso);
            var lstResumoParcelamento = _criarResumoDeParcelamento(lstRegiaoTabelaPreco, TPG_ID, QTD, TTP_ID, CMP_ID);
            return lstResumoParcelamento;
        }

        private IList<ResumoParcelamentoDTO> _criarResumoDeParcelamento(IEnumerable<RegiaoTabelaPrecoDTO> lstRegiaoTabelaPreco, int? TPG_ID, int? QTD, int? TTP_ID = null, int? CMP_ID = null)
        {
            TipoPagamentoDTO tipoPagamento = new TipoPagamentoSRV().FindByIdFullLoaded(TPG_ID);
            TipoPeriodoDTO tipoPeriodo = null;
            var entrada = (tipoPagamento.TPG_TIPO == 1);
            var ehCurso = false;
            
            if (CMP_ID != null)
            {
                ehCurso = new ProdutoComposicaoSRV().ChecaProdutoComposicaoEhCurso((int)CMP_ID);
                if (ehCurso)
                {
                    TTP_ID = 7;
                }
            }
            else
            {
                if (TTP_ID == null)
                {
                    TTP_ID = 6;
                }
            }

            tipoPeriodo = ServiceFactory.RetornarServico<TipoPeriodoSRV>().FindById(TTP_ID);
            IList<ResumoParcelamentoDTO> lstResumoParcelamento = new List<ResumoParcelamentoDTO>();

            if (tipoPagamento != null && lstRegiaoTabelaPreco != null)
            {
                int? ultimaParcela = 0;
                foreach (var regTabelaPreco in lstRegiaoTabelaPreco)
                {
                    if(regTabelaPreco != null && regTabelaPreco.TABELA_PRECO != null){

                        var tabelaPreco = regTabelaPreco.TABELA_PRECO;
                        int? parcelaInicial = 1;
                        int? parcelaFinal = 1;
                        bool? permitirParcelaCortesia = tabelaPreco.TP_PERMITIR_CORTESIA_PRIMEIRA_PARCELA;
                       
                        if (tipoPeriodo == null || tipoPeriodo.TTP_RECORRENTE != true)
                        {
                            parcelaInicial = regTabelaPreco.TABELA_PRECO.TP_NUM_PARCELAS_MIN;
                            parcelaFinal = regTabelaPreco.TABELA_PRECO.TP_NUM_PARCELAS_MAX;
                        }

                        if (parcelaInicial <= ultimaParcela)
                        {
                            parcelaInicial = ultimaParcela + 1;
                        }

                        if (entrada)
                        {
                            parcelaInicial++;
                            parcelaFinal++;
                        }

                        ultimaParcela = parcelaFinal;

                        for (int? index = parcelaInicial; index <= parcelaFinal; index++)
                        {
                            decimal? numParcela = Convert.ToDecimal(index);
                            decimal? precoUnitario = regTabelaPreco.RTP_PRECO_VENDA;
                            decimal? total = (regTabelaPreco.RTP_PRECO_VENDA * QTD);
                            decimal? precoParcela = MathUtil.TruncarCasasDecimais((total / numParcela), 2);
                            

                            var resumoTabelaPreco = new ResumoParcelamentoDTO()
                            {
                                PermitirParcelaCortesia = permitirParcelaCortesia,
                                Parcela = index,
                                REGIAO_TABELA_PRECO = regTabelaPreco,
                                TIPO_PAGAMENTO = tipoPagamento,
                                TIPO_PERIODO = tipoPeriodo,
                                PrecoUnitario = precoUnitario,
                                ValorParcela = precoParcela,
                                Total = total,
                            };

                            lstResumoParcelamento.Add(resumoTabelaPreco);
                        }
                    }
                    
                }
            }

            return lstResumoParcelamento;
        }

        public IList<RegiaoTabelaPrecoDTO> ListarPorTabelaPreco(int? TP_ID)
        {
            return _dao.ListarPorTabelaPreco(TP_ID);
        }

    }
}
