using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using COAD.SEGURANCA.Repositorios.Contexto;

namespace COAD.SEGURANCA.Repositorios.Base
{
    public class Autenticado
    {
        
        [Required(ErrorMessage = "O login do usuário não foi informado !")]
        [Display(Name = "Informe o Login:")]
        public string USU_LOGIN { get; set; }

        [Required(ErrorMessage = "Senha do usuário não foi informada !")]
        [DataType(DataType.Password)]
        [Display(Name = "Informe a Senha:")]
        public string USU_SENHA { get; set; }
        public string USU_NOME { get; set; }
        public string SESSION_ID { get; set; }
        public int EMP_ID { get; set; }
        public bool EMP_GRP_COAD { get; set; }
        public string PATH { get; set; }
        public string IP_ACESSO { get; set; }
        public string PER_ID { get; set; }
        public bool ADMIN { get; set; }
        public string SIS_ID { get; set; }
        public int USU_NOVA_SENHA { get; set; }
        public Nullable <int> REP_ID { get; set; }
        public bool USU_ADMIN_LOGIN_PERFIL {get; set;}
        public string USU_CPF { get; set; }
        public int SESSION_TIMEOUT { get; set; }
        public int SESSION_TIMEOUT_RESTANTE { get; set; }
        public string EMAIL { get; set; }
        public string EMAIL_SENHA { get; set; }
        public DateTime DATA_LOGIN { get; set; }
        public string MEIO_ACESSO { get; set; }
        public int? LIN_PRO_ID { get; set; }
        public int? CLI_ID { get; set; }
        public string strSiteUrl { get; set; }

    }
}
