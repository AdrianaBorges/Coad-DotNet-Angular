using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Custons.Pesquisas;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.DAO
{
    public class ImportacaoSuspectDAO : DAOAdapter<IMPORTACAO_SUSPECT, ImportacaoSuspectDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public ImportacaoSuspectDAO()
        {
            db = GetDb<COADCORPEntities>();
        }

        public IList<ImportacaoSuspectDTO> ListarSuspectsNaoImportados(int? impID)
        {
            var query = (from imSusp in db.IMPORTACAO_SUSPECT
                         where
                            imSusp.IMP_ID == impID &&
                            (imSusp.IMS_ID == 1 || imSusp.IMS_ID == 2)
                         select imSusp);
            return ToDTO(query);
        }

        public Pagina<ImportacaoSuspectDTO> PesquisarImportacaoSuspects(PesquisaImportacaoSuspectDTO pesquisaImportacao)
        {
            string Nome = pesquisaImportacao.Nome;
            string CPF_CNPJ = pesquisaImportacao.CPF_CNPJ;
            string Telefone = pesquisaImportacao.Telefone;
            string Celular = pesquisaImportacao.Celular;
            string EMail = pesquisaImportacao.EMail;
            string UF = pesquisaImportacao.UF;
            string Bairro = pesquisaImportacao.Bairro;
            string Regiao = pesquisaImportacao.Regiao;
            int? ImsID = pesquisaImportacao.ImsID;
            int? ImpID = pesquisaImportacao.ImpID;

            var query = (from ips in db.IMPORTACAO_SUSPECT
                         where
                            (Nome == null || ips.IPS_NOME.Contains(Nome)) &&
                            (CPF_CNPJ == null || ips.IPS_CPF_CNPJ.Contains(CPF_CNPJ)) &&
                            (Telefone == null || ips.IPS_TELEFONE == Telefone) &&
                            (Celular == null || ips.IPS_CELULAR == Celular) &&
                            (EMail == null || ips.IPS_EMAIL == EMail) &&
                            (UF == null || ips.IPS_UF == UF) &&
                            (Bairro == null || ips.IPS_BAIRRO == Bairro) &&
                            (Regiao == null || ips.IPS_REGIAO == Regiao) &&
                            (ImsID == null || ips.IMS_ID == ImsID) &&
                            (ImpID == null || ips.IMP_ID == ImpID)
                         select ips);

            return ToDTOPage(query, pesquisaImportacao.pagina, pesquisaImportacao.registrosPorPagina);
        }

    }
}
