using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Comparators
{
    public class CarteiraByEmpresaComparator : IEqualityComparer<CARTEIRA>
    {

        public bool Equals(CARTEIRA x, CARTEIRA y)
        {
            if ((x != null && x.EMP_ID != null) && (y != null && y.EMP_ID != null))
                return x.EMP_ID.Equals(y.EMP_ID);
            return false;
        }

        public int GetHashCode(CARTEIRA obj)
        {
            if (obj != null && obj.EMP_ID != null)
            {
                obj.EMP_ID.GetHashCode();
            }

            return 0;
        }
    }
}
