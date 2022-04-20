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
using GenericCrud.Service;
using COAD.CORPORATIVO.Model.Dto.Custons.Buscas;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("CAR_ID", "CLI_ID")]
    public class CarteiraClienteSRV : GenericService<CARTEIRA_CLIENTE, CarteiraClienteDTO, object>
    {
        public CarteiraClienteDAO _dao = new CarteiraClienteDAO();

        public CarteiraClienteSRV()
        {
            this.Dao = _dao;
        }

        public bool HasCarteiraCliente(string CAR_ID, int CLI_ID)
        {
            return _dao.HasCarteiraCliente(CAR_ID, CLI_ID);
        }

        public void SalvarCarteiraCliente(IList<CarteiraClienteDTO> lstCarteira)
        {
            if (lstCarteira != null)
            {
                SaveOrUpdateNonIdentityKeyEntity(lstCarteira, "HasCarteiraCliente");
            }
        }

        public void SalvarCarteiraCliente(CarteiraClienteDTO carteira)
        {
            SalvarCarteiraCliente(new List<CarteiraClienteDTO>() { carteira });
        }

        public CarteiraClienteDTO CriarESalvarCarteiraCliente(CarteiraDTO carteira, ClienteDto cliente, bool origemProspect = false)
        {
            if (carteira != null && cliente != null)
            {
                var carteiraCliente = new CarteiraClienteDTO()
                {
                    CAR_ID = carteira.CAR_ID,
                    CLI_ID = cliente.CLI_ID,
                    DATA_ASSOCIACAO = DateTime.Now,
                    CCL_ORIGEM_PROSPECT = origemProspect
                };

                SalvarCarteiraCliente(carteiraCliente);
                return carteiraCliente;
            }

            return null;
        }

        public IList<CarteiraClienteDTO> FindByCarId(string CAR_ID)
        {
            return _dao.FindByCarId(CAR_ID);
        }

        public void PreencherCarteiraAssinatura(CarteiraDTO carteira)
        {
            if (carteira != null && !string.IsNullOrWhiteSpace(carteira.CAR_ID))
            {
                carteira.CARTEIRA_CLIENTE = FindByCarId(carteira.CAR_ID);
            }
        }

        public void RemoverCarteiraAssinaturaFromAssinatura(AssinaturaDTO assinatura, string CAR_ID)
        {
            if (assinatura != null && !string.IsNullOrWhiteSpace(CAR_ID))
            {
                var carteiraAssinatura = FindById(CAR_ID, assinatura.ASN_NUM_ASSINATURA);

                if (carteiraAssinatura != null)
                {
                    Delete(carteiraAssinatura);
                }
            }
        }

        public IList<CarteiraClienteDTO> FindByClienteERegiao(int? CLI_ID, int? RG_ID)
        {
            return _dao.FindByClienteERegiao(CLI_ID, RG_ID);
        }

        public IList<CarteiraClienteDTO> FindByCliente(int? CLI_ID)
        {
            return _dao.FindByCliente(CLI_ID);
        }

        public void RemoverCarteiraClientePorClienteERegiao(ClienteDto cliente, int? RG_ID)
        {
            if (cliente != null && cliente.CLI_ID != null && RG_ID != null)
            {
                var carteiraCliente = FindByClienteERegiao(cliente.CLI_ID, RG_ID);

                if (carteiraCliente != null)
                {
                    DeleteAll(carteiraCliente);
                }
            }
        }

        public void PreencherCarteiramento(CarteiraDTO carteira, ClienteDto cliente, ICollection<CarteiraClienteDTO> lstCarteiraCliente)
        {
            if (carteira != null && cliente != null && lstCarteiraCliente != null)
            {
                var carteiraCliente = new CarteiraClienteDTO()
                {
                    CAR_ID = carteira.CAR_ID,
                    CLI_ID = cliente.CLI_ID,
                    DATA_ASSOCIACAO = DateTime.Now
                };

                lstCarteiraCliente.Add(carteiraCliente);
            }
        }

        public void SalvarCarteiraClienteEmMassa(IEnumerable<CarteiraClienteDTO> lstCarteiraCliente)
        {
            SaveOrUpdateNonIdentityKeyEntity(lstCarteiraCliente, null, null, true);
        }

        public CarteiraClienteDTO RetornarCarteiraClienteDeProspect(int? cliId, string carIdParaExcluir = null)
        {
            return _dao.RetornarCarteiraClienteDeProspect(cliId, carIdParaExcluir);
        }

        public void PreencherCarteiraCliente(ClienteDto cliente)
        {
            if (cliente != null && cliente.CLI_ID != null)
            {
                var cliId = cliente.CLI_ID;
                var lstCarteiraCliente = FindByCliente(cliId);
                cliente.CARTEIRA_CLIENTE = lstCarteiraCliente;
            }
        }

        public void ChecarEExcluirCarteira(int? clidId, ICollection<CarteiraClienteDTO> lstCarteira)
        {
            if (clidId != null &&
                lstCarteira != null)
            {
                var lstCarteirasDoBanco = FindByCliente(clidId);
                var lstParaExcluir = GetMissinList(lstCarteirasDoBanco, lstCarteira);

                RemoverCarteiramentos(lstParaExcluir);
            }
        }
        public void SalvarEExcluirCarteiraCliente(ClienteDto cliente)
        {
            if (cliente != null && cliente.CARTEIRA_CLIENTE != null && cliente.CARTEIRA_CLIENTE.Count() > 0)
            {
                CheckAndAssignKeyFromParentToChildsList(cliente, cliente.CARTEIRA_CLIENTE, "CLI_ID");

                var lstParaDeletar = cliente.CARTEIRA_CLIENTE.Where(x => x.Deletar == true);
                var lstParaAtualizar = cliente.CARTEIRA_CLIENTE.Where(x => x.Deletar == false).ToList();

                RemoverCarteiramentos(lstParaDeletar);
                ChecarEExcluirCarteira(cliente.CLI_ID, lstParaAtualizar);

                SalvarCarteiraCliente(lstParaAtualizar);

                PreencherCarteiraCliente(cliente);
            }
        }

        public void RemoverCarteiramentos(IEnumerable<CarteiraClienteDTO> lstCarteiras)
        {
            var lstDeletar = new HashSet<CarteiraClienteDTO>();

            if (lstCarteiras != null)
            {
                foreach (var car in lstCarteiras)
                {
                    var carId = car.CAR_ID;
                    var cliId = car.CLI_ID;

                    if (cliId != null && !string.IsNullOrWhiteSpace(carId))
                    {
                        if (HasCarteiraCliente(carId, (int)cliId))
                        {
                            lstDeletar.Add(car);
                        }
                    }
                }

                DeleteAll(lstDeletar);
            }
        }

        /// <summary>
        /// Pega a primeira carteiraCliente com a flag 'CCL_ORIGEM_PROSPECT' marcado como true
        /// e deleta.
        /// </summary>
        /// <param name="cliId"></param>
        public void RemoverCarteiraDeProspect(int? cliId, string carIdParaExcluir)
        {
            CarteiraClienteDTO cartCliente = RetornarCarteiraClienteDeProspect(cliId, carIdParaExcluir);

            if (cartCliente != null)
            {
                Delete(cartCliente);
            }
        }

        public void ChecarECriarCarteiraCliente(int? cliID, string CAR_ID, int? repID)
        {
            var cliService = ServiceFactory.RetornarServico<ClienteSRV>();
            if(cliID != null && !string.IsNullOrWhiteSpace(CAR_ID) && repID != null)
            {
                if(!HasCarteiraCliente(CAR_ID, (int)cliID))
                {
                    var clienteEhDoRep = cliService.ChecarProspectDoRepresentante(cliID, repID);

                    if (clienteEhDoRep)
                    {
                        Save(new CarteiraClienteDTO()
                        {
                            CAR_ID = CAR_ID,
                            CLI_ID = cliID,
                            DATA_ASSOCIACAO = DateTime.Now,                            
                        });
                    }
                }
            }
        }

    }
}
