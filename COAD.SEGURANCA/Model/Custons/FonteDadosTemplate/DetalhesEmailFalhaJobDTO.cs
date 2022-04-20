using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.SEGURANCA.Model.Custons.FonteDadosTemplate
{
    public class DetalhesEmailFalhaJobDTO
    {
        public string DescricaoCodigoReferencia { get; set; }
        public string CodReferencia { get; set; }
        public string CodReferenciaHash { get; set; }
        public string Ambiente { get; set; }
        public string HostName { get; set; }
        public string NomeDoJob { get; set; }
        public string ErroDescricao { get; set; }
        public string StackTrace { get; set; }
        public DateTime? DataErro { get; set; }
        public string HoraErro {
            get {
                if (DataErro != null)
                    return string.Format("{0:hh:mm:ss}", DataErro);
                    return null;
            }
        }
    }
}
