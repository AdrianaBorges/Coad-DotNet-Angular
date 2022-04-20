using COAD.SEGURANCA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Comparators
{
    public class PerfilUsuarioComparator : IEqualityComparer<PerfilUsuarioModel>
    {

        public bool Equals(PerfilUsuarioModel x, PerfilUsuarioModel y)
        {
            if ((x != null && x.EMP_ID != null && string.IsNullOrWhiteSpace(x.PER_ID) && string.IsNullOrWhiteSpace(x.USU_LOGIN))
                && (y != null && y.EMP_ID != null && string.IsNullOrWhiteSpace(y.PER_ID) && string.IsNullOrWhiteSpace(y.USU_LOGIN)))
                return (x.EMP_ID.Equals(y.EMP_ID) && x.PER_ID.Equals(y.PER_ID) && x.USU_LOGIN.Equals(y.USU_LOGIN));
            return false;
        }

        public int GetHashCode(PerfilUsuarioModel obj)
        {
            int hash = 0;
            if (obj != null)
            {
                hash += (obj.EMP_ID != null) ? obj.EMP_ID.GetHashCode() : 0;
                hash += (obj.PER_ID != null) ? obj.PER_ID.GetHashCode() : 0;
                hash += (obj.USU_LOGIN != null) ? obj.USU_LOGIN.GetHashCode() : 0;

                return hash;
            }

            return 0;
        }
    }
}
