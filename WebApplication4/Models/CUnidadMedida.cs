using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace WebApplication4.Models
{
    public class CUnidadMedida : CEntidad
    {
        //Atributos de constructor
        public int? id { get; set; }
        public string descripcion { get; set; }
        public int estado { get; set; }

        //Atributos de vista
        public string nombreEstado { get; set; }
        public CUnidadMedida(int? id, string descripcion, int estado)
        {
            this.id = id;
            this.descripcion = descripcion;
            this.estado = estado;
        }
        public async override Task<int> Insert() =>
          await ExecuteCommand(@"INSERT INTO UNIDAD_MEDIDA(DESCRIPCION,ESTADO)
                                                    VALUES(@DESCRIPCION,@ESTADO);", SqlLlenaParametros());


        public async override Task<int> Update() =>
            await ExecuteCommand(@"UPDATE UNIDAD_MEDIDA SET 
                                          DESCRIPCION=@DESCRIPCION,
                                          ESTADO=@ESTADO
                                          WHERE ID=@ID;", SqlLlenaParametros());

        public async static Task<int> Delete(int id) =>
            await ExecuteCommand($"DELETE FROM UNIDAD_MEDIDA WHERE ID={id}");

        public async static Task<List<CUnidadMedida>> Select(string searchString = null)
        {
            try
            {
                List<CUnidadMedida> UnidadesMedidas = new List<CUnidadMedida>();
                CUnidadMedida unidadMedida = null;
                string query = $@"SELECT * FROM UNIDAD_MEDIDA U INNER JOIN ESTADO E ON E.ID=U.ESTADO {searchString}";

                using (MySqlDataReader reader = await ExecuteReader(query))
                {
                    while (reader.Read())
                    {
                        unidadMedida = new CUnidadMedida((int)reader[0], (string)reader[1], (int)reader[2]) 
                        {
                            nombreEstado=(string)reader[4]
                        };
                        UnidadesMedidas.Add(unidadMedida);
                    }

                    await _connection.CloseAsync();
                    return UnidadesMedidas;
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

            if (id.HasValue)
            {
                SqlId.Value = id;
                parametros = new List<MySqlParameter>() { SqlId, SqlDescripcion, SqlEstado };
            }
            else
                parametros = new List<MySqlParameter>() { SqlDescripcion, SqlEstado };

            return parametros;

        }
    }
}
