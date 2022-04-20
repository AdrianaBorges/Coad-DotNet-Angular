using Coad.GenericCrud.Validations;
using COAD.SEGURANCA.Constants;
using COAD.SEGURANCA.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using GenericCrud.Validations.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.SEGURANCA.Model
{
    [Mapping(Source = typeof(USUARIO))]
    public class UsuarioModel
    {
        public UsuarioModel()
        {
            //this.LOG_OCORRENCIA = new HashSet<LogOcorrenciasModel>();
            this.PERFIL_USUARIO = new HashSet<PerfilUsuarioModel>();
            this.PARAMETROS = new HashSet<ParametrosDTO>();
            this.USUARIO1 = new HashSet<UsuarioModel>();
            this.JOB_NOTIFICACAO = new HashSet<JobNotificacaoDTO>();
        }
    
        [Required(ErrorMessage = "Digite o login do Usuário")]
        [MaxLength(15,ErrorMessage = "O login do usuário deve conter no máximo 15 caracteres")]
        public string USU_LOGIN { get; set; }
        public System.DateTime USU_DATA_CAD { get; set; }

        [RequiredIfNullOrEmpty("CADASTRADO_POR", ErrorMessage = "Selecine se o usuário é ou não um Ativo.")] 
        public int? USU_ATIVO { get; set; }

        public Nullable<System.DateTime> USU_DATA_EXPIRA { get; set; }
        public Nullable<System.DateTime> USU_DATA_ULTIMOACESSO { get; set; }
        public Nullable<System.DateTime> USU_DATA_LOGIN { get; set; }

        [Required(ErrorMessage = "Digite o nome do Usuário")]
        public string USU_NOME { get; set; }

        [IgnoreMemberMapping(MappingDirection.SourceToDestiny)]
        public string USU_SENHA { get; set; }

        public string USU_EMAIL_SENHA { get; set; }

        [RequiredIfNullOrEmpty("CADASTRADO_POR", ErrorMessage = "Selecine se o usuário é ou não um ADMIN.")] 
        public Nullable<int> USU_ADMIN { get; set; }

        [Required(ErrorMessage = "Digite o email.")]
        [EmailAddress(ErrorMessage = "Digite um email válido.")]
        public string USU_EMAIL { get; set; }

        public Nullable<int> USU_NOVA_SENHA { get; set; }

        [RequiredIf("editando", false, NullOrEmptyPropertyName = "CADASTRADO_POR", ErrorMessage = "Selecione a empresa.")]       
        public int? EMP_ID { get; set; }

        [MaxLength(14, ErrorMessage= "O cpf deve conter no máximo 14 caracteres")]
        public string USU_CPF { get; set; }
        public bool USU_ADMIN_LOGIN_PERFIL { get; set; }    
        public Nullable<int> REP_ID { get; set; }

        /// <summary>
        /// Usado apenas para indicar que esse usuário foi cadastrado por outro usuário
        /// que necessáriamente precisa ter o acesso de admin de perfis.
        /// Para usuários cadastrados por usuários admin (normal) não deverá ter o campo preenchido.
        /// </summary>
        public string CADASTRADO_POR { get; set; }

        /// <summary>
        /// Esse atributo não existe no banco. 
        /// Serve para indicar que perfil o usuário irá receber na 
        /// manutenção de cadastro de representantes 
        /// </summary>
        //public string PER_ID_A_ATRIBUIR { get; set; }

        [IgnoreMemberMapping(MappingDirection.Both)]
        public virtual EmpresaModel EMPRESA { get; set; }
        
//        [IgnoreMemberMapping(MappingDirection.Both)]
//        public virtual ICollection<LogOcorrenciasModel> LOG_OCORRENCIA { get; set; }
        
        [IgnoreMemberMapping(MappingDirection.Both)]
        public virtual ICollection<PerfilUsuarioModel> PERFIL_USUARIO { get; set; }

        public bool edit = false;
        public bool editando { get { return edit; } set { edit = value; } }
       
        /// <summary>
        /// Usuários cadastrados por esse usuário
        /// </summary>
        /// 
        [IgnoreMemberMapping(MappingDirection.SourceToDestiny)]
        public virtual ICollection<UsuarioModel> USUARIO1 { get; set; }

        /// <summary>
        /// Usuário que cadastrou esse usuário
        /// </summary> 
        [IgnoreMemberMapping(MappingDirection.SourceToDestiny)]
        public virtual UsuarioModel USUARIO2 { get; set; }

        [IgnoreMemberMapping(MappingDirection.SourceToDestiny)]
        public virtual ICollection<ParametrosDTO> PARAMETROS { get; set; }

        [IgnoreMemberMapping(MappingDirection.Both)]
        public virtual ICollection<JobNotificacaoDTO> JOB_NOTIFICACAO { get; set; }

    }
}


