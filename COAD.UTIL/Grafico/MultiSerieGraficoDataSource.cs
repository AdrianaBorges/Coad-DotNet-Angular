using COAD.UTIL.Grafico.Base;
using COAD.UTIL.Grafico.Intarfaces;
using GenericCrud.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.UTIL.Grafico
{
    public class MultiSerieGraficoDataSource : GraficoDataSource
    {
        public IList<CategoriasDTO> categories { get; set; }
        public IList<DataSetDTO> dataset { get; set; }
        public int showValues { get; set; }

        public MultiSerieGraficoDataSource()
        {
            categories = new List<CategoriasDTO>();
            dataset = new List<DataSetDTO>();
        }

        public CategoriasDTO InsertCategories()
        {
            if (categories != null)
            {
                var cat = new CategoriasDTO();
                categories.Add(cat);

                return cat;
            }

            return null;
        }

        public void AddCategoriaItem(CategoriasDTO category, JsonGrafico categoryItem)
        {
            if (category != null && categoryItem != null && category.category != null)
            {
                category.category.Add(categoryItem);
            }
        }

        public void AddCategoriaItem(int index, JsonGrafico categoryItem)
        {
            if (categories != null && categoryItem != null && categories[index].category != null)
            {
                categories[index].category.Add(categoryItem);
            }
        }

        public DataSetDTO InsertDataSet()
        {
            if (dataset != null)
            {
                var set = new DataSetDTO();
                dataset.Add(set);

                return set;
            }

            return null;
        }

        public void AddDataSetItem(DataSetDTO dataset, JsonGrafico datasetItem)
        {
            if (dataset != null && datasetItem != null && dataset.data != null)
            {
                dataset.data.Add(datasetItem);
            }
        }

        //public void AddCategoriaItem(int index, JsonGrafico datasetItem)
        //{
        //    if (dataset != null && datasetItem != null && dataset[index].data != null)
        //    {
        //        dataset[index].data.Add(datasetItem);
        //    }
        //}

    }
}
