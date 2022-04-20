using Coad.GenericCrud.Config;
using Coad.GenericCrud.Service.Base;
using Coad.Reflection;
using GenericCrud.Dao;
using GenericCrud.Models;
using GenericCrud.Models.Filtros;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GenericCrud.Service.Base
{
    public class ServiceReflectionProvider<T, D, Id>
    {
        public ServiceReflectionProvider()
        {

        }

        public ServiceReflectionProvider(string profileName)
        {
            this.ProfileName = profileName;
        }

        public string ProfileName { get; set; }
        public IBaseService RetornarServicoDoTipo(Type type)
        {
            if (type != null && type.Assembly != null)
            {
                var serviceType = ReflectionProvider.GetServicesBy(type.Assembly, type);

                if(serviceType == null)
                {
                    throw new Exception(string.Format("Não é possível encontrar o serviço do tipo {0}", type.FullName));
                }

                var serviceInstance = ServiceFactory.RetornarServico<IBaseService>(serviceType);
                return serviceInstance;
            }

            return null;
        }

        public IList<FiltroSelectItemDTO> RetornarDadosDeCombo(Type type, string labelName, string valueName)
        {
            var service = RetornarServicoDoTipo(type);
            var lstCombo = service.GerarSelectItems(labelName, valueName);
            return lstCombo;
        }

        /// <summary>
        /// Retorna o mapeamento de filtros baseado no tipo da classe que descreve o mapemanto anotado com o attribute QueryFilterAttribute
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ICollection<FiltroGrupoDTO> GetFilters(Type type)
        {
            ICollection<FiltroGrupoDTO> filtroGrupoDTO = new HashSet<FiltroGrupoDTO>();

            var annotedProperties = ReflectionProvider.GetPropertyByAttributes<QueryFilterPropertyDescAttribute>(type);
            foreach (var pro in annotedProperties)
            {
                var filterDef = ReflectionProvider.GetMemberAttribute<QueryFilterPropertyDescAttribute>(pro);
                var groupInf = ReflectionProvider.GetMemberAttribute<QueryFilterAgrupamentoAttribute>(pro);
                var ordemFiltro = ReflectionProvider.GetMemberAttribute<QueryFilterOrdemAttribute>(pro);


                if (filterDef != null)
                {

                    string idGrupo = null;
                    string nomeGrupo = null;

                    if(groupInf != null && !string.IsNullOrWhiteSpace(groupInf.NomeGrupo))
                    {
                        idGrupo = groupInf.NomeGrupo;
                        nomeGrupo = groupInf.NomeGrupo;
                    }
                    else
                    {
                        idGrupo = "Comuns";
                        idGrupo = "Comuns";
                    }

                    var grupo = filtroGrupoDTO.Where(x => x.idGrupo == idGrupo).FirstOrDefault();
                    if(grupo == null)
                    {
                        grupo = new FiltroGrupoDTO() {

                            idGrupo = idGrupo,
                            nomeGrupo = nomeGrupo
                        };
                        filtroGrupoDTO.Add(grupo);
                    }

                    if(filterDef.GetTipoFiltro() == TipoFiltroEnum.Texto)
                    {
                        grupo.filtros.Add(new FiltroDTO() {

                            chave = pro.Name,
                            TipoEnum = filterDef.GetTipoFiltro(),
                            label = filterDef.Label,
                            ordem = (ordemFiltro != null) ? ordemFiltro.Ordem : 0,
                            size = filterDef.Size
                        });
                    }

                    if (filterDef.GetTipoFiltro() == TipoFiltroEnum.Select && filterDef is QueryFilterSelectAttribute)
                    {
                        var filterSelect = filterDef as QueryFilterSelectAttribute;
                        var targeType = filterSelect.Target;
                        var lstCombos = RetornarDadosDeCombo(targeType, filterSelect.labelName, filterSelect.valueName);

                        grupo.filtros.Add(new FiltroSelectDTO()
                        {
                            chave = pro.Name,
                            TipoEnum = filterDef.GetTipoFiltro(),
                            label = filterDef.Label,
                            ordem = (ordemFiltro != null) ? ordemFiltro.Ordem : 0,
                            size = filterDef.Size,
                            labelName = "label",
                            valueName = "value",
                            listCombo = lstCombos,
                        });
                    }

                    if (filterDef.GetTipoFiltro() == TipoFiltroEnum.Query && filterDef is QueryFilterQueryAttribute)
                    {
                        var filterQuery = filterDef as QueryFilterQueryAttribute;

                        grupo.queryFilter = new FiltroDTO()
                        {
                            chave = pro.Name,
                            TipoEnum = filterDef.GetTipoFiltro(),
                            label = filterDef.Label,
                            ordem = (ordemFiltro != null) ? ordemFiltro.Ordem : 0,
                            size = filterDef.Size
                        };
                    }
                }
            }
            return filtroGrupoDTO;
        }

        public ICollection<FiltroGrupoDTO> GetFilters(string name = null)
        {
            var config = ProfileConfigurator.getProfileConfig(ProfileName);
            if (config != null)
            {
                var dtoNamespace = config.ScanNameSpaces;
                var assembly = config.assembly;

                var targetNamespace = string.Format(@"{0}.{1}", dtoNamespace, "FiltersInfo");
                var lstTypes = ReflectionProvider.GetTypesAnnotedWith<QueryFilterAttribute>(assembly, targetNamespace);
                var filterMetadataType = GetFilterInfoInstance(lstTypes, assembly, name);
                if(filterMetadataType != null)
                {
                    var filtros = GetFilters(filterMetadataType);
                    return filtros;
                }
            }

            return null;
        }

        public Type GetFilterInfoInstance(IEnumerable<Type> lstTypes, Assembly assembly, string name = null)
        {
            if (lstTypes != null)
            {
                var type = lstTypes.Where(x =>
                    x.GetCustomAttribute<QueryFilterAttribute>() != null &&
                    x.GetCustomAttribute<QueryFilterAttribute>().DTOType == typeof(D) &&
                    (name == null || x.GetCustomAttribute<QueryFilterAttribute>().Name == name)
                    ).FirstOrDefault();

                return type;                   
            }

            return null;
        }
    }
}
