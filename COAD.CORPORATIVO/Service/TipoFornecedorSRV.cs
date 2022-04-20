using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Service
{
    public  class TipoFornecedorSRV :  ServiceAdapter<TIPO_FORNECEDOR,TipoFornecedorDTO>
    {
        private TipoFornecedorDAO _dao = new TipoFornecedorDAO();

        public TipoFornecedorSRV()
        {
            SetDao(_dao);
        }
    }
}
