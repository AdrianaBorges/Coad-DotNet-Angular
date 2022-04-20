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
using Coad.GenericCrud.Exceptions;
using System.Transactions;
using COAD.CORPORATIVO.Model.Comparators;
using COAD.CORPORATIVO.Model.Dto.Custons;
using GenericCrud.Config.DataAttributes;
using GenericCrud.Config.DataAttributes.Maps;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("TP_ID")]
    public class TabelaPrecoSRV : GenericService<TABELA_PRECO, TabelaPrecoDTO, int>
    {
        private TabelaPrecoDAO _dao = new TabelaPrecoDAO();

        [ServiceProperty("TP_ID", Name = "rgTabPreco", PropertyName = "REGIAO_TABELA_PRECO")]
        public RegiaoTabelaPrecoSRV _regTabPrecoSRV {get; set;}

        [ServiceProperty("TP_ID", Name = "tabPrecoTpPag", PropertyName = "TABELA_PRECO_TIPO_PAGAMENTO")]
        public TabelaPrecoTipoPagamentoSRV _tabPrecoTpPagSRV { get; set; }

        public TabelaPrecoSRV()
        {
            this.Dao = _dao;
            this._regTabPrecoSRV = new RegiaoTabelaPrecoSRV();
            this._tabPrecoTpPagSRV = new TabelaPrecoTipoPagamentoSRV();
        }

        public TabelaPrecoSRV(TabelaPrecoDAO _dao)
        {
            this._dao = _dao;
            this.Dao = _dao;
        }

        public override TabelaPrecoDTO Save(TabelaPrecoDTO source)
        {
            if (source != null)
            {
                var regiaoTabelaPreco = source.REGIAO_TABELA_PRECO;
                source.REGIAO_TABELA_PRECO = null;            
                var dto = base.Save(source);

                source.REGIAO_TABELA_PRECO = regiaoTabelaPreco;
                return dto;
            }

            return null;
        }

        /// <summary>
        /// Pega os dados da tabela de preço a partir do id da composição
        /// </summary>
        /// <param name="CMP_ID"></param>
        /// <returns></returns>
        public IList<TabelaPrecoDTO> GetTabelaPrecoByComposicao(int CMP_ID)
        {
            var tabelaPreco = _dao.GetTabelaPrecoByComposicao(CMP_ID);
            GetAssociations(tabelaPreco, "tabPrecoTpPag");
            PreencherRegiaoTabelaPreco(tabelaPreco);

            return tabelaPreco;

        }

  
        
        private void _ValidarChavesTabelaPreco(TabelaPrecoDTO tabelaPreco)
        {
            if (tabelaPreco == null)
            {
                throw new ValidacaoException("Tabela de preço não pode ser null");
            }

            if (tabelaPreco.CMP_ID == null)
            {
                throw new ValidacaoException("O campo CMP_ID não pode ser null");
            }
        }

        private void _processarSalvamentoTabelaPreco(TabelaPrecoDTO tabelaPreco)
        {
            // Valida os campos essenciais para o salvamento
            _ValidarChavesTabelaPreco(tabelaPreco);

            var tipoPeriodo = tabelaPreco.TIPO_PERIODO;

            if (tabelaPreco.TTP_ID != null && tipoPeriodo != null)
            {
                tabelaPreco.TTP_ID = tipoPeriodo.TTP_ID;

                if (tipoPeriodo.TTP_RECORRENTE)
                {
                    if (tabelaPreco.TP_NUM_PARCELAS_MAX > 1 || tabelaPreco.TP_NUM_PARCELAS_MIN > 1)
                    {
                        throw new ValidacaoException("Não é possível salvar uma tabela. Configuração de venda recorrente deve possuir apenas 1 parcela.");
                    }
                }
                
            }

            var regiaoTabelaPreco = tabelaPreco.REGIAO_TABELA_PRECO;
            tabelaPreco.REGIAO_TABELA_PRECO = null;

            var tabelaPrecoSalva = SaveOrUpdate(tabelaPreco);
            tabelaPreco.TP_ID = tabelaPrecoSalva.TP_ID;

            tabelaPreco.REGIAO_TABELA_PRECO = regiaoTabelaPreco;

            // Inicia o processo de salvamento das Regioes da tabela de preço
            _regTabPrecoSRV.SalvarEExcluirRegiaoTabelaPreco(tabelaPreco);
            _tabPrecoTpPagSRV.SalvarEExcluirTabelaPrecoTipoPagamento(tabelaPreco);
        }


        /// <summary>
        /// Salva a tabela de preço de acordo com os campo chaves
        /// </summary>
        /// <param name="tabelaPreco"></param>
        public void SalvarTabelaPreco(TabelaPrecoDTO tabelaPreco) 
        {
            if (tabelaPreco != null)
            {
                SalvarTabelaPreco(new List<TabelaPrecoDTO>(){ tabelaPreco});
            }
        }


        /// <summary>
        /// Salva a tabela de preço de acordo com os campo chaves
        /// </summary>
        /// <param name="tabelaPreco"></param>
        public void SalvarTabelaPreco(IEnumerable<TabelaPrecoDTO> lstTabelaPreco, IEnumerable<TabelaPrecoDTO> lstDeletar = null)
        {

            // Configuro e crio um transação
            var txOpt = new TransactionOptions();
            txOpt.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted;
            txOpt.Timeout = TransactionManager.MaximumTimeout;

            if (lstTabelaPreco != null)
            {
                using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required, txOpt))
                {
                    DeletarTabelaPreco(lstDeletar);

                    foreach (var tabPreco in lstTabelaPreco)
                    {
                        if (tabPreco.TTP_ID == null)
                        {
                            tabPreco.TTP_ID = 6;
                        }
                        _processarSalvamentoTabelaPreco(tabPreco);
                    }


                    //Atualizar dados do plano


                    scope.Complete();
                }
            }

        }

        public void DeletarTabelaPreco(IEnumerable<TabelaPrecoDTO> lstDeletar = null)
        {
            if (lstDeletar != null)
            {
                foreach (var obj in lstDeletar)
                {
                    //_regTabPrecoSRV.DeletarRegiaoTabelaPreco(obj.REGIAO_TABELA_PRECO);
                    obj.TP_DATA_EXCLUSAO = DateTime.Now;
                    _regTabPrecoSRV.ExcluirRegiaoTabelaPreco(obj);
                }

                MergeAll(lstDeletar);                
            }
        }

        /// <summary>
        /// Salva a tabela de preço de acordo com os campo chaves
        /// </summary>
        /// <param name="tabelaPreco"></param>
        public void SalvarTabelaPreco(TabelaPrecoSaveRequestDTO tabelaPrecoSaveRequest)
        {
            var lstSalvar = tabelaPrecoSaveRequest.TABELA_PRECO_ATUALIZAR;
            var lstDeletar = tabelaPrecoSaveRequest.TABELA_PRECO_EXCLUSAO;
            SalvarTabelaPreco(lstSalvar, lstDeletar);
        }

        public TabelaPrecoDTO FindByIdFullLoaded(int? TP_ID, bool trazRegiaoTabelaPreco = false, bool trazTabelaPrecoTipoPagamento = false)
        {
            var tabPreco = FindById(TP_ID);

            var lstAlias = new List<string>();

            if (trazRegiaoTabelaPreco)
            {
                PreencherRegiaoTabelaPreco(tabPreco);
            }

            if (trazTabelaPrecoTipoPagamento)
            {
                lstAlias.Add("tabPrecoTpPag");
            }

            GetAssociations(tabPreco, lstAlias.ToArray());
            return tabPreco;
        }

        public IList<TabelaPrecoDTO> ListarTabelaPrecoByProdutoERegiao(int? CMP_ID, int? RG_ID)
        {
            return _dao.ListarTabelaPrecoByProdutoERegiao(CMP_ID, RG_ID);
        }

        public void PreencherRegiaoTabelaPreco(IEnumerable<TabelaPrecoDTO> lstTabelaPreco)
        {
            if (lstTabelaPreco != null)
            {
                foreach (var tabPreco in lstTabelaPreco)
                {
                    PreencherRegiaoTabelaPreco(tabPreco);
                }
            }
        }


        public void PreencherRegiaoTabelaPreco(TabelaPrecoDTO tabelaPreco)
        {
            if (tabelaPreco != null && tabelaPreco.TP_ID != null)
            {
                var tpId = tabelaPreco.TP_ID;
                var lstRegiaoTabelaPreco = _regTabPrecoSRV.ListarPorTabelaPreco(tpId);
                tabelaPreco.REGIAO_TABELA_PRECO = lstRegiaoTabelaPreco;
            }
        }

    }
}
