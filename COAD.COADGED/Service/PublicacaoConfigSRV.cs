using Coad.GenericCrud.Dao.Base;
using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Exceptions;
using Coad.GenericCrud.Service.Base;
using COAD.COADGED.DAO;
using COAD.COADGED.Model.DTO;
using COAD.COADGED.Repositorios.Contexto;
using COAD.SEGURANCA.Repositorios.Base;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COAD.COADGED.Service
{
    // ALT: 22/06/2015 15h45m
    // Redação e Editoração...

    public class PublicacaoConfigSRV : GenericService<PUBLICACAO_CONFIG, PublicacaoConfigDTO, int>
    {
        private PublicacaoConfigDAO _dao = new PublicacaoConfigDAO();

        public PublicacaoConfigSRV()
        {
            Dao = _dao;
        }

        public Pagina<PublicacaoConfigDTO> PublicacaoConfig(int? publicacaoId, int? areaId, int pagina = 1, int itensPorPagina = 10)
        {
            var resp = _dao.PublicacaoConfig(publicacaoId, areaId);
            return resp;
        }

        public void SalvarPublicacaoConfig(PublicacaoConfigDTO publicacaoConfig)
        {
            try
            {
                if (publicacaoConfig.PUB_ID != null && publicacaoConfig.ARE_CONS_ID != null)
                {
                    var chave = _dao.PublicacaoConfig(publicacaoConfig.PUB_ID, publicacaoConfig.ARE_CONS_ID).lista.FirstOrDefault();

                    publicacaoConfig.PCF_ID = (chave != null) ? chave.PCF_ID : null;

                    if (publicacaoConfig.PCF_ID != null)
                    {
                        Merge(publicacaoConfig, "PCF_ID");
                    }
                    else
                    {
                        Save(publicacaoConfig);
                    }
                }
            }
            catch (DbEntityValidationException dbEx)
            {
                var _erro = new FormattedDbEntityValidationException(dbEx);

                SysException.RegistrarLog(_erro.Message, "", SessionContext.autenticado);

                throw _erro;
            }
            catch (Exception ex)
            {
                throw new Exception(SysException.Show(ex));
            }
        }
    }
}
