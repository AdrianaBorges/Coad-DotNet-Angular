using COAD.CORPORATIVO.Model.Dto.Prospect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Comparators
{
    public class ProspectsTelefoneDTOComparator : IEqualityComparer<ProspectsTelefoneDTO>
    {
        public bool Equals(ProspectsTelefoneDTO x, ProspectsTelefoneDTO y)
        {
            return x.PTEL_ID.Equals(y.PTEL_ID);
        }

        public int GetHashCode(ProspectsTelefoneDTO obj)
        {
            return obj.PTEL_ID.GetHashCode();
        }
    }
}
