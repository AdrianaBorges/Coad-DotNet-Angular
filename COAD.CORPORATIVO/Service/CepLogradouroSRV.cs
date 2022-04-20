using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Custons.WebService.ClienteIntegracao;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Service
{
    public class CepLogradouroSRV : GenericService<CEP_LOGRADOURO, CepLogradouroDTO, int>
    {
        public CepLogradouroDAO _dao { get; set; }
        public MunicipioSRV _municipio { get; set; }

        public CepLogradouroSRV()
        {
            this._dao = new CepLogradouroDAO();
            this.Dao = new CepLogradouroDAO();
            this._municipio = new MunicipioSRV();
        }

        public CepLogradouroSRV(CepLogradouroDAO _dao)
        {
            this.Dao = _dao;
        }

        public CepLogradouroDTO BuscarCep(string _cep_numero)
        {
            return _dao.BuscarCep(_cep_numero);
        }
        public Pagina<CepLogradouroDTO> BuscarCep(string _cep_numero, string _cep_log, int pagina = 1, int registroPorPagina = 10)
        {
            return _dao.BuscarCep(_cep_numero, _cep_log, pagina, registroPorPagina);
        }
        public int BuscarUltimoID()
        {
            return _dao.BuscarUltimoID();
        }

        public ClienteIntegrEnderecoDTO BuscarCepIntegracao(string cep)
        {
            var cepLog = BuscarCep(cep);

            if (cepLog != null)
            {

                var municipio = _municipio.RetornarMunicipioIntegracao(cepLog.MUN_ID);
                var endIntegr = new ClienteIntegrEnderecoDTO()
                {
                    Bairro = (cepLog.CEP_BAIRRO != null) ? cepLog.CEP_BAIRRO.BAR_DESCRICAO : null,
                    CEP = cepLog.CEP_NUMERO,
                    Logradouro = cepLog.CEP_LOG,
                    Municipio = municipio,
                    UF = cepLog.CEP_UF
                };

                return endIntegr;
            }

            return null;
        }
    }

}
