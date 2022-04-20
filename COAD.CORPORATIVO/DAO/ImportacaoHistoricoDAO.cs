using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using System.Data.Objects;

namespace COAD.CORPORATIVO.DAO
{
    public class ImportacaoHistoricoDAO : AbstractGenericDao<IMPORTACAO_HISTORICO, ImportacaoHistoricoDTO,int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public ImportacaoHistoricoDAO()
        {
            db = GetDb<COADCORPEntities>(false);
        }

        public Pagina<ImportacaoHistoricoDTO> PesquisarHistorico(DateTime? dataInicial, DateTime? dataFinal,
            int? impID,
            int? ipsID, 
            int? imsID, 
            int pagina = 1, 
            int registroPorPagina = 6)
        {
            var query = (from im in db.IMPORTACAO_HISTORICO 
                         where
                            (dataInicial == null || EntityFunctions.TruncateTime(im.IMH_DATA) >= EntityFunctions.TruncateTime(dataInicial)) &&
                            (dataFinal == null || EntityFunctions.TruncateTime(im.IMH_DATA) <= EntityFunctions.TruncateTime(dataFinal)) &&
                            (imsID == null || im.IMS_ID == imsID) &&
                            (impID == null || im.IMP_ID == impID) &&
                            ((ipsID == null && im.IMH_HISTORICO_DA_IMPORTACAO == true) || im.IPS_ID == ipsID) 
                            orderby im.IMH_DATA descending
                         select im);

            return ToDTOPage(query, pagina, registroPorPagina);
        }


        public ImportacaoHistoricoDTO BuscarUltimoHistoricoDeErro(
            int? ipsID)
        {
            var query = (from im in db.IMPORTACAO_HISTORICO
                         where
                            im.IPS_ID == ipsID &&
                            im.IMH_ERRO == true
                         orderby im.IMH_DATA descending
                         select im);

            return ToDTO(query.FirstOrDefault());
        }


        public ICollection<ImportacaoHistoricoDTO> BuscarUltimosHistoricosDeErroDaImportacao(
            int? impID)
        {
            var query = (from 
                            im in db.IMPORTACAO_HISTORICO join
                            impSus in db.IMPORTACAO_SUSPECT on im.IPS_ID equals impSus.IPS_ID
                         where
                            im.IMH_ERRO == true &&
                            im.IMP_ID == impID
                         orderby im.IMH_DATA descending
                         select im).Take(15);

            return ToDTO(query);
        }
    }
}
