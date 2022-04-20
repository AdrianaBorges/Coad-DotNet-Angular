using Coad.GenericCrud.Service.Base;
using COAD.PORTAL.DAO.Uras;
using COAD.PORTAL.Model.DTO.Uras;
using COAD.PORTAL.Repositorios.Contexto;
using COAD.SEGURANCA.Repositorios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace COAD.PORTAL.Service.Uras
{
    public class coadSRV : GenericService<coad, coadDTO, int>
    {
        public coadDAO _dao = new coadDAO();

        public coadSRV()
        {
            SetKeys("id");
            this.Dao = _dao;
        }
        
        public IList<coadDTO> BuscarPorAssinatura(string _assinatura)
        {
            return _dao.BuscarPorAssinatura(_assinatura);
        }
 
    }
}
