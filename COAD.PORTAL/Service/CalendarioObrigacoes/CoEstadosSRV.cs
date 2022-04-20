using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Service.Base;
using COAD.PORTAL.DAO.CalendarioObrigacoes;
using COAD.PORTAL.Model.DTO.CalendarioObrigacoes;
using COAD.PORTAL.Repositorios.Contexto;

namespace COAD.PORTAL.Service.CalendarioObrigacoes
{
    public class CoEstadosSRV : GenericService<CO_ESTADOS, CoEstadosDTO, string>
    {
        private CoEstadosDAO _dao = new CoEstadosDAO();

        public CoEstadosSRV()
        {
            Dao = _dao;           
        }

        public IList<CoEstadosDTO> EstadosCalendario(IList<CoCalendarioDTO> calendarios)
        {
            var estadosDisponiveis = (from data in calendarios select new { data.CO_OBRIGACOES.CO_ESTADOS }).Distinct().ToList();
            CoEstadosDTO cedto = null;
            List<CoEstadosDTO> estados = new List<CoEstadosDTO>();
            if (estadosDisponiveis != null && estadosDisponiveis.Count() > 0)
            {
                foreach (var estado in estadosDisponiveis)
                {
                    if (estado.CO_ESTADOS != null)
                    {
                        cedto = new CoEstadosDTO();
                        cedto.COD_UF = estado.CO_ESTADOS.COD_UF;
                        cedto.NUM_UF = estado.CO_ESTADOS.NUM_UF;
                        estados.Add(cedto);
                    }
                }
            }

            return estados;
        }
    }
}
