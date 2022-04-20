using COAD.SEGURANCA.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.SEGURANCA.Model
{
    [Mapping(Source = typeof(DEPARTAMENTO))]
    public class DepartamentoDTO    
    {
        public DepartamentoDTO()
        {
            this.PERFIL = new HashSet<PerfilModel>();
        }
    
        public int? DP_ID { get; set; }

        [Required(ErrorMessage = "Digite o nome do departamento.")]
        public string DP_NOME { get; set; }

        public Nullable<System.DateTime> DATA_DESATIVACAO { get; set; }    

        [IgnoreMemberMapping(MappingDirection.SourceToDestiny)]
        public virtual ICollection<PerfilModel> PERFIL { get; set; }
    }
}
