using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.Mvc.Html;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Service;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Contexto;

namespace COAD.SEGURANCA.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString Alert(this HtmlHelper htmlHelper, string mensagem = null, string titleMessage = "")
        {
            //string result = "<script> alert('"+mensagem+"') </script> ";
            string id = "myModal";
            string titleMsg = "";
            string msg = mensagem;

            if(string.IsNullOrEmpty(titleMessage))
                titleMsg = "COAD - Sistema de Gestão";
            else
                titleMsg = titleMessage;

            string content = @"<!-- Modal -->
                <div class=""modal fade"" id=""" + id + @""" tabindex=""-1"" role=""dialog"" aria-labelledby=""myModalLabel"" aria-hidden=""true"">
                  <div class=""modal-dialog"">
                    <div class=""modal-content"">
                      <div class=""modal-header"">
                        <button type=""button"" class=""close"" data-dismiss=""modal""><span aria-hidden=""true"">&times;</span><span class=""sr-only"">Close</span></button>
                        <h4 class=""modal-title"" id=""myModalLabel"">" + titleMsg + @"</h4>
                      </div>
                      <div class=""modal-body"">"
                                  + msg +
                            @"</div>
                      <div class=""modal-footer"">
                        <button type=""button"" class=""btn btn-default"" data-dismiss=""modal"">Fechar</button>                        
                      </div>
                    </div>
                  </div>
                </div>";

            string result = content + "  " + "<script>  $('#myModal').modal('show') </script>";

            return new MvcHtmlString(result);
        }
        public static MvcHtmlString MostraLogin(this HtmlHelper htmlHelper)
        {
            string content = null;
            if (SessionContext.autenticado != null)
            {
                content = @"<ul class=""nav navbar-right top-nav"">
                                  <li class=""dropdown"">
                                      <a href=""#"" class=""dropdown-toggle"" data-toggle=""dropdown""><i class=""fa fa-user""></i> " +
                                     SessionContext.autenticado.USU_LOGIN + @" [ " + SessionContext.autenticado.PER_ID + @" ] <b class=""caret""></b></a>";

                content += @" <ul class=""dropdown-menu"">";

                List<PERFIL_USUARIO> _perfis = SessionContext.perfis_usuario;

                for (int i = 0; i < _perfis.Count; i++)
                {

                    content += @"<li><a href='/Home/MudaPerfil?_per_id=" + _perfis[i].PER_ID + @"'><i class=""fa fa-fw fa-user""></i>" + _perfis[i].PER_ID + "</a></li>";
                }

                content += @"<li class=""divider""></li>
                         <li><a href=""/Usuario/AlterarSenha""><i class=""fa fa-fw fa-edit""></i>Alterar Senha</a></li>";
                content += @"<li class=""divider""></li><li><a href=""../Sair/Index""><i class=""fa fa-fw fa-power-off""></i>Sair</a></li></ul>";
                content += "</li></ul>";

            }
            else
            {
                content = @"<ul class=""nav navbar-right top-nav"">" +
                                  @"<li class=""dropdown"">Login</li></ul>";
            }
            return new MvcHtmlString(content);
        }
        public static MvcHtmlString MostraLoginResponsivo(this HtmlHelper htmlHelper, string nomeRepresentante = null)
        {
            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);
            var action = urlHelper.Action("Index", "Sair");
            
            string repString = @" | " + ((!string.IsNullOrWhiteSpace(nomeRepresentante)) ? nomeRepresentante : "") + " | ";
            
            string content = null;
            if (SessionContext.autenticado != null)
            {
                content = @"<li class=""dropdown"">
                                      <a href=""#"" class=""dropdown-toggle"" data-toggle=""dropdown""><i class=""fa fa-user""></i> " +
                                      SessionContext.autenticado.USU_LOGIN + @" [ " + SessionContext.autenticado.PER_ID + @" ] <b class=""caret""></b></a>";

                content += @" <ul class=""dropdown-menu"">";

                content += @"<li><a href='javascript:void(0)'><i class=""fa fa-fw fa-user fa-2x"" style=""color:blue;""></i>" +  SessionContext.autenticado.USU_LOGIN + repString + "</a></li>";
                content += @"<li class=""divider""></li>";
                List<PERFIL_USUARIO> _perfis = SessionContext.perfis_usuario;

                for (int i = 0; i < _perfis.Count; i++)
                {

                    content += @"<li><a href='/Home/MudaPerfil?_per_id=" + _perfis[i].PER_ID + @"'><i class=""fa fa-fw fa-user""></i>" + _perfis[i].PER_ID + "</a></li>";
                }

                content += @"<li class=""divider""></li>
                         <li><a href=""/Usuario/AlterarSenha""><i class=""fa fa-fw fa-edit""></i>Alterar Senha</a></li>";
                content += @"<li class=""divider""></li><li><a href=""/Sair/Index""><i class=""fa fa-fw fa-power-off""></i>Sair</a></li></ul>";
                content += "</li>";
            }
            else
            {
                content = @"<li class=""dropdown"">Login</li>";
            }
            return new MvcHtmlString(content);
        }
        public static MvcHtmlString MostraMenu(this HtmlHelper htmlHelper, string context = "-id")
        {

            string content = "";
            
            List<Menu> _menu = SessionContext.menu_usuario;

            if (_menu != null)
            {

                foreach (var _item in _menu)
                {
                    content += @"<li>";

                    if (_item.MenuItens.Count > 0)
                    {
                        if (_item.MenuText == "Home")
                        {
                            content += "<a href='" + _item.MenuUrl + @"' data-toggle=""collapse"" role=""button"" aria-haspopup=""true"" aria-expanded=""false"" data-target=#" + _item.MenuText + context + @"><i class=""fa fa-home fa-fw""></i>   " + _item.MenuText + @" <i class=""""></i></a>";
                        }
                        else
                        {
                            content += "<a href='" + _item.MenuUrl + @"' data-toggle=""collapse"" role=""button"" aria-haspopup=""true"" aria-expanded=""false"" data-target=#" + _item.MenuText + context + @"><i class=""glyphicon glyphicon-folder-open""></i>   " + _item.MenuText + @" <i class=""""></i></a>";
                        }
                        content += @"<ul id='" + _item.MenuText + context + @"' class=""collapse"">";

                        foreach (var _subitem in _item.MenuItens)
                        {

                            if (_subitem.MenuItens.Count > 0)
                            {
                                content += "<li><a href='" + _subitem.MenuUrl + @"' data-toggle=""collapse"" role=""button"" aria-haspopup=""true"" aria-expanded=""false"" data-target=#" + _subitem.MenuText + context + @"><i class=""glyphicon glyphicon-folder-open""></i>   " + _subitem.MenuText + @" <i class=""""></i></a></li>";

                                content += @"<ul id='" + _subitem.MenuText + context + @"' class=""collapse"">";

                                foreach (var _subsubitem in _subitem.MenuItens)
                                {

                                    content += @"<li><a href='" + _subsubitem.MenuUrl + @"'><i class=""glyphicon glyphicon-list-alt""></i> " + _subsubitem.MenuText + "</a></li>";
                                }

                                content += @"</ul>";
                            }
                            else
                            {
                                content += @"<li><a href='" + _subitem.MenuUrl + @"'><i class=""glyphicon glyphicon-list-alt""></i> " + _subitem.MenuText + "</a></li>";
                            }

                        }

                        content += @"</ul>";
                    }
                    else
                    {
                        if (_item.MenuText == "Home")
                            content += "<li><a href='" + _item.MenuUrl + @"' ><i class=""fa fa-home""></i>  " + _item.MenuText + @"</a></li>";
                        else
                            content += "<li><a href='" + _item.MenuUrl + @"' ><i class=""glyphicon glyphicon-th-list""></i> " + _item.MenuText + @"</a></li>";
                    }

                    content += " </li> ";

                }
            }

            //    content += " </ul></div> ";

            return new MvcHtmlString(content);
        }
        public static MvcHtmlString MostraSistemas(this HtmlHelper htmlHelper)
        {

            string content = @"<div class=""navbar-header"">
                                <li class=""dropdown"">
                                    <a href=""#"" class=""navbar-brand"" data-toggle=""dropdown""><i class=""fa fa-envelope""></i> " +
                                   SessionContext.sis_id + @" <b class=""caret""></b></a> <ul class=""dropdown-menu alert-dropdown"">";

            List<SISTEMA> _sistemas = SessionContext.sistemas_coad;

            for (int i = 0; i < _sistemas.Count; i++)
            {
                content += @"<li><a href='" + _sistemas[i].SIS_URL + @"'><i class=""fa fa-fw fa-user""></i>" + _sistemas[i].SIS_ID + "</a></li>";
            }

            content += "</ul></li> </div>";

            return new MvcHtmlString(content);
        }
        public static MvcHtmlString PesquisaCidade(this HtmlHelper htmlHelper, string _ngretorno = null, string _ngretorno2 = null)
        {

            string _content = @"<div class=""row"" style=""overflow:auto; max-height:200px;position: absolute; z-index: 1000; background-color: #FFF;"">
                                    <div class=""col-lg-12"">
                                        <div class=""form-group"">
                                            <div class=""autocomplete"" ng-show=""completing"">
                                                <table class=""table table-striped table-bordered table-hover"" id=""tbpesquisa"">
                                                    <thead>
                                                        <tr>
                                                            <th>Código
                                                            </th>
                                                            <th>Descrição
                                                            </th>
                                                            <th>Uf
                                                            </th>
                                                            <th></th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <tr ng-repeat=""municipio in dbmunicipio"">
                                                            <td>{{municipio.MUN_ID}}
                                                            </td>
                                                            <td>{{municipio.MUN_DESCRICAO}}</td>
                                                            <td>{{municipio.UF}}
                                                            </td>
                                                            <td>
                                                                <a href=""javascript:void(0)"" ng_click=""seleciona(municipio,'" + _ngretorno + @"')""  class=""btn btn-default"">Seleciona</a>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </div>";

            return new MvcHtmlString(_content);
        }
        public static MvcHtmlString PesquisaCidade2(this HtmlHelper htmlHelper, string _ngretorno = null)
        {

            string _content = @"<div class=""row"" style=""overflow:auto; max-height:200px;position: absolute; z-index: 1000; background-color: #FFF;"">
                                    <div class=""col-lg-12"">
                                        <div class=""form-group"">
                                            <div class=""autocomplete"" ng-show=""completing2"">
                                                <table class=""table table-striped table-bordered table-hover"" id=""tbpesquisa"">
                                                    <thead>
                                                        <tr>
                                                            <th>Código
                                                            </th>
                                                            <th>Descrição
                                                            </th>
                                                            <th>Uf
                                                            </th>
                                                            <th></th>
                                                        </tr>
                                                    </thead>
                                                    <tbody>
                                                        <tr ng-repeat=""municipio in dbmunicipio"">
                                                            <td>{{municipio.MUN_ID}}
                                                            </td>
                                                            <td>{{municipio.MUN_DESCRICAO}}</td>
                                                            <td>{{municipio.UF}}
                                                            </td>
                                                            <td>
                                                                <a href=""javascript:void(0)"" ng_click=""seleciona2(municipio,'" + _ngretorno + @"')""  class=""btn btn-default"">Seleciona</a>
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </div>";

            return new MvcHtmlString(_content);
        }
        public static MvcHtmlString DescricaoProduto(this HtmlHelper htmlHelper, string _ngretorno = null)
        {

            //string result = "<script> alert('"+mensagem+"') </script> ";
            string id = "pesquisaProduto";
            string titleMsg = "COAD - Sistema de Gestão (Pesquisar Produtos)";
         
            string _content = @"<div class=""modal fade"" id=""" + id + @""" tabindex=""-1"" role=""dialog"" aria-labelledby=""myModalLabel"" aria-hidden=""true"">
                                    <div class=""modal-dialog"">
                                        <div class=""modal-content"">
                                            <div class=""modal-header"">
                                                <button type=""button"" class=""close"" data-dismiss=""modal""><span aria-hidden=""true"">&times;</span><span class=""sr-only"">Close</span></button>
                                                <h4 class=""modal-title"" id=""myModalLabel"">" + titleMsg + @"</h4>
                                            </div>
                                            <div class=""panel-body"">
                                                <div class=""col-lg-12"">
                                                    <div class=""form-group input-group"" style=""width: 400px; float: left;"">
                                                        <label for=""Informe"">Informe a descrição do produto</label>
                                                        <span class=""input-group-btn"" style=""width: 360px; float: left;"">
                                                           <input  class=""form-control""  id=""nomeproduto""   ng-model=""nomeproduto"" type=""text"" />
                                                           <input  class=""form-control""  id=""indexretorno""  ng-model=""indexretorno"" type=""text"" style=""display:none;"" />
                                                           <a href=""javascript:void(0)"" class=""btn btn-default"" ng-click=""BuscarProduto(nomeproduto)""><span class=""fa fa-search""></span></a>
                                                        </span>
                                                    </div>
                                                </div>
                                                <div class=""col-lg-12"" ng-show=""mostraconsulta"" style=""max-height: 300px; overflow: scroll;"">
                                                    <table class=""table table-striped table-bordered table-hover"" id=""tbpesquisa"">
                                                        <thead>
                                                            <tr>
                                                                <th>Código
                                                                </th>
                                                                <th>Descrição
                                                                </th>
                                                                <th></th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <tr ng-repeat=""produto in dbproduto"">
                                                                <td>{{produto.PRO_ID}}
                                                                </td>
                                                                <td>{{produto.PRO_NOME}}</td>
                                                                <td>
                                                                   <a href=""javascript:void(0)"" data-dismiss=""modal"" ng_click=""fechaJanelaProduto(produto)"" ><i class=""fa  fa-check-square-o""></i></a>
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                    <div class=""form-group"">
                                                        
                                                    </div>   
                                                </div>     
                                            </div>
                                            <div class=""modal-body"">
                                        
                                            </div>
                                        </div>
                                    </div>
                                </div>";


            return new MvcHtmlString(_content);
        }
        public static MvcHtmlString DescricaoCliente(this HtmlHelper htmlHelper, string _ngretorno = null)
        {

            string id = "pesquisaCliente";
            string titleMsg = "COAD - Sistema de Gestão (Pesquisar Cliente)";

            string _content = @"<div class=""modal fade"" id=""" + id + @""" tabindex=""-1"" role=""dialog"" aria-labelledby=""myModalLabel"" aria-hidden=""true"">
                                    <div class=""modal-dialog"">
                                        <div class=""modal-content"">
                                            <div class=""modal-header"">
                                                <button type=""button"" class=""close"" data-dismiss=""modal""><span aria-hidden=""true"">&times;</span><span class=""sr-only"">Close</span></button>
                                                <h4 class=""modal-title"" id=""myModalLabel"">" + titleMsg + @"</h4>
                                            </div>
                                            <div class=""panel-body"">
                                                <div class=""col-lg-12"">
                                                    <div class=""form-group input-group"" style=""width: 400px; float: left;"">
                                                        <label for=""Informe"">Informe o nome do cliente</label>
                                                        <span class=""input-group-btn"" style=""width: 360px; float: left;"">
                                                           <input  class=""form-control""  id=""nomecliente""   ng-model=""nomecliente"" type=""text"" />
                                                           <a href=""javascript:void(0)"" class=""btn btn-default"" ng-click=""BuscarClientes(nomecliente)""><span class=""fa fa-search""></span></a>
                                                        </span>
                                                    </div>
                                                </div>
                                                <div class=""col-lg-12"" ng-show=""dbCliente"" style=""max-height: 300px; overflow: scroll;"">
                                                    <table class=""table table-striped table-bordered table-hover"" id=""tbpesquisa"">
                                                        <thead>
                                                            <tr>
                                                                <th>Assinatura
                                                                </th>
                                                                <th>Código
                                                                </th>
                                                                <th>Descrição
                                                                </th>
                                                                <th></th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <tr ng-repeat=""Assinatura in dbCliente"">
                                                                <td>{{Assinatura.ASN_NUM_ASSINATURA}}
                                                                </td>
                                                                <td>{{Assinatura.CLI_ID}}
                                                                </td>
                                                                <td>{{Assinatura.CLIENTES.CLI_NOME}}</td>
                                                                <td>
                                                                   <a href=""javascript:void(0)"" data-dismiss=""modal"" ng_click=""fechaJanelaCliente(Assinatura)"" ><i class=""fa  fa-check-square-o""></i></a>
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                    <div class=""form-group"">
                                                        
                                                    </div>   
                                                </div>     
                                            </div>
                                            <div class=""modal-body"">
                                        
                                            </div>
                                        </div>
                                    </div>
                                </div>";


            return new MvcHtmlString(_content);
        }
        public static MvcHtmlString PesquisaPopUpCidade(this HtmlHelper htmlHelper, string _ngretorno = null)
        {

            //string result = "<script> alert('"+mensagem+"') </script> ";
            string id = "pesquisaCidade";
            string titleMsg = "COAD - Sistema de Gestão (Pesquisar Cidades)";


            string _content = @"<div class=""modal fade"" id=""" + id + @""" tabindex=""-1"" role=""dialog"" aria-labelledby=""myModalLabel"" aria-hidden=""true"">
                                    <div class=""modal-dialog"">
                                        <div class=""modal-content"">
                                            <div class=""modal-header"">
                                                <button type=""button"" class=""close"" data-dismiss=""modal""><span aria-hidden=""true"">&times;</span><span class=""sr-only"">Close</span></button>
                                                <h4 class=""modal-title"" id=""myModalLabel"">" + titleMsg + @"</h4>
                                            </div>
                                            <div class=""panel-body"">
                                                <div class=""col-lg-12"">
                                                    <div class=""form-group"" style=""width: 400px; float: left;"">
                                                       <label for=""Informe"">Informe a descrição da ciade</label>
                                                       <input  class=""form-control""  id=""nomemunicipio""  ng-model=""nomemunicipio"" type=""text"" />
                                                    </div>
                                                    <div class=""form-group"" style=""float: right; padding-left: 4px; padding-top: 28px;"">
                                                        <a href=""javascript:void(0)"" class=""btn btn-default"" ng-click=""pesquisarMunicipio(nomemunicipio)""><span class=""fa fa-search""></span></a>
                                                    </div>
                                                </div>
                                                <div class=""col-lg-12"" ng-show=""mostraconsulta"" style=""max-height: 300px; overflow: scroll;"">
                                                    <table class=""table table-striped table-bordered table-hover"" id=""tbpesquisa"">
                                                        <thead>
                                                            <tr>
                                                                <th>Código
                                                                </th>
                                                                <th>Descrição
                                                                </th>
                                                                <th>Uf
                                                                </th>
                                                                <th></th>
                                                            </tr>
                                                        </thead>
                                                        <tbody>
                                                            <tr ng-repeat=""municipio in dbmunicipio"">
                                                                <td>{{municipio.MUN_ID}}
                                                                </td>
                                                                <td>{{municipio.MUN_DESCRICAO}}</td>
                                                                <td>{{municipio.UF}}
                                                                </td>
                                                                <td>
                                                                   <a href=""javascript:void(0)"" data-dismiss=""modal"" ng_click=""FecharJanelaCidade(municipio)"" class=""btn btn-default"">Seleciona</a>
                                                                </td>
                                                            </tr>
                                                        </tbody>
                                                    </table>
                                                    <div class=""form-group"">
                                                        
                                                    </div>   
                                                </div>     
                                            </div>
                                            <div class=""modal-body"">
                                        
                                            </div>
                                        </div>
                                    </div>
                                </div>";


            return new MvcHtmlString(_content);
        }
        public static MvcHtmlString MostrarLinkArqSped(this HtmlHelper htmlHelper)
        {

            string _content = @"<div class=""col-lg-12""  ng-show=""lnkVisible"">
                                   <a href=""{{lnkPath}}""  ><i class=""fa fa-search fa-fw""></i> {{lnkLink}} </a></div>";
            //target="_blank"
            //target=""_blank""

            return new MvcHtmlString(_content);
        }
        public static MvcHtmlString MostraImgHomolog(this HtmlHelper htmlHelper, string _sistema = null)
        {
            
            string _content = "";
            string _ip = "http://"+HttpContext.Current.Request.Url.Authority;

            SISTEMA sis = new SistemaSRV().BuscarPorId(_sistema);

            if (_ip != sis.SIS_URL_PRODUCAO)
            {
                _content = @" <div  class=""navbar-header""><h3>Homologação</H3></div>";
            }

         
            //target="_blank"
            //target=""_blank""

            return new MvcHtmlString(_content);
        }

    }
}