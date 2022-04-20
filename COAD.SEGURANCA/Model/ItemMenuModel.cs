using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using COAD.SEGURANCA.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace COAD.SEGURANCA.Model
{
    public class ItemMenuModel
    {
        public ItemMenuModel()
        {
            this.ITEM_MENU1 = new HashSet<ItemMenuModel>();
            this.ITEM_MENU_PERFIL = new HashSet<ItemMenuPerfilModel>();
        }

        [Required(ErrorMessage = "O campo Código deve ser informado")]
        public Nullable<int> ITM_ID { get; set; }

        [Required(ErrorMessage = "O campo descrição deve ser informado", AllowEmptyStrings = false)]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "O campo descrição deve possuir no maximo 50 caracteres.")]
        public string ITM_DESCRICAO { get; set; }
                
        public string ITM_ARQUIVO { get; set; }
        
        public Nullable<int> ITM_NODE_ID { get; set; }
        
        [Required(ErrorMessage = "O campo ordem deve ser informado.", AllowEmptyStrings = false)]
        [Range(0, 99999, ErrorMessage = "O campo ordem deve estar entre  e 99999")]
        public Nullable<int> ITM_MENU_SEQ { get; set; }

        [Required(ErrorMessage = "O campo nível deve ser informado", AllowEmptyStrings = false)]
        public Nullable<int> ITM_MENU_NIVEL { get; set; }

        [Required(ErrorMessage = "O campo tipo deve ser informado", AllowEmptyStrings = false)]
        public Nullable<int> ITM_TIPO { get; set; }

        public Nullable<int> ITM_EXTERNO { get; set; }

        [Required(ErrorMessage = "O campo situação deve ser informado.", AllowEmptyStrings = false)]
        public Nullable<int> ITM_ATIVO { get; set; }
        public string ITM_RETORNO { get; set; }

        [Required(ErrorMessage = "O campo Caminho (PATH) deve ser informado.", AllowEmptyStrings = false)]
        public string ITM_NOME_ARQUIVO { get; set; }

        [Required(ErrorMessage = "O campo sistema deve ser informado.", AllowEmptyStrings = false)]
        public string SIS_ID { get; set; }

        /// <summary>
        /// Essa propriedade não existe no banco
        /// </summary>
        public bool possuiSubMenus { get; set; }

        public virtual ICollection<ItemMenuModel> ITEM_MENU1 { get; set; }
        public virtual ItemMenuModel ITEM_MENU2 { get; set; }
        public virtual ICollection<ItemMenuPerfilModel> ITEM_MENU_PERFIL { get; set; }
        public virtual SistemaModel SISTEMA { get; set; }

    }
}
