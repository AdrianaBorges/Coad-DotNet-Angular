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
    public class MateriaSRV : GenericService<BUSCAR_MATERIA_POR_ID_PORTAL_PROC_Result, MateriaDTO, string>
    {
        private MateriaDAO _dao = new MateriaDAO();

        public MateriaSRV()
        {
            Dao = _dao;           
        }

        public MateriaDTO Materia(string id)
        {
            MateriaDTO materia = _dao.Materia(id);
            return materia;
        }
    }
}
