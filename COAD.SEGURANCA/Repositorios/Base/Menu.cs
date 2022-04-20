using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COAD.SEGURANCA.Repositorios.Base
{
    public class Menu
    {
        public Menu()
        {
            this.MenuItens = new HashSet<Menu>();
        }
    
        public int MenuEmpId { get; set; }
        public string MenuPerid { get; set; }
        public string MenuUrl { get; set; }
        public string MenuValue { get; set; }
        public string MenuText { get; set; }
        public int MenuNivel { get; set; }
        public int MenuId { get; set; }
        public int MenuNodeId { get; set; }
        public int MenuOrden { get; set; }
        public int MenuNivAcesso { get; set; }
        public int MenuNivInsert { get; set; }
        public int MenuNivEdit { get; set; }
        public int MenuNivDelete { get; set; }
        public ICollection<Menu> MenuItens { get; set; }
 

    }
}
