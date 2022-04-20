using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Service.Base;
using COAD.COADGED.DAO;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Repositorios.Contexto;

namespace COAD.COADGED.Service
{
    public class CadernoConteudoSRV : GenericService<CADERNO_CONTEUDO, CadernoConteudoDTO, int>
    {
        private CadernoConteudoDAO _dao = new CadernoConteudoDAO();

        public CadernoConteudoSRV()
        {
            Dao = _dao;
        }
    }
}
