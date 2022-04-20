
using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Coad.GenericCrud.Extensions;
using COAD.CORPORATIVO.MapperConverters;

namespace COAD.CORPORATIVO.DAO
{
    public class CarteiraRepresentanteDAO : AbstractGenericDao<CARTEIRA_REPRESENTANTE, CarteiraRepresentanteDTO, object>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public CarteiraRepresentanteDAO()
        {
            this.db = GetDb<COADCORPEntities>(false);
        }

        public bool HasCarteirasRepresentantes(string CAR_ID, int REP_ID)
        {
            if (CAR_ID != null && REP_ID != null)
            {
                IQueryable<CARTEIRA_REPRESENTANTE> query = GetDbSet();
                var count = query.Where(op => op.CAR_ID == CAR_ID && op.REP_ID == REP_ID).Count();

                return (count > 0);
            }
            return false;
        }

        public bool CarteiraPodeSerAdicionada(string CAR_ID, int REP_ID_PARA_IGNORAR)
        {
            var count = GetDbSet().Where(x => x.CAR_ID == CAR_ID &&
                x.REP_ID != REP_ID_PARA_IGNORAR &&
                x.CARTEIRA.CAR_VARIOS_REPRESENTANTES != 1).Count();

            return (count == 0);
        }

        public IList<CarteiraRepresentanteDTO> FindByCarId(string CAR_ID)
        {
            var query = GetDbSet().Where(x => x.CAR_ID == CAR_ID);
            return ToDTO(query);
        }

        public IList<CarteiraRepresentanteDTO> BuscarCarteiraAssinatura(string _asn_id)
        {
            var query = (from a in db.CARTEIRA_ASSINATURA
                         join c in db.CARTEIRA on a.CAR_ID equals c.CAR_ID
                         where (a.ASN_NUM_ASSINATURA == _asn_id && c.AREA_ID == 1)
                         select a);

            IList<CarteiraRepresentanteDTO> _list = new List<CarteiraRepresentanteDTO>();

            foreach (var _item in query)
            {
                var _crep = db.CARTEIRA_REPRESENTANTE.Where(x => x.CAR_ID == _item.CAR_ID).FirstOrDefault();

                RepresentanteDTO _repres = new RepresentanteDTO();
                CarteiraRepresentanteDTO _rep = new CarteiraRepresentanteDTO();
                _rep.CAR_ID = _item.CAR_ID;

                if (_crep != null)
                {
                    _repres.REP_ID = _crep.REPRESENTANTE.REP_ID;
                    _repres.REP_NOME = _crep.REPRESENTANTE.REP_NOME;
                    //---------
                    _rep.REP_ID = (int)_repres.REP_ID;
                    _rep.REPRESENTANTE = _repres;

                }

                _list.Add(_rep);

            }
            return _list;
        }

        public string RepOperIdDaCarteirasRepresentantes(int? REP_ID, string CAR_ID)
        {
            if (CAR_ID != null && REP_ID != null)
            {
                var cartRepre = (from carRep in db.CARTEIRA_REPRESENTANTE
                                 where carRep.CAR_ID == CAR_ID && carRep.REP_ID == REP_ID
                                 select carRep.REP_OPER_ID)
                                 .FirstOrDefault();

                return cartRepre;
            }
            return null;
        }

        public ICollection<CarteiraRepresentanteDTO> ListarCarteiraRepresentante(int? repId)
        {
            var query = (from carRep in 
                             db.CARTEIRA_REPRESENTANTE
                         where carRep.REP_ID == repId
                         select carRep);
            return ToDTO(query);
        } 

        public IList<CarteiraRepresentanteDTO> ListarCarteiraRepresentantePorRegiao(int? rgId)
        {
            var query = (from
                            carRep in db.CARTEIRA_REPRESENTANTE join
                            rep in db.REPRESENTANTE on carRep.REP_ID equals rep.REP_ID join
                            car in db.CARTEIRA on carRep.CAR_ID equals car.CAR_ID
                          where
                              rep.UEN_ID == car.UEN_ID && 
                              car.RG_ID == rep.RG_ID && 
                              rep.NRP_ID == 4 &&
                              car.RG_ID == rgId &&
                              (rep.REP_INATIVO_RODIZIO_IMP == null || rep.REP_INATIVO_RODIZIO_IMP == false)
                          select carRep);

            return ToDTO(query);
        }
    }
}
