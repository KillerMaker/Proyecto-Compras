using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace WebApplication4.Models
{
    public class CMarca : CEntidad
    {
        public int? id { get; set; }
        public string descripcion { get; set; }
        public int estado { get; set; }

        private static MySqlConnection connection;
        private static MySqlCommand command;
        private static MySqlDataReader reader;

        public CMarca(int? id, string descripcion,int estado)
        {
            this.id = id;
            this.descripcion = descripcion;
            this.estado = estado;

            connection = _connection;
        }
        public async override Task<int> Insert()
        {
            try
            {
                if (connection.State.Equals(ConnectionState.Open))
                    await connection.CloseAsync();

                await connection.OpenAsync();

                command = new MySqlCommand($@"INSERT INTO MARCA(DESCRIPCION,ESTADO)
                                                    VALUES('{descripcion}',{estado});",connection);

                return await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async override Task<int> Update()
        {
            try
            {
                if (connection.State.Equals(ConnectionState.Open))
                    await connection.CloseAsync();

                await connection.OpenAsync();

                command = new MySqlCommand($@"UPDATE MARCA SET DESCRIPCION='{descripcion}',ESTADO={estado}
                                                     WHERE ID={id};", connection);

                return await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async static Task<int> Delete(int id)
        {
            try
            {
                connection = _connection;
                if (connection.State.Equals(ConnectionState.Open))
                    await connection.CloseAsync();

                await connection.OpenAsync();

                command = new MySqlCommand($"DELETE FROM MARCA WHERE ID={id}", connection);

                return await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async static Task<List<CMarca>> Select(string searchString=null)
        {
            try
            {
                List<CMarca> marcas = new List<CMarca>();
                CMarca marca = null;

                connection = _connection;
                if (connection.State.Equals(ConnectionState.Open))
                    await connection.CloseAsync();

                await connection.OpenAsync();

                command = new MySqlCommand($@"SELECT * FROM MARCA {searchString}", connection);
                reader = (MySqlDataReader)(await command.ExecuteReaderAsync());

                while (reader.Read())
                {
                    marca = new CMarca((int)reader[0], (string)reader[1], (int)reader[2]);
                    marcas.Add(marca);
                }

                return marcas;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
