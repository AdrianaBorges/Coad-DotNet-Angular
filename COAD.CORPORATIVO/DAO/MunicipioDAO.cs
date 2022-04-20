using Coad.GenericCrud.Repositorios.Base;
using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.Model;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Custons.WebService.ClienteIntegracao;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.DAO
{
    public class MunicipioDAO : DAOAdapter<MUNICIPIO, MunicipioDTO, int>
    {
        public COADCORPEntities db { get { return GetDb<COADCORPEntities>(); } set { } }

        public MunicipioDAO()
        {
            db = GetDb<COADCORPEntities>();
        }

        public List<MunicipioDTO> Buscar(string _mun_descricao)
        {
            List<MunicipioDTO> _municipio = new List<MunicipioDTO>();

            var _dados = (from m in db.MUNICIPIO
                          where (m.MUN_DESCRICAO.Contains(_mun_descricao))
                          select new MunicipioDTO()
                          {
                              MUN_ID = m.MUN_ID,
                              MUN_DESCRICAO = m.MUN_DESCRICAO,
                              MUN_TIPO = m.MUN_TIPO,
                              UF = m.UF,
                              IBGE_COD_COMPLETO = m.IBGE_COD_COMPLETO,
                              MUN_CEP = m.MUN_CEP
                          }).ToList();


            if (_dados != null)
                _municipio = (List<MunicipioDTO>)_dados;

            return _municipio;
        }
        public MunicipioDTO BuscarPorUFDescricao(string _mun_descricao, string _mun_uf)
        {
            var _municipio = (from m in db.MUNICIPIO
                              where (m.MUN_DESCRICAO == _mun_descricao) &&
                                    (m.UF == _mun_uf)
                              select new MunicipioDTO()
                              {
                                  MUN_ID = m.MUN_ID,
                                  MUN_DESCRICAO = m.MUN_DESCRICAO,
                                  MUN_TIPO = m.MUN_TIPO,
                                  UF = m.UF,
                                  IBGE_COD_COMPLETO = m.IBGE_COD_COMPLETO,
                                  MUN_CEP = m.MUN_CEP
                              }).FirstOrDefault();


            return _municipio;
        }
        public MunicipioDTO BuscarPorIBGE(string _ibge_codigo)
        {
            var _municipio = (from m in db.MUNICIPIO
                              where (m.IBGE_COD_COMPLETO == _ibge_codigo)
                              select new MunicipioDTO()
                              {
                                  MUN_ID = m.MUN_ID,
                                  MUN_DESCRICAO = m.MUN_DESCRICAO,
                                  MUN_TIPO = m.MUN_TIPO,
                                  UF = m.UF,
                                  IBGE_COD_COMPLETO = m.IBGE_COD_COMPLETO,
                                  MUN_CEP = m.MUN_CEP
                              }).FirstOrDefault();


            return _municipio;
        }

        public IList<MunicipioDTO> BuscarPorUF(string uf)
        {
            var query = GetDbSet().Where(op => op.UF == uf);
            return ToDTO(query);
        }

        /// <summary>
        /// Busca por descrição e opcionalmente uf
        /// </summary>
        /// <param name="descricao"></param>
        /// <param name="uf"></param>
        /// <returns></returns>
        public IList<MunicipioDTO> BuscarPorDescricaoEUF(string descricao, string uf = null)
        {
            if (string.IsNullOrWhiteSpace(uf))
                uf = null;

            var query = GetDbSet().Where(op => (uf == null || op.UF == uf) && op.MUN_DESCRICAO == descricao);
            return ToDTO(query);
        }

        public IList<MunicipioDTO> ListMunicipioPorRegiao(int? rgId)
        {
            var query = GetDbSet().Where(x => x.RG_ID == rgId);
            return ToDTO(query);
        }

        public IList<ClienteIntegrMunicipioDTO> BuscarMunicipiosIntegracao(string uf, string descricao = null)
        {
            var query = (from mun in db.MUNICIPIO
                         where mun.UF == uf &&
                             (descricao == null || mun.MUN_DESCRICAO.Contains(descricao))
                         select new ClienteIntegrMunicipioDTO() {                             
                             CodigoMunicipio = mun.MUN_ID,
                             DescricaoMunicipio = mun.MUN_DESCRICAO
                         });

            return query.ToList();
        }

        public ClienteIntegrMunicipioDTO BuscarMunicipiosIntegracaoPorCodIBGE(string codIBGE)
        {
            var query = (from mun in db.MUNICIPIO
                         where mun.IBGE_COD_COMPLETO == codIBGE
                         select new ClienteIntegrMunicipioDTO()
                         {
                             CodigoMunicipio = mun.MUN_ID,
                             DescricaoMunicipio = mun.MUN_DESCRICAO
                         });

            return query.FirstOrDefault();
        }

    }
}
