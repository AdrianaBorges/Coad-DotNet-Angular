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
    public class ProdutoFamiliaSRV : ServiceAdapter<PRODUTO_FAMILIA, ProdutoFamiliaDTO, int>
    {
        private ProdutoFamiliaDAO _dao = new ProdutoFamiliaDAO();

        public ProdutoFamiliaSRV()
        {
            SetDao(_dao);
        }

    }
}
