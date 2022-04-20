using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.LEGADO.Dao;
using COAD.CORPORATIVO.LEGADO.Model.Dto;
using COAD.CORPORATIVO.LEGADO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.LEGADO.Service
{
    [ServiceConfig("CONTRATO", "LETRA", "CD", "TIPO_DOC","NUMERO")]
    public class LiquidacaoLegadoSRV : GenericService<liquidacao, LiquidacaoLegadoDTO, string>
    {
        private LiquidacaoLegadoDAO _dao = new LiquidacaoLegadoDAO();

        public LiquidacaoLegadoSRV()
        {
            Dao = _dao;
        }

        public IList<LiquidacaoLegadoDTO> LerLiquidacaoLegado(string contrato = null, string letra = null, string cd = null)
        {
            var resp = _dao.LerLiquidacaoLegado(contrato, letra, cd);
            return resp;
        }

        public void SalvarLiquidacaoLegado(IEnumerable<LiquidacaoLegadoDTO> lstLiquidacao)
        {
            if (lstLiquidacao != null)
            {
                SaveOrUpdateNonIdentityKeyEntity(lstLiquidacao);
            }
        }

        /// <summary>
        /// Lista as liquidações exatamente pelo número da parcela
        /// </summary>
        /// <param name="parNumero">Busca pelo campo de número de parcela</param>
        /// <param name="pendenteGeracaoCodigo">true para retornar as liquidações que precisa ter seu código atualizado.</param>
        /// <returns></returns>
        public IList<LiquidacaoLegadoDTO> ListarLiquidacaoPorCodigoDaParcela(string parNum, bool? pendenteGeracaoCodigo = null)
        {
            return _dao.ListarLiquidacaoPorCodigoDaParcela(parNum, pendenteGeracaoCodigo);
        }

        /// <summary>
        /// Pega todas as liquidações que não possuem uma referência correta a parcela e atualiza com o código de contrato.
        /// </summary>
        /// <returns></returns>
        public void IncluirCodigoContratoNaLiquidacao(IList<ParcelasLegadoDTO> lstParcelasLegado)
        {
            if (lstParcelasLegado != null)
            {
                IEnumerable<LiquidacaoLegadoDTO> lstLiquidacaoLegado = new List<LiquidacaoLegadoDTO>();

                foreach (var parLeg in lstParcelasLegado)
                {
                    var lstLiquiRetornado = ListarLiquidacaoPorCodigoDaParcela(parLeg.PAR_NUM_PARCELA, true);

                    if (lstLiquiRetornado != null && lstLiquiRetornado.Count() > 0)
                    {
                        foreach (var liqLeg in lstLiquiRetornado)
                        {
                            if (!string.IsNullOrWhiteSpace(liqLeg.PAR_NUM_PARCELA))
                            {
                                liqLeg.CONTRATO = parLeg.CONTRATO;
                                liqLeg.LETRA = parLeg.LETRA;
                                liqLeg.CD = parLeg.CD;
                            }
                        }

                        lstLiquidacaoLegado = lstLiquidacaoLegado.Concat(lstLiquiRetornado);
                    }
                }

                SaveOrUpdateNonIdentityKeyEntity(lstLiquidacaoLegado);
            }
        }

        public void RemoverLiquidacaoLegado(string contrato, string letra, string cd)
        {
            if (!string.IsNullOrWhiteSpace(contrato) &&
                !string.IsNullOrWhiteSpace(letra) &&
                !string.IsNullOrWhiteSpace(cd))
            {
                var liquidacoes = this.LerLiquidacaoLegado(contrato, letra, cd);
                if (liquidacoes != null && liquidacoes.Count() > 0)
                {
                    DeleteAll(liquidacoes);
                }
            }
        }
    }
}
