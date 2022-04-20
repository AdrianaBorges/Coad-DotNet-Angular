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
    public class TransportadorSRV :  ServiceAdapter<TRANSPORTADOR,TransportadorDTO>
    {
        private TransportadorDAO _dao = new TransportadorDAO();

        public TransportadorSRV()
        {   
            SetDao(_dao);
        }
        public TransportadorDTO FindByIDFull(int _for_id)
        {
            return _dao.FindByIDFull(_for_id);
        }
        public TransportadorDTO BuscarPorCNPJ(string _tra_cnpj)
        {
            return new TransportadorDAO().BuscarPorCNPJ(_tra_cnpj);
        }
        public TransportadorDTO VerficarIncluir(TransportadorDTO _tran)
        {
            return new TransportadorDAO().VerficarIncluir(_tran);
        }
        public Pagina<TransportadorDTO> BuscarPorRazaoSocial(string _razaosocial, int numpagina = 1, int linhas = 10)
        {
            Pagina<TransportadorDTO> _listatransportador = new TransportadorDAO().BuscarPorRazaoSocial(_razaosocial, numpagina, linhas);

            return _listatransportador;
        }
    }
}
