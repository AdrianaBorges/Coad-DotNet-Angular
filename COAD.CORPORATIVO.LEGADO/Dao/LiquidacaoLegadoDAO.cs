using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.CORPORATIVO.LEGADO.Model.Dto;
using COAD.CORPORATIVO.LEGADO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.LEGADO.Dao
{
    public class LiquidacaoLegadoDAO : AbstractGenericDao<liquidacao, LiquidacaoLegadoDTO, string>
    {
        public corporativo2Entities db { get { return GetDb<corporativo2Entities>(); } set { } }

        public LiquidacaoLegadoDAO()
        {
            SetProfileName("corp_old");
            db = GetDb<corporativo2Entities>();
        }

        public IList<LiquidacaoLegadoDTO> LerLiquidacaoLegado(string contrato = null, string letra = null, string cd = null)
        {
            IQueryable<liquidacao> query = GetDbSet();

            if (!String.IsNullOrWhiteSpace(contrato))
            {
                query = query.Where(x => x.CONTRATO == contrato);
            }
            if (!String.IsNullOrWhiteSpace(letra))
            {
                query = query.Where(x => x.LETRA == letra);
            }
            if (!String.IsNullOrWhiteSpace(cd))
            {
                query = query.Where(x => x.CD == cd);
            }

            return ToDTO(query);
        }

        /// <summary>
        /// Lista as liquidações exatamente pelo número da parcela
        /// </summary>
        /// <param name="parNumero">Busca pelo campo de número de parcela</param>
        /// <param name="pendenteGeracaoCodigo">true para retornar as liquidações que precisa ter seu código atualizado.</param>
        /// <returns></returns>
        public IList<LiquidacaoLegadoDTO> ListarLiquidacaoPorCodigoDaParcela(string parNum, bool? pendenteGeracaoCodigo = null)
        {
            
            var query = (from parLiq in
                                db.liquidacao
                            where
                            parLiq.PAR_NUM_PARCELA == parNum &&
                            (pendenteGeracaoCodigo == null ||
                                parLiq.atualizarCodigo == pendenteGeracaoCodigo)
                            select parLiq);
            return ToDTO(query);
        }

    }
}
