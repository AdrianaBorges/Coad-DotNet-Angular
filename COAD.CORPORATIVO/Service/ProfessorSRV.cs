using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.CORPORATIVO.Model.Dto;
using GenericCrud.Config.DataAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("REP_ID")]
    public class ProfessorSRV : RepresentanteSRV
    {
        public ProfessorSRV() : base()
        {
        }
          
         public Pagina<RepresentanteDTO> ListarProfessorComUsuario(string nomeProfessor, int? RG_ID, int? UEN_ID, int pagina = 1,
           int registrosPorPagina = 7,
           int? nivelAcesso = null)
        {

            Pagina<RepresentanteDTO> lstRepresentantes = ListarProfessor(nomeProfessor, pagina, registrosPorPagina, RG_ID, UEN_ID, nivelAcesso: nivelAcesso);
            PreencherUsuario(lstRepresentantes.lista);

            return lstRepresentantes;
        }

         //public void SalvarProfessorEUsuario(RepresentanteDTO representante, bool update, int? RG_ID = null, int? UEN_ID = 1)
         //{
         //    representante.NRP_ID = 5;
         //    representante.NIVEL_REPRESENTANTE = new NivelRepresentanteSRV().FindById(5);
         //    SalvarRepresentanteEUsuario(representante, update, RG_ID, UEN_ID);
         //}

         public Pagina<RepresentanteDTO> ListarProfessor(
                string nomeProfessor = null,
                int pagina = 1,
                int registrosPorPagina = 15,
                int? RG_ID = null,
                int? UEN_ID = null,
                bool uenIdPreenchido = false,
                int? nivelAcesso = null)
         {
             return _dao.ListarProfessor(nomeProfessor, pagina, registrosPorPagina, RG_ID, UEN_ID, uenIdPreenchido, nivelAcesso);
         }       
        
    }
}
