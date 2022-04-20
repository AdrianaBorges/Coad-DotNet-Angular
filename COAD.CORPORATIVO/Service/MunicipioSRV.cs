using Coad.GenericCrud.Service.Base;
using COAD.CORPORATIVO.DAO;
using COAD.CORPORATIVO.Model;
using COAD.CORPORATIVO.Model.Dto;
using COAD.CORPORATIVO.Model.Dto.Custons.WebService.ClienteIntegracao;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using GenericCrud.Config.DataAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.CORPORATIVO.Service
{
    [ServiceConfig("MUN_ID")]
    public class MunicipioSRV : ServiceAdapter<MUNICIPIO, MunicipioDTO, int>
    {
        private MunicipioDAO _dao = new MunicipioDAO();

        public MunicipioSRV()
        {
            SetDao(_dao);
        }

        public List<MunicipioDTO> Buscar(string _mun_descricao)
        {
            return _dao.Buscar(_mun_descricao);
        }
        public MunicipioDTO BuscarPorIBGE(string _ibge_codigo)
        {
            return _dao.BuscarPorIBGE(_ibge_codigo);
        }
        public MunicipioDTO BuscarPorUFDescricao(string _mun_descricao, string _mun_uf)
        {
            return _dao.BuscarPorUFDescricao(_mun_descricao, _mun_uf);
        }

        public IList<MunicipioDTO> BuscarPorDescricaoEUF(string _mun_descricao, string uf)
        {
            return _dao.BuscarPorDescricaoEUF(_mun_descricao, uf);
        }

        public IList<MunicipioDTO> BuscarPorUF(string uf)
        {
            return _dao.BuscarPorUF(uf);
        }


        public void DesanexarMunicipio(RegiaoDTO regiao)
        {
            var rgId = regiao.RG_ID;
            var regiaoDoBanco = new RegiaoSRV().FindByIdFullLoaded(rgId, false, true);
            var lstMunicipioParaDesanexar = GetMissinList(regiao, regiaoDoBanco, "MUNICIPIO");

            foreach (var mun in lstMunicipioParaDesanexar)
            {
                mun.RG_ID = null;
            }

            SaveOrUpdateAll(lstMunicipioParaDesanexar);
        }


        public void SalvarOuDessanexarMunicipio(RegiaoDTO regiao)
        {
            if (regiao != null)
            {
                DesanexarMunicipio(regiao);

                var lstMunicipio = regiao.MUNICIPIO;
                CheckAndAssignKeyFromParentToChildsList(regiao, lstMunicipio, "RG_ID");
                SaveOrUpdateAll(lstMunicipio);

            }
        }

        public IList<MunicipioDTO> ListMunicipioPorRegiao(int? rgId)
        {
            return _dao.ListMunicipioPorRegiao(rgId);
        }

        public int? RetornarMunIdPorDescricao(string descricaoMun, string uf)
        {
            MunicipioDTO mun = BuscarPorUFDescricao(descricaoMun, uf);

            if (mun != null)
            {
                return mun.MUN_ID;
            }

            return null;
        }

        public IList<ClienteIntegrMunicipioDTO> BuscarMunicipiosIntegracao(string uf, string descricao = null)
        {
            return _dao.BuscarMunicipiosIntegracao(uf, descricao);
        }

        public ClienteIntegrMunicipioDTO BuscarMunicipiosIntegracaoPorCodIBGE(string codIBGE)
        {
            return _dao.BuscarMunicipiosIntegracaoPorCodIBGE(codIBGE);
        }

        public ClienteIntegrMunicipioDTO RetornarMunicipioIntegracao(int? munId)
        {
            var municipio = FindById(munId);
            if (municipio != null)
            {
                var munInte = new ClienteIntegrMunicipioDTO() 
                {
                    CodigoIBGE = municipio.IBGE_COD_COMPLETO,
                    CodigoMunicipio = municipio.MUN_ID,
                    DescricaoMunicipio = municipio.MUN_DESCRICAO
                };

                return munInte;
            }

            return null;
        }
    }
}
