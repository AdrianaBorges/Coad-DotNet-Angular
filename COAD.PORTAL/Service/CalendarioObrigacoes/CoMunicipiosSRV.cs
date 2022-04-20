using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Service.Base;
using COAD.PORTAL.DAO.CalendarioObrigacoes;
using COAD.PORTAL.Model.DTO.CalendarioObrigacoes;
using COAD.PORTAL.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.PORTAL.Service.CalendarioObrigacoes
{
    public class CoMunicipiosSRV : GenericService<CO_MUNICIPIOS, CoMunicipiosDTO, string>
    {
        private CoMunicipiosDAO _dao = new CoMunicipiosDAO();

        public CoMunicipiosSRV()
        {
            Dao = _dao;
           
        }

        public IList<CoMunicipiosDTO> MunicipiosCalendario(IList<CoCalendarioDTO> calendarios)
        {
            var municipiosDisponiveis = (from data in calendarios select new { data.CO_OBRIGACOES.CO_MUNICIPIOS }).Distinct().ToList();
            CoMunicipiosDTO cmdto = null;
            List<CoMunicipiosDTO> municipios = new List<CoMunicipiosDTO>();
            if (municipiosDisponiveis != null && municipiosDisponiveis.Count() > 0)
            {
                foreach (var mun in municipiosDisponiveis)
                {
                    if (mun.CO_MUNICIPIOS != null)
                    {
                        cmdto = new CoMunicipiosDTO();
                        cmdto.NUM_MUNICIPIO = mun.CO_MUNICIPIOS.NUM_UF;
                        cmdto.NOME_MUNICIPIO = mun.CO_MUNICIPIOS.NOME_MUNICIPIO;
                        municipios.Add(cmdto);
                    }
                }
            }

            return municipios;
        }
    }
}
