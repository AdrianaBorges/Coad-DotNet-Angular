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
//using Coad.GenericCrud.Service.Base;
using Coad.GenericCrud.Repositorios.Base;

namespace COAD.CORPORATIVO.Service
{
    public class OrigemCadastroSRV : GenericService<ORIGEM_CADASTRO, OrigemCadastroDTO, int>
    {
        public OrigemCadastroDAO _dao = new OrigemCadastroDAO();

        public OrigemCadastroSRV()
        {
            this.Dao = _dao;
        }

        public OrigemCadastroDTO ObterOrigemCadastroPorNome(string descOrigem)
        {
            return _dao.ObterOrigemCadastroPorNome(descOrigem);
        }
    }
}
