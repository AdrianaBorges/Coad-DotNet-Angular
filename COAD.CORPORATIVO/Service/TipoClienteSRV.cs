using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Model.Dto;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.Model.Dto.Custons.WebService.ClienteIntegracao;

namespace COAD.CORPORATIVO.Service
{
    public class TipoClienteSRV : ServiceAdapter<TIPO_CLIENTE, TipoClienteDTO, int>
    {
        private TipoClienteDAO _dao { get; set; }

        public TipoClienteSRV()
        {
            _dao = new TipoClienteDAO();
            SetDao(_dao);
        }

        public TipoClienteSRV(TipoClienteDAO _db)
        {
            this._dao = _db;
            SetDao(_db);
        }

        public IList<TipoClienteDTO> BuscarTiposDeClientesAtivos()
        {
            return _dao.BuscarTipoClienteAtivos();
        }

        public IList<ClienteIntegrTipoClienteDTO> BuscarTiposDeClienteIntegracao()
        {
            return _dao.BuscarTiposDeClienteIntegracao();
        }

        public ClienteIntegrTipoClienteDTO RetornarTipoClienteIntegracao(int? tpCliId)
        {
            var tpCliente = FindById(tpCliId);

            if (tpCliente != null)
            {
                var tpIntegracao = new ClienteIntegrTipoClienteDTO()
                {
                    CodigoTipoCliente = tpCliente.TIPO_CLI_ID,
                    DescricaoTipoCliente = tpCliente.TIPO_CLI_DESCRICAO
                };

                return tpIntegracao;
            }

            return null;
        }

        public IList<TipoClienteDTO> ListarTipoClienteEcommerce()
        {
            return _dao.ListarTipoClienteEcommerce();
        }
    }
}
