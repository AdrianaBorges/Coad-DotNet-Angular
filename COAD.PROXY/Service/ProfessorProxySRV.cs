using Coad.GenericCrud.Exceptions;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Service;
using COAD.PROXY.Model.DTO;
using COAD.SEGURANCA.Model;
using COAD.SEGURANCA.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace COAD.PROXY.Service
{
    public class ProfessorProxySRV : ProfessorSRV
    {
        private AreaConsultoriaRepresentanteProxySRV _areaConsultoriaRepresentanteProxySRV = new AreaConsultoriaRepresentanteProxySRV();
        private UsuarioSRV _usuarioSRV = new UsuarioSRV();
       
        
        public ProfessorProxyDTO FindByIdWithUser(int? REP_ID)
        {
            var professor = base.FindByIdWithUser(REP_ID, true);
            var professorProxy = ConvertWithProfile<RepresentanteDTO, ProfessorProxyDTO>(professor, "proxy");

            professorProxy.AREA_CONSULTORIA_REPRESENTANTE = professor.AREA_CONSULTORIA_REPRESENTANTE;
            professor.AREA_CONSULTORIA_REPRESENTANTE = null;

            _areaConsultoriaRepresentanteProxySRV.PreencherAreaConsultoriaRepresentante(professorProxy);

            return professorProxy;            
        }

        public void SalvarProfessorEUsuario(ProfessorProxyDTO professor, bool update, int? RG_ID = null, int? UEN_ID = 1)
        {

            RepresentanteDTO repNovo = null;
            SenhaDTO senha = null;
            UsuarioModel usuario = null;

            using (var scope = new TransactionScope())
            {
                professor.NRP_ID = 5;
                professor.NIVEL_REPRESENTANTE = new NivelRepresentanteSRV().FindById(5);
                                
                if (professor != null)
                {
                    usuario = professor.USUARIO;

                    if (usuario != null)
                    {
                        _usuarioSRV.ValidarUsuarioDisponivel(usuario);
                    }

                    if (professor.REP_ID == null)
                    {
                        professor.REP_OPER_ID = "0000";
                        professor.CAR_ID = "0";
                        professor.REP_ATIVO = 1;

                        if (UEN_ID != null)
                        {
                            professor.UEN_ID = UEN_ID;
                        }
                        else if (professor.UEN_ID == null)
                        {
                            throw new NegocioException("Não é possível criar o representante. Informações importantes não puderam ser obtidas automáticamente (uen).");
                        }

                        if (RG_ID != null)
                        {
                            professor.RG_ID = RG_ID;
                        }
                    }

                    repNovo = SaveOrUpdate(professor);

                    if (professor.REP_ID != null)
                    {
                        RemoverUsuarioNaoAssociado(professor);
                    }
                    else
                    {
                        professor.REP_ID = repNovo.REP_ID;
                    }

                    _areaConsultoriaRepresentanteProxySRV.SalvarEExcluirAreaConsultoriaRepresentante(professor);

                    if (usuario != null)
                    {
                        //usuario = representante.USUARIO;
                        usuario.REP_ID = repNovo.REP_ID;

                        string perId = null;

                        if (professor.NIVEL_REPRESENTANTE != null)
                        {
                            perId = professor.NIVEL_REPRESENTANTE.PER_ID;
                        }
                        senha = _usuarioSRV.SalvarUsuarioPorOutraAplicacao(usuario, update, true, perId);

                    }

                }

                scope.Complete();

                if (usuario != null && senha != null && senha.SenhaLiteral != null)
                {
                    _usuarioSRV.EnviaEmail(usuario.USU_EMAIL, usuario.USU_LOGIN, senha.SenhaLiteral);
                }

            }
        }

    }
}
