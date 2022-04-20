using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.SEGURANCA.Repositorios.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.DAO
{

    public class CadastroGratuitoDAO : DAOAdapter<CADASTRO_GRATUITO, CadastroGratuitoDTO, object>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public CadastroGratuitoDAO()
        {
            db = GetDb<COADCORPEntities>(false);
        }
        public Autenticado ValidarLogin(string _email)
        {
            try
            {

                Autenticado _autenticado = new Autenticado();

                var cadgratuito = (from p in db.CADASTRO_GRATUITO
                                    where (p.CGR_EMAIL == _email) 
                                    select p).FirstOrDefault();

                if (cadgratuito !=null)
                {
                    _autenticado.USU_NOME = cadgratuito.CGR_NOME;
                    _autenticado.USU_LOGIN = "GRATUITO";
                    _autenticado.PER_ID = cadgratuito.CGR_PERFIL;
                    _autenticado.EMAIL = cadgratuito.CGR_EMAIL;
                    _autenticado.USU_SENHA = cadgratuito.CGR_SENHA;
                    _autenticado.DATA_LOGIN = DateTime.Now;
                    _autenticado.MEIO_ACESSO = "PORTAL/APP";
                }

                return _autenticado;

            }
            catch (Exception ex)
            {

                SysException.RegistrarLog("Erro ao Validar login (" + SysException.Show(ex) + ")", SysException.ShowIdException(ex), SessionContext.autenticado);

                throw new Exception(SysException.Show(ex));

            }
        }
        public CadastroGratuitoDTO ValidarLogin(string _email, string _senha)
        {
            try
            {
                var _cadgratuito = (from p in db.CADASTRO_GRATUITO
                                    where (p.CGR_LOGIN == _email)
                                    orderby p.CGR_ID descending
                                    select p).FirstOrDefault();

                if (_cadgratuito != null)
                    if (SessionContext.HashMD5(_cadgratuito.CGR_SENHA) != _senha)
                        _cadgratuito = null;
                   
                return ToDTO(_cadgratuito);

            }
            catch (Exception ex)
            {

                SysException.RegistrarLog("Erro ao Validar login (" + SysException.Show(ex) + ")", SysException.ShowIdException(ex), SessionContext.autenticado);

                throw new Exception(SysException.Show(ex));

            }
        }

    }
}
