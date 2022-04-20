using COAD.CORPORATIVO.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Comparators
{
    public class AreaInfoMarketingDTOComparator : IEqualityComparer<AreaInfoMarketingDTO>
    {

        public bool Equals(AreaInfoMarketingDTO x, AreaInfoMarketingDTO y)
        {
            if (x != null && y != null)
            {

                return (x.MKT_CLI_ID.Equals(y.MKT_CLI_ID) && 
                    x.AREA_ID.Equals(y.AREA_ID));
            }

            return false;
        }

        public int GetHashCode(AreaInfoMarketingDTO obj)
        {
            var MKT_CLI_ID = (int) obj.MKT_CLI_ID;
            var AREA_ID = (int) obj.AREA_ID;

            return (MKT_CLI_ID.GetHashCode() + AREA_ID.GetHashCode());

        }
    }
}
