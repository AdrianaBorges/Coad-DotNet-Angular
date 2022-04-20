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

    public class InformativoSRV : GenericService<INFORMATIVO, InformativoDTO, object>
    {
        private InformativoDAO _dao = new InformativoDAO();

        public InformativoSRV()
        {
            Dao = _dao;
        }

        public Pagina<InformativoDTO> Informativos(int? informativoId=null, string ano = null, int? numero = null , int ativoId = 1, int pagina = 1, int itensPorPagina = 10)
        {
            var resp = _dao.Informativos(informativoId, ano, numero, ativoId, pagina, itensPorPagina);
            return resp;
        }

        public void IncluirInformativo(InformativoDTO informativo)
        {
            try
            {
                informativo.DATA_CADASTRO = DateTime.Now;
                Save(informativo);
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

        public void AlterarInformativo(InformativoDTO informativo)
        {
            try
            {
                if ((informativo.INF_ANO != null) && (informativo.INF_NUMERO != null))
                {
                    informativo.DATA_ALTERA = DateTime.Now;
                    Merge(informativo, "INF_ANO", "INF_NUMERO");
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
        
        /* Ler informativos ativos em produção */
        public Pagina<InformativoDTO> LerInformativosEmProducao(int pagina = 1, int itensPorPagina = 10)
        {
            Pagina<InformativoDTO> retorno = _dao.LerInformativosEmProducao(pagina, itensPorPagina);

            if (retorno.lista.Count() == 0)
            {
                throw new Exception("Não há Informativos em produção para serem concluídos!");
            }

            return retorno;
        }
        
        /* Buscar um informativo */
        public InformativoDTO BuscarInformativo(string ano, int numero)
        {
            return _dao.BuscarInformativo(ano, numero);
        }

        /* Salvar encerramento da produção do Informativo */
        public void SalvarEncerrarProducao(string ano, int? numero, DateTime? dtEncerramento)
        {
            try
            {
                if ((!String.IsNullOrEmpty(ano)) && (numero != null) && (dtEncerramento != null))
                {
                    InformativoDTO informativo = FindById(ano, numero);

                    if (informativo.INF_ANO == ano && informativo.INF_NUMERO == numero)
                    {
                        informativo.INF_DATA_FINAL = dtEncerramento;
                        informativo.DATA_ALTERA = DateTime.Now;
                        Merge(informativo, "INF_ANO", "INF_NUMERO");
                    }
                }
                else 
                {
                    throw new Exception("Data de encerramento não informada. Por favor, informe a data do encerramento deste informativo!");
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
