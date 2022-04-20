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
    //

    public class PublicacaoEditadaSRV : GenericService<PUBLICACAO_EDITADA, PublicacaoEditadaDTO, int>
    {
        private PublicacaoEditadaDAO _dao = new PublicacaoEditadaDAO();

        public PublicacaoEditadaSRV()
        {
            Dao = _dao;
        }

        public PublicacaoEditadaDTO BuscarQuemEstaEditando(int pub_id)
        {
            return _dao.BuscarSomenteLeitura(pub_id).FirstOrDefault();
        }

        public Pagina<PublicacaoEditadaDTO> Buscar(int? pub_id = null, string usuario = null, DateTime? acessouAposHora = null, DateTime? liberouAposHora = null, int pagina = 1, int itensPorPagina = 10)
        {
            var resp = _dao.Buscar(pub_id, usuario, acessouAposHora, liberouAposHora, pagina, itensPorPagina);
            return resp;
        }

        public IList<PublicacaoEditadaDTO> BuscarPublicacaoSendoEditada(int pub_id)
        {
            return _dao.BuscarPublicacaoSendoEditada(pub_id, SessionContext.autenticado.USU_LOGIN);
        }

        public bool BuscarSomenteLeitura(int pub_id)
        {
            var resp = _dao.BuscarSomenteLeitura(pub_id);
            if (resp == null || resp.Count() == 0)
                return false;
            else
                return true;
        }

        public void RegistrarEdicaoMateria(int pub_id, bool lSomenteLeitura)
        {
            PublicacaoEditadaDTO publicacaoEditada = new PublicacaoEditadaDTO();

            publicacaoEditada.PUB_ID = pub_id;
            publicacaoEditada.USU_LOGIN = SessionContext.autenticado.USU_LOGIN;
            publicacaoEditada.EDT_HORARIO = DateTime.Now;
            publicacaoEditada.EDT_EDITANDO = !lSomenteLeitura;

            this.SalvarPublicacaoEditada(publicacaoEditada);
        }

        public PublicacaoEditadaDTO SalvarPublicacaoEditada(PublicacaoEditadaDTO publicacaoEditada)
        {
            try
            {
                if (publicacaoEditada.PUB_ID == null)
                    throw new Exception("ID da Publicação não informado!");

                if (publicacaoEditada.EDT_ID != null)
                {
                    return Merge(publicacaoEditada, "EDT_ID");
                }
                else
                {
                    return Save(publicacaoEditada);
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
