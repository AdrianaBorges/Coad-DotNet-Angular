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

    public class PublicacaoPalavraChaveSRV : GenericService<PUBLICACAO_PALAVRA_CHAVE, PublicacaoPalavraChaveDTO, int>
    {
        private PublicacaoPalavraChaveDAO _dao = new PublicacaoPalavraChaveDAO();

        public PublicacaoPalavraChaveSRV()
        {
            Dao = _dao;
        }

        public Pagina<PublicacaoPalavraChaveDTO> PublicacaoPalavraChave(int? publicacaoId, int? areaId, int pagina = 1, int itensPorPagina = 10)
        {
            var resp = _dao.PublicacaoPalavraChave(publicacaoId, areaId);
            return resp;
        }

        public void SalvarPublicacaoPalavraChave(ICollection<PublicacaoPalavraChaveDTO> publicacaoPalavraChave)
        {
            try
            {
                if (publicacaoPalavraChave.Count > 0)
                {
                    // eliminando anteriores...
                    var eliminar = this.PublicacaoPalavraChave(publicacaoPalavraChave.First().PUB_ID, publicacaoPalavraChave.First().ARE_CONS_ID).lista;
                    DeleteAll(eliminar, "PPC_ID");

                    // salvando atuais...
                    foreach (var s in publicacaoPalavraChave)
                    {
                        if (s.PUB_ID != null && s.ARE_CONS_ID != null && s.PPC_TEXTO != null)
                        {
                            Save(s);
                        }
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

        private string First<T1>(ICollection<PublicacaoPalavraChaveDTO> publicacaoPalavraChave)
        {
            throw new NotImplementedException();
        }
    }
}
