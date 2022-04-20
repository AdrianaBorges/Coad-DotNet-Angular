using COAD.CORPORATIVO.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Comparators
{
    public class RegiaoInfoMarketingDTOComparator : IEqualityComparer<RegiaoTabelaPrecoDTO>
    {

        public bool Equals(RegiaoTabelaPrecoDTO x, RegiaoTabelaPrecoDTO y)
        {
            
            return false;
        }

        public int GetHashCode(RegiaoTabelaPrecoDTO obj)
        {
            return 0;
        }
    }
}
