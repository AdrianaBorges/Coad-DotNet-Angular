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
    public class CadernoCompartilhadoSRV : GenericService<CADERNO_COMPARTILHADO, CadernoCompartilhadoDTO, int>
    {
        private CadernoCompartilhadoDAO _dao = new CadernoCompartilhadoDAO();

        public CadernoCompartilhadoSRV()
        {
            Dao = _dao;
        }
    }
}
