using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Model;
using COAD.CORPORATIVO.Model.Dto;
using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Extensions;
using System.Data.SqlClient;
using COAD.CORPORATIVO.Service;

namespace COAD.CORPORATIVO.DAO
{
    public class CnabRegistrosDAO : AbstractGenericDao<CNAB_REGISTROS_TEMP , CnabRegistrosDTO, int> 
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }


        public CnabRegistrosDAO()
        {
            this.db = GetDb<COADCORPEntities>();
        }

        /// <summary>
        /// Este método seleciona e retorna os registros alocados para determinada remessa.
        /// </summary>
        /// <param name="remessa"></param>
        /// <returns></returns>
        public IList<CnabRegistrosDTO> SelecionarRegistrosCNAB(int _rem_id, bool preAlocado = false)
        {
            if (preAlocado)
            {
                var query = db.CNAB_REGISTROS_DIARIO(_rem_id);

                var lista = (from a in db.CNAB_REGISTROS_TEMP
                             where a.REM_ID == _rem_id
                             select a);
                return ToDTO(lista);
            }
            else
            {

                var query = db.CNAB_REGISTROS(_rem_id);
                var lista = (from a in db.CNAB_REGISTROS_TEMP
                             where a.REM_ID == _rem_id
                             select a);
                return ToDTO(lista);
            }
        }
    }
}
