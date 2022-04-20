using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Custons.Pesquisas;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.DAO
{
    public class CampanhaVendaDAO : DAOAdapter<CAMPANHA_VENDA, CampanhaVendaDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public CampanhaVendaDAO()
        {
            db = GetDb<COADCORPEntities>();
        }
        
        public Pagina<CampanhaVendaDTO> PesquisarCampanhaVenda(PesquisaCampanhaVendaDTO pesquisa)
        {
            if(pesquisa != null)
            {
                var tppId = pesquisa.TppId;
                var DataInicioDe = pesquisa.DataInicioDe;
                var DataInicioAte = pesquisa.DataInicioAte;

                var DataFimDe = pesquisa.DataFimDe;
                var DataFimAte = pesquisa.DataFimAte;

                IQueryable<CAMPANHA_VENDA> query = (from cam in db.CAMPANHA_VENDA
                             where
                                cam.DATA_EXCLUSAO == null &&
                                (DataInicioDe == null || EntityFunctions.TruncateTime(cam.CVE_PERIODO_INICIAL) >= EntityFunctions.TruncateTime(DataInicioDe)) &&
                                (DataInicioAte == null || EntityFunctions.TruncateTime(cam.CVE_PERIODO_INICIAL) <= EntityFunctions.TruncateTime(DataInicioAte)) &&

                                (DataFimDe == null || EntityFunctions.TruncateTime(cam.CVE_PERIODO_FINAL) >= EntityFunctions.TruncateTime(DataFimDe)) &&
                                (DataFimAte == null || EntityFunctions.TruncateTime(cam.CVE_PERIODO_FINAL) <= EntityFunctions.TruncateTime(DataFimAte))
                             select cam);

                if(tppId != null)
                {
                    query = (from 
                                cam_tp in db.CAMPANHA_VENDA_TIPO_PROPOSTA join
                                cam in query on cam_tp.CVE_ID equals cam.CVE_ID
                             where cam_tp.TPP_ID == tppId
                             select cam);
                }

                return ToDTOPage(query, pesquisa.pagina, pesquisa.registrosPorPagina);

            }
            return new Pagina<CampanhaVendaDTO>();
        }

        public IList<CampanhaVendaDTO> BuscarCampanhaVenda(DateTime data, int? tppId, int? tpgId, int? numParcela, int? cmpId = null)
        {
            var query =
            (from
                cmVend in db.CAMPANHA_VENDA join
                cmTip in db.CAMPANHA_VENDA_TIPO_PROPOSTA on cmVend.CVE_ID equals cmTip.CVE_ID join
                tpCmVen in db.TIPO_PAGAMENTO_CAMPANHA_VENDA on cmVend.CVE_ID equals tpCmVen.CVE_ID
             where cmVend.DATA_EXCLUSAO == null &&
                 (
                     cmVend.CVE_CAMPANHA_ATIVA == true
                 )
                 &&
                 EntityFunctions.TruncateTime(cmVend.CVE_PERIODO_INICIAL) <= EntityFunctions.TruncateTime(data) &&
                 EntityFunctions.TruncateTime(cmVend.CVE_PERIODO_FINAL) >= EntityFunctions.TruncateTime(data) &&
                 cmTip.TPP_ID == tppId &&
                 tpCmVen.TPG_ID == tpgId &&
                 cmVend.CVE_NUM_PARCELA_MIN <= numParcela &&
                 cmVend.CVE_NUM_PARCELA_MAX >= numParcela &&
                 (
                     (cmpId == null && cmVend.CAMPANHA_VENDAS_PRODUTO_COMPOSICAO.Count <= 0 ) ||
                     (from
                        cvCmp in db.CAMPANHA_VENDAS_PRODUTO_COMPOSICAO 
                      where cvCmp.CMP_ID == cmpId
                     select cvCmp.CVE_ID).Contains(cmVend.CVE_ID)
                )
            select cmVend);

            return ToDTO(query);
        }
    }
}
