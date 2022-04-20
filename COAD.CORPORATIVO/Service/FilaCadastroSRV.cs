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
//using Coad.GenericCrud.Service.Base;
using Coad.GenericCrud.Repositorios.Base;
using GenericCrud.Config.DataAttributes;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.CORPORATIVO.Model.Dto.Custons;
using System.Transactions;
using GenericCrud.Service;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("FLC_ID")]
    public class FilaCadastroSRV : GenericService<FILA_CADASTRO, FilaCadastroDTO, int>
    {
        private FilaCadastroDAO _dao;

        public FilaCadastroSRV()
        {
            this._dao = new FilaCadastroDAO();
            this.Dao = this._dao;
        }

        public FilaCadastroSRV(FilaCadastroDAO _dao)
        {
            this._dao = _dao;
            this.Dao = _dao;
        }

        public int QuantidadeFilaCadastro(int regiaoId, DateTime dataFila)
        {
            return _dao.QuantidadeFilaCadastro(regiaoId, dataFila);
        }

        public int QuantidadeFilaCadastroImportacao(int regiaoId)
        {
            return _dao.QuantidadeFilaCadastroImportacao(regiaoId);
        }

        public FilaCadastroDTO FindByRepId(int regiaoId, DateTime dataFila, int? REP_ID)
        {
            return _dao.FindByRepId(regiaoId, dataFila, REP_ID);
        }

        public bool HasFilaCadastro(int regiaoId, DateTime dataFila, int? REP_ID)
        {
            return _dao.HasFilaCadastro(regiaoId, dataFila, REP_ID);
        }


        public void RegistrarFilaCadastro(RepresentanteDTO representante)
        {

            if (representante != null && representante.REP_ID != null)
            {
                var REP_ID = representante.REP_ID;
                var dataAtual = DateTime.Now;

                var lstCarteiraRepresentante = ServiceFactory.RetornarServico<CarteiraRepresentanteSRV>().ListarCarteiraRepresentante(representante.REP_ID);
                if(lstCarteiraRepresentante != null)
                {
                    foreach(var carRep in lstCarteiraRepresentante)
                    {

                        if (carRep.CARTEIRA != null && 
                            carRep.CARTEIRA.RG_ID != null &&
                            !HasFilaCadastro((int) carRep.CARTEIRA.RG_ID, dataAtual, REP_ID))
                        {
                            var regiaoId = (int)carRep.CARTEIRA.RG_ID;

                            var qtdFilaEncontrado = QuantidadeFilaCadastro(regiaoId, dataAtual);

                            FilaCadastroDTO filaNova = new FilaCadastroDTO()
                            {
                                FLC_DATA = dataAtual,
                                FLC_ORDEM = qtdFilaEncontrado,
                                REP_ID = REP_ID,
                                RG_ID = regiaoId
                            };

                            Save(filaNova);
                        }
                    }
                }
            }
        }

        public IList<FilaCadastroDTO> FindAllByRepId(int regiaoId, int REP_ID)
        {
            return _dao.FindAllByRepId(regiaoId, REP_ID);
        }

        public IList<FilaCadastroDTO> FindByRegiao(int? regiaoId, DateTime dataFila)
        {
            return _dao.FindByRegiao(regiaoId, dataFila);
        }

        public Pagina<FilaCadastroDTO> FindByRegiao(int? regiaoId, DateTime dataFila, string nomeRepresentante = null, int pagina = 1, int registrosPorPagina = 50)
        {
            return _dao.FindByRegiao(regiaoId, dataFila, nomeRepresentante, pagina, registrosPorPagina);
        }


        public IList<FilaCadastroDTO> FindByRegiao(int? regiaoId, DateTime? dataFila, int? EXCETO_REP_ID)
        {
            return _dao.FindByRegiao(regiaoId, dataFila, EXCETO_REP_ID);
        }

        public Pagina<FilaCadastroDTO> FindByData(DateTime dataFila, string nomeRepresentante, int pagina = 1, int registroPorPagina = 50, int? UEN_ID = 1)
        {
            return _dao.FindByData(dataFila, nomeRepresentante, pagina, registroPorPagina, UEN_ID);
        }

        public void MoverFila(int? regiaoId)
        {
            var data = DateTime.Now;
            if (regiaoId != null)
            {
                var lstFila = FindByRegiao(regiaoId, data);

                var index = 0;
                foreach (var fila in lstFila)
                {
                    if (index == 0)
                    {
                        fila.FLC_ORDEM = (lstFila.Count() - 1);
                    }
                    else
                    {
                        fila.FLC_ORDEM--;
                    }
                    index++;
                }
                MergeAll(lstFila, true);
            }
        }

        public RepresentanteDTO ObterRepresentantePorRodizio(int REGIAO_ID, DateTime data)
        {
            return _dao.ObterRepresentantePorRodizio(REGIAO_ID, data);
        }

        public RepresentanteDTO ObterRepresentantePorRodizio(int REGIAO_ID)
        {
            var data = DateTime.Now;
            return _dao.ObterRepresentantePorRodizio(REGIAO_ID, data);
        }

        public void RemoverFilaCadastro(int? regiaoId, int? repId)
        {
            if (regiaoId != null && repId != null)
            {
                var REGIAO_ID = (int)regiaoId;
                var REP_ID = (int)repId;

                IList<FilaCadastroDTO> lstFilaCadastro = FindAllByRepId(REP_ID);
                DeleteAll(lstFilaCadastro);
            }
        }

        public void RemoverDaFila(int? FLC_ID)
        {

            using (var scope = new TransactionScope())
            {
                FilaCadastroDTO fila = FindById(FLC_ID);
                var RG_ID = fila.RG_ID;
                var data = fila.FLC_DATA;

                Delete(fila);
                ReordenarFila(RG_ID, data);

                scope.Complete();
            }
        }


        public FilaCadastroDTO ObterFilaAnterior(int? FLC_ID)
        {
            return _dao.ObterFilaAnterior(FLC_ID);
        }

        public FilaCadastroDTO ObterFilaSuperior(int? FLC_ID)
        {
            return _dao.ObterFilaSuperior(FLC_ID);
        }

        public void MoverFilaParaCima(int? FLC_ID)
        {
            using (TransactionScope scope = new TransactionScope())
            {

                var fila = FindById(FLC_ID);

                if (fila != null && fila.FLC_ORDEM > 0)
                {
                    var RG_ID = fila.RG_ID;
                    var data = fila.FLC_DATA;
                    ReordenarFila(RG_ID, data);

                    fila = FindById(FLC_ID);
                    var filaSuperior = ObterFilaSuperior(FLC_ID);

                    if (filaSuperior != null)
                    {
                        fila.FLC_ORDEM--;
                        filaSuperior.FLC_ORDEM++;

                        Merge(fila);
                        Merge(filaSuperior);
                    }
                }
                scope.Complete();
            }

        }

        public void MoverFilaParaBaixo(int? FLC_ID)
        {
            using (TransactionScope scope = new TransactionScope())
            {

                var fila = FindById(FLC_ID);
                var RG_ID = fila.RG_ID;
                var data = fila.FLC_DATA;

                ReordenarFila(RG_ID, data);

                fila = FindById(FLC_ID);
                var filaSuperior = ObterFilaAnterior(FLC_ID);

                if (fila != null && filaSuperior != null)
                {
                    fila.FLC_ORDEM++;
                    filaSuperior.FLC_ORDEM--;

                    Merge(fila);
                    Merge(filaSuperior);
                }
                scope.Complete();
            }

        }

        public FilaCadastroDTO ObterFilaPorOrdem(int? regiaoId, int? FLC_ORDEM, DateTime? data)
        {
            return _dao.ObterFilaPorOrdem(regiaoId, FLC_ORDEM, data);
        }

        public IEnumerable<FilaCadastroDTO> FindAllByDataERegiao(DateTime? data, int? RG_ID)
        {
            return _dao.FindAllByDataERegiao(data, RG_ID);
        }

        public void ReordenarFila(int? regiaoId, DateTime? data)
        {
            var lstFila = FindAllByDataERegiao(data, regiaoId);

            int index = 0;
            foreach (var fila in lstFila)
            {

                fila.FLC_ORDEM = index;
                index++;
            }
            MergeAll(lstFila);
        }
        public IList<FilaCadastroDTO> FindAllByRepId(int REP_ID)
        {
            return _dao.FindAllByRepId(REP_ID);
        }

        public void MoverFilaDeImportacao(int? rgID)
        {
            if (rgID != null)
            {
                var lstFila = ListarFilaDeCadastroDaImportacao(rgID);

                var index = 0;
                foreach (var fila in lstFila)
                {
                    if (index == 0)
                    {
                        fila.FLC_ORDEM = (lstFila.Count() - 1);
                    }
                    else
                    {
                        fila.FLC_ORDEM--;
                    }
                    index++;
                }
                MergeAll(lstFila);
            }
        }

        public void RegistrarFilaCadastroImportacao(ICollection<CarteiraRepresentanteDTO> lstCarteiraRepresentante)
        {

            if (lstCarteiraRepresentante != null && lstCarteiraRepresentante.Count > 0)
            {
                var lstFilaCadastro = new HashSet<FilaCadastroDTO>();
                foreach(var carRep in lstCarteiraRepresentante)
                {
                    var repID = carRep.REP_ID;
                    var rgID = (int)carRep.CARTEIRA.RG_ID;
                    var dataAtual = DateTime.Now;

                    if (!HasFilaCadastroImportacao(rgID, repID))
                    {
                        var qtdFilaEncontrado = QuantidadeFilaCadastroImportacao(rgID);

                        FilaCadastroDTO fila = new FilaCadastroDTO()
                        {
                            FLC_DATA = dataAtual,
                            FLC_ORDEM = qtdFilaEncontrado,
                            REP_ID = repID,
                            RG_ID = rgID,
                            FLC_IMPORTACAO = true,                            
                        };
                        var filaSalva = Save(fila);
                    }
                }

                        
            }
        }
        public bool HasFilaCadastroImportacao(int rgID, int? repID)
        {
            return _dao.HasFilaCadastroImportacao(rgID, repID);
        }

        public IList<FilaCadastroDTO> ListarFilaDeCadastroDaImportacao(int? rgID)
        {
            return _dao.ListarFilaDeCadastroDaImportacao(rgID);
        }

        public RepresentanteDTO ObterRepresentanteDeImportacaoEOrganizarFila(int? rgId)
        {
            var lstCarteiraRepresentante = ServiceFactory
                .RetornarServico<CarteiraRepresentanteSRV>()
                .ListarCarteiraRepresentantePorRegiao(rgId);

            AtualizarFilaImportacao(lstCarteiraRepresentante);

            return ObterRepresentantePorRodizioDeImportacao(rgId);

        }


        public RepresentanteDTO ObterRepresentantePorRodizioDeImportacao(int? rgId)
        {
            return _dao.ObterRepresentantePorRodizioDeImportacao(rgId);
        }
        public void AtualizarFilaImportacao(ICollection<CarteiraRepresentanteDTO> lstCarteiraRepresentante)
        {
            if (lstCarteiraRepresentante != null)
            {
                IEnumerable<int?> lstRg = lstCarteiraRepresentante
                    .Select(x => x.CARTEIRA.RG_ID)
                    .Distinct();

                if (lstRg != null)
                {
                    foreach (var rg in lstRg)
                    {
                        ICollection<CarteiraRepresentanteDTO> lstRepresentanteDaRegiao = lstCarteiraRepresentante
                            .Where(x => x.CARTEIRA.RG_ID == rg)
                            .ToList();

                        var lstFilaDaRegiao = ListarFilaDeCadastroDaImportacao(rg);
                        var lstRepresentanteForaDaFila = lstRepresentanteDaRegiao.Where(x => !lstFilaDaRegiao.Select(sel => sel.REP_ID).Contains(x.REP_ID)).ToList();
                        var lstRepresentanteParRemoverDaFila = lstFilaDaRegiao.Where(x => !lstRepresentanteDaRegiao.Select(sel => (int?)sel.REP_ID).Contains(x.REP_ID));

                        RegistrarFilaCadastroImportacao(lstRepresentanteForaDaFila);
                        DeleteAll(lstRepresentanteParRemoverDaFila);
                        var lstFilaDaRegiaoParaOrganizar = ListarFilaDeCadastroDaImportacao(rg);

                        int index = 0;
                        foreach (var fila in lstFilaDaRegiaoParaOrganizar)
                        {

                            fila.FLC_ORDEM = index;
                            index++;
                        }
                        MergeAll(lstFilaDaRegiaoParaOrganizar);
                    }
                }
            }
        }
    }
}
