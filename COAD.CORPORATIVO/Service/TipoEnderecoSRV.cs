using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Service.Base;
using Coad.GenericCrud.Repositorios.Base;

namespace COAD.CORPORATIVO.Service
{
    public class TipoEnderecoSRV : GenericService<TIPO_ENDERECO, TipoEnderecoDTO, int>
    {
        public TipoEnderecoDAO _dao = new TipoEnderecoDAO();

        public TipoEnderecoSRV()
        {
            Dao = _dao;
        }
    }
}
