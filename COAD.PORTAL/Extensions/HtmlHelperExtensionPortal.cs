using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Contexto;

namespace COAD.PORTAL.Extensions
{
    public static class HtmlHelperExtensionPortal
    {
        public static MvcHtmlString MostraLoginPortal(this HtmlHelper htmlHelper, string nomeExibicao = "")
        {
//            string content = @"
//                                  <li class=""dropdown"">
//                                      <a href=""#"" class=""dropdown-toggle"" data-toggle=""dropdown""><i class=""fa fa-user""></i> Seja Bem Vindo," +
//                                       nomeExibicao + @" [ " + SessionContext.autenticado.perId + @" ] <b class=""caret""></b></a>";
            string content = @"
                                  <li class=""dropdown"">
                                      <a href=""#"" class=""dropdown-toggle"" data-toggle=""dropdown""><i class=""fa fa-user""></i>Bem Vindo," +
                                       nomeExibicao + @" <b class=""caret""></b></a>";
            content += @" <ul class=""dropdown-menu"">";

            List<PERFIL_USUARIO> _perfis = SessionContext.perfis_usuario;

//          content += @"<li class=""divider""></li>
//          <li><a href=""../Usuario/AlterarSenha""><i class=""fa fa-fw fa-edit""></i>Alterar Senha</a></li>";
            content += @"<li><a href=""/Login/Logout""><i class=""fa fa-fw fa-power-off""></i>Sair</a></li></ul>";
            content += "</li>";

            return new MvcHtmlString(content);
        }
    }
}
