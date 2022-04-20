

using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Repositorios.Base;
using System.Transactions;
using Coad.GenericCrud.Dao.Base.Pagination;
using GenericCrud.Config.DataAttributes;
using System.Web;
using System.IO;
using GenericCrud.Service;
using GenericCrud.Validations;
using Coad.GenericCrud.Exceptions;
using COAD.RM.Repositorios.Base;
using COAD.RM.DAO;
using COAD.RM.Model.Dto;
using COAD.RM.Repositorios.Contexto;

namespace COAD.RM.Service
{
    [ServiceConfig("CODCOLIGADA", "CHAPA", "ANOCOMP", "MESCOMP", "NROPERIODO")]
    public class PfperffSRV : GenericService<PFPERFF, PfperffDTO, object>
    {

        public PfperffDAO _dao { get; set; }

        public PfperffSRV(PfperffDAO _dao)
        {
            this._dao = _dao;
            this.Dao = _dao;
        }

        public IList<PfperffDTO> ListarContraCheque(string _cpf)
        {
            return _dao.ListarContraCheque(_cpf);
        }
        public ContraChequeCustomDTO BuscarContraCheque(string _cpf, string _empresa, string _ano, string _mes, string _periodo)
        {
            return _dao.BuscarContraCheque(_cpf, _empresa, _ano, _mes, _periodo);

        }
    }
}
