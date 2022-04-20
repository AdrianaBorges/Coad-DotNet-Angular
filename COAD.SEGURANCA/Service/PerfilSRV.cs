using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.SEGURANCA.DAO;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Contexto;
using Coad.GenericCrud.Service.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using GenericCrud.Service.Base;
using System.Transactions;

namespace COAD.SEGURANCA.Service
{
    public class PerfilSRV : ServiceAdapter<PERFIL, PerfilModel, object>
    {
        private PerfilDAO _dao;

        public PerfilSRV(bool useDbContextCache = true)
        {
            _dao = new PerfilDAO(useDbContextCache);
            SetDao(_dao);
            SetKeys("EMP_ID", "PER_ID");

        }
        public PerfilModel VericarAcessoPerfil(string _per_id, string _dp_nome, int _niv_acesso)
        {
            return _dao.VericarAcessoPerfil(_per_id, _dp_nome, _niv_acesso);
        }

        /// <summary>
        /// Verifica se o usuário possui um perfil possuí o departamento e nível passado.
        /// Se permitirNiveisPrivilegiosSuperiores for true, assume que o nível passado é o nível mínimo para acessar. 
        /// E níveis com mais privilégios também podem acessar.
        /// </summary>
        /// <param name="_per_id">Id do perfil</param>
        /// <param name="_dp_nome">Nome do departamento</param>
        /// <param name="_niv_acesso">Nível do acesso</param>
        /// <param name="permitirNiveisPrivilegiosSuperiores">Permite níveis inferiores</param>
        /// <returns></returns>
        public Boolean VerificarNivelAcesso (string _per_id, string _dp_nome, int _niv_acesso, bool permitirNiveisPrivilegiosSuperiores = false)
        {
            bool resp = false;
                
            if (permitirNiveisPrivilegiosSuperiores)
                resp = _dao.VerificarNivelAcessoMinimo(_per_id,_dp_nome,_niv_acesso);
            else
                resp = _dao.VerificarNivelAcesso(_per_id, _dp_nome, _niv_acesso);
            
            return resp;
        }

      

        public List<ITEM_MENU_PERFIL> MontaMenuPerfil(string _per_id)
        {
            return new PerfilDAO().MontaMenuPerfil(_per_id);
        }
        public PerfilModel BuscarPorId(int _emp_id, string _per_id)
        {
            return FindById(_emp_id, _per_id);
        }
        public List<PerfilModel> BuscarLista(int _emp_id, string _per_id, string _sis_id)
        {
            return new PerfilDAO().BuscarLista(_emp_id, _per_id, _sis_id);
        }
        public List<PerfilModel> BuscarLista(string _per_id, string _sis_id)
        {
            return new PerfilDAO().BuscarLista(_per_id, _sis_id);
        }
        public void IncluirReg(PerfilModel perfil)
        {
            PERFIL _perfil = new PERFIL();
            _perfil.EMP_ID = (int) perfil.EMP_ID;
            _perfil.SIS_ID = perfil.SIS_ID;
            _perfil.PER_ID = perfil.PER_ID;
            _perfil.PER_ATIVO = perfil.PER_ATIVO;
            _perfil.PER_HORA_FIM = perfil.PER_HORA_FIM;
            _perfil.PER_HORA_INI = perfil.PER_HORA_INI;
            _perfil.PER_OUTROS_NIVEIS = perfil.PER_OUTROS_NIVEIS;


            new PerfilDAO().IncluirReg(_perfil);
        }
        public void SalvarReg(PerfilModel perfil)
        {
            //PERFIL _perfil = new PERFIL();
            //_perfil.EMP_ID = perfil.EMP_ID;
            //_perfil.SIS_ID = perfil.SIS_ID;
            //_perfil.perId = perfil.perId;
            //_perfil.PER_ATIVO = perfil.PER_ATIVO;
            //_perfil.PER_HORA_FIM = perfil.PER_HORA_FIM;
            //_perfil.PER_HORA_INI = perfil.PER_HORA_INI;
            //_perfil.PER_OUTROS_NIVEIS = perfil.PER_OUTROS_NIVEIS;           


            //new PerfilDAO().Salvar(_perfil);
            Merge(perfil);

        }
        public void ExcluirReg(PerfilModel perfil)
        {
            PERFIL _perfil = new PERFIL();
            _perfil.EMP_ID = (int) perfil.EMP_ID;
            _perfil.SIS_ID = perfil.SIS_ID;
            _perfil.PER_ID = perfil.PER_ID;
            _perfil.PER_ATIVO = perfil.PER_ATIVO;
            _perfil.PER_HORA_FIM = perfil.PER_HORA_FIM;
            _perfil.PER_HORA_INI = perfil.PER_HORA_INI;
            _perfil.PER_OUTROS_NIVEIS = perfil.PER_OUTROS_NIVEIS;


            new PerfilDAO().Excluir(_perfil);
        }

        public void SalvarMenuPerfil(List<Menu> _listamenu)
        {
            new PerfilDAO().SalvarMenuPerfil(_listamenu);
        }

        public Pagina<PerfilModel> Perfis(string nome = null, string sysId = null, int? DP_ID = null, int? NIV_ACE_ID = null, int pagina = 1, int registrosPorPagina = 10)
        {
            return _dao.Perfis(nome, sysId, DP_ID, NIV_ACE_ID, pagina, registrosPorPagina);
        }

        public IList<PerfilModel> ListarPerfis(string nome = null, string sysId = null, int? DP_ID = null, int? NIV_ACE_ID = null)
        {
            return _dao.ListarPerfis(nome, sysId, DP_ID, NIV_ACE_ID);
        }

        public void Salvar(PerfilModel perfil)
        {
            using (TransactionScope scope = new TransactionScope())
            {
            
                var callBack = new ActionCallback<PerfilModel>();

                callBack.BeforeSave(x => 
                    
                        {x.EMP_ID = 1;}
                    );

                SaveOrUpdateNonIdentityKeyEntity(perfil, null, callBack);

                new ItemMenuPerfilSRV().CriarAPartirDoPerfil(perfil);
                scope.Complete();
            }
        }
    }
}
