using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Custons.Batch;
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
    public class ImportacaoDAO : DAOAdapter<IMPORTACAO, ImportacaoDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public ImportacaoDAO()
        {
            db = GetDb<COADCORPEntities>();
        }

        public ImportacaoDTO RetornarProximaImportacaoEmAberto()
        {
            var query = (from im in db.IMPORTACAO
                         where 
                            im.IMS_ID == 6 && 
                            im.IMP_DATA_CANCELAMENTO == null
                        orderby im.IMP_DATA descending
                        select im).FirstOrDefault();
            return ToDTO(query);
        }


        public Pagina<ImportacaoDTO> PesquisarImportacoes(
            PesquisaImportacaoDTO pesquisaImportacaoDTO)
        {
            if(pesquisaImportacaoDTO != null)
            {
                var dataInicial = pesquisaImportacaoDTO.dataFinal;
                var dataFinal = pesquisaImportacaoDTO.dataFinal;
                var imsID = pesquisaImportacaoDTO.imsID;
                var repID = pesquisaImportacaoDTO.repID;
                var impDiaria = pesquisaImportacaoDTO.importacaoDiaria;

                var query = (from im in db.IMPORTACAO
                             where
                                (dataInicial == null || EntityFunctions.TruncateTime(im.IMP_DATA) >= EntityFunctions.TruncateTime(dataInicial)) &&
                                (dataFinal == null || EntityFunctions.TruncateTime(im.IMP_DATA) <= EntityFunctions.TruncateTime(dataFinal)) &&
                                (repID == null || im.REP_ID == repID) &&
                                (imsID == null || im.IMS_ID == imsID) &&
                                (impDiaria == false || im.IMP_WEB_SERVICE == true)
                                orderby im.IMP_DATA descending
                             select
                           im);

                return ToDTOPage(query, pesquisaImportacaoDTO.pagina, pesquisaImportacaoDTO.registrosPorPagina);
            }

            return new Pagina<ImportacaoDTO>();
        }

        public ImportacaoDTO RetornarImportacaoWebServiceDoDia(DateTime? date)
        {
            var query = (from im in db.IMPORTACAO
                         where
                            im.IMP_DATA_CANCELAMENTO == null &&
                            im.IMP_WEB_SERVICE == true && 
                            im.IMS_ID != 5 &&
                            EntityFunctions.TruncateTime(im.IMP_DATA) == EntityFunctions.TruncateTime(date)
                            
                         orderby im.IMP_DATA descending
                         select im).FirstOrDefault();
            return ToDTO(query);
        }

    }
}
