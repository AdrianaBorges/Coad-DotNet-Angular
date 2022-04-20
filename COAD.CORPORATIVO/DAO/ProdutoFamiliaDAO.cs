using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.DAO
{
    public class ProdutoFamiliaDAO : DAOAdapter<PRODUTO_FAMILIA, ProdutoFamiliaDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public ProdutoFamiliaDAO()
        {
            db = GetDb<COADCORPEntities>();
        }

    }
}
