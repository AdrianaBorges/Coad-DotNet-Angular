using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COAD.SEGURANCA.Repositorios.Base;
using COAD.CORPORATIVO.Repositorios.Contexto;
using COAD.CORPORATIVO.Repositorios.Base;
using COAD.CORPORATIVO.Model.Dto;
using Coad.GenericCrud.Dao.Base.Pagination;
using Coad.GenericCrud.Service.Base;
using Coad.GenericCrud.Repositorios.Base;
using System.Data.Entity;

namespace COAD.CORPORATIVO.DAO
{
    public class ProspectDAO : DAOAdapter<CLIENTES, ClienteDto>
    {
        public COADCORPEntities _db { get { return GetDb<COADCORPEntities>(); } set { } }

        public override IQueryable<CLIENTES> SetTemplateQuery(DbSet<CLIENTES> dbSet)
        {
            return dbSet.Where(op => op.CLA_CLI_ID == 2);
        }

        public ProspectDAO()
        {
            this._db = GetDb<COADCORPEntities>();
        }
        public CLIENTES BuscarPorCNPJ(string _cli_cnpj)
        {
            var query = GetDbSetWithTemplate();
            var _cli = (from c in query
                        where c.CLI_CPF_CNPJ == _cli_cnpj
                        select c).FirstOrDefault();

            return _cli;
        }

        public Pagina<ClienteDto> Prospects(string cnpj = null, string nome = null, int pagina = 1, int registroPorPagina = 7, int? representanteId = null)
        {

            IQueryable<CLIENTES> query = null;

            if (representanteId != null)
            {
                query = TemplateQueryProspectPorRepresentante(cnpj, nome, pagina, registroPorPagina, representanteId);
            }
            else
            {
                query = TemplateQueryProspect(cnpj, nome, pagina, registroPorPagina);
            }
            return ToDTOPage(query, pagina, registroPorPagina);
        }


        private IQueryable<CLIENTES> TemplateQueryProspect(string cnpj = null, string nome = null, int pagina = 1, int registroPorPagina = 7)
        {
            IQueryable<CLIENTES> query = GetDbSetWithTemplate();

            if (!String.IsNullOrWhiteSpace(cnpj))
            {
                query = query.Where(x => x.CLI_CPF_CNPJ == cnpj);
            }
            if (!String.IsNullOrWhiteSpace(nome))
            {
                query = query.Where(x => x.CLI_NOME.Contains(nome));
            }

            query = query.Where(op => op.CLA_CLI_ID == 3);

            return query;

        }

        private IQueryable<CLIENTES> TemplateQueryProspectPorRepresentante(string cnpj = null, string nome = null, int pagina = 1, int registroPorPagina = 7, int? representanteId = null)
        {
            var db = GetDb<COADCORPEntities>(false);

            var resp = db.CARTEIRA_CLIENTE.Where(op => 
                
                    db.CARTEIRA_REPRESENTANTE.
                        Where(o => o.REP_ID == representanteId).
                        Select(se => se.CAR_ID).Contains(op.CAR_ID)
                ).Select(sele => sele.CLIENTES).
                Where(cli => cli.CLA_CLI_ID == 2);


            //var resp = (from car_rep in db.CARTEIRA_REPRESENTANTE 
            //                     join car_ass in db.CARTEIRA_ASSINATURA 
            //                        on car_rep.CAR_ID equals car_ass.CAR_ID
            //                     where car_rep.REPRESENTANTE.REP_ATIVO == 1 
            //                     && car_rep.REP_ID == representanteId && car_ass.CARTEIRA.CARTEIRA_CLIENTE.
            //                    select car_ass.ASSINATURA.CLIENTES).Where(op => op.CLA_CLI_ID == 2);

            //var representante = (from rep in db.REPRESENTANTE
            //                     where rep.REP_ATIVO == 1 && rep.REP_ID == representanteId
            //                     select rep.CARTEIRA.CAR_ID);

            //var resp = (from car_ass in db.CARTEIRA_ASSINATURA
                        
            //            where representante.Contains(car_ass.CARTEIRA.CAR_ID)
            //            select car_ass.ASSINATURA.CLIENTES);

            if (!String.IsNullOrWhiteSpace(cnpj))
            {
                resp = resp.Where(x => x.CLI_CPF_CNPJ == cnpj);
            }
            if (!String.IsNullOrWhiteSpace(nome))
            {
                resp = resp.Where(x => x.CLI_NOME.Contains(nome));
            }
                 
            return resp.Distinct().OrderBy(or => or.CLI_NOME);              
        }


    }
}
