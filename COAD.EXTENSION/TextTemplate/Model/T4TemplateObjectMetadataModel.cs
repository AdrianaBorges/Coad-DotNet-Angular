using EnvDTE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.EXTENSION.TextTemplate.Model
{
    [Serializable]
    public class T4TemplateObjectMetadataModel
    {
        
        public T4TemplateObjectMetadataModel()
        {
            Usings = new HashSet<string>();
            PrimaryKeys = new HashSet<string>();
            Properties = new HashSet<T4TemplatePropertyMetadataModel>();
            GerarModel = true;
            GerarDAO = true;
            GerarService = true;
        }

        public bool PodeGerar
        {
            get
            {
                return (GerarDAO || GerarModel || GerarService);
            }
        }

        public bool GerarModel { get; set; }
        public bool GerarDAO { get; set; }
        public bool GerarService { get; set; }
        public bool GerarController { get; set; }
        public bool GerarViews { get; set; }

        public string ModelNamespace { get; set; }
        public string DAONamespace { get; set; }
        public string ServiceNamespace { get; set; }

        public string ProjectName { get; set; }
        public string DefaultNameSpace { get; set; }
        public string NameSpace { get; set; }
        public string ClassName { get; set; }
        public string RefClassName { get; set; }
        public string ProjectPath { get; set; }
        public string RefClassNamespace { get; set; }
        public string DbContextType { get {

                return string.Format("{0}Context", (!string.IsNullOrWhiteSpace(ProjectName) ? ProjectName : "Db"));
            } }
        public string PrimaryKeyType {
            get {

                if(PrimaryKeys != null)
                {
                    var keys = PrimaryKeys.Distinct();
                    var normalProperty = NormalProperties;

                    if(normalProperty != null)
                    {
                        normalProperty = normalProperty.Where(x => keys.Contains(x.Name)).ToList();
                        var type = normalProperty.Select(x => x.Type).Distinct().ToList();

                        if(type.Count() == 1)
                        {
                            return type[0].Split('.').Last();
                        }

                        return "object";
                    }
                }

                return "object";
            }
        }

        public ICollection<string> Usings { get; set; }
        public ICollection<string> PrimaryKeys { get; set; }
        public ICollection<T4TemplatePropertyMetadataModel> Properties { get; set; }

        public ICollection<T4TemplatePropertyMetadataModel> NormalProperties {
            get
            {
                if (Properties != null)
                    return Properties.Where(x => x.isEnum == false && x.TypeNamespace == "System").ToList();
                return null;
            }
        }

        public ICollection<T4TemplatePropertyMetadataModel> ObjectProperties
        {
            get
            {
                if (Properties != null)
                    return Properties.Where(x => x.isEnum == false && x.TypeNamespace != "System").ToList();
                return null;
            }
        }

        public ICollection<T4TemplatePropertyMetadataModel> CollectionProperties {
            get {
                if (Properties != null)
                    return Properties.Where(x => x.isEnum == true).ToList();
                return null;
            }
        }

        public ProjectItem projectItem { get; set; }
    }
}
