using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.CORPORATIVO.LEGADO.Model.Dto;
using COAD.CORPORATIVO.LEGADO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.LEGADO.Dao
{
    public class RepresentanteLegadoDAO : AbstractGenericDao<representante, RepresentanteLegadoDTO, string>
    {
        public corporativo2Entities db { get { return GetDb<corporativo2Entities>(); } set { } }

        public RepresentanteLegadoDAO()
        {
            SetProfileName("corp_old");
            db = GetDb<corporativo2Entities>();
        }

        public string GetCodOperadorRepresentante(string codRep)
        {
            var dbSet = GetDbSet();
            var codOper = (from rep in dbSet 
                         where rep.COD_REPR == codRep 
                         select rep.OPER)
                         .FirstOrDefault();

            return codOper;
        }

        public ICollection<string> BuscarCodigosCarteiramento(string codRep)
        {
            var dbSet = GetDbSet();

            if (string.IsNullOrWhiteSpace(codRep))
            {
                codRep = null;
            }

            var lstCarteiramento = (from rep in dbSet
                           where (codRep == null || (rep.REGIAO + rep.AREA + rep.COD_REPR).Contains(codRep)) &&
                           (rep.NOME != "VAGO")
                           select 
                               rep.REGIAO + 
                               rep.AREA + 
                               rep.COD_REPR)
                           .ToList();

            return lstCarteiramento;
        }

        public RepresentanteLegadoDTO BuscarPorCodigosCarteiramento(string carId)
        {
            var representante = (from rep in db.representante
                       where (rep.REGIAO + rep.AREA + rep.COD_REPR) == carId
                       select rep)
                       .FirstOrDefault();

            return ToDTO(representante);
        }
    }
}
