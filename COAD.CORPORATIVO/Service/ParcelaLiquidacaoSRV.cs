using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("PAR_NUM_PARCELA","PLI_TIPO_DOC","PLI_NUMERO")]
    public class ParcelaLiquidacaoSRV : ServiceAdapter<PARCELA_LIQUIDACAO, ParcelaLiquidacaoDTO, string>
    {
        private ParcelaLiquidacaoDAO _dao = new ParcelaLiquidacaoDAO();
        private TipoPagamentoSRV _tipoPagamentoSRV = new TipoPagamentoSRV();

        public ParcelaLiquidacaoSRV()
        {
            SetDao(_dao);
        }

        public IList<ParcelaLiquidacaoDTO> BuscarPorParcela(string _par_num_parcela)
        {
            return _dao.BuscarPorParcela(_par_num_parcela);
        }
        public Pagina<ParcelaLiquidacaoDTO> BuscarPorParcela(string _par_num_parcela, int numpagina = 1, int linhas = 10)
        {
            return _dao.BuscarPorParcela(_par_num_parcela, numpagina, linhas);
        }

        public void InserirLiquidacao(ParcelasDTO parcela)
        {
            //if (parcela != null)
            //{
            //    string docSigla = null;
            //    var tpgId = parcela.TPG_ID;

            //    var tipoPagamento =  _tipoPagamentoSRV.FindById(tpgId);

            //    if (tipoPagamento != null)
            //    {
            //        docSigla = tipoPagamento.DLI_SIGLA;
            //    }


            //    var parcelaLiquidacao = new ParcelaLiquidacaoDTO()
            //    {
            //        PAR_NUM_PARCELA = parcela.PAR_NUM_PARCELA,
            //        PLI_TIPO_DOC = docSigla,
            //        PLI_NUMERO = "0000000000",
            //        PLI_DATA = DateTime.Now,
            //        PLI_VALOR = parcela.PAR_VLR_PAGO,
            //        PLI_DATA_BAIXA = DateTime.Now,
            //    };

            //    SaveOrUpdateNonIdentityKeyEntity(parcelaLiquidacao);
            //}
        }

        public void RemoverLiquidacoes(string parNumParcela)
        {
            if (!string.IsNullOrWhiteSpace(parNumParcela))
            {
                var liquidacoes = this.BuscarPorParcela(parNumParcela);
                if(liquidacoes != null && liquidacoes.Count() > 0)
                {
                    DeleteAll(liquidacoes);
                }
            }
        }

    }
}
