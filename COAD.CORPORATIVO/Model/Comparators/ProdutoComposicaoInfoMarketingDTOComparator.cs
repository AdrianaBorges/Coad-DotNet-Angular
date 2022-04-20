using COAD.CORPORATIVO.Model.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Comparators
{
    public class ProdutoComposicaoInfoMarketingDTOComparator : IEqualityComparer<ProdutoComposicaoInfoMarketingDTO>
    {

        public bool Equals(ProdutoComposicaoInfoMarketingDTO x, ProdutoComposicaoInfoMarketingDTO y)
        {
            if (x != null && y != null)
            {

                return (x.MKT_CLI_ID.Equals(y.MKT_CLI_ID) && 
                    x.CMP_ID.Equals(y.CMP_ID));
            }

            return false;
        }

        public int GetHashCode(ProdutoComposicaoInfoMarketingDTO obj)
        {
            var MKT_CLI_ID = (int) obj.MKT_CLI_ID;
            var CMP_ID = (int) obj.CMP_ID;

            return (MKT_CLI_ID.GetHashCode() + CMP_ID.GetHashCode());

        }
    }
}
