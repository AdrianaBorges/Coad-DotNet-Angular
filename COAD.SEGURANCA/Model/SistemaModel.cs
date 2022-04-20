using COAD.SEGURANCA.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.SEGURANCA.Model
{
    [Mapping(Source = typeof(SISTEMA))]
    public class SistemaModel
    {
        public SistemaModel()
        {
            this.ITEM_MENU = new HashSet<ItemMenuModel>();
            this.PERFIL = new HashSet<PerfilModel>();
        }
    
        public string SIS_ID { get; set; }
        public string SIS_DESCRICAO { get; set; }
        public string SIS_VERSAO { get; set; }
        public string SIS_URL { get; set; }
        public string SIS_URL_PRODUCAO { get; set; }
    
        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<ItemMenuModel> ITEM_MENU { get; set; }

        [IgnoreMemberMapping(Direction = MappingDirection.SourceToDestiny)]
        public virtual ICollection<PerfilModel> PERFIL { get; set; }
    }
}
