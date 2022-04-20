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


namespace COAD.CORPORATIVO.DAO
{
    public class OrigemCadastroDAO : AbstractGenericDao<ORIGEM_CADASTRO, OrigemCadastroDTO,int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public OrigemCadastroDAO()
        {
            db = GetDb<COADCORPEntities>();
        }

        public OrigemCadastroDTO ObterOrigemCadastroPorNome(string descOrigem)
        {
            var query = (from 
                             oc in db.ORIGEM_CADASTRO 
                         where oc.O_CAD_DESCRICAO.Trim().ToLower() == descOrigem.Trim().ToLower() 
                         select oc)
                .FirstOrDefault();

            return ToDTO(query);
        }
    }
}
