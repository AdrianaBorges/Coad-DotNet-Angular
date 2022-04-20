using System;
using System.Linq;
using System.Text;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Threading.Tasks;
using System.Collections.Generic;
using COAD.SEGURANCA.Repositorios.Base;

namespace COAD.URAS.Model.Base
{
    public class BaseConnection<T> : IDisposable  where T : class 
    {
        public MySqlConnection db { set; get; }
        public string ConnectionString {get; set;}
        public BaseConnection()
        {

        }
        public MySqlConnection GetConnection(string _ura_id)
        {
            // descomentar quando terminar o teste

            if (_ura_id == "URAPR")
                this.ConnectionString = "Server=10.228.8.1;Database=asteriskcdrdb;Uid=coad;Pwd=c04d;";
            else if (_ura_id == "URARJ")
                this.ConnectionString = "Server=10.228.5.22;Database=asteriskcdrdb;Uid=coad;Pwd=c04d;";
            else if (_ura_id == "URAMG")
                this.ConnectionString = "Server=10.228.10.211;Database=asteriskcdrdb;Uid=coad;Pwd=c04d;";
            else if (_ura_id == "CONSULTORIA")
                this.ConnectionString = "Server=10.228.6.22;Database=consultoria;Uid=dbadmcoad;Pwd=jj@@21dell;";
            else if (_ura_id == "CTIRJ")
                this.ConnectionString = "Server=10.228.5.19;Database=tele_atendimento;Uid=root;Pwd=c04dxky;Connection Timeout=60;";
            else if (_ura_id == "CTIMG")
                this.ConnectionString = "Server=10.228.10.100;Database=tele_atendimento;Uid=root;Pwd=c04dxky;Connection Timeout=60;";


            //this.ConnectionString = "Server=127.0.0.1;Database=test;Uid=root;Pwd=ROOT;";

            this.db = new MySqlConnection(this.ConnectionString);
            
            return this.db;

        }
        public void Dispose()
        {
         
        }

    }
}
