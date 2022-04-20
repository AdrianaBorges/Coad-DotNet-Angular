using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Exceptions;
using Coad.GenericCrud.Service.Base;
using COAD.COADGED.DAO;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Repositorios.Contexto;
using COAD.CORPORATIVO.Model.Dto;
using COAD.SEGURANCA.Repositorios.Base;
using GenericCrud.Config.DataAttributes;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Service
{
    // ALT: 22/06/2015 15h45m
    // Redação e Editoração...

    [ServiceConfig("TIT_ID")]
    public class TitulacaoSRV : GenericService<TITULACAO, TitulacaoDTO, int>
    {
        private TitulacaoDAO _dao = new TitulacaoDAO();

        public TitulacaoSRV()
        {
            Dao = _dao;
        }

        public Pagina<TitulacaoDTO> Gg(int? colecionadorId, string[] ufs = null)
        {
            var gg = _dao.Gg(colecionadorId, ufs);
            return gg;
        }

        public Pagina<TitulacaoDTO> Verbetes(int? ggId)
        { 
            var vb = _dao.Verbetes(ggId);
            return vb;
        }

        public Pagina<TitulacaoDTO> Subverbetes(int? vbId)
        {
            var svb = _dao.Subverbetes(vbId);
            return svb;
        }

        public Pagina<TitulacaoDTO> Titulacoes(int? titulacaoId = null, int? areaId = null, string descricao = null, int? ativo = 1, string tipo = null, int? superiorId = null, string uf = null, bool nomeExato = false, int pagina = 1, int itensPorPagina = 10000)
        {
            var resp = _dao.Titulacoes(titulacaoId, areaId, descricao, ativo, tipo, superiorId, uf, nomeExato, pagina, itensPorPagina);
            return resp;
        }

        public Pagina<TitulacaoDTO> FiltrarTitulacoes(int? areaId, int? ggId, int? vbId, int pagina = 1, int itensPorPagina = 10)
        {
            var resp = _dao.FiltrarTitulacoes(areaId, ggId, vbId, pagina, itensPorPagina);
            return resp;
        }

        public Pagina<TitulacaoDTO> TitulacoesInferiores( string tipo )
        {
            var resp = _dao.TitulacoesInferiores( tipo );
            return resp;
        }

        public TitulacaoDTO SalvarTitulacao(TitulacaoDTO titulacao, bool checarExistencia = true)
        {
            TitulacaoDTO retorno = new TitulacaoDTO();
            try
            {
                if (checarExistencia && titulacao.TIT_ID == null && _dao.Titulacoes(null, titulacao.ARE_CONS_ID, titulacao.TIT_DESCRICAO, titulacao.TIT_ATIVO, titulacao.TIT_TIPO, null, titulacao.UF_ID).lista.Count() > 0)
                {
                    throw new Exception("Já existe uma titulação cadastrada com este nome!");
                }
                else
                {
                    titulacao.TIT_ATIVO = titulacao.TIT_ATIVO == null ? 1 : titulacao.TIT_ATIVO;

                    if (titulacao.TIT_ID != null)
                    {
                        retorno = Merge(titulacao, "TIT_ID");
                    }
                    else
                    {
                        retorno = Save(titulacao);
                    }
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                var _erro = new FormattedDbEntityValidationException(dbEx);

                SysException.RegistrarLog(_erro.Message, "", SessionContext.autenticado);

                throw _erro;
            }
            catch (Exception ex)
            {
                throw new Exception(SysException.Show(ex));
            }
            return retorno;
        }

        public IList<TitulacaoDTO> ListarGrandeGrupo(int? colecionadorId = null, string nome = null)
        {
            return _dao.ListarGrandeGrupo(colecionadorId, nome);
        }
        
    }
}
