using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace COAD.SEGURANCA.Model
{

    public partial class ParametrosDTO
    {
        public int PAR_ID { get; set; }
        public string PAR_KEY { get; set; }
        public string PAR_VALOR { get; set; }
        public string USU_LOGIN { get; set; }
        public Nullable<System.DateTime> PAR_DATA_ALTERA { get; set; }
        public Nullable<int> PGR_ID { get; set; }
        public string PAR_TIPO { get; set; }
        public string PAR_VALOR_PADRAO { get; set; }
        public string PAR_KEY_DESCRICAO { get; set; }

        public virtual ParametroGrupoDTO PARAMETRO_GRUPO { get; set; }
        public virtual UsuarioModel USUARIO { get; set; }
    }
}