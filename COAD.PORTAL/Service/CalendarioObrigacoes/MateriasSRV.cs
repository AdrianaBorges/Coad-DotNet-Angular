using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Service.Base;
using COAD.PORTAL.DAO.CalendarioObrigacoes;
using COAD.PORTAL.Model.DTO.CalendarioObrigacoes;
using COAD.PORTAL.Repositorios.Contexto;

namespace COAD.PORTAL.Service.CalendarioObrigacoes
{
    public class MateriasSRV : GenericService<BUSCAR_MATERIA_PORTAL_PROC_Result, MateriasDTO, string>
    {
        private MateriasDAO _dao = new MateriasDAO();

        public MateriasSRV()
        {
            Dao = _dao;           
        }

        public Pagina<MateriasDTO> Materias(string label, string tipo, string num_ato, string ano, int pagina = 1, int nLinha = 7)
        {
            return _dao.Materias(label, tipo, num_ato, ano, pagina, nLinha);
        }
    }
}
