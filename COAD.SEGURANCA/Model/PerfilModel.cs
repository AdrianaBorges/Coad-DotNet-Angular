using COAD.SEGURANCA.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace COAD.SEGURANCA.Model
{
    [Mapping(Source = typeof(PERFIL))]
    public partial class PerfilModel
    {
        public PerfilModel()
        {
            this.ITEM_MENU_PERFIL = new HashSet<ItemMenuPerfilModel>();
            this.PERFIL_USUARIO = new HashSet<PerfilUsuarioModel>();
        }
    
        public int? EMP_ID { get; set; }
        
        [Required(ErrorMessage = "Perfil não informado.", AllowEmptyStrings = false)]
        public string PER_ID { get; set; }
        public Nullable<int> PER_ATIVO { get; set; }

        [DisplayFormat(DataFormatString = "{0:99:99}")]
        public string PER_HORA_INI { get; set; }
        
        [DisplayFormat(DataFormatString = "{0:99:99}")]
        public string PER_HORA_FIM { get; set; }

        public Nullable<int> PER_OUTROS_NIVEIS { get; set; }
        [Required(ErrorMessage = "Sistema não informado.", AllowEmptyStrings = false)]
        public string SIS_ID { get; set; }
       
        public Nullable<int> PER_BUSCA_REP { get; set; }
        public string PER_PATH_HOME { get; set; }
        public Nullable<int> PER_DASHBOARD { get; set; }
        public bool GERENCIA { get; set; }
        public Nullable<int> DP_ID { get; set; }

        [Required(ErrorMessage = "Nível de acesso não informado.")]
        public int? NIV_ACE_ID { get; set; }
      
        public virtual EmpresaModel EMPRESA { get; set; }
        public virtual ICollection<ItemMenuPerfilModel> ITEM_MENU_PERFIL { get; set; }
        public virtual SistemaModel SISTEMA { get; set; }
        public virtual ICollection<PerfilUsuarioModel> PERFIL_USUARIO { get; set; }
        public virtual DepartamentoDTO DEPARTAMENTO { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.DestinyToSource)]
        public virtual NivelAcessoDTO NIVEL_ACESSO { get; set; }
        
    }
}