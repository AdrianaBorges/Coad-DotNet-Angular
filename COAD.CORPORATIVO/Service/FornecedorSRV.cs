using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Service.Base;
using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;

namespace COAD.CORPORATIVO.Service
{
    public class FornecedorSRV : ServiceAdapter<FORNECEDOR,FornecedorDTO>
    {
        private FornecedorDAO _dao = new FornecedorDAO();

        public FornecedorSRV()
        {
            SetDao(_dao);
        }
        public FornecedorDTO FindByIDFull(int _for_id)
        {
            return _dao.FindByIDFull(_for_id);
        }
        public FornecedorDTO BuscarPorCNPJ(string _for_cnpj)
        {
            FornecedorDTO f = _dao.BuscarPorCNPJ(_for_cnpj);

            return f;
        }
        public IList<FornecedorDTO> BuscarPorTipo(int _tipo_for_id)
        {
            return _dao.BuscarPorTipo(_tipo_for_id);
        }
        public FornecedorDTO VerficarIncluir(FornecedorDTO _forn)
        {
            return _dao.VerficarIncluir(_forn);
        }
        public Pagina<FornecedorDTO> BuscarPorRazaoSocial(string _razaosocial, int numpagina = 1, int linhas = 10)
        {
            Pagina<FornecedorDTO> _listafornecedor = _dao.BuscarPorRazaoSocial(_razaosocial, numpagina, linhas);

            return _listafornecedor;
        }
    }
}
