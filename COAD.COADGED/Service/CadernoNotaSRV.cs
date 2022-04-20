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
    public class CadernoNotaSRV : GenericService<CADERNO_NOTA, CadernoNotaDTO, int>
    {
        private CadernoNotaDAO _dao = new CadernoNotaDAO();

        public CadernoNotaSRV()
        {
            Dao = _dao;
        }
    }
}
