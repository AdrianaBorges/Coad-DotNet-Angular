using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Repositorios.Base;
using GenericCrud.Config.DataAttributes;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.SEGURANCA.Service;
using GenericCrud.Config.DataAttributes.Maps;
using System.Transactions;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("RG_ID")]
    public class RegiaoSRV : GenericService<REGIAO, RegiaoDTO, int>
    {
        private RegiaoDAO _dao; // = new RegiaoDAO();

        [ServiceProperty("EMP_ID", Name = "empresa", PropertyName = "EMPRESA", FindById = true)]
        public EmpresaSRV _empresaSRV { get; set; }

        public UFSRV _ufSRV { get; set; }
        public MunicipioSRV _municipioSRV { get; set; }

        public RegiaoSRV(RegiaoDAO _dao)
        {
            this._dao = _dao;
            Dao = _dao;
        }

        public RegiaoSRV()
        {
            _dao = new RegiaoDAO();
            _empresaSRV = new EmpresaSRV();
            _municipioSRV = new MunicipioSRV();
            _ufSRV = new UFSRV();
            Dao = _dao;
        }

        public IList<RegiaoDTO> FindAll(int? REP_ID_PARA_EXCLUIR, int? uenId = 1)
        {
            if (REP_ID_PARA_EXCLUIR != null)
            {
                return _dao.FindAllByUen((int) REP_ID_PARA_EXCLUIR, uenId);
            }

            return FindAll();            
        }

        public IList<RegiaoDTO> FindAllByUen(int? uenId = 1)
        {
            return _dao.FindAllByUen(uenId);
        }
        /// <summary>
        /// Traz todas das regiões onde a operadora está encarteirada
        /// </summary>
        /// <param name="CLI_ID"></param>
        /// <param name="uenId"></param>
        /// <returns></returns>
        public IList<RegiaoDTO> FindRegioesDoCliente(int? CLI_ID, int? uenId = 1)
        {
            return _dao.FindRegioesDoCliente(CLI_ID, uenId);
        }

        /// <summary>
        /// Traz todas das regiões onde a operadora está encarteirada
        /// </summary>
        /// <param name="CLI_ID"></param>
        /// <param name="uenId"></param>
        /// <returns></returns>
        public bool ClientePossuiRegiao(int? CLI_ID, int? RG_ID, int? uenId = 1)
        {
            return _dao.ClientePossuiRegiao(CLI_ID, RG_ID, uenId);
        }

        public IList<RegiaoDTO> ListarRegioes(int? UEN_ID = 1)
        {
            return _dao.ListarRegioes(UEN_ID);
        }

        public int? ObterRgIdDoRepresentante(int? REP_ID)
        {
            return _dao.ObterRgIdDoRepresentante(REP_ID);
        }

        public Pagina<RegiaoDTO> ListarRegiao(string descricao, int? empId, string uf, int? munId, string empRazaoSocial = null, int pagina = 1, int registrosPorPagina = 8, int? uenId = null)
        {
            IList<int> listIds = null;

            if (!string.IsNullOrWhiteSpace(empRazaoSocial))
            {
                listIds = _empresaSRV.ListEmpIds(empRazaoSocial);            
            }

            var lstRegiao = _dao.ListarRegiao(descricao, empId, uf, munId, listIds, pagina, registrosPorPagina);
            
            GetAssociations(lstRegiao.lista, "empresa");

            return lstRegiao;
        }


        public RegiaoDTO FindByIdFullLoaded(int? rgId, bool carregaEmpresa = false, bool carregaUfEBairro = false)
        {
            var obj = FindById(rgId);

            if (carregaEmpresa)
            {
                GetAssociations(obj, "empresa");
            }

            if (carregaUfEBairro)
            {
                PreencherInformacoesDeUfECidade(obj);
            }

            return obj;
        }

        public void SalvarRegiao(RegiaoDTO regiao)
        {
            if (regiao != null)
            {
                using (var scope = new TransactionScope())
                {
                    var regiaoSalva = SaveOrUpdate(regiao);

                    if (regiao.RG_ID == null)
                    {
                        regiao.RG_ID = regiaoSalva.RG_ID;
                    }

                    _ufSRV.SalvarOuDessanexarUF(regiao);
                    _municipioSRV.SalvarOuDessanexarMunicipio(regiao);

                    scope.Complete();
                }
            }
        }

        public void PreencherInformacoesDeUfECidade(RegiaoDTO regiao)
        {
            if (regiao != null && regiao.RG_ID != null)
            {
                var rgId = regiao.RG_ID;
                var lstUf = _ufSRV.ListUfsPorRegiao(rgId);
                var lstMunicipio = _municipioSRV.ListMunicipioPorRegiao(rgId);

                regiao.UF = lstUf;
                regiao.MUNICIPIO = lstMunicipio;
            }
        }

        public RegiaoDTO EncontrarRegiaoPorNome(string descricaoRg, bool usarLike = false)
        {
            return _dao.EncontrarRegiaoPorNome(descricaoRg, usarLike);
        }

        public int? EncontrarRegiaoIdPorNome(string descricaoRg, bool usarLike = false)
        {
            RegiaoDTO rg = EncontrarRegiaoPorNome(descricaoRg, usarLike);

            if (rg != null)
            {
                return rg.RG_ID;
            }

            return null;
        }

        public IList<RegiaoDTO> ListarRegioesPorNome(string nome, int? uenId)
        {

            return _dao.ListarRegioesPorNome(nome, uenId);
        }

        public IList<RegiaoDTO> ListarRegioesCombo(int? uenId = null)
        {
            return _dao.ListarRegioesCombo(uenId);
        }

        public RegiaoDTO PesquisarRegiao(string cidade, string uf)
        {
            return _dao.PesquisarRegiao(cidade, uf);
        }

        public RegiaoDTO PesquisarRegiaoPorCidadeEEstado(string UF)
        {
            //if(!string.IsNullOrWhiteSpace(estado) && !string.IsNullOrWhiteSpace(cidade))
            //{
            //    string UF = estado;
            //    if(estado.Length > 2)
            //    {
            //        UF = _ufSRV.BuscarCodUFPorDescricao(estado);
            //    }
            //    else
            //    {
            //        UF = estado;
            //    }

            //    return PesquisarRegiao(cidade, UF);
            //}

            return PesquisarRegiao(null, UF);
        }
        
    }
}
