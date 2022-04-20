using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Model.Dto.Custons
{
    public enum TipoValidacao
    {
        ERRO = 0,
        WARNING = 1,
        INFO = 2
    }

    public class ValidacaoTypeResultDTO 
    {
        public bool Falhou { get; set; }
        public TipoValidacao Tipo { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{Falhou?: ");
            sb.Append(Falhou);
            sb.Append(", Tipo: ");
            sb.Append(Tipo.ToString());

            return sb.ToString();
        }
    }
}
