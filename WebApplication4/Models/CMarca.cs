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

        public CMarca(int? id, string descripcion,int estado)
        {
            this.id = id;
            this.descripcion = descripcion;
            this.estado = estado;
        }
        public async override Task<int> Insert() =>
            await ExecuteCommand(@"INSERT INTO MARCA(DESCRIPCION,ESTADO)
                                                    VALUES(@DESCRIPCION,@ESTADO);", SqlLlenaParametros());


        public async override Task<int> Update() =>
            await ExecuteCommand(@"UPDATE MARCA SET 
                                          DESCRIPCION=@DESCRIPCION,
                                          ESTADO=@ESTADO
                                          WHERE ID=@ID;",SqlLlenaParametros());

        public async static Task<int> Delete(int id) =>
            await ExecuteCommand($"DELETE FROM MARCA WHERE ID={id}");

        public async static Task<List<CMarca>> Select(string searchString=null)
        {
            try
            {
                List<CMarca> marcas = new List<CMarca>();
                CMarca marca = null;
                string query = $@"SELECT * FROM MARCA {searchString}";

                using (MySqlDataReader reader=await ExecuteReader(query))
                {
                    while (reader.Read())
                    {
                        marca = new CMarca((int)reader[0], (string)reader[1], (int)reader[2]);
                        marcas.Add(marca);
                    }

                    await _connection.CloseAsync();
                    return marcas;
                }
               
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected override List<MySqlParameter> SqlLlenaParametros()
        {
            List<MySqlParameter> parametros;

            MySqlParameter SqlId = new MySqlParameter("@ID", MySqlDbType.Int32);
            MySqlParameter SqlDescripcion = new MySqlParameter("@DESCRIPCION", MySqlDbType.VarChar, 200);
            MySqlParameter SqlEstado = new MySqlParameter("@ESTADO", MySqlDbType.Int32);


            SqlDescripcion.Value = descripcion;
            SqlEstado.Value = estado;

            if(id.HasValue)
            {
                SqlId.Value = id;
                parametros = new List<MySqlParameter>() { SqlId, SqlDescripcion, SqlEstado };
            }
            else
                parametros = new List<MySqlParameter>() {SqlDescripcion, SqlEstado };

            return parametros;

        }
    }
}
