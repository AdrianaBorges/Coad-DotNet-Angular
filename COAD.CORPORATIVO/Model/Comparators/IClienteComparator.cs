using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Custons;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Comparators
{
    public class IClienteComparator : IEqualityComparer<ICliente>
    {
        public bool Equals(ICliente x, ICliente y)
        {
            if (x == null || y == null)
                return false;

            var lstCpfX = x.CNPJ_CPF;
            var lstCpfY = y.CNPJ_CPF;

            var lstTelefoneX = x.TELEFONE;
            var lstTelefoneY = y.TELEFONE;

            var lstFaxX = x.FAX;
            var lstFaxY = y.FAX;

            var lstCelularX = x.CELULAR;
            var lstCelularY = y.CELULAR;

            var lstEmailX = x.EMAIL;
            var lstEmailY = y.EMAIL;

            if (lstCpfX == null &&
                lstCpfY == null &&
                lstTelefoneX == null &&
                lstTelefoneY == null &&
                lstFaxX == null &&
                lstFaxY == null &&
                lstCelularX == null &&
                lstCelularY == null &&
                lstEmailX == null &&
                lstEmailY == null)
            {
                return false;
            }

            bool resultado = false;

            if (lstCpfX != null && lstCpfY != null)
            {
                var cpfCount = lstCpfX.Intersect(lstCpfY).Count();
                resultado = resultado || (cpfCount > 0);
            }

            if (lstTelefoneX != null && lstTelefoneY != null)
            {
                var telefoneCount = lstTelefoneX.Intersect(lstTelefoneY).Count();
                resultado = resultado || (telefoneCount > 0);            
            }

            if (lstFaxX != null && lstFaxY != null)
            {
                var faxCount = lstFaxX.Intersect(lstFaxY).Count();
                resultado = resultado || (faxCount > 0);
            
            }

            if (lstEmailX != null && lstEmailY != null)
            {
                var emailCount = lstEmailX.Intersect(lstEmailY).Count();
                resultado = resultado || (emailCount > 0);            
            }

            return resultado;
        }

        public int GetHashCode(ICliente obj)
        {
            int hashCode = 0;
            if (obj != null)
            {
                
                var lstCpf = obj.CNPJ_CPF;
                var lstTelefone = obj.TELEFONE;
                var lstFax = obj.FAX;
                var lstCelular = obj.CELULAR;
                var lstEmail = obj.EMAIL;

                if (lstCpf != null)
                {
                    hashCode += lstCpf.GetHashCode();
                }

                if (lstTelefone != null)
                {
                    hashCode += lstTelefone.GetHashCode();
                }

                if (lstFax != null)
                {
                    hashCode += lstFax.GetHashCode();
                }

                if (lstCelular != null)
                {
                    hashCode += lstCelular.GetHashCode();
                }

                if (lstEmail != null)
                {
                    hashCode += lstEmail.GetHashCode();
                }
            }
            return hashCode;
        }
    }
}
