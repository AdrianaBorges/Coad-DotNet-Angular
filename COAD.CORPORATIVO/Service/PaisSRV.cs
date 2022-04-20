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
    public class PaisSRV : GenericService<PAIS, PaisDTO, string>
    {
        public PaisDAO _dao = new PaisDAO();

        public PaisSRV()
        {
            Dao = _dao;
        }
    }
}
