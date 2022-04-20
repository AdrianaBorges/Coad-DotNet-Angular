using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Exceptions;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using COAD.CORPORATIVO.Service.Boleto;

namespace COAD.CORPORATIVO.Service
{
    public class CnabRegistrosSRV : GenericService<CNAB_REGISTROS_TEMP, CnabRegistrosDTO, int>
    {
        public CnabRegistrosDAO _dao = new CnabRegistrosDAO();

        public CnabRegistrosSRV()
        {
            this.Dao = _dao;
        }

        /// <summary>
        /// ALT: 06/09/2016 - Este método seleciona os registros que irão compor os arquivos remessas.
        /// </summary>
        /// <param name="remessa"></param>
        /// <returns></returns>
        public List<CnabRegistrosDTO> SelecionarRegistrosCNAB(int _rem_id, bool preAlocado = false)
        {
            return _dao.SelecionarRegistrosCNAB(_rem_id, preAlocado).ToList();
        }
    }
}
