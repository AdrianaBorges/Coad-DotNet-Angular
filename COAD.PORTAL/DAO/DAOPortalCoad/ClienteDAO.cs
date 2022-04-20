using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Dao.Base;
using COAD.PORTAL.Model.DTO.PortalCoad;
using COAD.PORTAL.Repositorios.Contexto;

namespace COAD.PORTAL.DAO.DAOPortalCoad
{
    public class ClienteDAO : AbstractGenericDao<clientes, ClienteDTO, string>
    {
        private coadEntities db { get; set; }

        public ClienteDAO()
        {
            SetProfileName("portalCoad");
            //db = GetDb<COADEntities>(false);
        }


        public ClienteDTO BuscarClienteEmailCPF(string email, string cpf)
        {
            clientes query = GetDbSet().Where(x => x.usuario == email || x.email == email || x.cpf == cpf).FirstOrDefault();
            return ToDTO(query);
        }

        public ClienteDTO Logar(string login, string senha)
        {
            clientes query = GetDbSet().Where(x => x.usuario == login && x.senha == senha).FirstOrDefault();
            return ToDTO(query);
        }
    }
}
