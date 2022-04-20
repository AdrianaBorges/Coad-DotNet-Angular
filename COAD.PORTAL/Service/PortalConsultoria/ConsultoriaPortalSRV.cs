using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Service.Base;
using COAD.PORTAL.DAO.PortalConsultoria;
using COAD.PORTAL.Model.DTO.PortalConsultoria;
using COAD.PORTAL.Repositorios.Contexto;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.SEGURANCA.Service;
using COAD.SEGURANCA.Service.Interfaces;
using GenericCrud.Service;
using GenericCrud.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace COAD.PORTAL.Service.PortalConsultoria
{
    public class ConsultoriaPortalSRV : GenericService<consultoria, ConsultoriaPortalDTO, int>
    {
        public ConsultoriaDAO _dao = new ConsultoriaDAO();

        public ConsultoriaPortalSRV()
        {
            //Dao = _dao;
            SetKeys("id");
            this.Dao = _dao;
        }

        public Pagina<ConsultoriaPortalDTO> BuscarConsultasPorPerfil(int id, string codigo, string status, string perini, string perfim, string uf, int colecionador, int pagina = 1, int registroPorPagina = 10)
        {
            /*ColecionadoresConsultaEmailPortalSRV ccep = new ColecionadoresConsultaEmailPortalSRV();
            var colecionador = ccep.BuscarColecionadorPorPerfil(autenticado.PER_ID);

            if (colecionador == null)
                throw new NotImplementedException("Perfil sem permissão de visualização de consultas!");*/

            DateTime periodoInicial;
            DateTime periodoFinal;
            string periodoInicialEditado = (perini.Length == 10) ? perini : "";
            string periodoFinalEditado = (perfim.Length == 10) ? perfim : "";
            if (!DateTime.TryParse(periodoInicialEditado, out periodoInicial)) {
                periodoInicial = DateTime.Parse("01-01-2000") ;
            }
            if (!DateTime.TryParse(periodoFinalEditado, out periodoFinal)) {
                periodoFinal = DateTime.Now;
            }

            return _dao.BuscarConsultasPorPerfil(id, codigo, status, periodoInicial, periodoFinal, uf, colecionador.ToString(), pagina, registroPorPagina);
        }

        public void SalvarEnviarConsultasPorEmail(ConsultoresPortalDTO consultor, ConsultoriaPortalDTO consulta, string _resposta)
        {
            try
            {
                if(String.IsNullOrEmpty(_resposta) || _resposta.Length < 10)
                    throw new NotImplementedException("Sua resposta está em branco ou não atinge o mínimo de caracteres desejado.");
                consulta.status = "5";
                consulta.dataRespSupervisor = DateTime.Now;
                consulta.cod_supervisor = consultor.id;
                consulta.resposta_supervisor = _resposta;
                consulta.dataUltimoAcesso = DateTime.Now;
                consulta.codFuncUltimoAcesso = consultor.id.ToString();
                EnviarRespostaConsultoriaEmail(consulta);
                Merge(consulta);
            }
            catch(Exception e)
            {
                throw new NotImplementedException("Erro ao salvar e enviar sua resposta.<br />Erro: "+e.Message);
            }
        }

        public void EnviarRespostaConsultoriaEmail(ConsultoriaPortalDTO consulta,
            HttpPostedFileBase arquivo = null)
        {
            var _templateHTMLSRV = ServiceFactory.RetornarServico<TemplateHTMLSRV>();
            var _fonteDadosDescricaoSRV = ServiceFactory.RetornarServico <FonteDadosDescricaoSRV>();
            try
            {
                if (consulta != null)
                {
                    string assunto = "Resposta Consultoria por email";
                    var template = _templateHTMLSRV.RetornarTemplatePorFuncionalidade(11);
                    var corpoEmail = _templateHTMLSRV.ProcessarTemplate(template, consulta);

                    //var email = SysUtils.DecidirEnderecoDeEmail(consulta.email);
                    var email = "jpereira@coad.com.br";
                    var emailSRV = ServiceFactory.RetornarServico<IEmailSRV>();


                    emailSRV.EnviarEmail(new EmailRequestDTO()
                    {
                        Assunto = assunto,
                        CorpoEmail = corpoEmail,
                        EmailDestino = email,
                        codSMTP = 6
                    });

                }
                else
                {
                    throw new NotImplementedException("O objeto consulta não está preenchido.");
                }
            }
            catch (Exception e)
            {
                throw new NotImplementedException("Não é possível salvar a caonsulta e enviar o E-Mail.", e);
            }
        }
    }
}
