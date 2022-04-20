using COAD.EXTENSION.TextTemplate;
using COAD.EXTENSION.TextTemplate.Model;
using COAD.EXTENSION.Util;
using EnvDTE;
using Microsoft.VisualStudio.TextTemplating;
using Microsoft.VisualStudio.TextTemplating.VSHost;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace COAD.EXTENSION.Service
{
    public class ProjectSRV
    {
        public DTE dte { get; set; }
        public T4TemplateCompiler compiler { get; set; }

        public SelectedItems ListSelectedItens()
        {

            if(dte != null)
            {
                var selectedItems = dte.SelectedItems;
                return selectedItems;
            }
            else
            {
                throw new Exception("O objto DTE não pode ser encontrado.");
            }
        }

        public ICollection<string> GetProjectsName()
        {
            var projects = dte.Solution.Projects;
            ICollection<string> projectsName = new HashSet<string>();
            if (projects != null && projects.Count > 0)
            {
                foreach(Project project in projects)
                {
                    if(project.Name != "ProjectItems")
                    {
                        projectsName.Add(project.Name);
                    }
                }
            }

            return projectsName;
        }

        public ICollection<T4TemplateObjectMetadataModel> CreateMetatadaFromSelectedItems()
        {
            ICollection<T4TemplateObjectMetadataModel> lstMetadatas = new HashSet<T4TemplateObjectMetadataModel>();
            var selectedItems = ListSelectedItens();
            
            int count = dte.SelectedItems.Count;
            
            StringBuilder sb = new StringBuilder();
            foreach (SelectedItem selectedItem in dte.SelectedItems)
            {
                if (selectedItem.ProjectItem == null) return null;
                var projectItem = selectedItem.ProjectItem;

               var metadata = GenerateDTOMetadata(projectItem);
                lstMetadatas.Add(metadata);
                //CreateClassFromMetadata(metadata);
            }
            return lstMetadatas;
        }
        
        public string ProcessTemplate(string templateName, ITextTemplatingCallback cb)
        {
            var filePath = string.Format(@"C:\COAD.EXTENSIONS\TextTemplate\{0}", templateName);

            if (!File.Exists(filePath))
            {
                var codeBase = GetType().Assembly.CodeBase;
                var uri = new Uri(codeBase, UriKind.Absolute);
                var installPath = uri.LocalPath;
                installPath = Path.GetDirectoryName(installPath);

                var fileOriginalPath = string.Format(@"{0}\TextTemplate\{1}", installPath, templateName);

                var path = @"C:\COAD.EXTENSIONS\TextTemplate\";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                File.Copy(fileOriginalPath, filePath, true);
            }

            var content = File.ReadAllText(filePath);
            var processedContent = compiler.T4.ProcessTemplate(@"", content, cb);
            return processedContent;

        }

        public void CreateClassFromMetadata(T4TemplateObjectMetadataModel metadata)
        {
            string modelPath = "Models";
            if(!string.IsNullOrWhiteSpace(metadata.DefaultNameSpace) &&
                !string.IsNullOrWhiteSpace(metadata.NameSpace))
            {
                modelPath = metadata.NameSpace.Replace(metadata.DefaultNameSpace, "").Replace(".", "/");
            }
            compiler.sessionHost.Session = compiler.sessionHost.CreateSession();
            compiler.sessionHost.Session["metadata"] = metadata;

            if (metadata.GerarModel)
            {
                T4CallBack cbDTO = new T4CallBack();

                var dtoOutput = ProcessTemplate(@"GenericCrudDTO.tt", cbDTO);

                var modelNamespace = metadata.ModelNamespace;
                if (!string.IsNullOrWhiteSpace(modelNamespace))
                {
                    modelNamespace = modelNamespace.Replace(metadata.DefaultNameSpace, "").Replace(".", "/");
                    WriteClass(metadata, dtoOutput, modelNamespace, metadata.projectItem, "DTO", cb: cbDTO);
                }
                else
                {
                    throw new Exception("Informe o NameSpace do Model");
                }
            }

            if (metadata.GerarDAO)
            {
                T4CallBack cbDAO = new T4CallBack();
                var daoOutput = ProcessTemplate(@"GenericCrudDAO.tt", cbDAO);

                var daoNamespace = metadata.DAONamespace;
                if (!string.IsNullOrWhiteSpace(daoNamespace))
                {
                    daoNamespace = daoNamespace.Replace(metadata.DefaultNameSpace, "").Replace(".", "/");
                    WriteClass(metadata, daoOutput, daoNamespace, metadata.projectItem, "DAO", cb: cbDAO);
                }
                else
                {
                    throw new Exception("Informe o NameSpace do DAO");
                }
            }

            if (metadata.GerarService)
            {
                T4CallBack cbSRV = new T4CallBack();
                var srvOutput = ProcessTemplate(@"GenericCrudSRV.tt", cbSRV);

                var serviceNamespace = metadata.ServiceNamespace;
                if (!string.IsNullOrWhiteSpace(serviceNamespace))
                {
                    serviceNamespace = serviceNamespace.Replace(metadata.DefaultNameSpace, "").Replace(".", "/");
                    WriteClass(metadata, srvOutput, serviceNamespace, metadata.projectItem, "SRV", cb: cbSRV);
                }
                else
                {
                    throw new Exception("Informe o NameSpace do Service");
                }

            }

            T4CallBack cbDbContext = new T4CallBack();
            var dbContextOutput = ProcessTemplate(@"GenericCrudDefaultDbContext.tt", cbDbContext);

            WriteClass(new T4TemplateObjectMetadataModel() {
                ClassName = metadata.ProjectName,
                DefaultNameSpace = metadata.DefaultNameSpace
            }, 
            dbContextOutput, "Repositorios/Base", metadata.projectItem, "Context", false, cbDbContext);


        }

        public void WriteClass(T4TemplateObjectMetadataModel metatada, string classContent, string toPath, ProjectItem projectItem, string sufix = "DTO", bool createAnother = true, T4CallBack cb = null)
        {
            var folderProjectItem = CheckCreateFolder(toPath, projectItem.ContainingProject);
            var filePath = folderProjectItem.Properties.Item("FullPath").Value.ToString();

            var path = string.Format(@"{0}{1}{2}[count].cs", filePath, metatada.ClassName, sufix);
            var cleanPath = path.Replace("[count]", "");

            if (File.Exists(cleanPath))
            {
                if (createAnother == false)
                    return;
                path = path.Replace("[count]", " (" + 1 + ")");
            }
            else
            {
                path = cleanPath;
            }

            File.WriteAllText(path, classContent);

            if (cb != null && cb.errorMessages.Count > 0)
            {
                File.AppendAllLines(path, cb.errorMessages);
            }

            folderProjectItem.ProjectItems.AddFromFile(path);
        }

        public void WriteDAO()
        {

        }

        public void WriteSRV()
        {

        }


        public T4TemplateObjectMetadataModel GenerateDTOMetadata(ProjectItem projectItem)
        {
            if (projectItem != null && projectItem.FileCodeModel != null)
            {
                var elements = projectItem.FileCodeModel.CodeElements;
                string assemblyName = null;
                string fullName = null;
                
                foreach (CodeElement code in elements)
                {
                    if (code.InfoLocation == vsCMInfoLocation.vsCMInfoLocationProject)
                    {
                        var assembly = code.ProjectItem.ContainingProject.Properties.Item("AssemblyName");
                        if (assembly != null)
                            assemblyName = assembly.Value.ToString();
                        break;
                    }
                    if (code.InfoLocation == vsCMInfoLocation.vsCMInfoLocationExternal)
                    {
                        dynamic exloc = code.Extender["ExternalLocation"];
                        var assembly = exloc.ExternalLocation;
                        if(assembly != null)
                        {
                            //assemblyName = assembly
                        }
                    }
                }
                //if (!string.IsNullOrWhiteSpace(assemblyName))
                //{
                //    var assembly = Assembly.Load(assemblyName);
                //}

                foreach (CodeElement code in elements)
                {
                    if (code.Kind == vsCMElement.vsCMElementNamespace)
                    {
                        var elementName = code as CodeNamespace;
                        var members = elementName.Members;
                        

                        foreach(var property in members)
                        {
                            var member = property as CodeType;
                            if (member == null)
                                continue;

                            foreach(var d in member.Bases)
                            {
                                var dClass = d as CodeClass;
                                var objMetadata = ReturnClassMetadata(dClass, member);
                                if (objMetadata != null)
                                    objMetadata.projectItem = projectItem;
                                return objMetadata;
                            }
                        }
                    }
                }
            }

            return null;
        }

        public T4TemplateObjectMetadataModel ReturnClassMetadata(CodeClass codeClass, CodeType memberClass)
        {

            if (codeClass == null)
                return null;
            
            T4TemplateObjectMetadataModel objectMetadata = new T4TemplateObjectMetadataModel();
            var projectNamespace = memberClass.ProjectItem.ContainingProject.Properties.Item("RootNamespace").Value.ToString();
            var projectPath = memberClass.ProjectItem.ContainingProject.Properties.Item("FullPath").Value.ToString();
            objectMetadata.ProjectName = 
                (!string.IsNullOrWhiteSpace(memberClass.ProjectItem.ContainingProject.Name)) ?
                memberClass.ProjectItem.ContainingProject.Name.Split('.').Last() 
                : "Project";

            objectMetadata.ProjectPath = projectPath;
            objectMetadata.NameSpace = string.Format(@"{0}.Models.DTO", projectNamespace);
            objectMetadata.DefaultNameSpace = projectNamespace;
            objectMetadata.RefClassName = memberClass.Name;
            objectMetadata.RefClassNamespace = memberClass.Namespace.FullName;
            objectMetadata.ClassName = TypeUtil.TransformToDTOName(memberClass.Name, false);
            AddUsings(memberClass.FullName, objectMetadata);


            foreach (CodeElement pro in memberClass.Members)
            {
                if (pro is CodeProperty)
                {
                    var propertyInfo = pro as CodeProperty;
                    var propertyName = propertyInfo.Name;
                    var propertyTypeName = propertyInfo.Type.AsFullName;
                    var isEnum = TypeUtil.IsEnumerable(propertyTypeName);
                    var isGenerics = TypeUtil.IsGenerics(propertyTypeName);
                    var rgx = new Regex(@"(.*)<(.*)>");

                    string propertyTypeNamespace = null;

                    if (isGenerics)
                    {
                        propertyTypeName = TransformGenericsToQuivalentDTO(propertyTypeName, objectMetadata.RefClassNamespace, objectMetadata.NameSpace);
                        if (!string.IsNullOrWhiteSpace(propertyTypeName) && rgx.IsMatch(propertyTypeName))
                        {
                            var matches = rgx.Matches(propertyTypeName);
                            foreach(Match match in matches)
                            {
                                var propertyTypeFullName = match.Groups[1].Value;
                                var genericTypeFullName = match.Groups[2].Value;

                                //propertyTypeFullName = new Regex(@"\<(.*)\>").Replace(propertyTypeFullName, "");
                                var propertyTypeShortName = propertyTypeFullName.Split('.').Last();
                                var genericTypeName = genericTypeFullName.Split('.').Last();

                                propertyTypeName = propertyTypeShortName + "<" + genericTypeName + ">";
                                propertyTypeNamespace = propertyTypeFullName.Replace("." + propertyTypeShortName, "");
                                AddUsings(propertyTypeFullName, objectMetadata);
                                AddUsings(genericTypeFullName, objectMetadata);
                            }
                        }
                    }
                    else
                    {
                        AddUsings(propertyTypeName, objectMetadata);
                        var auxPropertyTypeName = propertyTypeName.Split('.').Last();
                        propertyTypeNamespace = propertyTypeName.Replace("." + auxPropertyTypeName, "");
                        propertyTypeName = auxPropertyTypeName;

                        if(propertyTypeNamespace != "System")
                        {
                            propertyTypeName = TypeUtil.TransformToDTOName(propertyTypeName);
                        }
                    }
                    
                    objectMetadata.Properties.Add(new T4TemplatePropertyMetadataModel() {

                        Name = propertyName,
                        Type = propertyTypeName,
                        isEnum = isEnum,
                        TypeNamespace = propertyTypeNamespace
                    });
                }

            }
            return objectMetadata;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyType"></param>
        /// <param name="objMetada"></param>
        private void AddUsings(string propertyType, T4TemplateObjectMetadataModel objMetada)
        {
            if (!string.IsNullOrEmpty(propertyType))
            {
                var typeName = propertyType.Split('.').Last();
                var usin = propertyType.Replace("." + typeName, "");

                if(objMetada.Usings.Where(x => x == usin).Count() == 0)
                {
                    objMetada.Usings.Add(usin);
                }
            }
        }

        public string TransformGenericsToQuivalentDTO(string typeName, string modelNamespace, string dtoNamespace)
        {
            var genericTypeFullName = new Regex("(.*<(.*)>)").Replace(typeName, "$2");
            if (string.IsNullOrWhiteSpace(genericTypeFullName))
                return null;
            var genericTypeName = genericTypeFullName.Split('.').Last();
            var genericTypeNamespace = genericTypeFullName.Replace("." + genericTypeName, "");

            if(modelNamespace == genericTypeNamespace)
            {
                var dtoEquivalentName = TypeUtil.TransformToDTOName(genericTypeName);
                string fullDTOEqvName = string.Format(@"{0}.{1}", dtoNamespace, dtoEquivalentName);

                var newTypeName = new Regex(@"\<(.*)\>").Replace(typeName, "<" + fullDTOEqvName + ">");
                return newTypeName;
            }
            return typeName;
        }

        public ProjectItem RecursiveFindProjectItem(ProjectItems projectItems, string folderName)
        {
            if (!string.IsNullOrWhiteSpace(folderName))
            {
                foreach(ProjectItem projectItem in projectItems)
                {

                    if (projectItem.Kind == Constants.vsProjectItemKindPhysicalFolder)
                    {
                        if (folderName == projectItem.Name)
                        {
                            return projectItem;
                        }
                        else
                        {
                            if(projectItem.ProjectItems != null)
                            {
                                var subProjectItem = RecursiveFindProjectItem(projectItem.ProjectItems, folderName);
                                if (subProjectItem != null)
                                    return subProjectItem;
                            }
                        }
                    }
                }
                return null;
            }
            return null;
        }        

        public ProjectItem CheckCreateFolder(string path, Project project)
        {
            if (string.IsNullOrWhiteSpace(path))
                return null;

            var folders = path.Split('/');
            ProjectItem parentProjectItem = null;
            ProjectItems parentProjectItems = null;
            parentProjectItems = project.ProjectItems;

            foreach (var folder in folders)
            {
                if (!string.IsNullOrWhiteSpace(folder))
                {
                    var projectItem = RecursiveFindProjectItem(parentProjectItems, folder);
                    if(projectItem != null)
                    {
                        parentProjectItem = projectItem;                    
                    }
                    else
                    {
                        if(parentProjectItem != null)
                        {
                            parentProjectItem = parentProjectItem.ProjectItems.AddFolder(folder);
                        }
                        else
                        {
                            parentProjectItem = project.ProjectItems.AddFolder(folder);
                        }
                    }
                    parentProjectItems = parentProjectItem.ProjectItems;                
                }
            }
            return parentProjectItem;
        }

        public IList<string> ListarCampos(T4TemplateObjectMetadataModel metadata)
        {
            IList<string> lstProperties = new List<string>();

            if (metadata != null && 
                (metadata.NormalProperties != null || metadata.ObjectProperties != null))
            {

                if(metadata.NormalProperties != null)
                    lstProperties = lstProperties.Concat(metadata.NormalProperties.Select(x => x.Name)).ToList();

                if(metadata.ObjectProperties != null)
                    lstProperties = lstProperties.Concat(metadata.ObjectProperties.Select(x => x.Name)).ToList();
            }

            return lstProperties;
        }
    }
}
