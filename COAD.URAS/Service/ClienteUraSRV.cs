using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.URAS.DAO;
using COAD.URAS.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.URAS.Service
{
    public class ClienteUraSRV
    {
        public ClienteUraDAO _dao = new ClienteUraDAO();
        public ClienteUraSRV()
        {
        }
        public Pagina<ClienteUraDTO> BuscarTodos(string ura_id, int numpagina = 1, int linhas = 10)
        {
            return _dao.BuscarTodos(ura_id, numpagina, linhas);
        }
        public Pagina<ClienteUraDTO> BuscarPorAssinatura(string codigo, string ura_id, int numpagina = 1, int linhas = 10)
        {
            return _dao.BuscarPorAssinatura(codigo, ura_id, numpagina, linhas);
        }
        public IList<ClienteUraDTO> ListarPorAssinatura(string codigo)
        {
            return _dao.ListarPorAssinatura(codigo);
        }
        public Pagina<ClienteUraDTO> BuscarPorTelefone(int telefone, string ura_id, int numpagina = 1, int linhas = 10)
        {
            return _dao.BuscarPorTelefone(telefone, ura_id, numpagina, linhas);
        }
        public void AtualizaUra(ClienteUraDTO _cliura)
        {
            _dao.AtualizaUra(_cliura);
        }


    }
}
