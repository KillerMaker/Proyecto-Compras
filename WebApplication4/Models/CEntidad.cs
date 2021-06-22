using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace WebApplication4.Models
{
    public abstract class CEntidad
    {
        protected static MySqlConnection _connection = new MySqlConnection("Data Source=localhost; uid=root; pwd=Archipielago1@; database=compras");
        public abstract Task<int> Insert();
        public abstract Task<int> Update();
    }
}
