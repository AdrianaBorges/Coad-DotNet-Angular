using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
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
using COAD.CORPORATIVO.Exceptions;
using GenericCrud.Service;

namespace COAD.CORPORATIVO.Service
{

    [ServiceConfig("CAR_ID", "REP_ID")]
    public class CarteiraRepresentanteSRV : GenericService<CARTEIRA_REPRESENTANTE, CarteiraRepresentanteDTO, object>
    {
        public CarteiraRepresentanteDAO _dao = new CarteiraRepresentanteDAO();
        
        public CarteiraRepresentanteSRV()
        {
            Dao = _dao;
        }

        /// <summary>
        /// Pega as representantes da lista de carteira_representante, consulta e associa o nome do usuário associado a representante.
        /// </summary>
        /// <param name="carteirasRepresentantes"></param>
        public void PreencherOperadoraDaRepresentante(IEnumerable<CarteiraRepresentanteDTO> carteirasRepresentantes)
        {
            if (carteirasRepresentantes != null)
            {
                foreach (var carRep in carteirasRepresentantes)
                {
                    if (carRep.REPRESENTANTE != null)
                    {
                        //TODO: implementar quando necessário
                    }
                }
            }
        }

        /// <summary>
        /// Verifica se o encarteiramento já existe.
        /// </summary>
        /// <param name="CAR_ID"></param>
        /// <param name="REP_ID"></param>
        /// <returns></returns>
        public bool HasCarteirasRepresentantes(string CAR_ID, int REP_ID)
        {
            return _dao.HasCarteirasRepresentantes(CAR_ID, REP_ID);
        }

        /// <summary>
        /// Preenche as chaves estrangeiras da carteiraRepresentantes de acordo com
        /// o objeto Carteira e Representante
        /// </summary>
        /// <param name="carteirasRepresentantes"></param>
        public void AdicionarReferencias(IEnumerable<CarteiraRepresentanteDTO> carteirasRepresentantes)
        {
            if (carteirasRepresentantes != null)
            {
                foreach (var car in carteirasRepresentantes)
                {
                    if (car.CARTEIRA != null)
                        car.CAR_ID = car.CARTEIRA.CAR_ID;

                    if (car.REPRESENTANTE != null)
                        car.REP_ID = (int)car.REPRESENTANTE.REP_ID;
                }
            }

        }

        public void SalvarTodos(IEnumerable<CarteiraRepresentanteDTO> carteirasRepresentantes)
        {
            var lstSalvar = new List<CarteiraRepresentanteDTO>();
            //var lstAtualizar = new List<CarteiraRepresentanteDTO>();

            AdicionarReferencias(carteirasRepresentantes);

            if (carteirasRepresentantes != null)
            {
                foreach (var car in carteirasRepresentantes)
                {
                    if (!HasCarteirasRepresentantes(car.CAR_ID, (int) car.REP_ID))
                    {
                        lstSalvar.Add(car);
                        car.DATA_ASSOCIACAO = DateTime.Now;
                    }
                }
                SaveAll(lstSalvar);
               
            }

        }

        /// <summary>
        /// Pergunta se a carteira admite vários representantes, se não admite,
        /// verifica se a carteira já pertence a algum representante
        /// </summary>
        /// <param name="CAR_ID"></param>
        /// <param name="REP_ID_PARA_IGNORAR"></param>
        /// <returns></returns>
        public bool CarteiraPodeSerAdicionada(string CAR_ID, int REP_ID_PARA_IGNORAR)
        {
            return _dao.CarteiraPodeSerAdicionada(CAR_ID, REP_ID_PARA_IGNORAR);
        }

        public void TrocarCarteiraRepresentante(int REP_ID, string CAR_ID_ANTIGO, string CAR_ID_NOVO)
        {
            if (CarteiraPodeSerAdicionada(CAR_ID_NOVO, REP_ID))// verifica se pode ser adicionado
            {

                // excluo a carteira antiga
                DeletarCarteiraRepresentante(CAR_ID_ANTIGO, REP_ID);
                    
                if (!HasCarteirasRepresentantes(CAR_ID_NOVO, REP_ID))
                {
                    // crio uma carteira nova
                    CarteiraRepresentanteDTO car_rep = new CarteiraRepresentanteDTO()
                    {
                        CAR_ID = CAR_ID_NOVO,
                        REP_ID = REP_ID,
                        DATA_ASSOCIACAO = DateTime.Today
                    };
                    SaveOrUpdateNonIdentityKeyEntity(car_rep);
                }                         

            }
            else
            {
                var sb = new StringBuilder();
                sb.Append("Não é possível trocar a carteira.");
                sb.Append("A carteira já pertence a outro representante e ");
                sb.Append("essa carteira não está configurada para admitir multiplos representantes.");
                
                throw new CarteiramentoException(sb.ToString());
            }
        }

