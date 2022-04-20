using System;
using System.Text;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Service;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Service;
using COAD.SEGURANCA.Util;
using GenericCrud.Service;

namespace COAD.CORPORATIVO.SessionUtils
{
    public static class SessionUtil
    {
        public static void HandleException(Exception ex)
        {
            if (SessionContext.usu_login_desktop == "" || SessionContext.usu_login_desktop == null)
                SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), SessionContext.autenticado, exception: ex);
            else
            {
                Autenticado aut = new Autenticado();
                aut.USU_LOGIN = SessionContext.usu_login_desktop;

                SysException.RegistrarLog(SysException.Show(ex), SysException.ShowIdException(ex), aut, exception: ex);
            }

        }

        public static string RecursiveShowExceptionsMessage(Exception e){

            StringBuilder sb = new StringBuilder();
            RecursiveShowMessage(e, sb);
            return sb.ToString();
        }

        private static void RecursiveShowMessage(Exception ex, StringBuilder sb)
        {
            if (ex == null)
            {
                return;
            }
            else
            {
                sb.Append("\n\r");
                sb.Append("---->");
                sb.Append(ex.Message);
                RecursiveShowMessage(ex.InnerException, sb);
            }
        }
        

        public static int? GetRegiao()
        {
            int? RG_ID = SessionContext.GetInSession<int?>("rg_id");

            if (RG_ID != null)
            {
                return RG_ID;
            }

            int? REP_ID = null;

            if (AuthUtil.TryGetRepId(out REP_ID))
            {                
                RepresentanteDTO representante = ServiceFactory.RetornarServico<RepresentanteSRV>().FindById(REP_ID);

                if (representante != null && representante.RG_ID != null)
                {
                    SessionContext.PutInSession<int?>("rg_id", representante.RG_ID);
                    return representante.RG_ID;
                }
            }

            return null;
            
        }
        public static RegiaoDTO GetRegiaoDTO()
        {
            var rgId = GetRegiao();
            var regiao = new RegiaoSRV().FindByIdFullLoaded(rgId, true, true);

            return regiao;
        }

        public static RepresentanteDTO GetRepresentante()
        {
            var repId = SessionContext.GetIdRepresentante();
            var representante = ServiceFactory.RetornarServico<RepresentanteSRV>().FindById(repId);
            return representante;
        }

        /// <summary>
        /// Retorna a uen do representante logado
        /// </summary>
        /// <returns></returns>
        public static int? GetUenId()
        {
            int? uen_id = SessionContext.GetInSession<int?>("uen_id");

            if (uen_id != null)
            {
                return uen_id;
            }

            int? REP_ID = null;

            if (AuthUtil.TryGetRepId(out REP_ID))
            {
                RepresentanteDTO representante = ServiceFactory.RetornarServico<RepresentanteSRV>().FindById(REP_ID);

                if (representante != null && representante.UEN_ID != null)
                {
                    SessionContext.PutInSession<int?>("uen_id", representante.UEN_ID);
                    return representante.UEN_ID;
                }
            }

            return null;

        }
        /// <summary>
        /// Retorna a uen do representante logado
        /// </summary>
        /// <returns></returns>
        public static int? GetEmpIdDoRepresentante()
        {
            int? emp_id = SessionContext.GetInSession<int?>("emp_id");

            if (emp_id != null)
            {
                return emp_id;
            }

            int? REP_ID = null;

            if (AuthUtil.TryGetRepId(out REP_ID))
            {
                RepresentanteDTO representante = ServiceFactory.RetornarServico<RepresentanteSRV>().FindById(REP_ID);

                if (representante != null && representante.EMP_ID != null)
                {
                    SessionContext.PutInSession<int?>("emp_id", representante.EMP_ID);
                    return representante.EMP_ID;
                }
            }

            return null;

        }


        public static void SetRegiao(int? rgId)
        {
            SessionContext.PutInSession<int?>("rg_id", rgId);
        }

        public static void SetUenId(int? uenId)
        {
            SessionContext.PutInSession<int?>("uen_id", uenId);
        }

        public static string GetNomeRepresentante()
        {
            string rep_nome = SessionContext.GetInSession<string>("rep_nome");

            if (!string.IsNullOrWhiteSpace(rep_nome))
            {
                return rep_nome;
            }

            int? REP_ID = null;

            if (AuthUtil.TryGetRepId(out REP_ID))
            {
                RepresentanteDTO representante = ServiceFactory.RetornarServico<RepresentanteSRV>().FindById(REP_ID);

                if (representante != null && !string.IsNullOrWhiteSpace(representante.REP_NOME))
                {
                    SessionContext.PutInSession<string>("rep_nome", representante.REP_NOME);
                    return representante.REP_NOME;
                }
            }

            return null;

        }

        public static UENDTO GetUen()
        {
            int? uenId = GetUenId();
            UENDTO uen = null;

            if (uenId != null)
            {
                uen = ServiceFactory.RetornarServico<UENSRV>().FindById(uenId);
            }

            return uen;
        }

        public static string GetUenDescricao()
        {
            var uen = GetUen();

            if (uen != null)
            {
                return uen.UEN_DESCRICAO;
            }

            return null;
        }

        public static int? GetNivelAcessoPerfil()
        {
            int? nivel = null;
            if (!string.IsNullOrWhiteSpace(SessionContext.per_id))
            {
                var perId = SessionContext.per_id;
                var nivelRepresentante = ServiceFactory.RetornarServico<NivelRepresentanteSRV>().FindByPerId(perId);

                if (nivelRepresentante != null && nivelRepresentante.NRP_NIVEL != null)
                {
                    nivel = nivelRepresentante.NRP_NIVEL;
                }

            }

            return nivel;
        }

        public static PerfilModel GetPerfil()
        {
            var perId = SessionContext.per_id;
            var perfil = ServiceFactory.RetornarServico<PerfilSRV>().FindById(perId);

            return perfil;
        }

        /// <summary>
        /// Verifica se o representante logado pertence a uma região de franquia
        /// </summary>
        /// <returns></returns>
        public static bool Franquiado()
        {
            var rgId = GetRegiao();
            var regiao = ServiceFactory.RetornarServico<RegiaoSRV>().FindById(rgId);

            if (regiao.RG_FRANQUIA != null)
            {
                return (bool)regiao.RG_FRANQUIA;
            }
            return false;
        }

        /// <summary>
        /// Verifica se o representante logado é pertence a uma região de franquia ou se é
        /// um gerente ou administrador franquiador ou de TI.
        /// </summary>
        /// <returns></returns>
        public static bool FranquiadoOuGerente()
        {
            var gerenteOuAdm = SessionContext.IsGerenteDepartamentoOR(true, "TI", "FRANQUIADOR");
            var franquiado = Franquiado();

            return (gerenteOuAdm || franquiado);
        }

        /// <summary>
        /// Verifica se o representante logado é pertence a uma região de franquia ou se é
        /// um gerente ou administrador franquiador/franquiado ou de TI ou Adm de Vendas.
        /// </summary>
        /// <returns></returns>
        public static bool FranquiadoOuGerenteOuTI()
        {
            var gerenteOuAdm = SessionContext.IsGerenteDepartamentoOR(true, "TI", "FRANQUIADOR", "FRANQUIADO", "ADM_VENDAS", "VENDA_ASSI");
            return gerenteOuAdm;
        }

        
        /// <summary>
        /// Indica se o perfil logado é um dos que possui permissão para logar.
        /// </summary>
        /// <returns></returns>
        public static bool PossuiPermissaoParaFaturar()
        {
            var gerenteOuAdm =
                SessionContext.IsAdmDepartamentoOR("TI", "FRANQUIADOR")
                ||
                SessionContext.HasDepartamento("CONTROLADORIA");
            return gerenteOuAdm;
        }


        public static bool PossuiPermissaoParaEditarProspect()
        {
            var permissao =
                SessionContext.IsGerenteDepartamentoOR(true, "TI", "FRANQUIADOR", "VENDA_ASSI") ||
                SessionContext.HasDepartamento("SAC");
            return permissao;
        }

        public static bool AcessaAgenda()
        {
            var gerenteOuAdm = SessionContext.IsGerenteDepartamentoOR(true, "TI", "FRANQUIADOR");
            var franquiado = Franquiado();

            return (gerenteOuAdm || franquiado);
        }

        public static bool AdmAgenda()
        {
            var gerenteOuAdm = SessionContext.IsAdmDepartamentoOR("TI", "FRANQUIADOR");
            return gerenteOuAdm;
        }

        public static bool AdmSys()
        {
            var gerenteOuAdm = SessionContext.IsAdmDepartamentoOR("TI", "FRANQUIADOR");
            return gerenteOuAdm;
        }
        /// <summary>
        /// Permite executar uma função nesta classe com tipo bool para validar autenticação do usuário.
        /// </summary>
        /// <param name="nomeMetodo">Nome do Método ao ser executado.</param>
        /// <returns></returns>
        public static bool ValidarPermissaPorNomeMetodo(string nomeMetodo)
        {
            if(nomeMetodo != null)
            {
                var metodo = typeof(SessionUtil).GetMethod(nomeMetodo);

                if (metodo != null)
                {
                    if (!metodo.ReturnType.Equals(typeof(bool)))
                        throw new Exception(string.Format("Esse método não retorna o tipo bool. O método retorna o tipo {0}", metodo.ReturnType.FullName));
                    return (bool)metodo.Invoke(null, null);
                }
            }

            return false;
        }

        /// <summary>
        /// Indica se o perfil logado é um dos que possui permissão para logar.
        /// </summary>
        /// <returns></returns>
        public static bool PossuiGerenciaVenda()
        {
            var gerenteOuAdm =
                SessionContext.IsGerenteDepartamentoOR(true, "TI", "FRANQUIADOR", "VENDA_ASSI");
            return gerenteOuAdm;
        }

        /// <summary>
        /// Verifica se o representante logado possui notificacoes.
        /// </summary>
        /// <returns></returns>
        public static bool PossuiNotificacoes()
        {
            var gerenteOuAdm = SessionContext.IsGerenteDepartamentoOR(true, "TI", "FRANQUIADOR", "FRANQUIADO", "ADM_VENDAS", "VENDA_ASSI") 
                || SessionContext.HasDepartamento("FRANQUIADO");
            return gerenteOuAdm;
        }

        /// <summary>
        /// Habilita cancelamento de contrato para os seguintes perfis...
        /// </summary> 
        /// <returns></returns>
        public static bool PerfilPossuiPermissaoDeCancelamentoDeContrato()
        {
            var permissao =
                SessionContext.IsDepartamento("CONTROLADORIA") ||
                SessionContext.IsDepartamento("COMERCIAL");

            return permissao;

        }
    }
}