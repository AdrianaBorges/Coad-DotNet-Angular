using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.EXTENSION.TextTemplate.Model
{
    [Serializable]
    public class T4TemplatePropertyMetadataModel
    {
        public string Type { get; set; }
        public string TypeNamespace { get; set; }
        public string Name { get; set; }
        public bool isEnum { get; set; }

        public string ConcreteType {
            get {

                if (!string.IsNullOrWhiteSpace(Type))
                {
                    if (Type.Contains("IList"))
                    {
                        var concreteType = Type.Replace("IList", "List");
                        return concreteType;
                    }

                    if (Type.Contains("ICollection"))
                    {
                        var concreteType = Type.Replace("ICollection", "HashSet");
                        return concreteType;
                    }
                    if (Type.Contains("IEnumerable"))
                    {
                        var concreteType = Type.Replace("IEnumerable", "HashSet");
                        return concreteType;
                    }
                    if (Type.Contains("ISet"))
                    {
                        var concreteType = Type.Replace("ISet", "HashSet");
                        return concreteType;
                    }

                }
                return null;
            }
        }
    }
}
