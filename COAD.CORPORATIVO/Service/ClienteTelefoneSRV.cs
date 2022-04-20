using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model.Comparators;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Prospect;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.SEGURANCA.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Service
{
    public class ClienteTelefoneSRV : GenericService<CLIENTES_TELEFONE, ClienteTelefoneDTO, int>
    {
        private ClienteTelefoneDAO _dao = new ClienteTelefoneDAO();


        public ClienteTelefoneSRV()
        {
            Dao = _dao;
        }

        public IList<ClienteTelefoneDTO> FindAllByCliId(int CLI_ID)
        {
            return _dao.FindAllByCliId(CLI_ID);
        }

        public void PreecherClienteTelefone(ClienteDto cliente)
        {
            if (cliente != null && cliente.CLI_ID != null)
            {
                var cliId = cliente.CLI_ID;
                var lstTelefones = FindAllByCliId((int) cliId);
                cliente.CLIENTES_TELEFONE = lstTelefones;
            }
        }

        private void _preencherChaves(int CLI_ID, IQueryable<ClienteTelefoneDTO> telefones)
        {
            if (telefones != null)
            {
                foreach (var tel in telefones)
                {
                    tel.CLI_ID = CLI_ID;
                }

            }
        }

        /// <summary>
        /// Salva os telefones de um determinado prospect
        /// </summary>
        /// <param name="CLI_ID">Id inserido no telefone antes de salvar</param>
        /// <param name="telefones">Os telefones para serem salvos</param>
        /// <param name="atualizar">true = atualizar, false = incluir</param>
        public void SalvarTelefones(int CLI_ID, IQueryable<ClienteTelefoneDTO> telefones)
        {
            if (telefones != null)
            {
                foreach (var tel in telefones)
                {
                    if (tel.CLI_ID == null)
                        tel.CLI_ID = CLI_ID;                   
                }

                var telefonesParaSalvar = telefones.Where(op => op.CLI_TEL_ID == null);
                var telefonesParaAtualizar = telefones.Where(op => op.CLI_TEL_ID != null);

                MergeAll(telefonesParaAtualizar, "CLI_TEL_ID");                
                SaveAll(telefonesParaSalvar);                

            }
        }
        
        public void ExcluirClienteTelefone(ClienteDto cliente)
        {

            var CLI_ID = (int)cliente.CLI_ID;
                      
            if (cliente.CLIENTES_TELEFONE != null)
            {
                foreach (var tel in cliente.CLIENTES_TELEFONE)
                {
                    if(tel.CLI_ID == null)
                        tel.CLI_ID = CLI_ID;
                }

                var excecoes = cliente.CLIENTES_TELEFONE;
                var clienteOriginal = new ClienteSRV().FindByIdFullLoaded(CLI_ID, trazClienteTelefone: true);
                var clienteTelefone = clienteOriginal.CLIENTES_TELEFONE;

                if (clienteTelefone != null && excecoes != null)
                {
                    var clienteTelefonePraExcluir = clienteTelefone.Except(excecoes, new ClienteTelefoneComparator());

                    if (clienteTelefonePraExcluir != null && clienteTelefonePraExcluir.Count() > 0)
                    {
                        DeleteAll(clienteTelefonePraExcluir, "CLI_TEL_ID");
                    }

                }
            }
        }

        /// <summary>
        /// Atualiza os telefones e remove dos bancos os que foram excluidos da lista
        /// </summary>
        /// <param name="cliente"></param>
        public void SalvarEExcluirTelefones(ClienteDto cliente)
        {
            ExcluirClienteTelefone(cliente);
            var cliId = cliente.CLI_ID;
            var lstTelefone = cliente.CLIENTES_TELEFONE;

            if (lstTelefone != null)
            {
                SalvarTelefones((int)cliId, lstTelefone.AsQueryable());
            }            
        }

    }
}
