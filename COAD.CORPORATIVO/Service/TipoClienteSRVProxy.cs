using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Service;
using COAD.CORPORATIVO.Model.Dto;

namespace COAD.CORPORATIVO.Service
{
    public class TipoDeClienteSRVProxy
    {
        private TipoClienteSRV _service;
        
        public TipoDeClienteSRVProxy()
        {
            _service = new TipoClienteSRV();
        }

        public TipoDeClienteSRVProxy(TipoClienteSRV _service)
        {
            this._service = _service;
        }

        public IList<TipoClienteDTO> ListarTipoClientes()
        {
            IList<TipoClienteDTO> listTiposClientes = _service.BuscarTiposDeClientesAtivos().ToList();
            return listTiposClientes;
        }

        public List<SelectListItem> RetornarTiposDeCliente(int identificadorTipo)
        {
            IList<TipoClienteDTO> tipocli = _service.BuscarTiposDeClientesAtivos().ToList();

            var tiposDeCliente = new List<SelectListItem>();

            foreach (var tipo in tipocli)
            {
                tiposDeCliente.Add(new SelectListItem() { Value = tipo.TIPO_CLI_ID.ToString(), Text = tipo.TIPO_CLI_DESCRICAO, Selected = (tipo.TIPO_CLI_ID == identificadorTipo) ? true : false });
            }

            return tiposDeCliente;
        }

        /// <summary>
        /// Retorna a lista de tipo de pessoa
        /// </summary>
        /// <returns></returns>
        public ISet<TipoPessoaDTO> ListarTipoPessoa()
        {
            ISet<TipoPessoaDTO> tiposPessoa = new HashSet<TipoPessoaDTO>()
            {
                new TipoPessoaDTO(){SIGLA = "J", DESCRICAO = "Jurídica"},
                new TipoPessoaDTO(){SIGLA = "F", DESCRICAO = "Física"},
            };

            return tiposPessoa;
        }
    }
}