using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace COAD.SEGURANCA.Model
{
    public class ItemMenuPerfilModel
    {
        public int? EMP_ID { get; set; }
        public string PER_ID { get; set; }
        public int? ITM_ID { get; set; }
        public Nullable<int> NIV_ACESSO { get; set; }
        public Nullable<int> NIV_INSERT { get; set; }
        public Nullable<int> NIV_EDIT { get; set; }
        public Nullable<int> NIV_DELETE { get; set; }

        public virtual ItemMenuModel ITEM_MENU { get; set; }
        public virtual ICollection<ItemMenuPerfilModel> SUB_ITEM_MENU { get; set; }
        public virtual PerfilModel PERFIL { get; set; }
    }
}