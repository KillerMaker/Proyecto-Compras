using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace WebApplication4.Models
{
    public class CArticulo : CEntidad
    {
        public int? id { get; set; }
        public string descripcion { get; set; }
        public int marca { get; set; }
        public int unidadMedida { get; set; }
        public int existencia { get; set; }
        public int estado { get; set; }

        private static MySqlConnection connection;
        private static MySqlCommand command;
        private static MySqlDataReader reader;

        public CArticulo(int? id, string descripcion, int marca, int unidadMedida, int existencia, int estado)
        {
            this.id = id;
            this.descripcion = descripcion;
            this.marca = marca;
            this.unidadMedida = unidadMedida;
            this.existencia = existencia;
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

                command = new MySqlCommand($@"INSERT INTO ARTICULO
                                                (DESCRIPCION,MARCA,UNIDAD_MEDIDA,EXISTENCIA,ESTADO)
                                                    VALUES('{descripcion}',
                                                            {marca},
                                                            {unidadMedida},
                                                            {existencia},
                                                            {estado})", connection);

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

                command = new MySqlCommand($@"UPDATE ARTICULO SET
                                                DESCRIPCION = '{descripcion}',
                                                MARCA = {marca},
                                                UNIDAD_MEDIDA = {unidadMedida},
                                                EXISTENCIA = {existencia},
                                                ESTADO = {estado} WHERE ID={id}", connection);

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

                command = new MySqlCommand($@"DELETE FROM ARTICULO WHERE ID={id}", connection);

                return await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async static Task<List<CArticulo>> Select(string searchString = null)
        {
            try
            {
                List<CArticulo> articulos = new List<CArticulo>();
                CArticulo articulo = null;

                connection = _connection;
                if (connection.State.Equals(ConnectionState.Open))
                    await connection.CloseAsync();

                await connection.OpenAsync();

                command = new MySqlCommand($@"SELECT * FROM ARTICULO {searchString}", connection);
                reader = (MySqlDataReader)(await command.ExecuteReaderAsync());

                while (reader.Read())
                {
                    articulo = new CArticulo
                       (
                           (int)reader[0],
                           (string)reader[1],
                           (int)reader[2],
                           (int)reader[3],
                           (int)reader[4],
                           (int)reader[5]
                       );

                    articulos.Add(articulo);
                }

                return articulos;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
