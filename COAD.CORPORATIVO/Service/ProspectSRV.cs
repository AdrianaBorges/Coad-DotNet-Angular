using System;
using System.Linq;
using System.Collections.Generic;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Service.Base;
using Coad.GenericCrud.Repositorios.Base;

namespace COAD.CORPORATIVO.Service
{
    public class ProspectSRV : ServiceAdapter<CLIENTES, ClienteDto>
    {
        public ProspectDAO _dao { get; set; }//= new ProspectDAO();
      
        public CLIENTES BuscarPorCNPJ(string _cli_cnpj)
        {
            CLIENTES c = _dao.BuscarPorCNPJ(_cli_cnpj);

            return c;
        }

        public ProspectSRV()
        {
            _dao = new ProspectDAO();
            SetDao(_dao);
        }

        public ProspectSRV(ProspectDAO dao)
        {
            _dao = dao;
            SetDao(dao);
        }

        public Pagina<ClienteDto> Prospects(string cnpj = null, string nome = null, int pagina = 1, int registroPorPagina = 7, int? representanteId = null)
        {
            return _dao.Prospects(cnpj, nome, pagina, registroPorPagina, representanteId);
        }

      } 
}
