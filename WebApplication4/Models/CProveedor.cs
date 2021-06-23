using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;

namespace WebApplication4.Models
{
    public class CProveedor : CEntidad
    {
        public int? id { get; set; }
        public string cedula { get; set; }
        public string nombreComercial {get ; set;}
        public int estado { get; set; }

        public static MySqlCommand command;
        public static MySqlDataReader reader;
        public static MySqlConnection connection;

        public CProveedor(int? id,string cedula,string nombreComercial,int estado)
        {
            this.id = id;
            this.cedula = cedula;
            this.nombreComercial = nombreComercial;
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

                command = new MySqlCommand($@"INSERT INTO PROVEEDOR
                                                        (   CEDULA,
                                                            NOMBRE_COMERCIAL,
                                                            ESTADO )
                                            VALUES('{cedula}','{nombreComercial}',{estado});", connection);

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

                command = new MySqlCommand($@"UPDATE PROVEEDOR SET
                                                    CEDULA='{cedula}',
                                                    NOMBRE_COMERCIAL='{nombreComercial}',
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

                command = new MySqlCommand($@"DELETE FROM PROVEEDOR WHERE ID={id}", connection);

                return await command.ExecuteNonQueryAsync();

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async static Task<List<CProveedor>> Select(string searchString = null)
        {
            try
            {
                List<CProveedor> proveedores = new List<CProveedor>();
                CProveedor proveedor = null;

                connection = _connection;

                if (connection.State.Equals(ConnectionState.Open))
                    await connection.CloseAsync();

                await connection.OpenAsync();

                command = new MySqlCommand($@"SELECT * FROM PROVEEDOR {searchString}", connection);
                reader = (MySqlDataReader)(await command.ExecuteReaderAsync());

                while (reader.Read())
                {
                    proveedor = new CProveedor((int)reader[0], (string)reader[1], (string)reader[2], (int)reader[3]);
                    proveedores.Add(proveedor);
                }

                return proveedores;


            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
    }
}