        /// <summary>
        /// Adiciona uma nova associação entre uma carteira representante.
        /// </summary>
        /// <param name="REP_ID"></param>
        /// <param name="CAR_ID"></param>
        public void AdicionarCarteiraRepresentante(int REP_ID, string CAR_ID)
        {
            if (CarteiraPodeSerAdicionada(CAR_ID, REP_ID))// verifica se pode ser adicionado
            {

                if (!HasCarteirasRepresentantes(CAR_ID, REP_ID))
                {
                    // crio uma carteira nova
                    CarteiraRepresentanteDTO car_rep = new CarteiraRepresentanteDTO()
                    {
                        CAR_ID = CAR_ID,
                        REP_ID = REP_ID,
                        DATA_ASSOCIACAO = DateTime.Today
                    };
                    SaveOrUpdateNonIdentityKeyEntity(car_rep);
                }                                

            }
            else
            {
                var sb = new StringBuilder();
                sb.Append("Não é possível trocar a carteira.");
                sb.Append("A carteira já pertence a outro representante e ");
                sb.Append("essa carteira não está configurada para admitir multiplos representantes.");

                throw new CarteiramentoException(sb.ToString());
            }
        }


        public void DeletarCarteiraRepresentante(string CAR_ID, int REP_ID)
        {
           var CAR_REP_ANTIGA = FindById(CAR_ID, REP_ID);
            Delete(CAR_REP_ANTIGA);
        }

        public IList<CarteiraRepresentanteDTO> FindByCarId(string CAR_ID)
        {
            return _dao.FindByCarId(CAR_ID);
        }

        public IList<CarteiraRepresentanteDTO> BuscarCarteiraAssinatura(string _asn_id)
        {
            return _dao.BuscarCarteiraAssinatura(_asn_id);
        }

        /// <summary>
        /// Pega o código do representante do corporativo contido na carteira representante.
        /// </summary>
        /// <param name="CAR_ID"></param>
        /// <param name="REP_ID"></param>
        /// <returns></returns>
        public string RepOperIdDaCarteirasRepresentantes(int? REP_ID, string CAR_ID)
        {
            return _dao.RepOperIdDaCarteirasRepresentantes(REP_ID, CAR_ID);
        }

        public ICollection<CarteiraRepresentanteDTO> ListarCarteiraRepresentante(int? repId)
        {
            return _dao.ListarCarteiraRepresentante(repId);
        }

        public void PreencherCarteiraRepresentante(RepresentanteDTO representante)
        {
            if(representante != null)
            {
                representante.CARTEIRA_REPRESENTANTE = ListarCarteiraRepresentante(representante.REP_ID);
            }
        }

        public void SalvarEExcluirCarteiraRepresentante(RepresentanteDTO representante)
        {
            if (representante != null && 
                representante.CARTEIRA_REPRESENTANTE != null && 
                representante.CARTEIRA_REPRESENTANTE.Count() > 0)
            {
                CheckAndAssignKeyFromParentToChildsList(representante, representante.CARTEIRA_REPRESENTANTE, "REP_ID");

                var lstParaDeletar = representante.CARTEIRA_REPRESENTANTE.Where(x => x.Deletar == true);
                var lstParaAtualizar = representante.CARTEIRA_REPRESENTANTE.Where(x => x.Deletar == false).ToList();

                RemoverCarteiramentos(lstParaDeletar);

                foreach(var car in lstParaAtualizar)
                {
                    var _carSrv = ServiceFactory.RetornarServico<CarteiramentoSRV>();
                    if (!_carSrv.ChecarCarteiraExiste(car.CAR_ID))
                    {
                        throw new CarteiramentoException(string.Format("A carteira '{0}' não existe.", car.CAR_ID));
                    }
                }
                SalvarCarteiraCliente(lstParaAtualizar);
                PreencherCarteiraRepresentante(representante);
            }
        }

        public void RemoverCarteiramentos(IEnumerable<CarteiraRepresentanteDTO> lstCarteiras)
        {
            var lstDeletar = new HashSet<CarteiraRepresentanteDTO>();

            if (lstCarteiras != null)
            {
                foreach (var car in lstCarteiras)
                {
                    var carId = car.CAR_ID;
                    var repId = car.REP_ID;

                    if (repId != null && !string.IsNullOrWhiteSpace(carId))
                    {
                        if (HasCarteirasRepresentantes(carId, (int)repId))
                        {
                            lstDeletar.Add(car);
                        }
                    }
                }

                DeleteAll(lstDeletar);
            }
        }

        public void SalvarCarteiraCliente(IList<CarteiraRepresentanteDTO> lstCarteira)
        {
            if (lstCarteira != null)
            {
                SaveOrUpdateNonIdentityKeyEntity(lstCarteira, "HasCarteirasRepresentantes");
            }
        }

        public IList<CarteiraRepresentanteDTO> ListarCarteiraRepresentantePorRegiao(int? rgId)
        {
            return _dao.ListarCarteiraRepresentantePorRegiao(rgId);
        }

    }
}
