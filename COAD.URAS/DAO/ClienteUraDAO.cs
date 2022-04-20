using COAD.URAS.Model.Base;
using COAD.URAS.Model.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using COAD.SEGURANCA.Repositorios.Base;
using Coad.GenericCrud.Extensions;
using Coad.GenericCrud.Dao.Base.Pagination;
using System.Data;

namespace COAD.URAS.DAO
{
    public class ClienteUraDAO : BaseConnection<ClienteUraDAO>
    {
        public ClienteUraDAO()
        {
        }
        public Pagina<ClienteUraDTO> BuscarTodos(string _ura_id, int numpagina = 1, int linhas = 10)
        {
            List<ClienteUraDTO> _lista = new List<ClienteUraDTO>();

            try
            {
                string _sql = "  SELECT coad.id,   " +
                             "          coad.vip,  " +
                             "          coad.ddd,  " +
                             "          coad.telefone, " +
                             "          coad.senha, " +
                             "          coad.codigo, " +
                             "          coad.nome, " +
                             "          coad.pode, " +
                             "          coad.qte_cons, " +
                             "          coad.acesso, " +
                             "          coad.qte_realiz, " +
                             "          coad.grupo " +
                             "     FROM asteriskcdrdb.coad ";

                this.GetConnection(_ura_id);
                this.db.Open();

                MySqlDataReader dr = new MySqlCommand(_sql, this.db).ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        ClienteUraDTO _cliente = new ClienteUraDTO();
                        _cliente.id = (int)dr["id"];
                        _cliente.vip = dr["vip"].ToString() == "0" || dr["vip"].ToString().ToUpper() == "FALSE" ? "Não" : "Sim";
                        _cliente.ddd = dr["ddd"].ToString();
                        _cliente.telefone = dr["telefone"].ToString();
                        _cliente.senha = dr["senha"].ToString();
                        _cliente.codigo = dr["codigo"].ToString();
                        _cliente.nome = dr["nome"].ToString();
                        _cliente.pode = dr["pode"].ToString() == "0" || dr["pode"].ToString().ToUpper() == "FALSE" ? "Não" : "Sim";
                        _cliente.qte_cons = dr["qte_cons"].ToString();
                        _cliente.acesso = dr["acesso"].ToString();
                        _cliente.qte_realiz = dr["qte_realiz"].ToString();
                        _cliente.grupo = dr["grupo"].ToString();
                        _cliente.uraid = _ura_id; 

                        _lista.Add(_cliente);
                    }
                }

                dr.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception(SysException.Show(ex));
            }
            finally
            {
                if (this.db.State == ConnectionState.Open)
                    this.db.Close();

                this.db.Dispose();
            }

