using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace WebApplication4.Models
{
    public class CDepartamento : CEntidad
    {
        public int? id { get; set; }
        public string nombre { get; set; }
        public int estado { get; set; }

        private static MySqlConnection connection;
        private static MySqlCommand command;
        private static MySqlDataReader reader;

        public CDepartamento(int? id,string nombre, int estado)
        {
            this.id = id;
            this.nombre = nombre;
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

                command = new MySqlCommand($@"INSERT INTO DEPARTAMENTO(NOMBRE,ESTADO)VALUES('{nombre}',{estado})", connection);
                
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

                command = new MySqlCommand($@"UPDATE DEPARTAMENTO SET NOMBRE='{nombre}',ESTADO={estado} WHERE ID={id}", connection);

                return await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async static Task<int>Delete(int id)
        {
            try
            {
                connection = _connection;
                if (connection.State.Equals(ConnectionState.Open))
                    await connection.CloseAsync();

                await connection.OpenAsync();

                command = new MySqlCommand($@"DELETE FROM DEPARTAMENTO WHERE ID={id}", connection);

                return await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async static Task<List<CDepartamento>> Select(string searchString = null)
        {
            try
            {
                List<CDepartamento> departamentos = new List<CDepartamento>();
                CDepartamento departamento = null;

                connection = _connection;
                if (connection.State.Equals(ConnectionState.Open))
                    await connection.CloseAsync();

                await connection.OpenAsync();

                command = new MySqlCommand($@"SELECT * FROM DEPARTAMENTO {searchString}", connection);
                reader = (MySqlDataReader)(await command.ExecuteReaderAsync());

                while (await reader.ReadAsync())
                {
                    departamento = new CDepartamento((int)reader[0], (string)reader[1], (int)reader[2]);
                    departamentos.Add(departamento);
                }

                return departamentos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
