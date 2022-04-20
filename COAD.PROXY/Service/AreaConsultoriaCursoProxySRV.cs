using Coad.Reflection;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Service;
using COAD.PROXY.Model.DTO;
using GenericCrud.Config.DataAttributes.Maps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PROXY.Service
{
    public class AreaConsultoriaCursoProxySRV : AreaConsultoriaCursoSRV
    {
        [ServiceProperty("ARE_CONS_ID", Name = "colecionador", PropertyName = "COLECIONADOR", FindById = true)]
        protected ColecionadorProxySRV _colecionadorSRV = new ColecionadorProxySRV();

        [ServiceProperty("TIT_ID", Name = "grandeGrupo", PropertyName = "GRANDE_GRUPO", FindById = true)]
        protected GrandeGrupoProxySRV _grandeGrupoSRV = new GrandeGrupoProxySRV();

        public void PreencherAreaConsultoriaCurso(CursoProxyDTO curso)
        {
            if (curso != null && curso.CMP_ID != null)
            {
                var cmp = curso.CMP_ID;

                if (curso.AREA_CONSULTORIA_CURSO != null)
                {
                    foreach (var areaCur in curso.AREA_CONSULTORIA_CURSO)
                    {
                        areaCur.PRODUTO_COMPOSICAO = null;
                    }

                    var lstAreaConsultoriaRepresentante = GetProxyTools<AreaConsultoriaCursoProxyDTO>().ConvertToProxy(curso.AREA_CONSULTORIA_CURSO);
                    curso.AREA_CONSULTORIA_CURSO = null;
                    GetAssociations(lstAreaConsultoriaRepresentante, "colecionador", "grandeGrupo");
                    
                    curso.AREA_CONSULTORIA_CURSO_PROXY = lstAreaConsultoriaRepresentante;
                }
                
            }
        }

        public void ExcluirAreaConsultoriaRepresentante(CursoProxyDTO curso)
        {
            var CMP_ID = (int)curso.CMP_ID;
            var cursoDoBanco = new CursoProxySRV().FindByIdFullLoad(CMP_ID);

            ExcluirList<CursoProxyDTO, AreaConsultoriaCursoProxyDTO>(curso, cursoDoBanco, "AREA_CONSULTORIA_CURSO_PROXY");
        }

        public void SalvarEExcluirAreaConsultoriaRepresentante(CursoProxyDTO curso)
        {
            ExcluirAreaConsultoriaRepresentante(curso);
             //new CursoProxySRV().ExcluirPropertyList<AreaConsultoriaCursoProxyDTO>(curso, "areaConCursoProxy");
            
            var lstAreaConsultoriaCurso = curso.AREA_CONSULTORIA_CURSO_PROXY;

            if (lstAreaConsultoriaCurso != null)
            {
                SalvarAreasConsultoriaCurso(curso, lstAreaConsultoriaCurso.AsQueryable());
            }
        }

        public void SalvarAreasConsultoriaCurso(CursoProxyDTO curso, IQueryable<AreaConsultoriaCursoProxyDTO> areas)
        {
            if (areas != null)
            {
                CheckAndAssignKeyFromParentToChildsList(curso, areas, "CMP_ID");
                SaveOrUpdateNonIdentityKeyEntity(areas);
            }
        }
    }
}
