using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using Coad.GenericCrud.Dao.Base;
using COAD.CORPORATIVO.Model.Dto.Custons.Ecommerce;
using COAD.SEGURANCA.Service;

namespace COAD.CORPORATIVO.DAO
{
    public class ClienteUsuarioDAO : AbstractGenericDao<CLIENTE_USUARIO, ClienteUsuarioDTO,int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public SenhaSRV _senhaSRV { get; set; }
        public ClienteUsuarioDAO()
        {
            db = GetDb<COADCORPEntities>(false);
			this._senhaSRV = new SenhaSRV();				
        }

        public bool ChecarUsuarioJaExiste(string usuario)
        {
            var query = (from us in db.CLIENTE_USUARIO 
                         where us.USC_LOGIN.ToUpper() == usuario.ToUpper()
                             select us)
                             .Count();

            return (query > 0);
        }

        public bool ChecarClienteJaPossuiLogin(int? cliId)
        {
            var count = (from us in db.CLIENTE_USUARIO
                             where us.DATA_EXCLUSAO == null &&
                             us.CLI_ID == cliId
                             select us)
                             .Count();

            return (count > 0);
        }

        public bool ChecarClienteJaPossuiLogin(string asnAssinatura)
        {
            var count = (from us in db.CLIENTE_USUARIO
                         where us.DATA_EXCLUSAO == null && us.CLI_ID ==
                         (from assi in db.ASSINATURA where assi.ASN_NUM_ASSINATURA == asnAssinatura
                              select assi.CLI_ID).FirstOrDefault()
                         select us)
                             .Count();

            return (count > 0);
        }

        public ClienteUsuarioDTO LogarCliente(string usuario, string senha)
        {
            var clientUsuario = (from us in db.CLIENTE_USUARIO
                         where 
                            us.DATA_EXCLUSAO == null && 
                            us.USC_SENHA == senha
                         select us)
                             .FirstOrDefault();

            return ToDTO(clientUsuario);
        }
        public bool AdicionarClienteUsuario(string usuario, string hash)
        {
            try
            {


                CLIENTE_USUARIO usuarioEcommerce = new CLIENTE_USUARIO()
                {
                    USC_LOGIN = usuario,
                    USC_SENHA = hash,
                    USC_ATIVO = true

                };

                this.Save(usuarioEcommerce);

                //SysException.RegistrarLog("Cliente adicionado com sucesso!!", "", SessionContext.autenticado);

                return true;

            }
            catch (Exception ex)
            {

                //SysException.RegistrarLog("Erro ao adicionar cliente (" + SysException.Show(ex) + ")", SysException.ShowIdException(ex), SessionContext.autenticado);
                throw ex; // (ex.Message + " - " + ex.StackTrace);

            }


        }
    }
}
