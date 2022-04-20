using COAD.SEGURANCA.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.SEGURANCA.Model
{
    [Mapping(Source = typeof(NIVEL_ACESSO))]
    public class NivelAcessoDTO
    {
        public NivelAcessoDTO()
        {
            this.PERFIL = new HashSet<PerfilModel>();
        }
    
        public int? NIV_ACE_ID { get; set; }
        public string NIV_ACE_DESCRICAO { get; set; }
        public Nullable<int> NIV_ACE_NIVEL { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<PerfilModel> PERFIL { get; set; }
    }
}