            return _lista.Paginar(numpagina, linhas);
        }
        public Pagina<ClienteUraDTO> BuscarPorTelefone(int telefone, string _ura_id, int numpagina = 1, int linhas = 10)
        {
            List<ClienteUraDTO> _lista = new List<ClienteUraDTO>();

            try
            {
                string _sql = "  SELECT coad.id,   " +
                             "          coad.vip,  " +
                             "          coad.ddd,  " +
                             "          coad.telefone, " +
                             "          coad.senha, " +
                             "          coad.codigo, " +
                             "          coad.nome, " +
                             "          coad.pode, " +
                             "          coad.qte_cons, " +
                             "          coad.acesso, " +
                             "          coad.qte_realiz, " +
                             "          coad.grupo " +
                             "     FROM asteriskcdrdb.coad where  coad.telefone = " + telefone.ToString();

                this.db = this.GetConnection(_ura_id);
                this.db.Open();
                MySqlDataReader dr = new MySqlCommand(_sql, this.db).ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        ClienteUraDTO _cliente = new ClienteUraDTO();
                        _cliente.id = (int)dr["id"];
                        _cliente.vip = dr["vip"].ToString() == "0" || dr["vip"].ToString().ToUpper() == "FALSE" ? "Não" : "Sim";
                        _cliente.ddd = dr["ddd"].ToString();
                        _cliente.telefone = dr["telefone"].ToString();
                        _cliente.senha = dr["senha"].ToString();
                        _cliente.codigo = dr["codigo"].ToString();
                        _cliente.nome = dr["nome"].ToString();
                        _cliente.pode = dr["pode"].ToString() == "0" || dr["pode"].ToString().ToUpper() == "FALSE" ? "Não" : "Sim";
                        _cliente.qte_cons = dr["qte_cons"].ToString();
                        _cliente.acesso = dr["acesso"].ToString();
                        _cliente.qte_realiz = dr["qte_realiz"].ToString();
                        _cliente.grupo = dr["grupo"].ToString();
                        _cliente.uraid = _ura_id;

                        _lista.Add(_cliente);

                    }
                }

                dr.Dispose();
            }
            catch (Exception ex)
            {
                throw new Exception(SysException.Show(ex));
            }
            finally
            {
                if (this.db.State == ConnectionState.Open)
                    this.db.Close();

                this.db.Dispose();
            }

            return _lista.Paginar(numpagina, linhas);
        }
        public Pagina<ClienteUraDTO> BuscarPorAssinatura(string codigo, string _ura_id, int numpagina = 1, int linhas = 10)
        {
            List<ClienteUraDTO> _lista = new List<ClienteUraDTO>();

            try
            {
                string _sql = "  SELECT coad.id,   " +
                             "          coad.vip,  " +
                             "          coad.ddd,  " +
                             "          coad.telefone, " +
                             "          coad.senha, " +
                             "          coad.codigo, " +
                             "          coad.nome, " +
                             "          coad.pode, " +
                             "          coad.qte_cons, " +
                             "          coad.acesso, " +
                             "          coad.qte_realiz, " +
                             "          coad.grupo " +
                             "     FROM asteriskcdrdb.coad where coad.codigo = '" + codigo + "'";

                this.db = this.GetConnection(_ura_id);
                this.db.Open();
                MySqlDataReader dr = new MySqlCommand(_sql, this.db).ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        ClienteUraDTO _cliente = new ClienteUraDTO();
                        _cliente.id = (int)dr["id"];

                        int vip = 0;
                        string vipstring = dr["vip"].ToString();
                        int.TryParse(dr["vip"].ToString(), out vip);

                        _cliente.vip = dr["vip"].ToString() == "0" || dr["vip"].ToString().ToUpper() == "FALSE" ? "Não" : "Sim";   
                        _cliente.ddd = dr["ddd"].ToString();
                        _cliente.telefone = dr["telefone"].ToString();
                        _cliente.senha = dr["senha"].ToString();
                        _cliente.codigo = dr["codigo"].ToString();
                        _cliente.nome = dr["nome"].ToString();
                        _cliente.pode = dr["pode"].ToString() == "0" || dr["pode"].ToString().ToUpper() == "FALSE" ? "Não" : "Sim";
                        _cliente.qte_cons = dr["qte_cons"].ToString();
                        _cliente.acesso = dr["acesso"].ToString();  
                        _cliente.qte_realiz = dr["qte_realiz"].ToString();
                        _cliente.grupo = dr["grupo"].ToString();
                        _cliente.uraid = _ura_id; 

                        _lista.Add(_cliente);

                    }
                }
                
                dr.Dispose();

            }
            catch (Exception ex)
            {
                throw new Exception(SysException.Show(ex));
            }
            finally
            {
                if (this.db.State == ConnectionState.Open)
                    this.db.Close();

                this.db.Dispose();
            }

            return _lista.Paginar(numpagina, linhas);
        }
    
      
        public IList<ClienteUraDTO> ListarPorAssinatura(string codigo)
        {

            List<ClienteUraDTO> _lista = new List<ClienteUraDTO>();
            List<string> _listuras = new List<string>{ "URARJ", "URAPR", "URAMG" };

            try
            {
                foreach (var _ura_id in _listuras)
                {

                    string _sql = "  SELECT coad.id,   " +
                                 "          coad.vip,  " +
                                 "          coad.ddd,  " +
                                 "          coad.telefone, " +
                                 "          coad.senha, " +
                                 "          coad.codigo, " +
                                 "          coad.nome, " +
                                 "          coad.pode, " +
                                 "          coad.qte_cons, " +
                                 "          coad.acesso, " +
                                 "          coad.qte_realiz, " +
                                 "          coad.grupo " +
                                 "     FROM asteriskcdrdb.coad where coad.codigo = '" + codigo + "'";

                    this.db = this.GetConnection(_ura_id);
                    this.db.Open();
                    MySqlDataReader dr = new MySqlCommand(_sql, this.db).ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ClienteUraDTO _cliente = new ClienteUraDTO();
                            _cliente.id = (int)dr["id"];

                            int vip = 0;
                            string vipstring = dr["vip"].ToString();
                            int.TryParse(dr["vip"].ToString(), out vip);

                            _cliente.vip = dr["vip"].ToString() == "0" || dr["vip"].ToString().ToUpper() == "FALSE" ? "Não" : "Sim";
                            _cliente.ddd = dr["ddd"].ToString();
                            _cliente.telefone = dr["telefone"].ToString();
                            _cliente.senha = dr["senha"].ToString();
                            _cliente.codigo = dr["codigo"].ToString();
                            _cliente.nome = dr["nome"].ToString();
                            _cliente.pode = dr["pode"].ToString() == "0" || dr["pode"].ToString().ToUpper() == "FALSE" ? "Não" : "Sim";
                            _cliente.qte_cons = dr["qte_cons"].ToString();
                            _cliente.acesso = dr["acesso"].ToString();
                            _cliente.qte_realiz = dr["qte_realiz"].ToString();
                            _cliente.grupo = dr["grupo"].ToString();
                            _cliente.uraid = _ura_id;

                            _lista.Add(_cliente);

                        }
                    }

                    this.db.Close();
                    
                    dr.Dispose();
                }

            }
            catch (Exception ex)
            {
                throw new Exception(SysException.Show(ex));
            }
            finally
            {
                if (this.db.State == ConnectionState.Open)
                    this.db.Close();

                this.db.Dispose();
            }

            return _lista;
        }
        public void AtualizaUra(ClienteUraDTO _cliura)
        {
            List<ClienteUraDTO> _lista = new List<ClienteUraDTO>();

            try
            {
                this.db = this.GetConnection(_cliura.uraid);
                this.db.Open();

                string _sql = " SELECT COUNT(*)  FROM asteriskcdrdb.coad "  +
                              "  where coad.telefone = " + _cliura.telefone +
                              "    and coad.codigo   = '" + _cliura.codigo  + "'";

                int count1 = 0;
                bool count = int.TryParse(new MySqlCommand(_sql, this.db).ExecuteScalar().ToString(), out count1);

                if ((int)count1 > 0)
                {
                    _sql = " update asteriskcdrdb.coad " +
                           "    set coad.vip = " + _cliura.vip +
                           "       ,coad.ddd = " + _cliura.ddd +
                           "       ,coad.senha = " + _cliura.senha +
                           "       ,coad.nome = '" + _cliura.nome + "'" +
                           "       ,coad.pode = " + _cliura.pode +
                           "       ,coad.qte_cons = " + _cliura.qte_cons +
                           "       ,coad.acesso = " + _cliura.acesso +
                           "       ,coad.qte_realiz = " + _cliura.qte_realiz +
                           "       ,coad.grupo = " + _cliura.grupo +
                           "  where coad.telefone = " + _cliura.telefone +
                           "    and coad.codigo   = '" + _cliura.codigo + "'";
                }
                else
                {
                    _sql = " insert into asteriskcdrdb.coad " +
                           "       (coad.vip "+
                           "       ,coad.ddd "+
                           "       ,coad.senha "+
                           "       ,coad.nome  "+
                           "       ,coad.pode  "+
                           "       ,coad.qte_cons "+
                           "       ,coad.acesso "+
                           "       ,coad.qte_realiz "+
                           "       ,coad.grupo "+
                           "       ,coad.telefone "+      
                           "       ,coad.codigo) values "+
                           "       ("+_cliura.vip +
                           "       ," + _cliura.ddd +
                           "       ," + _cliura.senha +
                           "       ,'" + _cliura.nome + "'" +
                           "       ," + _cliura.pode +
                           "       ," + _cliura.qte_cons +
                           "       ," + _cliura.acesso +
                           "       ," + _cliura.qte_realiz +
                           "       ," + _cliura.grupo +
                           "       ," + _cliura.telefone +
                           "       ,'" + _cliura.codigo + "')";
                }

                new MySqlCommand(_sql, this.db).ExecuteNonQuery();
                
            }
            catch (Exception ex)
            {
                throw new Exception(SysException.Show(ex));
            }
            finally
            {
                if (this.db.State == ConnectionState.Open)
                    this.db.Close();

                this.db.Dispose();
            }
            
        }

     
    }
}
