using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Repositorios.Base;
using COAD.SEGURANCA.Repositorios.Contexto;
using COAD.SEGURANCA.Model;
using Coad.GenericCrud.Dao.Base.Pagination;
using GenericCrud.Config.DataAttributes;
using System.Transactions;
using Coad.Reflection;
using System.Text.RegularExpressions;
using System.Globalization;
using COAD.SEGURANCA.Model.Dto;
using COAD.SEGURANCA.DAO;
using COAD.SEGURANCA.Model.Dto.Custons.Pesquisas;

namespace COAD.SEGURANCA.Service
{
    [ServiceConfig("TPL_ID")]
    public class TemplateHTMLSRV : GenericService<TEMPLATE_HTML, TemplateHTMLDTO, int>
    {
        private TemplateHTMLDAO _dao { get; set; }     
        public FonteDadosDescricaoSRV _fonteDadosDescricaoSRV { get; set; }

        public TemplateHTMLSRV()
        {
            _dao = new TemplateHTMLDAO();
            Dao = _dao;
        }

        public TemplateHTMLSRV(TemplateHTMLDAO _dao)
        {
            this._dao = _dao;
            this.Dao = _dao;
        }

        public TemplateHTMLDTO FindByIdFullLoaded(int? tplId, bool trazFonteDadosDetalhes = false)
        {
            var template = FindById(tplId);
            if(template != null)
            {
                if (trazFonteDadosDetalhes)
                {
                    _fonteDadosDescricaoSRV.PreencherFonteDadosDescricaoNaFonteDados(template.FONTE_DADOS_TEMPLATE);
                }
            }
            return template;
        }

        public TemplateHTMLDTO SalvarTemplate(TemplateHTMLDTO template)
        {
            TemplateHTMLDTO templateSalvo = null;
            if(template != null)
            {
                using(var scope = new TransactionScope())
                {
                    SaveOrUpdate(template);
                    scope.Complete();
                }
            }
            return templateSalvo;
        }

        public Pagina<TemplateHTMLDTO> PesquisarTemplatesHTML(PesquisaTemplatesDTO pesquisaDTO)
        {
            return _dao.PesquisarTemplatesHTML(pesquisaDTO);
        }
        public Pagina<TemplateHTMLDTO> PesquisarTemplatesLayout(
            int pagina = 0,
            int registrosPorPagina = 10)
        {
            return _dao.PesquisarTemplatesLayout(pagina, registrosPorPagina);
        }
        

        public string GetDataSourceValue(string path, object DataSource)
        {
            if (!string.IsNullOrWhiteSpace(path) && DataSource != null)
            {
                var value = ReflectionProvider.GetMemberValue<object>(DataSource, path);
                if (value != null)
                {
                    if(value is decimal || value is float || value is double)
                    {
                        var real = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:N}", value);
                        return real;
                    }
                    if(value is DateTime)
                    {
                        var data = string.Format("{0:dd/MM/yyyy}", value);
                        return data;
                    }
                    return value.ToString();
                }
            }
            return null;
        }

        public TemplateHTMLDTO RetornarTemplatePorFuncionalidade(int? fsiId)
        {
            var dados = _dao.RetornarTemplatePorFuncionalidade(fsiId);

            if(dados != null && dados.FONTE_DADOS_TEMPLATE != null)
            {
                _fonteDadosDescricaoSRV.PreencherFonteDadosDescricaoNaFonteDados(dados.FONTE_DADOS_TEMPLATE);
            }
            return dados;
        }

        public string ProcessarTemplate(FuncionalidadeSistemaDTO funcionalidadeSistema, object dto)
        {
            if (funcionalidadeSistema != null && funcionalidadeSistema.FSI_ID != null)
                return ProcessarTemplate(funcionalidadeSistema.FSI_ID, dto);
            return null;
                    
        }

        public string ProcessarTemplate(int? psiId, object dto)
        {
            if (psiId != null && dto != null)
            {
                var template = RetornarTemplatePorFuncionalidade(psiId);
                return ProcessarTemplate(template, dto);
                
            }
            return null;
        }

        public string ProcessarTemplate(TemplateHTMLDTO template, object dto)
        {
            string templateHTML = null;
            if (template != null)
            {
                var html = template.HTMLCompleto;
                var dataSource = template.FONTE_DADOS_TEMPLATE;

                if (!string.IsNullOrWhiteSpace(html) &&
                    dataSource != null &&
                    dataSource.FONTE_DADOS_DESCRICAO != null)
                {
                    templateHTML = ProcessarHTMLTemplate(dataSource.FONTE_DADOS_DESCRICAO, html, dto);
                }
            }

            return templateHTML;
        }

        private string ProcessarHTMLTemplate(ICollection<FonteDadosDescricaoDTO> lstDescricao, string html, object dto)
        {
            if(lstDescricao != null && !string.IsNullOrWhiteSpace(html) && dto != null)
            {
                //Regex regexTr = new Regex(@"<tr[\s\S]*?<\/tr>");
                //Regex regexRepetirLinha = new Regex(@"\$\[\s*repetirLinha\s*=\s*""\s *.*\s * ""\]");

                //if(regexRepetirLinha.IsMatch(html))
                //{
                //    foreach (Match match in regexTr.Matches(html))
                //    {
                //        var valorAchado = match.Value;

                //        if (regexRepetirLinha.IsMatch(valorAchado))
                //        {
                //            Match subMatch = regexRepetirLinha.Match(valorAchado);
                //            if(subMatch.Groups.Count > 1)
                //            {
                //                var token = subMatch.Groups[1].Value;

                //                var path = lstDescricao
                //                .Where(x => x.DFD_TOKEN == token)
                //                .Select(x => x.DFD_PATH)
                //                .FirstOrDefault();
                //                var lstValue = GetDataSourceValue(path, dto);

                //                foreach(var value in lstValue)
                //                {
                //                    // Continuar implementação
                //                }
                //            }
                        
                //        }
                //    }
                //}

                //Realizando as checagens de 'If' padrão $(token)[ 'corpo' ]
                Regex regexIf = new Regex(@"\$\((.*)\)\[([^\]]*)]");

                if (regexIf.IsMatch(html))
                {
                    foreach(Match match in regexIf.Matches(html))
                    {
                        var valorAchado = match.Value;

                        if(match.Groups.Count > 1)
                        {
                            var token = match.Groups[1].Value;
                            token = new Regex(@"\<[^>]*\>").Replace(token, ""); // Limpa tags html
                            var path = lstDescricao
                                .Where(x => x.DFD_TOKEN == token)
                                .Select(x => x.DFD_PATH)
                                .FirstOrDefault();
                            var strValue = GetDataSourceValue(path, dto);

                            if(strValue != null && match.Groups.Count > 2)
                            {
                                var tokenSubstituir = match.Groups[2].Value;
                                html = html.Replace(valorAchado, tokenSubstituir);
                            }
                            else
                            {
                                html = html.Replace(valorAchado, "");
                            }
                        }
                    }
                }

                foreach (var dadosDesc in lstDescricao)
                {
                    var strValue = GetDataSourceValue(dadosDesc.DFD_PATH, dto);
                    html = html.Replace("{{" + dadosDesc.DFD_TOKEN + "}}", strValue);
                }
                Regex regex = new Regex(@"\{\{[^{{}}]*\}\}");
                html = regex.Replace(html, "");
                return html;
            }
            
            return "";
        }
    }
}
