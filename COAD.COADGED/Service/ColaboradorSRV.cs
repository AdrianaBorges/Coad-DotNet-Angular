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

    public class ColaboradorSRV : GenericService<COLABORADOR, ColaboradorDTO, int>
    {
        private ColaboradorDAO _dao = new ColaboradorDAO();

        public ColaboradorSRV()
        {
            Dao = _dao;
        }

        public int? BuscarColecionadorDoColaborador(string nome) {
            int? resp = _dao.BuscarColecionadorDoColaborador(nome);
            return resp;
        }

        public Pagina<ColaboradorDTO> Colaboradores(int? colaboradorId, string nome = null, int ativo = 1, int? cargoId = null, int? colecionadorId = null, int pagina = 1, int itensPorPagina = 10)
        {
            var resp = _dao.Colaboradores(colaboradorId, nome, ativo, cargoId, colecionadorId, pagina, itensPorPagina);
            return resp;
        }

        public void SalvarColaborador(ColaboradorDTO colaborador)
        {
            try
            {
                if (colaborador.COL_ID == null && _dao.Colaboradores(null, colaborador.COL_NOME).lista.Count() > 0)
                {
                    throw new Exception("Já existe um colaborador cadastrado com este nome!");
                }
                else
                {
                    colaborador.COL_ATIVO = colaborador.COL_ATIVO == null ? 1 : colaborador.COL_ATIVO;
                    
                    if (colaborador.COL_ID != null)
                    {
                        colaborador.DATA_ALTERA = DateTime.Now;
                        Merge(colaborador, "COL_ID");
                    }
                    else
                    {
                        colaborador.DATA_CADASTRO = DateTime.Now;
                        Save(colaborador);
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
