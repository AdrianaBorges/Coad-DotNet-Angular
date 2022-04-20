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
    public class CadernoSRV : GenericService<CADERNO, CadernoDTO, int>
    {
        private CadernoDAO _dao = new CadernoDAO();

        public CadernoSRV()
        {
            Dao = _dao;
        }

        public IList<CadernoDTO> BuscarCadernosCliente(int idCliente)
        {
            return _dao.BuscarCadernosCliente(idCliente);
        }

        public CadernoDTO BuscarCadernoRepetido(int idCliente, string nomeCad)
        {
            return _dao.BuscarCadernoRepetido(idCliente, nomeCad);
        }
    }
}
