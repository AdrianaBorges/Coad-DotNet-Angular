using COAD.EXTENSION.TextTemplate.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.EXTENSION.Model
{
    public class ModelDialogDTO
    {
        public ModelDialogDTO()
        {
            Keys = new ObservableCollection<DataGridSelectKeyRow>();
            ColunasListagem = new ObservableCollection<DefinicaoColunasDTO>();
            ComboKeys = new HashSet<string>();
        }
        
        public string ClasseSelecionada { get; set; }
        public T4TemplateObjectMetadataModel Metadata { get; set; }
        public ObservableCollection<DataGridSelectKeyRow> Keys { get; set; }
        public ObservableCollection<DefinicaoColunasDTO> ColunasListagem { get; set; }
        
        public ICollection<string> ComboKeys { get; set; }
        public ICollection<string> Projetos { get; set; }

        public string SelectedProject { get; set; }

        public ICollection<string> SelectedKey
        { get
            {
                if(Keys != null)
                {
                    var selectedKeys = Keys
                       .Where(x =>
                           !string.IsNullOrWhiteSpace(x.Key)
                       )
                       .OrderBy(or => or.Index)
                       .Select(x => x.Key).ToList();

                    return selectedKeys;
                }
                return new HashSet<string>();
            }
        }

        public ICollection<string> ComboKeysToUse
        {
            get
            {
                if (ComboKeys != null)
                {
                    if(SelectedKey != null && SelectedKey.Count > 0)
                    {
                        var comboToUse = ComboKeys.Where(x => !SelectedKey.Contains(x));
                        return comboToUse.ToList();
                    }
                    else
                    return ComboKeys;
                }
                return new HashSet<string>();
            }
        }
    }
}
