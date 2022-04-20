using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Service;
using COAD.PROXY.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PROXY.Service
{
    public class AreaConsultoriaRepresentanteProxySRV : AreaConsultoriaRepresentanteSRV
    {
        private ColecionadorProxySRV _colecionadorSRV = new ColecionadorProxySRV();
        private GrandeGrupoProxySRV _grandeGrupoSRV = new GrandeGrupoProxySRV();

        /// <summary>
        /// Converte a lista de objeto para a versão proxy dela.
        /// </summary>
        /// <param name="lstAreaConsultoriaRep"></param>
        /// <returns></returns>
        public new IList<AreaConsultoriaRepresentanteProxyDTO> ConverterParaProxy(ICollection<AreaConsultoriaRepresentanteDTO> lstAreaConsultoriaRep)
        {
            var lstAreaConsultoriaRepProxy = ConvertWithProfile<ICollection<AreaConsultoriaRepresentanteDTO>, List<AreaConsultoriaRepresentanteProxyDTO>>(lstAreaConsultoriaRep, "proxy");
            return lstAreaConsultoriaRepProxy;            
        }

        public void PreencherAreaConsultoriaRepresentante(ProfessorProxyDTO rep)
        {
            if (rep != null && rep.REP_ID != null)
            {
                var repId = rep.REP_ID;

                if (rep.AREA_CONSULTORIA_REPRESENTANTE != null)
                {
                    var lstAreaConsultoriaRepresentante = ConverterParaProxy(rep.AREA_CONSULTORIA_REPRESENTANTE);

                    PrencherColecionadorEGrandeGrupo(lstAreaConsultoriaRepresentante);
                    rep.AREA_CONSULTORIA_REPRESENTANTE_PROXY = lstAreaConsultoriaRepresentante;
                }
                
            }
        }

        private void PrencherColecionadorEGrandeGrupo(IList<AreaConsultoriaRepresentanteProxyDTO> lstAreaConsultoriaRepresentante)
        {
            if (lstAreaConsultoriaRepresentante != null)
            {
                foreach (var areConRep in lstAreaConsultoriaRepresentante)
                {
                    _colecionadorSRV.PreencherColecionador(areConRep);
                    _grandeGrupoSRV.PreencherGrandeGrupo(areConRep);
                }
            }
        }


        public void ExcluirAreaConsultoriaRepresentante(ProfessorProxyDTO professor)
        {
            var REP_ID = (int)professor.REP_ID;
            ProfessorProxyDTO professorDoBanco = new ProfessorProxySRV().FindByIdWithUser(REP_ID);

            ExcluirList<ProfessorProxyDTO, AreaConsultoriaRepresentanteProxyDTO>(professor, professorDoBanco, "AREA_CONSULTORIA_REPRESENTANTE_PROXY");

        }

        public void SalvarEExcluirAreaConsultoriaRepresentante(ProfessorProxyDTO professor)
        {
            ExcluirAreaConsultoriaRepresentante(professor);
            var lstAreaConsultoriaRepresentante = professor.AREA_CONSULTORIA_REPRESENTANTE_PROXY;

            if (lstAreaConsultoriaRepresentante != null)
            {
                SalvarAreasConsultoriaRepresentante(professor, lstAreaConsultoriaRepresentante.AsQueryable());
            }
        }

        public void SalvarAreasConsultoriaRepresentante(ProfessorProxyDTO professor, IQueryable<AreaConsultoriaRepresentanteProxyDTO> areas)
        {
            if (areas != null)
            {
                foreach (var are in areas)
                {
                    if (are.REP_ID == null)
                    {
                        are.REP_ID = professor.REP_ID;
                    }

                }
                SaveOrUpdateNonIdentityKeyEntity(areas);

            }
        }
    }
}
