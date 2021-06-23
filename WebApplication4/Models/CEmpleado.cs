using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text.Json.Serialization;

namespace WebApplication4.Models
{
    public class CEmpleado:CEntidad
    {
        public int? id { get; set; }
        public string cedula { get; set; }
        public string nombre { get; set; }
        public int departamento { get; set; }
        public  int estado { get; set; }

        public static MySqlCommand command;
        public static MySqlDataReader reader;
        public static MySqlConnection connection;

        [JsonConstructor]
        public CEmpleado(int? id, string cedula,string nombre,int departamento,int estado)
        {
            this.id = id;
            this.cedula = cedula;
            this.nombre = nombre;
            this.departamento = departamento;
            this.estado = estado;
        }

        public async override Task<int> Insert()
        {
            try
            {
                connection = _connection;

                if (connection.State.Equals(ConnectionState.Open))
                    await connection.CloseAsync();

                await connection.OpenAsync();

                command = new MySqlCommand($@"INSERT INTO EMPLEADO
                                                        (CEDULA,
                                                        NOMBRE,
                                                        DEPARTAMENTO,
                                                        ESTADO)
                                            VALUES('{cedula}','{nombre}',{departamento},{estado});", connection);

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
                connection = _connection;

                if (connection.State.Equals(ConnectionState.Open))
                    await connection.CloseAsync();

                await connection.OpenAsync();

                command = new MySqlCommand($@"UPDATE EMPLEADO SET
                                                    CEDULA='{cedula}',
                                                    NOMBRE='{nombre}',
                                                    DEPARTAMENTO={departamento},
                                                    ESTADO={estado}
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

                command = new MySqlCommand($@"DELETE FROM EMPLEADO WHERE ID={id}", connection);

                return await command.ExecuteNonQueryAsync();

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async static Task<List<CEmpleado>> Select(string searchString = null)
        {
            try
            {
                List<CEmpleado> empleados = new List<CEmpleado>();
                CEmpleado empleado = null;

                connection = _connection;

                if (connection.State.Equals(ConnectionState.Open))
                    await connection.CloseAsync();

                await connection.OpenAsync();

                command = new MySqlCommand($@"SELECT * FROM EMPLEADO {searchString}", connection);
                reader = (MySqlDataReader)(await command.ExecuteReaderAsync());

                while (await reader.ReadAsync())
                {
                    empleado = new CEmpleado((int)reader[0], (string)reader[1], (string)reader[2], (int)reader[3], (int)reader[4]);
                    empleados.Add(empleado);
                }

                return empleados;
                

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

    }
}
