using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
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
        protected abstract List<MySqlParameter> SqlLlenaParametros();

        protected async static Task<int> ExecuteCommand(string Query,List<MySqlParameter>parametros=null)
        {
            try
            {
                using(MySqlConnection con= new MySqlConnection("Data Source=localhost; uid=root; pwd=Archipielago1@; database=compras"))
                {
                    
                    using (MySqlCommand command= new MySqlCommand(Query,con))
                    {
                        await con.OpenAsync();
                        command.Connection = con;

                        if (parametros != null)
                            command.Parameters.AddRange(parametros.ToArray());

                        return await command.ExecuteNonQueryAsync();
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        protected async static Task<MySqlDataReader> ExecuteReader(string Query)
        {
            try
            {
                using(MySqlCommand command= new MySqlCommand(Query, _connection))
                {
                    await _connection.OpenAsync();
                    command.Connection = _connection;

                    return (MySqlDataReader)await command.ExecuteReaderAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

}
