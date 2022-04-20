using Coad.GenericCrud.Dao.Base;
using COAD.PORTAL.Model.DTO.Uras;
using COAD.PORTAL.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PORTAL.DAO.Uras
{
    public class coadDAO : AbstractGenericDao<coad, coadDTO, int>
    {
        public coadDAO()
        {
           SetProfileName("URARJ");
        }
        public IList<coadDTO> BuscarPorAssinatura(string _assinatura)
        {
            IQueryable<coad> query = GetDbSet();

            var _cli = query.Where(x => x.codigo == _assinatura).ToList();

            return ToDTO(_cli);
        }

    }
}
