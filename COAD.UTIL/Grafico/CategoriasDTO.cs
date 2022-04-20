using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.UTIL.Grafico
{
    public class CategoriasDTO
    {
        public IList<JsonGrafico> category { get; set; }

        public CategoriasDTO()
        {
            category = new List<JsonGrafico>();
        }

        public void AddCategoryItem(JsonGrafico categoryItem)
        {
            if (category != null)
            {
                category.Add(categoryItem);
            }
        }

        public void AddCategoryList(IList<JsonGrafico> categoryItem)
        {
            if (category != null)
            {
                category = categoryItem;
            }
        }

    }

}
