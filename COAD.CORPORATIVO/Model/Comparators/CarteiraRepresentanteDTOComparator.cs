using COAD.CORPORATIVO.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Comparators
{
    public class CarteiraRepresentanteDTOComparator : IEqualityComparer<CarteiraRepresentanteDTO>
    {

        public bool Equals(CarteiraRepresentanteDTO x, CarteiraRepresentanteDTO y)
        {
            if (x == null)
                return false;
            if (y == null)
                return false;

            return (x.CAR_ID.Equals(y.CAR_ID) && x.REP_ID.Equals(y.REP_ID));
        }

        public int GetHashCode(CarteiraRepresentanteDTO obj)
        {
            int hashCode = 0;
            if (obj.REP_ID != null)
                hashCode = obj.REP_ID.GetHashCode();
            if(obj.CAR_ID != null)
                hashCode +=  obj.CAR_ID.GetHashCode();

            return hashCode;
        }
    }
}
