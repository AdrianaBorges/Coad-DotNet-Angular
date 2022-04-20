using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Custons;
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
    public class ImportacaoResultadoRodizioDAO : DAOAdapter<IMPORTACAO_RESULTADO_RODIZIO, ImportacaoResultadoRodizioDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public ImportacaoResultadoRodizioDAO()
        {
            db = GetDb<COADCORPEntities>();
        }        

        public IList<ImportacaoResultadoRodizioDTO> ListarResultadosDeRodizio(int? impID)
        {
            var query = (from imRR in db.IMPORTACAO_RESULTADO_RODIZIO
                         where
                            imRR.IMP_ID == impID
                        select imRR);

            return ToDTO(query);
        }


        public ImportacaoResultadoRodizioDTO BuscarResultado(int? impID, int? repID, int? rgID)
        {
            var query = (from imRR in db.IMPORTACAO_RESULTADO_RODIZIO
                         where 
                            imRR.IMP_ID == impID &&
                            imRR.REP_ID == repID &&
                            imRR.RG_ID == rgID
                         select imRR);

            return ToDTO(query.FirstOrDefault());
        }
    }
}
